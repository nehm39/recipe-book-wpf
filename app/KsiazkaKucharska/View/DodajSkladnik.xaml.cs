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
using Microsoft.Win32;

namespace KsiazkaKucharska
{
    /// <summary>
    /// Interaction logic for DodajSkladnik.xaml
    /// </summary>
    public partial class DodajSkladnik : Window
    {
        public DodajSkladnik(MainWindowViewModel ParentVM, int? idSkladnika)
        {
            InitializeComponent();
            DodajSkladnikViewModel vm = new DodajSkladnikViewModel(ParentVM, idSkladnika);
            DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = new Action(() => this.Close());
        }

        private void btnPrzegladajZdjecie_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Pliki graficzne|*.jpeg;*.png;*.jpg";
            if (dialog.ShowDialog() == true)
            {
                    var vm = (DodajSkladnikViewModel) DataContext;
                    vm.ustawZdjecie(dialog.FileName);
            }
        }

        private void btnZarzadzajKategoriami_Click(object sender, RoutedEventArgs e)
        {
            ZarzadzanieKategoriami w = new ZarzadzanieKategoriami(DataContext);
            w.Owner = this;
            w.ShowDialog();
        }
    }
}
