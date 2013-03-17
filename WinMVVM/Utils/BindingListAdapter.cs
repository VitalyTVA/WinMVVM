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
        static Action<WeakEventHandler<TOwner, PropertyChangedEventArgs, PropertyChangedEventHandler>, object> action = (h, o) => ((INotifyPropertyChanged)o).PropertyChanged -= h.Handler;
        static Func<WeakEventHandler<TOwner, PropertyChangedEventArgs, PropertyChangedEventHandler>, PropertyChangedEventHandler> create = h => new PropertyChangedEventHandler(h.OnEvent);
        public PropertyChangedWeakEventHandler(TOwner owner, Action<TOwner, object, PropertyChangedEventArgs> onEventAction)
            : base(owner, onEventAction, action, create) {
        }
    }
    internal class CollectionChangedWeakEventHandler<TOwner> : WeakEventHandler<TOwner, NotifyCollectionChangedEventArgs, NotifyCollectionChangedEventHandler> where TOwner : class {
        static Action<WeakEventHandler<TOwner, NotifyCollectionChangedEventArgs, NotifyCollectionChangedEventHandler>, object> action = (h, o) => ((INotifyCollectionChanged)o).CollectionChanged -= h.Handler;
        static Func<WeakEventHandler<TOwner, NotifyCollectionChangedEventArgs, NotifyCollectionChangedEventHandler>, NotifyCollectionChangedEventHandler> create = h => new NotifyCollectionChangedEventHandler(h.OnEvent);
        public CollectionChangedWeakEventHandler(TOwner owner, Action<TOwner, object, NotifyCollectionChangedEventArgs> onEventAction)
            : base(owner, onEventAction, action, create) {
        }
    }
    [Flags]
    internal enum ItemPropertyNotificationMode {
        None = 0,
        PropertyChanged = 1
    }
    internal abstract class BindingListAdapterBase : IBindingList, IDisposable {
        static readonly PropertyChangedEventArgs EmptyEventArgs = new PropertyChangedEventArgs(null);
        readonly ItemPropertyNotificationMode itemPropertyNotificationMode;
        bool ShouldSubscribePropertyChanged { get { return (itemPropertyNotificationMode & ItemPropertyNotificationMode.PropertyChanged) != ItemPropertyNotificationMode.None; } }
        protected virtual bool ShouldSubscribePropertiesChanged { get { return itemPropertyNotificationMode != ItemPropertyNotificationMode.None; } }
        protected readonly IList source;
        INotifyCollectionChanged NotifyCollectionChanged { get { return source as INotifyCollectionChanged; } }
        PropertyChangedWeakEventHandler<BindingListAdapterBase> propertyChangedHandler;
        PropertyChangedWeakEventHandler<BindingListAdapterBase> PropertyChangedHandler {
            get {
                if(propertyChangedHandler == null) {
                    propertyChangedHandler = new PropertyChangedWeakEventHandler<BindingListAdapterBase>(this, (owner, o, e) => owner.OnObjectPropertyChanged(o, e));
                }
                return propertyChangedHandler;
            }
        }
        CollectionChangedWeakEventHandler<BindingListAdapterBase> collectionChangedHandler;
        CollectionChangedWeakEventHandler<BindingListAdapterBase> CollectionChangedHandler {
            get {
                if(collectionChangedHandler == null) {
                    collectionChangedHandler = new CollectionChangedWeakEventHandler<BindingListAdapterBase>(this, (owner, o, e) => owner.OnSourceCollectionChanged(o, e));
                }
                return collectionChangedHandler;
            }
        }
        public object OriginalDataSource { get; set; }
        public BindingListAdapterBase(IList source)
            : this(source, ItemPropertyNotificationMode.PropertyChanged) {
        }
        public BindingListAdapterBase(IList source, ItemPropertyNotificationMode itemPropertyNotificationMode)
            : this(source, itemPropertyNotificationMode, true) {
        }
        protected BindingListAdapterBase(IList source, ItemPropertyNotificationMode itemPropertyNotificationMode, bool doSubsribe) {
            this.source = source;
            this.itemPropertyNotificationMode = itemPropertyNotificationMode;
            if(doSubsribe)
                SubscribeAll(source);
        }
        protected void SubscribeAll(IList source) {
            if(NotifyCollectionChanged != null)
                NotifyCollectionChanged.CollectionChanged += CollectionChangedHandler.Handler;
            SubscribeItemsPropertyChangedEvent(source.Count);
        }
        #region IBindingList Members
        public bool SupportsSearching { get { return false; } }
        public int Find(PropertyDescriptor property, object key) {
            throw new NotSupportedException();
        }
        public bool AllowNew { get { return true; } }
        public object AddNew() {
            if(OriginalDataSource is IEditableCollectionView) {
                return ((IEditableCollectionView)OriginalDataSource).AddNew();
            } else {
                object obj = Activator.CreateInstance(ListBindingHelper.GetListItemType(source));
                Add(obj);
                AddNewInternal();
                return obj;
            }
        }
        protected virtual void AddNewInternal() { }
        public bool SupportsSorting { get { return false; } }
        public bool IsSorted { get { throw new NotSupportedException(); } }
        public void RemoveSort() { throw new NotSupportedException(); }
        public ListSortDirection SortDirection { get { throw new NotSupportedException(); } }
        public PropertyDescriptor SortProperty { get { throw new NotSupportedException(); } }
        public void ApplySort(PropertyDescriptor property, ListSortDirection direction) {
            throw new NotSupportedException();
        }
        public void AddIndex(PropertyDescriptor property) { }
        public void RemoveIndex(PropertyDescriptor property) { }

        public bool AllowRemove { get { return true; } }
        public bool AllowEdit { get { return true; } }
        public bool SupportsChangeNotification { get { return true; } }

        public event ListChangedEventHandler ListChanged;
        #endregion

        #region IList Members
        public int Add(object value) {
            return source.Add(value);
        }
        public void Clear() {
            source.Clear();
        }
        public bool Contains(object value) {
            return source.Contains(value);
        }
#if DEBUG
        public int IndexOfCallCount { get; private set; }
#endif
        public int IndexOf(object value) {
#if DEBUG
            IndexOfCallCount++;
#endif
            return source.IndexOf(value);
        }
        public void Insert(int index, object value) {
            source.Insert(index, value);
        }
        public bool IsFixedSize { get { return source.IsFixedSize; } }
        public bool IsReadOnly { get { return source.IsReadOnly; } }
        public void Remove(object value) {
            if(OriginalDataSource is IEditableCollectionView)
                ((IEditableCollectionView)OriginalDataSource).Remove(value);
            else
                source.Remove(value);
        }
        public void RemoveAt(int index) {
            if(OriginalDataSource is IEditableCollectionView)
                ((IEditableCollectionView)OriginalDataSource).RemoveAt(index);
            else
                source.RemoveAt(index);
        }
        public object this[int index] { get { return source[index]; } set { source[index] = value; } }
        #endregion

        #region ICollection Members
        public void CopyTo(Array array, int index) {
            source.CopyTo(array, index);
        }
        public int Count { get { return source.Count; } }
        public bool IsSynchronized { get { return source.IsSynchronized; } }
        public object SyncRoot { get { return source.SyncRoot; } }
        #endregion

        #region IEnumerable Members
        public IEnumerator GetEnumerator() {
            return source.GetEnumerator();
        }
        #endregion

        void SubscribeItemsPropertyChangedEvent(int count) {
            SubscribeItemsPropertyChangedEvent(count, 0);
        }
        void SubscribeItemsPropertyChangedEvent(int count, int startIndex) {
            if(!ShouldSubscribePropertiesChanged)
                return;

            for(int i = startIndex; i < count + startIndex; i++) {
                if(IsItemLoaded(i)) {
                    SubscribeItemPropertyChangedEvent(source[i]);
                }
            }
        }
        protected virtual bool IsItemLoaded(int index) {
            return true;
        }
        protected virtual void SubscribeItemPropertyChangedEvent(object item) {
            INotifyPropertyChanged notifyPropertyChanged = item as INotifyPropertyChanged;
            if(ShouldSubscribePropertyChanged && notifyPropertyChanged != null) {
                RemoveListener(notifyPropertyChanged);
                AddListener(notifyPropertyChanged);
            }
        }
        void AddListener(INotifyPropertyChanged notifyPropertyChanged) {
            notifyPropertyChanged.PropertyChanged += PropertyChangedHandler.Handler;
        }
        void RemoveListener(INotifyPropertyChanged notifyPropertyChanged) {
            notifyPropertyChanged.PropertyChanged -= PropertyChangedHandler.Handler;
        }
        void UnsubscribeItemsPropertyChangedEvent(IList oldItems, bool needCheckItemLoading) {
            if(!ShouldSubscribePropertiesChanged)
                return;
            if(oldItems != null) {
                for(int i = 0; i < oldItems.Count; i++) {
                    if(!needCheckItemLoading || IsItemLoaded(i)) {
                        UnsubscribeItemPropertyChangedEvent(oldItems[i]);
                    }
                }
            }
        }
        protected virtual void UnsubscribeItemPropertyChangedEvent(object item) {
            INotifyPropertyChanged notifyPropertyChanged = item as INotifyPropertyChanged;
            if(ShouldSubscribePropertyChanged && notifyPropertyChanged != null)
                RemoveListener(notifyPropertyChanged);
        }
        int lastChangedItemIndex = -1;
        PropertyDescriptorCollection itemProperties;

#if DEBUG
        public int ObjectPropertyChangedFireCount { get; protected set; }
#endif

        protected virtual void OnObjectPropertyChanged(object sender, PropertyChangedEventArgs e) {
            OnObjectPropertyChangedCore(sender, e);
        }
        protected virtual void OnObjectPropertyChangedCore(object sender, PropertyChangedEventArgs e) {
#if DEBUG
            ObjectPropertyChangedFireCount++;
#endif
            int itemIndex;
            if(lastChangedItemIndex >= 0 && lastChangedItemIndex < Count && object.Equals(this[lastChangedItemIndex], sender)) {
                itemIndex = lastChangedItemIndex;
            } else {
                itemIndex = IndexOf(sender);
                lastChangedItemIndex = itemIndex;
            }
            if(itemIndex >= 0) {
                if(itemProperties == null)
                    itemProperties = ListBindingHelper.GetListItemProperties(source);
                NotifyChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, itemIndex, !string.IsNullOrEmpty(e.PropertyName) ? itemProperties[e.PropertyName] : null));
            } else {
                UnsubscribeItemPropertyChangedEvent(sender);
            }
        }

        protected void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            int startingIndex;
            switch(e.Action) {
                case NotifyCollectionChangedAction.Add:
                    startingIndex = e.NewStartingIndex;
                    for(int i = 0; i < e.NewItems.Count; i++) {
                        if(startingIndex < 0)
                            startingIndex = source.IndexOf(e.NewItems[0]);
                        if(startingIndex < 0)
                            throw new InvalidOperationException("A collection Add event refers to item that does not belong to collection.");
                        NotifyChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, startingIndex + i));
                    }
                    SubscribeItemsPropertyChangedEvent(e.NewItems.Count, startingIndex);
                    break;
#if !SL
                case NotifyCollectionChangedAction.Move:
                    if(e.NewItems.Count == 1) {
                        if(e.OldStartingIndex < 0)
                            throw new InvalidOperationException("Cannot find moved item index.");
                        NotifyChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, e.NewStartingIndex, e.OldStartingIndex));
                    } else {
                        NotifyChanged(new ListChangedEventArgs(ListChangedType.Reset, e.NewStartingIndex));
                    }
                    break;
#endif
                case NotifyCollectionChangedAction.Remove:
                    if(e.OldStartingIndex < 0)
                        throw new InvalidOperationException("Cannot find removed item.");
                    for(int i = 0; i < e.OldItems.Count; i++) {
                        NotifyChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, e.OldStartingIndex, e.NewStartingIndex));
                    }
                    UnsubscribeItemsPropertyChangedEvent(e.OldItems, false);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    startingIndex = e.NewStartingIndex;
                    for(int i = 0; i < e.NewItems.Count; i++) {
                        if(startingIndex < 0)
                            startingIndex = source.IndexOf(e.NewItems[0]);
                        if(startingIndex < 0)
                            throw new InvalidOperationException("A collection Replace event refers to item that does not belong to collection.");
                        NotifyChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, startingIndex + i));
                    }
                    UnsubscribeItemsPropertyChangedEvent(e.OldItems, false);
                    SubscribeItemsPropertyChangedEvent(e.NewItems.Count, startingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    NotifyChanged(new ListChangedEventArgs(ListChangedType.Reset, e.NewStartingIndex));
                    UnsubscribeItemsPropertyChangedEvent(source, true);
                    SubscribeItemsPropertyChangedEvent(source.Count);
                    break;
            }
        }
        protected void NotifyChanged(ListChangedEventArgs e) {
            if(ListChanged != null)
                ListChanged(this, e);

        }

        #region IDisposable Members

        void IDisposable.Dispose() {
            UnsubscribeItemsPropertyChangedEvent(this.source, false);
        }

        #endregion
    }
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
