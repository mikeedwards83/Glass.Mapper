using System.Web;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Diagnostics
{
    public class ConstructionCreatedMonitorTask : AbstractObjectConstructionTask
    {

        public const string CacheMissKey = "GlassCacheMissKey";


        public ConstructionCreatedMonitorTask()
        {
            Name = "ConstructionMonitorTask";
        }

        public override void Execute(ObjectConstructionArgs args)
        {
            var innerCallback = args.CreatedCallback;
            //args.CreatedCallback = () =>
            //{
                var counter = GetCounter();
                counter++;
                ThreadData.SetValue(CacheMissKey, counter);
            //    innerCallback();
            //};

            base.Execute(args);
        }

        public static int GetCounter()
        {
            var counter = 0;

            if (ThreadData.Contains(CacheMissKey))
            {
                counter = (int)ThreadData.GetValue<int>(CacheMissKey);
            }
           
            return counter;
        }
    }
}
