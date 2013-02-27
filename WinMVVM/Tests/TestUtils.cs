using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinMVVM.Tests {
    static class TestUtils {
        public static void GarbageCollect() {
            for(int i = 0; i < 10; i++) {
                GC.GetTotalMemory(true);
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
