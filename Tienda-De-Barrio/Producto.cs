using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace Tienda_De_Barrio
{
    public class Producto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public double PrecioVenta { get; set; }
        public int StockActual { get; set; }
        public string ImagenPath { get; set; }
        public string Distribuidor { get; set; }

        public BitmapImage ImagenSource
        {
            get
            {
                try
                {
                    // Ruta predeterminada
                    string rutaPredeterminada = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Images", "default-product.png"
                    );

                    // Si no hay ruta, usar predeterminada
                    if (string.IsNullOrEmpty(ImagenPath))
                    {
                        if (File.Exists(rutaPredeterminada))
                            return new BitmapImage(new Uri(rutaPredeterminada));
                        return null;
                    }

                    // Intentar cargar la imagen especificada
                    string rutaAbsoluta = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        ImagenPath
                    );

                    if (File.Exists(rutaAbsoluta))
                    {
                        return new BitmapImage(new Uri(rutaAbsoluta));
                    }
                    else
                    {
                        // Si no existe, usar la predeterminada
                        if (File.Exists(rutaPredeterminada))
                            return new BitmapImage(new Uri(rutaPredeterminada));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al cargar imagen: {ex.Message}");

                    // Intentar con la predeterminada en caso de error
                    try
                    {
                        string rutaPredeterminada = Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            "Images", "default-product.png"
                        );
                        if (File.Exists(rutaPredeterminada))
                            return new BitmapImage(new Uri(rutaPredeterminada));
                    }
                    catch
                    {
                        // Ignorar errores finales
                    }
                }
                return null;
            }
        }

        public Producto()
        {
            Codigo = Guid.NewGuid().ToString("N").Substring(0, 8);
            Nombre = "Nuevo Producto";
            PrecioVenta = 0.0;
            StockActual = 0;
            ImagenPath = "Images/default-product.png";
            Distribuidor = "Local";
        }

        // ✅ MÉTODOS DE STOCK
        public void ReducirStock(int cantidad)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa.");
            if (StockActual < cantidad)
                throw new InvalidOperationException("Stock insuficiente.");
            StockActual -= cantidad;
        }

        public void AumentarStock(int cantidad)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa.");
            StockActual += cantidad;
        }
    }
}
