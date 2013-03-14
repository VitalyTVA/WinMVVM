using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM.Tests {
    [TestFixture]
    public class BindingOperationsTests {
        [Test]
        public void BindingModeProperty() {
            Assert.That(new Binding().Mode, Is.EqualTo(BindingMode.OneWay));
            Assert.That(new Binding("").Mode, Is.EqualTo(BindingMode.OneWay));
            Assert.That(new Binding(() => new TestViewModel().DoubleProperty).Mode, Is.EqualTo(BindingMode.OneWay));
            Assert.That(new Binding("", BindingMode.OneWay).Mode, Is.EqualTo(BindingMode.OneWay));
            Assert.That(new Binding("", BindingMode.TwoWay).Mode, Is.EqualTo(BindingMode.TwoWay));
            Assert.That(new Binding(() => new TestViewModel().DoubleProperty, BindingMode.OneWay).Mode, Is.EqualTo(BindingMode.OneWay));
            Assert.That(new Binding(() => new TestViewModel().DoubleProperty, BindingMode.TwoWay).Mode, Is.EqualTo(BindingMode.TwoWay));
            Assert.That(object.Equals(new Binding("", BindingMode.OneWay), new Binding("", BindingMode.TwoWay)), Is.False);
            Assert.That(object.Equals(new Binding("", BindingMode.OneWay), new Binding("", BindingMode.OneWay)), Is.True);
        }
        [Test]
        public void NullControl() {
            Assert.Throws<ArgumentNullException>(() => BindingOperations.SetBinding(null, "Test", new Binding()));
            Assert.Throws<ArgumentNullException>(() => BindingOperations.ClearBinding(null, "Test"));
            using(var button = new Button()) {
                Assert.Throws<ArgumentOutOfRangeException>(() => BindingOperations.SetBinding(button, null, new Binding()));
                Assert.Throws<ArgumentOutOfRangeException>(() => BindingOperations.SetBinding(button, string.Empty, new Binding()));
                Assert.Throws<ArgumentException>(() => BindingOperations.SetBinding(button, "Foo", new Binding()));
                Assert.Throws<ArgumentNullException>(() => BindingOperations.SetBinding(button, "Text", null));
                Assert.Throws<ArgumentOutOfRangeException>(() => BindingOperations.ClearBinding(button, null));
                Assert.Throws<ArgumentOutOfRangeException>(() => BindingOperations.ClearBinding(button, string.Empty));
                Assert.Throws<ArgumentException>(() => BindingOperations.ClearBinding(button, "Foo"));
                AttachedProperty<object> p = null;
                Assert.Throws<ArgumentNullException>(() => BindingOperations.SetBinding(button, p, new Binding()));
                Assert.Throws<ArgumentNullException>(() => BindingOperations.SetBinding(null, TextProperty, new Binding()));
                Assert.Throws<ArgumentNullException>(() => BindingOperations.ClearBinding(null, TextProperty));
                Assert.Throws<ArgumentNullException>(() => BindingOperations.ClearBinding(button, p));
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
        static readonly AttachedProperty<string> TextProperty = AttachedProperty<string>.Register(() => TextProperty);
        [Test]
        public void BindAttachedProperty() {
            var viewModel = new TestViewModel();
            using(var button = new Button()) {
                button.SetValue(TextProperty, "button1");
                button.SetDataContext(viewModel);
                button.SetBinding(TextProperty, new Binding(() => new TestViewModel().StringProperty));
                button.SetBinding(() => new Button().Text, new Binding(() => new TestViewModel().StringProperty2));
                Assert.That(button.GetValue(TextProperty), Is.EqualTo(null));
                Assert.That(button.Text, Is.EqualTo(string.Empty));

                viewModel.StringProperty = "test";
                Assert.That(button.GetValue(TextProperty), Is.EqualTo("test"));
                Assert.That(button.Text, Is.EqualTo(string.Empty));

                viewModel.StringProperty2 = "test2";
                Assert.That(button.GetValue(TextProperty), Is.EqualTo("test"));
                Assert.That(button.Text, Is.EqualTo("test2"));

                button.ClearBinding(TextProperty);
                Assert.That(button.GetValue(TextProperty), Is.Null);
            }
        }
        [Test]
        public void BindAttachedProperty2() {
            var viewModel = new TestViewModel();
            using(var button = new Button()) {
                button.SetValue(TextProperty, "button1");
                button.SetDataContext(viewModel);
                button.SetBinding(() => new Button().Text, new Binding(() => new TestViewModel().StringProperty2));
                button.SetBinding(TextProperty, new Binding(() => new TestViewModel().StringProperty));
                Assert.That(button.GetValue(TextProperty), Is.EqualTo(null));
                Assert.That(button.Text, Is.EqualTo(string.Empty));

                viewModel.StringProperty = "test";
                Assert.That(button.GetValue(TextProperty), Is.EqualTo("test"));
                Assert.That(button.Text, Is.EqualTo(string.Empty));

                viewModel.StringProperty2 = "test2";
                Assert.That(button.GetValue(TextProperty), Is.EqualTo("test"));
                Assert.That(button.Text, Is.EqualTo("test2"));

                button.ClearBinding(TextProperty);
                Assert.That(button.GetValue(TextProperty), Is.Null);
            }
        }
        [Test]
        public void AttachedPropertyDescriptorTests() {
            PropertyDescriptorBase pd1 = AttachedPropertyDescriptor.FromAttachedProperty(TextProperty);
            PropertyDescriptorBase pd2 = AttachedPropertyDescriptor.FromAttachedProperty(TextProperty);
            PropertyDescriptorBase pd3 = AttachedPropertyDescriptor.FromAttachedProperty(DataContextProvider.DataContextProperty);
            Assert.That(pd1.PropertyType, Is.EqualTo(typeof(string)));
            Assert.That(object.Equals(pd1, pd2), Is.True);
            Assert.That(object.Equals(pd1.GetHashCode(), pd2.GetHashCode()), Is.True);
            Assert.That(object.Equals(pd1, pd3), Is.False);
            using(Button button = new Button()) {
                Assert.That(object.Equals(pd1, StandardPropertyDescriptor.FromPropertyName(button, "Text")), Is.False);
                Assert.That(object.Equals(StandardPropertyDescriptor.FromPropertyName(button, "Text"), pd1), Is.False); 
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
        public void CollectBoundControl_TwoWay() {
            var viewModel = new TestViewModel();
            var reference = DoCollectTwoWayBoudnDataControl(viewModel);
            TestUtils.GarbageCollect();
            Assert.That(reference.IsAlive, Is.False);
            viewModel.StringProperty = "new Value";
        }
        WeakReference DoCollectTwoWayBoudnDataControl(TestViewModel viewModel) {
            var textBox = new TextBox();
            textBox.SetDataContext(viewModel);
            textBox.SetBinding("Text", new Binding(() => viewModel.StringProperty, BindingMode.TwoWay));
            return new WeakReference(textBox);
        }
        [Test]
        public void CollectBoundControl_TwoWay_AttachedProperty() {
            var viewModel = new TestViewModel();
            var reference = DoCollectTwoWayBoudnDataControl_AttachedProperty(viewModel);
            TestUtils.GarbageCollect();
            Assert.That(reference.IsAlive, Is.False);
            viewModel.StringProperty = "new Value";
        }
        WeakReference DoCollectTwoWayBoudnDataControl_AttachedProperty(TestViewModel viewModel) {
            var textBox = new TextBox();
            textBox.SetDataContext(viewModel);
            textBox.SetBinding(CommandProvider.CommandParameterProperty, new Binding(() => viewModel.StringProperty, BindingMode.TwoWay));
            return new WeakReference(textBox);
        }
        [Test]
        public void CollectViewModel_TwoWay_AttachedProperty() {
            var textBox = new TextBox();
            var reference = DoCollectViewModel_TwoWay_AttachedProperty(textBox);
            TestUtils.GarbageCollect();
            Assert.That(reference.IsAlive, Is.True);
            textBox.ClearDataContext();
            TestUtils.GarbageCollect();
            Assert.That(reference.IsAlive, Is.False);
        }
        WeakReference DoCollectViewModel_TwoWay_AttachedProperty(TextBox textBox) {
            var viewModel = new TestViewModel();
            textBox.SetDataContext(viewModel);
            textBox.SetBinding(CommandProvider.CommandParameterProperty, new Binding(() => viewModel.StringProperty, BindingMode.TwoWay));
            return new WeakReference(viewModel);
        }
        [Test]
        public void CollectBindingSourceOnlyAfterClearBinding() {
            using(var button = new Button()) {
                WeakReference reference = DoBindButtonAndGetViewModelReference(button);
                TestUtils.GarbageCollect();
                Assert.That(reference.IsAlive, Is.True);
                Assert.That(button.Text, Is.EqualTo("test"));
                button.ClearDataContext();
                TestUtils.GarbageCollect();
                Assert.That(reference.IsAlive, Is.False);
                Assert.That(button.Text, Is.EqualTo(string.Empty));
            }
        }
        WeakReference DoBindButtonAndGetViewModelReference(Button button) {
            var viewModel = new TestViewModel() { StringProperty = "test" };
            button.SetDataContext(viewModel);
            button.SetBinding("Text", new Binding(() => viewModel.StringProperty));
            return new WeakReference(viewModel);
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
        [Test]
        public void ConvertToTargetPropertyType() {
            var viewModel = new TestViewModel() { IntProperty = 9, DoubleProperty = 153.2 };
            using(var button = new Button()) {
                button.SetDataContext(viewModel);
                button.SetBinding(() => button.Text, new Binding(() => viewModel.IntProperty));
                Assert.That(button.Text, Is.EqualTo("9"));
                viewModel.IntProperty = 13;
                Assert.That(button.Text, Is.EqualTo("13"));

                button.SetBinding(() => button.TabIndex, new Binding(() => viewModel.DoubleProperty));
                Assert.That(button.TabIndex, Is.EqualTo(153));
            }
        }
        [Test]
        public void TwoWayBinding() {
            var viewModel = new TestViewModel() { };
            using(var textBox = new TextBox()) {
                textBox.SetDataContext(viewModel);
                textBox.SetBinding(() => textBox.Text, new Binding(() => viewModel.StringProperty));
                Assert.That(textBox.Text, Is.EqualTo(string.Empty));
                textBox.Text = "test";
                Assert.That(viewModel.StringProperty, Is.EqualTo(null));

                textBox.SetBinding(() => textBox.Text, new Binding(() => viewModel.StringProperty, BindingMode.TwoWay));
                Assert.That(textBox.Text, Is.EqualTo(string.Empty));
                textBox.Text = "test";
                Assert.That(viewModel.StringProperty, Is.EqualTo("test"));

            }
        }
        public class TestControl : Control {
            private string testProperty;
            public string TestProperty {
                get { return testProperty; }
                set {
                    if(testProperty == value)
                        return;
                    testProperty = value;
                    if(TestPropertyChanged != null)
                        TestPropertyChanged(this, EventArgs.Empty);
                }
            }
            public event EventHandler TestPropertyChanged;
            public int TestPropertyChangedSubscribeCount { get { return TestPropertyChanged != null ? TestPropertyChanged.GetInvocationList().Count() : 0; } }
        }
        [Test]
        public void ClearTwoWayBinding() {
            var viewModel = new TestViewModel() { };
            using(var testControl = new TestControl()) {
                testControl.SetDataContext(viewModel);

                Assert.That(testControl.TestPropertyChangedSubscribeCount, Is.EqualTo(0));
                testControl.SetBinding(() => testControl.TestProperty, new Binding(() => viewModel.StringProperty, BindingMode.TwoWay));
                Assert.That(testControl.TestPropertyChangedSubscribeCount, Is.EqualTo(1));

                Assert.That(testControl.TestProperty, Is.Null);
                testControl.TestProperty = "test";
                Assert.That(viewModel.StringProperty, Is.EqualTo("test"));
                Assert.That(testControl.TestPropertyChangedSubscribeCount, Is.EqualTo(1));

                testControl.ClearBinding(() => testControl.TestProperty);
                Assert.That(viewModel.StringProperty, Is.EqualTo("test"));
                Assert.That(testControl.TestProperty, Is.Null);
                Assert.That(testControl.TestPropertyChangedSubscribeCount, Is.EqualTo(0));

                viewModel.StringProperty = "test2";
                Assert.That(testControl.TestProperty, Is.Null);
            }
        }
        public class UnstablePropertyViewModel : BindableBase {
            string myProperty;
            public string MyProperty {
                get { return myProperty; }
                set { SetProperty(ref myProperty, value + "_", () => MyProperty); }
            }
        }
        [Test]
        public void TwoWayBindingToUnstableProperty() {
            var viewModel = new UnstablePropertyViewModel() { };
            using(var textBox = new TextBox()) {
                textBox.SetDataContext(viewModel);

                textBox.SetBinding(() => textBox.Text, new Binding(() => viewModel.MyProperty, BindingMode.TwoWay));
                Assert.That(textBox.Text, Is.EqualTo(string.Empty));
                textBox.Text = "test";
                Assert.That(viewModel.MyProperty, Is.EqualTo("test_"));
                Assert.That(textBox.Text, Is.EqualTo("test_"));

            }
        }
        [Test]
        public void TwoWayBindingDirectlyToDataContext() {
            var viewModel = "test";
            using(var textBox = new TextBox()) {
                textBox.SetDataContext(viewModel);
                Assert.Throws<InvalidOperationException>(() => {
                    textBox.SetBinding(() => textBox.Text, new Binding(string.Empty, BindingMode.TwoWay));
                }, SR.TwoWayBindingRequiresPathExceptionMessage);

            }
        }
        [Test]
        public void TwoWayBindingForAttachedProperty() {
            var viewModel = new TestViewModel() { };
            using(var button = new Button()) {
                button.SetDataContext(viewModel);

                PropertyEntry<object> entry = CommandProvider.CommandParameterProperty.GetPropertyEntry(button);

                Assert.That(entry.ChangedSubscribeCount, Is.EqualTo(0));
                button.SetBinding(CommandProvider.CommandParameterProperty, new Binding(() => viewModel.StringProperty, BindingMode.TwoWay));
                Assert.That(entry.ChangedSubscribeCount, Is.EqualTo(1));

                Assert.That(button.GetCommandParameter(), Is.Null);
                button.SetCommandParameter("test");
                Assert.That(viewModel.StringProperty, Is.EqualTo("test"));

                button.ClearBinding(CommandProvider.CommandParameterProperty);
                Assert.That(entry.ChangedSubscribeCount, Is.EqualTo(0));
                Assert.That(viewModel.StringProperty, Is.EqualTo("test"));
                Assert.That(button.GetCommandParameter(), Is.Null);

                viewModel.StringProperty = "test2";
                Assert.That(button.GetCommandParameter(), Is.Null);

                button.SetCommandParameter("test3");
            }
        }
        //TODO test when update comes to collected control
    }
}
