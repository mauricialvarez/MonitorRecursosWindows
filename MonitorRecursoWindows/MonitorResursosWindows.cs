using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace MonitorRecursoWindows
{
    public partial class MonitorResursosWindows : ServiceBase
    {
        private bool _Loop = false;
        private EventLog eventLog;
        Timer timer;
        private static PerformanceCounter cpuCounter;
        private static PerformanceCounter ramCounter;
        private static PerformanceCounter dskCounter;


        public MonitorResursosWindows()
        {
            InitializeComponent();

            InitialiseCPUCounter();
            InitializeRAMCounter();
            InitializeDSKCounter();

            eventLog = new System.Diagnostics.EventLog();

            //System.Diagnostics.EventLog.DeleteEventSource("SegurProMonitorRecursosWindows");


            if (!System.Diagnostics.EventLog.SourceExists("SegurProMonitorRecursosWindows"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "SegurProMonitorRecursosWindows", "SegurPro_LOG_MonitorRecursosWindows");
            }
            eventLog.Source = "SegurProMonitorRecursosWindows";
            eventLog.Log = "SegurPro_LOG_MonitorRecursosWindows";

            timer = new Timer();
            timer.Interval = 1000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.MonitoraRecursosWindows);
            

        }

        protected override void OnStart(string[] args)
        {
            _Loop = true;

            timer.Start();

            eventLog.WriteEntry("OnStart");
        }        

        protected override void OnStop()
        {
            _Loop = false;

            timer.Stop();

            eventLog.WriteEntry("OnStop");
        }

        protected override void OnContinue()
        {
            eventLog.WriteEntry("OnContinue.");
        }

        private void MonitoraRecursosWindows(object sender, ElapsedEventArgs args)
        {
            //eventLog.WriteEntry("CPU em USO: " + Convert.ToInt32(cpuCounter.NextValue()).ToString() + " %");
            //eventLog.WriteEntry("Memoria Disponivel MBytes: " + Convert.ToInt32(ramCounter.NextValue()).ToString() + " Mb");
            //eventLog.WriteEntry("Disco C em USO: " + Convert.ToInt32(dskCounter.NextValue()).ToString() + " %");

            eventLog.WriteEntry("Disco C em USO: " + Convert.ToInt32(dskCounter.NextValue()).ToString() + " %" +
                "\r\n" +
                "Memoria Disponivel MBytes: " + Convert.ToInt32(ramCounter.NextValue()).ToString() + " Mb" +
                "\r\n" +
                "Disco C em USO: " + Convert.ToInt32(dskCounter.NextValue()).ToString() + " % ");

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
