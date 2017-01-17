using System.Web;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Diagnostics
{
    public class ConstructionCalledMonitorTask : AbstractObjectConstructionTask
    {

        public const string CalledKey = "GlassCalledKey";


        public ConstructionCalledMonitorTask()
        {
            Name = "ConstructionMonitorTask";
        }

        public override void Execute(ObjectConstructionArgs args)
        {
            
                var counter = GetCounter();
                counter++;
            ThreadData.SetValue(CalledKey,counter);

            base.Execute(args);
        }

        public static int GetCounter()
        {
            var counter = 0;
            if (ThreadData. Contains(CalledKey))
            {
                counter = (int)ThreadData.GetValue<int>(CalledKey);
            }
            return counter;
        }
    }
}
