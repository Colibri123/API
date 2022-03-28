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
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set;}
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

            if (data.Contains("api.test/v1/nameUser&access_token"))
            {
                dataTable = sQLRequest.Request(@$"SELECT *
                                                  From NameDS JOIN
                                                  TokenDS ON TokenDS.Token = '{token}'");
                var nameUser = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new NameDS
                {
                    UserID = DataColumn.Field<Guid>("UserID"),
                    Name = DataColumn.Field<string>("Name"),
                    Gender = DataColumn.Field<string>("Gender"),
                    Age = DataColumn.Field<int>("Age")

                }).ToList();
                data = JsonConvert.SerializeObject(nameUser, Formatting.Indented, new JsonSerializerSettings { });
                requestTrue = true;
            }

            if (data.Contains(@$"api.test/v1/nameUser&userID={string.Join(",", ids)}&access_token"))
            {
                dataTable = sQLRequest.Request(@$"SELECT * 
                                                  From NameDS JOIN TokenDS ON TokenDS.Token = '{token}' 
                                                  { ($"WHERE UserID IN('{string.Join("','", ids)}') ")}");
                                                   
                var nameUser = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new NameDS
                {
                    UserID = DataColumn.Field<Guid>("UserID"),
                    Name = DataColumn.Field<string>("Name"),
                    Gender = DataColumn.Field<string>("Gender"),
                    Age = DataColumn.Field<int>("Age")

                }).ToList();
                data = JsonConvert.SerializeObject(nameUser,
                Formatting.Indented,
                new JsonSerializerSettings { });
                requestTrue = true;
            }

            else if (requestTrue != true)
            {
                data = errors.Error120();
            }
            return data;
        }
    }

   
}
