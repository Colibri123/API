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
            return data;
        }

    }
}
