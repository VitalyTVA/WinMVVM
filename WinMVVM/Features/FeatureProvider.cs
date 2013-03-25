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
    public static class FeatureProvider<TFeature> where TFeature : class, IFeature {
        class TypeFeaturePair {
            private TFeature feature;
            Type type;
            public TFeature Feature {
                get {
                    if(feature == null)
                        feature = (TFeature)Activator.CreateInstance(Type);
                    return feature;
                }
            }
            public Type Type {
                get { return type; }
            }
            public TypeFeaturePair(Type type) {
                this.type = type;
            }
        }
        static FeatureProvider() {
            DefaultFeatureRegistrator.RegisterDefaultFeatures();
        }
        static readonly List<TypeFeaturePair> Features = new List<TypeFeaturePair>();
        public static TFeature GetFeature(Control control) {
            return Features.FirstOrDefault(x => x.Feature.IsSupportedControl(control)).With(x => x.Feature);
        }
#if DEBUG
        private static bool throwOnDuplicateRegistration = true;
        public static bool ThrowOnDuplicateRegistration {
            get { return throwOnDuplicateRegistration; }
            set { throwOnDuplicateRegistration = value; }
        }
        
#endif
        public static void RegisterFeature<TImplementor>() where TImplementor : TFeature, new() {
            if(!Features.Any(x => x.Type == typeof(TImplementor))) {
                Features.Add(new TypeFeaturePair(typeof(TImplementor)));
            } else { 
#if DEBUG
                if(ThrowOnDuplicateRegistration)
                    Guard.InvalidOperation();
#endif
            }
        }
    }
}
