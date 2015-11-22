using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Music_Player {
    public class Serialisation {
        public void SavePlaylist(ListBox listbox, string filename) {

            try {
                var linesToSave = new List<string>();

                for (var i = 0; listbox.Items.Count > i; i++) {
                    listbox.SelectedIndex = i;
                    var item = (ListBoxItem)listbox.SelectedItem;
                    linesToSave.Add(item.Path);
                }

                using (var stream = new StreamWriter(filename)) {
                    foreach(var line in linesToSave) {
                        stream.WriteLine(line);
                    }
                    
                    stream.Flush();
                }

                MessageBox.Show("Saved");
            } catch (Exception ex) {
                MessageBox.Show("Could not open " + filename + " for saving.\nNo access rights to the folder, perhaps?", "File save problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MessageBox.Show("" + ex);
            }
        }

        public void OpenPlaylist() {

        }
    }
}
