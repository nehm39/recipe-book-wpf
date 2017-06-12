using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KsiazkaKucharska.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;
using KsiazkaKucharska.ViewModel;

namespace KsiazkaKucharska.ViewModel
{
    public class ZarzadzanieJednostkamiViewModel : INotifyPropertyChanged
    {

        public ZarzadzanieJednostkamiViewModel(MainWindowViewModel parentVM)
        {
            ParentVM = parentVM;
            wypiszJednostki();
        }

        public void wypiszJednostki()
        {
            jednostki = new ObservableCollection<string>();
            EntityMethods en = new EntityMethods();
            foreach (Jednostki j in en.listaJednostek())
            {
                jednostki.Add(j.NAZWA);
            }
            if (jednostki.Count > 0) jednostkiSelection = jednostki.ElementAt(0);
        }

        private MainWindowViewModel ParentVM { get; set; }
        public Action CloseAction { get; set; }
        private ICommand _pokazMessageBox;
        public event PropertyChangedEventHandler PropertyChanged;

        public MessageBoxWynik pokazMessageBox(string tytul, string tekst)
        {
            MessageBoxShow mb = new MessageBoxShow();
            MessageBoxWynik w = new MessageBoxWynik();
            _pokazMessageBox = new RelayCommand(() => w = mb.pokazMessageBox(tytul, tekst, MessageBoxTyp.Normalny, MessageBoxIkona.Blad));
            _pokazMessageBox.Execute(null);
            return w;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<string> _jednostki;
        private string _jednostkiSelection;
        private ICommand _usunJednostke;

        public ObservableCollection<string> jednostki
        {
            get { return _jednostki; }

            set
            {
                _jednostki = value;
                OnPropertyChanged("jednostki");
            }
        }

        public string jednostkiSelection
        {
            get { return _jednostkiSelection; }
            set
            {
                _jednostkiSelection = value;
                OnPropertyChanged("jednostkiSelection");
            }
        }

        public ICommand usunJednostke
        {
            get
            {
                return _usunJednostke ?? (_usunJednostke = new RelayCommand(() => usunJednostkeAkcja(), null));
            }
        }

        private void usunJednostkeAkcja()
        {
            EntityMethods en = new EntityMethods();
            int id = en.pobierzIdJednostki(jednostkiSelection);
            int liczba = en.liczbaPowiazanJednostki(id);
            if (liczba > 0)
            {
                MessageBoxShow mb = new MessageBoxShow();
                mb.pokazMessageBox("Informacja", "Nie możesz usunąć jednostki, która jest używana. Najpierw musisz usunąć wszelkie powiązania.", MessageBoxTyp.Normalny, MessageBoxIkona.Informacyjna);
            }
            else
            {
                en.usunJednostke(jednostkiSelection);
                wypiszJednostki();
            }

        }
    }
}
