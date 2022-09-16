using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransparentNotePad
{
    [System.Serializable]
    public struct StoredDataFile
    {
        public StoredDataFile(
            string lastText_Saved,
            string selected_Theme,
            string font,
            double text_zoom,
            string lastSaveDirectory,
            string tntxtFileInited)
        {
            LastText_Saved = lastText_Saved;
            Selected_Theme = selected_Theme;
            Font = font;
            LastTextZoom = text_zoom;
            LastSaveDirectory = lastSaveDirectory;
            TntxtFileInited = tntxtFileInited;
        }

        public string LastText_Saved { get; set; }
        public string Selected_Theme { get ; set; }
        public string Font { get; set; }
        public double LastTextZoom { get; set; }

        public string LastSaveDirectory { get; set; }
        public string TntxtFileInited { get; set; }
    }
}
