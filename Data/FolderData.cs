using System;
using System.Data.Odbc;
using Entities;
using System.Collections.Generic;

namespace Data
{
    public class FolderData : ConexionOdbc
    {
   
        /// <summary>
        /// Consultar todos los registros o el registro que vaya en el parametro
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public List<Folder> ConsultarFolder()
        {
            OdbcCommand odbcCommand =  GetOdbcCommandInstance("Select * from folder");
                      
            List<Folder> listaRegistros = new List<Folder>();
            Folder registro = null;

            using (OdbcDataReader reader = odbcCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    registro = new Folder();
                    registro.FolderName = (string)reader["FOLDER"];
                    registro.Fecha = (DateTime)reader["FECHA"];                    
                    listaRegistros.Add(registro);
                }
            }
            DisposeCommand(odbcCommand);

            return listaRegistros;
        }

    }
}
