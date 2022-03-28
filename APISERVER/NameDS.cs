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
        public Guid IDName { get; set; }
        public string Name { get; set; }
        public string Gender { get; set;}
        public int Age { get; set; }

        public string Get(string data)
        {
            Errors errors = new Errors();
            SQLRequest sQLRequest = new SQLRequest();
            DataSet dataTable = new DataSet();

            bool requestTrue = false;            

            Regex regex = new Regex("nameUser=(?<id>[^;]+)");

            var s = regex.Match(data).Groups["id"].Value;

            if (data == "api.test/v1/nameUser")
            {
                dataTable = sQLRequest.Request("SELECT * From NameDS");
                var nameUser = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new NameDS
                {
                    IDName = DataColumn.Field<Guid>("IDName"),
                    Name = DataColumn.Field<string>("Name"),
                    Gender = DataColumn.Field<string>("Gender"),
                    Age = DataColumn.Field<int>("Age")

                }).ToList();
                data = JsonConvert.SerializeObject(nameUser, Formatting.Indented, new JsonSerializerSettings { });
                requestTrue = true;
            }

            if (data == $"api.test/v1/nameUser&userID={s}")
            {
                dataTable = sQLRequest.Request($"SELECT * From NameDS WHERE IDName = '{s}'");
                var nameUser = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new NameDS
                {
                    IDName = DataColumn.Field<Guid>("IDName"),
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
                errors.Errors120();
            }
            return data;
        }
    }

   
}
