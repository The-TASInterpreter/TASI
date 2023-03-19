using System.Text;

namespace Text_adventure_Script_Interpreter
{
    internal class Logger
    {
        public string? path;
        public bool loggerEnabled = true;
        private StreamWriter? logWriter;

        public Logger(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
            this.path = path;
            logWriter = new StreamWriter(path, true, Encoding.UTF8, 100000);
        }
        public Logger()
        {
            loggerEnabled = false;
        }
        public void Log(string content)
        {

            if (loggerEnabled)
            {
                if (logWriter == null)
                {
                    if (File.Exists(path))
                        File.Delete(path);
                    logWriter = new StreamWriter(path, true, Encoding.UTF8, 100000);
                }
                logWriter.WriteLine(content);
            }
        }

        public void Flush()
        {
            logWriter.Flush();
        }

    }
}
