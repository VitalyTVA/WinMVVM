using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace WinMVVM.Tests {
    [TestFixture]
    public class CommandProviderTests {
        //TODO bind attached properties...
        [Test]
        public void AssignCommandToButton() {
            CommandProvider.OnClickCount = 0;
            TestViewModel viewModel = new TestViewModel();
            using(var button = new Button()) {
                button.SetCommand(viewModel.TestCommand);
                Assert.That(viewModel.TestCommandExecuteCount, Is.EqualTo(0));

                button.PerformClick();
                Assert.That(viewModel.TestCommandExecuteCount, Is.EqualTo(1));
                Assert.That(viewModel.ExecuteParameter, Is.Null);
                Assert.That(viewModel.CanExecuteParameter, Is.Null);
                Assert.That(CommandProvider.OnClickCount, Is.EqualTo(1));

                button.SetCommand(null);
                button.PerformClick();
                Assert.That(viewModel.TestCommandExecuteCount, Is.EqualTo(1));
                Assert.That(CommandProvider.OnClickCount, Is.EqualTo(1));

                viewModel.CanExecuteTestCommand = false;
                button.SetCommand(viewModel.TestCommand);
                button.PerformClick();
                Assert.That(viewModel.TestCommandExecuteCount, Is.EqualTo(1));
                Assert.That(CommandProvider.OnClickCount, Is.EqualTo(1));

                viewModel.CanExecuteTestCommand = true;
                button.SetCommandParameter("param");
                button.PerformClick();
                Assert.That(viewModel.TestCommandExecuteCount, Is.EqualTo(2));
                Assert.That(viewModel.ExecuteParameter, Is.EqualTo("param"));
                Assert.That(viewModel.CanExecuteParameter, Is.EqualTo("param"));
                Assert.That(CommandProvider.OnClickCount, Is.EqualTo(2));
            }
        }
        [Test]
        public void AssignCommandToLabel() {
            TestViewModel viewModel = new TestViewModel();
            using(var label = new Label()) {
                label.SetCommand(viewModel.TestCommand);
            }
        }
        [Test]
        public void CollectButtonWithAssignedCommand() {
            TestViewModel viewModel = new TestViewModel();
            WeakReference reference = CreateButtonAndAssignCommand(viewModel.TestCommand);
            TestUtils.GarbageCollect();
            Assert.That(reference.IsAlive, Is.False);
            Assert.That(viewModel.TestCommand.CanExecuteChangedSubscribeCount, Is.EqualTo(1));
            viewModel.TestCommand.RaiseCanExecuteChanged();
            Assert.That(viewModel.TestCommand.CanExecuteChangedSubscribeCount, Is.EqualTo(0));
        }

        WeakReference CreateButtonAndAssignCommand(ICommand command) {
            var button = new Button();
            button.SetCommand(command);
            return new WeakReference(button);
        }

        [Test]
        public void UpdateIsEnabledOnSetParameter() {
            var command = new DelegateCommand<string>(o => { }, o => string.IsNullOrEmpty(o));
            using(var button = new Button()) {
                button.SetCommand(command);
                Assert.That(button.Enabled, Is.True);
                button.SetCommandParameter("x");
                Assert.That(button.Enabled, Is.False);
                button.SetCommandParameter(null);
                Assert.That(button.Enabled, Is.True);
                button.SetCommandParameter("x");
                Assert.That(button.Enabled, Is.False);
                button.SetCommand(null);
                Assert.That(button.Enabled, Is.True);
                button.SetCommand(command);
                Assert.That(button.Enabled, Is.False);
            }
        }
        [Test]
        public void UpdateIsEnabledOnCanExecuteChanged() {
            bool canExecute = true;
            bool executed = false;
            var command = new DelegateCommand<string>(o => executed = true, o => canExecute);
            using(var button = new Button()) {
                button.SetCommand(command);
                Assert.That(button.Enabled, Is.True);
                canExecute = false;
                Assert.That(button.Enabled, Is.True);
                button.PerformClick();
                Assert.That(executed, Is.False);
                Assert.That(button.Enabled, Is.True);
                TestUtils.GarbageCollect();
                command.RaiseCanExecuteChanged();
                Assert.That(button.Enabled, Is.False);
                Assert.That(command.CanExecuteChangedSubscribeCount, Is.EqualTo(1));
                WeakReference handlerRef = GetHandlerRef(button);
                button.SetCommand(null);
                Assert.That(command.CanExecuteChangedSubscribeCount, Is.EqualTo(0));
                TestUtils.GarbageCollect();
                Assert.That(handlerRef.IsAlive, Is.False);
            }
        }
        static WeakReference GetHandlerRef(Button button) {
            return new WeakReference(button.GetValue(CommandProvider.HandlerProperty));
        }
    }
}
