using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc
{
    public abstract class WriteOptions : Options
    {
        public bool UpdateStatistics { get; set; }
        public bool Silent { get; set; }

        public WriteOptions()
        {
            UpdateStatistics = true;
        }

        public virtual void Copy(WriteOptions other)
        {
            UpdateStatistics = other.UpdateStatistics;
            other.Silent = other.Silent;
        }

        public override void Validate()
        {
        }
    }
}
