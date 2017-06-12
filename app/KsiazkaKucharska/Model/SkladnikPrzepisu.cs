using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsiazkaKucharska.Model
{
    public class SkladnikPrzepisu
    {
        public int ID { get; set; }
        public string NAZWA { get; set; }
        public double? ILOSC { get; set; }
        public string JEDNOSTKA { get; set; }
    }
}
