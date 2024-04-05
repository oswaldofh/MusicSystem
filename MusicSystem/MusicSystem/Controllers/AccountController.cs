using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Data;
using MusicSystem.Data.Entities;
using MusicSystem.Dtos;
using MusicSystem.Enums;
using MusicSystem.Helper;
using MusicSystem.Repository.IRpository;

namespace MusicSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly DataContext _context;
        private readonly IComboHelper _comboHelper;

        public AccountController(IUserRepository userRepository, DataContext context, IComboHelper comboHelper)
        {
            _userRepository = userRepository;
            _context = context;
            _comboHelper = comboHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) //User.Identity devuelve siempre el usuario logueado
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userRepository.LoginAsync(model); //FUNCION QUE LOGUEA AL USUARIO

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut) //PARA SABER SI EL USUARIO ESTA BLOQUEADO
                {
                    ModelState.AddModelError(string.Empty, "Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
                }
                else if (result.IsNotAllowed) //QUE NO SE HA CONFIRMADO
                {
                    ModelState.AddModelError(string.Empty, "El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitarte  en el sistema.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
                }

            }

            return View(model);
        }


        public async Task<IActionResult> Register()
        {
            AddUserDto model = new AddUserDto
            {
                UserType = UserType.User,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserDto model)
        {
            if (ModelState.IsValid)
            {


                var existUser = await _userRepository.GetUserAsync(model.Document);

                if (existUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Esta identificación ya está siendo usado por otro usuario");
                    return View(model);
                }

                var user = await _userRepository.AddUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "No se registro el usuario.");
                    return View(model);
                }
                //SE LOGUEA EL USUARIO CUANDO SE CREA
                LoginDto loginDto = new LoginDto
                {
                    Password = model.Password,
                    RememberMe = false,
                    Username = model.Document
                };

                Microsoft.AspNetCore.Identity.SignInResult result = await _userRepository.LoginAsync(loginDto);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut) //PARA SABER SI EL USUARIO ESTA BLOQUEADO
                {
                    ModelState.AddModelError(string.Empty, "Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
                }
                else if (result.IsNotAllowed) //QUE NO SE HA CONFIRMADO
                {
                    ModelState.AddModelError(string.Empty, "El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitarte  en el sistema.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Identificación o contraseña incorrectos.");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Los datos no son correctos");
            }


            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            AddUserDto model = new AddUserDto
            {
                UserType = UserType.Admin,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] //SOLO LOS ADMINISTRADORES
        public async Task<IActionResult> Create(AddUserDto model)
        {
            if (ModelState.IsValid)
            {
                var existUser = await _userRepository.GetUserAsync(model.Document);

                if (existUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Esta identificación ya está siendo usado por otro usuario");
                    return View(model);
                }

                var user = await _userRepository.AddUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "No se registro el usuario.");
                    return View(model);
                }
                //SE LOGUEA EL USUARIO CUANDO SE CREA
                LoginDto loginDto = new LoginDto
                {
                    Password = model.Password,
                    RememberMe = false,
                    Username = model.Document
                };

                Microsoft.AspNetCore.Identity.SignInResult result = await _userRepository.LoginAsync(loginDto);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut) //PARA SABER SI EL USUARIO ESTA BLOQUEADO
                {
                    ModelState.AddModelError(string.Empty, "Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
                }
                else if (result.IsNotAllowed) //QUE NO SE HA CONFIRMADO
                {
                    ModelState.AddModelError(string.Empty, "El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitarte  en el sistema.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Identificación o contraseña incorrectos.");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Los datos no son correctos");
            }


            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userRepository.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }


    }
}
