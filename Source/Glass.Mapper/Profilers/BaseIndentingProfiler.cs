using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Profilers
{
    public abstract class BaseIndentingProfiler : IPerformanceProfiler
    {

        private string _currentIndent = "";
        public static string IndentCharacter = "    ";


        public void Start(string key)
        {
            Start(key, _currentIndent);
        }

        public void End(string key)
        {
            End(key, _currentIndent);
        }

        protected abstract void Start(string key, string indent);
        protected abstract void End(string key, string indent);

        public void IndentIncrease()
        {
            _currentIndent += IndentCharacter;
        }

        public void IndentDecrease()
        {
            var len = IndentCharacter.Length;

            _currentIndent = _currentIndent.Substring(0, _currentIndent.Length - len);
        }
    }
}
