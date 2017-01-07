using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Music_Player {
    class MusicPlayer {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwndCallback);

		[DllImport("winmm.dll")]
		public static extern long waveOutSetVolume(IntPtr hwo, out uint dwVolume);

		[DllImport("winmm.dll")]
		public static extern long waveOutGetVolume(IntPtr hwo, uint dwVolume);

        public void open(string file) {
            string command = $"open \"{file}\" type MPEGVideo alias MyMp3";
            mciSendString(command, null, 0, 0);
        }

        public void play() {
            string command = "play MyMp3";
            mciSendString(command, null, 0, 0);
        }

        public void stop() {
            string command = "stop MyMp3";
            mciSendString(command, null, 0, 0);

            command = "close MyMp3";
            mciSendString(command, null, 0, 0);
        }

        public void pause() {
            string command = "pause MyMp3";
            mciSendString(command, null, 0, 0);
        }

		public int Volume {
			get {

				uint CurrVol = 0;
				waveOutGetVolume(IntPtr.Zero, CurrVol);
				ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
				return CalcVol / (ushort.MaxValue / 100);
			}
			set {
				if (value < 0 || value > 100)
					throw new ArgumentOutOfRangeException();
				int NewVolume = ((ushort.MaxValue / 100) * value);
				uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
				Console.WriteLine(waveOutSetVolume(IntPtr.Zero, out NewVolumeAllChannels));
			}
		}
	}
}