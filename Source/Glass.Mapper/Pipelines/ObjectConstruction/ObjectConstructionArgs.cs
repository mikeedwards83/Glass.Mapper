using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectConstruction
{
    public class ObjectConstructionArgs : AbstractPipelineArgs
    {
        
        private readonly AbstractItemContext _itemContext;

        /// <summary>
        /// Context of the item being created
        /// </summary>
        public AbstractItemContext ItemContext { get; private set; }

        /// <summary>
        /// The final instance of the object to create
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// Indicates if the object should be lazy loaded
        /// </summary>
        public bool IsLazy { get;  set; }

        /// <summary>
        /// The final object to return to the caller
        /// </summary>
        public object Object { get; set; }

        /// <summary>
        /// List of parameters to pass to the constructor
        /// </summary>
        public IEnumerable<object> ConstructorParameters { get; set; }


        public ObjectConstructionArgs()
        {

        }

        public ObjectConstructionArgs(AbstractItemContext itemContext, bool isLazy)
        {
            _itemContext = itemContext;
        }


    }
}
