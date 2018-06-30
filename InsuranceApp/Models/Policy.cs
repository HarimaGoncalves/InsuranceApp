using Newtonsoft.Json;
using System;

namespace InsuranceApp.Models
{
    public class Policy
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("amountInsured")]
        public double AmountInsured { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("inceptionDate")]
        public DateTime InceptionDate { get; set; }
        [JsonProperty("installmentPayment")]
        public bool InstallmentPayment { get; set; }
        [JsonProperty("clientId")]
        public string ClientId { get; set; }
        //References User
        //private User User { get; }
    }
}

