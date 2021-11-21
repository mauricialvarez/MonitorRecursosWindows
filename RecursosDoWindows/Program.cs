using System;
using System.Diagnostics;
using System.Threading;

namespace RecursosDoWindows
{
    class Program
    {
        private static PerformanceCounter cpuCounter;
        private static PerformanceCounter ramCounter;
        private static PerformanceCounter dskCounter;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            InitialiseCPUCounter();
            InitializeRAMCounter();
            InitializeDSKCounter();

            while (true)
            {
                Thread.Sleep(1000);

                Console.WriteLine("CPU em USO: " + Convert.ToInt32(cpuCounter.NextValue()).ToString() + " %");

                Console.WriteLine("Memoria Disponivel MBytes: " + Convert.ToInt32(ramCounter.NextValue()).ToString() + " Mb");

                Console.WriteLine("Disco C em USO: " + Convert.ToInt32(dskCounter.NextValue()).ToString() + " %");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
            }
        }

        private static void InitializeDSKCounter()
        {
            dskCounter = new PerformanceCounter(
                "LogicalDisk",
                "% Disk Time",
                "C:",
                true
                );
        }

        private static void InitialiseCPUCounter()
        {
            cpuCounter = new PerformanceCounter(
            "Processor",
            "% Processor Time",
            "_Total",
            true
            );
        }

        private static void InitializeRAMCounter()
        {
            ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
        }
    }
}
