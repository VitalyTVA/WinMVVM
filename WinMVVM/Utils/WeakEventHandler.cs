using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    internal class WeakEventHandler<TOwner, TEventArgs, THandler> : IWeakEventHandler<THandler> where TOwner : class {
        WeakReference ownerReference;
        Action<WeakEventHandler<TOwner, TEventArgs, THandler>, object> onDetachAction;
        Action<TOwner, object, TEventArgs> onEventAction;
        public THandler Handler { get; private set; }
        public WeakEventHandler(TOwner owner, Action<TOwner, object, TEventArgs> onEventAction, Action<WeakEventHandler<TOwner, TEventArgs, THandler>, object> onDetachAction, Func<WeakEventHandler<TOwner, TEventArgs, THandler>, THandler> createHandlerFunction) {
            this.ownerReference = new WeakReference(owner);
            this.onEventAction = onEventAction;
            this.onDetachAction = onDetachAction;
            this.Handler = createHandlerFunction(this);
        }
        public void OnEvent(object source, TEventArgs eventArgs) {
            TOwner target = ownerReference.Target as TOwner;
            if(target != null) {
                onEventAction(target, source, eventArgs);
            } else {
                onDetachAction(this, source);
            }
        }
    }
}
