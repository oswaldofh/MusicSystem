using Microsoft.AspNetCore.Identity;
using MusicSystem.Data.Entities;
using MusicSystem.Data;
using MusicSystem.Repository.IRpository;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Dtos;

namespace MusicSystem.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> AddUserAsync(AddUserDto userDto)
        {

            User user = new User
            {
                Address = userDto.Address,
                Document = userDto.Document,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                PhoneNumber = userDto.PhoneNumber,
                UserName = userDto.Document,
                UserType = userDto.UserType
            };

            IdentityResult result = await _userManager.CreateAsync(user, userDto.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            User newUser = await GetUserAsync(userDto.Document);
            await AddUserToRoleAsync(newUser, user.UserType.ToString());

            string token = await GenerateEmailConfirmationTokenAsync(newUser);
            await ConfirmEmailAsync(newUser, token);

            return newUser;

        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }

        }

        public async Task<User> GetUserAsync(string document)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == document);

        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId.ToString());
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);

        }

        public async Task<SignInResult> LoginAsync(LoginDto model)
        {
            return await _signInManager.PasswordSignInAsync(
               model.Username,
               model.Password,
               model.RememberMe,
               true); //TODO: modificar el valor FALSE HACE REFERENCIA AL CANTIDAD DE INTENTOS DE LOGUE Y BLOQUEA EL USERS

        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }


        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

    }
}
