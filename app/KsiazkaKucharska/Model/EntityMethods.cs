using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsiazkaKucharska.Model
{
    public class EntityMethods : IDisposable
    {
        #region Konstruktor

        private KsiazkaKucharskaConnection context = null;

        public EntityMethods()
        {
            context = new KsiazkaKucharskaConnection();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        #endregion 

        #region Metody przepisów

        public Przepisy pobierzPrzepis(int id)
        {
            return context.Przepisy.Where(x => x.ID_PRZEPISU == id).FirstOrDefault();
        }

        public List<Przepisy> listaPrzepisow()
        {
            return context.Przepisy.OrderBy(a => a.NAZWA).ToList();
        }

        public List<Przepisy> listaPrzepisow(string nazwa)
        {
            var query = from przepis in context.Przepisy
                        where przepis.NAZWA.Contains(nazwa)
                        orderby przepis.NAZWA
                        select przepis;
            return query.ToList();
        }

        public List<Przepisy> listaPrzepisow(string nazwa, string opis, string notatki, int kategoria, int iloscPorcji, int cbIloscPorcjiSelection, int czasPrzyrzadzenia, int cbCzasPrzyrzadzeniaSelection)
        {
            var query = from przepis in context.Przepisy
                        select przepis;
            if (!(String.IsNullOrWhiteSpace(nazwa)))
            {
                query = query.Where(a => a.NAZWA.Contains(nazwa));
            }
            if (!(String.IsNullOrWhiteSpace(opis)))
            {
                query = query.Where(a => a.OPIS.Contains(opis));
            }
            if (!(String.IsNullOrWhiteSpace(notatki)))
            {
                query = query.Where(a => a.NOTATKI.Contains(notatki));
            }
            if (kategoria > 0)
            {
                query = query.Where(a => a.ID_KATEGORII == kategoria);
            }
            if (iloscPorcji > 0)
            {
                if (cbIloscPorcjiSelection == 0) query = query.Where(a => a.ILOSC_PORCJI < iloscPorcji);
                else if (cbIloscPorcjiSelection == 1) query = query.Where(a => a.ILOSC_PORCJI == iloscPorcji);
                else if (cbIloscPorcjiSelection == 2) query = query.Where(a => a.ILOSC_PORCJI > iloscPorcji);
            }
            if (czasPrzyrzadzenia > 0)
            {
                if (cbCzasPrzyrzadzeniaSelection == 0) query = query.Where(a => a.CZAS_PRZYRZADZENIA < czasPrzyrzadzenia);
                else if (cbCzasPrzyrzadzeniaSelection == 1) query = query.Where(a => a.CZAS_PRZYRZADZENIA == czasPrzyrzadzenia);
                else if (cbCzasPrzyrzadzeniaSelection == 2) query = query.Where(a => a.CZAS_PRZYRZADZENIA > czasPrzyrzadzenia);
            }
            query = query.OrderBy(a => a.NAZWA);
            return query.ToList();
        }

        public string opisPrzepisu(int id)
        {
            return context.Przepisy.Where(x => x.ID_PRZEPISU == id).Select(x => x.OPIS).FirstOrDefault();
        }

        public string notatkiPrzepisu(int id)
        {
            return context.Przepisy.Where(x => x.ID_PRZEPISU == id).Select(x => x.NOTATKI).FirstOrDefault();
        }

        public double? porcjePrzepisu(int id)
        {
            return context.Przepisy.Where(x => x.ID_PRZEPISU == id).Select(x => x.ILOSC_PORCJI).FirstOrDefault();
        }

        public int? czasPrzepisu(int id)
        {
            return context.Przepisy.Where(x => x.ID_PRZEPISU == id).Select(x => x.CZAS_PRZYRZADZENIA).FirstOrDefault();
        }

        public byte[] zdjeciePrzepisu(int id)
        {
            return context.Przepisy.Where(x => x.ID_PRZEPISU == id).Select(x => x.ZDJECIE).FirstOrDefault();
        }

        public string kategoriaPrzepisu(int id)
        {
            var query = from przepis in context.Przepisy
                        where przepis.ID_PRZEPISU == id
                        let kat = przepis.Kategorie_Przepisy
                        select kat.P_NAZWA;

            return query.FirstOrDefault();
        }

        public int idKategoriiPrzepisu(string nazwa)
        {
            var query = from przepis in context.Kategorie_Przepisy
                        where przepis.P_NAZWA == nazwa
                        select przepis.P_ID_KATEGORII;

            return query.FirstOrDefault();
        }

        public List<Kategorie_Przepisy> listaKategoriiPrzepisow()
        {
            return context.Kategorie_Przepisy.OrderBy(a => a.P_NAZWA).ToList();
        }

        public List<SkladnikPrzepisu> skladnikiPrzepisu(int id)
        {
            var query = from przepis in context.Przepisy
                        where przepis.ID_PRZEPISU == id
                        from link in przepis.Przepisy_Skladniki
                        let skladnik = link.Skladniki
                        orderby skladnik.NAZWA
                        select new SkladnikPrzepisu
                        {
                            ID = skladnik.ID_SKLADNIKA,
                            NAZWA = skladnik.NAZWA,
                            ILOSC = link.ILOSC,
                            JEDNOSTKA = link.Jednostki.NAZWA
                        };      
            return query.ToList();
        }

        public int dodajPrzepis(Przepisy p)
        {
            context.Przepisy.Add(p);
            context.SaveChanges();
            return p.ID_PRZEPISU;
        }

        public void edytujPrzepis(Przepisy p)
        {
            Przepisy org = context.Przepisy.First(x => x.ID_PRZEPISU == p.ID_PRZEPISU);
            org.NAZWA = p.NAZWA;
            org.OPIS = p.OPIS;
            org.NOTATKI = p.NOTATKI;
            org.CZAS_PRZYRZADZENIA = p.CZAS_PRZYRZADZENIA;
            org.ILOSC_PORCJI = p.ILOSC_PORCJI;
            if (p.ZDJECIE != null) org.ZDJECIE = p.ZDJECIE;
            org.ID_KATEGORII = p.ID_KATEGORII;
            context.SaveChanges();
        }

        public void usunPrzepis(int id)
        {
            czyscPrzepisySkladniki(id);
            Przepisy p = context.Przepisy.First(x => x.ID_PRZEPISU == id);
            context.Przepisy.Remove(p);
            context.SaveChanges();  
        }

        public void czyscPrzepisySkladniki(int idPrzepisu)
        {
            foreach (Przepisy_Skladniki ps in context.Przepisy_Skladniki.Where(x => x.L_ID_PRZEPISU == idPrzepisu).ToList())
            {
                context.Przepisy_Skladniki.Remove(ps);
            }
            context.SaveChanges();
        }

        public void dodajPrzepisySkladniki(Przepisy_Skladniki ps)
        {
            context.Przepisy_Skladniki.Add(ps);
            context.SaveChanges();
        }

        #endregion

        #region Metody składników

        public Skladniki pobierzSkladnik(int id)
        {
            return context.Skladniki.Where(x => x.ID_SKLADNIKA == id).FirstOrDefault();
        }

        public List<Skladniki> listaSkladnikow()
        {
            return context.Skladniki.OrderBy(a => a.NAZWA).ToList();
        }

        public List<Skladniki> listaSkladnikow(string nazwa)
        {
            var query = from skladnik in context.Skladniki
                        where skladnik.NAZWA.Contains(nazwa)
                        orderby skladnik.NAZWA
                        select skladnik;
            return query.ToList();
        }

        public List<Skladniki> listaSkladnikow(string nazwa, string opis, string uwagi, int kategoria)
        {
            var query = from skladnik in context.Skladniki
                        select skladnik;
            if (!(String.IsNullOrWhiteSpace(nazwa)))
            {
                query = query.Where(a => a.NAZWA.Contains(nazwa));
            }
            if (!(String.IsNullOrWhiteSpace(opis)))
            {
                query = query.Where(a => a.OPIS.Contains(opis));
            }
            if (!(String.IsNullOrWhiteSpace(uwagi)))
            {
                query = query.Where(a => a.UWAGI.Contains(uwagi));
            }
            if (kategoria > 0)
            {
                query = query.Where(a => a.ID_KATEGORII == kategoria);
            }
            query = query.OrderBy(a => a.NAZWA);
            return query.ToList();
        }

        public List<Kategorie_Skladniki> listaKategoriiSkladnikow()
        {
            return context.Kategorie_Skladniki.OrderBy(a => a.S_NAZWA).ToList();
        }

        public string opisSkladnika(int id)
        {
            return context.Skladniki.Where(x => x.ID_SKLADNIKA == id).Select(x => x.OPIS).FirstOrDefault();
        }

        public string uwagiSkladnika(int id)
        {
            return context.Skladniki.Where(x => x.ID_SKLADNIKA == id).Select(x => x.UWAGI).FirstOrDefault();
        }

        public int idSkladnika(string nazwa)
        {
            return context.Skladniki.Where(x => x.NAZWA == nazwa).Select(x => x.ID_SKLADNIKA).First();
        }

        public string kategoriaSkladnika(int id)
        {
            var query = from skladnik in context.Skladniki
                        where skladnik.ID_SKLADNIKA == id
                        let kat = skladnik.Kategorie_Skladniki
                        select kat.S_NAZWA;

            return query.FirstOrDefault();
        }

        public int idKategoriiSkladnika(string nazwa)
        {
            var query = from kategoria in context.Kategorie_Skladniki
                        where kategoria.S_NAZWA == nazwa
                        select kategoria.S_ID_KATEGORII;

            return query.FirstOrDefault();
        }

        public byte[] zdjecieSkladnika(int id)
        {
            return context.Skladniki.Where(x => x.ID_SKLADNIKA == id).Select(x => x.ZDJECIE).FirstOrDefault();
        }

        public List<ElementGrid> przepisySkladnika(int id)
        {
            var query = from skladnik in context.Skladniki
                        where skladnik.ID_SKLADNIKA == id
                        from link in skladnik.Przepisy_Skladniki
                        let przepis = link.Przepisy
                        orderby przepis.NAZWA
                        select new ElementGrid
                        {
                            ID = przepis.ID_PRZEPISU,
                            NAZWA = przepis.NAZWA,
                        };
            return query.ToList();
        }

        public int dodajSkladnik(Skladniki s)
        {
            context.Skladniki.Add(s);
            context.SaveChanges();
            return s.ID_SKLADNIKA;
        }

        public void edytujSkladnik(Skladniki s)
        {
            Skladniki org = context.Skladniki.First(x => x.ID_SKLADNIKA == s.ID_SKLADNIKA);
            org.NAZWA = s.NAZWA;
            org.OPIS = s.OPIS;
            org.UWAGI = s.UWAGI;
            if (s.ZDJECIE != null) org.ZDJECIE = s.ZDJECIE;
            org.ID_KATEGORII = s.ID_KATEGORII;
            context.SaveChanges();
        }

        public void usunSkladnik(int id)
        {
            Skladniki s = context.Skladniki.First(x => x.ID_SKLADNIKA == id);
            context.Skladniki.Remove(s);
            context.SaveChanges();
        }

        public bool skladnikMaPowiazania(int id)
        {
            int liczbaPowiazan = (from przepisySkladniki in context.Przepisy_Skladniki
                                  where przepisySkladniki.L_ID_SKLADNIKA == id
                                  select przepisySkladniki).Count();
            if (liczbaPowiazan > 0) return true;
            else return false;
        }

        #endregion

        #region Metody jednostek

        public List<Jednostki> listaJednostek()
        {
            return context.Jednostki.OrderBy(x => x.NAZWA).ToList();
        }

        public Boolean czyJednostkaIstnieje(string nazwa)
        {
            var query = from jednostka in context.Jednostki
                        where jednostka.NAZWA == nazwa
                        select jednostka.ID_JEDNOSTKI;
            if (query.FirstOrDefault() == 0) return false;
            else return true;
        }

        public int dodajJednostke(string nazwa)
        {
            Jednostki j = new Jednostki();
            j.NAZWA = nazwa;
            context.Jednostki.Add(j);
            context.SaveChanges();
            return j.ID_JEDNOSTKI;
        }

        public string pobierzNazweJednostki(int id)
        {
            var query = from jednostka in context.Jednostki
                        where jednostka.ID_JEDNOSTKI == id
                        select jednostka.NAZWA;
            return query.First();
        }

        public int pobierzIdJednostki(string nazwa)
        {
            var query = from jednostka in context.Jednostki
                        where jednostka.NAZWA == nazwa
                        select jednostka.ID_JEDNOSTKI;
            return query.First();
        }

        public int liczbaPowiazanJednostki(int id)
        {
            return (from ps in context.Przepisy_Skladniki
                         where ps.ID_JEDNOSTKI == id
                         select ps.ID_JEDNOSTKI).Count();
        }

        public void usunJednostke(string nazwa)
        {
            Jednostki j = (from jednostka in context.Jednostki
                         where jednostka.NAZWA == nazwa
                         select jednostka).First();

            context.Jednostki.Remove(j);
            context.SaveChanges();
        }

        public void edytujJednostke(string nazwa, string nowaNazwa)
        {
            Jednostki org = context.Jednostki.First(x => x.NAZWA == nazwa);
            org.NAZWA = nowaNazwa;
            context.SaveChanges();
        }

        #endregion

        #region Metody kategorii

        public int dodajKategoriePrzepisow(string nazwa)
        {
            Kategorie_Przepisy kp = new Kategorie_Przepisy();
            kp.P_NAZWA = nazwa;
            context.Kategorie_Przepisy.Add(kp);
            context.SaveChanges();
            return kp.P_ID_KATEGORII;
        }

        public int dodajKategorieSkladnikow(string nazwa)
        {
            Kategorie_Skladniki ks = new Kategorie_Skladniki();
            ks.S_NAZWA = nazwa;
            context.Kategorie_Skladniki.Add(ks);
            context.SaveChanges();
            return ks.S_ID_KATEGORII;
        }

        public void edytujKategorieSkladnikow(string edytowanaKategoria, string nazwa)
        {
            Kategorie_Skladniki org = context.Kategorie_Skladniki.First(x => x.S_NAZWA == edytowanaKategoria);
            org.S_NAZWA = nazwa;
            context.SaveChanges();
        }

        public void edytujKategoriePrzepisow(string edytowanaKategoria, string nazwa)
        {
            Kategorie_Przepisy org = context.Kategorie_Przepisy.First(x => x.P_NAZWA == edytowanaKategoria);
            org.P_NAZWA = nazwa;
            context.SaveChanges();
        }

        public int liczbaPowiazanKategoriiSkladnika(string nazwa)
        {
            int id = context.Kategorie_Skladniki.First(x => x.S_NAZWA == nazwa).S_ID_KATEGORII;
            return (from ks in context.Skladniki
                    where ks.ID_KATEGORII == id
                    select ks.ID_KATEGORII).Count();
        }

        public int liczbaPowiazanKategoriiPrzepisu(string nazwa)
        {
            int id = context.Kategorie_Przepisy.First(x => x.P_NAZWA == nazwa).P_ID_KATEGORII;
            return (from kp in context.Przepisy
                    where kp.ID_KATEGORII == id
                    select kp.ID_KATEGORII).Count();
        }

        public void usunKategorieSkladnikow(string nazwa)
        {
            Kategorie_Skladniki ks = context.Kategorie_Skladniki.First(x => x.S_NAZWA == nazwa);
            context.Kategorie_Skladniki.Remove(ks);
            context.SaveChanges();
        }

        public void usunKategoriePrzepisow(string nazwa)
        {
            Kategorie_Przepisy kp = context.Kategorie_Przepisy.First(x => x.P_NAZWA == nazwa);
            context.Kategorie_Przepisy.Remove(kp);
            context.SaveChanges();
        }

        #endregion
    }
}
