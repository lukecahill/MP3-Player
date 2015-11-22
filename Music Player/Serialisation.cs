using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Music_Player {
    public class Serialisation {
        
        public void SavePlaylist(ListBox listbox, string filename) {

            try {
                var linesToSave = new List<string>();

                for (var i = 0; listbox.Items.Count > i; i++) {
                    listbox.SelectedIndex = i;
                    var item = listbox.SelectedItem as ListBoxItem;
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
                MessageBox.Show($"Could not open {filename} for saving.\nNo access rights to the folder, perhaps?", "File save problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MessageBox.Show($"{ex}");
            }
        }

        public void OpenPlaylist(ListBox listbox, string filename) {
            var open = new OpenFileDialog();
            var re = new Regex("\r\n");
            var result = open.ShowDialog();

            if (result == DialogResult.OK) {
                var file = open.FileName;
                try {
                    listbox.Items.Clear();

                    var lines = new List<string>();

                    using (var reader = new StreamReader(file)) {
                        var text = reader.ReadToEnd();
                        var newLines = re.Split(text);
                        foreach(var item in newLines) {
                            lines.Add(item);
                        }
                    }

                    foreach (var line in lines) {
                        SetFilenames(listbox, line);
                    }


                } catch (IOException) {
                    MessageBox.Show("Could not load data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        public void SetFilenames(ListBox listbox, string path) {
            var item = new ListBoxItem();

            string[] playing = path.Split('\\');
            string[] nowplaying = Regex.Split(playing.Last(), ".mp3");
            item.Text = nowplaying.First();
            item.Name = nowplaying.First();
            item.Path = path;

            listbox.Items.Add(item);
        }
    }
}
