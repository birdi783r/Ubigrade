using System;
using System.Collections.Generic;
using System.Text;

namespace Ubigrade.Library.Models
{
    public class NotenDLModel
    {
        public int NId { get; set; }
        public int Bezeichnung { get; set; }
        public int Mindestanforderung { get; set; }

        public NotenDLModel()
        { }

        public NotenDLModel(int nid, int bez, int minanf)
        {
            NId = nid;
            Bezeichnung = bez;
            Mindestanforderung = minanf;
        }
    }
}
