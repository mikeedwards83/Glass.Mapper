

namespace Glass.Mapper.Pipelines.ObjectSaving.Tasks
{
    /// <summary>
    /// Class StandardSavingTask
    /// </summary>
    public class StandardSavingTask : AbstractObjectSavingTask
    {

        public StandardSavingTask()
        {
            Name = "StandardSavingTask";
        }
        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <exception cref="Glass.Mapper.Pipelines.PipelineException">No config set, can not save object</exception>
        public override void Execute(ObjectSavingArgs args)
        {
            var savingContext = args.SavingContext;
            AbstractDataMappingContext dataMappingContext = savingContext.CreateDataMappingContext(args.Service);

            if(savingContext.Config == null)
                throw new PipelineException("No config set, can not save object", args);

            savingContext.Config.Properties.ForEach(x => x.Mapper.MapPropertyToCms(dataMappingContext));
            base.Execute(args);
        }
    }
}




