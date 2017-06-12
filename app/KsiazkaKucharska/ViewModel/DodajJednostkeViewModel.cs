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
    class DodajJednostkeViewModel : INotifyPropertyChanged
    {

        public DodajJednostkeViewModel(ZarzadzanieJednostkamiViewModel parentVM, string nazwaJednostki)
        {
            ParentVM = parentVM;
            if (String.IsNullOrWhiteSpace(nazwaJednostki))
            {
                windowTitle = "Dodaj jednostkę";
                btnDodaj = "Dodaj";
            }
            else
            {
                isEdit = true;
                windowTitle = "Edytuj jednostkę";
                btnDodaj = "Zapisz";
                nazwa = parentVM.jednostkiSelection;
                nazwaEdytowanejJednostki = nazwa;

            }
        }

        private string nazwaEdytowanejJednostki = null;
        private bool isEdit = false;
        public ZarzadzanieJednostkamiViewModel ParentVM { get; set; }
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

        private string _windowTitle;
        private string _btnDodaj;
        private string _nazwa;
        private ICommand _dodaj;

        public string windowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                OnPropertyChanged("windowTitle");
            }
        }

        public string btnDodaj
        {
            get { return _btnDodaj; }
            set
            {
                _btnDodaj = value;
                OnPropertyChanged("btnDodaj");
            }
        }

        public string nazwa
        {
            get { return _nazwa; }
            set
            {
                _nazwa = value;
                OnPropertyChanged("nazwa");
            }
        }

        public ICommand dodaj
        {
            get
            {
                return _dodaj ?? (_dodaj = new RelayCommand(() => dodajAkcja(), dodajAkcjaCanExecute));
            }
        }

        private void dodajAkcja()
        {
            string jednostka = null;
            if (ParentVM.jednostki.Count > 0)
            {
                try
                {
                    jednostka = ParentVM.jednostki.First(x => x == nazwa);
                }
                catch (Exception)
                {
                    jednostka = null;
                }
            }
            if (jednostka == null || jednostka == nazwaEdytowanejJednostki)
            {
                EntityMethods en = new EntityMethods();
                if (isEdit)
                {
                    en.edytujJednostke(nazwaEdytowanejJednostki, nazwa);
                    ParentVM.wypiszJednostki();
                }
                else
                {
                    en.dodajJednostke(nazwa);
                    ParentVM.wypiszJednostki();
                }
                CloseAction();
                
            }
            else
            {
                MessageBoxShow mb = new MessageBoxShow();
                mb.pokazMessageBox("Informacja", "Została już dodana taka jednostka.", MessageBoxTyp.Normalny, MessageBoxIkona.Informacyjna);
            }
        }

        private bool dodajAkcjaCanExecute()
        {
            if (!String.IsNullOrWhiteSpace(nazwa))
            {
                return true;
            }
            else return false;
        }
    }
}
