using APISERVER.Struct;
using Newtonsoft.Json;
using System.Data;

namespace APISERVER.Entity
{
    public class DialogueListDS
    {
        public string dialogueListGet(string nameid,string token)
        {
            Errors errors = new Errors();
            SQLRequest sQLRequest = new SQLRequest();
            DataSet dataTable = new DataSet();
            string data;

            dataTable = sQLRequest.Request(@$"SELECT  MessegesListDS.DateOfDispatch,
                                              		  MessegesListDS.DialogueID,
                                              		  MessegesListDS.Messeges,
                                              		  MessegesListDS.MessegesID,
                                              		  NameDS.Name,
                                                      NameDS.UserID,
                                                      DialogueDS.DialogueName
                                              FROM DialogueListDS INNER JOIN
                                              MessegesListDS ON DialogueListDS.DialogueID = MessegesListDS.DialogueID LEFT JOIN
                                              NameDS ON MessegesListDS.UserID =NameDS.UserID INNER JOIN
                                              DialogueDS ON DialogueListDS.DialogueID = DialogueDS.DialogueID JOIN 
                                              DialogueListDS as f ON f.UserID = '{nameid}' JOIN
                                              TokenDS ON TokenDS.Token = '{token}' AND 
                                              TokenDS.UserActualID = '6A34703A-2D63-40CE-898A-4664D3983E51'");

            var dialogGet = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new DialogueListStruct
            {
                DialogueID = DataColumn.Field<Guid>("DialogueID"),
                MessegesID = DataColumn.Field<Guid>("MessegesID"),
                UserID = DataColumn.Field<Guid>("UserID"),
                DateOfDispatch = DataColumn.Field<DateTime>("DateOfDispatch"),
                DialogueName = DataColumn.Field<string>("DialogueName"),
                Messeges = DataColumn.Field<string>("Messeges"),
                Name = DataColumn.Field<string>("Name"),

            }).GroupBy(a=>a.DialogueName).ToList();

            //var LastDateTime = dialogGet.Find(a => a.DateOfDispatch == dialogGet.Select(b=>b.DateOfDispatch).Max());

            data = JsonConvert.SerializeObject(dialogGet,
            Formatting.Indented,
            new JsonSerializerSettings { });
            return data;
        }
    }
}
