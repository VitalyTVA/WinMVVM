using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinMVVM.Tests {
    [TestFixture]
    public class BindingOperationsTests {
        [Test]
        public void NullControl() {
            Assert.Throws<ArgumentNullException>(() => BindingOperations.SetBinding(null, "Test", new Binding()));
            using(var button = new Button()) {
                Assert.Throws<ArgumentOutOfRangeException>(() => BindingOperations.SetBinding(button, null, new Binding()));
                Assert.Throws<ArgumentOutOfRangeException>(() => BindingOperations.SetBinding(button, string.Empty, new Binding()));
                Assert.Throws<ArgumentException>(() => BindingOperations.SetBinding(button, "Foo", new Binding()));
                Assert.Throws<ArgumentNullException>(() => BindingOperations.SetBinding(button, "Text", null));
            }
        }
        [Test]
        public void SetSimpleBindingBeforeSetDataContext() {
            using(var button = new Button()) {
                button.SetBinding("Text", new Binding());
                Assert.That(button.HasLocalDataContext(), Is.False);

                button.SetDataContext("test");
                Assert.That(button.Text, Is.EqualTo("test"));
                Assert.That(button.HasLocalDataContext(), Is.True);

                button.SetDataContext("test2");
                Assert.That(button.Text, Is.EqualTo("test2"));

                button.ClearDataContext();
                Assert.That(button.Text, Is.EqualTo(string.Empty));

                button.SetDataContext("test3");
                Assert.That(button.Text, Is.EqualTo("test3"));

                button.ClearBinding("Text");
                button.ClearBinding("Tag");
                Assert.That(button.Text, Is.EqualTo(string.Empty));
                Assert.That(button.Tag, Is.Null);
                button.SetDataContext("test4");
                Assert.That(button.Text, Is.EqualTo(string.Empty));
            }
        }
        [Test]
        public void SetBindingWithLambdaExpression() {
            using(var button = new Button()) {
                button.SetBinding(() => button.Text, new Binding());
                button.SetDataContext("test");
                Assert.That(button.Text, Is.EqualTo("test"));

                button.ClearBinding(() => button.Text);
                Assert.That(button.Text, Is.EqualTo(string.Empty));
            }
        }
        [Test]
        public void SetSimpleBindingAfterSetDataContext() {
            using(var button = new Button()) {
                button.SetDataContext("test");
                button.SetBinding("Text", new Binding());
                Assert.That(button.Text, Is.EqualTo("test"));

                button.SetDataContext("test2");
                Assert.That(button.Text, Is.EqualTo("test2"));

                int textChangedCount = 0;
                button.TextChanged += (sender, e) => {
                    textChangedCount++;
                };
                button.SetDataContext("test2");
                Assert.That(textChangedCount, Is.EqualTo(0));

                button.ClearDataContext();
                Assert.That(button.Text, Is.EqualTo(string.Empty));
                button.SetDataContext("test3");
                Assert.That(button.Text, Is.EqualTo("test3"));
            }
        }
        public class TestButton : Button {
            public int Text2SetCount;
            string text2;
            public string Text2 { 
                get { return text2; } 
                set { 
                    text2 = value; 
                    Text2SetCount++; 
                } 
            }
        }
        [Test]
        public void NoExtraBindingUpdates() {
            using(var button = new TestButton()) {
                button.SetDataContext("test");
                button.SetBinding("Text2", new Binding());
                Assert.That(button.Text2SetCount, Is.EqualTo(1));
                button.SetDataContext("test");
                Assert.That(button.Text2SetCount, Is.EqualTo(1));
            }
        }
        [Test]
        public void InheritedDataContextBinding_BindAfterAddToTree() {
            using(var form = new Form()) {
                var button1 = new Button();
                var button2 = new Button();

                form.Controls.Add(button1);
                form.SetDataContext("test");
                form.Controls.Add(button2);

                button1.SetBinding("Text", new Binding());
                Assert.That(button1.Text, Is.EqualTo("test"));
                Assert.That(button1.HasLocalDataContext(), Is.False);

                button2.SetBinding("Text", new Binding());
                Assert.That(button2.Text, Is.EqualTo("test"));
                Assert.That(button2.HasLocalDataContext(), Is.False);

                form.SetDataContext("test2");
                Assert.That(button1.Text, Is.EqualTo("test2"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                Assert.That(button2.Text, Is.EqualTo("test2"));
                Assert.That(button2.HasLocalDataContext(), Is.False);

                button1.SetDataContext("button1");
                Assert.That(button1.Text, Is.EqualTo("button1"));
                Assert.That(button1.HasLocalDataContext(), Is.True);
                button2.SetDataContext(null);
                Assert.That(button2.Text, Is.EqualTo(string.Empty));
                Assert.That(button2.HasLocalDataContext(), Is.True);

                button1.ClearDataContext();
                Assert.That(button1.Text, Is.EqualTo("test2"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                button2.ClearDataContext();
                Assert.That(button2.Text, Is.EqualTo("test2"));
                Assert.That(button2.HasLocalDataContext(), Is.False);

                form.ClearDataContext();
                Assert.That(button1.Text, Is.EqualTo(string.Empty));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                Assert.That(button2.Text, Is.EqualTo(string.Empty));
                Assert.That(button2.HasLocalDataContext(), Is.False);

                button1.SetDataContext("button1");
                Assert.That(button1.Text, Is.EqualTo("button1"));
                Assert.That(button1.HasLocalDataContext(), Is.True);

                form.SetDataContext("test");
                Assert.That(button1.Text, Is.EqualTo("button1"));
                Assert.That(button1.HasLocalDataContext(), Is.True);
                Assert.That(button2.Text, Is.EqualTo("test"));
                Assert.That(button2.HasLocalDataContext(), Is.False);

                form.Controls.Clear();
                Assert.That(button1.Text, Is.EqualTo("button1"));
                Assert.That(button2.Text, Is.EqualTo(string.Empty));
            }
        }
        [Test]
        public void InheritedDataContextBinding_BindBeforeAddToTree() {
            using(var form = new Form()) {
                var button1 = new Button();
                var button2 = new Button();
                button1.SetBinding("Text", new Binding());
                button2.SetBinding("Text", new Binding());

                form.Controls.Add(button1);
                form.SetDataContext("test");
                form.Controls.Add(button2);

                Assert.That(button1.Text, Is.EqualTo("test"));
                Assert.That(button1.HasLocalDataContext(), Is.False);

                Assert.That(button2.Text, Is.EqualTo("test"));
                Assert.That(button2.HasLocalDataContext(), Is.False);
            }
        }
        [Test]
        public void MultiLevelDataContextInheritance1() {
            using(var form = new Form()) {
                var panel = new Panel();
                var button1 = new Button();
                button1.SetBinding("Text", new Binding());
                panel.SetBinding("Tag", new Binding());
                panel.Controls.Add(button1);
                form.Controls.Add(panel);
                form.SetDataContext("test");

                Assert.That(button1.Text, Is.EqualTo("test"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                form.SetDataContext("test2");
                Assert.That(button1.Text, Is.EqualTo("test2"));
                Assert.That(button1.HasLocalDataContext(), Is.False);

                form.ClearDataContext();
                Assert.That(button1.Text, Is.EqualTo(string.Empty));

                form.SetDataContext("test2");
                Assert.That(panel.Tag, Is.EqualTo("test2"));
                Assert.That(button1.Text, Is.EqualTo("test2"));

                panel.SetDataContext("panel");
                Assert.That(panel.Tag, Is.EqualTo("panel"));
                Assert.That(button1.Text, Is.EqualTo("panel"));

                form.ClearDataContext();
                Assert.That(panel.Tag, Is.EqualTo("panel"));
                Assert.That(button1.Text, Is.EqualTo("panel"));

                form.SetDataContext("test2");
                Assert.That(panel.Tag, Is.EqualTo("panel"));
                Assert.That(button1.Text, Is.EqualTo("panel"));

                panel.ClearDataContext();
                Assert.That(panel.Tag, Is.EqualTo("test2"));
                Assert.That(button1.Text, Is.EqualTo("test2"));

                form.Controls.Clear();
                Assert.That(panel.Tag, Is.Null);
                Assert.That(button1.Text, Is.EqualTo(string.Empty));
            }
        }
        [Test]
        public void MultiLevelDataContextInheritance2() {
            using(var form = new Form()) {
                var panel = new Panel();
                var button = new Button();
                panel.Controls.Add(button);
                form.SetDataContext("test");
                form.Controls.Add(panel);
                button.SetBinding("Text", new Binding());

                Assert.That(button.Text, Is.EqualTo("test"));
                Assert.That(button.HasLocalDataContext(), Is.False);
                form.SetDataContext("test2");
                Assert.That(button.Text, Is.EqualTo("test2"));
                Assert.That(button.HasLocalDataContext(), Is.False);

                panel.SetDataContext("panel");
                Assert.That(button.Text, Is.EqualTo("panel"));
                Assert.That(button.HasLocalDataContext(), Is.False);
            }
        }
        [Test]
        public void CollectBoundControl() {
            var reference = DoCollectControlWithDataContextSet();
            TestUtils.GarbageCollect();
            Assert.That(reference.IsAlive, Is.False);
        }
        WeakReference DoCollectControlWithDataContextSet() {
            var button = new Button();
            button.SetDataContext("test");
            button.SetBinding("Text", new Binding());
            return new WeakReference(button);
        }
        [Test]
        public void SetBindingTwice() {
            using(var button = new TestButton()) {
                button.SetDataContext("test");
                button.SetBinding("Text2", new Binding());
                button.SetBinding("Text2", new Binding());
                Assert.That(button.Text2, Is.EqualTo("test"));
                Assert.That(button.Text2SetCount, Is.EqualTo(1));

                button.SetDataContext("test2");
                Assert.That(button.Text2, Is.EqualTo("test2"));
                Assert.That(button.Text2SetCount, Is.EqualTo(2));
            }
        }
        [Test]
        public void BindingWithPath() {
            var viewModel = new TestViewModel() { StringProperty = "test", StringProperty2 = "StringProperty2" };
            using(var button = new Button()) {
                button.SetDataContext(viewModel);
                button.SetBinding(() => button.Text, new Binding("StringProperty"));

                Assert.That(button.Text, Is.EqualTo("test"));
                viewModel.StringProperty = "test2";
                Assert.That(button.Text, Is.EqualTo("test2"));

                button.ClearBinding(() => button.Text);
                Assert.That(button.Text, Is.EqualTo(string.Empty));

                button.SetBinding(() => button.Text, new Binding("StringProperty"));
                Assert.That(button.Text, Is.EqualTo("test2"));
                button.SetBinding(() => button.Text, new Binding("StringProperty2"));
                Assert.That(button.Text, Is.EqualTo("StringProperty2"));

                viewModel = new TestViewModel() { StringProperty2 = "StringProperty2_", NestedViewModel = new NestedTestViewModel() { NestedStringProperty = "NestedStringProperty" } };
                button.SetDataContext(viewModel);
                Assert.That(button.Text, Is.EqualTo("StringProperty2_"));

                button.SetBinding(() => button.Text, new Binding("NestedViewModel.NestedStringProperty"));
                Assert.That(button.Text, Is.EqualTo("NestedStringProperty"));
                viewModel.NestedViewModel.NestedStringProperty = "NestedStringProperty2";
                Assert.That(button.Text, Is.EqualTo("NestedStringProperty2"));
                viewModel.NestedViewModel = new NestedTestViewModel() { NestedStringProperty = "NestedStringProperty3" };
                Assert.That(button.Text, Is.EqualTo("NestedStringProperty3"));
                viewModel.NestedViewModel = null;
                Assert.That(button.Text, Is.EqualTo(string.Empty));
            }
        }
        [Test]
        public void BindingToPathWithInheritedDataContext() {
            var viewModel = new TestViewModel();
            using(var form = new Form()) {
                var button1 = new Button() { Text = "button1" };
                form.Controls.Add(button1);

                form.SetDataContext(viewModel);
                button1.SetBinding(() => button1.Text, new Binding("StringProperty"));
                Assert.That(button1.Text, Is.EqualTo(string.Empty));

                viewModel.StringProperty = "test";
                Assert.That(button1.Text, Is.EqualTo("test"));
            }
        }
        [Test]
        public void ExtractPathFromLambdaExpression() {
            var viewModel = new TestViewModel() { StringProperty = "StringProperty", NestedViewModel = new NestedTestViewModel() { NestedStringProperty = "NestedStringProperty" } };
            using(var button = new Button()) {
                button.SetDataContext(viewModel);
                button.SetBinding(() => button.Text, new Binding(() => viewModel.StringProperty));
                Assert.That(button.Text, Is.EqualTo("StringProperty"));
                button.SetBinding(() => button.Text, new Binding(() => viewModel.NestedViewModel.NestedStringProperty));
                Assert.That(button.Text, Is.EqualTo("NestedStringProperty"));

                button.SetBinding(() => new Button().Text, new Binding(() => new TestViewModel().StringProperty));
                Assert.That(button.Text, Is.EqualTo("StringProperty"));
                button.SetBinding(() => new Button().Text, new Binding(() => new TestViewModel().NestedViewModel.NestedStringProperty));
                Assert.That(button.Text, Is.EqualTo("NestedStringProperty"));
            }
        }
        //TODO test when update comes to collected control
    }
}
