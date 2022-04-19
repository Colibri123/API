using APISERVER.Entity;

namespace APISERVER
{
    public class DataProcessing
    {

        public string request(string data)
        {
            NameDS nameDS = new NameDS();
            Auth auth = new Auth();
            Errors errors = new Errors();
            DialogueDS dialogueDS = new DialogueDS();
            DialogueListDS dialogueListDS = new DialogueListDS();
            Messeges messeges = new Messeges();

            

            if (data.Contains("api.test/v1/login"))
            {
                char[] delimiterChars = { ' ', ',', ':', '/', '=', '&' };
                var f = data.Split(delimiterChars);
                data = auth.LogIn(f[4], f[6]);
                return data;
            }

            if (data.Contains("access_token") != true)
            {
                data = errors.Error101();
                
            }

            if (data.Contains("api.test/v1/nameUser"))
            {
                data = nameDS.Get(data);
            }
            if (data.Contains("api.test/v1/dialogGet"))
            {
                char[] delimiterChars = { ' ', ',', ':', '/', '=', '&' };
                var f = data.Split(delimiterChars);
                data = dialogueDS.DialogGet(f[3], f[4]);
            }
            
            if (data.Contains("api.test/v1/dialogListGet"))
            {
                char[] delimiterChars = { ' ', ',', ':', '/', '=', '&' };
                var f = data.Split(delimiterChars);
                data = dialogueListDS.dialogueListGet(f[4], f[6]);
            }
            if (data.Contains("api.test/v1/messegesGet"))
            {
                char[] delimiterChars = { ' ', ',', ':', '/', '=', '&' };
                var f = data.Split(delimiterChars);
                data = messeges.GetMesseges(f[6], f[4]);
            }
            if (data.Contains("api.test/v1/messegesSend"))
            {
                char[] delimiterChars = { '/', '=', '&' };
                var f = data.Split(delimiterChars);
                data = messeges.SendMesseges(mess: f[8], dialogeID: f[4], userID: f[6], token:f[10]);
            }
            if (data.Contains("api.test/v1/dialogeListFind"))
            {
                char[] delimiterChars = { '/', '=', '&' };
                var f = data.Split(delimiterChars);
                data = dialogueListDS.DialogFind(token: f[6], nameDialoge: f[4]);
            }
            return data;
        }

    }
}
