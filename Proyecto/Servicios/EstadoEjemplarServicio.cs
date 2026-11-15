using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    internal class EstadoEjemplarServicio
    {
        public static async Task<List<EstadoEjemplar>> ListarAsync()
            => await EstadoEjemplarEF.ListarAsync();

        public static async Task<EstadoEjemplar?> BuscarAsync(int id)
        {
            ComprobarId(id);
            return await EstadoEjemplarEF.BuscarAsync(id);
        }

        private static void ComprobarId(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                "El identificador debe ser positivo.");
        }

        private static void ComprobarTexto(string? texto, int maximo, bool obligatorio)
        {
            if (obligatorio && string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException(
                "Falta un texto obligatorio.");
            if (texto is not null && texto.Length > maximo)
                throw new ArgumentException(
                $"Un texto supera los {maximo} caracteres.");
        }

        static void ComprobarCampos(EstadoEjemplar e)
        {
            ComprobarTexto(e.NombreEstado, 100, true);
        }

        public static async Task<int> InsertarAsync(EstadoEjemplar e)
        {
            ComprobarCampos(e);

            return await EstadoEjemplarEF.InsertarAsync(e);
        }

        public static async Task<int> ModificarAsync(EstadoEjemplar e)
        {
            EstadoEjemplar? mat = await BuscarAsync(e.IdEstadoEjemplar);

            if (mat is null)
            {
                throw new ArgumentException(
                "No existe ese Id de EstadoEjemplar.");
            }

            ComprobarCampos(e);

            return await EstadoEjemplarEF.ModificarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            ComprobarId(id);
            return await EstadoEjemplarEF.BorrarAsync(id);
        }
    }
}