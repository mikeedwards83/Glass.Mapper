
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Mapper.Pipelines;

namespace Glass.Mapper.Tests.Pipelines
{
    [TestFixture]
    public class AbstractPipelineRunnerFixture
    {

        [SetUp]
        public void Setup()
        {
        }

        #region Method - Run

        [Test]
        public void Run_CreatesAPipelineArgs_ReturnsAnEmptyClass()
        {
            //Assign
            var args = new StubAbstractPipelineArgs(null);
            var runner = new StubAbstractPipelineRunner(new List<AbstractPipelineTask<StubAbstractPipelineArgs>>());
            
            //Act
            var result = runner.Run(args);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubAbstractPipelineArgs);
        }

        [Test]
        public void Run_CallsPipelineTasks_EachTaskIsMarkedAsCalled()
        {
            //Assign
            var task1 = new StubPipelineTask();
            var task2 = new StubPipelineTask();
            var args = new StubAbstractPipelineArgs(null);

            var runner = new StubAbstractPipelineRunner(new []{task1, task2});

            Assert.IsFalse(task1.HasExecuted);
            Assert.IsFalse(task2.HasExecuted);

            //Act
            runner.Run(args);

            //Assert
            Assert.IsTrue(task1.HasExecuted);
            Assert.IsTrue(task2.HasExecuted);
        }

        [Test]
        public void Run_ArgumentsPassedToEachTask()
        {
            //Assign
            var task1 = new StubPipelineTask();
            var task2 = new StubPipelineTask();
            var args = new StubAbstractPipelineArgs(null);

            var runner = new StubAbstractPipelineRunner(new[] { task1, task2 });

            //Act
            var result = runner.Run(args);

            //Assert
            Assert.AreEqual(2, result.CalledTasks.Count);
            Assert.IsTrue(result.CalledTasks.Any(x => x == task1));
            Assert.IsTrue(result.CalledTasks.Any(x => x == task2));
        }


        #endregion

        #region Stubs

        public class StubAbstractPipelineArgs : AbstractPipelineArgs{

            public StubAbstractPipelineArgs(Context context):base(context)
            {
                CalledTasks = new List<StubPipelineTask>();
            }
            public List<StubPipelineTask> CalledTasks { get; private set; }
        }


        public class StubAbstractPipelineRunner : AbstractPipelineRunner<StubAbstractPipelineArgs, AbstractPipelineTask<StubAbstractPipelineArgs>>
        {
            public StubAbstractPipelineRunner(IEnumerable<AbstractPipelineTask<StubAbstractPipelineArgs>> tasks  ):base(tasks)
            {
            }
        }

        public class StubPipelineTask : AbstractPipelineTask<StubAbstractPipelineArgs>
        {
            public Action<StubAbstractPipelineArgs> TaskToPerform { get; set; }

            public override void Execute(StubAbstractPipelineArgs args)
            {
                if (TaskToPerform != null) 
                    TaskToPerform(args);

                args.CalledTasks.Add(this);
                HasExecuted = true;
                base.Execute(args);
            }

            public bool HasExecuted { get; set; }
        }

        #endregion
    }
}




