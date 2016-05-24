using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc
{
    public class Log : ILog
    {
        public void Warn(string message)
        {
            Sitecore.Diagnostics.Log.Warn(message, this);
        }

        public void Info(string message)
        {
            Sitecore.Diagnostics.Log.Info(message, this);
        }

        public void Debug(string message)
        {
            Sitecore.Diagnostics.Log.Debug(message, this);
        }

        public void Error(string message)
        {
            Sitecore.Diagnostics.Log.Error(message, this);
        }
    }
}
