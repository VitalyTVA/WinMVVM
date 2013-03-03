using System;
using System.Collections.Generic;
using System.Linq;

namespace WinMVVM.Utils {
    static class Guard {
        public static void ArgumentNotNull(object obj, string argumentName) {
            if(obj == null)
                throw new ArgumentNullException(argumentName);
        }
        public static void ArgumentInRange(bool condition, string argumentName) {
            if(!condition)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        public static void ArgumentException(string argumentName) {
            throw new ArgumentException(argumentName);
        }

        public static void InvalidOperation() {
            throw new InvalidOperationException();
        }
    }
}
