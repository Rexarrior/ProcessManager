using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;


using CMValue;
using SupportFunctions;
namespace ProccessAnalize

{
    class ProcessAnalyzer
    {
        //private======================================================================================================
        private C_M_Value _doorstepMemory;  //only value
        private C_M_Value _doorstepCPU;     //only percent
        private IEnumerable<Process> _allProcess;
        private IEnumerable<Process> _moreThanDoorstepProcess;
        private IEnumerable<Process> _lessThanDoorstepProcess;

        private bool _isUpdateByInvoke;

        //methods------------------------------------------------------------------------------------------------------
        private uint _getCPUPercent(Process x)
        {
            //PerformanceCounter theCPUCounter = new PerformanceCounter("Process", "% Processor Time", x.ProcessName);
            //return (uint)theCPUCounter.NextValue();
            TimeSpan t1;
            try
            {
                t1 = x.TotalProcessorTime;
            }
            catch (Exception e)
            {
                return 200;
            }
            Thread.Sleep(2);
            uint r = (uint)((x.TotalProcessorTime - t1).Milliseconds / 2);
            return r;

           

        }
       


        private bool _checkProcess(Process x)
        {
            bool isMoreMemory = x.PagedMemorySize64 > _doorstepMemory.Size;

            uint tmp = _getCPUPercent(x);
            bool isLessCPU = tmp < _doorstepCPU.Count;

            return isLessCPU && isMoreMemory;
        }




        private void _selectLessAndMorehanDoorstep ()
        {

            _moreThanDoorstepProcess = _allProcess.Where(x => _checkProcess(x));
            _lessThanDoorstepProcess = _allProcess.Where(x => !_checkProcess(x));
        }




        //public=======================================================================================================

        public static int InitialStringElemCount = 7;


        //properties----------------------------------------------------------------------------------------------------

        public C_M_Value DoorstepMemory
        {
            get
            {
                return _doorstepMemory;
            }

            set
            {
                _doorstepMemory = value;
            }
        }



        public C_M_Value DoorstepCPU
        {
            get
            {
                return _doorstepCPU;
            }

            set
            {
                _doorstepCPU = value;
            }
        }



        public bool IsUpdateByInvoke
        {
            get
            {
                return _isUpdateByInvoke;
            }

            set
            {
                _isUpdateByInvoke = value;
            }
        }

        

        
        //methods------------------------------------------------------------------------------------------------------
        public void Update()
        {
            _allProcess = Process.GetProcesses();
            _selectLessAndMorehanDoorstep();

        }



        public IEnumerable<Process> GetAllProcess()
        {
            if (IsUpdateByInvoke)
                Update();

            return _allProcess;
               
            
        }



        public IEnumerable<Process> GetMoreThanDoorstepProcess()
        {
            if (IsUpdateByInvoke)
                Update();

            return _moreThanDoorstepProcess;
        }



        public IEnumerable<Process> GetLessThanDoorstepProcess()
        {
            if (IsUpdateByInvoke)
                Update();

            return _lessThanDoorstepProcess;
        }



        public string GetInitialString()
        {
            return "" + _doorstepMemory.GetInitialString() + ' ' + _doorstepCPU.GetInitialString() + ' ' + Support.GetInitialStingFor(_isUpdateByInvoke) ;
        }



        public ProcessAnalyzer(C_M_Value doorstepMemory, C_M_Value doorstepCPU, bool isUpdateByInvoke = true)
        {
            this._doorstepMemory = doorstepMemory;
            this._doorstepCPU = doorstepCPU;
            this._isUpdateByInvoke = isUpdateByInvoke;
            Update();
        }



        public ProcessAnalyzer(string initialString)
        {
            string[] initArrStr = initialString.Trim(' ').Split(' ');

            this._doorstepMemory = new C_M_Value(initArrStr.Take(3).Aggregate("", (x,acum) => x + ' ' + acum));
            this._doorstepCPU = new C_M_Value(initArrStr.Skip(3).Take(3).Aggregate("", (x, acum) => x + ' ' + acum)); 
            this._isUpdateByInvoke = initArrStr[6] == "0" ? false : true;
            Update();
        }

    }
}
