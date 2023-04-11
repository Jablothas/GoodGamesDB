using System.IO;
// using System.Data.SQLite;
using System.Data;
using System.Data.SQLite;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Drawing.Imaging;
using System.Windows.Forms.VisualStyles;

namespace GoodGamesDB
{
    public partial class Index : Form
    {
        /* GLOBAL VARIABLES
         * -----------------------------------------------------------------------------
         */
        public static int Status = 0;
        public static bool BackgroundWorksDone = false;
        public static bool CfgFileLoaded = false; // Switches to true if cfg file found and loaded
        public static string User; // Sets the current user name
        public static string SteamID = "";
        public static string SteamApiKey = "";
        public static int WindowSizeW; // Sets the saved window width
        public static int WindowSizeH; // Sets the saved window height
        public static DataTable Data = new DataTable(); // Global DataTable; accessible through the complete runtime
        public static DataRow[]? SearchRequest; // Store rows from search request
        List<GameObject> GameLst = new List<GameObject>(); // List of objects which need to be created
        public static bool LoginStatus = false; // Is the user logged in?
        public static bool Searching = false; // Checks if user is currently searching through Data
        public static int DataRowCount = 0; // Counts the amount of Rows in Data
        public static Panel PnlGameObj = new Panel(); // Dummy to store generated panels
        public static int PnlWidth = 450; // Width of GameObject-Panels // ! Former Width: 350
        public static int PnlHeight = 225; // Height of GameObject-Panels
        public static int PnlSpace = 20; // The Space between panels
        public static int PnlEdgeLeft = 0; // Empty space in pixel to the left edge
        public static int PnlEdgeRight = 0; // Empty space in pixel to the right edge
        public static int PnlLocationX = PnlLocationX_Std; // Starting location for horizontal
        public static int PnlLocationY = PnlLocationY_Std; // Starting location for vertical
        public static int ViewOption = 1; // 1 = GridView; 2 = TableView
        public int CurrentYear = 9999; // Std state for CurrentYear
        public int LineCount = 0; // counts the current position in line and avoid placing panels beyond the application bounds
        public int ObjCountPerYear = 0; // Counts how many objects related to current yearly loop
        public static string LatestNewEntry = "";
        public static Panel PnlNotify = new Panel();
        public static List<string> GamesList = new List<string>();
        // Dictonary for SteamData.cs
        public static Dictionary<string, string> DictGamesList = new Dictionary<string, string>();
        // Make Panel_Title dragable when click & hold
        // --------------------------------------------------------------------------------------|
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        /* GLOBAL CONSTANTS
         * -----------------------------------------------------------------------------
         */
        public const int PnlLocationX_Std = 20;
        public const int PnlLocationY_Std = 20;
        public const int WindowH_Std = 950;
        public const int WindowW_Std = 1520;

        public Index(string pwd)
        {
            InitializeComponent();
            UserConfig.Load();
            this.Size = new Size(WindowSizeW, WindowSizeH);
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            // Makes the header black and launches the standard methods
            FormStyle.DarkMode(Handle, true);
            // Hide Scrollbar
            UpdateHiddenScrollBar();
            Log.Write(Status, $"Application started.");
            Start();
            PnlDataBody.Visible = true;
            textBoxSearch.TextAlign = HorizontalAlignment.Left;
        }

        private void Start()
        {
            BuildDataTable();
            if (ViewOption == 1) GridView();
            PnlNotify = PnlBottom;

            try
            {
                SteamData.Get();
            }
            catch (Exception ex)
            {
                _ = Notify("Unable to download Data from Steam. Offline-Mode enabled.", 3, PnlBottom);
                Log.Write(Status, $"Counting panels per line failed.{Environment.NewLine}{ex}");
            }
        }

        private void GridView()
        {
            double ppl = CountPanelsPerLine(); // panels per line
            int cnt = 0; // counts the numer of objects added to List<GameObject>
            int lstItems; // counts the amount of items in lst after all foreach loops
            if (Searching == false) foreach (DataRow dr in Data.Rows) AddGameObjectToLst(dr, ppl);
            if (Searching == true) foreach (DataRow dr in SearchRequest) AddGameObjectToLst(dr, ppl);
            // count the true amount of GameObjects added to list
            lstItems = GameLst.Count - 1;
            // for each created list object build the panel
            foreach (var item in GameLst)
            {
                PnlGameObj = item.Create();
                if (cnt <= lstItems) cnt++;
                PnlDataBody.Controls.Add(PnlGameObj);
            }
            Status = 0;
            Log.Write(Status, $"GridView created.");
        }

        private void AddGameObjectToLst(DataRow dr, double ppl)
        {
            // Counts the game per year
            int YearlyGameCount = 0;

            // Find the latest year used
            if (CurrentYear == 9999)
            {
                CurrentYear = Convert.ToDateTime(dr["date"]).Year;
                YearlySpacer(CurrentYear, YearlyGameCount, PnlLocationY);
                LineCount = 0;
            }

            // If a year is already set check if year changed
            else if (CurrentYear != Convert.ToDateTime(dr["date"]).Year)
            {
                CurrentYear = Convert.ToDateTime(dr["date"]).Year;
                // There is an issue when the last object of a line is divideable trough the line count. 
                // Workaround: If the yearly count 0 via modulo we jump one vertical panel back. 
                // %4 should be replaced by lineCount in future for more consistence 
                if ((ObjCountPerYear > 0) && (ObjCountPerYear % 4 == 0)) PnlLocationY -= PnlHeight + PnlSpace;
                PnlLocationY += PnlHeight + PnlSpace;
                YearlySpacer(CurrentYear, YearlyGameCount, PnlLocationY);
                LineCount = 0;
                ObjCountPerYear = 0;
            }
            // counts the amount of entries
            LineCount++;
            // sets the id from datatable to call GameObject later
            int id = (Convert.ToInt32(dr["id"]));
            // sets the name from datatable to call GameObject later
            string name = Convert.ToString(dr["name"]) ?? string.Empty;
            // adds a new GameObject based on current data
            GameLst.Add(new GameObject(id, name, PnlLocationX, PnlLocationY));
            ObjCountPerYear++;
            // move the cursor to the next panel space
            PnlLocationX += PnlWidth + PnlSpace;
            // if the horiontal space is full, break to the next line of GameObjects
            if (LineCount >= ppl)
            {
                PnlLocationY += PnlHeight + PnlSpace;
                PnlLocationX = 20;
                LineCount = 0;
            }
        }

        // Adds a spacer for each years of entries in database
        private void YearlySpacer(int year, int gameCount, int y)
        {
            Panel Spacer = new Panel();
            Spacer.Location = new Point(PnlLocationX_Std, y);
            Spacer.Size = new Size((PnlDataBody.Width - 64), 30);
            Spacer.BackColor = Color.FromArgb(15, 15, 15);
            PnlDataBody.Controls.Add(Spacer);

            Panel SpacerBorder = new Panel();
            SpacerBorder.Size = new Size(1, 1);
            SpacerBorder.BackColor = Color.FromArgb(187, 187, 187);
            SpacerBorder.Dock = DockStyle.Bottom;
            Spacer.Controls.Add(SpacerBorder);

            Panel SpacerBorder2 = new Panel();
            SpacerBorder2.Size = new Size(1, 1);
            SpacerBorder2.BackColor = Color.FromArgb(187, 187, 187);
            SpacerBorder2.Dock = DockStyle.Left;
            Spacer.Controls.Add(SpacerBorder2);

            Label Year = new Label();
            Year.Text = year.ToString();
            Year.Location = new Point(5, 2);
            Year.Font = new Font(Year.Font.Name, 14, FontStyle.Bold);
            Year.AutoSize = true;
            Year.ForeColor = Color.White;

            Label YearCount = new Label();
            YearCount.Text = $"{GamesPerYear(year)}";
            YearCount.Location = new Point(95, 3);
            YearCount.Font = new Font(Year.Font.Name, 12, FontStyle.Bold);
            YearCount.Size = new Size(300, 30);
            YearCount.AutoSize = true;
            YearCount.ForeColor = Color.FromArgb(187, 187, 187);
            //YearCount.TextAlign = ContentAlignment.MiddleRight;
            //YearCount.Dock = DockStyle.Right;

            PictureBox GameIcon = new PictureBox();
            GameIcon.Image = Image.FromFile(@"img/game.png");
            GameIcon.Location = new Point(70, 2);
            GameIcon.Size = new Size(25, 25);
            GameIcon.SizeMode = PictureBoxSizeMode.Zoom;
            Spacer.Controls.Add(GameIcon);

            //+ " (" + GamesPerYear(year) + " games)"; 

            Spacer.Controls.Add(Year);
            Spacer.Controls.Add(YearCount);

            PnlLocationY += 50;
            PnlLocationX = 20;
        }

        // Counts the amount of panels which fit inside the current window size
        private double CountPanelsPerLine()
        {
            double ppl = 0; // panels per line
            try
            {
                double currentPnlWidth = PnlDataBody.Width; // current width of application
                ppl = (currentPnlWidth - PnlSpace) / (PnlWidth + PnlSpace);
                ppl = Math.Truncate(ppl);
                Status = 0;
                Log.Write(Status, $"{ppl} panels per line counted.");
            }
            catch (Exception e)
            {
                Status = -1;
                Log.Write(Status, $"Counting panels per line failed.{Environment.NewLine}{e}");
            }
            return ppl;
        }

        private void BuildDataTable()
        {
            try
            {
                var conn = new SQLiteConnection("Data Source=database.db;Version=3;", true);
                string query = @"SELECT id, records.name, date, location, hours, note, dlc, replay, link, pcount,  
                    rId, appid, gameplay, presentation, narrative, quality, sound, content, pacing, balance, impression, sum, hours_total, img
                    FROM records
                    INNER JOIN infos
                    ON records.link = rID
                    ORDER BY date DESC";
                conn.Open();
                SQLiteCommand cmd;
                cmd = conn.CreateCommand();
                cmd.CommandText = query;
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(Data);
                DataRowCount = Data.Rows.Count;
                if (Data.Rows.Count > 0) Status = 1;
                Log.Write(Status, $"{Data.Rows.Count} entries loaded from database.");
            }
            catch (Exception e)
            {
                Status = -1;
                Log.Write(Status, $"Couldn't load data from sql connection: {e}");
            }

        }

        public Dictionary<int, int> GamesPerYear2()
        {
            int y = 0;
            int i = 0;
            Dictionary<int, int> GamesPerYearList = new Dictionary<int, int>();
            foreach (DataRow dr in Data.Rows)
            {
                if (y == 0) y = Convert.ToDateTime(dr[0]).Year;
                if (y == Convert.ToDateTime(dr[0]).Year)
                {
                    i++;
                }
                else
                {
                    GamesPerYearList.Add(Convert.ToDateTime(dr[0]).Year, i);
                    i = 0;
                    y = Convert.ToDateTime(dr[0]).Year;
                }
            }
            return GamesPerYearList;
        }

        public int GamesPerYear(int y)
        {
            int i = 0;
            foreach (DataRow dr in Data.Rows) if (Convert.ToDateTime(dr["date"]).Year == y) i++;
            return i;
        }

        public static async Task Notify(string msg, int model, Panel p, int duration = 5000)
        {
            /* list of model:
             * 1: info  msg
             * 2: loading msg
             * 3: error msg
             */

            Panel Panel_Notify = new Panel();
            Panel Panel_Splitter = new Panel();
            Label Message = new Label();

            Color BackColor = Color.FromArgb(129, 186, 106);
            Color ForeColor = Color.Black;

            if (model == 2) BackColor = Color.FromArgb(255, 255, 171);
            if (model == 3) BackColor = Color.FromArgb(201, 71, 90);

            // Body of notification popup
            Panel_Notify.Visible = true;
            Panel_Notify.Size = new Size(800, 25);
            Panel_Notify.BackColor = BackColor;
            Panel_Notify.ForeColor = ForeColor;
            Panel_Notify.Dock = DockStyle.Bottom;
            p.Controls.Add(Panel_Notify);
            // Loading animation
            PictureBox Icon = new PictureBox();
            if (model == 3) Icon.Image = Image.FromFile(@"img/critical.png");
            if (model == 2) Icon.Image = Image.FromFile(@"img/loading.gif");
            if (model == 1) Icon.Image = Image.FromFile(@"img/done.png");
            Icon.Location = new Point(20, 0);
            Icon.Size = new Size(30, 30);
            Icon.SizeMode = PictureBoxSizeMode.Zoom;
            Icon.BackColor = Color.Transparent;
            Icon.Dock = DockStyle.Left;
            Icon.Margin = new Padding(0);
            Panel_Notify.Controls.Add(Icon);
            //Splitter between multiple notification popus
            Panel_Splitter.Size = new Size(1, 1);
            Panel_Splitter.Dock = DockStyle.Bottom;
            Panel_Splitter.BackColor = Color.Black;
            Panel_Notify.Controls.Add(Panel_Splitter);
            // Text of notification popup
            Message.Text = msg;
            //if (model == 1) Message.Text = "✔️ " + msg;
            Message.Location = new Point(30, 1);
            Message.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            Message.Size = new Size(800, 20);
            Message.ForeColor = ForeColor;
            Message.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            Panel_Notify.Controls.Add(Message);

            await Task.Delay(duration);
            Panel_Notify.Dispose();

        }

        // Login-Dummy
        private bool Login()
        {
            if ("x" == "x")
            {
                LoginStatus = true;
                _ = Notify("Login succesful", 1, PnlBottom);
            }
            return LoginStatus;
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            UserConfig.Save();
            Log.Write(Status, $"Application terminated by user.");
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (BackgroundWorksDone == true)
            {
                Input window = new Input();
                window.ShowDialog();
                if (LatestNewEntry != string.Empty) _ = Notify($"{LatestNewEntry} wurde in der Datenbank gespeichert.", 1, PnlBottom);
                LatestNewEntry = string.Empty;
                Data.Clear();
                BuildDataTable();
                CleanUpGlobals();
                GridView();
            }

        }

        // When user stops resizing window
        private void ResizeWindow(object sender, EventArgs e)
        {
            /*
            PnlDataBody.Visible = true;
            WindowSizeW = this.Size.Width;
            WindowSizeH = this.Size.Height;
            Log.Write(Status, $"Window size changed. GridView cleared.");
            CleanUpGlobals();
            GridView();
            */
        }

        // Triggered during the user is changing size
        private void Index_SizeChanged(object sender, EventArgs e)
        {
            /*PnlDataBody.Visible = false;
            WindowSizeW = this.Size.Width;
            WindowSizeH = this.Size.Height;
            CleanUpGlobals();
            GridView();*/
        }

        // Set all necessary globals to std value
        private void CleanUpGlobals()
        {
            GameLst.Clear();
            PnlDataBody.Controls.Clear();
            PnlLocationX = PnlLocationX_Std;
            PnlLocationY = PnlLocationY_Std;
            CurrentYear = 9999;
            LineCount = 0;
            UpdateHiddenScrollBar();
        }

        private void UpdateHiddenScrollBar()
        {
            PnlDataBody.Width = WindowSizeW + 20;
            PnlDataBody.AutoScroll = true;
        }
        // When the user starts typing in searchbox
        private void txtBox_Search_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearch.Text.Length > 2)
            {
                SearchRequest = Data.Select("name like '%" + textBoxSearch.Text + "%'");
                Searching = true;
                CleanUpGlobals();
                GridView();
            }
            else
            {
                if (Searching == true)
                {
                    SearchRequest = null;
                    Searching = false;
                    CleanUpGlobals();
                    GridView();
                }

            }
        }

        private void BtnMiniApp_MouseEnter(object sender, EventArgs e)
        {
            BtnMiniApp.Image = Image.FromFile(@"img/mini2.png");
        }

        private void BtnMiniApp_MouseLeave(object sender, EventArgs e)
        {
            BtnMiniApp.Image = Image.FromFile(@"img/mini.png");
        }

        private void BtnCloseApp_MouseEnter(object sender, EventArgs e)
        {
            BtnCloseApp.Image = Image.FromFile(@"img/close2.png");
        }

        private void BtnCloseApp_MouseLeave(object sender, EventArgs e)
        {
            BtnCloseApp.Image = Image.FromFile(@"img/close.png");
        }

        private void BtnCloseApp_Click(object sender, EventArgs e)
        {
            UserConfig.Save();
            Log.Write(Status, $"Application terminated by user.");
            Application.Exit();
        }

        private void BtnMiniApp_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Panel_Header_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
    }
}