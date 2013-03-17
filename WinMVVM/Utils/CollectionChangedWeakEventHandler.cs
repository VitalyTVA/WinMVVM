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
    internal class CollectionChangedWeakEventHandler<TOwner> : WeakEventHandler<TOwner, NotifyCollectionChangedEventArgs, NotifyCollectionChangedEventHandler> where TOwner : class {
        public CollectionChangedWeakEventHandler(TOwner owner, Action<TOwner, object, NotifyCollectionChangedEventArgs> onEventAction)
            : base(owner, onEventAction, (h, o) => ((INotifyCollectionChanged)o).CollectionChanged -= h.Handler, h => new NotifyCollectionChangedEventHandler(h.OnEvent)) {
        }
    }
}
