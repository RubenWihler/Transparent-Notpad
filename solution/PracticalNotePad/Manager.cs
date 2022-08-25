using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows;
using System.IO;
using System.Windows.Documents;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Media;

namespace PracticalNotePad
{
    public class Manager
    {
        private static Manager instance;

        private OptionWindow optionWindow;
        private MainWindow mainWindow;

        private Theme currentTheme;


        #region /*---------- Proprety ----------*/

        public static Manager Instance
        {
            get
            {
                return instance;
            }
        }

        public static OptionWindow OptionWin
        {
            get
            {
                return Instance.optionWindow;
            }
        }
        public static MainWindow MainWindow
        {
            get
            {
                return Instance.mainWindow;
            }
        }
        public static bool isOpened_OptionWin
        {
            get
            {
                if (TryGetWindowIsActive<OptionWindow>(Instance.optionWindow, out bool result)) return result;
                return false;
            }
        }

        private static string AppDataPath
        {
            get
            {
                string path = Path.Combine(Environment
                    .GetFolderPath(Environment.SpecialFolder.ApplicationData), "Transparent-Notepad");

                return path;
            }
        }
        private static string ThemesPath
        {
            get
            {
                return Path.Combine(AppDataPath, "Themes");
            }
        }
        private static string StoredDataFilePath
        {
            get
            {
                return Path.Combine(AppDataPath, "StoredData.xml");
            }
        }

        #endregion

        public Manager(MainWindow mainWindow)
        {
            instance = this;
            this.mainWindow = mainWindow;
            this.optionWindow = new OptionWindow();
            CheckFile();
        }

        private static void CheckFile()
        {
            if (!Directory.Exists(AppDataPath))
            {
                GenerateAllAppFile();
                return;
            }
            
            if (!Directory.Exists(ThemesPath))
            {
                GenerateTheme();
            }

            if (!File.Exists(StoredDataFilePath))
            {
                GenerateStoredData();
            }

            //temp
            List<Theme> themes;
            if (TryGetThemeFromXMLFile(out themes))
            {
                SetTheme(themes[0]);
            }
            
        }
        private static void GenerateAllAppFile()
        {
            Directory.CreateDirectory(AppDataPath);
            GenerateStoredData();
            GenerateTheme();
        }
        private static void GenerateStoredData()
        {
            File.WriteAllText(StoredDataFilePath, "not yet imlepemented");
        }
        private static void GenerateTheme()
        {
            Directory.CreateDirectory(ThemesPath);
            GenerateDefaultTheme();
        }
        private static void GenerateDefaultTheme()
        {
            string path = Path.Combine(ThemesPath, "defaultTheme_Bright.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(Theme));

            try
            {
                Theme theme = new(
                "FF 00 00 00",
                "FF DA DA DA",
                "FF BF BF BF",
                "FF A4 A4 A4",
                "4C FF FF FF",
                "FF ED 4D 4D",
                "FF ED A7 5F",
                "FF 63 ED 5F",
                "FF 5F 61 ED",
                "00 00 00 00",
                "FF A4 A4 A4",
                "00 00 00 00",
                "00 00 00 00",
                "FF 94 94 94",
                "FF B1 B1 B1",
                "FF EA EA EA",
                "FF 77 77 77",
                "FF 58 58 58",
                "FF 35 35 35",
                "poppins");

                using (Stream writer = new FileStream(path, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(writer, theme);
                }
            }
            catch (Exception e)
            {
                #if DEBUG
                Console.WriteLine($"Error while serialize theme into xml file!\nerror: {e}");
                #endif
            }
        }

        public static bool TryOpenWindow<T>(T win) where T : Window
        {
            if (win != null)
            {
                (win as T).IsEnabled = true;
                (win as T).Activate();
                (win as T).Show();
                return true;
            }

            return false;
        }
        public static bool TryCloseWindow<T>(T win) where T : Window
        {
            if (win != null)
            {
                (win as T).Hide();
                return true;
            }

            return false;
        }

        public static bool TryGetWindowIsActive<T>(T win, out bool result) where T : Window
        {
            if (win != null)
            {
                result = (win as T).IsActive;
                return true;
            }

            result = false;
            return false;
        }


        public static void SetTheme(Theme theme)
        {
            MainWindow.TextAreaColor = GetColorFromThemeFileString(theme.Color_TextArea);

            MainWindow.tbox_mainText.Foreground = new
                SolidColorBrush(GetColorFromThemeFileString(theme.Color_DefaultText));
            MainWindow.upBar.Background = new
               SolidColorBrush(GetColorFromThemeFileString(theme.Color_UpBar));
            MainWindow.panel.Background = new
               SolidColorBrush(GetColorFromThemeFileString(theme.Color_Panel));
            MainWindow.display_panel.Background = new
               SolidColorBrush(GetColorFromThemeFileString(theme.Color_DisplayPanel));
            
            MainWindow.btn_quit.Background = new
               SolidColorBrush(GetColorFromThemeFileString(theme.Color_Btn_Quit));
            MainWindow.btn_minimise.Background = new
               SolidColorBrush(GetColorFromThemeFileString(theme.Color_Btn_Minimise));
            MainWindow.btn_fullscreen.Background = new
               SolidColorBrush(GetColorFromThemeFileString(theme.Color_Btn_Maximise));
            MainWindow.btn_top.Background = new
               SolidColorBrush(GetColorFromThemeFileString(theme.Color_Btn_Top));

            MainWindow.lbl_opacity.Foreground = new
               SolidColorBrush(GetColorFromThemeFileString(theme.Color_Text_Opacity_lbl));
        }
        private static System.Windows.Media.Color GetColorFromThemeFileString(string hex)
        {
            byte a, r, g, b;
            string[] parts = hex.Split(' ');

            if (parts.Length != 4) return System.Windows.Media.Color.FromArgb(0,0,0,0);
            
            a = Convert.ToByte(parts[0], 16);
            r = Convert.ToByte(parts[1], 16);
            g = Convert.ToByte(parts[2], 16);
            b = Convert.ToByte(parts[3], 16);

            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }

        public static bool TryGetThemeFromXMLFile(out List<Theme> themes)
        {
            themes = new List<Theme>();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Theme));
                string[] themeFile_Path = Directory.GetFiles(ThemesPath);

                for (int i = 0; i < themeFile_Path.Length; i++)
                {
                    using (Stream reader = new FileStream(themeFile_Path[i], FileMode.Open))
                    {
                        object? t = serializer.Deserialize(reader);
                        if (t != null) themes.Add((Theme)t);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                #if DEBUG
                Console.WriteLine($"Error while loading themes from xml!\nerror: {e}");
                #endif

                return false;
            }
        }

    }
}
