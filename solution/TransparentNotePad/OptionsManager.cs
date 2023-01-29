using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using TransparentNotePad.SaveSystem;
using Xceed.Wpf.AvalonDock.Themes;

namespace TransparentNotePad
{
    public static class OptionsManager
    {
        private static OptionFile? _currentOptionFile;

        public static OptionFile CurrentOptionFile
        {
            get
            {
                if (_currentOptionFile == null)
                {
                    LoadOptions(SaveManager.GetOptionFileFromFile());
                }

                return _currentOptionFile!.Value;
            }
            set
            {
                LoadOptions(value);
            }
        }

        public static void LoadOptions(OptionFile optionFile, bool openLastText = false)
        {
            var fontFamilyConverter = new FontFamilyConverter();
            var fontfamily = (FontFamily)fontFamilyConverter.ConvertFromString(optionFile.EditorFont)!;
            var instanceOfMainWindow = Manager.InstanceOfMainWindow;

            fontfamily ??= (FontFamily)fontFamilyConverter.ConvertFromString(Theme.DEFAULT_FONT_FAMILY)!;

            //load theme
            if (!ThemeManager.LoadThemeByName(optionFile.SelectedTheme))
            {
                optionFile.SelectedTheme = OptionFile.GetDefault().SelectedTheme;
                _ = ThemeManager.LoadThemeByName(optionFile.SelectedTheme);
            }

            instanceOfMainWindow.tbox_mainText.FontFamily =  fontfamily;
            instanceOfMainWindow.tbox_mainText.FontSize = optionFile.EditorZoom;

            if (openLastText && optionFile.LastTextFileSaved != "none")
            {
                _ = Manager.TryOpenTextFromFile(optionFile.LastTextFileSaved);
            }

            _currentOptionFile = optionFile;
        }
        public static bool SaveOptions(OptionFile optionFile)
        {
            _currentOptionFile = optionFile;
            return SaveManager.SaveOptionFile(optionFile);
        }

        public static bool SetFileSaveEmplacement(string path)
        {
            var option_file = CurrentOptionFile;
            option_file.FileSavePath = path;
            return SaveManager.SaveOptionFile(option_file);
        }
        public static bool SetSelectedTheme(Theme theme)
        {
            var option_file = CurrentOptionFile;
            option_file.SelectedTheme = theme.ThemeName;
            return SaveManager.SaveOptionFile(option_file);
        }
        public static bool SetDefaultFont(string fontName)
        {
            var option_file = CurrentOptionFile;
            option_file.EditorFont = fontName;
            option_file.NoteWin_Default_Font = fontName;
            return SaveManager.SaveOptionFile(option_file);
        }
        public static bool SetDefaultZoom(double zoomValue)
        {
            var option_file = CurrentOptionFile;
            option_file.EditorZoom = zoomValue;
            return SaveManager.SaveOptionFile(option_file);
        }
    }
}
