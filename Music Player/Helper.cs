using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Music_Player {
    public class Helper {
        public void SetNowPlayingText(ListBox listbox, Label label) {
            string[] playing = listbox.SelectedItem.ToString().Split('\\');
            string[] nowplaying = Regex.Split(playing.Last(), ".mp3");
            label.Text = $"Now Playing:    {nowplaying.First()}";
        }

        public void PreviousNextEnabled(ListBox listbox, Button nextBtn, Button previousBtn) {
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

        public void SetButtons(bool enabled, Label label, Button pauseBtn, Button stopBtn) {
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
    }
}
