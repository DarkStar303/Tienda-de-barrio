using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para NuevoProducto.xaml
    /// </summary>
    public partial class NuevoProducto : Window
    {
        private string _rutaImagenSeleccionada;

        public NuevoProducto()
        {
            InitializeComponent();
        }

        private void BtnSeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.bmp",
                Title = "Seleccionar imagen del producto"
            };

            if (dialog.ShowDialog() == true)
            {
                _rutaImagenSeleccionada = dialog.FileName;
                imgPreview.Source = new BitmapImage(new Uri(_rutaImagenSeleccionada));
                lblRutaImagen.Text = System.IO.Path.GetFileName(_rutaImagenSeleccionada);
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Validar campos
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar el nombre del producto.");
                return;
            }

            if (!double.TryParse(txtPrecio.Text, out double precio) || precio <= 0)
            {
                MessageBox.Show("Precio inválido. Ingrese un número mayor a 0.");
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Stock inválido. Ingrese un número entero ≥ 0.");
                return;
            }

            string rutaImagenGuardada = "Images/default-product.png"; // Valor por defecto
            if (!string.IsNullOrEmpty(_rutaImagenSeleccionada))
            {
                try
                {
                    // ✅ DECLARA nombreArchivo DENTRO del bloque if
                    string nombreArchivo = Guid.NewGuid().ToString().Substring(0, 8) +
                                         System.IO.Path.GetExtension(_rutaImagenSeleccionada);
                    string carpetaDestino = System.IO.Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Images", "Productos"
                    );
                    System.IO.Directory.CreateDirectory(carpetaDestino);
                    string rutaDestino = System.IO.Path.Combine(carpetaDestino, nombreArchivo);
                    System.IO.File.Copy(_rutaImagenSeleccionada, rutaDestino, overwrite: true);

                    // ✅ Usa nombreArchivo SOLO después de declararlo
                    rutaImagenGuardada = $"Images/Productos/{nombreArchivo}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar la imagen: {ex.Message}");
                    return;
                }
            }

            var producto = new Producto
            {
                Nombre = txtNombre.Text,
                PrecioVenta = precio,
                StockActual = stock,
                ImagenPath = rutaImagenGuardada
            };

            TiendaData.Productos.Add(producto);
            TiendaData.GuardarProductos();

            MessageBox.Show("Producto agregado exitosamente.");
            var admin = new Administracion();
            admin.Show();
            this.Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Administracion ad=new Administracion();
            ad.Show();
            this.Close();
        }
    }
}
