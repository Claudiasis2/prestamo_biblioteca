
using DotNetEnv;
using Microsoft.Data.SqlClient;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using ProyectoConjuntoMaterialPrestamo.Frontend.Menus;

public class LoginIntegracionTests : UsuarioAutenticadoTests
{
    
    [Fact]
    public async Task LoginAsync_ConCredencialesCorrectas_DevuelveUsuario()
    {
        var usuario = await LoginServicio.LoginAsync(
            Email, Password);

        Assert.NotNull(usuario);
        Assert.Equal(Email, usuario.Email);
        Assert.Equal("UsuarioTest", usuario.NombreUsuario.Trim());
    }

    [Fact]
    public async Task LoginAsync_ConPasswordIncorrecta_DevuelveNull()
    {
        var usuario = await LoginServicio.LoginAsync(
            Email, "PasswordIncorrecta");

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
            "2",
            Email,
            Password,
            "3",
            "0",
            "0"
        );

        // Act
        await MenuPrincipal.Menu(consola);

        // Assert
        string salida = consola.ObtenerTextoMostrado();

        Assert.Contains("¡BIENVENIDO/A UsuarioTest!", salida);
        Assert.Contains("Sesión Usuario activa: UsuarioTest", salida);
        Assert.Contains("GESTIÓN DE SOLICITUDES DE PRÉSTAMO", salida);
    }
}
