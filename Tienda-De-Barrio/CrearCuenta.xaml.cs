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
    /// Lógica de interacción para CrearCuenta.xaml
    /// </summary>
    public partial class CrearCuenta : Window
    {
        public CrearCuenta()
        {
            InitializeComponent();
        }

        private void CrearCuenta_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtUsuario.Text.Trim();
            string contraseña = txtPassword.Password.Trim();
            string rol = rbAdmin.IsChecked == true ? "Admin" : "Empleado";

            // Validar campos vacíos
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contraseña))
            {
                MessageBox.Show("Debe ingresar nombre de usuario y contraseña.",
                                "Campos incompletos",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // Verificar si el usuario ya existe
            bool existe = TiendaData.Usuarios.Exists(u => u.Nombre == nombre);
            if (existe)
            {
                MessageBox.Show("Ya existe un usuario con ese nombre.",
                                "Usuario duplicado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            // ✅ CREAR EL NUEVO USUARIO (forma correcta)
            var nuevoUsuario = new Usuario();           // Usa el constructor vacío
            nuevoUsuario.Nombre = nombre;              // Asigna el nombre
            nuevoUsuario.EstablecerContraseña(contraseña); // ¡Esto cifra la contraseña!
            nuevoUsuario.Rol = rol;                    // Asigna el rol

            // Guardar en la lista y en el archivo
            TiendaData.Usuarios.Add(nuevoUsuario);
            TiendaData.GuardarUsuarios();
            Administracion ad = new Administracion();
            ad.Show();

            MessageBox.Show($"Cuenta creada exitosamente como {rol}.",
                            "Éxito",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
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
