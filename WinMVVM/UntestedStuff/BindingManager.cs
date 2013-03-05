using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public class BindingManager : Component, ISupportInitialize {
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)
        ]
        public SetBindingActionCollection Collection { get; private set; }

        public BindingManager() {
            Collection = new SetBindingActionCollection(this);
        }
        void ISupportInitialize.BeginInit() {
        }
        void ISupportInitialize.EndInit() {
        }
    }
}
