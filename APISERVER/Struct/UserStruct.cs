using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APISERVER.Struct
{
    public class UserStruct
    {
        public Guid? UserID { get; set; }
        public Guid? TokenID { get; set; }
        public DateTime? TokenLife { get; set; }
        public string Token { get; set; }
        public Guid? IDName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Actual { get; set; }
        public int Age { get; set; }
        public Guid? ActualID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }


    }
}
