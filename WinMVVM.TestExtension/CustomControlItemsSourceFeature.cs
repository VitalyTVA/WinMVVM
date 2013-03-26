using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMVVM.Features;

namespace WinMVVM.TestExtension {
    class CustomControlItemsSourceFeature : BaseFeature<CustomControl>, IItemsSourceFeature {
        void IItemsSourceFeature.SetDataSource(System.Windows.Forms.Control control, object value) {
            throw new NotImplementedException();
        }

        string[] IItemsSourceFeature.GetItemsSourceAffectedProperties() {
            throw new NotImplementedException();
        }
    }
}
