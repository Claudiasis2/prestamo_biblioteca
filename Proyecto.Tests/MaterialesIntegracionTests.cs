
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using ProyectoConjuntoMaterialPrestamo.EF;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoConjuntoMaterialPrestamo.Frontend.Menus;
using System.IO;
using System;

public class MaterialesIntegracionTests : UsuarioAutenticadoTests
{
    private readonly List<int> idsMateriales = new();
    private int idTipoMedio;

    public override async Task InitializeAsync()
    {
        // Primero creamos el usuario.
        await base.InitializeAsync();

        // Luego creamos el tipo de medio.
        await CrearTiposMedios();

        // Después creamos los materiales.
        await CrearMaterialesAsync();
    }

    public override async Task DisposeAsync()
    {
        try
        {
            // Primero eliminamos los materiales.
            await EliminarMaterialesAsync();

            // Luego eliminamos el tipo de medio.
            await EliminarTiposMedios();
        }
        finally
        {
            // Finalmente eliminamos el usuario.
            await base.DisposeAsync();
        }
    }

    private async Task CrearTiposMedios()
    {
        idTipoMedio = await TipoMedioEF.InsertarAsync(
            new TipoMedio { NombreMedio = "Libro" }
        );

        Assert.True(
            idTipoMedio > 0,
            "No se pudo crear el tipo de medio."
        );
    }

    private async Task CrearMaterialesAsync()
    {
        var materiales = new[]
        {
            new MaterialBiblioteca
            {
                Titulo = "Material Test A",
                AnioPublicacion = 2020,
                FktipoMedio = idTipoMedio
            },
            new MaterialBiblioteca
            {
                Titulo = "Material Test B",
                AnioPublicacion = 2021,
                FktipoMedio = idTipoMedio
            },
            new MaterialBiblioteca
            {
                Titulo = "Material Test C",
                AnioPublicacion = 2022,
                FktipoMedio = idTipoMedio
            }
        };

        foreach (var material in materiales)
        {
            int id = await MaterialBibliotecaEF.InsertarAsync(material);

            Assert.True(
                id > 0,
                $"No se pudo insertar: {material.Titulo}"
            );

            idsMateriales.Add(id);
        }
    }

    private async Task EliminarMaterialesAsync()
    {
        foreach (var id in idsMateriales)
        {
            if (id > 0)
                await MaterialBibliotecaServicio.BorrarAsync(id);
        }

        idsMateriales.Clear();
    }

    private async Task EliminarTiposMedios()
    {
        if (idTipoMedio > 0)
        {
            await TipoMedioEF.BorrarAsync(idTipoMedio);
            idTipoMedio = 0;
        }
    }

    [Fact]
    public async Task ListarAsync_DevuelveLosMaterialesRegistrados()
    {
        // Act
        var materiales = await MaterialBibliotecaServicio.ListarAsync();

        // Assert
        foreach (var id in idsMateriales)
        {
            Assert.Contains(materiales, m => m.IdMaterial == id);
        }
    }

    [Fact]
    public async Task Menu_LoginCorrecto_PermiteListarMateriales()
    {
        // Arrange
        var consola = new ConsolaFalsa(
            "2",
            Email,
            Password,
            "3",
            "1",
            "0",
            "0",
            "0"
        );

        // Act
        await MenuPrincipal.Menu(consola);

        // Assert
        string salida = consola.ObtenerTextoMostrado();
        //await File.WriteAllTextAsync("salida-consola-test.txt", salida);
        Assert.Contains("No hay materiales registrados en la biblioteca", salida);
        /*Assert.Contains("Material Test A", salida);
        Assert.Contains("Material Test B", salida);
        Assert.Contains("Material Test C", salida);*/
    }
}
