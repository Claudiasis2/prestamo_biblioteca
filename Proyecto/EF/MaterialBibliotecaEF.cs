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
            };
        }

    }
}
