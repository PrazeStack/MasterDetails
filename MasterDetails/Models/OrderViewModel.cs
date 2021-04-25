using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterDetails.Models
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {

            this.OrderDetails = new HashSet<OrderDetailViewModel>();
        }


        public System.Guid MasterId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public System.DateTime OrderDate { get; set; }

        public virtual ICollection<OrderDetailViewModel> OrderDetails { get; set; }
    
    }

    public class OrderDetailViewModel
    {
        public System.Guid DetailsId { get; set; }
        public System.Guid MasterId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public string State { get; set; }

        public virtual OrderMaster OrderMaster { get; set; }
    }


}