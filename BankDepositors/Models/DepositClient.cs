using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankDepositors
{
    public class DepositClient
    {
        [Key, ForeignKey("Deposit"), Column(Order = 0)]
        public int Deposit_Id { get; set; }
        public Deposit Deposit { get; set; }

        [Key, ForeignKey("Client"), Column(Order = 1)] 
        public int Client_Id { get; set; }
        public Client Client { get; set; }

        public decimal Enrollment { get; set; }
        public DateTime DateOfEnrollment { get; set; }
        public decimal Summ { get; set; }
        public DateTime EndDate { get; set; }
        
    }
}
