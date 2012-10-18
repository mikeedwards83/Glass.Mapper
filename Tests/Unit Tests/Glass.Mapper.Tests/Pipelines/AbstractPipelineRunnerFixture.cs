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

        #region Method - Add

        [Test]
        public void Add_AddsTasksToTheTaskList_TaskListContainsTwoTasksInTheOrderAdded()
        {
            //Assign
            var task1 = new StubPipelineTask();
            var task2 = new StubPipelineTask();

            var runner = new StubAbstractPipelineRunner(new List<IPipelineTask<StubAbstractPipelineArgs>>());

            //Act

            runner.Add(task1);
            runner.Add(task2);

            //Assert
            Assert.AreEqual(2, runner.Tasks.Count());
            Assert.AreEqual(task1, runner.Tasks.First());
            Assert.AreEqual(task2, runner.Tasks.Skip(1).First());
        }

        #endregion

        #region Method - Remove

        [Test]
        public void Remove_RemovesTasksFromTheTaskList_TaskListContainsTwoTasksOneTaskShouldRemain()
        {
            //Assign
            var task1 = new StubPipelineTask();
            var task2 = new StubPipelineTask();

            var runner = new StubAbstractPipelineRunner(new List<IPipelineTask<StubAbstractPipelineArgs>>());

            runner.Add(task1);
            runner.Add(task2);


            //Act
            runner.Remove(task1);
         
            //Assert
            Assert.AreEqual(1, runner.Tasks.Count());
            Assert.AreEqual(task2, runner.Tasks.First());
            Assert.IsFalse(runner.Tasks.Any(x=>x == task1));
        }

        #endregion

        #region Method - Run

        [Test]
        public void Run_CreatesAPipelineArgs_ReturnsAnEmptyClass()
        {
            //Assign
            var args = new StubAbstractPipelineArgs(null);
            var runner = new StubAbstractPipelineRunner(new List<IPipelineTask<StubAbstractPipelineArgs>>());
            
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

            var runner = new StubAbstractPipelineRunner(new List<IPipelineTask<StubAbstractPipelineArgs>>());

            runner.Add(task1);
            runner.Add(task2);

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

            var runner = new StubAbstractPipelineRunner(new List<IPipelineTask<StubAbstractPipelineArgs>>());

            runner.Add(task1);
            runner.Add(task2);

            //Act
            var result = runner.Run(args);

            //Assert
            Assert.AreEqual(2, result.CalledTasks.Count);
            Assert.IsTrue(result.CalledTasks.Any(x => x == task1));
            Assert.IsTrue(result.CalledTasks.Any(x => x == task2));
        }

        [Test]
        public void Run_PipelineAborted_RunsOnlyOneTask()
        {
            //Assign
            var task1 = new StubPipelineTask();
            task1.TaskToPerform = (tArgs) => tArgs.AbortPipeline();
            
            var task2 = new StubPipelineTask();

            var runner = new StubAbstractPipelineRunner(new List<IPipelineTask<StubAbstractPipelineArgs>>());

            var args = new StubAbstractPipelineArgs(null);

            runner.Add(task1);
            runner.Add(task2);

            //Act
            var result = runner.Run(args);

            //Assert
            Assert.AreEqual(1, result.CalledTasks.Count);
            Assert.IsTrue(result.CalledTasks.Any(x => x == task1));
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


        public class StubAbstractPipelineRunner : AbstractPipelineRunner<StubAbstractPipelineArgs, IPipelineTask<StubAbstractPipelineArgs>>
        {
            public StubAbstractPipelineRunner(IList<IPipelineTask<StubAbstractPipelineArgs>> tasks  ):base(tasks)
            {
            }
        }

        public class StubPipelineTask : IPipelineTask<StubAbstractPipelineArgs>
        {
            public Action<StubAbstractPipelineArgs> TaskToPerform { get; set; } 

            public void Execute(StubAbstractPipelineArgs args)
            {
                if (TaskToPerform != null) 
                    TaskToPerform(args);

                args.CalledTasks.Add(this);
                HasExecuted = true;
            }

            public bool HasExecuted { get; set; }
        }

        #endregion
    }
}
