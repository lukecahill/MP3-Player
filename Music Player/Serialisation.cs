using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Music_Player {
    public class Serialisation {
        Regex re = new Regex("\r\n");

        public void SavePlaylist(ListBox listbox, string filename) {
            try {
                var linesToSave = new List<string>();

                for (var i = 0; listbox.Items.Count > i; i++) {
                    listbox.SelectedIndex = i;
                    var item = listbox.SelectedItem as ListBoxItem;
                    linesToSave.Add(item.Path);
                }
                WritePlaylist(filename, linesToSave);

                MessageBox.Show("Saved");
            } catch (Exception ex) {
                MessageBox.Show($"Could not open {filename} for saving.\nNo access rights to the folder, perhaps?", "File save problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MessageBox.Show($"{ex}");
            }
        }

        public void OpenPlaylist(ListBox listbox, string filename) {
            var open = new OpenFileDialog();
            var result = open.ShowDialog();

            if (result == DialogResult.OK) {
                var file = open.FileName;
                try {
                    listbox.Items.Clear();

                    var lines = ReadPlaylist(file);
                    foreach (var line in lines) {
                        SetFilenames(listbox, line);
                    }
                    
                } catch (IOException) {
                    MessageBox.Show("Could not load data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        public void WritePlaylist(string filename, List<string> linesToSave) {
            using (var stream = new StreamWriter(filename)) {
                foreach (var line in linesToSave) {
                    stream.WriteLine(line);
                }
                stream.Flush();
            }
        }

        public List<string> ReadPlaylist(string file) {
            var lines = new List<string>();

            using (var reader = new StreamReader(file)) {
                var text = reader.ReadToEnd();
                var newLines = re.Split(text);
                foreach (var item in newLines) {
                    lines.Add(item);
                }
            }

            return lines;
        }

        public void SetFilenames(ListBox listbox, string path) {
            var item = new ListBoxItem();

            var nowplaying = Path.GetFileNameWithoutExtension(path);
            item.Text = nowplaying;
            item.Name = nowplaying;
            item.Path = path;

            listbox.Items.Add(item);
        }
    }
}
