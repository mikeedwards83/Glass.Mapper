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
        Stack<string> _models = new Stack<string>();

        public override void Execute(ObjectConstructionArgs args)
        {
            using (new DepthCheck(args.Configuration.Type))
            {
                base.Execute(args);
            }
        }

        public class DepthCheck : IDisposable
        {
            const string Key = "1C983BD1-4642-48EA-BB41-C3CBB1B0376D";

            public DepthCheck(Type type)
                : this(8, type)
            {

            }
            public DepthCheck(int maxDepth, Type type)
            {
                Stack<string> stack = new Stack<string>();
                if (ThreadData.Contains(Key))
                {
                    stack = ThreadData.GetClass<Stack<string>>(Key);
                    
                }
                stack.Push(type.FullName);
                if (stack.Count > maxDepth)
                {
                    throw new MapperStackException(Constants.Errors.ErrorLazyLoop.Formatted(
                        stack.Aggregate((x,y)=> "{0}{1}\n\r".Formatted(x,y))
                        ));
                }
                ThreadData.SetValue(Key, stack);

            }
            public void Dispose()
            {
                if (ThreadData.Contains(Key))
                {
                    var stack = ThreadData.GetClass<Stack<string>>(Key);
                    stack.Pop();
                    ThreadData.SetValue(Key, stack);
                }
            }
        }
    }
}
