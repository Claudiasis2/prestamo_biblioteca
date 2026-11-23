
using ProyectoConjuntoMaterialPrestamo.Frontend.Menus;
using Xunit;

public class MenuPrincipalTests
{
	[Fact]
	public async Task Menu_SinSesion_NoPermiteAccederASolicitudes()
	{
		// Arrange
		var consola = new ConsolaFalsa("3", "0");

		// Act
		await MenuPrincipal.Menu(consola);

		// Assert
		string salida = consola.ObtenerTextoMostrado();

		Assert.Contains(
			"Debe iniciar sesión",
			salida
		);
	}
}
