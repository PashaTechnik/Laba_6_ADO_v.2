using System;
using System.Collections.Generic;

namespace Laba_6_ADO_v._2
{
    public partial class Orders
    {
        public Orders()
        {
            Orderdetails = new HashSet<Orderdetails>();
        }

        public int Orderid { get; set; }
        public string Companybuyer { get; set; }

        public virtual ICollection<Orderdetails> Orderdetails { get; set; }
    }
}
