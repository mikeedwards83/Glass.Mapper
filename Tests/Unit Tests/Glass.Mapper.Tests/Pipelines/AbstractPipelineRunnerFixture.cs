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
        StubAbstractPipelineRunner _runner;

        [SetUp]
        public void Setup()
        {
            _runner = new StubAbstractPipelineRunner();
        }

        #region Method - Add

        [Test]
        public void Add_AddsTasksToTheTaskList_TaskListContainsTwoTasksInTheOrderAdded()
        {
            //Assign
            var task1 = new StubPipelineTask();
            var task2 = new StubPipelineTask();

            //Act
            _runner.Add(task1);
            _runner.Add(task2);

            //Assert
            Assert.AreEqual(2, _runner.Tasks.Count());
            Assert.AreEqual(task1, _runner.Tasks.First());
            Assert.AreEqual(task2, _runner.Tasks.Skip(1).First());
        }

        #endregion

        #region Method - Remove

        [Test]
        public void Remove_RemovesTasksFromTheTaskList_TaskListContainsTwoTasksOneTaskShouldRemain()
        {
            //Assign
            var task1 = new StubPipelineTask();
            var task2 = new StubPipelineTask();

            _runner.Add(task1);
            _runner.Add(task2);


            //Act
            _runner.Remove(task1);
         
            //Assert
            Assert.AreEqual(1, _runner.Tasks.Count());
            Assert.AreEqual(task2, _runner.Tasks.First());
            Assert.IsFalse(_runner.Tasks.Any(x=>x == task1));
        }

        #endregion

        #region Method - Run

        [Test]
        public void Run_CreatesAPipelineArgs_ReturnsAnEmptyClass()
        {
            //Assign
            var args = new StubAbstractPipelineArgs();
            
            //Act
            var result = _runner.Run(args);

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
            var args = new StubAbstractPipelineArgs();

            _runner.Add(task1);
            _runner.Add(task2);

            Assert.IsFalse(task1.HasExecuted);
            Assert.IsFalse(task2.HasExecuted);

            //Act
            _runner.Run(args);

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
            var args = new StubAbstractPipelineArgs();

            _runner.Add(task1);
            _runner.Add(task2);

            
           

            //Act
            var result = _runner.Run(args);

            //Assert
            Assert.AreEqual(2, result.CalledTasks.Count);
            Assert.IsTrue(result.CalledTasks.Any(x => x == task1));
            Assert.IsTrue(result.CalledTasks.Any(x => x == task2));
        }

        #endregion

        #region Stubs

        public class StubAbstractPipelineArgs : AbstractPipelineArgs{

            public StubAbstractPipelineArgs()
            {
                CalledTasks = new List<StubPipelineTask>();
            }
            public List<StubPipelineTask> CalledTasks { get; private set; }
        }


        public class StubAbstractPipelineRunner : AbstractPipelineRunner<StubAbstractPipelineArgs, IPipelineTask<StubAbstractPipelineArgs>>
        {
        }

        public class StubPipelineTask : IPipelineTask<StubAbstractPipelineArgs>
        {

            public void Execute(StubAbstractPipelineArgs args)
            {
                args.CalledTasks.Add(this);
                HasExecuted = true;
            }

            public bool HasExecuted { get; set; }
        }

        #endregion
    }
}
