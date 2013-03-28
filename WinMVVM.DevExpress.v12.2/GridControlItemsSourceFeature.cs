using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinMVVM.Features;

namespace WinMVVM.DevExpress {
    class GridControlItemsSourceFeature : BaseItemsSourceFeature<GridControl>, IItemsSourceFeature {
        static readonly string[] AffectedProperties;
        static GridControlItemsSourceFeature() {
            AffectedProperties = new string[] { GetPropertyName(() => new GridControl().DataSource) };
        }
        protected override string[] GetItemsSourceAffectedProperties() {
            return AffectedProperties;
        }
        protected override void SetDataSource(GridControl control, object value) {
            control.DataSource = value;
        }

        protected override void SetSelectedItem(GridControl tControl, object value) {
            throw new NotImplementedException();
        }
        protected override object GetSelectedItem(GridControl control) {
            throw new NotImplementedException();
        }

    }
}
