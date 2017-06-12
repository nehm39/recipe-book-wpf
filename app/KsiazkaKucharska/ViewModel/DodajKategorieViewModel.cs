using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KsiazkaKucharska.Model;

namespace KsiazkaKucharska.ViewModel
{
    class DodajKategorieViewModel : INotifyPropertyChanged
    {

        public DodajKategorieViewModel(ZarzadzanieKategoriamiViewModel parentVM, string nazwaKategorii)
        {
            ParentVM = parentVM;
            if (String.IsNullOrWhiteSpace(nazwaKategorii))
            {
                windowTitle = "Dodaj kategorię";
                btnDodaj = "Dodaj";
            }
            else
            {
                isEdit = true;
                windowTitle = "Edytuj kategorię";
                btnDodaj = "Zapisz";
                nazwa = parentVM.kategorieSelection;
                nazwaEdytowanejKategorii = nazwa;

            }
        }

        private string nazwaEdytowanejKategorii = null;
        private bool isEdit = false;
        public ZarzadzanieKategoriamiViewModel ParentVM { get; set; }
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
            string kategoria = null;
            if (ParentVM.kategorie.Count > 0)
            {
                try
                {
                    kategoria = ParentVM.kategorie.First(x => x == nazwa);
                }
                catch (Exception)
                {
                    kategoria = null;
                }
            }
            if (kategoria == null || kategoria == nazwaEdytowanejKategorii)
            {
                EntityMethods en = new EntityMethods();
                int index = ParentVM.rodzajeKategorii.IndexOf(ParentVM.rodzajeKategoriiSelection);
                if (isEdit)
                {
                    if (index == 0)
                    {
                        en.edytujKategorieSkladnikow(nazwaEdytowanejKategorii, nazwa);
                    }
                    else if (index == 1)
                    {
                        en.edytujKategoriePrzepisow(nazwaEdytowanejKategorii, nazwa);
                    }
                }
                else
                {
                    if (index == 0)
                    {
                        en.dodajKategorieSkladnikow(nazwa);
                    }
                    else if (index == 1)
                    {
                        en.dodajKategoriePrzepisow(nazwa);
                    }
                }
                ParentVM.wypiszKategorie(index);
                CloseAction();

            }
            else
            {
                MessageBoxShow mb = new MessageBoxShow();
                mb.pokazMessageBox("Informacja", "Została już dodana taka kategoria.", MessageBoxTyp.Normalny, MessageBoxIkona.Informacyjna);
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
