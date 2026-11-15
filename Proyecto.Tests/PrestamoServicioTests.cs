
using Xunit;
using ProyectoConjuntoMaterialPrestamo.Servicios;

namespace Proyecto.Tests;

public class PrestamoServicioTests
{
    [Fact]
    public async Task BuscarAsync_IdCero_LanzaExcepcion()
    {
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => PrestamoServicio.BuscarAsync(0)
        );

        Assert.Equal(
            "El identificador debe ser positivo.",
            exception.Message
        );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CrearPrestamoAsync_DiasInvalidos_LanzaExcepcion(
        int dias)
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => PrestamoServicio.CrearPrestamoAsync(1, dias)
        );
    }

    [Fact]
    public async Task RegistrarDevolucionAsync_IdInvalido_LanzaExcepcion()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => PrestamoServicio.RegistrarDevolucionAsync(0)
        );
    }
}
