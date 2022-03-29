using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APISERVER
{
    public class Errors
    {
        public int CodeError { get; set; }
        public string Error { get; set; }

        public string Error120() 
        {
            string res = "";
            Errors errors = new Errors

            {
                CodeError = 120,
                Error = "invalid request"
            };
            res = JsonConvert.SerializeObject(errors,
            Formatting.Indented, new JsonSerializerSettings { });
            return res;
        }

        public string Error100()
        {
            string res = "";
            
            Errors errors = new Errors

            {
                CodeError = 100,
                Error = "authorization error please try again later"
            };
            res = JsonConvert.SerializeObject(errors,
            Formatting.Indented, new JsonSerializerSettings { });
            return res;
        }

        public string Error101()
        {
            string res = "";

            Errors errors = new Errors

            {
                CodeError = 101,
                Error = "Token Initialization error"
            };
            res = JsonConvert.SerializeObject(errors,
            Formatting.Indented, new JsonSerializerSettings { });
            return res;
        }

    }
}
