using System;
using System.Collections.Generic;
using System.Linq;

namespace WinMVVM.Features {
    [AttributeUsage(AttributeTargets.Assembly)]
    public class FeatureRegistratorAttribute : Attribute {
        public FeatureRegistratorAttribute(Type featureRegistratorType) {
            FeatureRegistratorType = featureRegistratorType;
        }
        public Type FeatureRegistratorType { get; private set; }
    }
}
