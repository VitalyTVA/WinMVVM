using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinMVVM.Tutorial {
    public class MainViewModel : ViewModelBase {

        string messageText;

        public string MessageText {
            get { return messageText; }
            set { Set(() => MessageText, ref messageText, value); }
        }
        
    }
}
