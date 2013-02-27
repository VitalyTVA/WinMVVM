using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinMVVM.Utils {
    public static class Guard {
        public static void ArgumentNotNull(object obj, string paramName) {
            if(obj == null)
                throw new ArgumentNullException();
        }
    }
}
