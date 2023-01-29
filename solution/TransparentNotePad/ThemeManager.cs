using System;
using System.Collections.Generic;
using TransparentNotePad.SaveSystem;

namespace TransparentNotePad
{
    public static class ThemeManager
    {
        public static event Action<Theme> onThemeChanged;

        private static Theme? _currentTheme = null;
        private static List<Theme> __lastFoundedThemesFiles = null!;
        private static Dictionary<string, Theme> __dicNameThemeOfLastFoundedThemesFiles = null!;

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

        public static void LoadTheme(Theme theme)
        {
            ApplyTheme(theme);
            _currentTheme = theme;
        }
        public static bool LoadThemeByName(string themeName)
        {
            if (DicThemeName.TryGetValue(themeName, out var theme))
            {
                LoadTheme(theme);
                return true;
            }

            return false;
        }
        
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
        public static bool CreateThemeFile(Theme theme)
        {
            if (SaveManager.SaveNewThemeFile(theme))
            {
                UpdateThemeFileList();
                return true;
            }

            return false;
        }

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

        private static void ApplyTheme(Theme theme)
        {
            onThemeChanged?.Invoke(theme);
        }
    }
}
