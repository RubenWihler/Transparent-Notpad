using System;

namespace TransparentNotePad
{
    [System.Serializable]
    public struct OptionFile
    {
        public OptionFile(
            string lastText_Saved,
            string selected_Theme,
            string font,
            double text_zoom,
            string lastSaveDirectory,
            bool tntxtFileInited,
            int noteWin_Default_WindowOpacity,
            string noteWin_Default_Font)
        {
            LastTextFileSaved = lastText_Saved;
            SelectedTheme = selected_Theme;
            EditorFont = font;
            EditorZoom = text_zoom;
            FileSavePath = lastSaveDirectory;
            TntxtFileInitialized = tntxtFileInited;
            NoteWin_Default_WindowOpacity = noteWin_Default_WindowOpacity;
            NoteWin_Default_Font = noteWin_Default_Font;
        }

        public string LastTextFileSaved { get; set; }
        public string SelectedTheme { get ; set; }
        public string EditorFont { get; set; }
        public double EditorZoom { get; set; }

        public string FileSavePath { get; set; }
        public bool TntxtFileInitialized { get; set; }

        public string NoteWin_Default_Font { get; set; }
        public int NoteWin_Default_WindowOpacity { get; set; }

        public static OptionFile GetDefault()
        {
            return new OptionFile()
            {
                LastTextFileSaved = "none",
                SelectedTheme = "Dark-Orange",
                EditorFont = "poppins",
                EditorZoom = 24,
                FileSavePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                TntxtFileInitialized = false,
                NoteWin_Default_WindowOpacity = 255,
                NoteWin_Default_Font = "Cascadia Code"
            };
        }
    }
}
