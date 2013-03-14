using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    class AttachedPropertyDescriptor : PropertyDescriptorBase {
        public static PropertyDescriptorBase FromAttachedProperty(AttachedPropertyBase property) {
            return new AttachedPropertyDescriptor(property);
        }
        private readonly AttachedPropertyBase property;
        internal AttachedPropertyBase Property { get { return property; } }
        AttachedPropertyDescriptor(AttachedPropertyBase property)
            : base(property.Name, property.PropertyType) {
                this.property = property;
            
        }
        internal override object GetValue(Control control) {
            return property.GetValueInternal(control);
        }

        //public override Type PropertyType {
        //    get { return typeof(T); } //TODO
        //}

        internal override void SetValue(Control control, object value) {
            property.SetValueInternal(control, value);
        }

        internal override void ResetValue(Control control) {
            property.ClearValue(control);
        }

        public override int GetHashCode() {
            return property.GetHashCode();
        }
        public override bool Equals(object obj) {
            var other = obj as AttachedPropertyDescriptor;
            return other != null && other.property == property;
        }

        //class Subscriber {
        //    readonly EventHandler handler;
        //    public Subscriber(EventHandler handler) {
        //        this.handler = handler;
        //    }
        //    public override bool Equals(object obj) {
        //        var other = obj as Subscriber;
        //        return other != null && other.handler == handler;
        //    }
        //    public override int GetHashCode() {
        //        return base.GetHashCode();
        //    }
        //    public void OnEvent(object sender, PropertyChangedEventArgs e) {
        //        handler(sender, EventArgs.Empty);
        //    }
        //}

        internal override void AddValueChanged(Control control, EventHandler handler) {
            INotifyPropertyChanged propertyChanged = property.GetTrackableEntry(control);
            //propertyChanged.PropertyChanged += new Subscriber(handler).OnEvent;
            propertyChanged.PropertyChanged += (sender, e) => {
                handler(sender, e);
            };
        }


        internal override void RemoveValueChanged(Control control, EventHandler handler) {
            INotifyPropertyChanged propertyChanged = property.GetTrackableEntry(control);
            //propertyChanged.PropertyChanged -= new Subscriber(handler).OnEvent;
            //propertyChanged.PropertyChanged -= (sender, e) => {
            //    handler(sender, e);
            //};
        }
    }
}
