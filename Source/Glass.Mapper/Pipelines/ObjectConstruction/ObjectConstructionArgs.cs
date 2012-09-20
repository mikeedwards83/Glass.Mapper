using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectConstruction
{
    public class ObjectConstructionArgs : AbstractPipelineArgs
    {

        /// <summary>
        /// The type of the object to create
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// The final instance of the object to create
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// Indicates if the object should be lazy loaded
        /// </summary>
        public bool IsLazy { get; private set; }


        
        public ObjectConstructionArgs(Type type, bool isLazy)
        {

        }
        
        

    }
}
