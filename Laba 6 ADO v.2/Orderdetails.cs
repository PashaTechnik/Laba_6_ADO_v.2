using System;
using System.Collections.Generic;

namespace Laba_6_ADO_v._2
{
    public partial class Orderdetails
    {
        public int Orderid { get; set; }
        public int Chemicalid { get; set; }
        public int? Quantity { get; set; }
        public int? Price { get; set; }

        public virtual Chemical Chemical { get; set; }
        public virtual Orders Order { get; set; }
    }
}
