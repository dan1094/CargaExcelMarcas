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
    public class FolderRule : IDisposable
    {
        public List<Folder> ConsultarFolder()
        {
            using (FolderData data = new FolderData())
            {
                return data.ConsultarFolder();
            }
        }

        public void Dispose()
        { }
    }
}
