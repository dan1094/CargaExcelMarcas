using System;
using System.Data.SqlClient;
using Entities;
using System.Collections.Generic;

namespace Data
{
    public class ComparacionData : Conexion
    {
        /// <summary>
        /// Insertar nuevo registro
        /// </summary>
        /// <param name="registro"></param>

        public void InsertarResultadoComparacion(ResultadoComparacion registro)
        {
            SqlCommand sqlcommand = GetSqlCommandInstance("[SPT_RESULTADO_COMPARACION]");
            sqlcommand.Parameters.AddWithValue("@MARCA", registro.Marca);
            sqlcommand.Parameters.AddWithValue("@FOLDER", registro.Folder);
            sqlcommand.Parameters.AddWithValue("@RESULTADO", registro.Resultado);
            sqlcommand.Parameters.AddWithValue("@FECHA", registro.Fecha);
            sqlcommand.Parameters.AddWithValue("@CLASE", registro.Clase);
            sqlcommand.Parameters.AddWithValue("@GACETA", registro.Gaceta);
            sqlcommand.Parameters.AddWithValue("@OPERACION", 0);

            ScalarSQLCommand(sqlcommand);
        }

        /// <summary>
        /// Limpiar la tabla RESULTADO_COMPARACION
        /// </summary>
        public void BorrarTablaBD()
        {
            SqlCommand sqlcommand = GetSqlCommandInstance("[SPT_RESULTADO_COMPARACION]");
            sqlcommand.Parameters.AddWithValue("@OPERACION", 1);

            ScalarSQLCommand(sqlcommand);
        }

        public List<ResultadoComparacion> ConsultarAgrupaciones()
        {
            SqlCommand sqlcommand = GetSqlCommandInstance("[SPT_RESULTADO_COMPARACION]");


            sqlcommand.Parameters.AddWithValue("@OPERACION", 2); //Todos                    

            List<ResultadoComparacion> listaRegistros = new List<ResultadoComparacion>();
            ResultadoComparacion registro = null;

            using (SqlDataReader reader = sqlcommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    registro = new ResultadoComparacion();
                    registro.Marca = reader.IsDBNull(reader.GetOrdinal("MARCA")) ? string.Empty : (string)reader["MARCA"];
                    registro.Clase = reader.IsDBNull(reader.GetOrdinal("CLASE")) ? 0 : (int)reader["CLASE"];
                    registro.Gaceta = reader.IsDBNull(reader.GetOrdinal("GACETA")) ? 0 : (int)reader["GACETA"];
                    listaRegistros.Add(registro);
                }
            }
            DisposeCommand(sqlcommand);

            return listaRegistros;
        }

        public List<ResultadoComparacion> Consultargrupo(ResultadoComparacion filtro)
        {
            SqlCommand sqlcommand = GetSqlCommandInstance("[SPT_RESULTADO_COMPARACION]");

            sqlcommand.Parameters.AddWithValue("@MARCA", filtro.Marca);
            sqlcommand.Parameters.AddWithValue("@CLASE", filtro.Clase);
            sqlcommand.Parameters.AddWithValue("@GACETA", filtro.Gaceta);
            sqlcommand.Parameters.AddWithValue("@OPERACION", 3); //Todos                   

            List<ResultadoComparacion> listaRegistros = new List<ResultadoComparacion>();
            ResultadoComparacion registro = null;

            using (SqlDataReader reader = sqlcommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    registro = new ResultadoComparacion();
                    registro.Id = (int)reader["ID"];
                    registro.Marca = reader.IsDBNull(reader.GetOrdinal("MARCA")) ? string.Empty : (string)reader["MARCA"];
                    registro.Folder = reader.IsDBNull(reader.GetOrdinal("FOLDER")) ? string.Empty : (string)reader["FOLDER"];
                    registro.Resultado = reader.IsDBNull(reader.GetOrdinal("RESULTADO")) ? 0 : (int)reader["RESULTADO"];
                    registro.Fecha = (DateTime)reader["FECHA"];
                    registro.Clase = reader.IsDBNull(reader.GetOrdinal("CLASE")) ? 0 : (int)reader["CLASE"];
                    registro.Gaceta = reader.IsDBNull(reader.GetOrdinal("CLASE")) ? 0 : (int)reader["GACETA"];
                    listaRegistros.Add(registro);
                }
            }
            DisposeCommand(sqlcommand);

            return listaRegistros;
        }

    }
}
