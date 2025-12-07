using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tienda_De_Barrio
{
    public static class TiendaData
    {
        public static List<Usuario> Usuarios { get; set; }
        public static List<Producto> Productos { get; set; }
        public static List<VentaRegistro> Ventas { get; set; }
        public static List<Abastecimiento> Abastecimientos { get; set; } // ← NUEVO

        static TiendaData()
        {
            Usuarios = new List<Usuario>();
            Productos = new List<Producto>();
            Ventas = new List<VentaRegistro>();
            Abastecimientos = new List<Abastecimiento>(); // ← Inicializa
        }

        private static string RutaUsuarios => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Datos", "usuarios.json");

        private static string RutaProductos => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Datos", "productos.json");

        private static string RutaVentas => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Datos", "ventas.json");

        private static string RutaAbastecimientos => Path.Combine( // ← NUEVO
            AppDomain.CurrentDomain.BaseDirectory, "Datos", "abastecimientos.json");

        // === USUARIOS ===
        public static void CargarUsuarios()
        {
            try
            {
                if (File.Exists(RutaUsuarios))
                {
                    string json = File.ReadAllText(RutaUsuarios);
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Usuarios = JsonSerializer.Deserialize<List<Usuario>>(json, opciones);
                }
                else
                {
                    CrearUsuariosDeEjemplo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CrearUsuariosDeEjemplo();
            }
        }

        public static void GuardarUsuarios()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(RutaUsuarios));
                var opciones = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(RutaUsuarios, JsonSerializer.Serialize(Usuarios, opciones));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar usuarios:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private static void CrearUsuariosDeEjemplo()
        {
            Usuarios = new List<Usuario>
            {
                new Usuario { Nombre = "admin", Rol = "Admin" }.Apply(u => u.EstablecerContraseña("1234")),
                new Usuario { Nombre = "empleado", Rol = "Empleado" }.Apply(u => u.EstablecerContraseña("1234"))
            };
        }

        // === PRODUCTOS ===
        private static readonly string[] NombresProductosBasicos = new[]
        {
            "Coca Cola 500ml", "Pil Frut 1L", "Leche PIL 900ml", "Galleta Margarita",
            "Chicle Bubbaloo", "Bon Bon Bum", "Fósforos Paraná", "Jabón Bolívar"
        };

        public static void CargarProductos()
        {
            try
            {
                if (File.Exists(RutaProductos))
                {
                    string json = File.ReadAllText(RutaProductos);
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Productos = JsonSerializer.Deserialize<List<Producto>>(json, opciones);
                }
                else
                {
                    CrearProductosDeEjemplo();
                    return;
                }

                var productosBasicos = ObtenerProductosBasicos();
                foreach (string nombreBasico in NombresProductosBasicos)
                {
                    if (!Productos.Any(p => p.Nombre == nombreBasico))
                    {
                        var productoBasico = productosBasicos.FirstOrDefault(p => p.Nombre == nombreBasico);
                        if (productoBasico != null)
                        {
                            Productos.Add(productoBasico);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CrearProductosDeEjemplo();
            }
        }

        public static void GuardarProductos()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(RutaProductos));
                var opciones = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(RutaProductos, JsonSerializer.Serialize(Productos, opciones));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar productos:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private static void CrearProductosDeEjemplo()
        {
            Productos = ObtenerProductosBasicos();
        }

        private static List<Producto> ObtenerProductosBasicos()
        {
            return new List<Producto>
            {
                new Producto { Nombre = "Coca Cola 500ml", PrecioVenta = 8.00, StockActual = 25, ImagenPath = "Images/Productos/coca_cola.png" },
                new Producto { Nombre = "Pil Frut 1L", PrecioVenta = 10.00, StockActual = 20, ImagenPath = "Images/Productos/pilfrut.png" },
                new Producto { Nombre = "Leche PIL 900ml", PrecioVenta = 12.00, StockActual = 15, ImagenPath = "Images/Productos/leche_pil.png" },
                new Producto { Nombre = "Galleta Margarita", PrecioVenta = 3.50, StockActual = 30, ImagenPath = "Images/Productos/margarita.png" },
                new Producto { Nombre = "Chicle Bubbaloo", PrecioVenta = 2.00, StockActual = 40, ImagenPath = "Images/Productos/chicle.png" },
                new Producto { Nombre = "Bon Bon Bum", PrecioVenta = 1.50, StockActual = 50, ImagenPath = "Images/Productos/bon_bon_bum.png" },
                new Producto { Nombre = "Fósforos Paraná", PrecioVenta = 2.00, StockActual = 60, ImagenPath = "Images/Productos/fosforos.png" },
                new Producto { Nombre = "Jabón Bolívar", PrecioVenta = 4.00, StockActual = 25, ImagenPath = "Images/Productos/jabon_bolivar.png" }
            };
        }

        // === VENTAS ===
        public static void GuardarVentas()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(RutaVentas));
                var opciones = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(RutaVentas, JsonSerializer.Serialize(Ventas, opciones));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar ventas:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static void CargarVentas()
        {
            try
            {
                if (File.Exists(RutaVentas))
                {
                    string json = File.ReadAllText(RutaVentas);
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Ventas = JsonSerializer.Deserialize<List<VentaRegistro>>(json, opciones) ;
                }
                else
                {
                    Ventas = new List<VentaRegistro>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ventas:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                Ventas = new List<VentaRegistro>();
            }
        }

        // === ABASTECIMIENTOS === (NUEVO)
        public static void GuardarAbastecimientos()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(RutaAbastecimientos));
                var opciones = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(RutaAbastecimientos, JsonSerializer.Serialize(Abastecimientos, opciones));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar abastecimientos:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static void CargarAbastecimientos()
        {
            try
            {
                if (File.Exists(RutaAbastecimientos))
                {
                    string json = File.ReadAllText(RutaAbastecimientos);
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Abastecimientos = JsonSerializer.Deserialize<List<Abastecimiento>>(json, opciones);
                }
                else
                {
                    Abastecimientos = new List<Abastecimiento>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar abastecimientos:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                Abastecimientos = new List<Abastecimiento>();
            }
        }
    }

    public class Abastecimiento // ← NUEVO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Distribuidor { get; set; }
        public string ProductoCodigo { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadAgregada { get; set; }
        public double PrecioCompraTotal { get; set; }
    }

    // Método de extensión
    public static class Extensiones
    {
        public static T Apply<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }
}
