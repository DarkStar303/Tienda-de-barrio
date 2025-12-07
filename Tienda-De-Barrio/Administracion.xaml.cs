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
        }

        private void Vender_CLICK(object sender, RoutedEventArgs e)
        {
            Venta ven= new Venta();
            ven.Show();
            this.Close();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NuevoProducto np =new NuevoProducto();
            np.Show();
            this.Close();
        }

        private void Crear_Click(object sender, RoutedEventArgs e)
        {
            CrearCuenta cc= new CrearCuenta();
            cc.Show();
            this.Close();
        }

    }
}
