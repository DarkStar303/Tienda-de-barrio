using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tienda_De_Barrio
{
    public class Usuario
    {
        public string Id { get; set; } // ← Ahora es seteable
        public string Nombre { get; set; }
        public string ContraseñaHash { get; set; } // ← Propiedad pública
        public string Rol { get; set; }

        // Constructor predeterminado
        public Usuario()
        {
            Id = Guid.NewGuid().ToString();
            Rol = "Empleado";
        }

        // Método para establecer contraseña (hash)
        public void EstablecerContraseña(string contraseña)
        {
            if (string.IsNullOrWhiteSpace(contraseña))
                throw new ArgumentException("La contraseña no puede estar vacía.");

            ContraseñaHash = Hash(contraseña);
        }

        // Método para verificar contraseña
        public bool VerificarContraseña(string contraseñaIngresada)
        {
            if (string.IsNullOrWhiteSpace(contraseñaIngresada))
                return false;

            string hashIngresado = Hash(contraseñaIngresada);
            return ContraseñaHash == hashIngresado;
        }

        // Método para saber si es admin
        public bool EsAdmin()
        {
            return Rol == "Admin";
        }

        // Método privado de utilidad
        private static string Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
