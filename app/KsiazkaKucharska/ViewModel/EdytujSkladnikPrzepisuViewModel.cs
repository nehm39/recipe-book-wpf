using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KsiazkaKucharska.Model;

namespace KsiazkaKucharska.ViewModel
{
    class EdytujSkladnikPrzepisuViewModel : INotifyPropertyChanged
    {
        public EdytujSkladnikPrzepisuViewModel(DodajPrzepisViewModel parentVM)
        {
            ParentVM = parentVM;
            EntityMethods en = new EntityMethods();
            foreach (Skladniki s in en.listaSkladnikow())
            {
                nazwySkladnikow.Add(s.NAZWA);
            }
            nazwySkladnikowSelection = ParentVM.listaSkladnikowSelection.NAZWA;

            jednostki.Add("<brak>");
            foreach (Jednostki j in en.listaJednostek())
            {
                jednostki.Add(j.NAZWA);
            }
            if (String.IsNullOrWhiteSpace(ParentVM.listaSkladnikowSelection.JEDNOSTKA)) jednostkiSelection = jednostki.ElementAt(0);
            else jednostkiSelection = ParentVM.listaSkladnikowSelection.JEDNOSTKA;

            iloscSkladnika = ParentVM.listaSkladnikowSelection.ILOSC.ToString();
        }

        private ICommand _pokazMessageBox;
        public DodajPrzepisViewModel ParentVM { get; set; }
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

        public MessageBoxWynik pokazMessageBox(string tytul, string tekst)
        {
            MessageBoxShow mb = new MessageBoxShow();
            MessageBoxWynik w = new MessageBoxWynik();
            _pokazMessageBox = new RelayCommand(() => w = mb.pokazMessageBox(tytul, tekst, MessageBoxTyp.Normalny, MessageBoxIkona.Blad));
            _pokazMessageBox.Execute(null);
            return w;
        }

        private ObservableCollection<string> _nazwySkladnikow = new ObservableCollection<string>();
        private string _nazwySkladnikowSelection;
        private ObservableCollection<string> _jednostki = new ObservableCollection<string>();
        private string _jednostkiSelection;
        private string _iloscSkladnika;
        private ICommand _zapisz;

        public ObservableCollection<string> nazwySkladnikow
        {
            get { return _nazwySkladnikow; }

            set
            {
                _nazwySkladnikow = value;
                OnPropertyChanged("nazwySkladnikow");
            }
        }

        public string nazwySkladnikowSelection
        {
            get { return _nazwySkladnikowSelection; }
            set
            {
                _nazwySkladnikowSelection = value;
                OnPropertyChanged("nazwySkladnikowSelection");
            }
        }

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

        public string iloscSkladnika
        {
            get { return _iloscSkladnika; }
            set
            {
                _iloscSkladnika = value;
                OnPropertyChanged("iloscSkladnika");
            }
        }

        public ICommand zapisz
        {
            get
            {
                return _zapisz ?? (_zapisz = new RelayCommand(() => zapiszAkcja(), zapiszAkcjaCanExecute));
            }
        }

        private void zapiszAkcja()
        {
            SkladnikPrzepisu sp = null;
            if (ParentVM.listaSkladnikow.Count > 0 && (ParentVM.listaSkladnikowSelection.NAZWA != nazwySkladnikowSelection))
            {
                try
                {
                    sp = ParentVM.listaSkladnikow.First(x => x.NAZWA == nazwySkladnikowSelection);
                }
                catch (Exception)
                {
                    sp = null;
                }
            }
            if (sp == null)
            {
                EntityMethods en = new EntityMethods();
                sp = new SkladnikPrzepisu();
                if (jednostki.IndexOf(jednostkiSelection) != 0) sp.JEDNOSTKA = jednostkiSelection;
                sp.NAZWA = nazwySkladnikowSelection;
                double iloscDouble;
                Double.TryParse(iloscSkladnika, out iloscDouble);
                if (iloscDouble != 0) sp.ILOSC = iloscDouble;
                ParentVM.listaSkladnikow.ElementAt(ParentVM.listaSkladnikow.IndexOf(ParentVM.listaSkladnikowSelection)).NAZWA = sp.NAZWA;
                ParentVM.listaSkladnikow.ElementAt(ParentVM.listaSkladnikow.IndexOf(ParentVM.listaSkladnikowSelection)).JEDNOSTKA = sp.JEDNOSTKA;
                ParentVM.listaSkladnikow.ElementAt(ParentVM.listaSkladnikow.IndexOf(ParentVM.listaSkladnikowSelection)).ILOSC = sp.ILOSC;
                ObservableCollection<SkladnikPrzepisu> temp = ParentVM.listaSkladnikow;
                ParentVM.listaSkladnikow = new ObservableCollection<SkladnikPrzepisu>();
                ParentVM.listaSkladnikow = temp;
                CloseAction();
            }
            else
            {
                MessageBoxShow mb = new MessageBoxShow();
                mb.pokazMessageBox("Informacja", "Został już dodany taki składnik", MessageBoxTyp.Normalny, MessageBoxIkona.Informacyjna);
            }
        }

        private bool zapiszAkcjaCanExecute()
        {
            double parsedValue;
            if (String.IsNullOrWhiteSpace(iloscSkladnika) || Double.TryParse(iloscSkladnika, out parsedValue))
            {
                return true;
            }
            else return false;
        }

    }
}
