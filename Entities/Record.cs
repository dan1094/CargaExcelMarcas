using System;

namespace Entities
{
    public class Record
    {
        public string Marca { get; set; }
        public string Fonema { get; set; }
        public int Nogaceta { get; set; }
        public DateTime Fgaceta { get; set; }
        public string FgacetaString { get { return string.Format("{0:dd/MM/yyyy}", Fgaceta); } }
        public int Codigo_clase { get; set; }
        public DateTime Fpresenta { get; set; }
        public string FpresentaString { get { return string.Format("{0:dd/MM/yyyy}", Fpresenta); } }
        public int Nopub { get; set; }
        public int Noexp { get; set; }
        public string Solicitant { get; set; }
        public int Codigo_pais { get; set; }
        public string Apoderado { get; set; }
        public string Tipo { get; set; }
        public DateTime Fdigitacio { get; set; }
        public string FdigitacioString { get { return string.Format("{0:dd/MM/yyyy}", Fdigitacio); } }
    }
}
