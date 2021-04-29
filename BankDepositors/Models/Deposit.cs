using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankDepositors
{
    public class Deposit
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Term { get; set; }
        public decimal Percent { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public List<Client> Clients { get; set; } = new List<Client>();
        public List<DepositClient> DepositClients { get; set; } = new List<DepositClient>();
    }
}
