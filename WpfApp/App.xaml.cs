using System.Windows;
using BusinessObjects;

namespace WpfApp
{
    public partial class App : Application
    {
        public static Account? CurrentAccount { get; set; }
    }
}
