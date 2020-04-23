using System;

namespace Entities
{
    public class Filtro
    {
        public string FolderName { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaString { get { return string.Format("{0:dd/MM/yyyy}", Fecha ); } }       
    }
}
