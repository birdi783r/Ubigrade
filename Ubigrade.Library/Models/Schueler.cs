using System;
using System.Collections.Generic;
using System.Text;

namespace Ubigrade.Library.Models
{
    public class SchuelerDLModel
    {
        public int Schuelerkennzahl { get; set; }
        public int Checkpersonnumber { get; set; }
        public string NName { get; set; }
        public string VName { get; set; }
        public string Geschlecht { get; set; }
        public string EmailAdresse { get; set; }
        public int Schuljahr { get; set; }
        public List<FaecherDLModel> Faecher { get; set; }


        public SchuelerDLModel()
        { }

        public SchuelerDLModel(int skennzahl, int cprnr, string nn, string vn, string g, string e, int sj)
        {
            Schuelerkennzahl = skennzahl;
            Checkpersonnumber = cprnr;
            NName = nn;
            VName = vn;
            Geschlecht = g;
            EmailAdresse = e;
            Schuljahr = sj;
        }
        public SchuelerDLModel(int skennzahl, int cprnr, string nn, string vn, string g, string e, int sj, List<FaecherDLModel> faech)
        {
            Schuelerkennzahl = skennzahl;
            Checkpersonnumber = cprnr;
            NName = nn;
            VName = vn;
            Geschlecht = g;
            EmailAdresse = e;
            Schuljahr = sj;
            Faecher = faech;
        }
    }
}
