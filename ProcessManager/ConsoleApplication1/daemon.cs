using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using ProcessManager; 

namespace KillingCostlyDaemon
{
    class Daemon
    {
        static int DEFAULTINTERVAL = 600000; //one minute
        

        void run(string initialString)
        {
            string[] arr = new string[1];
            arr[0] = initialString;
            System.IO.File.WriteAllLines("log.txt",arr );


            Manager manager = new Manager(initialString);
            manager.IsAutomaticControl = false;
            manager.IsAutomaticUpdate = false;

            while (true)
            {
                manager.Update();
                manager.CloseAllCostly();
                Thread.Sleep((int)manager.PauseInterval);
            }

        }
        static void Main(string[] args)
        {
           
            if (args.Length == 0)
            {
                throw new Exception("Не предназначено для запуска вне ProcessManager");
                
            }

            Daemon daemon = new Daemon();
            daemon.run(args.Aggregate("", (acum, x) => acum + ' ' + x));


        }
    }
}
