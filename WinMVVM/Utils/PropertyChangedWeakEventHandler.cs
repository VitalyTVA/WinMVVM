using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinMVVM.Utils {
    internal class PropertyChangedWeakEventHandler<TOwner> : WeakEventHandler<TOwner, PropertyChangedEventArgs, PropertyChangedEventHandler> where TOwner : class {
        public PropertyChangedWeakEventHandler(TOwner owner, Action<TOwner, object, PropertyChangedEventArgs> onEventAction)
            : base(owner, onEventAction, (h, o) => ((INotifyPropertyChanged)o).PropertyChanged -= h.Handler, h => new PropertyChangedEventHandler(h.OnEvent)) {
        }
    }
}
