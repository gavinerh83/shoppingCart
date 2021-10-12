using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Models
{
    public class ActivationCode
    {
        public ActivationCode()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }
        
        public virtual Product Product { get; set; }
        public virtual Purchase Purchase { get; set; }

    }
}
