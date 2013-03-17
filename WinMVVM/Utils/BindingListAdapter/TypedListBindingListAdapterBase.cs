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
    internal abstract class TypedListBindingListAdapterBase : BindingListAdapterBase, ITypedList {
        ITypedList TypedList { get { return (ITypedList)source; } }
        public TypedListBindingListAdapterBase(IList source)
            : this(source, ItemPropertyNotificationMode.PropertyChanged) {
        }
        public TypedListBindingListAdapterBase(IList source, ItemPropertyNotificationMode itemPropertyNotificationMode)
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
