using M09T2.Act6;
using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace M09T2.Activities
{
    internal class Program
    {
        /*** Act 10 ***/
        const char sprite = '@';
        const int freq = 100;                          //Frequencia de refresc en hz
        static int columns = Console.BufferWidth;
        static int rows = Console.BufferHeight;

        static CancellationTokenSource AnimationCts;
        static CancellationToken AnimCancelToken;


        static async Task Main(string[] args)
        {
            /*** Act 6 ***/
            //CamelRace();

            /*** Act 7 ***/
            //Act7BankAccount();

            /*** Act 9 ***/
            MakeBreakfast();
            await MakeBreakfastAsync();

            /*** Act 9 ***/
            //await Act9BankAccount();

            /*** Act 10 ***/
            //await Act10();
        }

        static async Task MakeBreakfastAsync()
        {

            Console.WriteLine("\n== Esmorzar PARAL·LEL ==");
            var sw = Stopwatch.StartNew();
           
            var cafe = Tasca("Cafè", 5, ConsoleColor.DarkYellow);
            var ous = Tasca("Ous ferrats", 10, ConsoleColor.Yellow);
            var torrat = Tasca("Pa torrat", 7, ConsoleColor.DarkMagenta)
                         .ContinueWith(_ => Tasca("Melmelada", 2, ConsoleColor.Magenta));  // la melmelada espera el pa
            
            var poma = Tasca("Poma", 3, ConsoleColor.Green);

            // Esperem que tot acabi (melmelada està “enganxada” al pa)
            //await Task.WhenAll(cafe, ous, torrat, poma);

            var a = new Task(() => { Task.WhenAll(cafe, ous); torrat.Start(); });

            Console.ResetColor();
            Console.WriteLine($"Tot emplatat! Temps total: {sw.Elapsed:mm\\:ss\\.fff}");
        }

        static async Task Tasca(string nom, int segons, ConsoleColor color)
        {
            lock (Console.Out) { Console.ForegroundColor = color; Console.WriteLine($"-> Inici {nom} \t Fil:{Thread.CurrentThread.ManagedThreadId}"); }
            await Task.Delay(segons * 1000);
            lock (Console.Out) { Console.ForegroundColor = color; Console.WriteLine($"-> Fi    {nom} \t Fil:{Thread.CurrentThread.ManagedThreadId}"); }
        }

        static void MakeBreakfast()
        {
            Console.WriteLine("== Esmorzar SEQÜENCIAL ==");
            var sw = Stopwatch.StartNew();

            FerCafe();
            FerOus();
            FerPaTorrat();
            UntarMelmelada();
            PelarPoma();

            Console.ResetColor();
            Console.WriteLine($"Tot emplatat! Temps total: {sw.Elapsed:mm\\:ss\\.fff}");
        }

        // -------- “Tasques” (simulació) --------
        static void FerCafe() => Simula("Cafè", 5, ConsoleColor.DarkYellow);
        static void FerOus() => Simula("Ous ferrats", 10, ConsoleColor.Yellow);
        static void FerPaTorrat() => Simula("Pa torrat", 7, ConsoleColor.DarkMagenta);
        static void UntarMelmelada() => Simula("Melmelada", 2, ConsoleColor.Magenta);
        static void PelarPoma() => Simula("Poma", 3, ConsoleColor.Green);

        static void Simula(string nom, int segons, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"-> Inici {nom}");
            Thread.Sleep(segons * 1000);
            Console.WriteLine($"-> Fi    {nom}");
        }

        /// <summary>
        /// Act 9: Agafant l’exercici 7 ara implementa fent servir OOP i tasks. 
        /// Ademés el banc no pot donar més diners que els que hi ha al compte corrent,
        /// si una persona vol treure 50€ i en el compte queden 25€, la persona no podrà treure els diners. 
        /// En el moment que no poden treure diners les persones paren de fer-ho. 
        /// </summary>
        /// <returns></returns>
        static async Task Act9BankAccount()
        {
            var account = new BankAccount(initial: 500);

            using var cts = new CancellationTokenSource();
            CancellationToken tk = cts.Token;

            var people = new[]
            {
                new Person("Anna",  account, 40, tk, cts),
                new Person("Biel",  account, 70, tk, cts),
                new Person("Clara", account, 30, tk, cts),
                new Person("David", account, 90, tk, cts)
            };

            var tasks = people.Select(p => Task.Run(() => p.RunAsync())).ToArray();

            await Task.WhenAll(tasks); //Accepta una array de Tasks

            Console.WriteLine($"\nTothom para d'extreure diners. Saldo final: {account.Balance} €");
        }

        /// <summary>
        /// Implementa amb OOP i Threads un sistema bancari on diferents persones volen extreure diners d’un mateix compte. 
        /// Cada persona vol treure una quantitat diferent de diners tantes vegades com pugui. 
        /// Un cop el compte tingui x < = 0 euros, ningú podrà treure diners i pararan d’intentar-ho.
        /// </summary>
        static void Act7BankAccount()
        {
            var account = new BankAccount(initial: 500);   // saldo inicial
            var people = new[]
            {
                new Person("Anna",  account, 40),
                new Person("Biel",  account, 70),
                new Person("Clara", account, 30),
                new Person("David", account, 90)
            };

            // Convertim cada persona en un Thread i l’arrenquem
            var threads = new List<Thread>();
            foreach (var p in people)
            {
                var t = new Thread(p.Run) { Name = p.ToString() };
                threads.Add(t);
                t.Start();
            }

            // Esperem que tots acabin
            foreach (var t in threads) t.Join();

            Console.WriteLine($"\nCompte esgotat. Saldo final: {account.Balance} €");

        }

        /// <summary>
        /// Carrera de camells!
        /// Realitza un programa que emuli una carrera de camells.
        /// Cada camell és un thread diferent. Els camells han de comptar de 0 a 100. 
        /// A cada comptatge escriu per consola el número de camell i el número pel qual va,
        /// a més a més descansarà X mil·lisegons.X serà un nombre aleatori a cada cicle d’entre dos valors. 
        /// Els dos valors són paràmetres diferents entre els camells.
        /// </summary>
        static void CamelRace()
        {
             const int Goal = 100;
             const int CamelCount = 5;
            List<Camel> Camels  = new List<Camel>(); 
            List<Thread> Threads  = new List<Thread>();

            //Creem els camell i threads:
            for (int i = 0; i < CamelCount; i++)
            {
                Camel c = new Camel(Random.Shared.Next(2, 5) * 10, Random.Shared.Next(5, 10) * 10, Goal);
                Camels.Add(c);
                Console.WriteLine($"----> Main:{i} Camel{c.Id} Color{c.Color}");
                Thread t = new Thread(c.Run) { Name = $"CamelThread{c.Id}" };
                Threads.Add(t);
                t.Start();
            }

            //Start the race!
            Threads.ForEach(t =>
            {
                t.Join();
            });

            var Ranking = Camels.OrderBy(c => c.finishTime).Select((c, idx) => new
            {
                Place = idx + 1,      // 1r, 2n, 3r…
                Camel = c
            });

            Console.WriteLine("\n");

            foreach (var r in Ranking)
            {
                Console.BackgroundColor = (ConsoleColor)r.Camel.Color;
                Console.WriteLine($"{r.Place}º: Camel{r.Camel.Id}");
                Console.ResetColor();
            }
        }
        static async Task Act10()
        {
            AnimationCts = new CancellationTokenSource();
            AnimCancelToken = AnimationCts.Token;
            Task animateTask = Task.Run(() => Render(freq, AnimCancelToken));
            Task listenKeyTask = Task.Run(() => ListentKey(AnimationCts));

            Console.WriteLine("Prem qualsevol tecla per aturar l'animacio.");
            Thread.Sleep(2000);

            //Espera simultania
            await Task.WhenAny(animateTask, listenKeyTask);

            // Neteja final
            Console.CursorVisible = true;
            Console.SetCursorPosition(0, rows - 1);
            Console.WriteLine("\nAnimacio finalitzada.");
        }
        static async Task Render(int freq, CancellationToken tk)
        {
            int x = 0;
            int y = 0;

            while (!tk.IsCancellationRequested) //Renderitzem fins que s'activi la cancel·lacio
            {
                Console.SetCursorPosition(x, y);
                Console.Write(sprite);

                Thread.Sleep(freq);      //Velocitat dels fotogrames

                //Esborrem el character de la posicio anterior
                Console.SetCursorPosition(x, y);
                Console.Write(' ');

                x++;
                if (x >= columns)
                {
                    x = 0;
                    y++;
                }
            }
        }

        static async Task ListentKey(CancellationTokenSource cts)
        {
            Console.ReadKey(true);          //Es queda a l'espera d'una entrada de teclat
            cts.Cancel();                   //Quan la fa activem el token.
        }
    }
}
