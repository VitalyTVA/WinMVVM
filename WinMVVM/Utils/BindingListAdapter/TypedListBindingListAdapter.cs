using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinMVVM.Utils.Adapter {
    internal class TypedListBindingListAdapter : BindingListAdapter, ITypedList {
        ITypedList TypedList { get { return (ITypedList)source; } }
        public TypedListBindingListAdapter(IList source, ItemPropertyNotificationMode itemPropertyNotificationMode = ItemPropertyNotificationMode.PropertyChanged)
            : base(source, itemPropertyNotificationMode) {
            if(!(source is ITypedList))
                throw new ArgumentException("source");
        }
        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors) {
            return TypedList.GetItemProperties(listAccessors);
        }
        string ITypedList.GetListName(PropertyDescriptor[] listAccessors) {
            return TypedList.GetListName(listAccessors);
        }
    }
}
