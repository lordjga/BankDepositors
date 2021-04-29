using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankDepositors
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public List<Deposit> Deposits { get; set; } = new List<Deposit>();
    }
}