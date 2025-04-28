using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Threading;

namespace M09T2_Process
{
    internal class Program
    {
        public static bool IsPing = true;
        public static int Rounds = 10;

        public static object locker = new object();
        static void Main(string[] args)
        {
            /****** Processos *****/
            LaunchAProcess();          
            GetInfoOfAProcess();            //Capturar un proces i extreure dades, fils, modules
            ProcessorGame();                //Extra: Mirar els procesadors per a un proces
            TimeConsume();                  //Com mesurar el temps consumit per un bloc de codi


            /******* Threads ******/

            MyFirstsThreads();

                /*** Compte enrere i endavant ***/
            Thread thCountingR = new Thread(ReverseCount);
             Thread thCounting = new Thread(Count);

             thCounting.Start();
             thCountingR.Start();

             thCounting.Join();
             thCountingR.Join();

                /*** Lock i Monitor: Ping Pong ***/

            Thread thPing = new Thread(Ping);
            Thread thPong = new Thread(Pong);

            thPing.Start();
            thPong.Start();

            thPing.Join();
            thPong.Join();

        }

        static void MyFirstsThreads()
        {
            Thread t1 = new Thread(() => Console.WriteLine("Hola soc el fil 1"));
            Thread t2 = new Thread(() => Console.WriteLine("Hola soc el fil 2"));

            Console.WriteLine("Comença l'execucio de Threads:");
            t1.Start();
            t2.Start();

            t1.Join();          //El programa principal espera fins que t1 ha acabat
            t2.Join();          //El programa principal espera fins que t2 ha acabat

            Console.WriteLine("Acabem l'execucio del metode");
        }

        static void ReverseCount()
        {
            const int startCount = 10;
            for (int i = startCount; i > 0; i--)
            {
                Console.WriteLine("RCount: " + i);
                Thread.Sleep(3000);
            }
        }

        static void Count()
        {
            const int count = 10;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("Count: " + i);
                Thread.Sleep(1000);
            }
        }

        static void Ping()
        {
            for (int i = 0; i < Rounds; i++)
            {
                lock (locker)
                {
                    while (!IsPing)
                        Monitor.Wait(locker);
                    Console.WriteLine("Ping");
                    IsPing = false;
                    Monitor.Pulse(locker);
                }
            }
        }
        static void Pong()
        {
            for (int i = 0; i < Rounds; i++)
            {
                lock (locker)
                {
                    while (IsPing)
                        Monitor.Wait(locker);
                    Console.WriteLine("\t Pong");
                    IsPing = true;
                    Monitor.Pulse(locker);
                }
            }
        }

        static void GetInfoOfAProcess()
        {
            var processos = Process.GetProcesses();

            foreach (Process os in processos)
            {
                Console.WriteLine($"PID: {os.Id} \t Name: {os.ProcessName} ");
            }

            int pid = int.Parse(Console.ReadLine());

            Process chromeP = null;
            try
            {
                chromeP = Process.GetProcessById(pid);
                ProcessThreadCollection pTC = chromeP.Threads;
                Console.WriteLine("Threads del programa {0}, ThreadCount: {1}", chromeP.ProcessName, pTC.Count);
                foreach (ProcessThread pt in pTC)
                {
                    Console.WriteLine($"{pt.Id} \t Container: {pt.Container} ");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ProcessModuleCollection pMC = chromeP.Modules;
            Console.WriteLine("Moduls del programa {0}, ModulCount: {1}", chromeP.ProcessName, pMC.Count);
            foreach (ProcessModule pM in pMC)
            {
                Console.WriteLine($"{pM.ModuleName} \t Container: {pM.Container} ");
            }

        }

        static void LaunchAProcess()
        {
            //Executa el programa dotnet amb l’argument –info. 
            var process = new Process
             {
                 //Configurem el process amb la classe ProcessStartInfo
                 StartInfo = new ProcessStartInfo
                 {
                     FileName = "dotnet",  //Arxiu a obrir
                     Arguments = "--info",   //Arguments
                     RedirectStandardOutput = true, //
                     UseShellExecute = false,
                     CreateNoWindow = true
                 }
             };

             process.Start();

             //Capturem el que s’ha imprés per pantalla:
             string output = process.StandardOutput.ReadToEnd();
             process.WaitForExit();
             Console.WriteLine(output);
         }

        //Funcio per mesurar el rendiment:
        static void TimeConsume()
        {
            var sw = Stopwatch.StartNew();
            MesurementFunc();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        static void MesurementFunc()
        {
            Thread.Sleep(2200);
        }

        static void ProcessorGame()
        {

            var currentProc = Process.GetCurrentProcess();
            Console.WriteLine($"Processador: {currentProc.ProcessorAffinity:X}; Processadors totals: {Environment.ProcessorCount}");
            /*  ProcessorAffinity be amb una mascara de bits.
             *  Exemples de màscares (bits):
                *  0x1  -> només CPU 0
                *  0x2  -> només CPU 1
                *  0x3  -> CPU 0 i 1
                *  0x5  -> CPU 0 i 2  (0b00101)
                */
            currentProc.ProcessorAffinity = (IntPtr)0x1;        //Fixem a un sol processador
            Console.WriteLine($"Processador: {currentProc.ProcessorAffinity:X}; Processadors totals: {Environment.ProcessorCount}");

        }

    }


}

