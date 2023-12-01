using System;
using System.Collections.Generic;

namespace Chat.AuthServer.DataBase
{
    public class SaveChangesResult
    {
        public List<string> Messages { get; set; }
        public Exception? Exception { get; set; }
        public bool IsOK { get; set; }
        public SaveChangesResult()
        {
            Messages = new List<string>();
        }
        public SaveChangesResult(string message) : base()
        {
            AddMesages(message);
        }

        public void AddMesages(string message)
        {
            Messages.Add(message);
        }
    }
}
