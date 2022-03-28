using APISERVER.Struct;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APISERVER
{
    public class Auth
    {
        Errors errors = new Errors();
        SQLRequest sQLRequest = new SQLRequest();
        DataSet dataTable = new DataSet();
        AOToken aOToken = new AOToken();


        public string LogIn(string Login, string Password) 
        {
            string resoult = "";

            dataTable = sQLRequest.Request(@$"SELECT UserInfoDS.Login,
		                                            UserInfoDS.IDName,
		                                            Actual.Actual,
		                                            UserInfoDS.Password,
		                                            NameDS.Name
                                              FROM UserInfoDS INNER JOIN
                                              		Actual ON Actual.UserActualID = UserInfoDS.UserActualID INNER JOIN
                                              NameDS ON NameDS.IDName = UserInfoDS.IDName
                                              WHERE UserInfoDS.Login = '{Login}' AND 
                                              		UserInfoDS.Password = '{Password}' AND Actual.Actual = 1 ");
            var nameUser = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new UserStruct
            {
                IDName = DataColumn.Field<Guid>("IDName"),
                Login = DataColumn.Field<string>("Login"),
                Password = DataColumn.Field<string>("Password"),
                Actual = DataColumn.Field<int>("Actual"),
                UserName = DataColumn.Field<string>("Name"),

            }).ToList();

            //if (nameUser.Count == 0)
            //{
            //    resoult =  errors.UserError();
            //}

            if (nameUser.Select(a=>a.Actual).First() == 0 || nameUser.Count == 0)
            {
                resoult = errors.Error100();
                return resoult;
            }

            if (nameUser.Count != 0)
            {
                dataTable = sQLRequest.Request(@$"SELECT NameDS.IDName,
                                                  	     TokenDS.TokenLife,
                                                         TokenDS.TokenID,
                                                  	     TokenDS.UserActualID
                                                  FROM TokenDS INNER JOIN
                                                  NameDS ON TokenDS.IDUser = NameDS.IDName
                                                  WHERE NameDS.IDName = '{nameUser.Select(a=>a.IDName).First()}'                                                  
                                                  ");
                var AOtoken = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new UserStruct
                {
                    UserID = DataColumn.Field<Guid?>("IDName"),
                    TokenLife = DataColumn.Field<DateTime?>("TokenLife"),
                    TokenID = DataColumn.Field<Guid?>("TokenID"),
                    ActualID = DataColumn.Field<Guid?>("UserActualID")

                }).ToList();
                
                if (AOtoken.Count == 0)
                {
                    dataTable = sQLRequest.Request(@$"INSERT INTO TokenDS (TokenID, IDUser, TokenLife,Token,UserActualID) 
                                                      VALUES (NEWID(), '{nameUser.Select(a => a.IDName).First()}', '{DateTime.Now.AddHours(1)}','{aOToken.RandomString()}','6a34703a-2d63-40ce-898a-4664d3983e51')");
                    dataTable = sQLRequest.Request(@$"SELECT TokenDS.Token
                                                  FROM TokenDS INNER JOIN
                                                  NameDS ON TokenDS.IDUser = NameDS.IDName
                                                  WHERE NameDS.IDName = '{nameUser.Select(a => a.IDName).First()}' AND TokenDS.UserActualID = '6a34703a-2d63-40ce-898a-4664d3983e51'");
                    AOtoken.Clear();
                    AOtoken = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new UserStruct
                    {
                        Token = DataColumn.Field<string>("Token")
                    }).ToList();

                    var json = JObject.FromObject(new
                    {
                        request = new
                        {
                            user = new
                            {
                                login = new
                                {
                                    loginUser = nameUser.Select(a => a.Login),
                                    token = AOtoken.Select(a => a.Token)
                                }
                            }
                        }
                    });
                    return resoult = json.ToString();
                }

                if (AOtoken.Select(a => a.TokenLife).Max() <= DateTime.Now)
                {

                    dataTable = sQLRequest.Request(@$"UPDATE TokenDS
                                                      SET TokenDS.UserActualID = '76665038-2d62-4b76-bd74-1feb54f2b7d4'
                                                      WHERE TokenDS.TokenID = '{AOtoken.Select(a=>a.TokenID).First()}'");
                    dataTable = sQLRequest.Request(@$"INSERT INTO TokenDS (TokenID, IDUser, TokenLife,Token,UserActualID) 
                                                      VALUES (NEWID(), '{nameUser.Select(a => a.IDName).First()}', '{DateTime.Now.AddHours(1)}','{aOToken.RandomString()}','6a34703a-2d63-40ce-898a-4664d3983e51')");
                    dataTable = sQLRequest.Request(@$"SELECT TokenDS.Token
                                                  FROM TokenDS INNER JOIN
                                                  NameDS ON TokenDS.IDUser = NameDS.IDName
                                                  WHERE NameDS.IDName = '{nameUser.Select(a => a.IDName).First()}' AND TokenDS.UserActualID = '6a34703a-2d63-40ce-898a-4664d3983e51'");
                    AOtoken.Clear();
                    AOtoken = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new UserStruct
                    {
                        Token = DataColumn.Field<string>("Token")
                    }).ToList();

                    var json = JObject.FromObject(new
                    {
                        request = new
                        {
                            user = new
                            {
                                login = new
                                {
                                    loginUser = nameUser.Select(a => a.Login),
                                    token = AOtoken.Select(a => a.Token)
                                }
                            }
                        }
                    });

                    return resoult = json.ToString(); 

                }

                if (AOtoken.Select(a => a.TokenLife).Max() >= DateTime.Now)
                {
                    dataTable = sQLRequest.Request(@$"SELECT TokenDS.Token
                                                  FROM TokenDS INNER JOIN
                                                  NameDS ON TokenDS.IDUser = NameDS.IDName
                                                  WHERE NameDS.IDName = '{nameUser.Select(a => a.IDName).First()}' AND TokenDS.UserActualID = '6a34703a-2d63-40ce-898a-4664d3983e51'");
                    AOtoken.Clear();
                    AOtoken = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new UserStruct
                    {
                        Token = DataColumn.Field<string>("Token")
                    }).ToList();

                    var json = JObject.FromObject(new
                    {
                        request = new
                        {
                            user = new
                            {
                                login = new
                                {
                                    loginUser = nameUser.Select(a => a.Login),
                                    token = AOtoken.Select(a => a.Token)
                                }
                            }
                        }
                    });

                    return resoult = json.ToString();

                }


            }

            return resoult;


        }

        
    }
}
