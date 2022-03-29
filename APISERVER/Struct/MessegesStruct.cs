using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APISERVER.Struct
{
    public class MessegesStruct
    {
        public Guid MessegesID { get; set; }

        public Guid DialogueID { get; set; }

        public Guid UserID { get; set; }

        public string Messeges { get; set; }

        public DateTime DateOfDispatch { get; set; }
    }
}
