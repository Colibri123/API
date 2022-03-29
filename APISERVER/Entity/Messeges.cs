using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APISERVER
{
    public class Messeges
    {
        public string GetMesseges (string messGet) 
        {
            Errors errors = new Errors();
            SQLRequest sQLRequest = new SQLRequest();
            DataSet dataTable = new DataSet();
            if (messGet.Contains("api.test/v1/messegesGet"))
            {
                dataTable = sQLRequest.Request(@$"SELECT *
                                                  From NameDS JOIN
                                                  TokenDS ON TokenDS.Token = ''");
                var nameUser = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new 
                {
                    UserID = DataColumn.Field<Guid>("UserID"),
                    Name = DataColumn.Field<string>("Name"),
                    Gender = DataColumn.Field<string>("Gender"),
                    Age = DataColumn.Field<int>("Age")

                }).ToList();
                if (nameUser.Count == 0)
                {
                    messGet = errors.Error101();
                    return messGet;
                }
                messGet = JsonConvert.SerializeObject(nameUser, Formatting.Indented, new JsonSerializerSettings { });
            }

            return messGet;
        }
    }
}
