using System;

namespace TransparentNotePad
{
    class ThemeManager
    {
        private static ThemeManager _instance;
        public static event Action<Theme> onThemeChanged;

        private Theme? _currentTheme = null;


        public static ThemeManager Instance
        {
            get
            {
                return _instance;
            }
        }
        public static Theme? CurrentTheme
        {
            get
            {
                return Instance._currentTheme;
            }
        }

        public static void LoadTheme(Theme theme)
        {
            var inst = Instance;

            inst.ApplyTheme(theme);
            inst._currentTheme = theme;
        }

        private void ApplyTheme(Theme theme)
        {
            onThemeChanged?.Invoke(theme);
        }
    }
}
