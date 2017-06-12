using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KsiazkaKucharska.Model;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace KsiazkaKucharska.ViewModel
{
    public class DodajPrzepisViewModel : INotifyPropertyChanged
    {
        public DodajPrzepisViewModel(MainWindowViewModel parentVM, int? idPrzepisu)
        {
            ParentVM = parentVM;
            wypiszKategorie();
            wypiszNazwySkladnikow();
            wypiszJednostki();
            idEdytowanegoPrzepisu = idPrzepisu;
            if (idPrzepisu != null)
            {
                isEdit = true;
                windowTitle = "Edytuj przepis";
                btnDodajText = "Zapisz";
                EntityMethods en = new EntityMethods();
                Przepisy p = en.pobierzPrzepis((int)idPrzepisu);
                nazwa = p.NAZWA;
                opis = p.OPIS;
                notatki = p.NOTATKI;
                iloscPorcji = p.ILOSC_PORCJI.ToString();
                int? czasWM = p.CZAS_PRZYRZADZENIA;
                int? godziny = czasWM / 60;
                czasH = godziny.ToString();
                int? minuty = czasWM - godziny * 60;
                czasM = minuty.ToString();
                string kategoria = en.kategoriaPrzepisu((int)idPrzepisu);
                kategorieSelection = kategorie.First(x => x == kategoria);
                byte[] zdj = new byte[0];
                zdj = (byte[])p.ZDJECIE;
                zdjecie = null;
                if (zdj != null && zdj.Length > 0)
                {
                    MemoryStream stream = new MemoryStream(zdj);
                    zdjecie = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
                listaSkladnikow = new ObservableCollection<SkladnikPrzepisu>();
                foreach (SkladnikPrzepisu sp in en.skladnikiPrzepisu((int)idPrzepisu))
                {
                    listaSkladnikow.Add(sp);
                }
            }
            else
            {
                windowTitle = "Dodaj nowy przepis";
                btnDodajText = "Dodaj";
            }
        }
        public void wypiszKategorie()
        {
            kategorie = new ObservableCollection<string>();
            EntityMethods en = new EntityMethods();
            foreach (Kategorie_Przepisy k in en.listaKategoriiPrzepisow())
            {
                kategorie.Add(k.P_NAZWA);
            }
            if (kategorie.Count > 0) kategorieSelection = kategorie.ElementAt(0);
        }

        private void wypiszNazwySkladnikow()
        {
            EntityMethods en = new EntityMethods();
            foreach (Skladniki s in en.listaSkladnikow())
            {
                nazwySkladnikow.Add(s.NAZWA);
            }
            if (nazwySkladnikow.Count > 0) nazwySkladnikowSelection = nazwySkladnikow.ElementAt(0);
        }

        private void wypiszJednostki()
        {
            EntityMethods en = new EntityMethods();
            jednostki.Add("<brak>");
            foreach (Jednostki j in en.listaJednostek())
            {
                jednostki.Add(j.NAZWA);
            }
            if (jednostki.Count > 0) jednostkiSelection = jednostki.ElementAt(0);
        }

        private int? idEdytowanegoPrzepisu;
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
        private string _notatki;
        private string _iloscPorcji;
        private string _czasH;
        private string _czasM;
        private ObservableCollection<string> _kategorie = new ObservableCollection<string>();
        private string _kategorieSelection;
        private ObservableCollection<string> _nazwySkladnikow = new ObservableCollection<string>();
        private string _nazwySkladnikowSelection;
        private ObservableCollection<string> _jednostki = new ObservableCollection<string>();
        private string _jednostkiSelection;
        private ObservableCollection<SkladnikPrzepisu> _listaSkladnikow = new ObservableCollection<SkladnikPrzepisu>();
        private SkladnikPrzepisu _listaSkladnikowSelection;
        private string _iloscSkladnika;
        private ImageSource _zdjecie = null;
        private ICommand _dodajPrzepis;
        private ICommand _dodajSkladnik;
        private ICommand _usunSkladnik;


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

        public ObservableCollection<SkladnikPrzepisu> listaSkladnikow
        {
            get { return _listaSkladnikow; }

            set
            {
                _listaSkladnikow = value;
                OnPropertyChanged("listaSkladnikow");
            }
        }

        public SkladnikPrzepisu listaSkladnikowSelection
        {
            get { return _listaSkladnikowSelection; }
            set
            {
                _listaSkladnikowSelection = value;
                OnPropertyChanged("listaSkladnikowSelection");
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

        public string notatki
        {
            get { return _notatki; }
            set
            {
                _notatki = value;
                OnPropertyChanged("notatki");
            }
        }

        public string iloscPorcji
        {
            get { return _iloscPorcji; }
            set
            {
                _iloscPorcji = value;
                OnPropertyChanged("iloscPorcji");
            }
        }

        public string czasH
        {
            get { return _czasH; }
            set
            {
                _czasH = value;
                OnPropertyChanged("czasH");
            }
        }

        public string czasM
        {
            get { return _czasM; }
            set
            {
                _czasM = value;
                OnPropertyChanged("czasM");
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
            SkladnikPrzepisu sp = null;
            if (listaSkladnikow.Count > 0)
            {
                try
                {
                    sp = listaSkladnikow.First(x => x.NAZWA == nazwySkladnikowSelection);
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
                listaSkladnikow.Add(sp);
            }
            else
            {
                MessageBoxShow mb = new MessageBoxShow();
                mb.pokazMessageBox("Informacja", "Został już dodany taki składnik.", MessageBoxTyp.Normalny, MessageBoxIkona.Informacyjna);
            }
        }

        private bool dodajSkladnikAkcjaCanExecute()
        {
            double parsedValue;
            if (String.IsNullOrWhiteSpace(iloscSkladnika) || Double.TryParse(iloscSkladnika, out parsedValue))
            {
                return true;
            }
            else return false;
        }

        public ICommand usunSkladnik
        {
            get
            {
                return _usunSkladnik ?? (_usunSkladnik = new RelayCommand(() => usunSkladnikAkcja(), null));
            }
        }

        private void usunSkladnikAkcja()
        {
            listaSkladnikow.Remove(listaSkladnikowSelection);
        }

        public ICommand dodajPrzepis
        {
            get
            {
                return _dodajPrzepis ?? (_dodajPrzepis = new RelayCommand(() => dodajPrzepisAkcja(), dodajPrzepisAkcjaCanExecute));
            }
        }

        private void dodajPrzepisAkcja()
        {
            EntityMethods en = new EntityMethods();

            ElementGrid pTest = null;
            if (ParentVM.listaPrzepisow.Count > 0)
            {
                try
                {
                    pTest = ParentVM.listaPrzepisow.First(x => x.NAZWA == nazwa);
                }
                catch (Exception)
                {
                    pTest = null;
                }
            }
            if (pTest == null)
            {

                Przepisy p = new Przepisy();
                p.NAZWA = nazwa;
                p.NOTATKI = notatki;
                p.OPIS = opis;
                p.ILOSC_PORCJI = Double.Parse(iloscPorcji);
                p.ID_KATEGORII = en.idKategoriiPrzepisu(kategorieSelection);
                int czas = 0;
                if (czasH != null) czas = Int32.Parse(czasH) * 60 + Int32.Parse(czasM);
                else czas = Int32.Parse(czasM);
                p.CZAS_PRZYRZADZENIA = czas;
                if (isEdit == false)
                {
                    string path = ((BitmapImage)zdjecie).UriSource.OriginalString;
                    byte[] image = File.ReadAllBytes(path);
                    p.ZDJECIE = image;

                    idEdytowanegoPrzepisu = en.dodajPrzepis(p);
                }
                else
                {
                    if (zdjecie.GetType() == typeof(BitmapImage))
                    {
                        string path = ((BitmapImage)zdjecie).UriSource.OriginalString;
                        byte[] image = File.ReadAllBytes(path);
                        p.ZDJECIE = image;
                    }
                    else p.ZDJECIE = null;

                    p.ID_PRZEPISU = (int)idEdytowanegoPrzepisu;

                    en.edytujPrzepis(p);

                    en.czyscPrzepisySkladniki((int)idEdytowanegoPrzepisu);
                }

                List<Przepisy_Skladniki> lista = new List<Przepisy_Skladniki>();
                foreach (SkladnikPrzepisu sp in listaSkladnikow)
                {
                    Przepisy_Skladniki ps = new Przepisy_Skladniki();
                    ps.L_ID_PRZEPISU = (int)idEdytowanegoPrzepisu;
                    if (sp.ILOSC != null) ps.ILOSC = sp.ILOSC;
                    if (!String.IsNullOrWhiteSpace(sp.JEDNOSTKA)) ps.ID_JEDNOSTKI = en.pobierzIdJednostki(sp.JEDNOSTKA);
                    ps.L_ID_SKLADNIKA = en.idSkladnika(sp.NAZWA);
                    en.dodajPrzepisySkladniki(ps);
                }

                ParentVM.wypiszPrzepisy((int)idEdytowanegoPrzepisu);
                CloseAction();
            }
            else
            {
                MessageBoxShow mb = new MessageBoxShow();
                mb.pokazMessageBox("Informacja", "Został już dodany taki przepis.", MessageBoxTyp.Normalny, MessageBoxIkona.Informacyjna);
            }
        }

        private bool dodajPrzepisAkcjaCanExecute()
        {
            double parsedValueIloscPorcji;
            int parsedValueCzasH;
            int parsedValueCzasM;
            if (String.IsNullOrWhiteSpace(nazwa) || String.IsNullOrWhiteSpace(opis) || String.IsNullOrWhiteSpace(iloscPorcji) || String.IsNullOrWhiteSpace(czasM) || 
                zdjecie == null || listaSkladnikow.Count == 0 || !Double.TryParse(iloscPorcji, out parsedValueIloscPorcji) || (!Int32.TryParse(czasH, out parsedValueCzasH) && !String.IsNullOrWhiteSpace(czasH)) || 
                !Int32.TryParse(czasM, out parsedValueCzasM) || parsedValueIloscPorcji < 1 || (Int32.TryParse(czasH, out parsedValueCzasH) && parsedValueCzasH < 0) ||
                parsedValueCzasM < 1 || parsedValueCzasM > 59)
            {
                return false;
            }
            else return true;
        }
    }
}
