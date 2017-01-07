using System.IO;
using System.Windows.Forms;

namespace Music_Player {
    public class Helper {
		Serialisation serialisation = new Serialisation();

		public void SetNowPlayingText(ListBox listbox, Label label) {
            var playing = Path.GetFileName(listbox.SelectedItem.ToString());
            label.Text = $"Now Playing:    { playing }";
        }

        public void previousNextEnabled(ListBox listbox, Button nextBtn, Button previousBtn) {
            if (listbox.SelectedIndex == (listbox.Items.Count - 1)) {
                nextBtn.Enabled = false;
                previousBtn.Enabled = true;
            } else if (listbox.SelectedIndex == 0) {
                previousBtn.Enabled = false;
                nextBtn.Enabled = true;
            } else {
                nextBtn.Enabled = true;
                previousBtn.Enabled = true;
            }
        }

        public void setButtons(bool enabled, Label label, Button pauseBtn, Button stopBtn) {
            if (enabled) {
                label.Visible = false;
                pauseBtn.Enabled = false;
                stopBtn.Enabled = false;
            } else {
                label.Visible = true;
                pauseBtn.Enabled = true;
                stopBtn.Enabled = true;
            }
        }

		public void addMusic(ListBox playlist) {
			var dlg = new OpenFileDialog();
			dlg.Filter = "Music (*.mp3) | *.mp3";
			dlg.Multiselect = true;
			var result = dlg.ShowDialog();

			if (result == DialogResult.OK) {
				try {
					foreach (var file in dlg.FileNames) {
						serialisation.SetFilenames(playlist, file);
					}
				} catch {
					MessageBox.Show("Could not add file");
				}
			}
		}
		public DialogResult showSaveDialog(string filename) {
			var dialog = new SaveFileDialog();
			dialog.Filter = "Data File (*.dat, *.play) | *.dat, .play";
			var result = dialog.ShowDialog();

			if (result == DialogResult.OK) {
				filename = dialog.FileName;
			}

			return result;
		}
	}
}
