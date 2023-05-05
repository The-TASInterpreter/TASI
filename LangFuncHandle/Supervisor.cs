namespace TASI.LangFuncHandle
{
    internal class Supervisor
    {
        public static bool alreadySupervisor = false;
        public static string startFile;
        public static string reportExit;
        public static void SetInf(string inf, Value value)
        {
            switch (inf.ToLower())
            {
                case "startfile":
                    startFile = value.StringValue;
                    break;
                case "savepath":
                    Global.savePath = value.StringValue;
                    break;
                case "reportexit":
                    reportExit = value.StringValue;
                    break;
                case "showerror":
                    Global.showError = value.GetBoolValue;
                    break;
                default: throw new CodeSyntaxException($"Invalid inf variable: \"{inf}\"");

            }
        }
    }
}
