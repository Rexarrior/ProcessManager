using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using System.Diagnostics;

using ProccessAnalize;
using CMValue;
using SupportFunctions;

namespace ProcessManager
{
    public delegate void ShowUpdatedDelegate();
    public class Manager
    {
        //private==========================================================================================================

        private Timer _timer;
        private Process _daemon;
        private UInt64 _pause;  


        private IEnumerable<Process> _costlyProceses;

        private C_M_Value _CPUlevel;
        private C_M_Value _memoryLevel;
        private ProcessAnalyzer _analyzer;

        private bool _isAutomaticUpdate;
        private bool _isAutomaticControl;
        private bool _isKilling;
        private bool _isEconomy;

       


        
        //methods----------------------------------------------------------------------------------------------------------

        private string _toInitialString()
        {
            string res = "";



            return res;
        }

        private void _onTime(Object source, System.Timers.ElapsedEventArgs e)
        {

            if (_isAutomaticUpdate)
                Update();
            if (_isAutomaticControl)
                if (CloseAllCostly())
                    _costlyProceses = null;

        }


        private void _commonPartInit()
        {
            _timer = new Timer(PauseInterval);
            _timer.Elapsed += _onTime;
            _timer.AutoReset = true;
            _costlyProceses = _analyzer.GetMoreThanDoorstepProcess();
            _daemon = new Process();


        }
        
        
        //public===========================================================================================================

        public static int InitialStringElemCount = 18;

        //properties-------------------------------------------------------------------------------------------------------

        public ulong PauseInterval
        {
            get
            {
                return _pause;
            }

            set
            {
                _pause = value;

                _timer.Interval = _pause;
            }
        }

        public bool IsAutomaticUpdate
        {
            get
            {
                return _isAutomaticUpdate;
            }

            set
            {
                _isAutomaticUpdate = value;
         
            }
        }

        public bool IsAutomaticControl
        {
            get
            {
                return _isAutomaticControl;
            }

            set
            {
                _isAutomaticControl = value;
            }
        }

        public bool IsKilling
        {
            get
            {
                return _isKilling;
            }

            set
            {
                _isKilling = value;
            }
        }

        public bool IsEconomy
        {
            get
            {
                return _isEconomy;
            }

            set
            {
                _isEconomy = value;
            }
        }

        public C_M_Value CPUlevel
        {
            get
            {
                return _CPUlevel;
            }

            set
            {
                _CPUlevel = value;
                _analyzer.DoorstepCPU = value;
            }
        }

        public C_M_Value MemoryLevel
        {
            get
            {
                return _memoryLevel;
            }

            set
            {
                _memoryLevel = value;
                _analyzer.DoorstepMemory = value;
            }
        }

        //events



        public event ShowUpdatedDelegate Updated; 
        //methods----------------------------------------------------------------------------------------------------------


        
        public void Update()
        {
            _analyzer.Update();
            this._costlyProceses = _analyzer.GetMoreThanDoorstepProcess();
           if (Updated != null)
                Updated();
        }


        public IEnumerable<Process> GetCostlyProceses()
        {
            return _costlyProceses;
        }


        public bool CloseProcess(Process proc)
        {
            if (_isKilling || proc.MainWindowHandle == null)
            {
                try
                {
                    proc.Kill();
                }
                catch
                {
                    return false;

                }
            }
            else
            {
                try
                {
                    proc.CloseMainWindow();
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public bool CloseProcess(string processName)
        {
            return Process.GetProcessesByName(processName).Select(x => CloseProcess(x)).Aggregate(true, (acum, x) => x && acum);

        }

        public bool CloseAllCostly()
        {
           return _costlyProceses.Select(x => CloseProcess(x)).Aggregate(true, (acum, x) => x && acum);
        }


        public void RunAutomatic()
        {
            _timer.Start();
        }
        public void StopAutomatic()
        {
            _timer.Stop();
        }



        public void RunDaemon()
        {
            

            _daemon.StartInfo.CreateNoWindow =true;
            _daemon.StartInfo.UseShellExecute = false;
            _daemon.StartInfo.Arguments = this.GetInitialString();
            _daemon.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\daemon.exe";
            _daemon.Start();
           
        }


        public void StopDaemon()
        {
           
            _daemon.Kill();
            
        }



        public string GetInitialString()
        {
            return "" + _pause + ' ' + _CPUlevel.GetInitialString() + ' ' +
                _memoryLevel.GetInitialString() + ' ' +
                _analyzer.GetInitialString() + ' ' +
                Support.GetInitialStingFor(_isAutomaticUpdate) + ' ' +
                Support.GetInitialStingFor(_isAutomaticControl) + ' ' +
                Support.GetInitialStingFor(_isKilling) + ' ' +
                Support.GetInitialStingFor(_isEconomy);  
        }

        //constructors-----------------------------------------------------------------------------------------------------

        

        public Manager(C_M_Value cpuLevel, C_M_Value memoryLevel, bool isKilling, bool isEconomy=true , bool isAutomaticUpdate=true, bool isAutomaticControl=false, UInt64 pause=3600000)
        {


            _CPUlevel = cpuLevel;
            _memoryLevel = memoryLevel;
            _isKilling = isKilling;
            _isEconomy = isEconomy;
            _isAutomaticUpdate = isAutomaticUpdate;
            _isAutomaticControl = isAutomaticControl;
            _pause = pause;


            _analyzer = new ProcessAnalyzer(_memoryLevel, _CPUlevel, !isEconomy);

            _commonPartInit();


        }
        public Manager(string initialString)
        {

            string[] initArrStr = initialString.Trim(' ').Split(' ');

            int skipCount = 0;

            _pause = UInt64.Parse(initArrStr[0]); skipCount += 1;


            _CPUlevel = new C_M_Value(initArrStr.Skip(skipCount).Take(C_M_Value.InitialStringElemCount).
                Aggregate("", (x, acum) => x + ' ' + acum));
            skipCount += C_M_Value.InitialStringElemCount;


            _memoryLevel = new C_M_Value(initArrStr.Skip(skipCount).Take(C_M_Value.InitialStringElemCount).
                Aggregate("", (x, acum) => x + ' ' + acum));
            skipCount += C_M_Value.InitialStringElemCount;


            _analyzer = new ProcessAnalyzer(initArrStr.Skip(skipCount).Take(ProcessAnalyzer.InitialStringElemCount).
                Aggregate("", (x, acum) => x + ' ' + acum));
            skipCount += ProcessAnalyzer.InitialStringElemCount;

           // System.IO.File.WriteAllText("log1.txt", System.IO.File.ReadAllText("log1.txt") +
            //    "Initial str: " + initialString + "  skipCount=" + skipCount + " length inarr= " + initArrStr.Length);

            _isAutomaticUpdate = initArrStr[skipCount++] == "0" ? false : true; 
            _isAutomaticControl = initArrStr[skipCount++] == "0" ? false : true; 
            _isKilling = initArrStr[skipCount++] == "0" ? false : true; ;
            _isEconomy = initArrStr[skipCount++] == "0" ? false : true; 


            _commonPartInit();

          

        }
    }
}
