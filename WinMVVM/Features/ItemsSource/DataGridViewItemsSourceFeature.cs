using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinMVVM.Features.ItemsSource {
    internal class DataGridViewItemsSourceFeature : BaseItemsSourceFeature<DataGridView> {
        static readonly string[] AffectedProperties;
        static DataGridViewItemsSourceFeature() {
            AffectedProperties = new string[] { GetPropertyName(() => new DataGridView().DataSource) };
        }
        protected override string[] GetItemsSourceAffectedProperties() {
            return AffectedProperties;
        }
        protected override void SetDataSource(DataGridView control, object value) {
            control.DataSource = value;
        }
    }
}
