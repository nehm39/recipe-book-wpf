using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KsiazkaKucharska.Model;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;

namespace KsiazkaKucharska.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        #region Konstruktor

        public MainWindowViewModel()
        {
            EntityMethods en = new EntityMethods();

            //Przepisy:

            wypiszPrzepisy(null);

            cbSzukaniePrzepisowKategorie.Add("<dowolna>");
            foreach (Kategorie_Przepisy k in en.listaKategoriiPrzepisow())
            {
                cbSzukaniePrzepisowKategorie.Add(k.P_NAZWA);
            }
            cbSzukaniePrzepisowKategorieSelection = cbSzukaniePrzepisowKategorie.ElementAt(0);

            cbSzukaniePrzepisowIloscPorcjiTyp.Add("<");
            cbSzukaniePrzepisowIloscPorcjiTyp.Add("=");
            cbSzukaniePrzepisowIloscPorcjiTyp.Add(">");
            cbSzukaniePrzepisowIloscPorcjiTypSelection = cbSzukaniePrzepisowIloscPorcjiTyp.ElementAt(0);


            cbSzukaniePrzepisowCzasPrzyrzadzeniaTyp.Add("<");
            cbSzukaniePrzepisowCzasPrzyrzadzeniaTyp.Add("=");
            cbSzukaniePrzepisowCzasPrzyrzadzeniaTyp.Add(">");
            cbSzukaniePrzepisowCzasPrzyrzadzeniaTypSelection = cbSzukaniePrzepisowCzasPrzyrzadzeniaTyp.ElementAt(0);

            //Składniki:

            wypiszSkladniki(null);

            cbSzukanieSkladnikowKategorie.Add("<dowolna>");
            foreach (Kategorie_Skladniki k in en.listaKategoriiSkladnikow())
            {
                cbSzukanieSkladnikowKategorie.Add(k.S_NAZWA);
            }
            cbSzukanieSkladnikowKategorieSelection = cbSzukanieSkladnikowKategorie.ElementAt(0);

            
        }

        public void wypiszPrzepisy(int? idToSelect)
        {
            listaPrzepisow = new ObservableCollection<ElementGrid>();
            int selectionIndex = 0;
            EntityMethods en = new EntityMethods();
            foreach (Przepisy p in en.listaPrzepisow())
            {
                listaPrzepisow.Add(new ElementGrid { ID = p.ID_PRZEPISU, NAZWA = p.NAZWA });
                if (idToSelect != null)
                {
                    if (p.ID_PRZEPISU == idToSelect)
                    {
                        selectionIndex = listaPrzepisow.Count - 1;
                    }
                }
            }
            listaPrzepisowSelection = listaPrzepisow.ElementAt(selectionIndex);
        }

        public void wypiszSkladniki(int? idToSelect)
        {
            listaSkladnikow = new ObservableCollection<ElementGrid>();
            int selectionIndex = 0;
            EntityMethods en = new EntityMethods();
            foreach (Skladniki s in en.listaSkladnikow())
            {
                listaSkladnikow.Add(new ElementGrid { ID = s.ID_SKLADNIKA, NAZWA = s.NAZWA });
                if (idToSelect != null)
                {
                    if (s.ID_SKLADNIKA == idToSelect)
                    {
                        selectionIndex = listaSkladnikow.Count - 1;
                    }
                }
            }
            listaSkladnikowSelection = listaSkladnikow.ElementAt(selectionIndex);
        }

        #endregion

        #region Ogólne

        public event PropertyChangedEventHandler PropertyChanged;
        private int _tabControlIndex = 0;

        public int tabControlIndex
        {
            get { return _tabControlIndex; }
            set
            {
                _tabControlIndex = value;
                OnPropertyChanged("tabControlIndex");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Przepisy

        private ObservableCollection<ElementGrid> _listaPrzepisow = new ObservableCollection<ElementGrid>();
        private ElementGrid _listaPrzepisowSelection;
        private ObservableCollection<SkladnikPrzepisu> _listaSkladnikowPrzepisu;
        private SkladnikPrzepisu _listaSkladnikowPrzepisuSelection;
        private ObservableCollection<string> _cbSzukaniePrzepisowKategorie = new ObservableCollection<string>();
        private string _cbSzukaniePrzepisowKategorieSelection;
        private ObservableCollection<string> _cbSzukaniePrzepisowIloscPorcjiTyp = new ObservableCollection<string>();
        private string _cbSzukaniePrzepisowIloscPorcjiTypSelection;
        private ObservableCollection<string> _cbSzukaniePrzepisowCzasPrzyrzadzeniaTyp = new ObservableCollection<string>();
        private string _cbSzukaniePrzepisowCzasPrzyrzadzeniaTypSelection;
        private string _opisPrzepisu;
        private string _notatkiPrzepisu;
        private string _kategoriaPrzepisu;
        private string _iloscPorcjiPrzepisu;
        private string _czasHPrzepisu;
        private string _czasMPrzepisu;
        private ImageSource _zdjeciePrzepisu;
        private string _szukaniePrzepisow;
        private ICommand _drukujPrzepis;
        private ICommand _rozwinSzukaniePrzepisow;
        private ICommand _szukajPrzepisy;
        private ICommand _znajdzSkladnikPrzepisu;
        private ICommand _usunPrzepis;
        private Boolean _widocznoscGridWyszukiwaniePrzepisow = false;
        private Boolean _txbSzukaniePrzepisowAktywnosc = true;
        private string _btnSzukaniePrzepisow = "Rozwiń zaawansowane wyszukiwanie";
        private string _szukaniePrzepisowNazwa;
        private string _szukaniePrzepisowOpis;
        private string _szukaniePrzepisowNotatki;
        private string _szukaniePrzepisowPorcje;
        private string _szukaniePrzepisowCzasPrzyrzadzenia;


        public ObservableCollection<SkladnikPrzepisu> listaSkladnikowPrzepisu
        {
            get { return _listaSkladnikowPrzepisu; }

            set
            {
                _listaSkladnikowPrzepisu = value;
                OnPropertyChanged("listaSkladnikowPrzepisu");
            }
        }

        public SkladnikPrzepisu listaSkladnikowPrzepisuSelection
        {
            get { return _listaSkladnikowPrzepisuSelection; }
            set
            {
                _listaSkladnikowPrzepisuSelection = value;
                OnPropertyChanged("listaSkladnikowPrzepisuSelection");
            }
        }

        public ObservableCollection<ElementGrid> listaPrzepisow
        {
            get { return _listaPrzepisow; }

            set
            {
                _listaPrzepisow = value;
                OnPropertyChanged("listaPrzepisow");
            }
        }

        public ElementGrid listaPrzepisowSelection
        {
            get
            {
                return _listaPrzepisowSelection;
            }
            set
            {
                _listaPrzepisowSelection = value;
                OnPropertyChanged("zaznaczonyPrzepisGrid");
                if (value != null)
                {
                    EntityMethods en = new EntityMethods();
                    Przepisy p = en.pobierzPrzepis(value.ID);
                    opisPrzepisu = p.OPIS;
                    notatkiPrzepisu = p.NOTATKI;
                    kategoriaPrzepisu = en.kategoriaPrzepisu(value.ID);
                    iloscPorcjiPrzepisu = p.ILOSC_PORCJI.ToString();
                    int? czasM = p.CZAS_PRZYRZADZENIA;
                    int? godziny = czasM / 60;
                    czasHPrzepisu = godziny.ToString();
                    int? minuty = czasM - godziny * 60;
                    czasMPrzepisu = minuty.ToString();
                    byte[] zdj = new byte[0];
                    zdj = (byte[])p.ZDJECIE;
                    zdjeciePrzepisu = null;
                    if (zdj != null && zdj.Length > 0)
                    {
                        MemoryStream stream = new MemoryStream(zdj);
                        zdjeciePrzepisu = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                    listaSkladnikowPrzepisu = new ObservableCollection<SkladnikPrzepisu>();
                    foreach (SkladnikPrzepisu sp in en.skladnikiPrzepisu(value.ID))
                    {
                        listaSkladnikowPrzepisu.Add(sp);
                    }
                }
            }
        }

        public ObservableCollection<string> cbSzukaniePrzepisowKategorie
        {
            get { return _cbSzukaniePrzepisowKategorie; }

            set
            {
                _cbSzukaniePrzepisowKategorie = value;
                OnPropertyChanged("cbSzukaniePrzepisowKategorie");
            }
        }

        public string cbSzukaniePrzepisowKategorieSelection
        {
            get { return _cbSzukaniePrzepisowKategorieSelection; }
            set
            {
                _cbSzukaniePrzepisowKategorieSelection = value;
                OnPropertyChanged("cbSzukaniePrzepisowKategorieSelection");
            }
        }

        public ObservableCollection<string> cbSzukaniePrzepisowIloscPorcjiTyp
        {
            get { return _cbSzukaniePrzepisowIloscPorcjiTyp; }

            set
            {
                _cbSzukaniePrzepisowIloscPorcjiTyp = value;
                OnPropertyChanged("cbSzukaniePrzepisowIloscPorcjiTyp");
            }
        }

        public string cbSzukaniePrzepisowIloscPorcjiTypSelection
        {
            get { return _cbSzukaniePrzepisowIloscPorcjiTypSelection; }
            set
            {
                _cbSzukaniePrzepisowIloscPorcjiTypSelection = value;
                OnPropertyChanged("cbSzukaniePrzepisowIloscPorcjiTypSelection");
            }
        }

        public ObservableCollection<string> cbSzukaniePrzepisowCzasPrzyrzadzeniaTyp
        {
            get { return _cbSzukaniePrzepisowCzasPrzyrzadzeniaTyp; }

            set
            {
                _cbSzukaniePrzepisowCzasPrzyrzadzeniaTyp = value;
                OnPropertyChanged("cbSzukaniePrzepisowCzasPrzyrzadzeniaTyp");
            }
        }

        public string cbSzukaniePrzepisowCzasPrzyrzadzeniaTypSelection
        {
            get { return _cbSzukaniePrzepisowCzasPrzyrzadzeniaTypSelection; }
            set
            {
                _cbSzukaniePrzepisowCzasPrzyrzadzeniaTypSelection = value;
                OnPropertyChanged("cbSzukaniePrzepisowCzasPrzyrzadzeniaTypSelection");
            }
        }

        public string opisPrzepisu
        {
            get { return _opisPrzepisu; }
            set
            {
                _opisPrzepisu = value;
                OnPropertyChanged("opisPrzepisu");
            }
        }

        public string notatkiPrzepisu
        {
            get { return _notatkiPrzepisu; }
            set
            {
                _notatkiPrzepisu = value;
                OnPropertyChanged("notatkiPrzepisu");
            }
        }

        public string kategoriaPrzepisu
        {
            get { return _kategoriaPrzepisu; }
            set
            {
                _kategoriaPrzepisu = value;
                OnPropertyChanged("kategoriaPrzepisu");
            }
        }

        public string iloscPorcjiPrzepisu
        {
            get { return _iloscPorcjiPrzepisu; }
            set
            {
                _iloscPorcjiPrzepisu = value;
                OnPropertyChanged("iloscPorcjiPrzepisu");
            }
        }

        public string czasHPrzepisu
        {
            get { return _czasHPrzepisu; }
            set
            {
                _czasHPrzepisu = value;
                OnPropertyChanged("czasHPrzepisu");
            }
        }

        public string czasMPrzepisu
        {
            get { return _czasMPrzepisu; }
            set
            {
                _czasMPrzepisu = value;
                OnPropertyChanged("czasMPrzepisu");
            }
        }

        public ImageSource zdjeciePrzepisu
        {
            get { return _zdjeciePrzepisu; }
            set
            {
                _zdjeciePrzepisu = value;
                OnPropertyChanged("zdjeciePrzepisu");
            }
        }

        public string szukaniePrzepisow
        {
            get { return _szukaniePrzepisow; }
            set
            {
                _szukaniePrzepisow = value;
                OnPropertyChanged("szukaniePrzepisow");
                EntityMethods en = new EntityMethods();
                listaPrzepisow = new ObservableCollection<ElementGrid>();
                foreach (Przepisy p in en.listaPrzepisow(value))
                {
                    listaPrzepisow.Add(new ElementGrid { ID = p.ID_PRZEPISU, NAZWA = p.NAZWA });
                }
                if (listaPrzepisow.Count > 0) listaPrzepisowSelection = listaPrzepisow.ElementAt(0);
            }
        }

        public Boolean widocznoscGridWyszukiwaniePrzepisow
        {
            get { return _widocznoscGridWyszukiwaniePrzepisow; }
            set
            {
                _widocznoscGridWyszukiwaniePrzepisow = value;
                OnPropertyChanged("widocznoscGridWyszukiwaniePrzepisow");
            }
        }

        public Boolean txbSzukaniePrzepisowAktywnosc
        {
            get { return _txbSzukaniePrzepisowAktywnosc; }
            set
            {
                _txbSzukaniePrzepisowAktywnosc = value;
                OnPropertyChanged("txbSzukaniePrzepisowAktywnosc");
            }
        }

        public string btnSzukaniePrzepisow
        {
            get { return _btnSzukaniePrzepisow; }
            set
            {
                _btnSzukaniePrzepisow = value;
                OnPropertyChanged("btnSzukaniePrzepisow");
            }
        }

        public string szukaniePrzepisowNazwa
        {
            get { return _szukaniePrzepisowNazwa; }
            set
            {
                _szukaniePrzepisowNazwa = value;
                OnPropertyChanged("szukaniePrzepisowNazwa");
            }
        }

        public string szukaniePrzepisowOpis
        {
            get { return _szukaniePrzepisowOpis; }
            set
            {
                _szukaniePrzepisowOpis = value;
                OnPropertyChanged("szukaniePrzepisowOpis");
            }
        }

        public string szukaniePrzepisowNotatki
        {
            get { return _szukaniePrzepisowNotatki; }
            set
            {
                _szukaniePrzepisowNotatki = value;
                OnPropertyChanged("szukaniePrzepisowNotatki");
            }
        }

        public string szukaniePrzepisowPorcje
        {
            get { return _szukaniePrzepisowPorcje; }
            set
            {
                _szukaniePrzepisowPorcje = value;
                OnPropertyChanged("szukaniePrzepisowPorcje");
            }
        }

        public string szukaniePrzepisowCzasPrzyrzadzenia
        {
            get { return _szukaniePrzepisowCzasPrzyrzadzenia; }
            set
            {
                _szukaniePrzepisowCzasPrzyrzadzenia = value;
                OnPropertyChanged("szukaniePrzepisowCzasPrzyrzadzenia");
            }
        }

        public ICommand rozwinSzukaniePrzepisow
        {
            get
            {
                return _rozwinSzukaniePrzepisow ?? (_rozwinSzukaniePrzepisow = new RelayCommand(() => rozwinSzukaniePrzepisowAkcja(), null));
            }
        }

        private void rozwinSzukaniePrzepisowAkcja()
        {
            if (!String.IsNullOrWhiteSpace(szukaniePrzepisow)) szukaniePrzepisow = String.Empty;

            if (widocznoscGridWyszukiwaniePrzepisow)
            {
                wypiszPrzepisy(listaPrzepisowSelection.ID);
                widocznoscGridWyszukiwaniePrzepisow = false;
                btnSzukaniePrzepisow = "Rozwiń zaawansowane wyszukiwanie";
                txbSzukaniePrzepisowAktywnosc = true;
            }
            else
            {
                widocznoscGridWyszukiwaniePrzepisow = true;
                btnSzukaniePrzepisow = "Zwiń zaawansowane wyszukiwanie";
                txbSzukaniePrzepisowAktywnosc = false;
            }
        }

        public ICommand drukujPrzepis
        {
            get
            {
                return _drukujPrzepis ?? (_drukujPrzepis = new RelayCommand(() => wydrukujPrzepis(), null));
            }
        }

        private void wydrukujPrzepis()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument doc = new FlowDocument();
                Section sec = new Section();
                EntityMethods en = new EntityMethods();
                Przepisy p = en.pobierzPrzepis(listaPrzepisowSelection.ID);
                Paragraph pg = new Paragraph();
                Bold bld = new Bold();
                bld.Inlines.Add(new Run(p.NAZWA));
                pg.Inlines.Add(bld);
                pg.Inlines.Add(new Run("\n\n" + "Kategoria: " + en.kategoriaPrzepisu(p.ID_KATEGORII.Value)));
                pg.Inlines.Add(new Run("\nIlość porcji: " + p.ILOSC_PORCJI.ToString()));
                int? czasM = p.CZAS_PRZYRZADZENIA;
                int? godziny = czasM / 60;
                int? minuty = czasM - godziny * 60;
                pg.Inlines.Add(new Run("\nCzas przyrządzenia: " + godziny + " h " + minuty + " m "));
                pg.Inlines.Add(new Run("\n\nOpis:\n" + p.OPIS));
                string skladniki = "";
                List<SkladnikPrzepisu> listaSkladnikow = en.skladnikiPrzepisu(p.ID_PRZEPISU);
                foreach (SkladnikPrzepisu s in listaSkladnikow)
                {
                    if (String.IsNullOrWhiteSpace(s.ILOSC.ToString()) && String.IsNullOrWhiteSpace(s.JEDNOSTKA))
                    {
                        skladniki += s.NAZWA + "\n";
                    }
                    else
                    {
                        skladniki += s.NAZWA + " - " + s.ILOSC.ToString() + " " + s.JEDNOSTKA + "\n";
                    }
                }
                pg.Inlines.Add(new Run("\n\nSkładniki:\n" + skladniki));
                if (!String.IsNullOrWhiteSpace(p.NOTATKI)) pg.Inlines.Add(new Run("\nNotatki:\n" + p.NOTATKI));
                sec.Blocks.Add(pg);
                doc.Blocks.Add(sec);
                IDocumentPaginatorSource idpSource = doc;
                printDialog.PrintDocument(idpSource.DocumentPaginator, "Drukowanie przepisu");
            }
        }

        public ICommand szukajPrzepisy
        {
            get
            {
                return _szukajPrzepisy ?? (_szukajPrzepisy = new RelayCommand(() => szukajPrzepisyAkcja(), null));
            }
        }

        private void szukajPrzepisyAkcja()
        {
            listaPrzepisow = new ObservableCollection<ElementGrid>();
            EntityMethods en = new EntityMethods();
            int idKategorii = 0;
            if (cbSzukaniePrzepisowKategorie.IndexOf(cbSzukaniePrzepisowKategorieSelection) != 0)
            {
                idKategorii = en.idKategoriiPrzepisu(cbSzukaniePrzepisowKategorieSelection);
            }
            int iloscPorcji = 0;
            int czasPrzyrzadzenia = 0;
            try
            {
                iloscPorcji = Int32.Parse(szukaniePrzepisowPorcje); 
            }
            catch (Exception) { }

            try
            {
                czasPrzyrzadzenia = Int32.Parse(szukaniePrzepisowCzasPrzyrzadzenia);
            }
            catch (Exception) { }

            List<Przepisy> lista = en.listaPrzepisow(szukaniePrzepisowNazwa, szukaniePrzepisowOpis, szukaniePrzepisowNotatki, idKategorii, iloscPorcji, cbSzukaniePrzepisowIloscPorcjiTyp.IndexOf(cbSzukaniePrzepisowIloscPorcjiTypSelection), czasPrzyrzadzenia, cbSzukaniePrzepisowCzasPrzyrzadzeniaTyp.IndexOf(cbSzukaniePrzepisowCzasPrzyrzadzeniaTypSelection));
            foreach (Przepisy p in lista)
            {
                listaPrzepisow.Add(new ElementGrid { ID = p.ID_PRZEPISU, NAZWA = p.NAZWA });
            }
            if (listaPrzepisow.Count > 0) listaPrzepisowSelection = listaPrzepisow.ElementAt(0);
        }


        public ICommand znajdzSkladnikPrzepisu
        {
            get
            {
                return _znajdzSkladnikPrzepisu ?? (_znajdzSkladnikPrzepisu = new RelayCommand(() => znajdzSkladnikPrzepisuAkcja(), null));
            }
        }

        private void znajdzSkladnikPrzepisuAkcja()
        {
            SkladnikPrzepisu s = listaSkladnikowPrzepisuSelection;
            int id = s.ID;
            tabControlIndex = 1;
            wypiszSkladniki(id);
        }

        public ICommand usunPrzepis
        {
            get
            {
                return _usunPrzepis ?? (_usunPrzepis = new RelayCommand(() => usunPrzepisAkcja(), null));
            }
        }

        private void usunPrzepisAkcja()
        {
            MessageBoxShow mb = new MessageBoxShow();
            EntityMethods en = new EntityMethods();
            MessageBoxWynik w = mb.pokazMessageBox("Potwierdzenie", "Czy na pewno chcesz usunąć zaznaczony przepis?", MessageBoxTyp.TakNie, MessageBoxIkona.Pytanie);
            if (w == MessageBoxWynik.Tak)
            {
                en.usunPrzepis(listaPrzepisowSelection.ID);
                int selectIndex = 0;
                if (listaPrzepisow.Count > 2 && listaPrzepisow.IndexOf(listaPrzepisowSelection) > 0) selectIndex = listaPrzepisow.IndexOf(listaPrzepisowSelection) - 1;
                wypiszPrzepisy(null);
                wypiszSkladniki(null);
                if (listaPrzepisow.Count > 0) listaPrzepisowSelection = listaPrzepisow.ElementAt(selectIndex);
                mb.pokazMessageBox("Informacja", "Przepis został pomyślnie usunięty.", MessageBoxTyp.Normalny, MessageBoxIkona.Informacyjna);
            }
        }

        #endregion

        #region Składniki

        private ObservableCollection<ElementGrid> _listaSkladnikow = new ObservableCollection<ElementGrid>();
        private ElementGrid _listaSkladnikowSelection;
        private ObservableCollection<ElementGrid> _listaPrzepisowSkladnika;
        private ElementGrid _listaPrzepisowSkladnikaSelection;
        private ObservableCollection<string> _cbSzukanieSkladnikowKategorie = new ObservableCollection<string>();
        private string _cbSzukanieSkladnikowKategorieSelection;
        private string _opisSkladnika;
        private string _uwagiSkladnika;
        private string _kategoriaSkladnika;
        private ImageSource _zdjecieSkladnika;
        private string _szukanieSkladnikow;
        private Boolean _widocznoscGridWyszukiwanieSkladnikow = false;
        private string _btnSzukanieSkladnikow = "Rozwiń zaawansowane wyszukiwanie";
        private Boolean _txbSzukanieSkladnikowAktywnosc = true;
        private string _szukanieSkladnikowNazwa;
        private string _szukanieSkladnikowOpis;
        private string _szukanieSkladnikowUwagi;
        private ICommand _znajdzPrzepisSkladnika;
        private ICommand _rozwinSzukanieSkladnikow;
        private ICommand _szukajSkladniki;
        private ICommand _szukajSkladnikaAlma;
        private ICommand _szukajSkladnikaGoogle;
        private ICommand _usunSkladnik;

        public ObservableCollection<ElementGrid> listaSkladnikow
        {
            get { return _listaSkladnikow; }

            set
            {
                _listaSkladnikow = value;
                OnPropertyChanged("listaSkladnikow");
            }
        }

        public ElementGrid listaSkladnikowSelection
        {
            get
            {
                return _listaSkladnikowSelection;
            }
            set
            {
                _listaSkladnikowSelection = value;
                OnPropertyChanged("listaSkladnikow");
                if (value != null)
                {
                    EntityMethods en = new EntityMethods();
                    Skladniki s = en.pobierzSkladnik(value.ID);
                    opisSkladnika = s.OPIS;
                    uwagiSkladnika = s.UWAGI;
                    kategoriaSkladnika = en.kategoriaSkladnika(value.ID);
                    byte[] zdj = new byte[0];
                    zdj = (byte[])s.ZDJECIE;
                    zdjecieSkladnika = null;
                    if (zdj != null && zdj.Length > 0)
                    {
                        MemoryStream stream = new MemoryStream(zdj);
                        zdjecieSkladnika = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                    listaPrzepisowSkladnika = new ObservableCollection<ElementGrid>();
                    foreach (ElementGrid p in en.przepisySkladnika(value.ID))
                    {
                        listaPrzepisowSkladnika.Add(p);
                    }
                }
            }
        }

        public ObservableCollection<ElementGrid> listaPrzepisowSkladnika
        {
            get { return _listaPrzepisowSkladnika; }

            set
            {
                _listaPrzepisowSkladnika = value;
                OnPropertyChanged("listaPrzepisowSkladnika");
            }
        }

        public ElementGrid listaPrzepisowSkladnikaSelection
        {
            get { return _listaPrzepisowSkladnikaSelection; }
            set
            {
                _listaPrzepisowSkladnikaSelection = value;
                OnPropertyChanged("listaPrzepisowSkladnikaSelection");
            }
        }

        public ObservableCollection<string> cbSzukanieSkladnikowKategorie
        {
            get { return _cbSzukanieSkladnikowKategorie; }

            set
            {
                _cbSzukanieSkladnikowKategorie = value;
                OnPropertyChanged("cbSzukanieSkladnikowKategorie");
            }
        }

        public string cbSzukanieSkladnikowKategorieSelection
        {
            get { return _cbSzukanieSkladnikowKategorieSelection; }
            set
            {
                _cbSzukanieSkladnikowKategorieSelection = value;
                OnPropertyChanged("cbSzukanieSkladnikowKategorieSelection");
            }
        }

        public string opisSkladnika
        {
            get { return _opisSkladnika; }
            set
            {
                _opisSkladnika = value;
                OnPropertyChanged("opisSkladnika");
            }
        }

        public string uwagiSkladnika
        {
            get { return _uwagiSkladnika; }
            set
            {
                _uwagiSkladnika = value;
                OnPropertyChanged("uwagiSkladnika");
            }
        }

        public string kategoriaSkladnika
        {
            get { return _kategoriaSkladnika; }
            set
            {
                _kategoriaSkladnika = value;
                OnPropertyChanged("kategoriaSkladnika");
            }
        }

        public ImageSource zdjecieSkladnika
        {
            get { return _zdjecieSkladnika; }
            set
            {
                _zdjecieSkladnika = value;
                OnPropertyChanged("zdjecieSkladnika");
            }
        }

        public string szukanieSkladnikow
        {
            get { return _szukanieSkladnikow; }
            set
            {
                _szukanieSkladnikow = value;
                OnPropertyChanged("szukanieSkladnika");
                EntityMethods en = new EntityMethods();
                listaSkladnikow = new ObservableCollection<ElementGrid>();
                foreach (Skladniki s in en.listaSkladnikow(value))
                {
                    listaSkladnikow.Add(new ElementGrid { ID = s.ID_SKLADNIKA, NAZWA = s.NAZWA });
                }
                if (listaSkladnikow.Count > 0) listaSkladnikowSelection = listaSkladnikow.ElementAt(0);
            }
        }

        public Boolean widocznoscGridWyszukiwanieSkladnikow
        {
            get { return _widocznoscGridWyszukiwanieSkladnikow; }
            set
            {
                _widocznoscGridWyszukiwanieSkladnikow = value;
                OnPropertyChanged("widocznoscGridWyszukiwanieSkladnikow");
            }
        }

        public string btnSzukanieSkladnikow
        {
            get { return _btnSzukanieSkladnikow; }
            set
            {
                _btnSzukanieSkladnikow = value;
                OnPropertyChanged("btnSzukanieSkladnikow");
            }
        }

        public Boolean txbSzukanieSkladnikowAktywnosc
        {
            get { return _txbSzukanieSkladnikowAktywnosc; }
            set
            {
                _txbSzukanieSkladnikowAktywnosc = value;
                OnPropertyChanged("txbSzukanieSkladnikowAktywnosc");
            }
        }

        public string szukanieSkladnikowNazwa
        {
            get { return _szukanieSkladnikowNazwa; }
            set
            {
                _szukanieSkladnikowNazwa = value;
                OnPropertyChanged("szukanieSkladnikowNazwa");
            }
        }

        public string szukanieSkladnikowOpis
        {
            get { return _szukanieSkladnikowOpis; }
            set
            {
                _szukanieSkladnikowOpis = value;
                OnPropertyChanged("szukanieSkladnikowOpis");
            }
        }

        public string szukanieSkladnikowUwagi
        {
            get { return _szukanieSkladnikowUwagi; }
            set
            {
                _szukanieSkladnikowUwagi = value;
                OnPropertyChanged("szukanieSkladnikowUwagi");
            }
        }

        public ICommand rozwinSzukanieSkladnikow
        {
            get
            {
                return _rozwinSzukanieSkladnikow ?? (_rozwinSzukanieSkladnikow = new RelayCommand(() => rozwinSzukanieSkladnikowAkcja(), null));
            }
        }

        private void rozwinSzukanieSkladnikowAkcja()
        {
            if (!String.IsNullOrWhiteSpace(szukanieSkladnikow)) szukanieSkladnikow = String.Empty;

            if (widocznoscGridWyszukiwanieSkladnikow)
            {
                wypiszSkladniki(listaSkladnikowSelection.ID);
                widocznoscGridWyszukiwanieSkladnikow = false;
                btnSzukanieSkladnikow = "Rozwiń zaawansowane wyszukiwanie";
                txbSzukanieSkladnikowAktywnosc = true;
            }
            else
            {
                widocznoscGridWyszukiwanieSkladnikow = true;
                btnSzukanieSkladnikow = "Zwiń zaawansowane wyszukiwanie";
                txbSzukanieSkladnikowAktywnosc = false;
            }
        }

        public ICommand szukajSkladniki
        {
            get
            {
                return _szukajSkladniki ?? (_szukajSkladniki = new RelayCommand(() => szukajSkladnikiAkcja(), null));
            }
        }

        private void szukajSkladnikiAkcja()
        {
            listaSkladnikow = new ObservableCollection<ElementGrid>();
            EntityMethods en = new EntityMethods();
            int idKategorii = 0;

            if (cbSzukanieSkladnikowKategorie.IndexOf(cbSzukanieSkladnikowKategorieSelection) != 0)
            {
                idKategorii = en.idKategoriiSkladnika(cbSzukanieSkladnikowKategorieSelection);
            }
            List<Skladniki> lista = en.listaSkladnikow(szukanieSkladnikowNazwa, szukanieSkladnikowOpis, szukanieSkladnikowUwagi, idKategorii);
            foreach(Skladniki s in lista)
            {
                listaSkladnikow.Add(new ElementGrid { ID = s.ID_SKLADNIKA, NAZWA = s.NAZWA });
            }
            if (listaSkladnikow.Count > 0) listaSkladnikowSelection = listaSkladnikow.ElementAt(0);
        }

        public ICommand znajdzPrzepisSkladnika
        {
            get
            {
                return _znajdzPrzepisSkladnika ?? (_znajdzPrzepisSkladnika = new RelayCommand(() => znajdzPrzepisSkladnikaAkcja(), null));
            }
        }

        private void znajdzPrzepisSkladnikaAkcja()
        {
            ElementGrid e = listaPrzepisowSkladnikaSelection;
            int id = e.ID;
            tabControlIndex = 0;
            wypiszPrzepisy(id);
        }

        public ICommand szukajSkladnikaAlma
        {
            get
            {
                return _szukajSkladnikaAlma ?? (_szukajSkladnikaAlma = new RelayCommand(() => szukajSkladnikaAlmaAkcja(), null));
            }
        }

        private void szukajSkladnikaAlmaAkcja()
        {
            string selection = listaSkladnikowSelection.NAZWA;
            System.Diagnostics.Process.Start("http://alma24.pl/szukaj?search=" + selection);
        }

        public ICommand szukajSkladnikaGoogle
        {
            get
            {
                return _szukajSkladnikaGoogle ?? (_szukajSkladnikaGoogle = new RelayCommand(() => szukajSkladnikaGoogleAkcja(), null));
            }
        }

        private void szukajSkladnikaGoogleAkcja()
        {
            string selection = listaSkladnikowSelection.NAZWA;
            System.Diagnostics.Process.Start("http://www.google.pl/search?q=" + selection + "&ie=utf-8&oe=utf-8");
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
            MessageBoxShow mb = new MessageBoxShow();
            EntityMethods en = new EntityMethods();
            if (en.skladnikMaPowiazania(listaSkladnikowSelection.ID))
            {
                mb.pokazMessageBox("Nie możesz usunąć tego składnika", "Wybrany składnik jest używany. Aby usunąć ten składnik, najpierw usuń przepisy, w których się znajduje.", MessageBoxTyp.Normalny, MessageBoxIkona.Informacyjna);
            }
            else
            {
                MessageBoxWynik w = mb.pokazMessageBox("Potwierdzenie", "Czy na pewno chcesz usunąć zaznaczony składnik?", MessageBoxTyp.TakNie, MessageBoxIkona.Pytanie);
                if (w == MessageBoxWynik.Tak)
                {
                    en.usunSkladnik(listaSkladnikowSelection.ID);
                    int selectIndex = 0;
                    if (listaSkladnikow.Count > 2 && listaSkladnikow.IndexOf(listaSkladnikowSelection) > 0) selectIndex = listaSkladnikow.IndexOf(listaSkladnikowSelection) - 1;
                    wypiszSkladniki(null);
                    if (listaSkladnikow.Count > 0) listaSkladnikowSelection = listaSkladnikow.ElementAt(selectIndex);
                    mb.pokazMessageBox("Informacja", "Składnik został pomyślnie usunięty.", MessageBoxTyp.Normalny, MessageBoxIkona.Informacyjna);
                }
            }
        }

        #endregion

    }
}
