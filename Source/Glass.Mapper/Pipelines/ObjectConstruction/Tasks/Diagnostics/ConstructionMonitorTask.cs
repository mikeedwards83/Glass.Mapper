using System.Web;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Diagnostics
{
    public class ConstructionMonitorTask : AbstractObjectConstructionTask
    {

        public const string CalledKey = "GlassCalledKey";
        public const string CacheMissKey = "GlassCacheMissKey";

        private readonly string _key;

        public ConstructionMonitorTask(string key)
        {
            _key = key;
            Name = "ConstructionMonitorTask";
        }

        public override void Execute(ObjectConstructionArgs args)
        {
            if (HttpContext.Current != null)
            {
                var innerCallback = args.CreatedCallback;
                args.CreatedCallback = () =>
                {
                    var counter = GetCounter(_key);
                    counter++;
                    HttpContext.Current.Items[_key] = counter;

                    innerCallback();
                };
            }

            base.Execute(args);
        }

        public static int GetCounter(string key)
        {
            var counter = 0;
            if (HttpContext.Current.Items.Contains(key))
            {
                counter = (int)HttpContext.Current.Items[key];
            }
            return counter;
        }
    }
}
