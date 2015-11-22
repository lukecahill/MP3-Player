using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player
{
    class MusicFile
    {
        string _name, _path;

        public MusicFile(string Name, string Path)
        {
            _name = Name;
            _path = Path;
        }

        public string Name
        {
            get { return _name; }
        }

        public string SetName
        {
            set { _name = value; }
        }

        public string Path
        {
            get { return _path; }
        }

        public string SetPath
        {
            set { _path = value; }
        }
    }
}
