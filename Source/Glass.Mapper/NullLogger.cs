using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public class NullLogger : ILog
    {
        public void Warn(string message)
        {
        }

        public void Info(string message)
        {
        }

        public void Debug(string message)
        {
        }

        public void Error(string message)
        {
        }
    }
}
