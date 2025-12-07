using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Lógica de interacción para Venta.xaml
    /// </summary>
    public partial class Venta : Window
    {
        public Venta()
        {
            InitializeComponent();
            CargarProductos();
        }
        private void CargarProductos()
        {
            foreach (var producto in TiendaData.Productos)
            {
                var rutaAbsoluta = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    producto.ImagenPath
                );
                System.Diagnostics.Debug.WriteLine($"Producto: {producto.Nombre}");
                System.Diagnostics.Debug.WriteLine($"Ruta: {rutaAbsoluta}");
                System.Diagnostics.Debug.WriteLine($"Existe: {File.Exists(rutaAbsoluta)}");
            }

            listaProductos.ItemsSource = TiendaData.Productos;
        }

        private void Dettale_Click(object sender, RoutedEventArgs e)
        {
            var productosSeleccionados = new List<ProductoCantidad>();

            // Obtener todos los elementos del ItemsControl
            for (int i = 0; i < listaProductos.Items.Count; i++)
            {
                var contenedor = listaProductos.ItemContainerGenerator.ContainerFromIndex(i);
                if (contenedor == null) continue;

                // Encontrar el TextBox de cantidad dentro del DataTemplate
                var textBox = BuscarTextBox(contenedor);
                if (textBox != null &&
                    int.TryParse(textBox.Text, out int cantidad) &&
                    cantidad > 0)
                {
                    var producto = (Producto)listaProductos.Items[i];
                    productosSeleccionados.Add(new ProductoCantidad
                    {
                        Producto = producto,
                        Cantidad = cantidad
                    });
                }
            }

            if (productosSeleccionados.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un producto con cantidad mayor a 0.");
                return;
            }

            // Abrir detalle de venta
            DetalleVenta dv=new DetalleVenta(productosSeleccionados);
            dv.Show();
            this.Close();
            
            // Opcional: actualizar stock si la venta fue confirmada
            // (esto se haría en DetalleVenta.xaml.cs al confirmar)
        }

        // Método auxiliar para encontrar el TextBox dentro del DataTemplate
        private TextBox BuscarTextBox(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is TextBox textBox && textBox.Name == "txtCantidad")
                    return textBox;

                var result = BuscarTextBox(child);
                if (result != null)
                    return result;
            }
            return null;
        }

    }

    // Clase auxiliar para pasar datos a DetalleVenta
    public class ProductoCantidad
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
