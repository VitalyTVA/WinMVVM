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
    internal class BindingListAdapter : BindingListAdapterBase, ICancelAddNew {
        static readonly PropertyChangedEventArgs EmptyEventArgs = new PropertyChangedEventArgs(null);
        public static BindingListAdapter CreateFromList(IList list, ItemPropertyNotificationMode itemPropertyNotificationMode = ItemPropertyNotificationMode.PropertyChanged) {
            return list is ITypedList ? new TypedListBindingListAdapter(list, itemPropertyNotificationMode) : new BindingListAdapter(list, itemPropertyNotificationMode);
        }
        bool isNewItemRowEditing;
        readonly ItemPropertyNotificationMode itemPropertyNotificationMode;
        bool ShouldSubscribePropertyChanged { get { return (itemPropertyNotificationMode & ItemPropertyNotificationMode.PropertyChanged) != ItemPropertyNotificationMode.None; } }
        protected override bool ShouldSubscribePropertiesChanged { get { return itemPropertyNotificationMode != ItemPropertyNotificationMode.None; } }
        internal BindingListAdapter(IList source, ItemPropertyNotificationMode itemPropertyNotificationMode = ItemPropertyNotificationMode.PropertyChanged)
            : base(source, itemPropertyNotificationMode, false) {
            this.itemPropertyNotificationMode = itemPropertyNotificationMode;
            SubscribeAll(source);
        }

        void OnObjectDependencyPropertyChanged(object sender, EventArgs e) {
            OnObjectPropertyChanged(sender, EmptyEventArgs);
        }

        protected override void AddNewInternal() {
            isNewItemRowEditing = true;
        }
        #region ICancelAddNew Members
        public void CancelNew(int itemIndex) {
            if(OriginalDataSource is IEditableCollectionView) {
                ((IEditableCollectionView)OriginalDataSource).CancelNew();
            } else {
                if(isNewItemRowEditing) {
                    RemoveAt(itemIndex);
                    isNewItemRowEditing = false;
                }
            }
        }
        public void EndNew(int itemIndex) {
            if(OriginalDataSource is IEditableCollectionView) {
                ((IEditableCollectionView)OriginalDataSource).CommitNew();
            } else {
                isNewItemRowEditing = false;
            }
        }
        #endregion
    }
}
