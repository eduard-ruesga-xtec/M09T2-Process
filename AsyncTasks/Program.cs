using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace AsyncTasks
{
    internal class Program
    {
        //Mesures de temps:
        static Stopwatch stopWatch;
        static async Task Main(string[] args)
        {
            stopWatch = new Stopwatch();


            /*** Exemples de tasques asincronesfjoc sequencia i paral·lel***/
           /* Console.BackgroundColor = ConsoleColor.DarkBlue;
            stopWatch.Start();

            int i1 = await LongWork("Task 1", 500000);          //Crida directa a la tasca, s'executa inmediatament.
            Console.WriteLine("Codi entre esperes");            //Amb await es bloqueja fins no acabar la tasca
            int i2 = await LongWork("Task 2", 400000);          

            stopWatch.Stop();

            Console.WriteLine($"Resultat t1: {i1} \t Resultat t2: {i2}");
            Console.WriteLine($"Les dos tasques en sequencia han finalitzat en: {stopWatch.Elapsed}\n");

            //Un altre forma, pero les tasques s'executen sequencialment
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            int[] iArr = new int[2];
            stopWatch = Stopwatch.StartNew();

            Task<int> t3 = LongWork("Task 3", 500000);          //S'executa inmediatament
            Task<int> t4 = LongWork("Task 4", 400000);

            //Abans de l'await podem executar el codi que vulguem
            Console.WriteLine("Despres del run, abans de l'await");

            iArr = await Task.WhenAll(t3, t4);

            stopWatch.Stop();
            Console.WriteLine($"Resultat t3: {iArr[0]} \t Resultat t4: {iArr[1]}");
            Console.WriteLine($"Les dos tasques en sequencia amb WhenAll han finalitzat en: {stopWatch.Elapsed} \n");

            //Forma optima per fer en PARAL·LEL: Task.Run() -> fixa cada run en un nou fil.
            Console.BackgroundColor = ConsoleColor.Green;
            int[] iArr2 = new int[2];

            Console.WriteLine("Paral·lel amb Task.run()");

            stopWatch = Stopwatch.StartNew();
            Task<int> t5 = Task.Run(() =>
            {
                return LongWork("Task 5", 500000);
            });
            Task<int> t6 = Task.Run(() =>
            {
                return LongWork("Task 6", 400000);
            });

            //Abans de l'await podem executar el codi que vulguem
            Console.WriteLine("Despres del run, abans de l'await");

            iArr2 = await Task.WhenAll(t5, t6);

            stopWatch.Stop();
            Console.WriteLine($"Resultat t1: {iArr[0]} \t Resultat t2: {iArr[1]}");
            Console.WriteLine($"Les dos tasques en paral·lel han finalitzat en: {stopWatch.Elapsed}");
            Console.ResetColor();
            */
            /*******/

            //Exemple 2:
            await WhenAllDemoAsync();

            /*** Sleep vs Delay ***/
            //exemple de l'us optim dels fils per part de Task
            //await SleepVsDelay();

            /**** Cancel·lacio de tasques **/
            await CancelTaskExemple();



        }
        static async Task<int> LongWork(string taskName, int limit)
        {
            int result = 0;
            Console.WriteLine($"{taskName} comença en el fil {Thread.CurrentThread.ManagedThreadId}!");
            for (int i = 0; i < limit; i++)
            {
                Task.Delay(10);
                result += i;
            }
            Console.WriteLine($"{taskName} completada!.");
            return result;
        }
        static async Task SleepVsDelay()
        {
            //Sleep bloqueja el fil actual
            Console.WriteLine($"ID del fil ABANS de dormir: {Environment.CurrentManagedThreadId}");
            Thread.Sleep(1000);
            Console.WriteLine($"ID del fil DESPRES de dormir:  {Environment.CurrentManagedThreadId}");

            //Delay afegeix enderreriment sense bloquejar el fil que el crida.
            Console.WriteLine($"ID del fil ABANS de dormir: {Environment.CurrentManagedThreadId}");
            await Task.Delay(1000);
            //Segurament el ID del fil canvia, salta a un altre i no bloquejar el 1.
            Console.WriteLine($"ID del fil DESPRES de dormir:  {Environment.CurrentManagedThreadId}");
        
        }
        static async Task WhenAllDemoAsync()
        {
            string[] urls =
            {
                "https://dotnet.microsoft.com",
                "https://learn.microsoft.com",
                "https://github.com"
            };

            HttpClient client = new HttpClient();

            // Llista de tasques de baixada (I/O-bound)
            Task<byte[]>[] downloads = urls
                .Select(u => client.GetByteArrayAsync(u))
                .ToArray();

            byte[][] pages = await Task.WhenAll(downloads);   //Espera sense bloquejar

            Console.WriteLine($"Total bytes descarregats: {pages.Sum(p => p.Length)}");     //suma de bits descarregats en total.
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
