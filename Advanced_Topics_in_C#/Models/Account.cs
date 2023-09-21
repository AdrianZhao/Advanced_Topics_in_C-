using Lab02.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Lab02.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        [Range(0, Int32.MaxValue)]
        public int Deposit {  get; set; }
    }
}
