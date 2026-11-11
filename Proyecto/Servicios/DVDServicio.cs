using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    internal class DVDServicio
    {
        public static async Task<List<Dvd>> ListarAsync()
            => await DVDEF.ListarAsync();

        public static async Task<Dvd?> BuscarAsync(int id)
        {
            ComprobarId(id);
            return await DVDEF.BuscarAsync(id);
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

        static void ComprobarCampos(Dvd e)
        {
            ComprobarTexto(e.NombreDirector, 50, false);
            ComprobarTexto(e.ClasificacionEdad, 50, false);
        }

        public static async Task<int> InsertarAsync(Dvd e)
        {
            ComprobarCampos(e);

            return await DVDEF.InsertarAsync(e);
        }

        public static async Task<int> ModificarAsync(Dvd e)
        {
            Dvd? mat = await BuscarAsync(e.FkmaterialBiblioteca);

            if (mat is null)
            {
                throw new ArgumentException(
                "No existe ese Id de DVD.");
            }

            ComprobarCampos(e);

            return await DVDEF.ModificarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            ComprobarId(id);
            return await DVDEF.BorrarAsync(id);
        }
    }
}
