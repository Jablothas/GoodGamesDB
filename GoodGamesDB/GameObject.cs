using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Security.Cryptography;
using Microsoft.VisualBasic.Logging;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Media;

namespace GoodGamesDB
{
    public class GameObject
    {
        // Return object
        private Panel p = new Panel();
        private Panel borderTop = new Panel();
        // Location
        private int x = 0; // hor-pos from panel
        private int y = 0; // vert-pos from oanel
        // DataTable: records
        private string name = string.Empty;
        private string location = string.Empty;
        private string note = string.Empty;
        private string dlc = string.Empty;
        private string date = DateTime.Now.ToShortDateString();
        private int id = 0;
        private int hours = 0;
        private int replay = 0;
        private int link = 0;
        // DataTable: infos
        private string imgPath = string.Empty;
        private int rId = 0;
        private int appid = 0;
        private int gameplay = 0;
        private int presentation = 0;
        private int narrative = 0;
        private int quality = 0;
        private int sound = 0;
        private int content = 0;
        private int pacing = 0;
        private int balance = 0;
        private int impression = 0;
        private int sum = 0;
        private int pcount = 0;
        const int infoAreaStartX = 160;

        public GameObject(int id, string name, int x, int y)
        {
            this.id = id;
            this.name = name;
            this.x = x;
            this.y = y;
        }

        public Panel Create()
        {
            LoadDataFromTable(id);

            if (Index.Status >= 0)
            {
                DrawPanel();
            }
            else
            {
                Index.Status = -1;
            }
            return p;
        }

        private void DrawPanel()
        {
            string playthrough = string.Empty;
            switch (pcount)
            {
                case 1:
                    playthrough = "1st";
                    break;
                case 2:
                    playthrough = "2nd";
                    break;
                case 3:
                    playthrough = "3rd";
                    break;
                default:
                    playthrough = pcount + "th";
                    break;
            }
            // Set panel attributes
            p.Name = $"GameObj_{id}";
            p.Size = new Size(Index.PnlWidth, Index.PnlHeight);
            p.Location = new Point(x, y);
            p.BackColor = Color.FromArgb(15, 15, 15);
            p.MouseEnter += new EventHandler(MouseEnterOnPanel);
            p.MouseLeave += new EventHandler(MouseLeaveOnPanel);
            p.Click += new EventHandler(MouseClick);

            // panel borders
            borderTop.Dock = DockStyle.Right;
            borderTop.Size = new Size(1, 1);
            borderTop.BringToFront();
            borderTop.BackColor = Color.FromArgb(32, 201, 151);
            borderTop.Visible = false;
            p.Controls.Add(borderTop);
            
            


            // Cover of game
            PictureBox c = new PictureBox();
            c.Size = new Size(150, Index.PnlHeight);
            c.Location = new Point(0, 0);
            c.Dock = DockStyle.Left;
            c.SizeMode = PictureBoxSizeMode.Zoom;
            c.Image = Image.FromFile($"img/grid/{imgPath}");

            // Title of game
            Label t = new Label();
            t.Location = new Point(infoAreaStartX, 10);
            t.AutoSize = true;
            t.Text = name;
            //title.Font = new Font(title.Font.Name, 11, FontStyle.Regular);

            // Score of game
            if(replay == 0) ScoreDisplay.Create(infoAreaStartX, 35, sum, false, this.p);
            if (replay == 1) ScoreDisplay.Create(infoAreaStartX, 35, sum, true, this.p);



            // Note of game
            Label n = new Label();
            n.Location = new Point(infoAreaStartX, 80);
            n.AutoSize = true;
            n.Text = $"{note}";
            n.ForeColor = Color.DarkGray;

            // Date of game finish
            Label d = new Label();
            d.Location = new Point(infoAreaStartX, 190);
            d.AutoSize = true;
            d.Text = $"Record Nr. {pcount} on {date}";
            d.ForeColor = Color.DarkGray;

            // Logo of location finished on
            PictureBox l = new PictureBox();
            l.Size = new Size(50, 50);
            l.Location = new Point(390, 165);
            l.SizeMode = PictureBoxSizeMode.Zoom;
            l.Image = Image.FromFile($"{SetLocationIcon()}");
            l.BackColor = Color.Transparent;

            p.Controls.Add(c);
            p.Controls.Add(t);
            p.Controls.Add(d);
            p.Controls.Add(n);  
            p.Controls.Add(l);

        }

        private string SetLocationIcon()
        {
            StringBuilder s = new StringBuilder();
            s.Append(@"img\locations\");
            switch (location.ToUpper())
            {
                case "STEAM":
                    s.Append("1");
                    break;
                case "UPLAY":
                    s.Append("2");
                    break;
                case "BATTLE.NET":
                    s.Append("3");
                    break;
                case "EPIC GAMES":
                    s.Append("4");
                    break;
                case "NINTENDO SWITCH":
                    s.Append("5");
                    break;
                default:
                    s.Append("0");
                    break;
            }
            s.Append(".png");
            return s.ToString();
        }

        private void P_MouseLeave(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoadDataFromTable(int id)
        {
            //DataRow dt = Index.Data.Rows[id];
            //var dt = Index.Data.AsEnumerable().Where(dt => dt.Field<int>("ID") == 1);
            DataRow[] dt = Index.Data.Select(string.Format("ID ='{0}' ", id));
            try
            {
                
                // set records
                this.id = Convert.ToInt32(dt[0]["id"]);
                name = Convert.ToString(dt[0]["name"]) ?? String.Empty;
                location = Convert.ToString(dt[0]["location"]) ?? String.Empty;
                note = Convert.ToString(dt[0]["note"]) ?? String.Empty;
                dlc = Convert.ToString(dt[0]["dlc"]) ?? String.Empty;
                date = Convert.ToDateTime(dt[0]["date"]).ToShortDateString() ?? String.Empty;
                hours = Convert.ToInt32(dt[0]["hours"]);
                replay = Convert.ToInt32(dt[0]["replay"]);
                imgPath = Convert.ToString(dt[0]["img"]) ?? String.Empty;
                // set infos
                rId = Convert.ToInt32(dt[0]["rId"]);
                appid = Convert.ToInt32(dt[0]["appid"]);
                gameplay = Convert.ToInt32(dt[0]["gameplay"]);
                presentation = Convert.ToInt32(dt[0]["presentation"]);
                narrative = Convert.ToInt32(dt[0]["narrative"]);
                quality = Convert.ToInt32(dt[0]["quality"]);
                sound = Convert.ToInt32(dt[0]["sound"]);
                content = Convert.ToInt32(dt[0]["content"]);
                pacing = Convert.ToInt32(dt[0]["pacing"]);
                balance = Convert.ToInt32(dt[0]["balance"]);
                impression = Convert.ToInt32(dt[0]["impression"]);
                sum = Convert.ToInt32(dt[0]["sum"]);
                pcount = Convert.ToInt32(dt[0]["pcount"]);
                Index.Status = 1;
            }
            catch (Exception ex)
            {
                Index.Status = -1;
                Log.Write(Index.Status, $"Failed to read row from DataTable: {ex.Message}");
            }
        }

        private void MouseEnterOnPanel(object sender, EventArgs e)
        {
            //p.BackColor = Color.FromArgb(25, 25, 25);
            borderTop.Visible = true;
        }

        private void MouseLeaveOnPanel(object sender, EventArgs e)
        {
            //p.BackColor = Color.FromArgb(15, 15, 15);
            borderTop.Visible = false;
        }

        private void MouseClick(object sender, EventArgs e)
        {
            InfoTab.CurrentItemID = id;
            InfoTab.CurrentItemName = name;
            InfoTab.CurrentItemRId = rId;
            InfoTab.CurrentItemReplayStatus = replay;
            InfoTab tab = new InfoTab();
            Index.PlaySound("OPEN_PANEL");
            tab.ShowDialog();
        }
    }
}
