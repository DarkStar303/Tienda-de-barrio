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
    /// Lógica de interacción para Administracion.xaml
    /// </summary>
    public partial class Administracion : Window
    {
        public Administracion()
        {
            InitializeComponent();
            CargarResumen(); // ← Carga las ventas al abrir
        }

        private void CargarResumen()
        {
            var resumen = new StringBuilder();

            // === VENTAS ===
            resumen.AppendLine("📋 ÚLTIMAS VENTAS:");
            if (TiendaData.Ventas?.Count > 0)
            {
                var ventas = TiendaData.Ventas
                    .OrderByDescending(v => v.Fecha)
                    .Take(5);

                foreach (var v in ventas)
                {
                    resumen.AppendLine($"  • {v.Fecha:dd/MM HH:mm} - Bs {v.Total:F2}");
                }
            }
            else
            {
                resumen.AppendLine("  No hay ventas registradas.");
            }

            resumen.AppendLine("\n📦 ÚLTIMOS ABASTECIMIENTOS:");
            if (TiendaData.Abastecimientos?.Count > 0)
            {
                var abast = TiendaData.Abastecimientos
                    .OrderByDescending(a => a.Fecha)
                    .Take(5);

                foreach (var a in abast)
                {
                    resumen.AppendLine($"  • {a.Fecha:dd/MM HH:mm} - {a.NombreProducto} x{a.CantidadAgregada} (Bs {a.PrecioCompraTotal:F2})");
                }
            }
            else
            {
                resumen.AppendLine("  No hay abastecimientos registrados.");
            }

            txtResumen.Text = resumen.ToString();
        }

        private void Vender_CLICK(object sender, RoutedEventArgs e)
        {
            var ven = new Venta();
            ven.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var np = new NuevoProducto();
            np.Show();
            this.Close();
        }

        private void Crear_Click(object sender, RoutedEventArgs e)
        {
            CrearCuenta cc = new CrearCuenta();
            cc.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AumentarStock a= new AumentarStock();
            a.Show();
            this.Close();

        }
    }
}
