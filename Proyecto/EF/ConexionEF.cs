using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo;
using ProyectoConjuntoMaterialPrestamo.Models;
public static class ConexionEf
{
    public static MaterialPrestamoContext CrearContexto()
    {
        var opciones =
        new DbContextOptionsBuilder<MaterialPrestamoContext>();
        opciones.UseSqlServer(Configuracion.Cadena);

        return new MaterialPrestamoContext(opciones.Options);
    }
}