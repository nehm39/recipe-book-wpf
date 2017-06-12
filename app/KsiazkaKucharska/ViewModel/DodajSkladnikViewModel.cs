using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KsiazkaKucharska.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.IO;
using System.Messaging;
using System.Windows;

namespace KsiazkaKucharska.ViewModel
{
    class DodajSkladnikViewModel : INotifyPropertyChanged
    {
        public DodajSkladnikViewModel(MainWindowViewModel parentVM, int? idSkladnika)
        {
            ParentVM = parentVM;
            wypiszKategorie();
            idEdytowanegoSkladnika = idSkladnika;
            if (idSkladnika != null)
            {
                isEdit = true;
                windowTitle = "Edytuj składnik";
                btnDodajText = "Zapisz";
                EntityMethods en = new EntityMethods();
                Skladniki s = en.pobierzSkladnik((int)idSkladnika);
                nazwa = s.NAZWA;
                opis = s.OPIS;
                uwagi = s.UWAGI;
                string kategoria = en.kategoriaSkladnika((int)idSkladnika);
                kategorieSelection = kategorie.First(x => x == kategoria);
                byte[] zdj = new byte[0];
                zdj = (byte[])s.ZDJECIE;
                zdjecie = null;
                if (zdj != null && zdj.Length > 0)
                {
                    MemoryStream stream = new MemoryStream(zdj);
                    zdjecie = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
            }
            else
            {
                windowTitle = "Dodaj nowy składnik";
                btnDodajText = "Dodaj";
            }
        }

        public void wypiszKategorie()
        {
            kategorie = new ObservableCollection<string>();
            EntityMethods en = new EntityMethods();
            foreach (Kategorie_Skladniki k in en.listaKategoriiSkladnikow())
            {
                kategorie.Add(k.S_NAZWA);
            }
            if (kategorie.Count > 0) kategorieSelection = kategorie.ElementAt(0);
        }

        private int? idEdytowanegoSkladnika;
        private ICommand _pokazMessageBox;
        private Boolean isEdit = false;
        public MainWindowViewModel ParentVM { get; set; }
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

        private string _windowTitle;
        private string _btnDodajText;
        private string _nazwa;
        private string _opis;
        private string _uwagi;
        private ObservableCollection<string> _kategorie = new ObservableCollection<string>();
        private string _kategorieSelection;
        private ImageSource _zdjecie = null;
        private ICommand _dodajSkladnik;
        

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

        public string nazwa
        {
            get { return _nazwa; }
            set
            {
                _nazwa = value;
                OnPropertyChanged("nazwa");
            }
        }

        public string opis
        {
            get { return _opis; }
            set
            {
                _opis = value;
                OnPropertyChanged("opis");
            }
        }

        public string uwagi
        {
            get { return _uwagi; }
            set
            {
                _uwagi = value;
                OnPropertyChanged("uwagi");
            }
        }

        public ImageSource zdjecie
        {
            get { return _zdjecie; }
            set
            {
                _zdjecie = value;
                OnPropertyChanged("zdjecie");
            }
        }

        public string windowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                OnPropertyChanged("windowTitle");
            }
        }

        public string btnDodajText
        {
            get { return _btnDodajText; }
            set
            {
                _btnDodajText = value;
                OnPropertyChanged("btnDodajText");
            }
        }

        public void ustawZdjecie(string fileName)
        {
            if (!String.IsNullOrWhiteSpace(fileName)) zdjecie = new BitmapImage(new Uri(fileName)); 
        }

        public ICommand dodajSkladnik
        {
            get
            {
                return _dodajSkladnik ?? (_dodajSkladnik = new RelayCommand(() => dodajSkladnikAkcja(), dodajSkladnikAkcjaCanExecute));
            }
        }

        private void dodajSkladnikAkcja()
        {
            ElementGrid pTest = null;
            if (ParentVM.listaSkladnikow.Count > 0)
            {
                try
                {
                    pTest = ParentVM.listaSkladnikow.First(x => x.NAZWA == nazwa);
                }
                catch (Exception)
                {
                    pTest = null;
                }
            }
            if (pTest == null)
            {
                EntityMethods en = new EntityMethods();
                Skladniki s = new Skladniki();
                s.NAZWA = nazwa;
                s.OPIS = opis;
                s.UWAGI = uwagi;
                s.ID_KATEGORII = en.idKategoriiSkladnika(kategorieSelection);
                if (isEdit)
                {
                    s.ID_SKLADNIKA = (int)idEdytowanegoSkladnika;
                    if (zdjecie.GetType() == typeof(BitmapImage))
                    {
                        string path = ((BitmapImage)zdjecie).UriSource.OriginalString;
                        byte[] image = File.ReadAllBytes(path);
                        s.ZDJECIE = image;
                    }
                    else s.ZDJECIE = null;
                    en.edytujSkladnik(s);
                }
                else
                {

                    string path = ((BitmapImage)zdjecie).UriSource.OriginalString;
                    byte[] image = File.ReadAllBytes(path);
                    s.ZDJECIE = image;
                    idEdytowanegoSkladnika = en.dodajSkladnik(s);
                }
                CloseAction();
                ParentVM.wypiszSkladniki(idEdytowanegoSkladnika);
            }
            else
            {
                MessageBoxShow mb = new MessageBoxShow();
                mb.pokazMessageBox("Informacja", "Został już dodany taki składnik.", MessageBoxTyp.Normalny, MessageBoxIkona.Informacyjna);
            }
        }

        private bool dodajSkladnikAkcjaCanExecute()
        {
            if (String.IsNullOrWhiteSpace(nazwa) || String.IsNullOrWhiteSpace(opis) || zdjecie == null)
            {
                return false;
            }
            else return true;
        }
    }
}
