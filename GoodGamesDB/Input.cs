using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GoodGamesDB
{
    public partial class Input : Form
    {
        string Name = string.Empty;
        string Location = string.Empty;
        public static string Appid = string.Empty;
        string PlaytimeH = string.Empty;
        int RId = 0;
        int PlaythroughCount = 0;
        int Day = 0;
        int Month = 0;
        int Year = 0;    
        bool IsReplay = false;
        string Note = string.Empty;
        string AdditionalContent = string.Empty;
        List<string> DLC = new List<string>();
        bool ImageHasBeenSet = false;

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


        public Input()
        {
            InitializeComponent();
            CreateScorePreview();
            textBoxDay.Text = DateTime.Now.Day.ToString();
            textBoxDay.Text = CheckForSingleDigitInDate(textBoxDay.Text);
            textBoxMonth.Text = DateTime.Now.Month.ToString();
            textBoxMonth.Text = CheckForSingleDigitInDate(textBoxMonth.Text);
            textBoxYear.Text = DateTime.Now.Year.ToString();
            textBoxPlaytime.Text = "" + 0;
            var source = new AutoCompleteStringCollection();
            foreach (string s in Index.GamesList)
            {
                source.Add(s);
            }
            textBoxGame.AutoCompleteCustomSource = source;
            textBoxGame.AutoCompleteMode = AutoCompleteMode.Suggest;
            SumScore = CountSum();
        }

        private void WriteToDatabase()
        {
            // if we handle non-steam game
            if(Appid == "" || Appid == null) 
            {
                Appid = "0";
            }
            // workaround for sql errors 
            if (textBoxGame.Text.Contains('\'')) textBoxGame.Text = textBoxGame.Text.Replace('\'', '´');
            if (!IsReplay)
            {
                int rId = 0;
                SQLiteConnection conn = new SQLiteConnection(@"Data source = database.db");
                conn.Open();

                // Create infos row
                string query = $"INSERT INTO infos(name, appid, gameplay, presentation, narrative, quality, sound, content, pacing, balance, impression, " +
                    $"sum, hours_total, hours_update, img)\r\nVALUES " +
                    $"(\"{textBoxGame.Text}\", {Appid}, {Rate_Gameplay.Value}, {Rate_Presentation.Value}, {Rate_Narrative.Value}, {Rate_Quality.Value}, " +
                    $"{Rate_Sound.Value}, {Rate_Content.Value}, {Rate_Pacing.Value}, {Rate_Balance.Value}, {Rate_Impression.Value}, {SumScore}," +
                    $" {textBoxPlaytime.Text}, '{(textBoxYear.Text)}-{(textBoxMonth.Text)}-{(textBoxDay.Text)}', \"{textBoxImagePath.Text}\") ";
                SQLiteCommand InsertSQL = new SQLiteCommand(query, conn);
                InsertSQL.ExecuteNonQuery();

                // Get rID from infos
                query = $"SELECT * FROM infos where name = \"{textBoxGame.Text}\" LIMIT 1";
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                rId = Convert.ToInt32(reader[0]);
                reader.Close();

                // Create new record
                query = $"INSERT INTO records(name, date, location, hours, note, dlc, replay, link, pcount) VALUES " +
                $" (\"{textBoxGame.Text}\", '{(textBoxYear.Text)}-{(textBoxMonth.Text)}-{(textBoxDay.Text)}', \"{textBoxLocation.Text}\", {textBoxPlaytime.Text}, \"{textBoxNote.Text}\", \"{AdditionalContent}\", 0, {rId}, 1)";
                InsertSQL = new SQLiteCommand(query, conn);
                InsertSQL.ExecuteNonQuery();
                conn.Close();
            }
            if (IsReplay)
            {
                // Get the related infos for the replay.
                int rId = 0;
                SQLiteConnection conn = new SQLiteConnection(@"Data source = database.db");
                conn.Open();
                string query = $"SELECT * FROM infos where name = '{textBoxGame.Text}' LIMIT 1";
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                // Get the related row in infos
                rId = Convert.ToInt32(reader[0]);
                reader.Close();
                // increment playthrough by 1
                query = $"SELECT * FROM records where name = '{textBoxGame.Text}' ORDER BY date DESC LIMIT 1";
                cmd.CommandText = query;
                SQLiteDataReader reader2 = cmd.ExecuteReader();
                reader2.Read();
                PlaythroughCount = Convert.ToInt32(reader2[9]);
                PlaythroughCount++;
                reader2.Close();
                // Create new record
                query = $"INSERT INTO records(name, date, location, hours, note, dlc, replay, link, pcount) VALUES " +
                $" (\"{textBoxGame.Text}\", '{(textBoxYear.Text)}-{(textBoxMonth.Text)}-{(textBoxDay.Text)}', \"{textBoxLocation.Text}\", {textBoxPlaytime.Text}, \"{textBoxNote.Text}\", \"{AdditionalContent}\", 1, {rId}, {PlaythroughCount})";
                SQLiteCommand InsertSQL = new SQLiteCommand(query, conn);
                InsertSQL.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void LoadGameInfo(string game)
        {
            DataRow[]? info = Index.Data.Select("name like '%" + game + "%'");
            foreach(DataRow row in info)
            {
                // Load cover and prepare it for data writing
                pictureBoxCover.Image = Image.FromFile($"img/grid/{row["img"].ToString()}");
                pictureBoxCover.SizeMode = PictureBoxSizeMode.Zoom;
                textBoxImagePath.Text = row["img"].ToString();
                ImageHasBeenSet = true;

                // Load score values and disable editing
                // Gameplay
                VisualScoreGameplay.Update(Convert.ToInt32(row["gameplay"]));
                Rate_Gameplay.Value = Convert.ToDecimal(row["gameplay"]);
                Rate_Gameplay.Enabled = false;
                checkBoxGameplay.Enabled = false;

                // Presentation
                VisualScorePresentation.Update(Convert.ToInt32(row["presentation"]));
                Rate_Presentation.Value = Convert.ToDecimal(row["presentation"]);
                Rate_Presentation.Enabled = false;
                checkBoxPresentation.Enabled = false;

                // Narrative
                VisualScoreNarrative.Update(Convert.ToInt32(row["narrative"]));
                Rate_Narrative.Value = Convert.ToDecimal(row["narrative"]);
                Rate_Narrative.Enabled = false;
                checkBoxNarrative.Enabled = false;

                // Quality
                VisualScoreQuality.Update(Convert.ToInt32(row["quality"]));
                Rate_Quality.Value = Convert.ToDecimal(row["quality"]);
                Rate_Quality.Enabled = false;
                checkBoxQuality.Enabled = false;

                // Sound
                VisualScoreSound.Update(Convert.ToInt32(row["sound"]));
                Rate_Sound.Value = Convert.ToDecimal(row["sound"]);
                Rate_Sound.Enabled = false;
                checkBoxSound.Enabled = false;

                // Content
                VisualScoreContent.Update(Convert.ToInt32(row["content"]));
                Rate_Content.Value = Convert.ToDecimal(row["content"]);
                Rate_Content.Enabled = false;
                checkBoxContent.Enabled = false;

                // Pacing
                VisualScorePacing.Update(Convert.ToInt32(row["pacing"]));
                Rate_Pacing.Value = Convert.ToDecimal(row["pacing"]);
                Rate_Pacing.Enabled = false;
                checkBoxPacing.Enabled = false;

                // Balance
                VisualScoreBalance.Update(Convert.ToInt32(row["balance"]));
                Rate_Balance.Value = Convert.ToDecimal(row["balance"]);
                Rate_Balance.Enabled = false;
                checkBoxBalance.Enabled = false;

                // Impression
                VisualScoreImpression.Update(Convert.ToInt32(row["impression"]));
                Rate_Impression.Value = Convert.ToDecimal(row["impression"]);
                Rate_Impression.Enabled = false;

                // Finally recount the total score
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                break;
            }

        }
        private void ResetGameInfo()
        {
            // Load cover and prepare it for data writing
            pictureBoxCover.Image = null;
            textBoxImagePath.Text = string.Empty;
            ImageHasBeenSet = false;

            // Load score values and disable editing
            // Gameplay
            VisualScoreGameplay.Update(1);
            Rate_Gameplay.Value = Convert.ToDecimal(1);
            Rate_Gameplay.Enabled = true;
            checkBoxGameplay.Enabled = true;

            // Presentation
            VisualScorePresentation.Update(1);
            Rate_Presentation.Value = Convert.ToDecimal(1);
            Rate_Presentation.Enabled = true;
            checkBoxPresentation.Enabled = true;

            // Narrative
            VisualScoreNarrative.Update(1);
            Rate_Narrative.Value = Convert.ToDecimal(1);
            Rate_Narrative.Enabled = true;
            checkBoxNarrative.Enabled = true;

            // Quality
            VisualScoreQuality.Update(1);
            Rate_Quality.Value = Convert.ToDecimal(1);
            Rate_Quality.Enabled = true;
            checkBoxQuality.Enabled = true;

            // Sound
            VisualScoreSound.Update(1);
            Rate_Sound.Value = Convert.ToDecimal(1);
            Rate_Sound.Enabled = true;
            checkBoxSound.Enabled = true;

            // Content
            VisualScoreContent.Update(1);
            Rate_Content.Value = Convert.ToDecimal(1);
            Rate_Content.Enabled = true;
            checkBoxContent.Enabled = true;

            // Pacing
            VisualScorePacing.Update(1);
            Rate_Pacing.Value = Convert.ToDecimal(1);
            Rate_Pacing.Enabled = true;
            checkBoxPacing.Enabled = true;

            // Balance
            VisualScoreBalance.Update(1);
            Rate_Balance.Value = Convert.ToDecimal(1);
            Rate_Balance.Enabled = true;
            checkBoxBalance.Enabled = true;

            // Impression
            VisualScoreImpression.Update(1);
            Rate_Impression.Value = Convert.ToDecimal(1);
            Rate_Impression.Enabled = true;

            // Finally recount the total score
            CountSum();
            ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
        }
        private void CreateScorePreview()
        {
            VisualScoreGameplay.Create(750, 105, "Gameplay", PnlContent); // y+30
            VisualScorePresentation.Create(750, 135, "Presentation", PnlContent);
            VisualScoreNarrative.Create(750, 165, "Narrative", PnlContent);
            VisualScoreQuality.Create(750, 195, "Quality", PnlContent);
            VisualScoreSound.Create(750, 225, "Sound", PnlContent);
            VisualScoreContent.Create(750, 255, "Content", PnlContent);
            VisualScorePacing.Create(750, 285, "Pacing", PnlContent);
            VisualScoreBalance.Create(750, 315, "Balance", PnlContent);
            VisualScoreImpression.Create(750, 345, "Impression", PnlContent);

            ScoreSum.SumCreate(750, 400, 19, PnlContent);
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ImageHasBeenSet)
                {
                    // Set basic values
                    Name = textBoxGame.Text;
                    Location = textBoxLocation.Text;
                    Year = Convert.ToInt32(textBoxYear.Text);
                    Month = Convert.ToInt32(textBoxMonth.Text);
                    Day = Convert.ToInt32(textBoxDay.Text);
                    PlaytimeH = textBoxPlaytime.Text;
                    Note = textBoxNote.Text;
                    // Build database-string from dlc
                    foreach (string item in DLC)
                    {
                        AdditionalContent += $"{item};!";
                    }
                    if(AdditionalContent == null) AdditionalContent = string.Empty;
                    WriteToDatabase();
                    // Sent confirmation details to parent form
                    Index.LatestNewEntry = $"{Name}";
                    Index.Status = 0;
                    Log.Write(Index.Status, $"{Name} wurde in der Datenbank gespeichert.");
                    this.Close();
                }
                else
                {
                    Index.Notify("You can't save this entry without an image!", 3, Index.PnlNotify);
                }

            }
            catch (Exception ex)
            {
                Index.Status = -1;
                Log.Write(Index.Status, $"{Name} konnte nicht gespeichert werden: {ex}");
                _ = Index.Notify("Couldn't write to database. Check logfile for more information.", 3, Index.PnlNotify);
            }
        }
        private void BtnSetReplay_Click(object sender, EventArgs e)
        {
            if (!IsReplay)
            {
                BtnSetReplay.BackColor = Color.FromArgb(192, 255, 192);
                BtnSetReplay.Text = "✔️";
                IsReplay = true;
                LoadGameInfo(textBoxGame.Text);
            }
            else
            {
                BtnSetReplay.BackColor = Color.WhiteSmoke;
                BtnSetReplay.Text = "Replay";
                IsReplay = false;
                ResetGameInfo();
            }
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private decimal CountSum()
        {
            SumScore = Rate_Gameplay.Value + Rate_Presentation.Value + Rate_Narrative.Value + Rate_Quality.Value +
            Rate_Sound.Value + Rate_Content.Value + Rate_Pacing.Value + Rate_Balance.Value + Rate_Impression.Value + 10;
            //Lbl_SumScore.Text = SumScore + "";
            return SumScore;
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

        private void BtnAddContent_Click(object sender, EventArgs e)
        {
            if (textBoxAdditionalContent.Text.Length > 0)
            {
                DLC.Add(textBoxAdditionalContent.Text);
                LblAddContentConfirm.Visible = true;
                LblAddContentConfirm.Text += "+ " + textBoxAdditionalContent.Text + Environment.NewLine;
                textBoxAdditionalContent.Text = string.Empty;
            }
        }

        private void pictureBoxCover_Click(object sender, EventArgs e)
        {
            Random rng = new Random();
            string filePath = "";
            if (textBoxGame.Text != "")
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
                    }
                }
                string targetPath = @$"{(textBoxGame.Text.Substring(0, 3).ToUpper() + DateTime.Now.Millisecond.ToString() + rng.Next(10000, 99999))}.png";
                //File.Move(filePath, (textBoxGame.Text.Substring(1, 3) + DateTime.Now.Millisecond.ToString()));
                File.Copy(filePath, @$"img\grid\{targetPath}");
                textBoxImagePath.Text = targetPath;
                pictureBoxCover.Image = Image.FromFile(@"img\grid\" + targetPath);
                pictureBoxCover.SizeMode = PictureBoxSizeMode.StretchImage;
                ImageHasBeenSet = true;
            }
            else
            {
                ImageHasBeenSet = false;
                _ = Index.Notify("Please add a game title before set picture", 3, Index.PnlNotify, 3000);
            }

        }

        private void textBoxLocation_Leave(object sender, EventArgs e)
        {
            if (textBoxLocation.Text == "Steam")
            {
                textBoxPlaytime.Text = PlaytimeH;
            }
        }

        private void textBoxGame_Leave(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, string> elem in Index.DictGamesList)
            {
                if (elem.Key == textBoxGame.Text) PlaytimeH = SteamData.ShowPlaytime(elem.Value);
            }
        }

        private void checkBoxGameplay_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxGameplay.Checked)
            {
                Rate_Gameplay.Value = 10;
                Rate_Gameplay.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreGameplay.Update(Convert.ToInt32(Rate_Gameplay.Value));
                _ = Index.Notify("Gameplay won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Gameplay.Enabled = true;
                _ = Index.Notify("Gameplay will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxPresentation_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxPresentation.Checked)
            {
                Rate_Presentation.Value = 10;
                Rate_Presentation.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScorePresentation.Update(Convert.ToInt32(Rate_Presentation.Value));
                _ = Index.Notify("Presentation won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Presentation.Enabled = true;
                _ = Index.Notify("Presentation will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxNarrative_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxNarrative.Checked)
            {
                Rate_Narrative.Value = 10;
                Rate_Narrative.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreNarrative.Update(Convert.ToInt32(Rate_Narrative.Value));
                _ = Index.Notify("Narrative won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Narrative.Enabled = true;
                _ = Index.Notify("Narrative will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxQuality_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxQuality.Checked)
            {
                Rate_Quality.Value = 10;
                Rate_Quality.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreQuality.Update(Convert.ToInt32(Rate_Quality.Value));
                _ = Index.Notify("Quality won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Quality.Enabled = true;
                _ = Index.Notify("Quality will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxSound_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxSound.Checked)
            {
                Rate_Sound.Value = 10;
                Rate_Sound.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreSound.Update(Convert.ToInt32(Rate_Sound.Value));
                _ = Index.Notify("Sound won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Sound.Enabled = true;
                _ = Index.Notify("Sound will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxContent_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxContent.Checked)
            {
                Rate_Content.Value = 10;
                Rate_Content.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreContent.Update(Convert.ToInt32(Rate_Content.Value));
                _ = Index.Notify("Content won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Quality.Enabled = true;
                _ = Index.Notify("Content will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxPacing_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxPacing.Checked)
            {
                Rate_Pacing.Value = 10;
                Rate_Pacing.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScorePacing.Update(Convert.ToInt32(Rate_Pacing.Value));
                _ = Index.Notify("Pacing won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Pacing.Enabled = true;
                _ = Index.Notify("Pacing will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private void checkBoxBalance_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxBalance.Checked)
            {
                Rate_Balance.Value = 10;
                Rate_Balance.Enabled = false;
                CountSum();
                ScoreSum.SumUpdate(Convert.ToInt32(SumScore));
                VisualScoreBalance.Update(Convert.ToInt32(Rate_Balance.Value));
                _ = Index.Notify("Balance won't be rated", 3, Index.PnlNotify, 3000);
            }
            else
            {
                Rate_Balance.Enabled = true;
                _ = Index.Notify("Balance will be rated again.", 1, Index.PnlNotify, 3000);
            }
        }

        private string CheckForSingleDigitInDate(string s)
        {
            if (s.Length == 1) s = "0" + s;
            return s;
        }

        private void textBoxMonth_Leave(object sender, EventArgs e)
        {
            textBoxMonth.Text = CheckForSingleDigitInDate(textBoxMonth.Text);
        }

        private void textBoxDay_Leave(object sender, EventArgs e)
        {
            textBoxDay.Text = CheckForSingleDigitInDate(textBoxDay.Text);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            this.Close();
            Input window = new Input();
            window.ShowDialog();
        }
    }
}