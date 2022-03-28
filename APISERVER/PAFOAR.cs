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
        
        public string DataProcessing(string data)
        {
            NameDS nameDS = new NameDS();
            Auth auth = new Auth();   

            if (data.Contains("api.test/v1/login"))
            {
                char[] delimiterChars = { ' ', ',', ':', '/','=','&' };
                var f = data.Split(delimiterChars);
                data = auth.LogIn(f[4], f[6]);
            }

            if (data.Contains("api.test/v1/nameUser"))
            {
                data = nameDS.Get(data);
            }           
            return data;
        }
                
    }
}
