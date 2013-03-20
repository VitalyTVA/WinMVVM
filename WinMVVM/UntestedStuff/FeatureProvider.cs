using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Features.ItemsSource;
using WinMVVM.Utils;
using WinMVVM.Utils.Adapter;

namespace WinMVVM.Features {
    //TODO it is key to performance to optimize this class - track last suitable features, cache ever used features, use binary tree cashes, etc.
    //search in cash by control type first, then full search by features list and cache type if feature is found to speed up access in the future
    public static class FeatureProvider<TFeature> where TFeature : IFeature {
        static FeatureProvider() {
            DefaultFeatureRegistrator.RegisterDefaultFeatures();
        }
        static readonly List<TFeature> Features = new List<TFeature>();
        public static TFeature GetFeature(Control control) {
            return Features.FirstOrDefault(x => x.IsSupportedControl(control));
        }
        public static void RegisterFeature<TImplementor>() where TImplementor : TFeature, new() {
            Features.Add(new TImplementor()); //TODO check unique, lazy creation
        }
    }
}
