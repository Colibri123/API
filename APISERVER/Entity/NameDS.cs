﻿using APISERVER.Struct;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace APISERVER
{
    
    public class NameDS
    {
        public string UserName { get; set; }
        public string Gender { get; set; }
        public Guid? UserID { get; set; }
        public int Age { get; set; }
        public string Get(string data)
        {

            Errors errors = new Errors();
            SQLRequest sQLRequest = new SQLRequest();
            DataSet dataTable = new DataSet();
            char[] delimiterChars = { ' ', ',', ':', '/', '=', '&' };
            

            bool requestTrue = false;            

            Regex ID = new Regex("userID=(?<id>[^;&]+)");

            var iDs = ID.Match(data).Groups["id"].Value;
            var ids = iDs.Split(delimiterChars);


            if (data.Contains("access_token") == false)
            {
                data = errors.Error101();
                return data;
            }

             Regex Token = new Regex("&access_token=(?<token>[^;]+)");

            string token = Token.Match(data).Groups["token"].Value;

            if (data.Contains("api.test/v1/nameUser"))
            {
                dataTable = sQLRequest.Request(@$"SELECT *
                                                  From NameDS JOIN
                                                  TokenDS ON TokenDS.Token = '{token}' AND 
                                                  TokenDS.UserActualID = '6A34703A-2D63-40CE-898A-4664D3983E51'");
                var nameUser = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new NameDS
                {
                    UserID = DataColumn.Field<Guid>("UserID"),
                    UserName = DataColumn.Field<string>("Name"),
                    Gender = DataColumn.Field<string>("Gender"),
                    Age = DataColumn.Field<int>("Age")

                }).ToList();
                if (nameUser.Count == 0)
                {
                    data = errors.Error101();
                    return data;
                }
                data = JsonConvert.SerializeObject(nameUser, Formatting.Indented, new JsonSerializerSettings { });
                requestTrue = true;
            }

            if (data.Contains(@$"api.test/v1/nameUser&userID={string.Join(",", ids)}"))
            {
                dataTable = sQLRequest.Request(@$"SELECT * 
                                                  From NameDS JOIN
                                                  TokenDS ON TokenDS.Token = '{token}' AND 
                                                  TokenDS.UserActualID = '6A34703A-2D63-40CE-898A-4664D3983E51' 
                                                  { ($"WHERE UserID IN('{string.Join("','", ids)}') ")}");
                                                   
                var nameUser = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new NameDS
                {
                    UserID = DataColumn.Field<Guid>("UserID"),
                    UserName = DataColumn.Field<string>("Name"),
                    Gender = DataColumn.Field<string>("Gender"),
                    Age = DataColumn.Field<int>("Age")

                }).ToList();
                if (nameUser.Count == 0)
                {
                    data = errors.Error101();
                }
                data = JsonConvert.SerializeObject(nameUser,
                Formatting.Indented,
                new JsonSerializerSettings { });
                requestTrue = true;
            }

            else if (requestTrue != true)
            {
                data = errors.Error120();
                return data;
            }
            return data;
        }
    }

   
}
