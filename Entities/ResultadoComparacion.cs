using System;

namespace Entities
{
    public class ResultadoComparacion
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Folder { get; set; }
        public int Resultado { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaString { get { return string.Format("{0:dd/MM/yyyy}", Fecha); } }
        public int Clase { get; set; }
        public int Gaceta { get; set; }

    }
}
