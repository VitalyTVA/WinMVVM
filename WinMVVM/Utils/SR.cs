using System;
using System.Collections.Generic;
using System.Linq;

namespace WinMVVM.Utils {
    static class SR {
        public const string DesignAssemblyName = "WinMVVM.Design";
        public const string DesignAssemblyFullName = DesignAssemblyName + ", Version=1.0.0.0, Culture=neutral, PublicKeyToken=2566bbae264d9adc";
        public const string PublicKey = "0024000004800000940000000602000000240000525341310004000001000100a5da97130befe6b341c2c65a10064acbc25bdb5e17b0cc92c6cf9b947574d20ba7516c5353c297eb2d187a389658e9f58cd69fd28cbb473df0a0c175b767238174e95e1bc0cc8295fe40e5c746fad36854c0b80091fc3c36954d3cfdab2573070070f325f58c8ec8fcc452e8b6d3fe12ff2d73cc2bd2f084e285563ff2c1afc8";
        public const string DesignAssemblyNameWithPublicKey = DesignAssemblyName + ", PublicKey=" + PublicKey;

        public const string BindingManagerDesignerTypeName = DesignAssemblyName + ".BindingManagerDesigner";
        public const string BindingManagerDesignerAssemblyQualifiedName = BindingManagerDesignerTypeName + ", " + DesignAssemblyFullName;

        public const string BindingSerializerDesignerTypeName = DesignAssemblyName + ".BindingManagerCodeDomSerializer";
        public const string BindingManagerSerializerAssemblyQualifiedName = BindingSerializerDesignerTypeName + ", " + DesignAssemblyFullName;

        public const string CodeDomSerializerAssemblyQualifiedName = "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

        public const string TwoWayBindingRequiresPathExceptionMessage = "Two-way binding requires Path";
    }
}
