using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Act6.CamelRace
{
    public class Camel
    {
        public static int CamelCount { get; set; } = 0;
        private static int[] _sleepLimitsDefault { get; set; } = { 30, 50 };

        public readonly object locker = new();                       //No es obligatori, en servirà per protegir els colors de la consola

        public  int Goal;
        public int Id { get; set; }                 //Número de Dorsal
        public int Position { get; set; }
        public int Color { get; set; }

        public DateTime finishTime { get; private set; }

        private int[] _sleepLimits { get; set; }    //[min, Max]

        public Camel() 
        {
            Id       = CamelCount;
            Position = 0;
            Color    = Id;
            Goal     = 100;
            _sleepLimits = new int[] { _sleepLimitsDefault[0], _sleepLimitsDefault[1] };
            Camel.CamelCount = CamelCount + 1;
        }
        public Camel(int speedMin, int speedMax, int goal)
        {
            Id = CamelCount;
            Position = 0;
            Color = Id;
            Goal = goal;
            this._sleepLimits = new int[] { speedMin, speedMax };
            Camel.CamelCount = CamelCount + 1;
        }

        public override string ToString()
        {
            return $"Camell{this.Id} Position: {this.Position}m";
        }

        public int GetSleepTime()
        {
            return Random.Shared.Next(_sleepLimits[0], _sleepLimits[1]);    
        }

        public void Run()
        {
            int sleepTime;
            for (int i =0; i < Goal; i++)
            {
                Position += 1;
                sleepTime = GetSleepTime();
                lock (locker)                           //Console és un recurs comú que bloquejem
                {
                    Console.BackgroundColor = (ConsoleColor)Color;
                    Console.WriteLine(ToString() + $" SleepTime: {sleepTime}");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Thread.Sleep(sleepTime);
            }


            lock (locker)
            {
                finishTime = DateTime.Now;
                Console.BackgroundColor = (ConsoleColor)Color;
                Console.WriteLine($"Camel{Id} Finish!");
                Console.ResetColor();
            }
        }

    }
}
