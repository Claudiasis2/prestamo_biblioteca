using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    internal class MaterialBibliotecaServicio
    {
        public static async Task<List<MaterialBiblioteca>> ListarAsync()
            => await MaterialBibliotecaEF.ListarAsync();

        public static async Task<MaterialBiblioteca?> BuscarAsync(int id)
        {
            ComprobarId(id);
            return await MaterialBibliotecaEF.BuscarAsync(id);
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

        static void ComprobarCampos(MaterialBiblioteca e)
        {
            ComprobarTexto(e.Titulo, 55, false);
        }

        public static async Task<int> InsertarAsync(MaterialBiblioteca e)
        {
            ComprobarCampos(e);

            return await MaterialBibliotecaEF.InsertarAsync(e);
        }

        public static async Task<int> ModificarAsync(MaterialBiblioteca e)
        {
            MaterialBiblioteca? mat = await BuscarAsync(e.IdMaterial);

            if (mat is null) { 
                throw new ArgumentException(
                "No existe ese Id de material.");
            }

            ComprobarCampos(e);

            return await MaterialBibliotecaEF.ModificarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            ComprobarId(id);
            return await MaterialBibliotecaEF.BorrarAsync(id);
        }
    }
}
