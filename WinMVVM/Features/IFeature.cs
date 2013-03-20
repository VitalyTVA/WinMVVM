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
    public interface IFeature {
        bool IsSupportedControl(Control control);
    }
    //TODO it is key to performance to optimize this class - track last suitable features, cache those ever used, etc.
    //TODO register features explicitly only from metadata provider to avoid static ctors initialization order problems
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
    public static class DefaultFeatureRegistrator {
        static bool registered;
        public static void RegisterDefaultFeatures() { 
            if(registered)
                return;
            registered = true;
            FeatureProvider<IItemsSourceFeature>.RegisterFeature<ListBoxItemsSourceFeature>();
            FeatureProvider<IItemsSourceFeature>.RegisterFeature<ComboBoxItemsSourceFeature>();
        }
    }
}
