/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-


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

        [Test]
        public void Run_PipelineAborted_RunsOnlyOneTask()
        {
            //Assign
            var task1 = new StubPipelineTask();
            task1.TaskToPerform = (tArgs) => tArgs.AbortPipeline();
            
            var task2 = new StubPipelineTask();

            var runner = new StubAbstractPipelineRunner(new[] { task1, task2 });

            var args = new StubAbstractPipelineArgs(null);

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
            public StubAbstractPipelineRunner(IEnumerable<IPipelineTask<StubAbstractPipelineArgs>> tasks  ):base(tasks)
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




