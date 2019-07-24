using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Security.Claims;

namespace AspNetIdentity
{
    class Program
    {
        static void Main(string[] args)
        {
            var userName = "becker.utia.123@gmail.com";
            var userPassword = "Password123!";
            var userStore = new UserStore<IdentityUser>();
            var userManage = new UserManager<IdentityUser>(userStore);

            var creationResult = userManage.Create(new IdentityUser(userName), userPassword);

            Console.WriteLine($"Creation {creationResult}");
            var user = userManage.FindByName(userName);
            var claimResult = userManage.AddClaim(user.Id, new Claim("given_name", "Becker"));

            Console.WriteLine($"Claims {claimResult.Succeeded}");

            var isMath = userManage.CheckPassword(user, userPassword);
            Console.WriteLine($"Is User {isMath}");

            Console.ReadLine();
        }
    }
}
