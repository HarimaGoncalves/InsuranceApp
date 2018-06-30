using InsuranceApp.Models;
using System.Collections.Generic;

namespace InsuranceApp.Models
{
    public class RootObject
    {
        public List<Client> clients { get; set; }

        public List<Policy> policies { get; set; }
    }
}