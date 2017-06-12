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
    /// Interaction logic for DodajPrzepis.xaml
    /// </summary>
    public partial class DodajPrzepis : Window
    {
        public DodajPrzepis(MainWindowViewModel ParentVM, int? idPrzepisu)
        {
            InitializeComponent();
            DodajPrzepisViewModel vm = new DodajPrzepisViewModel(ParentVM, idPrzepisu);
            DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = new Action(() => this.Close());
        }

        private void btnPrzegladajZdjecie_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Pliki graficzne|*.jpeg;*.png;*.jpg";
            if (dialog.ShowDialog() == true)
            {
                var vm = (DodajPrzepisViewModel)DataContext;
                vm.ustawZdjecie(dialog.FileName);
            }
        }

        private void btnEdytujSkladnik_Click(object sender, RoutedEventArgs e)
        {
            EdytujSkladnikPrzepisu w = new EdytujSkladnikPrzepisu((DodajPrzepisViewModel)DataContext);
            w.Owner = this;
            w.ShowDialog();
        }

        private void btnZarzadzajKategoriami_Click(object sender, RoutedEventArgs e)
        {
            ZarzadzanieKategoriami w = new ZarzadzanieKategoriami(DataContext);
            w.Owner = this;
            w.ShowDialog();
        }
    }
}
