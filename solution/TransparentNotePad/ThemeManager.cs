using System;
using System.Collections.Generic;
using TransparentNotePad.SaveSystem;

namespace TransparentNotePad
{
    /// <summary>
    /// Provide Static Methods and Propreties for managing themes
    /// </summary>
    public static class ThemeManager
    {
        public static event Action<Theme> onThemeChanged;

        private static Theme? _currentTheme = null;
        private static List<Theme> __lastFoundedThemesFiles = null!;
        private static Dictionary<string, Theme> __dicNameThemeOfLastFoundedThemesFiles = null!;

        /// <summary>
        /// Get the current application theme
        /// </summary>
        public static Theme CurrentTheme
        {
            get
            {
                if (_currentTheme == null)
                {
                    var theme_name = SaveManager.GetOptionFileFromFile().SelectedTheme;
                    _currentTheme = DicThemeName[theme_name];
                }
                return _currentTheme!.Value;
            }
        }
        /// <summary>
        /// Get all loaded theme
        /// </summary>
        public static List<Theme> AllThemesFiles
        {
            get
            {
                if (__lastFoundedThemesFiles == null)
                {
                    UpdateThemeFileList();
                }
                 
                return __lastFoundedThemesFiles;
            }
        }
        /// <summary>
        /// Get a Dictionnary with all loaded theme and they name as key
        /// </summary>
        public static Dictionary<string, Theme> DicThemeName
        {
            get
            {
                if (__dicNameThemeOfLastFoundedThemesFiles == null)
                {
                    UpdateThemeFileList();
                }

                return __dicNameThemeOfLastFoundedThemesFiles!;
            }
        }

        /// <summary>
        /// Apply the theme and set it as current
        /// </summary>
        /// <param name="theme"></param>
        public static void LoadTheme(Theme theme)
        {
            ApplyTheme(theme);
            _currentTheme = theme;
        }
        /// <summary>
        /// Try find a loaded theme by it's name and Load it by calling <see cref="LoadTheme(Theme)"/>
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns>theme as been founded</returns>
        public static bool LoadThemeByName(string themeName)
        {
            if (DicThemeName.TryGetValue(themeName, out var theme))
            {
                LoadTheme(theme);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Save a <see cref="Theme"/> as selected in Option
        /// </summary>
        /// <param name="theme">the targeted theme</param>
        /// <param name="loadTheme">does the theme have to load</param>
        /// <returns>result of the operation</returns>
        public static bool SetSelectedTheme(Theme theme, bool loadTheme)
        {
            if (OptionsManager.SetSelectedTheme(theme))
            {
                if (loadTheme)
                {
                    LoadTheme(theme);
                }

                return true;
            }

            return false;
        }
        /// <summary>
        /// Create a new Theme and save it into a files
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public static bool CreateThemeFile(Theme theme)
        {
            if (SaveManager.SaveNewThemeFile(theme))
            {
                UpdateThemeFileList();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Update <see cref="AllThemesFiles"/> and <see cref="DicThemeName"/> by searching in theme files
        /// </summary>
        public static void UpdateThemeFileList()
        {
            var all_theme_file = SaveManager.GetAllThemeFromFiles();
            var dic_theme_name = new Dictionary<string, Theme>();

            foreach (Theme theme in all_theme_file)
            {
                dic_theme_name.Add(theme.ThemeName, theme);
            }

            __lastFoundedThemesFiles = all_theme_file;
            __dicNameThemeOfLastFoundedThemesFiles = dic_theme_name;
        }

        /// <summary>
        /// Call <see cref="onThemeChanged"/> event
        /// </summary>
        /// <param name="theme"></param>
        private static void ApplyTheme(Theme theme)
        {
            onThemeChanged?.Invoke(theme);
        }
    }
}
