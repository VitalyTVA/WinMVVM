using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WinMVVM.Tests.ItemsSource;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System.Collections;

namespace WinMVVM.DevExpress.Tests {
    [TestFixture]
    public class XtraGridItemsSourceFeatureTests : ItemsSourceTestsBase<GridControl> {
        protected override IList GetItems(GridControl control) {
            return GetItemsCore(control).ToList();
        }
        IEnumerable<object> GetItemsCore(GridControl control) {
            GridView view = (GridView)control.MainView;
            for(int i = 0; i < view.DataRowCount; i++) {
                yield return view.GetRow(i);
            }
        }
        protected override object GetDataSource(GridControl control) {
            return control.DataSource;
        }
        protected override string[] GetItemsSourceDependentProperties() {
            return new string[] { GetPropertyName(() => new GridControl().DataSource) };
        }
        protected override GridControl CreateControl() {
            var gridControl = new GridControl();
            gridControl.MainView = new GridView();
            return gridControl;
        }
        protected override int GetSelectedIndex(GridControl control) {
            return ((GridView)control.MainView).FocusedRowHandle;
        }
        protected override void SetSelectedIndex(GridControl control, int value) {
            ((GridView)control.MainView).FocusedRowHandle = value;
        }

    }
}
