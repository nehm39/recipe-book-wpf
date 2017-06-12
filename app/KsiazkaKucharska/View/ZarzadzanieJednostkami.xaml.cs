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
    /// Interaction logic for ZarzadzanieJednostkami.xaml
    /// </summary>
    public partial class ZarzadzanieJednostkami : Window
    {
        private MainWindowViewModel ParentVM;

        public ZarzadzanieJednostkami(MainWindowViewModel parentVM)
        {
            ParentVM = parentVM;
            InitializeComponent();
            ZarzadzanieJednostkamiViewModel vm = new ZarzadzanieJednostkamiViewModel(parentVM);
            DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = new Action(() => this.Close());
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            DodajJednostke j = new DodajJednostke((ZarzadzanieJednostkamiViewModel)DataContext, null);
            j.Owner = this;
            j.ShowDialog();
        }

        private void btnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            DodajJednostke j = new DodajJednostke((ZarzadzanieJednostkamiViewModel)DataContext, ((ZarzadzanieJednostkamiViewModel)DataContext).jednostkiSelection);
            j.Owner = this;
            j.ShowDialog();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ParentVM.wypiszPrzepisy(ParentVM.listaPrzepisowSelection.ID);
            ParentVM.wypiszSkladniki(ParentVM.listaSkladnikowSelection.ID);
        }

    }
}
