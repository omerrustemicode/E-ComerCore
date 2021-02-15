using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E_ComerCore.Models
{
    [Table("Invoice")]
    public class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetails>();

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public int Status { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<InvoiceDetails> InvoiceDetails { get; set; }
    }
}
