using POCApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace POCApi.Context
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            base.Seed(context);

            Users appUsers = new Users()
            {
                Name = "AppUser",
                PasswordHash = "7EeyEl2uUVvw6VLaNZZ9mHE+Ixe5oe4n1JTFISqXdRdn2nRM9WRWUOYWY4GFaCY5rQKM7Y+pkcahs69T+sP5RgmpitB3wNOUNUwR6gD8xZksGrBD+ds/uQDhpsCmAhRRXPOmQAOCaQgQYfJ2h4SOhKA5M3k8zcjg2EwCuQ23uYbShGcLYYS4El1kRHF6WnNE9NqqKni3N/49z4UVFcZl6R/O2Osk9lhJWYePDgirbek2IyOnr1N8yyOPRvGN8fLBGysmqlMs8/uNaNCYe6V86Vvb4hjd0wBzNsRXJr4d8gHKCE3RvVZXuFACrm+Tf0NtF6vFeQkjHFiMeJUmfa4ToQ==",
                PasswordSalt = "Fvs8d/iItB+wKjMWYYgNpfGiFrRce57kzxBFeRGsYQJysJempam8JR59PYKGUTuxTLFzHEqCl/YZwGbX0eW8sA==",
                UserName = "AppUser",
                Role = "Admin,User"
            };
            context.Users.Add(appUsers);
            context.SaveChanges();
        }
    }
}