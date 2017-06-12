using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KsiazkaKucharska.Model;
using KsiazkaKucharska.ViewModel;

namespace KsiazkaKucharska
{
    /// <summary>
    /// Interaction logic for DodajJednostke.xaml
    /// </summary>
    public partial class DodajJednostke : Window
    {

        public DodajJednostke(ZarzadzanieJednostkamiViewModel parentVM, string nazwaJednostki)
        {
            InitializeComponent();
            DodajJednostkeViewModel vm = new DodajJednostkeViewModel(parentVM, nazwaJednostki);
            DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = new Action(() => this.Close());
        }
    }
}

