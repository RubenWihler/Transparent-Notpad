using System;

namespace TransparentNotePad
{
    /// <summary>
    /// Data structure that represent application settings that store various things like: selected theme, editor font, default save directory etc
    /// </summary>
    [Serializable]
    public struct OptionFile
    {
        public OptionFile(
            string lastText_Saved,
            string selected_Theme,
            string font,
            double text_zoom,
            string lastSaveDirectory,
            string temporaryTextFilePath,
            bool tntxtFileInited,
            int noteWin_Default_WindowOpacity,
            string noteWin_Default_Font)
        {
            this.LastTextFileSaved = lastText_Saved;
            this.SelectedTheme = selected_Theme;
            this.EditorFont = font;
            this.EditorZoom = text_zoom;
            this.FileSavePath = lastSaveDirectory;
            this.TemporaryTextFilePath = temporaryTextFilePath;
            this.TntxtFileInitialized = tntxtFileInited;
            this.NoteWin_Default_WindowOpacity = noteWin_Default_WindowOpacity;
            this.NoteWin_Default_Font = noteWin_Default_Font;
        }
        /// <summary>
        /// The path of the last none-saved text in appdata file
        /// </summary>
        public string LastTextFileSaved { get; set; }
        /// <summary>
        /// The selected font
        /// </summary>
        public string SelectedTheme { get ; set; }
        /// <summary>
        /// The editor font
        /// </summary>
        public string EditorFont { get; set; }
        /// <summary>
        /// the editor zoom value
        /// </summary>
        public double EditorZoom { get; set; }

        /// <summary>
        /// The default path when saving a file
        /// </summary>
        public string FileSavePath { get; set; }
        /// <summary>
        /// Path that is used for store temporary text file by <see cref="SaveSystem.SaveManager"/>
        /// </summary>
        public string TemporaryTextFilePath { get; set; }
        /// <summary>
        /// If the icon text file as been initialized
        /// </summary>
        public bool TntxtFileInitialized { get; set; }

        /// <summary>
        /// The default editor font of note window
        /// </summary>
        public string NoteWin_Default_Font { get; set; }
        /// <summary>
        /// The default opacity of note window
        /// </summary>
        public int NoteWin_Default_WindowOpacity { get; set; }

        /// <summary>
        /// Get the Default OptionFile
        /// </summary>
        /// <returns></returns>
        public static OptionFile GetDefault()
        {
            return new OptionFile()
            {
                LastTextFileSaved = "none",
                SelectedTheme = "Dark-Orange",
                EditorFont = "poppins",
                EditorZoom = 24,
                FileSavePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                TemporaryTextFilePath = SaveSystem.SaveManager.DefaultTemporaryTextDirectoryPath,
                TntxtFileInitialized = false,
                NoteWin_Default_WindowOpacity = 200,
                NoteWin_Default_Font = "Cascadia Code"
            };
        }
    }
}
