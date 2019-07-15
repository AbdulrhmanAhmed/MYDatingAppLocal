using System;
using System.Threading.Tasks;
using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _DataContext;
        public AuthRepository(DataContext DataContext)
        {
            _DataContext = DataContext;

        }
        public async Task<User> Login(string username, string password)
        {
              var user =await _DataContext.Users.FirstOrDefaultAsync();
              if(user==null)
              return null;
              if(!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt))
              return null;
              return user;
         }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac=new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
            var ComputeHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < ComputeHash.Length;i++)
                 
                 {
                     if(ComputeHash[i]!=passwordHash[i])
                     return false;
                 }
                return true;
            }

             }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordhash;
            byte[] passwordsalt;
            createPasswordHash(password, out passwordhash, out passwordsalt);
            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordsalt;
            await _DataContext.Users.AddAsync(user);
            await _DataContext.SaveChangesAsync();
            return user;
        }

        private void createPasswordHash(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        public async Task<bool> UserExists(string username)
        {
            if(await _DataContext.Users.AnyAsync(x=>x.Username==username))
            return true;

            return false;
        }
    }
}