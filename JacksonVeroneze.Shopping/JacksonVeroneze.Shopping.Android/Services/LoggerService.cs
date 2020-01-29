using Android.Util;
using JacksonVeroneze.Shopping.Common;

namespace JacksonVeroneze.Shopping.Droid.Services
{
    public class LoggerService : ILogger
    {
        private string _tag = "app";

        public void Debug(string message)
            => Log.Debug(_tag, message);

        public void Info(string message)
            => Log.Info(_tag, message);

        public void Trace(string message)
            => Log.Verbose(_tag, message);

        public void Warn(string message)
            => Log.Warn(_tag, message);
    }
}