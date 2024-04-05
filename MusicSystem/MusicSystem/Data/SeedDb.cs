using Mono.TextTemplating;
using MusicSystem.Data.Entities;
using MusicSystem.Enums;
using MusicSystem.Repository.IRpository;
using System.Diagnostics.Metrics;

namespace MusicSystem.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;

        public SeedDb(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        public async Task SeedAsync()
        {
            //ESTA FUNCION CREA LA BASE DE DATOS Y LE APLICA LAS MIGRACIONES (hace la funcion de update-datebase) en codigo
            await _context.Database.EnsureCreatedAsync();

           
            await CheckRolesAsync();
            await CheckUserAsync("123456789", "Antonio Fuentes", "antonio@yopmail.com", "3135232226", "Calle 45 # 84-57", UserType.Admin);
            await CheckUserAsync("987654321", "Oswaldo Fuentes", "oswaldo@yopmail.com", "3135232226", "Calle 45 # 84-57", UserType.User);

        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userRepository.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    Email = email,
                    UserName = document,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType,
                };

                await _userRepository.AddUserAsync(user, "123456");
                await _userRepository.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userRepository.GenerateEmailConfirmationTokenAsync(user);
                await _userRepository.ConfirmEmailAsync(user, token);

            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userRepository.CheckRoleAsync(UserType.Admin.ToString());
            await _userRepository.CheckRoleAsync(UserType.User.ToString());

        }
    }
}
