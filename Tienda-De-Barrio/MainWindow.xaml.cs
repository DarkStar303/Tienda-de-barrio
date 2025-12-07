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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tienda_De_Barrio
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TiendaData.CargarUsuarios();
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            string nombreUsuario = txtUsuario.Text.Trim();
            string contraseñaIngresada = txtPassword.Password.Trim();
            bool deseaIniciarComoAdmin = rbAdmin.IsChecked == true;

            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contraseñaIngresada))
            {
                MessageBox.Show("Por favor ingrese su nombre de usuario y contraseña.",
                                "Campos incompletos",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            Usuario usuarioEncontrado = TiendaData.Usuarios
                .FirstOrDefault(u => u.Nombre == nombreUsuario);

            if (usuarioEncontrado == null)
            {
                MessageBox.Show("Usuario no encontrado. Verifique el nombre e intente de nuevo.",
                                "Usuario incorrecto",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            if (!usuarioEncontrado.VerificarContraseña(contraseñaIngresada))
            {
                MessageBox.Show("Contraseña incorrecta. Inténtelo nuevamente.",
                                "Error de autenticación",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            if (deseaIniciarComoAdmin && !usuarioEncontrado.EsAdmin())
            {
                MessageBox.Show("No tiene permisos de administrador.\nSolo puede iniciar sesión como EMPLEADO.",
                                "Acceso restringido",
                                MessageBoxButton.OK,
                                MessageBoxImage.Stop);
                return;
            }

            // ✅ CARGAR PRODUCTOS ANTES DE ABRIR CUALQUIER VENTANA
            TiendaData.CargarProductos();

            // ✅ ABRIR VENTANA SEGÚN EL ROL
            if (usuarioEncontrado.EsAdmin())
            {
                var ventanaAdmin = new Administracion();
                ventanaAdmin.Show();
            }
            else
            {
                var ventanaVenta = new Venta();
                ventanaVenta.Show();
            }

            this.Close();
        }
    }
}
