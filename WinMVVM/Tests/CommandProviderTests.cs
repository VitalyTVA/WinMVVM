using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinMVVM.Tests {
    [TestFixture]
    public class CommandProviderTests {
        //TODO bind attached properties...
        [Test]
        public void AssignCommandToButton() {
            TestViewModel viewModel = new TestViewModel();
            using(var button = new Button()) {
                button.SetCommand(viewModel.TestCommand);
                Assert.That(viewModel.TestCommandExecuteCount, Is.EqualTo(0));

                button.PerformClick();
                Assert.That(viewModel.TestCommandExecuteCount, Is.EqualTo(1));
                Assert.That(CommandProvider.OnClickCount, Is.EqualTo(1));

                button.SetCommand(null);
                button.PerformClick();
                Assert.That(viewModel.TestCommandExecuteCount, Is.EqualTo(1));
                Assert.That(CommandProvider.OnClickCount, Is.EqualTo(1));
            }
        }
        [Test]
        public void AssignCommandToLabel() {
            TestViewModel viewModel = new TestViewModel();
            using(var label = new Label()) {
                label.SetCommand(viewModel.TestCommand);
            }
        }
    }
}
