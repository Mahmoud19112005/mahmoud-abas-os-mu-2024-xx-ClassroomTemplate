using System;
using System.Threading;

namespace BankAccountApp
{
    public class BankAccount
    {
        private decimal _balance;
        private readonly object _lock = new object();

        // Constructor
        public BankAccount(decimal initialBalance)
        {
            _balance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            lock (_lock)
            {
                _balance += amount;
                Console.WriteLine($"Deposited: {amount}, New Balance: {_balance}");
            }
        }

        public void Withdraw(decimal amount)
        {
            lock (_lock)
            {
                if (_balance >= amount)
                {
                    _balance -= amount;
                    Console.WriteLine($"Withdrew: {amount}, Remaining Balance: {_balance}");
                }
                else
                {
                    Console.WriteLine($"Insufficient funds for withdrawal of: {amount}");
                }
            }
        }

        public decimal GetBalance()
        {
            lock (_lock)
            {
                return _balance;
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("BankAccount simulation started.");

            BankAccount account = new BankAccount(1000);

            Thread[] depositThreads = new Thread[5];
            Thread[] withdrawThreads = new Thread[5];

            for (int i = 0; i < 5; i++)
            {
                depositThreads[i] = new Thread(() => account.Deposit(200));
                withdrawThreads[i] = new Thread(() => account.Withdraw(150));
            }

            foreach (var thread in depositThreads)
            {
                thread.Start();
            }

            foreach (var thread in withdrawThreads)
            {
                thread.Start();
            }

            foreach (var thread in depositThreads)
            {
                thread.Join();
            }

            foreach (var thread in withdrawThreads)
            {
                thread.Join();
            }

            Console.WriteLine($"Final Balance: {account.GetBalance()}");
            Console.WriteLine("BankAccount simulation completed.");
        }
    }
}
