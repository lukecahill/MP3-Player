using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Music_Player {
    public partial class interfaceForm : Form {
        MusicPlayer player = new MusicPlayer();
        Serialisation serialisation = new Serialisation();
        private Timer timer;
        private int scroll { get; set; }

        private string filename = "playlist.dat";

        public interfaceForm() {
            InitializeComponent();
        }

        private void addMusicBtn_Click(object sender, EventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Music (*.mp3) | *.mp3";
            dlg.Multiselect = true;
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK) {
                try {
                    foreach (var file in dlg.FileNames) {
                        serialisation.SetFilenames(listBox1, file);
                    }
                } catch {
                    MessageBox.Show("Could not add file");
                }
            }
        }

        private void playBtn_Click(object sender, EventArgs e) {
            try {
                if (listBox1.SelectedIndex == -1) {
                    if (listBox1.Items.Count >= 1) {
                        listBox1.SelectedIndex = 0;
                    }
                }

                stopBtn.Enabled = true;
                label1.Visible = true;
                pauseBtn.Enabled = true;

                SetNowPlayingText();

                var item = listBox1.SelectedItem as ListBoxItem;
                player.open(item.Path);
                player.play();
                PreviousNextEnabled();
            } catch {
                label1.Visible = false;
                pauseBtn.Enabled = false;
                stopBtn.Enabled = false;
                MessageBox.Show("Not a valid mp3 file!");
            }
        }

        private void stopBtn_Click(object sender, EventArgs e) {
            player.stop();
            label1.Visible = false;
            pauseBtn.Enabled = false;
        }

        private void pauseBtn_Click(object sender, EventArgs e) {
            player.pause();
        }

        private void nextBtn_Click(object sender, EventArgs e) {
            try {
                player.stop();
                try {
                    listBox1.SelectedIndex += 1;
                } catch {
                    MessageBox.Show("No more songs in list!");
                }
                playBtn.PerformClick();
            } catch {
                MessageBox.Show("Could not play next song");
            }
        }

        private void previousBtn_Click(object sender, EventArgs e) {
            try {
                player.stop();
                try {
                    listBox1.SelectedIndex -= 1;
                } catch {
                    MessageBox.Show("No more songs in list!");
                }
                playBtn.PerformClick();
            } catch {
                MessageBox.Show("Could not play next song");
            }
        }

        private DialogResult ShowSaveDialog() {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Data File (*.dat, *.play) | *.dat, .play";
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK) {
                filename = dialog.FileName;
            }

            return result;
        }

        private void saveBtn_Click(object sender, EventArgs e) {
            if (ShowSaveDialog() != DialogResult.OK) {
                return;
            }

            serialisation.SavePlaylist(listBox1, filename);
        }

        private void loadBtn_Click(object sender, EventArgs e) {
            serialisation.OpenPlaylist(listBox1, filename);
        }

        private void Form1_Load(object sender, EventArgs e) {
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(this.TimerTick);
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e) {
            //ScrollLabel();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e) {
            stopBtn.PerformClick();
            playBtn.PerformClick();
        }



        private void SetNowPlayingText() {
            string[] playing = listBox1.SelectedItem.ToString().Split('\\');
            string[] nowplaying = Regex.Split(playing.Last(), ".mp3");
            label1.Text = $"Now Playing:    {nowplaying.First()}";
        }

        private void PreviousNextEnabled() {
            if (listBox1.SelectedIndex == (listBox1.Items.Count - 1)) {
                nextBtn.Enabled = false;
                previousBtn.Enabled = true;
            } else if (listBox1.SelectedIndex == 0) {
                previousBtn.Enabled = false;
                nextBtn.Enabled = true;
            } else {
                nextBtn.Enabled = true;
                previousBtn.Enabled = true;
            }
        }
    }
}