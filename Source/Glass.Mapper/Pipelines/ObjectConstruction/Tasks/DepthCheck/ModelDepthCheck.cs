using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.DepthCheck
{
    public class ModelDepthCheck : AbstractObjectConstructionTask
    {
        const string Key = "EF601809-8711-4F34-AE8B-BDD814CE6FCC";
        public override void Execute(ObjectConstructionArgs args)
        {
            using (new DepthCheck())
            {
                base.Execute(args);
            }
        }

        public class DepthCheck : IDisposable
        {
            const string Key = "1C983BD1-4642-48EA-BB41-C3CBB1B0376D";

            public DepthCheck()
                : this(8)
            {

            }
            public DepthCheck(int maxDepth)
            {
                var current = 0;
                if (ThreadData.Contains(Key))
                {
                    current = ThreadData.GetValue<int>(Key);
                }
                current++;
                if (current > maxDepth)
                {
                    throw new MapperStackException(Constants.Errors.ErrorLazyLoop);
                }
                ThreadData.SetValue(Key, current);

            }
            public void Dispose()
            {
                if (ThreadData.Contains(Key))
                {
                    var current = ThreadData.GetValue<int>(Key);
                    current--;
                    ThreadData.SetValue(Key, current);
                }
            }
        }
    }
}
