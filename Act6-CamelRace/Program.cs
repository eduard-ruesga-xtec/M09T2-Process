namespace Act6.CamelRace
{
    internal class Program
    {
        public const int Goal = 100;
        public const int CamelCount = 5;
        public static List<Camel> Camels { get; set; } = null;
        public static List<Thread> Threads { get; set; } = null;

        static void Main(string[] args)
        {
            Camels  = new List<Camel>();
            Threads = new List<Thread>();

            //Creem els camell i threads:
            for (int i = 0; i < CamelCount; i++)
            {
                Camel c = new Camel(Random.Shared.Next(2, 5) * 10, Random.Shared.Next(5, 10) * 10, Goal);
                Camels.Add(c);
                Console.WriteLine($"----> Main:{i} Camel{c.Id} Color{c.Color}");
                Thread t = new Thread(c.Run) { Name = $"CamelThread{c.Id}"};
                Threads.Add(t);
                t.Start();
            }

            //Start the race!
            Threads.ForEach(t => {
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
    }
}
