using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KsiazkaKucharska.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace KsiazkaKucharska.ViewModel
{
    public class ZarzadzanieKategoriamiViewModel : INotifyPropertyChanged
    {
        public ZarzadzanieKategoriamiViewModel(object parentVM)
        {
            if (parentVM.GetType() == typeof(DodajPrzepisViewModel)) wypiszRodzajeKategorii(1);
            else wypiszRodzajeKategorii(0);
            ParentVM = parentVM;
        }

        private void wypiszRodzajeKategorii(int selectIndex)
        {
            rodzajeKategorii.Add("Kategorie składników");
            rodzajeKategorii.Add("Kategorie przepisów");
            rodzajeKategoriiSelection = rodzajeKategorii.ElementAt(selectIndex);
        }

        public void wypiszKategorie(int index)
        {
            EntityMethods en = new EntityMethods();
            kategorie = new ObservableCollection<string>();
            if (index == 0)
            {
                foreach (Kategorie_Skladniki ks in en.listaKategoriiSkladnikow())
                {
                    kategorie.Add(ks.S_NAZWA);
                }
                if (kategorie.Count > 0) kategorieSelection = kategorie.ElementAt(0);
            }
            else
            {
                foreach (Kategorie_Przepisy kp in en.listaKategoriiPrzepisow())
                {
                    kategorie.Add(kp.P_NAZWA);
                }
                if (kategorie.Count > 0) kategorieSelection = kategorie.ElementAt(0);
            }
        }

        public object ParentVM { get; set; }
        public Action CloseAction { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<string> _rodzajeKategorii = new ObservableCollection<string>();
        private string _rodzajeKategoriiSelection;
        private ObservableCollection<string> _kategorie = new ObservableCollection<string>();
        private string _kategorieSelection;
        private ICommand _usun;

        public ObservableCollection<string> rodzajeKategorii
        {
            get { return _rodzajeKategorii; }

            set
            {
                _rodzajeKategorii = value;
                OnPropertyChanged("rodzajeKategorii");

            }
        }

        public string rodzajeKategoriiSelection
        {
            get { return _rodzajeKategoriiSelection; }
            set
            {
                _rodzajeKategoriiSelection = value;
                OnPropertyChanged("rodzajeKategoriiSelection");

                int index = rodzajeKategorii.IndexOf(rodzajeKategoriiSelection);
                wypiszKategorie(index);
            }
        }

        public ObservableCollection<string> kategorie
        {
            get { return _kategorie; }

            set
            {
                _kategorie = value;
                OnPropertyChanged("kategorie");
            }
        }

        public string kategorieSelection
        {
            get { return _kategorieSelection; }
            set
            {
                _kategorieSelection = value;
                OnPropertyChanged("kategorieSelection");
            }
        }


        public ICommand usun
        {
            get
            {
                return _usun ?? (_usun = new RelayCommand(() => usunAkcja(), null));
            }
        }

        private void usunAkcja()
        {
            EntityMethods en = new EntityMethods();
            int index = rodzajeKategorii.IndexOf(rodzajeKategoriiSelection);
            int liczba;
            if (index == 0) liczba = en.liczbaPowiazanKategoriiSkladnika(kategorieSelection);
            else liczba = en.liczbaPowiazanKategoriiSkladnika(kategorieSelection);
            if (liczba > 0)
            {
                MessageBoxShow mb = new MessageBoxShow();
                mb.pokazMessageBox("Informacja", "Nie możesz usunąć kategorii, która jest używana. Najpierw musisz usunąć wszelkie powiązania.", MessageBoxTyp.Normalny, MessageBoxIkona.Informacyjna);
            }
            else
            {
                if (index == 0) en.usunKategorieSkladnikow(kategorieSelection);
                else en.usunKategoriePrzepisow(kategorieSelection);
                wypiszKategorie(index);
            }

        }
    }
}
