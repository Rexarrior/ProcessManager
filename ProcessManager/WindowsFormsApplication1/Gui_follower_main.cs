using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Timers;


using ProcessManager;
using CMValue;



namespace WindowsFormsApplication1
{
    
    public partial class Gui_follower_main : Form
    {
        Manager manager;

        System.Timers.Timer timer;

        bool isDaemonRunning;

        string daemonName ;

        public void ShowUpdated(object sender, ElapsedEventArgs e)
        {
            textBox1.Lines = manager.GetCostlyProceses().Select(
                x => x.ProcessName + ": " + (x.PrivateMemorySize64 / (1024 * 1024)) + " Mb").
                ToArray();
        }


        private void StopAutomatic()
        {
            this.checkBox1.Checked = false;
            manager.IsAutomaticControl = false;
            manager.StopAutomatic();
        }

        public Gui_follower_main()
        {
            InitializeComponent();

            int timerInterval = 1000;


            manager = new Manager(
                new C_M_Value(TypeValue.CPU, 1, LetterValue.Percent),
                new C_M_Value(TypeValue.Memory, 500, LetterValue.Mb), true);
            manager.PauseInterval =(ulong)timerInterval;
            manager.RunAutomatic();
            isDaemonRunning = false;



            /* timer = new System.Timers.Timer(timerInterval);
            timer.AutoReset = true;
            timer.Elapsed += ShowUpdated;
            timer.Start();*/
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void Gui_main_Resize(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if(!manager.IsAutomaticControl)
                {
                this.checkBox1.Checked = true;
                manager.IsAutomaticControl = true;
                manager.RunAutomatic();
                WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;

                
            }
            else
            {
                StopAutomatic();     
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int X = 0;
            if (int.TryParse(textBox2.Text, out X))
                manager.CPUlevel = new C_M_Value(TypeValue.CPU, (uint)X, LetterValue.Percent);

            if (int.TryParse(textBox3.Text, out X))
                manager.MemoryLevel = new C_M_Value(TypeValue.Memory, (uint)X, LetterValue.Mb);

            manager.Update();
            textBox1.Lines = manager.GetCostlyProceses().Select(x => x.ProcessName + ": " + (x.PrivateMemorySize64 / (1024 * 1024) ) + " Mb" ).ToArray();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            manager.CloseAllCostly();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (manager.IsAutomaticControl)
            {
                this.checkBox1.Checked = false;
                manager.IsAutomaticControl = false;
                manager.StopAutomatic();
            }
            if (!isDaemonRunning)
            {
               daemonName =  manager.RunDaemon().ProcessName;
                isDaemonRunning = true;
                checkBox2.Checked = true;
            }
            else if (Process.GetProcessesByName(daemonName).Length != 0)
            {
                manager.StopDaemon();
                isDaemonRunning = false;
                checkBox2.Checked = false;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
