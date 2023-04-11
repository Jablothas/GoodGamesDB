using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GoodGamesDB
{
    internal static class SteamData
    {
        static DataTable Games = new DataTable();
        static string PathPersonalInfos = $"https://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/" +
                                          $"?key={Index.SteamApiKey}&steamid={Index.SteamID}&format=xml";
        static string PathAppList =       $"http://api.steampowered.com/ISteamApps/GetAppList/v0002/?key=STEAMKEY&format=xml";
        static string XmlAppList =        "appList.xml"; // Local copy of .xml-File
        static string XmlPersonalList =   "myGamesList.xml"; // Local copy of personal .xml-File
        public static List<string> CompareList = new List<string>();

        public static async Task Get()
        {
            try
            {
                // Needs to be replaced by a check if older than 24h for performance optimization
                File.Delete(XmlAppList);
                File.Delete(XmlPersonalList);
                // Step 1: Download appList.xml from SteamApi if not exist or older than 48 hours
                if ((!File.Exists(XmlAppList) || !File.Exists(XmlPersonalList)))
                {
                    _ = Index.Notify("Downloading data from Steam...", 2, Index.PnlNotify, 8000);
                    await DownloadFiles();
                }
                _ = Index.Notify("Processing data in background...", 2, Index.PnlNotify);
                // Step 2: Fill DictGamesList with game title and values
                await FillLists();

                _ = Index.Notify("Processing data in background...", 2, Index.PnlNotify);
                // Step 3: Compare with own List and Add to Index.GamesList
                await CompareLists();
                _ = Index.Notify("Data successfully collected.", 1, Index.PnlNotify);
                // Step 4: Set done status do index.
                Index.BackgroundWorksDone = true;
            }
            catch (Exception ex)
            {
                _ = Index.Notify("Unable to download Data from Steam. Offline-Mode enabled.", 3, Index.PnlNotify);
                Log.Write(Index.Status, $"Counting panels per line failed.{Environment.NewLine}{ex}");
            }

        }

        public static string ShowPlaytime(string appid)
        {
            string playtime = "";
            using (var xmlReader = new StreamReader(XmlPersonalList))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                var parentNode = xmlDoc.SelectNodes("//response//games//message");
                foreach (XmlNode subNode in parentNode)
                {
                    if (subNode["appid"].InnerText == appid)
                    {
                        Input.Appid = appid;
                        playtime = subNode["playtime_forever"].InnerText;
                        break;
                    }
                }
            }
            // Math: From sec to h
            try
            {
                if (playtime != "") playtime = (Convert.ToInt32(playtime) / 60).ToString();
            }
            catch (Exception ex)
            {
                playtime = "0";
                Log.Write(1, "Math failed: " + ex);
            }

            return playtime;
        }

        private static async Task DownloadFiles()
        {
            await Task.Run(() =>
            {
                string text;
                using (var client = new WebClient())
                {
                    text = client.DownloadString(PathAppList);
                    File.WriteAllText(XmlAppList, text);
                    text = client.DownloadString(PathPersonalInfos);
                    File.WriteAllText(XmlPersonalList, text);
                }
            });
        }

        private static async Task FillLists()
        {
            await Task.Run(() =>
            {
                using (var xmlReader = new StreamReader(XmlAppList))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlReader);
                    var parentNode = xmlDoc.SelectNodes("//applist//apps//app");
                    foreach (XmlNode subNode in parentNode)
                    {
                        if (subNode["name"].InnerText == "") continue;
                        if (Index.DictGamesList.Keys.Contains(subNode["name"].InnerText)) continue;
                        Index.DictGamesList.Add(subNode["name"].InnerText, subNode["appid"].InnerText);
                    }
                }

                using (var xmlReader = new StreamReader(XmlPersonalList))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlReader);
                    var parentNode = xmlDoc.SelectNodes("//response//games//message");
                    foreach (XmlNode subNode in parentNode)
                    {
                        CompareList.Add(subNode["appid"].InnerText);
                    }
                }
            });
        }

        private static async Task CompareLists()
        {
            await Task.Run(() =>
            {
                foreach(string appid in CompareList)
                {
                    foreach (var entry in Index.DictGamesList)
                    {
                        if (appid == entry.Value)
                        {
                            Index.GamesList.Add(entry.Key);
                        }
                    }
                }
            });
        }
    }
}
