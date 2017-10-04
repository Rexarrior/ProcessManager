

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Journalization
{
    
    class UnsuccesfullyWrittenException:Exception
    {
        public UnsuccesfullyWrittenException(string message):
            base( message)
        {
           
        }
    }




    class CanNotInitializeJournalFileException : Exception
    {
        public CanNotInitializeJournalFileException(string message) :
            base(message)
        {

        }
    }




    class Logger
    {

        //private sector-----------------------------------------------------------------------------------------------


       private const string LOGFILENAME = "mainLog.txt";

       private System.IO.StreamWriter _logf;
       
       private string _initStr;
      

        private void _write(string message, string place="")
        {
            try
            {
                this._logf.Write("");
            }
            catch
            {
                throw new UnsuccesfullyWrittenException(String.Format("Ошибка записи в журнал. Сообщение: {0}. Источник: {1}",
                      message, place == "" ? "undefined" : place));
            }

        }

        private void _backupLog()
        {
            try
            {
                string newFileName = "oldLog\\" + File.ReadLines(LOGFILENAME).First();
                System.IO.File.Move(LOGFILENAME, newFileName);
              
            }
            catch (FileNotFoundException)
            {
                _initStr = "Бэкап журнала не удался: отсутствует " +LOGFILENAME;
            }
        }

        //public sector------------------------------------------------------------------------------------------------


        public void Write(string message, string place = "")
        {
            this._write(message, place);
        }






        //constructors-------------------------------------------------------------------------------------------------
        public Logger()
        {
            this._backupLog();
           
            try
            {
                _logf = File.CreateText(LOGFILENAME);
                if (_initStr != "")
                    this._write(_initStr, "constructor");

            }
               
            
            catch
            {
                _logf.Close();
                throw new CanNotInitializeJournalFileException("Адрес:" + System.IO.Directory.GetCurrentDirectory() + LOGFILENAME);
            }
            
            
        }
        ~Logger()
        {
            _logf.Close();
        }

    }
}
