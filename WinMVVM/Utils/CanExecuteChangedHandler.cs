using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    internal class CanExecuteChangedHandler : WeakEventHandler<Button, EventArgs, EventHandler> {
        public CanExecuteChangedHandler(Button owner, Action<Button, object, EventArgs> onEventAction)
            : base(owner, onEventAction, (h, o) => ((ICommand)o).CanExecuteChanged -= h.Handler, h => new EventHandler(h.OnEvent)) {
        }
    }
}
