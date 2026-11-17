
using DotNetEnv;
using Microsoft.Data.SqlClient;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;

public class LoginIntegracionTests : IAsyncLifetime
{
    private int _idUsuario;
    private readonly string _email =
        $"test_{Guid.NewGuid():N}@example.com";

    private const string Password = "Test123456";

    public async Task InitializeAsync()
    {
        // Buscar .env.test desde el directorio del test
        var directorio = new DirectoryInfo(AppContext.BaseDirectory);

        while (directorio != null &&
               !File.Exists(Path.Combine(directorio.FullName, ".env.test")))
        {
            directorio = directorio.Parent;
        }

        if (directorio == null)
            throw new FileNotFoundException(
                "No se encontró el archivo .env.test");

        // Cargar explícitamente la conexión de pruebas
        var rutaEnv = Path.Combine(directorio.FullName, ".env.test");

        Environment.SetEnvironmentVariable(
            "MATERIAL_PRESTAMO_CONNECTION", null);

        Env.Load(rutaEnv);

        // Protección: impedir que los tests utilicen otra base
        var cadena = Environment.GetEnvironmentVariable(
            "MATERIAL_PRESTAMO_CONNECTION");

        if (string.IsNullOrWhiteSpace(cadena))
            throw new InvalidOperationException(
                "No se configuró la conexión de pruebas.");

        var conexion = new SqlConnectionStringBuilder(cadena);

        if (conexion.InitialCatalog != "MaterialPrestamo_Test")
            throw new InvalidOperationException(
                "Los tests solo pueden ejecutarse contra MaterialPrestamo_Test.");

        // Crear usuario exclusivo para esta prueba
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
}
