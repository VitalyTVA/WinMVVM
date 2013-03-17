using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WinMVVM.Utils;

namespace WinMVVM.Tests {
    public class CarData {
        public string Model { get; set; }
        public string Trademark { get; set; }
    }
    #region CollectionWrapper
    public class CollectionWrapperWithoutPublicIndexer<T> : IList, IList<T>, INotifyCollectionChanged {
        ObservableCollection<T> collection;
        NotifyCollectionChangedEventHandler collectionChanged;
        IList List { get { return collection; } }
        public CollectionWrapperWithoutPublicIndexer(ObservableCollection<T> collection) {
            this.collection = collection;
        }
        #region IList Members
        int IList.Add(object value) { return List.Add(value); }
        void IList.Clear() { List.Clear(); }
        bool IList.Contains(object value) { return List.Contains(value); }
        int IList.IndexOf(object value) { return List.IndexOf(value); }
        void IList.Insert(int index, object value) { List.Insert(index, value); }
        bool IList.IsFixedSize { get { return List.IsFixedSize; } }
        bool IList.IsReadOnly { get { return List.IsReadOnly; } }
        void IList.Remove(object value) { List.Remove(value); }
        void IList.RemoveAt(int index) { List.RemoveAt(index); }
        object IList.this[int index] { get { return List[index]; } set { List[index] = value; } }
        #endregion
        #region ICollection Members
        void ICollection.CopyTo(Array array, int index) { List.CopyTo(array, index); }
        int ICollection.Count { get { return List.Count; } }
        bool ICollection.IsSynchronized { get { return List.IsSynchronized; } }
        object ICollection.SyncRoot { get { return List.SyncRoot; } }
        #endregion
        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator() { return List.GetEnumerator(); }
        #endregion
        #region IList<T> Members
        int IList<T>.IndexOf(T item) { return collection.IndexOf(item); }
        void IList<T>.Insert(int index, T item) { collection.Insert(index, item); }
        void IList<T>.RemoveAt(int index) { collection.RemoveAt(index); }
        T IList<T>.this[int index] { get { return collection[index]; } set { collection[index] = value; } }
        #endregion
        #region ICollection<T> Members
        void ICollection<T>.Add(T item) { collection.Add(item); }
        void ICollection<T>.Clear() { collection.Clear(); }
        bool ICollection<T>.Contains(T item) { return collection.Contains(item); }
        void ICollection<T>.CopyTo(T[] array, int arrayIndex) { collection.CopyTo(array, arrayIndex); }
        int ICollection<T>.Count { get { return collection.Count; } }
        bool ICollection<T>.IsReadOnly { get { return ((ICollection<T>)collection).IsReadOnly; } }
        bool ICollection<T>.Remove(T item) { return collection.Remove(item); }
        #endregion
        #region IEnumerable<T> Members
        IEnumerator<T> IEnumerable<T>.GetEnumerator() { return collection.GetEnumerator(); }
        #endregion
        #region INotifyCollectionChanged Members
        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged {
            add { collectionChanged += value; }
            remove { collectionChanged -= value; }
        }
        #endregion
    }
    #endregion
    public static class TestDataObjectExtensions {
        public static object GetNumber(this object obj) {
            return obj.GetPropertyValue("Number");
        }
        public static object GetText(this object obj) {
            return obj.GetPropertyValue("Text");
        }
        public static object GetGroup1(this object obj) {
            return obj.GetPropertyValue("Group1");
        }
        public static object GetGroup2(this object obj) {
            return obj.GetPropertyValue("Group2");
        }
        public static object GetPropertyValue(this object obj, string propertyName) {
            return GetProperties(obj)[propertyName].GetValue(obj);
        }
        public static void SetPropertyValue(this object obj, string propertyName, object value) {
            GetProperties(obj)[propertyName].SetValue(obj, value);
        }
        public static PropertyDescriptorCollection GetProperties(object obj) {
#if SL
            if(obj is CustomTypeDescriptorBase)
                return ((CustomTypeDescriptorBase)obj).GetProperties(null);
#endif
            return TypeDescriptor.GetProperties(obj);
        }
    }
    [TestFixture]
    public class BindingListAdapterTests {
        public class TestData : INotifyPropertyChanged {
            int number;
            string text;
            public int Number {
                get { return number; }
                set {
                    if(number == value)
                        return;
                    number = value;
                    NotifyChanged("Number");
                }
            }
            public string Text {
                get { return text; }
                set {
                    if(text == value)
                        return;
                    text = value;
                    NotifyChanged("Text");
                }
            }
            public int GetPropertyChangedInvocationCount() {
                return propertyChanged != null ? propertyChanged.GetInvocationList().Count() : 0;
            }
            public Delegate[] GetPropertyChangedInvocationList() {
                return propertyChanged != null ? propertyChanged.GetInvocationList() : null;
            }
            protected void NotifyChanged(string propertyName) {
                if(propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            #region INotifyPropertyChanged Members
            event PropertyChangedEventHandler propertyChanged;
            #endregion

            #region INotifyPropertyChanged Members

            event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
                add { propertyChanged += value; }
                remove { propertyChanged -= value; }
            }

            #endregion
        }
        class ManuallyNotifiedCollection : List<TestData>, INotifyCollectionChanged {
            public void NotifyChanged(NotifyCollectionChangedEventArgs e) {
                if(CollectionChanged != null)
                    CollectionChanged(this, e);
            }
            public event NotifyCollectionChangedEventHandler CollectionChanged;
        }
        ObservableCollection<TestData> collection;
        BindingListAdapter adapter;
        BindingList<TestData> patternBindingList;
        List<ListChangedEventArgs> adapterNotifications = new List<ListChangedEventArgs>();
        List<ListChangedEventArgs> patternBindingListNotifications = new List<ListChangedEventArgs>();
        [SetUp]
        public void SetUp() {
            collection = new ObservableCollection<TestData>();
            collection.Add(new TestData() { Number = 0, Text = "text 0" });
            collection.Add(new TestData() { Number = 1, Text = "text 1" });
            collection.Add(new TestData() { Number = 2, Text = "text 2" });
            adapter = new BindingListAdapter(collection);

            patternBindingList = new BindingList<TestData>();
            patternBindingList.Add(new TestData() { Number = 0, Text = "text 0" });
            patternBindingList.Add(new TestData() { Number = 1, Text = "text 1" });
            patternBindingList.Add(new TestData() { Number = 2, Text = "text 2" });

            ResetListening();
        }
        [TearDown]
        public void TearDown() {
            adapter.ListChanged -= new ListChangedEventHandler(adapter_ListChanged);
            adapter = null;
            patternBindingList.ListChanged -= new ListChangedEventHandler(patternBindingList_ListChanged);
            patternBindingList = null;
        }
        void StartListening() {
            adapter.ListChanged += new ListChangedEventHandler(adapter_ListChanged);
            patternBindingList.ListChanged += new ListChangedEventHandler(patternBindingList_ListChanged);
        }
        void ResetListening() {
            adapterNotifications.Clear();
            patternBindingListNotifications.Clear();
        }
        void patternBindingList_ListChanged(object sender, ListChangedEventArgs e) {
            patternBindingListNotifications.Add(e);
        }
        void adapter_ListChanged(object sender, ListChangedEventArgs e) {
            adapterNotifications.Add(e);
        }
        void AssertNotifications() {
            Assert.AreEqual(patternBindingListNotifications.Count, adapterNotifications.Count);
            for(int i = 0; i < patternBindingListNotifications.Count; i++) {
                Assert.AreEqual(patternBindingListNotifications[i].ListChangedType, adapterNotifications[i].ListChangedType);
                Assert.AreEqual(patternBindingListNotifications[i].NewIndex, adapterNotifications[i].NewIndex);
                Assert.AreEqual(patternBindingListNotifications[i].OldIndex, adapterNotifications[i].OldIndex);
#if !SL
                Assert.AreEqual(patternBindingListNotifications[i].PropertyDescriptor, adapterNotifications[i].PropertyDescriptor);
#endif
            }
        }
        [Test]
        public void ItemSubscriptionAtOnce() {
            TestData item = collection[0];
            collection.Clear();
            collection.Add(item);
            collection.Clear();
            collection.Add(item);
            collection.Clear();
            collection.Add(item);
            collection.Clear();
            collection.Add(item);
            Assert.AreEqual(0, adapter.ObjectPropertyChangedFireCount);
            item.Number = 100;
            Assert.AreEqual(1, adapter.ObjectPropertyChangedFireCount);
        }
        [Test]
        public void IEnumerableImplementation() {
            IEnumerator fromCollection = collection.GetEnumerator();
            IEnumerator fromAdapter = adapter.GetEnumerator();
            while(fromCollection.MoveNext()) {
                Assert.IsTrue(fromAdapter.MoveNext());
                Assert.AreEqual(fromCollection.Current, fromAdapter.Current);
            }
        }
        [Test]
        public void ICollectionImplementation() {
            Assert.AreEqual(collection.Count, adapter.Count);
            Assert.AreEqual(((ICollection)collection).IsSynchronized, adapter.IsSynchronized);
            Assert.AreEqual(((ICollection)collection).SyncRoot, adapter.SyncRoot);
            TestData[] fromAdapter = new TestData[3];
            TestData[] fromCollection = new TestData[3];
            adapter.CopyTo(fromAdapter, 0);
            collection.CopyTo(fromCollection, 0);
            Assert.AreEqual(fromAdapter[0], collection[0]);
            Assert.AreEqual(fromCollection[0], collection[0]);
        }
        [Test]
        public void IListImplementation() {
            BindingListAdapter readOnlyAdapter = new BindingListAdapter(new ReadOnlyObservableCollection<TestData>(collection));
            Assert.IsTrue(readOnlyAdapter.IsReadOnly);
            Assert.IsFalse(adapter.IsReadOnly);
            Assert.IsFalse(((IList)collection).IsReadOnly);
            Assert.AreEqual(collection[0], adapter[0]);

            adapter[1] = new TestData() { Number = -1 };
            Assert.AreEqual(collection[1], adapter[1]);
            Assert.AreEqual(-1, collection[1].GetNumber());

            adapter.Remove(collection[0]);
            Assert.AreEqual(-1, collection[0].GetNumber());
            Assert.AreEqual(2, collection.Count);

            adapter.RemoveAt(0);
            Assert.AreEqual(2, collection[0].GetNumber());
            Assert.AreEqual(1, collection.Count);

            Assert.AreEqual(((IList)collection).IsFixedSize, adapter.IsFixedSize);

            adapter.Insert(0, new TestData() { Number = -2 });
            Assert.AreEqual(-2, collection[0].GetNumber());
            Assert.AreEqual(2, collection.Count);

            adapter.Add(new TestData() { Number = 10 });
            Assert.AreEqual(10, collection[2].GetNumber());
            Assert.AreEqual(3, collection.Count);

            Assert.AreEqual(1, adapter.IndexOf(collection[1]));
            Assert.AreEqual(-1, adapter.IndexOf(new TestData()));

            Assert.AreEqual(true, adapter.Contains(collection[1]));
            Assert.AreEqual(false, adapter.Contains(new TestData()));

            adapter.Clear();
            Assert.AreEqual(0, collection.Count);
        }
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void IBindingListImplementation_SupportSearching() {
            Assert.IsFalse(adapter.SupportsSearching);
            adapter.Find(TypeDescriptor.GetProperties(typeof(TestData))["Number"], 1);
        }
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void IBindingListImplementation_SupportSorting_IsSorted() {
            Assert.IsFalse(adapter.SupportsSorting);
            bool ignore = adapter.IsSorted;
        }
        [Test]
        public void IBindingListImplementation_SupportSorting_AllowNew() {
            AssertIBindingListImplementation_SupportSorting_AllowNew();
        }
        void AssertIBindingListImplementation_SupportSorting_AllowNew() {
            Assert.IsTrue(adapter.AllowNew);
            int oldCount = adapter.Count;
            adapter.AddNew();
            Assert.AreEqual(oldCount + 1, adapter.Count);
            //Assert.IsInstanceOfType(typeof(TestData), adapter[adapter.Count - 1]);
        }
        [Test]
        public void IBindingListImplementation_SupportSorting_AllowNew_NoPublicIndexer() {
            adapter = new BindingListAdapter(new CollectionWrapperWithoutPublicIndexer<TestData>(collection));
            AssertIBindingListImplementation_SupportSorting_AllowNew();
        }
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void IBindingListImplementation_SupportSorting_RemoveSort() {
            adapter.RemoveSort();
        }
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void IBindingListImplementation_SupportSorting_SortDirection() {
            ListSortDirection ignore = adapter.SortDirection;
        }
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void IBindingListImplementation_SupportSorting_SortProperty() {
            object ignore = adapter.SortProperty;
        }
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void IBindingListImplementation_SupportSorting_ApplySort() {
            adapter.ApplySort(TypeDescriptor.GetProperties(typeof(TestData))["Number"], ListSortDirection.Ascending);
        }
        [Test]
        public void IBindingListImplementation_SupportSorting_AddRemoveIndex_AllowRemove_AllowEdit_SupportsChangeNotification() {
            adapter.AddIndex(TypeDescriptor.GetProperties(typeof(TestData))["Number"]);
            adapter.RemoveIndex(TypeDescriptor.GetProperties(typeof(TestData))["Number"]);
            Assert.IsTrue(adapter.AllowRemove);
            Assert.IsTrue(adapter.AllowEdit);
            Assert.IsTrue(adapter.SupportsChangeNotification);
        }
        [Test]
        public void AddItemNotification() {
            StartListening();
            patternBindingList.Add(new TestData() { Number = 117 });
            collection.Add(new TestData() { Number = 117 });
            AssertNotifications();
            patternBindingList.Add(new TestData() { Number = 117 });
            collection.Add(new TestData() { Number = 117 });
            AssertNotifications();
        }
        [Test]
        public void RemoveItemNotification() {
            StartListening();
            patternBindingList.RemoveAt(2);
            collection.RemoveAt(2);
            AssertNotifications();
            patternBindingList.RemoveAt(0);
            collection.RemoveAt(0);
            AssertNotifications();
        }
        [Test]
        public void ReplaceItemNotification() {
            StartListening();
            patternBindingList[0] = new TestData();
            collection[0] = new TestData();
            AssertNotifications();
            patternBindingList[1] = new TestData();
            collection[1] = new TestData();
            AssertNotifications();
        }
        [Test]
        public void ChangeItemNotification() {
            StartListening();
            patternBindingList[0].Number = 117;
            collection[0].Number = 117;
            AssertNotifications();
            patternBindingList[1].Number = 117;
            collection[1].Number = 117;
            AssertNotifications();
        }
        [Test]
        public void ClearNotification() {
            StartListening();
            patternBindingList.Clear();
            collection.Clear();
            AssertNotifications();
        }
        [Test]
        public void ListCollectedWhileItemInsideIsReferenced() {
            TestData item = new TestData();
            CreateListWithItem(item);
            TestUtils.GarbageCollect();
            Assert.AreEqual(1, item.GetPropertyChangedInvocationCount());
            item.Number++;
            Assert.AreEqual(0, item.GetPropertyChangedInvocationCount());
        }
        void CreateListWithItem(TestData item) {
            ObservableCollection<TestData> innerList = new ObservableCollection<TestData>();
            BindingListAdapter list = BindingListAdapter.CreateFromList(innerList);
            innerList.Add(item);
        }
        [Test]
        public void CollectionChangedWeakEventSubscribtion1() {
            WeakReference collectionReference = DoCollectionChangedWeakEventSubscribtion1();
            TestUtils.GarbageCollect();
            Assert.IsNull(collectionReference.Target);
        }
        WeakReference DoCollectionChangedWeakEventSubscribtion1() {
            ObservableCollection<CarData> collection = new ObservableCollection<CarData>();
            BindingListAdapter adapter = new BindingListAdapter(collection);
            return new WeakReference(collection);
        }
        [Test]
        public void CollectionChangedWeakEventSubscribtion2() {
            ObservableCollection<CarData> collection = new ObservableCollection<CarData>();
            WeakReference adapterReference = DoCollectionChangedWeakEventSubscribtion2(collection);
            TestUtils.GarbageCollect();
            Assert.IsNull(adapterReference.Target);
        }
        WeakReference DoCollectionChangedWeakEventSubscribtion2(ObservableCollection<CarData> collection) {
            BindingListAdapter adapter = new BindingListAdapter(collection);
            return new WeakReference(adapter);
        }
        [Test]
        public void NoNotificationFromRemovedItem() {
            StartListening();
            TestData fromPatternBindingList = patternBindingList[0];
            TestData fromCollection = collection[0];
            patternBindingList.RemoveAt(0);
            collection.RemoveAt(0);
            fromPatternBindingList.Number = 117;
            fromCollection.Number = 117;
            AssertNotifications();
            patternBindingList[1].Number = 117;
            collection[1].Number = 117;
            AssertNotifications();
        }
        [Test]
        public void NotificationFromAddedItem() {
            StartListening();
            TestData fromPatternBindingList = new TestData();
            TestData fromCollection = new TestData();
            patternBindingList.Add(fromPatternBindingList);
            collection.Add(fromCollection);
            fromPatternBindingList.Number = 117;
            fromCollection.Number = 117;
            AssertNotifications();
            patternBindingList[0].Number = 117;
            collection[0].Number = 117;
            AssertNotifications();
        }
        [Test]
        public void NotificationFromReplacedItem() {
            StartListening();
            TestData fromPatternBindingList = new TestData();
            TestData fromCollection = new TestData();
            TestData fromPatternBindingListOld = patternBindingList[0];
            TestData fromCollectionOld = collection[0];
            collection[0] = fromCollection;
            patternBindingList[0] = fromPatternBindingList;
            AssertNotifications();

            fromPatternBindingListOld.Number = 117;
            fromCollectionOld.Number = 117;
            AssertNotifications();

            fromPatternBindingList.Number = 117;
            fromCollection.Number = 117;
            AssertNotifications();
        }
        [Test]
        public void NoNotificationsAfterReset() {
            StartListening();
            TestData fromPatternBindingList = patternBindingList[0];
            TestData fromCollection = collection[0];
            patternBindingList.Clear();
            collection.Clear();
            fromPatternBindingList.Number = 117;
            fromCollection.Number = 117;
            AssertNotifications();

            int oldObjectPropertyChangedFireCount = adapter.ObjectPropertyChangedFireCount;
            fromPatternBindingList.Number = 118;
            fromCollection.Number = 118;
            AssertNotifications();
            Assert.AreEqual(oldObjectPropertyChangedFireCount, adapter.ObjectPropertyChangedFireCount);
        }
        [Test]
        public void PropertyChangedWeakEventSubscribtion1() {
            ObservableCollection<TestData> collection = new ObservableCollection<TestData>();
            BindingListAdapter adapter = new BindingListAdapter(collection);
            collection.Add(new TestData());
            WeakReference itemReference = new WeakReference(collection[0]);
            collection.Clear();
            TestUtils.GarbageCollect();
            Assert.IsNull(itemReference.Target);
        }
        [Test]
        public void PropertyChangedWeakEventSubscribtion2() {
            ObservableCollection<TestData> collection = new ObservableCollection<TestData>();
            collection.Add(new TestData());
            WeakReference adapterReference = DoPropertyChangedWeakEventSubscribtion2(collection);
            TestUtils.GarbageCollect();
            Assert.IsNull(adapterReference.Target);
        }
        WeakReference DoPropertyChangedWeakEventSubscribtion2(ObservableCollection<TestData> collection) {
            BindingListAdapter adapter = new BindingListAdapter(collection);
            return new WeakReference(adapter);
        }
        [Test]
        public void ResetIfCollectionIsNotEmpty() {
            ManuallyNotifiedCollection collection = new ManuallyNotifiedCollection();
            collection.Add(new TestData());
            collection.Add(new TestData());
            collection.Add(new TestData());
            BindingListAdapter adapter = new BindingListAdapter(collection);
            collection.NotifyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            int listChangedFireCount = 0;
            adapter.ListChanged += delegate(object sender, ListChangedEventArgs e) {
                listChangedFireCount++;
            };
            int oldObjectPropertyChangedFireCount = adapter.ObjectPropertyChangedFireCount;
            collection[0].Number = 117;
            Assert.AreEqual(oldObjectPropertyChangedFireCount + 1, adapter.ObjectPropertyChangedFireCount);
            Assert.AreEqual(1, listChangedFireCount);
        }
#if !SL
        [Test]
        public void AddRangeNotification() {
            ManuallyNotifiedCollection collection = new ManuallyNotifiedCollection();
            collection.Add(new TestData());
            BindingListAdapter adapter = new BindingListAdapter(collection);
            collection.Add(new TestData());
            collection.Add(new TestData());
            patternBindingList.RemoveAt(1);
            patternBindingList.RemoveAt(1);
            StartListening();
            patternBindingList.Add(new TestData());
            patternBindingList.Add(new TestData());
            adapter.ListChanged += delegate(object sender, ListChangedEventArgs e) {
                adapterNotifications.Add(e);
            };
            collection.NotifyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new TestData[] { collection[1], collection[2] }, 1));
            AssertNotifications();
            Assert.AreEqual(2, adapterNotifications.Count);
            Assert.AreEqual(1, adapterNotifications[0].NewIndex);
            Assert.AreEqual(2, adapterNotifications[1].NewIndex);
            Assert.AreEqual(-1, adapterNotifications[0].OldIndex);
            Assert.AreEqual(-1, adapterNotifications[1].OldIndex);
            Assert.AreEqual(ListChangedType.ItemAdded, adapterNotifications[0].ListChangedType);
            Assert.AreEqual(ListChangedType.ItemAdded, adapterNotifications[1].ListChangedType);
        }
        [Test]
        public void ReplaceRangeNotification() {
            ManuallyNotifiedCollection collection = new ManuallyNotifiedCollection();
            collection.Add(new TestData());
            collection.Add(new TestData());
            collection.Add(new TestData());
            BindingListAdapter adapter = new BindingListAdapter(collection);
            StartListening();
            adapter.ListChanged += delegate(object sender, ListChangedEventArgs e) {
                adapterNotifications.Add(e);
            };

            patternBindingList[1] = new TestData();
            patternBindingList[2] = new TestData();
            TestData[] oldItems = new TestData[] { collection[1], collection[2] };
            collection[1] = new TestData();
            collection[2] = new TestData();
            collection.NotifyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new TestData[] { collection[1], collection[2] }, oldItems, 1));
            AssertNotifications();
            Assert.AreEqual(2, adapterNotifications.Count);
            Assert.AreEqual(1, adapterNotifications[0].NewIndex);
            Assert.AreEqual(2, adapterNotifications[1].NewIndex);
            Assert.AreEqual(-1, adapterNotifications[0].OldIndex);
            Assert.AreEqual(-1, adapterNotifications[1].OldIndex);
            Assert.AreEqual(ListChangedType.ItemChanged, adapterNotifications[0].ListChangedType);
            Assert.AreEqual(ListChangedType.ItemChanged, adapterNotifications[1].ListChangedType);
        }
        [Test]
        public void RemoveRangeNotification() {
            ManuallyNotifiedCollection collection = new ManuallyNotifiedCollection();
            collection.Add(new TestData());
            collection.Add(new TestData());
            collection.Add(new TestData());
            BindingListAdapter adapter = new BindingListAdapter(collection);
            List<ListChangedEventArgs> adapterNotifications = new List<ListChangedEventArgs>();
            adapter.ListChanged += delegate(object sender, ListChangedEventArgs e) {
                adapterNotifications.Add(e);
            };
            collection.NotifyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new TestData[] { collection[1], collection[2] }, 1));
            Assert.AreEqual(2, adapterNotifications.Count);
        }
        [Test]
        public void MoveItemNotification() {
            ManuallyNotifiedCollection collection = new ManuallyNotifiedCollection();
            collection.Add(new TestData());
            collection.Add(new TestData());
            collection.Add(new TestData());
            BindingListAdapter adapter = new BindingListAdapter(collection);
            List<ListChangedEventArgs> adapterNotifications = new List<ListChangedEventArgs>();
            adapter.ListChanged += delegate(object sender, ListChangedEventArgs e) {
                adapterNotifications.Add(e);
            };
            TestData movedItem = collection[0];
            collection.Remove(movedItem);
            collection.Add(movedItem);
            collection.NotifyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, movedItem, 2, 0));
            Assert.AreEqual(1, adapterNotifications.Count);
            Assert.AreEqual(2, adapterNotifications[0].NewIndex);
            Assert.AreEqual(0, adapterNotifications[0].OldIndex);
            Assert.AreEqual(ListChangedType.ItemMoved, adapterNotifications[0].ListChangedType);
        }
        [Test]
        public void MoveItemNotification2() {
            ObservableCollection<TestData> collection = new ObservableCollection<TestData>();
            collection.Add(new TestData());
            collection.Add(new TestData());
            collection.Add(new TestData());
            BindingListAdapter adapter = new BindingListAdapter(collection);
            List<ListChangedEventArgs> adapterNotifications = new List<ListChangedEventArgs>();
            adapter.ListChanged += delegate(object sender, ListChangedEventArgs e) {
                adapterNotifications.Add(e);
            };
            collection.Move(0, 2);
            Assert.AreEqual(1, adapterNotifications.Count);
            Assert.AreEqual(2, adapterNotifications[0].NewIndex);
            Assert.AreEqual(0, adapterNotifications[0].OldIndex);
            Assert.AreEqual(ListChangedType.ItemMoved, adapterNotifications[0].ListChangedType);
        }
        [Test]
        public void MoveRangeNotification() {
            ManuallyNotifiedCollection collection = new ManuallyNotifiedCollection();
            collection.Add(new TestData());
            collection.Add(new TestData());
            collection.Add(new TestData());
            BindingListAdapter adapter = new BindingListAdapter(collection);
            List<ListChangedEventArgs> adapterNotifications = new List<ListChangedEventArgs>();
            adapter.ListChanged += delegate(object sender, ListChangedEventArgs e) {
                adapterNotifications.Add(e);
            };
            TestData[] movedItems = new TestData[] { collection[0], collection[1] };
            collection.NotifyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, movedItems, 1, 0));
            Assert.AreEqual(1, adapterNotifications.Count);
            Assert.AreEqual(ListChangedType.Reset, adapterNotifications[0].ListChangedType);
        }
#endif

        [Test]
        public void NotifcationsFromNotObservableCollection() {
            List<TestData> list = new List<TestData>();
            list.Add(new TestData());
            BindingListAdapter adapter = new BindingListAdapter(list);
            List<ListChangedEventArgs> adapterNotifications = new List<ListChangedEventArgs>();
            adapter.ListChanged += delegate(object sender, ListChangedEventArgs e) {
                adapterNotifications.Add(e);
            };
            list[0].Number = 117;
            Assert.AreEqual(1, adapterNotifications.Count);
            Assert.AreEqual(ListChangedType.ItemChanged, adapterNotifications[0].ListChangedType);
        }
        [Test, ExpectedException(typeof(ArgumentException))]
        public void BindingListAdapterTest_IncorrectList() {
            new TypedListBindingListAdapter(new List<object>());
        }
        public class TypedListObservableCollection : ObservableCollection<TestData>, ITypedList {
            public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors) {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Size));
                PropertyDescriptorCollection result = new PropertyDescriptorCollection(new PropertyDescriptor[] { properties["Height"] });
                return result;
            }
            public string GetListName(PropertyDescriptor[] listAccessors) {
                return "list name";
            }
        }
        [Test]
        public void BindingListAdapterTest_ITypedListImplemetation() {
            ITypedList adapter = new TypedListBindingListAdapter(new TypedListObservableCollection());
            Assert.AreEqual("list name", adapter.GetListName(null));
            Assert.IsNull(adapter.GetItemProperties(null)["Width"]);
            Assert.IsNotNull(adapter.GetItemProperties(null)["Height"]);
        }
        [Test]
        public void FindingElementWhenPropertyChanged() {
            List<TestData> list = new List<TestData>();
            list.Add(new TestData());
            list.Add(new TestData());
            BindingListAdapter adapter = new BindingListAdapter(list);
            List<ListChangedEventArgs> adapterNotifications = new List<ListChangedEventArgs>();
            adapter.ListChanged += delegate(object sender, ListChangedEventArgs e) {
                adapterNotifications.Add(e);
            };
            list[0].Number++;
            Assert.AreEqual(1, adapter.IndexOfCallCount);
            Assert.AreEqual(0, adapterNotifications[0].NewIndex);
            list[0].Number++;
            Assert.AreEqual(0, adapterNotifications[1].NewIndex);
            list[0].Text = "text";
            Assert.AreEqual(0, adapterNotifications[2].NewIndex);
            Assert.AreEqual(1, adapter.IndexOfCallCount);

            list[1].Number++;
            Assert.AreEqual(2, adapter.IndexOfCallCount);
            Assert.AreEqual(1, adapterNotifications[3].NewIndex);

            list.RemoveAt(1);

            adapterNotifications.Clear();
            list[0].Number++;
            Assert.AreEqual(3, adapter.IndexOfCallCount);
            Assert.AreEqual(0, adapterNotifications[0].NewIndex);

        }
        #region Q382800
        public class SomeCollection<T> : Collection<T>, INotifyCollectionChanged {
            public void AddItem(T item) {
                Items.Add(item);
                RaiseEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, -1));
            }
            public new void RemoveAt(int index) {
                T item = this[index];
                Items.RemoveAt(index);
#if !SL
                RaiseEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
#else
                RaiseEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, -1));
#endif
            }
            public void Replace(T item, int index) {
                T oldItem = Items[index];
                Items[index] = item;
#if !SL
                RaiseEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem));
#else
                RaiseEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, -1));
#endif
            }
#if !SL
            public void MoveToEnd(int oldIndex) {
                T movedItem = Items[oldIndex];
                Items.RemoveAt(oldIndex);
                Items.Add(movedItem);
                RaiseEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, movedItem, Items.Count - 1, -1));

            }
#endif
            public void RaiseEvent(NotifyCollectionChangedEventArgs e) {
                if(CollectionChanged != null)
                    CollectionChanged(this, e);
            }
            public event NotifyCollectionChangedEventHandler CollectionChanged;
        }
        [Test]
        public void Q382800_AddAndReplaceWithoutStartIndex() {
            SomeCollection<TestData> collection = new SomeCollection<TestData>();
            BindingListAdapter adapter = new BindingListAdapter(collection);
            List<ListChangedEventArgs> adapterNotifications = new List<ListChangedEventArgs>();
            adapter.ListChanged += delegate(object sender, ListChangedEventArgs e) {
                adapterNotifications.Add(e);
            };
            collection.AddItem(new TestData());
            Assert.AreEqual(1, adapterNotifications.Count);
            Assert.AreEqual(0, adapterNotifications[0].NewIndex);
            collection.AddItem(new TestData());
            Assert.AreEqual(2, adapterNotifications.Count);
            Assert.AreEqual(1, adapterNotifications[1].NewIndex);

            collection.Replace(new TestData(), 1);
            Assert.AreEqual(3, adapterNotifications.Count);
            Assert.AreEqual(1, adapterNotifications[2].NewIndex);
        }
#if !SL
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Q382800_Move() {
            SomeCollection<TestData> collection = new SomeCollection<TestData>();
            BindingListAdapter adapter = new BindingListAdapter(collection);
            List<ListChangedEventArgs> adapterNotifications = new List<ListChangedEventArgs>();
            adapter.ListChanged += delegate(object sender, ListChangedEventArgs e) {
                adapterNotifications.Add(e);
            };
            collection.AddItem(new TestData());
            collection.AddItem(new TestData());

            collection.MoveToEnd(0);
            Assert.AreEqual(4, adapterNotifications.Count);
            Assert.AreEqual(1, adapterNotifications[3].NewIndex);
            Assert.AreEqual(0, adapterNotifications[3].OldIndex);
        }
#endif
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Q382800_RaiseAddWithInvalidItem() {
            SomeCollection<TestData> collection = new SomeCollection<TestData>();
            BindingListAdapter adapter = new BindingListAdapter(collection);
            collection.RaiseEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new TestData(), -1));
        }
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Q382800_RemoveWithoutStartIndex() {
            SomeCollection<TestData> collection = new SomeCollection<TestData>();
            collection.Add(new TestData());
            BindingListAdapter adapter = new BindingListAdapter(collection);
            collection.RemoveAt(0);
        }
        #endregion
    }
}
