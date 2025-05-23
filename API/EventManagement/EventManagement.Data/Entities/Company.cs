using EventManagement.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Entities
{
    [EntityTypeConfiguration(typeof(CompanyConfiguration))]
    public class Company
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool SubscriptionActive { get; set; }
    }
}
