using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public interface ILog
    {
        void Warn(string message);
        void Info(string message);
        void Debug(string message);
        void Error(string message);
    }
}
