using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace ProyectoConjuntoMaterialPrestamo
{
    public static class Configuracion
    {
        public static string Cadena =
            @"Server=79.143.95.126,54321;" +
            "Database=MaterialPrestamo;" +
            "User ID=sa;" +
            "Password=Sql#123456789;" +
            "Encrypt=True;TrustServerCertificate=True;";


        public static void OpenDB()
        {
            try
            {
                using var cn = new SqlConnection(Configuracion.Cadena);
                cn.Open();
                Console.WriteLine("Conexión abierta: " + cn.Database);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("No se ha podido conectar.");
                Console.WriteLine(ex.Message);
            }
        }

        public static string? ReturnDB()
        {
            using var cn = new SqlConnection(Configuracion.Cadena);
            cn.Open();
            using var cmd = new SqlCommand("SELECT DB_NAME();", cn);
            string? baseActual = cmd.ExecuteScalar()?.ToString();
            Console.WriteLine("Base actual: " + baseActual);

            return baseActual;
        }

    }
}
