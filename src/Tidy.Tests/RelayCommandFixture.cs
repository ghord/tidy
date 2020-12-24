using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tidy.Mvvm;
using Tidy.Tests.Mocks;

namespace Tidy.Tests
{
    [TestClass]
    public class RelayCommandFixture
    {
        [TestMethod]
        public void RelayCommand_ShouldRunCommand()
        {
            bool executed = false;
            var command = new RelayCommand(() => executed = true);

            command.Execute();

            Assert.IsTrue(executed, "Action was not executed");
        }

        [TestMethod]
        public void RelayCommand_ShoulRunCommandWithParameter()
        {
            var command = new RelayCommand<MockParameter>(p => p.Mark());
            var parameter = new MockParameter();
            command.Execute(parameter);

            Assert.IsTrue(parameter.IsMarked);
        }

        [TestMethod]
        public void RelayCommand_ShouldRunAsyncCommand()
        {
            bool executedFirstPart = false;
            bool executedSecondPart = false;

            var command = new RelayCommand(async () =>
            {
                executedFirstPart = true;
                await Task.Delay(10);
                executedSecondPart = true;
            });

            command.Execute();

            Assert.IsTrue(executedFirstPart, "Action was not executed");
            Assert.IsTrue(executedSecondPart, "Action was not executed");
        }

        [TestMethod]
        public void RelayCommand_ShouldRunAsyncCommandWithParameter()
        {
            var command = new RelayCommand<MockParameter[]>(async p =>
            {
                p[0].Mark();
                await Task.Delay(10);
                p[1].Mark();
            });

            var param1 = new MockParameter();
            var param2 = new MockParameter();

            command.Execute(new[] { param1, param2 });

            Assert.IsTrue(param1.IsMarked, "Action was not executed");
            Assert.IsTrue(param2.IsMarked, "Action was not executed");
        }

        [TestMethod]
        public void RelayCommand_ShouldRespectCanExecute()
        {
            var command1 = new RelayCommand(() => { }, () => true);
            var command2 = new RelayCommand(() => { }, () => false);

            Assert.IsTrue(command1.CanExecute());
            Assert.IsFalse(command2.CanExecute());
        }

        [TestMethod]
        public void RelayCommand_ShouldRespectCanExecuteWithParameter()
        {
            var command = new RelayCommand<MockParameter>(_ => { }, p => p.IsMarked);

            var parameter = new MockParameter();

            Assert.IsFalse(command.CanExecute(parameter));

            parameter.Mark();

            Assert.IsTrue(command.CanExecute(parameter));
        }

        [TestMethod]
        public void RelayCommand_ShouldThrowExceptionWithCorrectCallstack()
        {
            void throwException()
            {
                throw new Exception("test");
            }

            var command = new RelayCommand(() => throwException());

            try
            {
                command.Execute();
            }
            catch (Exception e)
            {
                Assert.AreEqual("test", e.Message);

                var trace = e.StackTrace!.Split(Environment.NewLine).First();
                Assert.IsTrue(trace.Contains(nameof(throwException)));
            }
        }

        [TestMethod]
        public void RelayCommand_ShouldNotRunActionWithNullParameter()
        {
            var command = new RelayCommand<MockParameter>(
                p => throw new InvalidOperationException(), 
                _ => throw new InvalidOperationException());

            Assert.IsFalse(command.CanExecute(null));

            command.Execute(null);
          
        }
    }
}
