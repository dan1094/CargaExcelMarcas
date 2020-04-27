using System;
using Data;
using Entities;
using System.Collections.Generic;
using SpreadsheetLight;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Rule
{
    public class MarcasRule : IDisposable
    {
        public void LeerExcel(string fileLocation)
        {
            if (File.Exists(fileLocation))
            {
                SLDocument hoja = new SLDocument(fileLocation);
                SLWorksheetStatistics statics = hoja.GetWorksheetStatistics();

                List<Record> registros = new List<Record>();
                Record reg = null;

                for (int i = 2; i < statics.EndRowIndex + 1; i++)
                {
                    if (string.IsNullOrEmpty(hoja.GetCellValueAsString(i, 1)) &&
                        string.IsNullOrEmpty(hoja.GetCellValueAsString(i, 2)) &&
                        string.IsNullOrEmpty(hoja.GetCellValueAsString(i, 3)))
                    {
                        break;
                    }
                    reg = new Record();
                    reg.Marca = hoja.GetCellValueAsString(i, 1);
                    reg.Fonema = hoja.GetCellValueAsString(i, 2);
                    reg.Nogaceta = hoja.GetCellValueAsInt32(i, 3);
                    reg.Fgaceta = hoja.GetCellValueAsDateTime(i, 4);
                    reg.Codigo_clase = hoja.GetCellValueAsInt32(i, 5);
                    reg.Fpresenta = hoja.GetCellValueAsDateTime(i, 6);
                    reg.Nopub = hoja.GetCellValueAsInt32(i, 7);
                    reg.Noexp = hoja.GetCellValueAsInt32(i, 8);
                    reg.Solicitant = hoja.GetCellValueAsString(i, 9);
                    reg.Codigo_pais = hoja.GetCellValueAsInt32(i, 10);
                    reg.Apoderado = hoja.GetCellValueAsString(i, 11);
                    reg.Tipo = hoja.GetCellValueAsString(i, 12);
                    reg.Fdigitacio = hoja.GetCellValueAsDateTime(i, 13);

                    registros.Add(reg);
                }
                InsertarRegistroEnBD(registros);
                File.Delete(fileLocation);
            }
        }

        public void InsertarRegistroEnBD(List<Record> registros)
        {
            using (MarcasData data = new MarcasData())
            {
                foreach (var item in registros)
                {
                    data.InsertarRegistroMarca(item);
                }
            }
        }

        public List<Record> ConsultarMarcas(string marca)
        {
            using (MarcasData data = new MarcasData())
            {
                return data.ConsultarBDMarcas(marca);
            }
        }
        public List<ResultadoComparacion> ConsultarGrupos()
        {
            using (ComparacionData data = new ComparacionData())
            {
                return data.ConsultarAgrupaciones();
            }
        }

        public List<ResultadoComparacion> ConsultarGrupo(ResultadoComparacion filtro)
        {
            using (ComparacionData data = new ComparacionData())
            {
                return data.Consultargrupo(filtro);
            }
        }
        public void BorrarTablaMarcas()
        {
            using (MarcasData data = new MarcasData())
                data.BorrarTablaBD();

            using (FiltroRule r = new FiltroRule())
                r.BorrarTablaFiltro();

            using (ComparacionData r = new ComparacionData())
                r.BorrarTablaBD();
        }

        public void RealizarComparacion()
        {
            var marcas = ConsultarMarcas(string.Empty);

            List<Filtro> filtros = null;
            using (FiltroRule r = new FiltroRule())
            {
                filtros = r.ConsultarFiltro();
            }
            if (filtros == null) filtros = new List<Filtro>();
            if (marcas == null) marcas = new List<Record>();

            int resultado = 0;

            using (ComparacionData data = new ComparacionData())
            {
                ResultadoComparacion comparacion = null;
                foreach (var marca in marcas)
                {
                    foreach (var filtro in filtros)
                    {
                        resultado = Comparar(marca.Marca, filtro.FolderName);
                        if (resultado > 0)
                        {
                            comparacion = new ResultadoComparacion();
                            comparacion.Marca = marca.Marca;
                            comparacion.Folder = filtro.FolderName;
                            comparacion.Resultado = resultado;
                            comparacion.Fecha = DateTime.Now;
                            comparacion.Clase = marca.Codigo_clase;
                            comparacion.Gaceta = marca.Nogaceta;

                            data.InsertarResultadoComparacion(comparacion);
                        }
                    }
                }
            }
        }

        private int Comparar(string marca, string folder)
        {
            return 85;
        }

        #region Armar PDF

        public void ArmarPDFMarcas(List<Record> registros, string rutaDepositar)
        {
            FileStream fs = new FileStream(rutaDepositar,
                FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document();
            doc.SetPageSize(PageSize.A4.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();

            //Report Header
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntHead = new Font(bfntHead, 14, 1, BaseColor.BLACK);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk("Marcas".ToUpper(), fntHead));
            doc.Add(prgHeading);

            //Add a line seperation
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK,
                Element.ALIGN_CENTER, 1)));
            doc.Add(p);
            //Add line break
            doc.Add(new Chunk("\n", fntHead));

            //Write the table
            PdfPTable table = new PdfPTable(13);

            //table.HorizontalAlignment = 0;
            table.TotalWidth = 700f;
            table.LockedWidth = true;
            float[] widths = new float[] { 140f, 40f, 30f, 50f, 30f, 50f, 30f, 30f, 80f, 30f, 80f, 40f, 50f };
            table.SetWidths(widths);

            //Table header            
            AddCellHeader(table, "Marca", 9);
            AddCellHeader(table, "Fonema", 9);
            AddCellHeader(table, "No gaceta", 9);
            AddCellHeader(table, "Fecha Gaceta", 9);
            AddCellHeader(table, "Cod. Clase", 9);
            AddCellHeader(table, "Fecha Presenta", 9);
            AddCellHeader(table, "Nopub", 9);
            AddCellHeader(table, "Noexp", 9);
            AddCellHeader(table, "Solicitant", 9);
            AddCellHeader(table, "Cod. País", 9);
            AddCellHeader(table, "Apoderado", 9);
            AddCellHeader(table, "Tipo", 9);
            AddCellHeader(table, "Fecha digitacion", 9);


            foreach (var item in registros)
            {
                AddCellText(table, item.Marca.ToString(), 8);
                AddCellText(table, item.Fonema.ToString(), 8);
                AddCellText(table, item.Nogaceta.ToString(), 8);
                AddCellText(table, item.FgacetaString.ToString(), 8);
                AddCellText(table, item.Codigo_clase.ToString(), 8);
                AddCellText(table, item.FpresentaString.ToString(), 8);
                AddCellText(table, item.Nopub.ToString(), 8);
                AddCellText(table, item.Noexp.ToString(), 8);
                AddCellText(table, item.Solicitant.ToString(), 8);
                AddCellText(table, item.Codigo_pais.ToString(), 8);
                AddCellText(table, item.Apoderado.ToString(), 8);
                AddCellText(table, item.Tipo.ToString(), 8);
                AddCellText(table, item.FdigitacioString.ToString(), 8);
            }

            doc.Add(table);
            doc.Close();
            writer.Close();
            fs.Close();
        }

        private static void AddCellHeader(PdfPTable table, string text, float size)
        {
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntColumnHeader = new Font(btnColumnHeader, size, 1, BaseColor.WHITE);
            PdfPCell cell = new PdfPCell(new Phrase(text, fntColumnHeader));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.BackgroundColor = BaseColor.GRAY;
            table.AddCell(cell);
        }
        private static void AddCellText(PdfPTable table, string text, float size)
        {
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntColumnHeader = new Font(btnColumnHeader, size, Font.ITALIC);
            PdfPCell cell = new PdfPCell(new Phrase(text, fntColumnHeader));
            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cell);
        }

        public void ArmarPDFComparacion(string rutaDepositar)
        {
            List<ResultadoComparacion> grupos = ConsultarGrupos();
            if (grupos == null) grupos = new List<ResultadoComparacion>();

            FileStream fs = new FileStream(rutaDepositar,
                FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document();
            doc.SetPageSize(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();


            //Report Header
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntHead = new Font(bfntHead, 14, 1, BaseColor.BLACK);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk("resultado comparación".ToUpper(), fntHead));
            doc.Add(prgHeading);

            //Add a line seperation
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK,
                Element.ALIGN_CENTER, 1)));
            doc.Add(p);
            //Add line break
            doc.Add(new Chunk("\n", fntHead));


            PdfPTable table = null;
            float[] widths = null;
            List<ResultadoComparacion> registros = null;
            PdfPTable t1 = null;

            Font fnt = new Font(bfntHead, 10, 1, BaseColor.BLACK);
            Paragraph prg = new Paragraph();
            prg.Alignment = Element.ALIGN_LEFT;
            prg.Add(new Chunk($"Fecha Proceso: {DateTime.Now.ToString("MM/dd/yyyy")}", fnt));

            foreach (var grupo in grupos)
            {
                registros = ConsultarGrupo(grupo);
                if (registros == null) registros = new List<ResultadoComparacion>();

                //Write the table
                t1 = new PdfPTable(2);
                t1.TotalWidth = 500f;
                t1.LockedWidth = true;
                widths = new float[] { 100f, 400f };
                t1.SetWidths(widths);
                AddCellText(t1, "Marca:", 15);
                AddCellText(t1, grupo.Marca, 14);
                doc.Add(t1);

                //Write the table
                t1 = new PdfPTable(4);
                t1.TotalWidth = 500f;
                t1.LockedWidth = true;
                widths = new float[] { 60f, 140f, 60f, 140f };
                t1.SetWidths(widths);
                AddCellText(t1, "Clase:", 15);
                AddCellText(t1, grupo.Clase.ToString(), 15);

                AddCellText(t1, "Gaceta N°:", 15);
                AddCellText(t1, grupo.Gaceta.ToString(), 15);
                doc.Add(t1);

                //Write the table
                table = new PdfPTable(3);
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                widths = new float[] { 180f, 180f, 140f };
                table.SetWidths(widths);

                //Table header            
                AddCellHeader(table, "Folder", 12);
                AddCellHeader(table, "Resultado Comparar", 12);
                AddCellHeader(table, "Fecha", 12);

                foreach (var item in registros)
                {
                    AddCellText(table, item.Folder.ToString(), 10);
                    AddCellText(table, item.Resultado.ToString(), 10);
                    AddCellText(table, item.FechaString, 10);
                }
                doc.Add(table);

                doc.Add(new Chunk("\n", fnt));
                doc.Add(prg); // fecha proceso
                doc.Add(Chunk.NEXTPAGE);
            }
            doc.Close();
            writer.Close();
            fs.Close();

            ImpresionRule.FoliarPdf(rutaDepositar);
        }


        public void ArmarExel(List<Record> registros, string rutaDepositar)
        {
            SLDocument sl = new SLDocument();
            sl.SetCellValue(1, 1, "Marca");
            sl.SetCellValue(1, 2, "Fonema");
            sl.SetCellValue(1, 3, "Nogaceta");
            sl.SetCellValue(1, 4, "Fgaceta");
            sl.SetCellValue(1, 5, "Codigo_clase");
            sl.SetCellValue(1, 6, "Fpresenta");
            sl.SetCellValue(1, 7, "Nopub");
            sl.SetCellValue(1, 8, "Noexp");
            sl.SetCellValue(1, 9, "Solicitant");
            sl.SetCellValue(1, 10, "Codigo_pais");
            sl.SetCellValue(1, 11, "Apoderado");
            sl.SetCellValue(1, 12, "Tipo");
            sl.SetCellValue(1, 13, "Fdigitacio");

            int i = 2;
            foreach (var item in registros)
            {
                sl.SetCellValue(i, 1, item.Marca);
                sl.SetCellValue(i, 2, item.Fonema);
                sl.SetCellValue(i, 3, item.Nogaceta);
                sl.SetCellValue(i, 4, item.FgacetaString);
                sl.SetCellValue(i, 5, item.Codigo_clase);
                sl.SetCellValue(i, 6, item.FpresentaString);
                sl.SetCellValue(i, 7, item.Nopub);
                sl.SetCellValue(i, 8, item.Noexp);
                sl.SetCellValue(i, 9, item.Solicitant);
                sl.SetCellValue(i, 10, item.Codigo_pais);
                sl.SetCellValue(i, 11, item.Apoderado);
                sl.SetCellValue(i, 12, item.Tipo);
                sl.SetCellValue(i, 13, item.FdigitacioString);
                i++;
            }
            sl.SaveAs(rutaDepositar);
        }

        #endregion


        public void Dispose()
        { }
    }


}
