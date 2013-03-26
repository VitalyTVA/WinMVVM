using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinMVVM.Features;

namespace WinMVVM.DevExpress {
    class XtraGridItemsSourceFeature : BaseFeature<GridControl>, IItemsSourceFeature {
        void IItemsSourceFeature.SetDataSource(Control control, object value) {
            ((GridControl)control).DataSource = value;
        }
        string[] IItemsSourceFeature.GetItemsSourceAffectedProperties() {
            return new string[0];
        }
    }
}
