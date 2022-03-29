namespace APISERVER.Struct
{
    internal class DialogueListStruct
    {
        public Guid DialogueID { get; set; }
        public string DialogueName { get; set; }
        public string Name { get; set; }
        public DateTime DateCreation { get; set; }

        public Guid MessegesID { get; set; }

        public Guid UserID { get; set; }

        public string Messeges { get; set; }

        public DateTime DateOfDispatch { get; set; }
    }
}
