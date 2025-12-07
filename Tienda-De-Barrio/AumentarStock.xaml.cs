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
    /// Lógica de interacción para AumentarStock.xaml
    /// </summary>
    public partial class AumentarStock : Window
    {
        public AumentarStock()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void CargarProductos()
        {
            cmbProductos.Items.Clear();
            foreach (var producto in TiendaData.Productos)
            {
                // Mostrar nombre + stock actual
                cmbProductos.Items.Add($"{producto.Nombre} (Stock: {producto.StockActual})");
            }
            if (cmbProductos.Items.Count > 0)
                cmbProductos.SelectedIndex = 0;
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbProductos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un producto.");
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Cantidad inválida.");
                return;
            }

            if (!double.TryParse(txtPrecioCompra.Text, out double precioCompra) || precioCompra < 0)
            {
                MessageBox.Show("Precio de compra inválido.");
                return;
            }

            int indice = cmbProductos.SelectedIndex;
            var producto = TiendaData.Productos[indice];

            // Actualizar stock
            producto.AumentarStock(cantidad);

            // Registrar abastecimiento
            TiendaData.Abastecimientos.Add(new Abastecimiento
            {
                Fecha = DateTime.Now,
                Distribuidor = producto.Distribuidor,
                ProductoCodigo = producto.Codigo,
                NombreProducto = producto.Nombre,
                CantidadAgregada = cantidad,
                PrecioCompraTotal = precioCompra * cantidad
            });

            // Guardar todo
            TiendaData.GuardarProductos();
            TiendaData.GuardarAbastecimientos();

            Administracion ad = new Administracion();
            ad.Show();
            this.Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Administracion ad = new Administracion();
            ad.Show();
            this.Close();
        }
    }
}
