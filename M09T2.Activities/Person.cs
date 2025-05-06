using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M09T2.Activities
{
    public class Person
    {
        private readonly BankAccount _account;
        private readonly int _amount;         // quant vol retirar cada cop
        private readonly string _name;
        private readonly CancellationToken _tk;
        private readonly CancellationTokenSource _cts;

        public Person(string name, BankAccount acc, int amount)
        {
            _name = name;
            _account = acc;
            _amount = amount;
        }
        public Person(string name, BankAccount acc, int amount, CancellationToken token, CancellationTokenSource cts)
        {
            _name = name;
            _account = acc;
            _amount = amount;
            _tk = token;
            _cts = cts;
        }

        /// <summary>Intenta extreure fins que no pot retirar.</summary>
        public void Run()                     // mètode per al Thread
        {
            while (_account.TryToTakeMoney(_amount, _name))
            {
                Thread.Sleep(Random.Shared.Next(50, 150));   // simulació de temps
            }
            Console.WriteLine($"{_name} deixa d’intentar-ho: no hi ha diners.");
        }

        /// <summary>Act 9: Intenta extreure fins que no pot retirar. Asíncron (Task).</summary>
        public async Task RunAsync()
        {
            while (!_tk.IsCancellationRequested)
            {
                if (!_account.TryToTakeMoneyNoOverdraft(_amount, _name))
                {
                    // No hi ha fons: avisa tothom
                    _cts.Cancel();
                    break;
                }

                try
                {
                    await Task.Delay(Random.Shared.Next(50, 150), _tk); // pausa cooperativa
                }
                catch (OperationCanceledException) { Console.WriteLine($"{_name}: Operació Cancel·lada"); }
            }
            Console.WriteLine($"{_name} s’atura.");
        }
    }
}
