using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoodGamesDB
{
    public partial class InfoTab : Form
    {
        /* CURRENT SELECTED ITEM
         * -----------------------------------------------------------------------------
         */
        public static int CurrentItemID = 0;
        public static int CurrentItemRId = 0;
        public static string CurrentItemName = string.Empty;
        public static int CurrentItemReplayStatus = 0;
        public static DataRow[]? Item;
        public static bool EditMode = false;
        public static string RecordDate;
        public static string Day;
        public static string Month;
        public static string Year;
        public static string Cover_Path = string.Empty;
        VisualScore VisualScoreGameplay = new VisualScore();
        VisualScore VisualScorePresentation = new VisualScore();
        VisualScore VisualScoreNarrative = new VisualScore();
        VisualScore VisualScoreQuality = new VisualScore();
        VisualScore VisualScoreSound = new VisualScore();
        VisualScore VisualScoreContent = new VisualScore();
        VisualScore VisualScorePacing = new VisualScore();
        VisualScore VisualScoreBalance = new VisualScore();
        VisualScore VisualScoreImpression = new VisualScore();
        VisualScore ScoreSum = new VisualScore();
        decimal SumScore = 0;

        public InfoTab()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Item = Index.Data.Select($"ID = {CurrentItemID}");
            CurrentItemRId = Convert.ToInt32(Item[0]["rId"]);
            string LogoID = string.Empty;
            // Load Cover
            Cover_Path = $"{Item[0]["img"]}";
            // Replay-Status
            if (Item[0]["replay"].ToString() == "1")
            {
                CurrentItemReplayStatus = 1;
            }
            // Cover
            PicBoxCover.Image = Image.FromFile($"img/grid/{Item[0]["img"]}");
            PicBoxCover.SizeMode = PictureBoxSizeMode.Zoom;
            // Logo from location
            switch (Item[0]["location"].ToString().ToUpper())
            {
                case "STEAM":
                    LogoID = "1";
                    break;
                case "UPLAY":
                    LogoID = "2";
                    break;
                case "XBOX GAME PASS":
                    LogoID = "3";
                    break;
                case "EPIC GAMES":
                    LogoID = "4";
                    break;
                case "NINTENDO SWITCH":
                    LogoID = "5";
                    break;
                default:
                    LogoID = "0";
                    break;
            }
            // Record date
            RecordDate = Convert.ToDateTime(Item[0]["date"]).ToShortDateString();
            Lbl_Date.Text = $"Record from {RecordDate}";
            PicLocation.Image = Image.FromFile($"img/locations/{LogoID}.png");
            // Split date
            Day = RecordDate.Substring(0, 2);
            Month = RecordDate.Substring(3, 2);
            Year = RecordDate.Substring(6, 4);
            // Name
            Lbl_Name.Text = Item[0]["name"].ToString();
            // Total score
            if (CurrentItemReplayStatus == 0) ScoreDisplay.Create(378, 45, Convert.ToInt32(Item[0]["sum"]), false, PnlContent);
            if (CurrentItemReplayStatus == 1) ScoreDisplay.Create(378, 45, Convert.ToInt32(Item[0]["sum"]), true, PnlContent);
            // Load detailed score dummies
            VisualScoreGameplay.Create(315, 90, "Gameplay", PnlContent); // y+30
            VisualScorePresentation.Create(315, 120, "Presentation", PnlContent);
            VisualScoreNarrative.Create(315, 150, "Narrative", PnlContent);
            VisualScoreQuality.Create(315, 180, "Quality", PnlContent);
            VisualScoreSound.Create(315, 210, "Sound", PnlContent);
            VisualScoreContent.Create(315, 240, "Content", PnlContent);
            VisualScorePacing.Create(315, 270, "Pacing", PnlContent);
            VisualScoreBalance.Create(315, 300, "Balance", PnlContent);
            VisualScoreImpression.Create(315, 330, "Impression", PnlContent);
            // Load detailed scores by data
            // Gameplay
            VisualScoreGameplay.Update(Convert.ToInt32(Item[0]["gameplay"]));
            Lbl_Gameplay.Text = Item[0]["gameplay"].ToString();
            // Presentation
            VisualScorePresentation.Update(Convert.ToInt32(Item[0]["presentation"]));
            Lbl_Presentation.Text = Item[0]["presentation"].ToString();
            // Narrative
            VisualScoreNarrative.Update(Convert.ToInt32(Item[0]["narrative"]));
            Lbl_Narrative.Text = Item[0]["narrative"].ToString();
            // Quality
            VisualScoreQuality.Update(Convert.ToInt32(Item[0]["quality"]));
            Lbl_Quality.Text = Item[0]["quality"].ToString();
            // Sound
            VisualScoreSound.Update(Convert.ToInt32(Item[0]["sound"]));
            Lbl_Sound.Text = Item[0]["sound"].ToString();
            // Content
            VisualScoreContent.Update(Convert.ToInt32(Item[0]["content"]));
            Lbl_Content.Text = Item[0]["content"].ToString();
            // Pacing
            VisualScorePacing.Update(Convert.ToInt32(Item[0]["pacing"]));
            Lbl_Pacing.Text = Item[0]["pacing"].ToString();
            // Balance
            VisualScoreBalance.Update(Convert.ToInt32(Item[0]["balance"]));
            Lbl_Balance.Text = Item[0]["balance"].ToString();
            // Impression
            VisualScoreImpression.Update(Convert.ToInt32(Item[0]["impression"]));
            Lbl_Impression.Text = Item[0]["impression"].ToString();
            // Note
            if (Item[0]["note"].ToString() != string.Empty)
            {
                TextNote.Visible = true;
                Pic_Note.Visible = true;
                TextNote.Text = Item[0]["note"].ToString();
            }
        }


        private void PnlContent_MouseMove(object sender, MouseEventArgs e)
        {
            Index.ReleaseCapture();
            Index.SendMessage(Handle, Index.WM_NCLBUTTONDOWN, Index.HT_CAPTION, 0);
        }

        private void BtnCloseView_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
            EditMode = false;

        }

        public decimal CountSum()
        {
            // if we disable a rating we still need to count it as a 10 internally. 
            int n = 0;
            if (Rate_Gameplay.Value == 0) n += 10;
            if (Rate_Presentation.Value == 0) n += 10;
            if (Rate_Narrative.Value == 0) n += 10;
            if (Rate_Quality.Value == 0) n += 10;
            if (Rate_Sound.Value == 0) n += 10;
            if (Rate_Content.Value == 0) n += 10;
            if (Rate_Pacing.Value == 0) n += 10;
            if (Rate_Balance.Value == 0) n += 10;
            SumScore = Rate_Gameplay.Value + Rate_Presentation.Value + Rate_Narrative.Value + Rate_Quality.Value +
            Rate_Sound.Value + Rate_Content.Value + Rate_Pacing.Value + Rate_Balance.Value + Rate_Impression.Value + 10 + n;
            //Lbl_SumScore.Text = SumScore + "";
            return SumScore;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (EditMode == false)
            {
                EditMode = true;
                // Enable Buttons
                BtnSaveChanges.Visible = true;
                BtnSaveChanges2.Visible = true;
                BtnSaveChanges2.BringToFront();
                BtnDelete.Visible = true;
                BtnEditCover.Visible = true;
                // Hide score
                Pnl_HideScore.Visible = true;
                Pnl_HideScore.BringToFront();
                // Change name
                Edit_Name.Visible = true;
                Edit_Name.Text = Item[0]["name"].ToString();
                // Change date
                Pnl_HideDate.Visible = true;
                textBoxDay.Visible = true;
                textBoxDay.Text = Day;
                textBoxMonth.Visible = true;
                textBoxMonth.Text = Month;
                textBoxYear.Visible = true;
                textBoxYear.Text = Year;
                textBoxDay.BringToFront();
                textBoxMonth.BringToFront();
                textBoxYear.BringToFront();
                // Change gameplay score
                Rate_Gameplay.Visible = true;
                Rate_Gameplay.Value = Convert.ToInt32(Item[0]["gameplay"]);
                checkBoxGameplay.Visible = true;
                if (Convert.ToInt32(Item[0]["gameplay"]) == 0) checkBoxGameplay.Checked = false;
                // Change presentation score
                Rate_Presentation.Visible = true;
                Rate_Presentation.Value = Convert.ToInt32(Item[0]["presentation"]);
                checkBoxPresentation.Visible = true;
                if (Convert.ToInt32(Item[0]["presentation"]) == 0) checkBoxPresentation.Checked = false;
                // Change narrative score
                Rate_Narrative.Visible = true;
                Rate_Narrative.Value = Convert.ToInt32(Item[0]["narrative"]);
                checkBoxNarrative.Visible = true;
                if (Convert.ToInt32(Item[0]["narrative"]) == 0) checkBoxNarrative.Checked = false;
                // Change quality score
                Rate_Quality.Visible = true;
                Rate_Quality.Value = Convert.ToInt32(Item[0]["quality"]);
                checkBoxQuality.Visible = true;
                if (Convert.ToInt32(Item[0]["quality"]) == 0) checkBoxQuality.Checked = false;
                // Change sound score
                Rate_Sound.Visible = true;
                Rate_Sound.Value = Convert.ToInt32(Item[0]["sound"]);
                checkBoxSound.Visible = true;
                if (Convert.ToInt32(Item[0]["sound"]) == 0) checkBoxSound.Checked = false;
                // Change content score
                Rate_Content.Visible = true;
                Rate_Content.Value = Convert.ToInt32(Item[0]["content"]);
                checkBoxContent.Visible = true;
                if (Convert.ToInt32(Item[0]["content"]) == 0) checkBoxContent.Checked = false;
                // Change pacing score
                Rate_Pacing.Visible = true;
                Rate_Pacing.Value = Convert.ToInt32(Item[0]["pacing"]);
                checkBoxPacing.Visible = true;
                if (Convert.ToInt32(Item[0]["pacing"]) == 0) checkBoxPacing.Checked = false;
                // Change balance score
                Rate_Balance.Visible = true;
                Rate_Balance.Value = Convert.ToInt32(Item[0]["balance"]);
                checkBoxBalance.Visible = true;
                if (Convert.ToInt32(Item[0]["balance"]) == 0) checkBoxBalance.Checked = false;
                // Change impression score
                Rate_Impression.Visible = true;
                Rate_Impression.Value = Convert.ToInt32(Item[0]["impression"]);
                // Edit note
                Edit_Note.Visible = true;
                Edit_Note.Text = Item[0]["note"].ToString();
                // Dispose sum score
                Pnl_HideScore.Visible = true;
                Pnl_HideScore.BringToFront();
                // Note icon 
                Pic_Note.Visible = true;
                BtnDelete.Visible = true;
                BtnSaveChanges2.Visible = true;
                ScoreSum.SumCreate(5, 7, Convert.ToInt32(CountSum()), Pnl_HideScore);
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
            }
        }

        private void Rate_Gameplay_ValueChanged(object sender, EventArgs e)
        {
            CountSum();
            ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
            VisualScoreGameplay.Update(Convert.ToInt32(Rate_Gameplay.Value));
        }

        private void Rate_Presentation_ValueChanged(object sender, EventArgs e)
        {
            CountSum();
            ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
            VisualScorePresentation.Update(Convert.ToInt32(Rate_Presentation.Value));
        }

        private void Rate_Narrative_ValueChanged(object sender, EventArgs e)
        {
            CountSum();
            ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
            VisualScoreNarrative.Update(Convert.ToInt32(Rate_Narrative.Value));
        }

        private void Rate_Quality_ValueChanged(object sender, EventArgs e)
        {
            CountSum();
            ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
            VisualScoreQuality.Update(Convert.ToInt32(Rate_Quality.Value));
        }

        private void Rate_Sound_ValueChanged(object sender, EventArgs e)
        {
            CountSum();
            ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
            VisualScoreSound.Update(Convert.ToInt32(Rate_Sound.Value));
        }

        private void Rate_Content_ValueChanged(object sender, EventArgs e)
        {
            CountSum();
            ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
            VisualScoreContent.Update(Convert.ToInt32(Rate_Content.Value));
        }

        private void Rate_Pacing_ValueChanged(object sender, EventArgs e)
        {
            CountSum();
            ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
            VisualScorePacing.Update(Convert.ToInt32(Rate_Pacing.Value));
        }

        private void Rate_Balance_ValueChanged(object sender, EventArgs e)
        {
            CountSum();
            ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
            VisualScoreBalance.Update(Convert.ToInt32(Rate_Balance.Value));
        }

        private void Rate_Impression_ValueChanged(object sender, EventArgs e)
        {
            CountSum();
            ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
            VisualScoreImpression.Update(Convert.ToInt32(Rate_Impression.Value));
        }

        private void checkBoxGameplay_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxGameplay.Checked)
            {
                Rate_Gameplay.Value = 0;
                Rate_Gameplay.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreGameplay.Update(Convert.ToInt32(Rate_Gameplay.Value));
                _ = Index.Notify("Gameplay won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Gameplay.Enabled = true;
                Rate_Gameplay.Value = 1;
                _ = Index.Notify("Gameplay will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxPresentation_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxPresentation.Checked)
            {
                Rate_Presentation.Value = 0;
                Rate_Presentation.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScorePresentation.Update(Convert.ToInt32(Rate_Presentation.Value));
                _ = Index.Notify("Presentation won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Presentation.Value = 1;
                Rate_Presentation.Enabled = true;
                _ = Index.Notify("Presentation will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxNarrative_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxNarrative.Checked)
            {
                Rate_Narrative.Value = 0;
                Rate_Narrative.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreNarrative.Update(Convert.ToInt32(Rate_Narrative.Value));
                _ = Index.Notify("Narrative won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Narrative.Value = 1;
                Rate_Narrative.Enabled = true;
                _ = Index.Notify("Narrative will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxQuality_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxQuality.Checked)
            {
                Rate_Quality.Value = 0;
                Rate_Quality.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreQuality.Update(Convert.ToInt32(Rate_Quality.Value));
                _ = Index.Notify("Quality won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Quality.Value = 1;
                Rate_Quality.Enabled = true;
                _ = Index.Notify("Quality will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxSound_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxSound.Checked)
            {
                Rate_Sound.Value = 0;
                Rate_Sound.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreSound.Update(Convert.ToInt32(Rate_Sound.Value));
                _ = Index.Notify("Sound won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Sound.Value = 1;
                Rate_Sound.Enabled = true;
                _ = Index.Notify("Sound will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxContent_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxContent.Checked)
            {
                Rate_Content.Value = 0;
                Rate_Content.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreContent.Update(Convert.ToInt32(Rate_Content.Value));
                _ = Index.Notify("Content won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Content.Value = 1;
                Rate_Content.Enabled = true;
                _ = Index.Notify("Content will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxPacing_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxPacing.Checked)
            {
                Rate_Pacing.Value = 0;
                Rate_Pacing.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScorePacing.Update(Convert.ToInt32(Rate_Pacing.Value));
                _ = Index.Notify("Pacing won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Pacing.Value = 1;
                Rate_Pacing.Enabled = true;
                _ = Index.Notify("Pacing will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxBalance_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxBalance.Checked)
            {
                Rate_Balance.Value = 0;
                Rate_Balance.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreBalance.Update(Convert.ToInt32(Rate_Balance.Value));
                _ = Index.Notify("Balance won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Balance.Value = 1;
                Rate_Balance.Enabled = true;
                _ = Index.Notify("Balance will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            BtnDeleteConfirm.Visible = true;
            BtnCancel.BringToFront();
            BtnCancel.Visible = true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            BtnDeleteConfirm.Visible = false;
            BtnCancel.Visible = false;
        }

        private void BtnDeleteConfirm_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(@"Data source = database.db");
            conn.Open();
            string query = "";
            try
            {
                if (CurrentItemReplayStatus == 1)
                {
                    // Delete record, but not infos, cause duplicate
                    query = $"DELETE FROM records WHERE id = '{CurrentItemID}'";
                    SQLiteCommand InsertSQL = new SQLiteCommand(query, conn);
                    InsertSQL = new SQLiteCommand(query, conn);
                    InsertSQL.ExecuteNonQuery();
                }
                if (CurrentItemReplayStatus == 0)
                {
                    // Delete record
                    query = $"DELETE FROM records WHERE id = '{CurrentItemID}'";
                    SQLiteCommand InsertSQL = new SQLiteCommand(query, conn);
                    InsertSQL = new SQLiteCommand(query, conn);
                    InsertSQL.ExecuteNonQuery();

                    // Delete information cause unique
                    query = $"DELETE FROM infos WHERE rId = '{CurrentItemRId}'";
                    SQLiteCommand InsertSQL2 = new SQLiteCommand(query, conn);
                    InsertSQL2 = new SQLiteCommand(query, conn);
                    InsertSQL2.ExecuteNonQuery();
                }
                _ = Index.Notify($"{CurrentItemName} has been deleted.", 1, Index.PnlNotify);
            }
            catch
            {
                _ = Index.Notify($"Couldn't delete {CurrentItemName}.", 3, Index.PnlNotify);
            }
            conn.Close();
            this.Close();
        }

        private void textBoxDay_Leave(object sender, EventArgs e)
        {
            textBoxDay.Text = Input.CheckForSingleDigitInDate(textBoxDay.Text);
        }

        private void textBoxMonth_Leave(object sender, EventArgs e)
        {
            textBoxMonth.Text = Input.CheckForSingleDigitInDate(textBoxMonth.Text);
        }

        private void BtnEditCover_Click(object sender, EventArgs e)
        {
            Random rng = new Random();
            string filePath = "";
            if (Edit_Name.Text != "")
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = @"C:\";
                    openFileDialog.Filter = "Files|*.jpg;*.jpeg;*.png;";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = openFileDialog.FileName;
                        string targetPath = @$"{(Edit_Name.Text.Substring(0, 3).ToUpper() + DateTime.Now.Millisecond.ToString() + rng.Next(10000, 99999))}.png";
                        MessageBox.Show(targetPath);
                        File.Copy(filePath, @$"img\grid\{targetPath}");
                        PicBoxCover.Image = Image.FromFile(@$"img\grid\{targetPath}");
                        // Set new cover
                        Cover_Path = targetPath;

                    }
                }
            }
            else
            {
                _ = Index.Notify("Please add a game title before set picture", 3, Index.PnlNotify, 3000);
            }
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(@"Data source = database.db");
            conn.Open();
            string query = "";
            try
            {
                // Delete record, but not infos, cause duplicate
                if (Edit_Name.Text.Contains('\'')) Edit_Name.Text.Replace("\'", "´");
                if (Edit_Note.Text.Contains('\'')) Edit_Name.Text.Replace("\'", "´");
                query = $"UPDATE records " +
                    $"SET " +
                    $"name = '{Edit_Name.Text}', " +
                    $"date = '{(textBoxYear.Text)}-{(textBoxMonth.Text)}-{(textBoxDay.Text)}', " +
                    $"note = '{Edit_Note.Text}' " +
                    $"WHERE id = '{CurrentItemID}'";
                SQLiteCommand InsertSQL = new SQLiteCommand(query, conn);
                InsertSQL = new SQLiteCommand(query, conn);
                InsertSQL.ExecuteNonQuery();
                query = $"UPDATE infos " +
                    $"SET " +
                    $"gameplay = '{Rate_Gameplay.Value}', " +
                    $"presentation = '{Rate_Presentation.Value}', " +
                    $"narrative = '{Rate_Narrative.Value}', " +
                    $"quality = '{Rate_Quality.Value}', " +
                    $"sound = '{Rate_Sound.Value}', " +
                    $"content = '{Rate_Content.Value}', " +
                    $"pacing = '{Rate_Pacing.Value}', " +
                    $"balance = '{Rate_Balance.Value}', " +
                    $"impression = '{Rate_Impression.Value}', " +
                    $"sum = '{SumScore}', " +
                    $"img = '{Cover_Path}' " +
                    $"WHERE rId = '{CurrentItemRId}'";              
                SQLiteCommand InsertSQL2 = new SQLiteCommand(query, conn);
                InsertSQL2 = new SQLiteCommand(query, conn);
                InsertSQL2.ExecuteNonQuery();
                _ = Index.Notify($"{CurrentItemName} has been edited.", 1, Index.PnlNotify);
            }
            catch (Exception ex)
            {
                Log.Write(1, ex.ToString());
                _ = Index.Notify($"Couldn't edit {CurrentItemName}.", 3, Index.PnlNotify);
            }
            conn.Close();
            EditMode = false;
            this.Close();
        }
    }
}
