using System.Collections.Generic;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ObjectConstruction
{
    public class ObjectConstructionArgs : AbstractPipelineArgs
    {
        /// <summary>
        /// Context of the item being created
        /// </summary>
        public IDataContext DataContext { get; private set; }

        /// <summary>
        /// The configuration to use to load the object
        /// </summary>
        public AbstractTypeConfiguration Configuration { get; private set; }

        public object Result { get; set; }

        public ObjectConstructionArgs(Context context, IDataContext dataContext, AbstractTypeConfiguration configuration)
            : base(context)
        {
            DataContext = dataContext;
            Configuration = configuration;
        }

       

     

      

    }
}
