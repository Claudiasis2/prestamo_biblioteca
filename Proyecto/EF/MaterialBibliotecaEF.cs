using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    public static class MaterialBibliotecaEF
    {
        public static async Task<List<MaterialBiblioteca>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.MaterialBibliotecas
                    .OrderBy(f => f.IdMaterial).ToListAsync<MaterialBiblioteca>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<MaterialBiblioteca>();
            }
        }

        public static async Task<MaterialBiblioteca?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.MaterialBibliotecas
                .SingleOrDefaultAsync(f => f.IdMaterial == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(MaterialBiblioteca e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new MaterialBiblioteca
                {
                    IdMaterial = e.IdMaterial,
                    Titulo = e.Titulo,
                    AnioPublicacion = e.AnioPublicacion,
                    DuracionMinutos = e.DuracionMinutos,
                    NombreArtista = e.NombreArtista,
                    Autor = e.Autor,
                    NumeroPaginas = e.NumeroPaginas,
                    Isbn = e.Isbn,
                    NombreDirector = e.NombreDirector,
                    ClasificacionEdad = e.ClasificacionEdad,
                };
                db.MaterialBibliotecas.Add(fila);
                await db.SaveChangesAsync();
                return fila.IdMaterial;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<int> ModificarAsync(MaterialBiblioteca e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new MaterialBiblioteca
                {
                    IdMaterial = e.IdMaterial,
                    Titulo = e.Titulo,
                    AnioPublicacion = e.AnioPublicacion,
                    DuracionMinutos = e.DuracionMinutos,
                    NombreArtista = e.NombreArtista,
                    Autor = e.Autor,
                    NumeroPaginas = e.NumeroPaginas,
                    Isbn = e.Isbn,
                    NombreDirector = e.NombreDirector,
                    ClasificacionEdad = e.ClasificacionEdad,
                };
                db.MaterialBibliotecas.Update(fila);
                await db.SaveChangesAsync();
                return fila.IdMaterial;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<bool> BorrarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = db.MaterialBibliotecas.SingleOrDefault(
                f => f.IdMaterial == id);
                if (fila is null) return false;
                db.MaterialBibliotecas.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static MaterialBiblioteca Convertir(MaterialBiblioteca fila)
        {
            return new MaterialBiblioteca
            {
                IdMaterial = fila.IdMaterial,
                Titulo = fila.Titulo,
                AnioPublicacion = fila.AnioPublicacion,
                DuracionMinutos = fila.DuracionMinutos,
                NombreArtista = fila.NombreArtista,
                Autor = fila.Autor,
                NumeroPaginas = fila.NumeroPaginas,
                Isbn = fila.Isbn,
                NombreDirector = fila.NombreDirector,
                ClasificacionEdad = fila.ClasificacionEdad,
            };
        }

        public class MaterialConTipo
        {
            public int IdMaterial { get; set; }
            public string Titulo { get; set; } = string.Empty;
            public int? AnioPublicacion { get; set; }
            public string Tipo { get; set; } = string.Empty;
            public int? DuracionMinutos { get; set; }
            public string NombreArtista { get; set; } = string.Empty;
            public string Autor { get; set; } = string.Empty;
            public int? NumeroPaginas { get; set; }
            public string Isbn { get; set; } = string.Empty;
            public string NombreDirector { get; set; } = string.Empty;
            public string? ClasificacionEdad { get; set; }
        }

        public static async Task<List<MaterialConTipo>> ListarDisponiblesConTipoAsync()
        {
            await using var db = ConexionEf.CrearContexto();

            /*var materiales = await db.MaterialBibliotecas
                .Include(m => m.Ejemplars)
                .Where(m => m.Ejemplars.Any(e => e.Fkestado == 1))
                .OrderBy(m => m.IdMaterial)
                .ToListAsync();

            var idsLibros = await db.Libros.Select(l => l.FkmaterialBiblioteca).ToListAsync(); 
            var idsCds = await db.Cds.Select(c => c.FkmaterialBiblioteca).ToListAsync(); 
            var idsDvds = await db.Dvds.Select(d => d.FkmaterialBiblioteca).ToListAsync();
            
            foreach (var m in materiales)
            {
                string tipo = "Material";
                if (idsLibros.Contains(m.IdMaterial)) tipo = "Libro"; 
                else if (idsCds.Contains(m.IdMaterial)) tipo = "CD"; 
                else if (idsDvds.Contains(m.IdMaterial)) tipo = "DVD";

                resultado.Add(new MaterialConTipo
                {
                    IdMaterial = m.IdMaterial,
                    Titulo = m.Titulo,
                    AnioPublicacion = m.AnioPublicacion,
                    Tipo = tipo,
                    DuracionMinutos = m.DuracionMinutos,
                    NombreArtista = m.NombreArtista,
                    Autor = m.Autor,
                    NumeroPagina = m.NumeroPagina,
                    Isbn = m.Isbn,
                    NombreDirector = e.NombreDirector,
                    ClasificacionEdad = e.ClasificacionEdad,
                });
            } 
             */

            var materiales = await db.MaterialBibliotecas
                .Include(m => m.FktipoMedio)
                .Include(m => m.Ejemplars)
                .Where(m => m.Ejemplars.Any(e => e.Fkestado == 1))
                .OrderBy(m => m.IdMaterial)
                .ToListAsync();

            var resultado = new List<MaterialConTipo>();

            foreach (var m in materiales)
            {
                

                resultado.Add(new MaterialConTipo
                {
                    IdMaterial = m.IdMaterial,
                    Titulo = m.Titulo,
                    AnioPublicacion = m.AnioPublicacion,
                    Tipo = m.FktipoMedioNavigation.NombreMedio,
                    DuracionMinutos = m.DuracionMinutos,
                    NombreArtista = m.NombreArtista,
                    Autor = m.Autor,
                    NumeroPaginas = m.NumeroPaginas,
                    Isbn = m.Isbn,
                    NombreDirector = m.NombreDirector,
                    ClasificacionEdad = m.ClasificacionEdad,
                });
            }

            return resultado;
        }
    }

}
 
    










