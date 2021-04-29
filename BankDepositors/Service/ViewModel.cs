using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BankDepositors
{
    public class ViewModel : INotifyPropertyChanged
    {
        List<string> liststrCln;
        List<string> liststrDep;
        public BankContext bd = new BankContext();
        private List<string> DepClColl;
        public IList<string> ListCln
        {
            get
            {
                liststrCln = new List<string>();
                var clients = (from p in bd.Clients
                    select new { Id = p.Id, Name = p.Surname + " " + p.Name.Substring(0, 1) + "." }).ToList();

                foreach (var item in clients)
                {
                    liststrCln.Add(item.Id + ". " + item.Name);
                }

                return liststrCln;
            }
        }
        public IList<string> ListDep
        {
            get
            {
                liststrDep = new List<string>();
                var deposits = (from p in bd.Deposits
                    select new { Id = p.Id, Name = p.Name}).ToList();

                foreach (var item in deposits)
                {
                    liststrDep.Add(item.Id + ". " + item.Name);
                }

                return liststrDep;
            }
        }
        /*
        public ObservableCollection<DepositClient> ListDepCLn
        {
            get
            {
                DepClColl = new List<string>();
                ObservableCollection<DepositClient> DepositClients = new ObservableCollection<DepositClient>();
                var depClColl = (from p in bd.DepositClients
                    select new 
                    {
                        Id = p.Client_Id
                    }).ToList();
               foreach (var item in bd.DepositClients)
               {
                   DepositClients.Add(item);
                }

               return DepositClients;
            }
        }*/

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}