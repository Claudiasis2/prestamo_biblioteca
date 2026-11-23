using DotNetEnv;
using Microsoft.Data.SqlClient;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using Xunit;
using System;
using System.IO;
using System.Threading.Tasks;
public abstract class UsuarioAutenticadoTests : IAsyncLifetime
{
    protected readonly string Password = "Test123456";

    protected readonly string Email =
        $"test_{Guid.NewGuid():N}@example.com";

    protected int IdUsuario;

    public virtual async Task InitializeAsync()
    {
        var cadena = Environment.GetEnvironmentVariable(
            "MATERIAL_PRESTAMO_CONNECTION");

        if (string.IsNullOrWhiteSpace(cadena))
        {
            var directorio = new DirectoryInfo(AppContext.BaseDirectory);

            while (directorio != null &&
                   !File.Exists(Path.Combine(directorio.FullName, ".env.test")))
            {
                directorio = directorio.Parent;
            }

            if (directorio == null)
                throw new FileNotFoundException(
                    "No se encontró el archivo .env.test");

            Env.Load(Path.Combine(directorio.FullName, ".env.test"));

            cadena = Environment.GetEnvironmentVariable(
                "MATERIAL_PRESTAMO_CONNECTION");
        }

        if (string.IsNullOrWhiteSpace(cadena))
            throw new InvalidOperationException(
                "No se configuró la conexión de pruebas.");

        var conexion = new SqlConnectionStringBuilder(cadena);

        if (conexion.InitialCatalog != "MaterialPrestamo_Test")
            throw new InvalidOperationException(
                "Los tests solo pueden ejecutarse contra MaterialPrestamo_Test.");

        IdUsuario = await UsuarioEF.InsertarAsync(new Usuario
        {
            Email = Email,
            NombreUsuario = "UsuarioTest",
            PrimerApellido = "ApellidoTest",
            SegundoApellido = "Prueba",
            Password = Password,
            Activo = true
        });
    }

    public virtual async Task DisposeAsync()
    {
        if (IdUsuario > 0)
            await UsuarioEF.BorrarAsync(IdUsuario);
    }
}
