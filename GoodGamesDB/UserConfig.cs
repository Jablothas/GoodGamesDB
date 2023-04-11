using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GoodGamesDB
{
    internal static class UserConfig
    {
        static string Path = @"cfg.txt";
        static string[]? Settings;
        public static int Load()
        {
            try
            {
                Settings = File.ReadAllLines(Path);
                foreach (string line in Settings)
                {
                    if (line.Contains("user=")) Index.User = line.Substring("user=".Length);
                    if (line.Contains("window-size-h")) Index.WindowSizeH = Convert.ToInt32(line.Substring("window-size-h=".Length));
                    if (line.Contains("window-size-w")) Index.WindowSizeW = Convert.ToInt32(line.Substring("window-size-w=".Length));
                    if (line.Contains("steamid")) Index.SteamID = Convert.ToString(line.Substring("steamid=".Length));
                    if (line.Contains("steamapikey")) Index.SteamApiKey = Convert.ToString(line.Substring("steamapikey=".Length));
                }
                Index.Status = 0;
                Log.Write(Index.Status, "Config loaded.");
            }
            catch (Exception e)
            {
                Index.Status = -1;
                Log.Write(Index.Status, $"Couldn't fetch data from cfg.file: {e}");
            }
            return Index.Status;
        }
        public static void Save()
        {
            try
            {
                //if(File.Exists(Path)) File.Delete(Path);
                //if (!File.Exists(Path)) File.Create(Path);
                string toSave = 
                    $"user={Index.User}{Environment.NewLine}" +
                    $"window-size-h={Index.WindowSizeH}{Environment.NewLine}" +
                    $"window-size-w={Index.WindowSizeW}{Environment.NewLine}" +
                    $"steamid={Index.SteamID}{Environment.NewLine}" +
                    $"steamapikey={Index.SteamApiKey}{Environment.NewLine}";
                File.WriteAllText(Path, toSave);
                Index.Status = 0;
                Log.Write(Index.Status, "Config updated.");
            }
            catch (Exception e) 
            {
                Index.Status = -1;
                Log.Write(Index.Status, $"Couldn't update config file: {e}");
            }

        }
    }
}
