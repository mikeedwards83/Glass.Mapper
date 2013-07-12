using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc
{
    public class RenderingResult: IDisposable
    {
        private readonly TextWriter _writer;
        private readonly string _firstPart;
        private readonly string _lastPart;
        private readonly string _result;

        public RenderingResult(TextWriter writer, string firstPart, string lastPart)
        {
            _writer = writer;
            _firstPart = firstPart;
            _lastPart = lastPart;
            _writer.Write(_firstPart);
        }

        public void Dispose()
        {
            _writer.Write(_lastPart);
        }
    }
}
