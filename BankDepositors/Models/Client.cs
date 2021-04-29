using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankDepositors
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Passport { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public int StaffId { get; set; }
        public Staff Staff { get; set; }
        public List<Deposit> Deposits { get; set; } = new List<Deposit>();
        public List<DepositClient> DepositClients { get; set; } = new List<DepositClient>();

    }
}
