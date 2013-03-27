using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Features.ItemsSource;
using WinMVVM.Utils;
using WinMVVM.Utils.Adapter;

namespace WinMVVM.Features {
    public static class DefaultFeatureRegistrator {
        static bool registered;
        public static void RegisterDefaultFeatures() {
            if(registered)
                return;
            registered = true;
            FeatureProvider<IItemsSourceFeature>.RegisterFeature<ListBoxItemsSourceFeature>();
            FeatureProvider<IItemsSourceFeature>.RegisterFeature<ComboBoxItemsSourceFeature>();
            FeatureProvider<IItemsSourceFeature>.RegisterFeature<DataGridViewItemsSourceFeature>();

            LoadExtensions();
        }
        static void LoadExtensions() {
            string location = Assembly.GetExecutingAssembly().Location;
            if(string.IsNullOrWhiteSpace(location))
                return;
            string directory = Path.GetDirectoryName(location);
            if(string.IsNullOrWhiteSpace(directory))
                return;
            string[] files = Directory.GetFiles(directory, "WinMVVM.*.dll");
            foreach(string file in files) {
                if(!file.Contains("Design")) {
                    try {
                        var assembly = Assembly.LoadFrom(file);
                        var attribute = assembly.GetCustomAttribute(typeof(FeatureRegistratorAttribute)) as FeatureRegistratorAttribute;
                        if(attribute != null) {
                            assembly = Assembly.LoadFrom(file);
                            var featureRegistrator = Activator.CreateInstance(attribute.FeatureRegistratorType) as IFeatureRegistrator;
                            featureRegistrator.RegisterFeatures();
                        }
                    } catch { }
                }
            }
        }
    }
}
