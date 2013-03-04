using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinMVVM.Tutorial {
    public class MainViewModel : ViewModelBase {

        string messageText;

        public string MessageText {
            get { return messageText; }
            set { Set(() => MessageText, ref messageText, value); }
        }

        public RelayCommand ShowMessageCommand { get; private set; }

        public MainViewModel() {
            ShowMessageCommand = new RelayCommand(() => MessageBox.Show("Test"));
        }
    }
}
