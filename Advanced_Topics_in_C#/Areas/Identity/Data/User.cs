using Microsoft.AspNetCore.Identity;
using Lab02.Models;
namespace Lab02.Areas.Identity.Data
{
    public class User : IdentityUser
    {
        public HashSet<Account> Accounts { get; set; } = new HashSet<Account>();
    }
}
    