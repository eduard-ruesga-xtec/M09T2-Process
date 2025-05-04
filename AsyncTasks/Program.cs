namespace AsyncTasks
{
    internal class Program
    {
        static void Main(string[] args)
        {

            /**** Cancel·lacio de tasques **/
            CancelTaskExemple();


        }


        static async Task CancelTaskExemple()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken cancellationToken = cts.Token;

            Task t1 = Task.Run(() => { CountWithCancel(cancellationToken); });
            Console.ReadLine();

            cts.Cancel();
            t1.Wait();
            Console.WriteLine("Tasca escoltar teclat finalitzada");

        }
        static void CountWithCancel(CancellationToken token)
        {
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(1000);
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("Counting:{0}  Tasca que s'executa en el fil: {1}", i, Thread.CurrentThread.ManagedThreadId);
                Console.ResetColor();

                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Interrupcio!");
                    return;
                }

            }
        }
    }
}
