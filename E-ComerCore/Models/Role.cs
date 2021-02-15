using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_ComerCore.Models
{
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            RoleAccount = new HashSet<RoleAccount>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<RoleAccount> RoleAccount { get; set; }
    }
}
