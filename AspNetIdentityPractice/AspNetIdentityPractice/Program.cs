using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace AspNetIdentityPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManage = new UserManager<IdentityUser>(userStore);

            var creationResult = userManage.Create(new IdentityUser("becker.utia.123@gmail.com"), "Password123!");

            Console.WriteLine($"Creation {creationResult}");
            Console.ReadLine();
        }
    }
}
