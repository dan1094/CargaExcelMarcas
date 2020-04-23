using System;
using Data;
using Entities;
using System.Collections.Generic;
using SpreadsheetLight;
using System.IO;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Rule
{
    public class FiltroRule : IDisposable
    {
        public void InsertarFiltro(Filtro filtro)
        {
            using (FiltroData data = new FiltroData())
            {
                 data.InsertarRegistro(filtro);
            }
        }
        public List<Filtro> ConsultarFiltro()
        {
            using (FiltroData data = new FiltroData())
            {
                return data.ConsultarBD();
            }
        }
        public void BorrarTablaFiltro()
        {
            using (FiltroData data = new FiltroData())
            {
                data.BorrarTablaBD();
            }
        }
        public void Dispose()
        { }
    }
}
