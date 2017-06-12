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
using KsiazkaKucharska.ViewModel;

namespace KsiazkaKucharska
{
    /// <summary>
    /// Interaction logic for DodajKategorie.xaml
    /// </summary>
    public partial class DodajKategorie : Window
    {
        public DodajKategorie(ZarzadzanieKategoriamiViewModel parentVM, string nazwaKategorii)
        {
            InitializeComponent();
            DodajKategorieViewModel vm = new DodajKategorieViewModel(parentVM, nazwaKategorii);
            DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = new Action(() => this.Close());
        }
    }
}
