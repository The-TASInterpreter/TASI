using TASI.PluginManager;

namespace TASI.Exceptions
{
    /// <summary>
    /// This should get thrown by plugins if they encounter an error
    /// </summary>
    public class InternalPluginException : Exception
    {
        public ITASIPlugin plugin;
        public InternalPluginException(string message, ITASIPlugin plugin)
            : base(message)
        {
            this.plugin = plugin;
        }
    }

}
