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
#if DEBUG
        internal 
#endif
        static readonly AttachedProperty<CanExecuteChangedHandler> HandlerProperty = AttachedProperty<CanExecuteChangedHandler>.Register(() => HandlerProperty);

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
            Button button = sender as Button;
            if(button == null)
                return;
            UpdateButtonEnabled(button, e.NewValue, GetCommandParameter(button));
            button.Click -= OnClick;
            if(e.OldValue != null) {
                var handler = button.GetValue(HandlerProperty);
                e.OldValue.CanExecuteChanged -= handler.Handler;
                button.ClearValue(HandlerProperty);
            }
            if(e.NewValue != null) {
                button.Click += OnClick;
                var handler = new CanExecuteChangedHandler(button, (b, o, ea) => UpdateButtonEnabled(b, b.GetCommand(), b.GetCommandParameter()));
                e.NewValue.CanExecuteChanged += handler.Handler;
                button.SetValue(HandlerProperty, handler);
            }
        }
        private static void OnCommandParameterChanged(Control sender, AttachedPropertyChangedEventArgs<object> e) {
            var button = sender as Button;
            if(button != null)
                UpdateButtonEnabled(button, GetCommand(button), e.NewValue);
        }

#if DEBUG
        internal static int OnClickCount { get; set; }
#endif
        static void OnClick(object sender, EventArgs e) {
#if DEBUG
            OnClickCount++;
#endif
            Button control = (Button)sender;
            ICommand command = GetCommand(control);
            object parameter = GetCommandParameter(control);
            if(command != null && command.CanExecute(parameter))
                command.Execute(parameter);
        }
        static void UpdateButtonEnabled(Button button, ICommand command, object commandParameter) {
            button.Enabled = command != null ? command.CanExecute(commandParameter) : true;
        }
    }
}
