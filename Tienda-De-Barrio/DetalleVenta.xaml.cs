using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tienda_De_Barrio
{
    /// <summary>
    /// Lógica de interacción para DetalleVenta.xaml
    /// </summary>
    public partial class DetalleVenta : Window
    {
        private List<ProductoCantidad> _productosSeleccionados;

        public DetalleVenta(List<ProductoCantidad> productos)
        {
            InitializeComponent();
            _productosSeleccionados = productos;
            CargarDetalle();
        }

        private void CargarDetalle()
        {
            // Mostrar fecha actual
            txtFecha.Text = $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}";

            double total = 0;
            stackProductos.Children.Clear();

            foreach (var item in _productosSeleccionados)
            {
                var producto = item.Producto;
                var cantidad = item.Cantidad;
                var subtotal = producto.PrecioVenta * cantidad;
                total += subtotal;

                // Crear fila para el producto
                var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 4, 0, 4) };
                panel.Children.Add(new TextBlock { Text = "● ", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1976D2")) });
                panel.Children.Add(new TextBlock { Text = $"{producto.Nombre} x{cantidad}", Foreground = Brushes.White, Width = 200 });
                panel.Children.Add(new TextBlock { Text = $"Bs {subtotal:F2}", Foreground = Brushes.White, HorizontalAlignment = HorizontalAlignment.Right, Width = 100 });

                stackProductos.Children.Add(panel);
            }

            txtTotal.Text = $"TOTAL: Bs {total:F2}";
        }

        private void Vender_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Actualizar stock de cada producto
                foreach (var item in _productosSeleccionados)
                {
                    var producto = TiendaData.Productos.FirstOrDefault(p => p.Codigo == item.Producto.Codigo);
                    if (producto != null)
                    {
                        if (producto.StockActual < item.Cantidad)
                        {
                            MessageBox.Show($"Stock insuficiente para {producto.Nombre}");
                            return;
                        }
                        producto.ReducirStock(item.Cantidad);
                    }
                }

                // 2. Registrar la venta
                var nuevaVenta = new VentaRegistro
                {
                    UsuarioId = "EmpleadoTemporal", // ← Reemplaza con el ID del usuario logueado
                    Detalles = _productosSeleccionados.Select(p => new DetalleVentaItem
                    {
                        ProductoCodigo = p.Producto.Codigo,
                        NombreProducto = p.Producto.Nombre,
                        Cantidad = p.Cantidad,
                        PrecioUnitario = p.Producto.PrecioVenta
                    }).ToList()
                };
                nuevaVenta.CalcularTotal();
                TiendaData.Ventas.Add(nuevaVenta);

                // 3. Guardar cambios
                TiendaData.GuardarProductos();
                TiendaData.GuardarVentas(); // ← Asegúrate de tener este método

                MessageBox.Show("Venta registrada exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                Administracion ad=new Administracion();
                ad.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar la venta: {ex.Message}");
            }
        }

        private void Atras_Click(object sender, RoutedEventArgs e)
        {
            Venta ve = new Venta();
            ve.Show();
            this.Close(); // Vuelve a la ventana de ventas (asumiendo que usas ShowDialog)
        }
    }

    // Clases auxiliares para la venta (si no las tienes)


    public class VentaRegistro
    {
        public string Id { get; set; } = $"V-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4)}";
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string UsuarioId { get; set; }
        public List<DetalleVentaItem> Detalles { get; set; }
        public double Total { get; set; }

        public void CalcularTotal()
        {
            Total = Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
        }
    }

    public class DetalleVentaItem
    {
        public string ProductoCodigo { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
    }
}
