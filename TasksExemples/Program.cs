namespace TasksExemples
{
    internal class Program
    {
        static int count = 0;
        static async Task Main(string[] args)
        {
            /*** Formes de crear tasques ***/
            //Crear una tasca que crida un metode
            /*Task tK = new Task(Counting);

            //Crear una tasca amb Lambda
            Task tK2 = new Task(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(100);
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine("tk2: Tasca que s'executa en el fil: {0}", Thread.CurrentThread.ManagedThreadId);
                }
            });

            tK.Start();
            tK2.Start();

            Task tK3 = Task.Run(() => MaxCounting(10));

            tk.wait();
            tk2.wait();
            tk3.wait();

            Console.WriteLine("Finalitzacio del bloc");*/

            /*** Retorn d'informacio per part d'una tasca ***/
            /* Task<int> calcul = Task.Run(() =>
            {
                // codi pesat
                return (10);       // resultat ⇢ Task<int>
            });

            int val = await calcul;         // o calcul.Result
            Console.WriteLine(val);

            Console.WriteLine("Finalitzacio del bloc");*/

            FinishAllTask();
            FinishAnyTask();
            Console.ReadLine();
            
        }

        static void Counting()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Counting:{0}  Tasca que s'executa en el fil: {1}", i, Thread.CurrentThread.ManagedThreadId);
                Console.ResetColor();
            }
        }
        static void MaxCounting(int max)
        {
            for (int i = 0; i < max; i++)
            {
                Thread.Sleep(100);
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Counting:{0}  Tasca que s'executa en el fil: {1}", i, Thread.CurrentThread.ManagedThreadId);
                Console.ResetColor();
            }
        }

        static void FinishAllTask()
        {
            Console.WriteLine("Iniciant tasques t1 i t2");
            Task t1 = Task.Run(() => { MaxCounting(10); });
            Task t2 = Task.Run(() => { MaxCounting(15); });

            Task.WaitAll(t1, t2);

            Console.WriteLine("Finalitzades les dos tasques");
        }
        static void FinishAnyTask()
        {
            Console.WriteLine("Iniciant tasques t1 i t2");
            Task t1 = Task.Run(() => { MaxCounting(10); });
            Task t2 = Task.Run(() => { MaxCounting(15); });

            Task.WaitAll(t1, t2);

            Console.WriteLine("Finalitzades les una de dues tasques");
        }
    }
}
