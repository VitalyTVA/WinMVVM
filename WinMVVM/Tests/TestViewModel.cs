using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace WinMVVM.Tests {
    public class TestViewModel : BindableBase {
        string stringProperty;
        public string StringProperty {
            get { return stringProperty; }
            set { SetProperty(ref stringProperty, value, () => StringProperty); }
        }

        string stringProperty2;
        public string StringProperty2 {
            get { return stringProperty2; }
            set { SetProperty(ref stringProperty2, value, () => StringProperty2); }
        }

        NestedTestViewModel nestedViewModel;
        public NestedTestViewModel NestedViewModel {
            get { return nestedViewModel; }
            set { SetProperty(ref nestedViewModel, value, () => NestedViewModel); }
        }

        public int TestCommandExecuteCount { get; private set; }
        public object ExecuteParameter { get; private set; }
        public object CanExecuteParameter { get; private set; }
        public bool CanExecuteTestCommand { get; set; }
        ICommand testCommand;
        public ICommand TestCommand {
            get {
                if(testCommand == null) {
                    testCommand = new DelegateCommand<object>(o => {
                        ExecuteParameter = o;
                        TestCommandExecuteCount++; 
                    }, o => {
                        CanExecuteParameter = o;
                        return CanExecuteTestCommand; 
                    });
                }
                return testCommand;
            }
        }


        int intProperty;
        public int IntProperty {
            get { return intProperty; }
            set { SetProperty(ref intProperty, value, () => IntProperty); }
        }

        double doubleProperty;
        public double DoubleProperty {
            get { return doubleProperty; }
            set { SetProperty(ref doubleProperty, value, () => DoubleProperty); }
        }

        public TestViewModel() {
            CanExecuteTestCommand = true;
        }
        
    }
    public class NestedTestViewModel : BindableBase {

        string nestedStringProperty;

        public string NestedStringProperty {
            get { return nestedStringProperty; }
            set { SetProperty(ref nestedStringProperty, value, () => NestedStringProperty); }
        }

    }
}
