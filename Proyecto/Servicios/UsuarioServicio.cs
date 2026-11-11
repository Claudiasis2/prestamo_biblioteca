using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    public static class UsuarioServicio
    {

        public static async Task<List<Usuario>> BuscarAsync(int id = 0, string email = "", string nombre = "")
        {
            return await UsuarioEF.BuscarAsync(id, email, nombre);
        }


        public static async Task<List<Usuario>> ListarAsync()
            => await UsuarioEF.ListarAsync();


        public static async Task<int> InsertarAsync(Usuario u)
        {
            ComprobarTexto(u.NombreUsuario, 20, true);
            ComprobarTexto(u.PrimerApellido, 20, true);
            ComprobarTexto(u.SegundoApellido, 20, false);
            ComprobarTexto(u.Email, 255, true);
            ComprobarTexto(u.Password, 50, true);

            return await UsuarioEF.InsertarAsync(u);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            return await UsuarioEF.BorrarAsync(id);
        }

        public static async Task<bool> ModificarAsync(Usuario u)
        {
            return await UsuarioEF.ModificarAsync(u);
        }



        private static void ComprobarTexto(string? texto, int maximo, bool obligatorio)
        {
            if (obligatorio && string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("Falta un texto obligatorio.");
            if (texto is not null && texto.Length > maximo)
                throw new ArgumentException($"Un texto supera los {maximo} caracteres.");
        }

        public static async Task<int> InsertarAsync(Usuario e)
        {
            ComprobarCampos(e);

            return await UsuarioEF.InsertarAsync(e);
        }

        public static async Task<int> ModificarAsync(Usuario e)
        {
            Usuario? usr = await BuscarAsync(e.IdUsuario);

            if (usr is null)
            {
                throw new ArgumentException(
                "No existe ese Id de usuario.");
            }

            ComprobarCampos(e);

            return await UsuarioEF.ModificarAsync(e);
        }

        static void ComprobarCampos(Usuario e)
        {
            ComprobarTexto(e.NombreUsuario, 20, false);
            ComprobarTexto(e.PrimerApellido, 20, true);
            ComprobarTexto(e.SegundoApellido, 20, true);
            ComprobarTexto(e.Email, 100, true);
        }



    }
}
