using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMVVM.Features;

namespace WinMVVM.TestExtension.Properties {
    public class FeatureRegistrator : IFeatureRegistrator {
        void IFeatureRegistrator.RegisterFeatures() {
            FeatureProvider<IItemsSourceFeature>.RegisterFeature<CustomControlItemsSourceFeature>();
        }
    }
}
