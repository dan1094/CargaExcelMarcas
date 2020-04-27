using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;

namespace Rule
{
    public class ImpresionRule
    {
        public static void FoliarPdf(string RutaPDf)
        {
            //System.Drawing.Color? color = System.Drawing.Color.Black;
            //FolioPosition folioPosition = FolioPosition.BottomRight;
            //FolioType folioType = FolioType.Numeric;
            string fechaProceso = $"Fecha Proceso: {DateTime.Now.ToString("dd/MM/yyyy")}";
            string folioPrefix = "Pag. ";
            string folioSuffix = "";
            //string folioFont = FontFactory.TIMES_ROMAN.ToString();
            //FolioFace folioFace = FolioFace.Roman;
            float folioSize = 9;
            PdfReader reader = new PdfReader(RutaPDf);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int totalNumberOfPages = reader.NumberOfPages;
                string folioTotalNumber = totalNumberOfPages.ToString();
                PdfStamper pdfStamper = new PdfStamper(reader, memoryStream);
                for (int i = 1; i <= totalNumberOfPages; i++)
                {
                    // skip adding folio if not started yet
                    if (i < 1)
                    {
                        continue;
                    }
                    // page number preparation
                    string folioNumber = i.ToString();
                    string folio = folioPrefix + folioNumber;
                    folio += " de " + folioTotalNumber;
                    folio += folioSuffix;
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    PdfContentByte pdfPageContents = pdfStamper.GetUnderContent(i);
                    // apply style
                    pdfPageContents.BeginText();
                    pdfPageContents.SetFontAndSize(baseFont, folioSize);
                    //pdfPageContents.SetRGBColorFill(Convert.ToInt32(color.Value.R), 
                    //    Convert.ToInt32(color.Value.G), Convert.ToInt32(color.Value.B));


                    // prepare x,y cords 0,0 = bottom left
                    Rectangle pageSize = reader.GetPageSizeWithRotation(i);
                    float strWidth = pdfPageContents.GetEffectiveStringWidth(folio, false);
                    const int padding = 10;
                    float foliox = padding;
                    float folioy = folioSize;
                    foliox = pageSize.Width - strWidth - padding;
                    pdfPageContents.ShowTextAligned(PdfContentByte.ALIGN_LEFT, folio, foliox, folioy, 0);

                    strWidth = pdfPageContents.GetEffectiveStringWidth(fechaProceso, false);
                    pdfPageContents.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, fechaProceso, padding + strWidth, folioy, 0);

                    pdfPageContents.EndText();
                }
                pdfStamper.Close();
                reader.Close();
                File.WriteAllBytes(RutaPDf, memoryStream.ToArray());

            }

        }

        
    }
}
