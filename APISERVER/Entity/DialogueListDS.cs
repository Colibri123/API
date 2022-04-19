using APISERVER.Struct;
using Newtonsoft.Json.Linq;
using System.Data;

namespace APISERVER.Entity
{
    public class DialogueListDS
    {
        public string dialogueListGet(string nameid, string token)
        {
            Errors errors = new Errors();
            SQLRequest sQLRequest = new SQLRequest();
            DataSet dataTable = new DataSet();
            List<DialogueListStruct> dialogueList = new List<DialogueListStruct>();
            string data;

            dataTable = sQLRequest.Request(@$"SELECT NameDS.UserID 
                                              FROM TokenDS INNER JOIN
                                              NameDS ON TokenDS.IDUser = NameDS.UserID AND
                                              TokenDS.Token = '{token}'");
            var tokenGet = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new UserStruct
            {
                UserID = DataColumn.Field<Guid>("UserID")

            }).ToList();

            if (tokenGet.Select(a => a.UserID).First().ToString() != nameid)
            {
                data = errors.Error102();
                return data;
            }

            dataTable = sQLRequest.Request(@$"SELECT  NameDS.UserID,
                                                      DialogueDS.DialogueName,
                                                      DialogueDS.DialogueID
                                              FROM DialogueListDS INNER JOIN
                                              NameDS ON DialogueListDS.UserID = NameDS.UserID AND
                                              NameDS.UserID = '{nameid}' INNER JOIN 
                                              DialogueDS on DialogueListDS.DialogueID = DialogueDS.DialogueID JOIN
                                              TokenDS ON TokenDS.Token = '{token}' AND 
                                              TokenDS.UserActualID = '6A34703A-2D63-40CE-898A-4664D3983E51'");



            var dialogGet = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new DialogueListStruct
            {
                UserID = DataColumn.Field<Guid>("UserID"),
                DialogueID = DataColumn.Field<Guid>("DialogueID"),
                DialogueName = DataColumn.Field<string>("DialogueName"),
            }).ToList();

            foreach (var item in dialogGet)
            {
                dataTable = sQLRequest.Request(@$"SELECT TOP 1 * FROM MessegesListDS  JOIN
                                              TokenDS ON TokenDS.Token = '{token}' AND 
                                              TokenDS.UserActualID = '6A34703A-2D63-40CE-898A-4664D3983E51'
                                              WHERE MessegesListDS.DialogueID = '{item.DialogueID}'
                                              ORDER BY MessegesListDS.DateOfDispatch DESC;");



                var dialogGetMesseges = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new DialogueListStruct
                {
                    DialogueID = DataColumn.Field<Guid>("DialogueID"),
                    Messeges = DataColumn.Field<string>("Messeges")
                }).ToList();
                foreach (var t in dialogGetMesseges)
                {
                    dialogueList.Add(new DialogueListStruct
                    {
                        DialogueID = item.DialogueID,
                        DialogueName = item.DialogueName,
                        Messeges = t.Messeges
                    });
                }
               
            }

            JObject rss = new JObject(
        new JProperty("user",
            new JObject(
                new JProperty("dialogesList",
                    new JArray(
                        from p in dialogueList
                        select new JObject(
                            new JProperty("userID", p.UserID),
                            new JProperty("dialogueID", p.DialogueID),
                            new JProperty("dialogueName", p.DialogueName),
                            new JProperty("messeges", p.Messeges)
                            ))))));

            data = rss.ToString();
            return data;
        }

        public string DialogFind(string token,string nameDialoge)
        {
            Errors errors = new Errors();
            SQLRequest sQLRequest = new SQLRequest();
            DataSet dataTable = new DataSet();
            string data;            

            dataTable = sQLRequest.Request(@$"SELECT * 
                                              FROM DialogueDS JOIN
                                              TokenDS ON TokenDS.Token = '{token}' AND 
                                              TokenDS.UserActualID = '6A34703A-2D63-40CE-898A-4664D3983E51'
                                              WHERE DialogueDS.DialogueName LIKE '%{nameDialoge}%' ");



            var dialogGet = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new DialogueListStruct
            {
                DateCreation = DataColumn.Field<DateTime>("DateCreation"),
                DialogueID = DataColumn.Field<Guid>("DialogueID"),
                DialogueName = DataColumn.Field<string>("DialogueName"),
            }).ToList();

            JObject rss = new JObject(
        new JProperty("user",
            new JObject(
                new JProperty("dialogesListFind",
                    new JArray(
                        from p in dialogGet
                        select new JObject(
                            new JProperty("dateCreation", p.DateCreation),
                            new JProperty("dialogueID", p.DialogueID),
                            new JProperty("dialogueName", p.DialogueName)
                            ))))));

            data = rss.ToString();
            return data;
        }
    }
}
