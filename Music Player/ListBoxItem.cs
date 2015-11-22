using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player
{
    public class ListBoxItem : Object
    {
        public virtual string Text { get; set; }
        public virtual object Object { get; set; }
        public virtual string Name { get; set; }
        public virtual string Path { get; set; }

        public ListBoxItem()
        {
            this.Text = string.Empty;
            this.Name = string.Empty;
            this.Path = string.Empty;
            this.Object = null;
        }

        public ListBoxItem(string Text, string Name, object Object, string Path)
        {
            this.Text = Text;
            this.Name = Name;
            this.Path = Path;
            this.Object = Object;
        }

        public ListBoxItem(object Object)
        {
            this.Text = Object.ToString();
            this.Name = Object.ToString();
            this.Object = Object;
            this.Path = Object.ToString();
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
