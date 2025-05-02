namespace ThreadPool_Exemple
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int WorkCount = 100;

            for (int i = 0; i < WorkCount; i++)
            {
                //Cridem al Pool i posem en cua el treball a fer.
                ThreadPool.QueueUserWorkItem(ExecuteWorkInPool, i); //Requereix que el metode cridat tingui un objecte per paramentre
            }

        }

        static void ExecuteWorkInPool(object o)
        {
            int i = (int)o;
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} starts the work number: {i}");
            Thread.Sleep(1000);
        }
        static void ClassicThreading(int workCount)
        {
            for (int i = 0; i < workCount; i++)
            {
                Thread t = new Thread(ExecuteWork);
                t.Start();
            }
        }

        static void ExecuteWork()
        {
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} starts work");
            Thread.Sleep(1000);
        }
    }
}
