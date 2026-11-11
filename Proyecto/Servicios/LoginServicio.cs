using ProyectoConjuntoMaterialPrestamo.Models;


namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    public static class LoginServicio
    {
        public static async Task<bool> LoginAsync(string Email, string Password)
            => await UsuarioEF.LoginAsync(Email, Password);
    }

}
