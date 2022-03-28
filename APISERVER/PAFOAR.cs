using APISERVER.Struct;
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
    public class PAFOAR
    {
        Errors errors = new Errors();
        NameDS nameDS = new NameDS();

        string f = "";
        public string DataProcessing(string data)
        {
            Auth auth = new Auth();           

            if (data.Contains("api.test/v1/login"))
            {
                char[] delimiterChars = { ' ', ',', ':', '/','=','&' };
                var f = data.Split(delimiterChars);

                data = auth.LogIn(f[4], f[6]);
            }

            //switch (data)
            //{
            //    case "api.test/v1/nameUser":
            //        data =  nameDS.Get(data);
            //        break;

            //        default:
            //        data = errors.GetErrors(data);
            //        break;
            //}
            return data;
        }
                
    }
}
