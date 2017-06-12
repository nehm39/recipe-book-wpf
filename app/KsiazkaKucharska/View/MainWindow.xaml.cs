using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using KsiazkaKucharska.ViewModel;
using KsiazkaKucharska.Model;

namespace KsiazkaKucharska
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void menuZamknij_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuInformacje_Click(object sender, RoutedEventArgs e)
        {
            Informacje i = new Informacje();
            i.Owner = this;
            i.ShowDialog();
        }


        private void menuJednostki_Click(object sender, RoutedEventArgs e)
        {
            ZarzadzanieJednostkami z = new ZarzadzanieJednostkami((MainWindowViewModel)DataContext);
            z.Owner = this;
            z.ShowDialog();
        }

        private void btnDodajPrzepis_Click(object sender, RoutedEventArgs e)
        {
            DodajPrzepis w = new DodajPrzepis(((MainWindowViewModel)DataContext), null);
            w.Owner = this;
            w.ShowDialog();
        }

        private void btnDodajSkladnik_Click(object sender, RoutedEventArgs e)
        {
            DodajSkladnik w = new DodajSkladnik(((MainWindowViewModel)DataContext), null);
            w.Owner = this;
            w.ShowDialog();
        }

        private void btnEdytujSkladnik_Click(object sender, RoutedEventArgs e)
        {
            int id = ((MainWindowViewModel)DataContext).listaSkladnikowSelection.ID;
            DodajSkladnik w = new DodajSkladnik(((MainWindowViewModel)DataContext), id);
            w.Owner = this;
            w.ShowDialog();
        }

        private void btnEdytujPrzepis_Click(object sender, RoutedEventArgs e)
        {
            int id = ((MainWindowViewModel)DataContext).listaPrzepisowSelection.ID;
            DodajPrzepis w = new DodajPrzepis(((MainWindowViewModel)DataContext), id);
            w.Owner = this;
            w.ShowDialog();
        }

        private void menuKategorie_Click(object sender, RoutedEventArgs e)
        {
            ZarzadzanieKategoriami w = new ZarzadzanieKategoriami(DataContext);
            w.Owner = this;
            w.ShowDialog();
        }

        



     



        

        
    }
}
