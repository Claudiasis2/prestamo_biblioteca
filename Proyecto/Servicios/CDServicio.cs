using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    internal class CDServicio
    {
        /*public static async Task<List<Cd>> ListarAsync()
            => await CDEF.ListarAsync();

        public static async Task<Cd?> BuscarAsync(int id)
        {
            ComprobarId(id);
            return await CDEF.BuscarAsync(id);
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

        static void ComprobarCampos(Cd e)
        {
            ComprobarTexto(e.NombreArtista, 50, false);
        }

        public static async Task<int> InsertarAsync(Cd e)
        {
            ComprobarCampos(e);

            return await CDEF.InsertarAsync(e);
        }

        public static async Task<int> ModificarAsync(Cd e)
        {
            Cd? mat = await BuscarAsync(e.FkmaterialBiblioteca);

            if (mat is null)
            {
                throw new ArgumentException(
                "No existe ese Id de CD.");
            }

            ComprobarCampos(e);

            return await CDEF.ModificarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            ComprobarId(id);
            return await CDEF.BorrarAsync(id);
        }*/
    }
}
