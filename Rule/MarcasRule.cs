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
                    data.InsertarRegistro(item);
                }
            }
        }

        public List<Record> Consultar(string marca)
        {
            using (MarcasData data = new MarcasData())
            {
                return data.ConsultarBD(marca);
            }
        }

        public void BorrarTablaMarcas()
        {
            using (MarcasData data = new MarcasData())
            {
                data.BorrarTablaBD();
            }
        }

        public void ArmarPDF(List<Record> registros, string rutaDepositar)
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
            AddCellHeader(table, "Marca");
            AddCellHeader(table, "Fonema");
            AddCellHeader(table, "No gaceta");
            AddCellHeader(table, "Fecha Gaceta");
            AddCellHeader(table, "Cod. Clase");
            AddCellHeader(table, "Fecha Presenta");
            AddCellHeader(table, "Nopub");
            AddCellHeader(table, "Noexp");
            AddCellHeader(table, "Solicitant");
            AddCellHeader(table, "Cod. País");
            AddCellHeader(table, "Apoderado");
            AddCellHeader(table, "Tipo");
            AddCellHeader(table, "Fecha digitacion");


            foreach (var item in registros)
            {
                AddCellText(table, item.Marca.ToString());
                AddCellText(table, item.Fonema.ToString());
                AddCellText(table, item.Nogaceta.ToString());
                AddCellText(table, item.FgacetaString.ToString());
                AddCellText(table, item.Codigo_clase.ToString());
                AddCellText(table, item.FpresentaString.ToString());
                AddCellText(table, item.Nopub.ToString());
                AddCellText(table, item.Noexp.ToString());
                AddCellText(table, item.Solicitant.ToString());
                AddCellText(table, item.Codigo_pais.ToString());
                AddCellText(table, item.Apoderado.ToString());
                AddCellText(table, item.Tipo.ToString());
                AddCellText(table, item.FdigitacioString.ToString());
            }

            doc.Add(table);
            doc.Close();
            writer.Close();
            fs.Close();
        }

        private static void AddCellHeader(PdfPTable table, string text)
        {
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntColumnHeader = new Font(btnColumnHeader, 9, 1, BaseColor.WHITE);
            PdfPCell cell = new PdfPCell(new Phrase(text, fntColumnHeader));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.BackgroundColor = BaseColor.GRAY;
            table.AddCell(cell);
        }
        private static void AddCellText(PdfPTable table, string text)
        {
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntColumnHeader = new Font(btnColumnHeader, 8, Font.ITALIC);
            PdfPCell cell = new PdfPCell(new Phrase(text, fntColumnHeader));
            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cell);
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

        public void Dispose()
        { }
    }
}
