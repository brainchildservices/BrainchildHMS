using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainchild.HMS.Data.DTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int PaymentTypeId { get; set; }
        public double PaymentAmount { get; set; }
        public string PaymentDescription { get; set; }
        public int BillingId { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
