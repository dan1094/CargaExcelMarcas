using System;
using System.Data.SqlClient;
using Entities;
using System.Collections.Generic;

namespace Data
{
    public class MarcasData : Conexion
    {
        /// <summary>
        /// Insertar nuevo registro
        /// </summary>
        /// <param name="registro"></param>

        public void InsertarRegistroMarca(Record registro)
        {
            SqlCommand sqlcommand = GetSqlCommandInstance("[SPT_MARCAS]");
            sqlcommand.Parameters.AddWithValue("@MARCA", registro.Marca);
            sqlcommand.Parameters.AddWithValue("@FONEMA", registro.Fonema);
            sqlcommand.Parameters.AddWithValue("@NOGACETA", registro.Nogaceta);
            sqlcommand.Parameters.AddWithValue("@FGACETA", registro.Fgaceta);
            sqlcommand.Parameters.AddWithValue("@CODIGO_CLASE", registro.Codigo_clase);
            sqlcommand.Parameters.AddWithValue("@FPRESENTA", registro.Fpresenta);
            sqlcommand.Parameters.AddWithValue("@NOPUB", registro.Nopub);
            sqlcommand.Parameters.AddWithValue("@NOEXP", registro.Noexp);
            sqlcommand.Parameters.AddWithValue("@SOLICITANT", registro.Solicitant);
            sqlcommand.Parameters.AddWithValue("@CODIGO_PAIS", registro.Codigo_pais);
            sqlcommand.Parameters.AddWithValue("@APODERADO", registro.Apoderado);
            sqlcommand.Parameters.AddWithValue("@TIPO", registro.Tipo);
            sqlcommand.Parameters.AddWithValue("@FDIGITACIO", registro.Fdigitacio);

            sqlcommand.Parameters.AddWithValue("@OPERACION", 0);

            ScalarSQLCommand(sqlcommand);
        }

        /// <summary>
        /// Limpiar la tabla MARCAS
        /// </summary>
        public void BorrarTablaBD()
        {
            SqlCommand sqlcommand = GetSqlCommandInstance("[SPT_MARCAS]");
            sqlcommand.Parameters.AddWithValue("@OPERACION", 1);

            ScalarSQLCommand(sqlcommand);
        }

        /// <summary>
        /// Consultar todos los registros o el registro que vaya en el parametro
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public List<Record> ConsultarBDMarcas(string filtro)
        {
            SqlCommand sqlcommand = GetSqlCommandInstance("[SPT_MARCAS]");


            if (string.IsNullOrEmpty(filtro))
                sqlcommand.Parameters.AddWithValue("@OPERACION", 3); //Todos
            else
            {
                sqlcommand.Parameters.AddWithValue("@MARCA", string.Format("%{0}%", filtro.Replace(" ", "%")));
                sqlcommand.Parameters.AddWithValue("@OPERACION", 2);// por filtr
            }

            List<Record> listaRegistros = new List<Record>();
            Record registro = null;

            using (SqlDataReader reader = sqlcommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    registro = new Record();
                    registro.Marca = (string)reader["Marca"];
                    registro.Fonema = (string)reader["Fonema"];
                    registro.Nogaceta = (int)reader["Nogaceta"];
                    registro.Fgaceta = (DateTime)reader["Fgaceta"];
                    registro.Codigo_clase = (int)reader["Codigo_clase"];
                    registro.Fpresenta = (DateTime)reader["Fpresenta"];
                    registro.Nopub = (int)reader["Nopub"];
                    registro.Noexp = (int)reader["Noexp"];
                    registro.Solicitant = (string)reader["Solicitant"];
                    registro.Codigo_pais = (int)reader["Codigo_pais"];
                    registro.Apoderado = (string)reader["Apoderado"];
                    registro.Tipo = (string)reader["Tipo"];
                    registro.Fdigitacio = (DateTime)reader["Fdigitacio"];
                    listaRegistros.Add(registro);
                }
            }
            DisposeCommand(sqlcommand);

            return listaRegistros;
        }
               

    }
}
