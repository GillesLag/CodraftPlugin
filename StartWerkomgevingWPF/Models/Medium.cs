using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StartWerkomgevingWPF.Models
{
    public class Medium
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Fabrikant { get; set; }
        public string Kleur { get; set; }
        public List<byte> RGBKleur { get; set; }
        public string Naam { get; set; }
        public string ExtraInfo { get; set; }
        public string OrgineleNaam { get; set; }
        public string Afkorting { get; set; }
        public double NominaleDiameter1 { get; set; }
        public double NominaleDiameter2 { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Medium medium &&
                   OrgineleNaam == medium.OrgineleNaam
                   && Id == medium.Id;
        }

        public override int GetHashCode()
        {
            return -1386946022 + EqualityComparer<string>.Default.GetHashCode(Naam);
        }
    }
}
