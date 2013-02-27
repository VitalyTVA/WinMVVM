using System;
using System.Collections.Generic;
using System.Linq;

namespace WinMVVM.Utils {
    public static class Guard {
        public static void ArgumentNotNull(object obj, string paramName) {
            if(obj == null)
                throw new ArgumentNullException(paramName);
        }
    }
}
