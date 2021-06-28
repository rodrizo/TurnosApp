using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class LoginController : Controller 
    {
        private readonly TurnosContext _context;
        public LoginController(TurnosContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(Login login)
        {
            if(ModelState.IsValid)//verificamos que el formulario ha pasado la verificación de data anotations
            {
                //EncriptarPassword
                string passwordEncriptado = Encriptar(login.Password);//nos encripta el pass que viene desde el form, luego lo compara con el pass de la tabla
                
                var loginUsuario = _context.Login.Where(l => l.Usuario == login.Usuario && l.Password == passwordEncriptado)
                .FirstOrDefault(); //ejecutamos una consulta para buscar en el modelo login si el usuario y contraseña ingresado existe.

                if(loginUsuario != null)//comprueba si el login fue satisfactorio
                {
                    HttpContext.Session.SetString("usuario", loginUsuario.Usuario);//httpcontext, nos servirá para la seguridad de la sesión
                    return RedirectToAction("Index", "Home");
                }else{
                    ViewData["errorLogin"] = "Los datos ingresados son incorrectos.";
                    return View("Index");
                }
            }
            return View("Index");
        }

        public string Encriptar(string password)//método de encriptación estándar
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));//conversión a bytes

                StringBuilder stringBuilder = new StringBuilder();
                for(int i=0; i<bytes.Length; i++)
                {
                    stringBuilder.Append(bytes[i].ToString("x2"));
                }

                return stringBuilder.ToString();//una vez encriptado, retorna un string hexadecimal 
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

    }
}