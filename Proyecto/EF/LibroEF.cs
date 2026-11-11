using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    internal class LibroEF
    {
        public static async Task<List<Libro>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.Libros
                    .OrderBy(f => f.FkmaterialBiblioteca)
                    .ToListAsync<Libro>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<Libro>();
            }
        }

        public static async Task<Libro?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.Libros
                .SingleOrDefaultAsync(f => f.FkmaterialBiblioteca == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(Libro e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Libro
                {
                    FkmaterialBiblioteca = e.FkmaterialBiblioteca,
                    Autor = e.Autor,
                    NumeroPagina = e.NumeroPagina,
                    Isbn = e.Isbn
                };
                db.Libros.Add(fila);
                await db.SaveChangesAsync();
                return fila.FkmaterialBiblioteca;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }
        }

        public static async Task<int> ModificarAsync(Libro e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Libro
                {
                    FkmaterialBiblioteca = e.FkmaterialBiblioteca,
                    Autor = e.Autor,
                    NumeroPagina = e.NumeroPagina,
                    Isbn = e.Isbn
                };
                db.Libros.Update(fila);
                await db.SaveChangesAsync();
                return fila.FkmaterialBiblioteca;
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
                var fila = db.Libros.SingleOrDefault(
                f => f.FkmaterialBiblioteca == id);
                if (fila is null) return false;
                db.Libros.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static Libro Convertir(Libro fila)
        {
            return new Libro
            {
                FkmaterialBiblioteca = fila.FkmaterialBiblioteca,
                Autor = fila.Autor,
                NumeroPagina = fila.NumeroPagina,
                Isbn = fila.Isbn
            };
        }

    }
}