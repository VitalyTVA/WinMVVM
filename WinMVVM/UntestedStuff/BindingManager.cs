using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    //[Designer(typeof(BindingManagerDesigner))]
    [Designer("WinMVVM.Design.BindingManagerDesigner, WinMVVM.Design, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2566bbae264d9adc")]
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