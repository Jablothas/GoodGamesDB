using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodGamesDB
{
    public static class Log
    {
        /* Error-Codes:
         * -1   = Something went wrong. Stop application to avoid data loss.
         * 0    = Everything is running fine.
         * 1    = Data access sucessfully.
         * 2    = Data created successfully by user.
         * 3    = Data edited successfully by user.
         */
        // root folder for logfiles
        static string Dir = @"logs\";
        static char Symbol = '-';
        static string Separator = new string('-', 100);//createSeparator();
        // complete path including filename based on current date
        static string FilePath = Dir + DateTime.Now.ToShortDateString() + ".txt";
        // Build string for error or info message
        static string DeployText = string.Empty;
        public static void Write(int code, string msg)
        {
            // if there is no error (>=0) msg will be created as info
            if (code >= 0)  DeployText = $"[{DateTime.Now}] Info({code}): \t\t {msg} {Environment.NewLine}{Separator}{Environment.NewLine}";
            // if there is no error (>=0) msg will be created as alert
            if (code < 0)   DeployText = $"[{DateTime.Now}] Error({code}): \t\t {msg} {Environment.NewLine}{Separator}{Environment.NewLine}";
            // check if target Directory exist, if not, create
            if(!Directory.Exists(Dir)) Directory.CreateDirectory(Dir);
            // write new line in current file
            File.AppendAllText(FilePath, DeployText);
            //await File.AppendAllTextAsync(FilePath, DeployText);
        }

        private static string createSeparator()
        {
            StringBuilder s = new StringBuilder();  
            for (int i = 0; i < 100; i++) s.Append(Symbol);
            return s.ToString();
        }
    }
}
