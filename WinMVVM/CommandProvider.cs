using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Utils;

namespace WinMVVM {
    public static class CommandProvider {
        public static readonly AttachedProperty<ICommand> CommandProperty = AttachedProperty<ICommand>.Register(() => CommandProperty, new PropertyMetadata<ICommand>(null));
        public static ICommand GetCommand(this Control control) {
            return control.GetValue(CommandProperty);
        }
        public static void SetCommand(this Control control, ICommand value) {
            control.SetValue(CommandProperty, value);
        }
    }
}
