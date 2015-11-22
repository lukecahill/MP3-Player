using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Music_Player
{
    public partial class interfaceForm : Form
    {
        MusicPlayer player = new MusicPlayer();
        private Timer tmr;
        private int scrll { get; set; }
        
        private string filename = "playlist.dat";

        public interfaceForm()
        {
            InitializeComponent();
        }

        private void addMusicBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Music (*.mp3) | *.mp3";
            dlg.Multiselect = true;
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    foreach (String file in dlg.FileNames)
                    {
                        ListBoxItem item = new ListBoxItem();

                        string[] playing = file.Split('\\');
                        string[] nowplaying = Regex.Split(playing.Last(), ".mp3");
                        item.Text = nowplaying.First();
                        item.Name = nowplaying.First();
                        item.Path = file;

                        listBox1.Items.Add(item);
                    }
                }
                catch
                {
                    MessageBox.Show("Could not add file");
                }
            }
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex == -1)
                {
                    if (listBox1.Items.Count >= 1)
                    {
                        listBox1.SelectedIndex = 0;
                    }
                    //else
                    //{
                    //    MessageBox.Show("No music in playlist! Please add some before retrying...");
                    //}
                }

                stopBtn.Enabled = true;
                label1.Visible = true;
                pauseBtn.Enabled = true;

                string[] playing = listBox1.SelectedItem.ToString().Split('\\');
                string[] nowplaying = Regex.Split(playing.Last(), ".mp3");
                label1.Text = "Now Playing:    " + nowplaying.First();

                ListBoxItem item = (ListBoxItem)listBox1.SelectedItem;
                player.open(item.Path);
                player.play();

                if (listBox1.SelectedIndex == (listBox1.Items.Count - 1))
                {
                    nextBtn.Enabled = false;
                    previousBtn.Enabled = true;
                }
                else if (listBox1.SelectedIndex == 0)
                {
                    previousBtn.Enabled = false;
                    nextBtn.Enabled = true;
                }
                else
                {
                    nextBtn.Enabled = true;
                    previousBtn.Enabled = true;
                }
            }
            catch
            {
                label1.Visible = false;
                pauseBtn.Enabled = false;
                stopBtn.Enabled = false;
                MessageBox.Show("Not a valid mp3 file!");
            }
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            player.stop();
            label1.Visible = false;
            pauseBtn.Enabled = false;
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            player.pause();
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            try
            {
                player.stop();
                try
                {
                    listBox1.SelectedIndex += 1;
                }
                catch
                {
                    MessageBox.Show("No more songs in list!");
                }
                playBtn.PerformClick();
            }
            catch
            {
                MessageBox.Show("Could not play next song");
            }
        }

        private void previousBtn_Click(object sender, EventArgs e)
        {
            try
            {
                player.stop();
                try
                {
                    listBox1.SelectedIndex -= 1;
                }
                catch
                {
                    MessageBox.Show("No more songs in list!");
                }
                playBtn.PerformClick();
            }
            catch
            {
                MessageBox.Show("Could not play next song");
            }
        }

        DialogResult ShowSaveDialog()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Data File (*.dat, *.play) | *.dat, .play";
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                filename = dialog.FileName;
            }

            return result;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (ShowSaveDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                List<string> linesToSave = new List<string>();

                for (int i = 0; listBox1.Items.Count > i ; i++)
                {
                    listBox1.SelectedIndex = i;
                    ListBoxItem item = (ListBoxItem)listBox1.SelectedItem;
                    linesToSave.Add(item.Path);
                }


                //foreach (ListBoxItem i in listBox1.SelectedItem)
                //{
                //    linesToSave.Add(line );
                //}


                File.WriteAllLines(filename, linesToSave);
                MessageBox.Show("Saved");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open " + filename + " for saving.\nNo access rights to the folder, perhaps?", "FILE SAVING PROBLEM", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MessageBox.Show("" + ex);
            }
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            DialogResult result = open.ShowDialog();

            if (result == DialogResult.OK)
            {
                string file = open.FileName;
                try
                {
                    listBox1.Items.Clear();
                    string[] lines = File.ReadAllLines(filename);
                    foreach (var item in lines)
                    {
                        listBox1.Items.Add(item);
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Could not load data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tmr = new Timer();
            tmr.Interval = 100;
            tmr.Tick += new EventHandler(this.TimerTick);
            tmr.Start();
        }

        private void ScrollLabel()
        {
            //if (listBox1.SelectedIndex == -1)
            //{
            //    label1.Text = "Scollable placeholder";
            //}
            //else
            //{
            //    string[] playing = listBox1.SelectedItem.ToString().Split('\\');
            //    string[] nowplaying = Regex.Split(playing.Last(), ".mp3");

            //    string strString = nowplaying.First();

            //    scrll = scrll + 1;
            //    int iLmt = strString.Length - scrll;
            //    if (iLmt < nowplaying.Length)
            //    {
            //        scrll = 0;
            //    }
            //    string str = "Now Playing:    " + nowplaying.First().Substring(scrll, nowplaying.Length);//strString.Substring(scrll, 20);
            //    label1.Text = str;
            //}
        }

        private void TimerTick(object sender, EventArgs e)
        {
            ScrollLabel();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            stopBtn.PerformClick();
            playBtn.PerformClick();
        }
    }
}