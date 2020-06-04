using System;
using System.Collections.Generic;

namespace Laba_6_ADO_v._2
{
    public partial class Chemical
    {
        public Chemical()
        {
            Orderdetails = new HashSet<Orderdetails>();
        }

        public int Chemicalid { get; set; }
        public string Name { get; set; }
        public string Kindofchemical { get; set; }
        public string Application { get; set; }
        public string Companymanufacturer { get; set; }
        public int? Quantity { get; set; }
        public int? Price { get; set; }

        public virtual ICollection<Orderdetails> Orderdetails { get; set; }
    }
}
