using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M09T2.Activities
{
    public class BankAccount
    {
        private readonly object _lock = new();     // candau 
        public int Balance { get; private set; }   // saldo en euros (enter per simplicitat)

        public BankAccount(int initial) => Balance = initial;

        /// <summary>Intenta retirar <paramref name="amount"/> €.  
        /// Retorna true si s’ha pogut, false si el saldo no n’hi ha prou.</summary>
        public bool TryToTakeMoney(int amount, string who)
        {
            lock (_lock)                            // exclusió mútua
            {
                if (Balance <= 0) return false;     // compte “mort”
                if (Balance < amount) return false; // no hi ha prou

                Balance -= amount;
                Console.WriteLine(
                    $"{who,-10} retira {amount,4} €  | saldo restant: {Balance,4} €");
                return true;
            }
        }

        /// <summary>
        /// Intenta retirar l’import. Retorna true si hi ha diners suficients.
        /// </summary>
        public bool TryToTakeMoneyNoOverdraft(int amount, string who)
        {
            lock (_lock)
            {
                if (Balance < amount) return false;   // no hi ha prou diners
                Balance -= amount;
                Console.WriteLine(
                    $"{who,-8} retira {amount,4} € | saldo restant: {Balance,4} €");
                return true;
            }
        }
    }
}
