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
    /// Interaction logic for ZarzadzanieKategoriami.xaml
    /// </summary>
    public partial class ZarzadzanieKategoriami : Window
    {
        private object ParentVM;

        public ZarzadzanieKategoriami(object parentVM)
        {
            InitializeComponent();
            ParentVM = parentVM;
            ZarzadzanieKategoriamiViewModel vm = new ZarzadzanieKategoriamiViewModel(parentVM);
            DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = new Action(() => this.Close());
        }

        private void btnDodajKategorie_Click(object sender, RoutedEventArgs e)
        {
            DodajKategorie w = new DodajKategorie((ZarzadzanieKategoriamiViewModel)DataContext, null);
            w.Owner = this;
            w.ShowDialog();
        }

        private void btnEdytujKategorie_Click(object sender, RoutedEventArgs e)
        {
            DodajKategorie w = new DodajKategorie((ZarzadzanieKategoriamiViewModel)DataContext, ((ZarzadzanieKategoriamiViewModel)DataContext).kategorieSelection);
            w.Owner = this;
            w.ShowDialog();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ParentVM.GetType() == typeof(DodajPrzepisViewModel))
            {
                ((DodajPrzepisViewModel)ParentVM).wypiszKategorie();
            }
            else if (ParentVM.GetType() == typeof(DodajSkladnikViewModel))
            {
                ((DodajSkladnikViewModel)ParentVM).wypiszKategorie();
            }
            else if (ParentVM.GetType() == typeof(MainWindowViewModel))
            {
                ((MainWindowViewModel)ParentVM).wypiszPrzepisy(((MainWindowViewModel)ParentVM).listaPrzepisowSelection.ID);
                ((MainWindowViewModel)ParentVM).wypiszSkladniki(((MainWindowViewModel)ParentVM).listaSkladnikowSelection.ID);
            }
        }
    }
}
