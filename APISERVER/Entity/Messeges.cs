using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace APISERVER
{
    public class Messeges
    {
        public string GetMesseges(string token, string dialogeID)
        {
            Errors errors = new Errors();
            SQLRequest sQLRequest = new SQLRequest();
            DataSet dataTable = new DataSet();
            string data;
            dataTable = sQLRequest.Request(@$"SELECT DialogueDS.DialogueID,
                                                  		DialogueDS.DialogueName,
                                                  		NameDS.Name,
                                                  		MessegesListDS.Messeges,
                                                  		MessegesListDS.MessegesID,
                                                  		MessegesListDS.DateOfDispatch
                                                  FROM DialogueDS INNER JOIN
                                                  MessegesListDS ON DialogueDS.DialogueID = MessegesListDS.DialogueID INNER JOIN
                                                  NameDS ON MessegesListDS.UserID = NameDS.UserID JOIN
                                                  TokenDS ON TokenDS.Token = '{token}' AND 
                                                  TokenDS.UserActualID = '6A34703A-2D63-40CE-898A-4664D3983E51' AND DialogueDS.DialogueID = '{dialogeID}'
                                                  ORDER BY MessegesListDS.DateOfDispatch");
            var messeges = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new
            {
                DialogueID = DataColumn.Field<Guid>("DialogueID"),
                DialogueName = DataColumn.Field<string>("DialogueName"),
                Name = DataColumn.Field<string>("Name"),
                Messeges = DataColumn.Field<string>("Messeges"),
                MessegesID = DataColumn.Field<Guid>("MessegesID"),
                DateOfDispatch = DataColumn.Field<DateTime>("DateOfDispatch"),

            }).ToList();
            if (messeges.Count == 0)
            {
                data = errors.Error101();
                return data;
            }

            JObject rss = new JObject(
        new JProperty("user",
            new JObject(
                new JProperty("messeges",
                    new JArray(
                        from p in messeges
                        select new JObject(
                            new JProperty("dialogueID", p.DialogueID),
                            new JProperty("DialogueName", p.DialogueName),
                            new JProperty("userName", p.Name),
                            new JProperty("messeges", p.Messeges),
                            new JProperty("messegesID", p.MessegesID),
                            new JProperty("dateOfDispatch", p.DateOfDispatch)
                            ))))));
            data = rss.ToString();

            return data;
        }

        public string SendMesseges(string token, string dialogeID,string mess,string userID) 
        {
            
            Errors errors = new Errors();
            SQLRequest sQLRequest = new SQLRequest();
            DataSet dataTable = new DataSet();
            if (token == "")
            {
                return errors.Error102();
            }
            string data = "";
            dataTable = sQLRequest.Request(@$"INSERT MessegesListDS VALUES(NEWID(),'{dialogeID}','{userID}','{mess}',GETDATE())"); 
            return data;
        }
    }
}
