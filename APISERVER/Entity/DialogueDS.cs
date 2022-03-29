using APISERVER.Struct;
using Newtonsoft.Json;
using System.Data;

namespace APISERVER.Entity
{
    public class DialogueDS
    {
        public string DialogGet(string dialogID,string token)
        {
            Errors errors = new Errors();
            SQLRequest sQLRequest = new SQLRequest();
            DataSet dataTable = new DataSet();
            string data;

            dataTable = sQLRequest.Request(@$"SELECT DialogueDS.DialogueID,
                                                  	     DialogueDS.DateCreation,
                                                  	     DialogueDS.DialogueName,
                                                  	     MessegesListDS.DateOfDispatch,
                                                  	     MessegesListDS.Messeges,
                                                  	     MessegesListDS.MessegesID,
                                                  	     NameDS.Name,
                                                  	     NameDS.UserID
                                                  FROM DialogueDS INNER JOIN
                                                  MessegesListDS ON DialogueDS.DialogueID = MessegesListDS.DialogueID INNER JOIN
                                                  NameDS ON MessegesListDS.UserID = NameDS.UserID INNER JOIN
                                                  DialogueListDS ON NameDS.UserID = DialogueListDS.UserID AND
                                                  DialogueDS.DialogueID = '{dialogID}' JOIN
                                                  TokenDS ON TokenDS.Token = '{token}' AND 
                                                  TokenDS.UserActualID = '6A34703A-2D63-40CE-898A-4664D3983E51'");

            var dialogGet = dataTable.Tables[0].AsEnumerable().Select(DataColumn => new DialogueListStruct
            {
                DialogueID = DataColumn.Field<Guid>("DialogueID"),
                MessegesID = DataColumn.Field<Guid>("MessegesID"),
                UserID = DataColumn.Field<Guid>("UserID"),
                DateCreation = DataColumn.Field<DateTime>("DateCreation"),
                DateOfDispatch = DataColumn.Field<DateTime>("DateOfDispatch"),
                DialogueName = DataColumn.Field<string>("DialogueName"),
                Messeges = DataColumn.Field<string>("Messeges"),
                Name = DataColumn.Field<string>("Name"),

            }).ToList();
            data = JsonConvert.SerializeObject(dialogGet,
            Formatting.Indented,
            new JsonSerializerSettings { });
            return data;
        }
    }
}
