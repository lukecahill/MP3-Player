using System;
using System.Windows.Forms;

namespace Music_Player {
    public partial class interfaceForm : Form {
        MusicPlayer player = new MusicPlayer();
        Serialisation serialisation = new Serialisation();
        Helper helper = new Helper();
        private Timer timer;

        private string filename = "";

        public interfaceForm() {
            InitializeComponent();
        }

        private void addMusicBtn_Click(object sender, EventArgs e) {
			helper.addMusic(playlist);
        }

        private void playBtn_Click(object sender, EventArgs e) {
            try {
                if (playlist.SelectedIndex == -1) {
                    if (playlist.Items.Count >= 1) {
                        playlist.SelectedIndex = 0;
                    }
                }

                helper.setButtons(false, label1, pauseBtn, stopBtn);
                helper.SetNowPlayingText(playlist, label1);

                var item = playlist.SelectedItem as ListBoxItem;
				TagLib.File tagFile = TagLib.File.Create(item.Path);
				albumLabel.Text = tagFile.Tag.Album;

				player.open(item.Path);
                player.play();
                helper.previousNextEnabled(playlist, nextBtn, previousBtn);
            } catch {
                helper.setButtons(true, label1, pauseBtn, stopBtn);
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
                playlist.SelectedIndex += 1;
                playBtn.PerformClick();
            } catch {
                MessageBox.Show("No more songs in list");
            }
        }

        private void previousBtn_Click(object sender, EventArgs e) {
            try {
                player.stop();
                playlist.SelectedIndex -= 1;
                playBtn.PerformClick();
            } catch {
                MessageBox.Show("No more songs in list!");
            }
        }
		
        private void saveBtn_Click(object sender, EventArgs e) {
            if (helper.showSaveDialog(filename) != DialogResult.OK) {
                return;
            }

            serialisation.SavePlaylist(playlist, filename);
        }

        private void loadBtn_Click(object sender, EventArgs e) {
            serialisation.OpenPlaylist(playlist);
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

        private void clearPlaylist_Click(object sender, EventArgs e) {
            var result = MessageBox.Show("Are you sure you wish to clear the current playlist?", "", MessageBoxButtons.OKCancel);
            if(result == DialogResult.OK) {
                player.stop();
                playlist.Items.Clear();
                helper.setButtons(false, label1, pauseBtn, stopBtn);
            }
        }
	}
}