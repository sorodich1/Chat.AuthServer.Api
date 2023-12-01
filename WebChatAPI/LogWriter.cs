using System;
using System.IO;
using System.Text;

namespace WebChatAPI
{
    public class LogWriter : TextWriter
    {
        public override void WriteLine(string value)
        {
            //do whatever with value
        }
        protected void Application_Start(object sender, EventArgs e)
        {
            var writer = new LogWriter();
            Console.SetOut(writer);
        }
        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}
