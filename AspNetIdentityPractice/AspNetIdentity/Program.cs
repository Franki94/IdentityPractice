using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;

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

    public class CustomUser : IUser<int>
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }

    public class CustomUserDbContext : DbContext
    {
        public CustomUserDbContext() : base("DefaultConnection")
        {

        }
        public DbSet<CustomUser> Users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var userEntity = modelBuilder.Entity<CustomUser>();
            userEntity.HasKey(x => x.Id);
            userEntity.Property(x => x.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            userEntity.Property(x => x.UserName).IsRequired().HasMaxLength(256).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") {IsUnique = true }));
            base.OnModelCreating(modelBuilder);
        }
    }

    public class CustomUserStore : IUserPasswordStore<CustomUser, int>
    {
        private readonly CustomUserDbContext context;
        public CustomUserStore(CustomUserDbContext context)
        {
            this.context = context;
        }
        public Task CreateAsync(CustomUser user)
        {
            context.Users.Add(user);
            return context.SaveChangesAsync();
        }

        public Task DeleteAsync(CustomUser user)
        {
            context.Users.Remove(user);
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Task<CustomUser> FindByIdAsync(int userId)
        {
            return context.Users.FindAsync(userId);
        }

        public Task<CustomUser> FindByNameAsync(string userName)
        {
            return context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public Task<string> GetPasswordHashAsync(CustomUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(CustomUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(CustomUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return context.SaveChangesAsync();
        }

        public Task UpdateAsync(CustomUser user)
        {
            context.Users.Attach(user);
            return context.SaveChangesAsync();
        }
    }

}

