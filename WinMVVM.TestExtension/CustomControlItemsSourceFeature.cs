using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMVVM.Features;

namespace WinMVVM.TestExtension {
    class CustomControlItemsSourceFeature : BaseItemsSourceFeature<CustomControl>, IItemsSourceFeature {
        protected override void SetSelectedItem(CustomControl tControl, object value) {
            throw new NotImplementedException();
        }

        protected override string[] GetItemsSourceAffectedProperties() {
            throw new NotImplementedException();
        }

        protected override void SetDataSource(CustomControl control, object value) {
            throw new NotImplementedException();
        }
        protected override object GetSelectedItem(CustomControl control) {
            throw new NotImplementedException();
        }
    }
}
