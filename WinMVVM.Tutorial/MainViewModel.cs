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

        public List<Customer> Customers { get { return Customer.Customers; } }

        public RelayCommand ShowMessageCommand { get; private set; }

        public MainViewModel() {
            ShowMessageCommand = new RelayCommand(() => MessageBox.Show("Test"));
        }
    }
    public class Customer {
        public static List<Customer> Customers = new List<Customer>() {
            new Customer() { FirstName = "Nancy", LastName = "Davoio" },
            new Customer() { FirstName = "Andrew", LastName = "Fuller" },
            new Customer() { FirstName = "Ann", LastName = "Dodsworth" },
        };
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
