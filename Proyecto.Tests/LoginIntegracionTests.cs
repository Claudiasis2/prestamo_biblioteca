
using DotNetEnv;
using Microsoft.Data.SqlClient;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using ProyectoConjuntoMaterialPrestamo.Frontend.Menus;

public class LoginIntegracionTests : IAsyncLifetime
{
    private int _idUsuario;
    private readonly string _email =
        $"test_{Guid.NewGuid():N}@example.com";

    private const string Password = "Test123456";


    public async Task InitializeAsync()
    {
        // Comprobar si ya existe una conexión configurada.
        // GitHub Actions proporcionará esta variable directamente.
        var cadena = Environment.GetEnvironmentVariable(
            "MATERIAL_PRESTAMO_CONNECTION");

        // Si no existe, cargar la configuración local de pruebas.
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

            var rutaEnv = Path.Combine(directorio.FullName, ".env.test");

            Env.Load(rutaEnv);

            cadena = Environment.GetEnvironmentVariable(
                "MATERIAL_PRESTAMO_CONNECTION");
        }

        // Protección: impedir que los tests utilicen otra base.
        if (string.IsNullOrWhiteSpace(cadena))
            throw new InvalidOperationException(
                "No se configuró la conexión de pruebas.");

        var conexion = new SqlConnectionStringBuilder(cadena);

        if (conexion.InitialCatalog != "MaterialPrestamo_Test")
            throw new InvalidOperationException(
                "Los tests solo pueden ejecutarse contra MaterialPrestamo_Test.");

        // Crear usuario exclusivo para esta prueba.
        _idUsuario = await UsuarioEF.InsertarAsync(new Usuario
        {
            Email = _email,
            NombreUsuario = "UsuarioTest",
            PrimerApellido = "ApellidoTest",
            SegundoApellido = "Prueba",
            Password = Password,
            Activo = true
        });
    }


    public async Task DisposeAsync()
    {
        // Limpiar el usuario creado por el test
        if (_idUsuario > 0)
            await UsuarioEF.BorrarAsync(_idUsuario);
    }

    [Fact]
    public async Task LoginAsync_ConCredencialesCorrectas_DevuelveUsuario()
    {
        var usuario = await LoginServicio.LoginAsync(
            _email, Password);

        Assert.NotNull(usuario);
        Assert.Equal(_email, usuario.Email);
        Assert.Equal("UsuarioTest", usuario.NombreUsuario.Trim());
    }

    [Fact]
    public async Task LoginAsync_ConPasswordIncorrecta_DevuelveNull()
    {
        var usuario = await LoginServicio.LoginAsync(
            _email, "PasswordIncorrecta");

        Assert.Null(usuario);
    }

    [Fact]
    public async Task LoginAsync_ConUsuarioInexistente_DevuelveNull()
    {
        var usuario = await LoginServicio.LoginAsync(
            "usuario_inexistente@example.com", Password);

        Assert.Null(usuario);
    }
    [Fact]
    public async Task Menu_LoginCorrecto_MuestraBienvenida()
    {
        // Arrange
        var consola = new ConsolaFalsa(
            "3",
            _email,
            Password,
            "4",
            "0",
            "0"
        );

        // Act
        await MenuPrincipal.Menu(consola);

        // Assert
        string salida = consola.ObtenerTextoMostrado();

        Assert.Contains("¡BIENVENIDO/A UsuarioTest!", salida);
        Assert.Contains("Sesión activa: UsuarioTest", salida);
        Assert.Contains("GESTIÓN DE SOLICITUDES DE PRÉSTAMO", salida);
    }
}
