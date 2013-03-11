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
        public static readonly AttachedProperty<ICommand> CommandProperty = AttachedProperty<ICommand>.Register(() => CommandProperty, new PropertyMetadata<ICommand>(null, OnCommandChanged));
        public static ICommand GetCommand(this Control control) {
            return control.GetValue(CommandProperty);
        }
        public static void SetCommand(this Control control, ICommand value) {
            control.SetValue(CommandProperty, value);
        }

        public static readonly AttachedProperty<object> CommandParameterProperty = AttachedProperty<object>.Register(() => CommandParameterProperty, new PropertyMetadata<object>(null, OnCommandParameterChanged));
        public static object GetCommandParameter(this Control control) {
            return control.GetValue(CommandParameterProperty);
        }
        public static void SetCommandParameter(this Control control, object value) {
            control.SetValue(CommandParameterProperty, value);
        }

        static void OnCommandChanged(Control sender, AttachedPropertyChangedEventArgs<ICommand> e) {
            Button b = sender as Button;
            if(b == null)
                return;
            b.Click -= OnClick;
            if(e.NewValue != null)
                b.Click += OnClick;
        }
        private static void OnCommandParameterChanged(Control sender, AttachedPropertyChangedEventArgs<object> e) {
        }

#if DEBUG
        internal static int OnClickCount { get; set; }
#endif
        static void OnClick(object sender, EventArgs e) {
#if DEBUG
            OnClickCount++;
#endif
            Control control = (Control)sender;
            ICommand command = GetCommand(control);
            object parameter = GetCommandParameter(control);
            if(command != null && command.CanExecute(parameter))
                command.Execute(parameter);
        }
    }
}
