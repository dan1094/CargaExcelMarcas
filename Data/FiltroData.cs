using System;
using System.Data.SqlClient;
using Entities;
using System.Collections.Generic;

namespace Data
{
    public class FiltroData : Conexion
    {
        /// <summary>
        /// Insertar nuevo registro
        /// </summary>
        /// <param name="registro"></param>

        public void InsertarRegistro(Filtro registro)
        {
            SqlCommand sqlcommand = GetSqlCommandInstance("[SPT_FILTRO]");
            sqlcommand.Parameters.AddWithValue("@FOLDER", registro.FolderName);
            sqlcommand.Parameters.AddWithValue("@FECHA", registro.Fecha);
            sqlcommand.Parameters.AddWithValue("@OPERACION", 0);

            ScalarSQLCommand(sqlcommand);
        }

        /// <summary>
        /// Limpiar la tabla MARCAS
        /// </summary>
        public void BorrarTablaBD()
        {
            SqlCommand sqlcommand = GetSqlCommandInstance("[SPT_FILTRO]");
            sqlcommand.Parameters.AddWithValue("@OPERACION", 1);

            ScalarSQLCommand(sqlcommand);
        }

        /// <summary>
        /// Consultar todos los registros
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public List<Filtro> ConsultarBD()
        {
            SqlCommand sqlcommand = GetSqlCommandInstance("[SPT_FILTRO]");

            sqlcommand.Parameters.AddWithValue("@OPERACION", 2); //Todos

            List<Filtro> listaRegistros = new List<Filtro>();
            Filtro registro = null;

            using (SqlDataReader reader = sqlcommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    registro = new Filtro();
                    registro.FolderName = (string)reader["Folder"];
                    registro.Fecha = (DateTime)reader["Fecha"];
                    listaRegistros.Add(registro);
                }
            }
            DisposeCommand(sqlcommand);

            return listaRegistros;
        }

    }
}
