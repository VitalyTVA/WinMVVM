using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Utils;

namespace WinMVVM.Tests.ItemsSource {
    //[TestFixture]
    //public class DataGridViewItemsSourceTests : ItemsSourceTestsBase<DataGridView> {
    //    protected override IList GetItems(DataGridView control) {
    //        return GetItemsCore(control).ToList();
    //    }
    //    IEnumerable<object> GetItemsCore(DataGridView control) {
    //        for(int i = 0; i < control.RowCount; i++) {
    //            yield return control.Rows[i].DataBoundItem;
    //        }
    //    }

    //    protected override object GetDataSource(DataGridView control) {
    //        return control.DataSource;
    //    }

    //    protected override string[] GetItemsSourceDependentProperties() {
    //        return new string[] { GetPropertyName(() => new DataGridView().DataSource) };
    //    }

    //    protected override int GetSelectedIndex(DataGridView control) {
    //        return control.Rows.Cast<DataGridViewRow>().FirstOrDefault(x => x.Selected).Return(x => x.Index, () => 1);
    //    }

    //    protected override void SetSelectedIndex(DataGridView control, int value) {
    //        control.Rows[value].Selected = true;
    //    }

    //    protected override DataGridView CreateControl() {
    //        var control = base.CreateControl();
    //        return control;
    //    }
    //}
}
