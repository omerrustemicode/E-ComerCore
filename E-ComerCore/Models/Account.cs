using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_ComerCore.Models
{
    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
            RoleAccount = new HashSet<RoleAccount>();
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<RoleAccount> RoleAccount { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }

    }
}
