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
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Threading;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Win32;
using System.Data;
using System.Security.Permissions;

namespace TransparentNotePad
{
    public class Manager
    {
        private static Manager instance;
        private static CancellationTokenSource? zoomTimerToken;

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

        public static Theme CurrentTheme
        {
            get
            {
                return Instance.currentTheme;
            }
            set
            {
                SetTheme(value);
            }
        }
        public static string CurrentThemeName
        {
            get
            {
                return Instance.currentTheme.Theme_Name;
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
                string path = System.IO.Path.Combine(Environment
                    .GetFolderPath(Environment.SpecialFolder.ApplicationData), "Transparent-Notepad");

                return path;
            }
        }
        private static string ThemesPath
        {
            get
            {
                return System.IO.Path.Combine(AppDataPath, "Themes");
            }
        }

        private static string StoredDataFilePath
        {
            get
            {
                return System.IO.Path.Combine(AppDataPath, "StoredData.json");
            }
        }
        private static StoredDataFile StoredDataFile
        {
            get
            {
                if (TryGetStoredFile(out StoredDataFile result)) return result;
                else return GenerateStoredDataFile();
            }
        }
        private static string LastSavedTextFile
        {
            get
            {
                return StoredDataFile.LastText_Saved;
            }
        }
        public static string LastTextFileSaveDirectory
        {
            get
            {
                string path = StoredDataFile.LastSaveDirectory;

                if (!Directory.Exists(path))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                return path;
            }
        }

        private static string TempDirectoryPath
        {
            get
            {
                return System.IO.Path.Combine(AppDataPath, "TempFiles");
            }
        }
        private static string TempFilePath
        {
            get
            {
                return System.IO.Path.Combine(TempDirectoryPath, "Text_Files");
            }
        }

        

        #endregion

        public Manager(MainWindow mainWindow)
        {
            instance = this;
            this.mainWindow = mainWindow;
            CheckFile();
            this.optionWindow = new OptionWindow();
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

            if (!Directory.Exists(TempFilePath))
            {
                GenerateTempFiles();
            }

            if (!File.Exists(StoredDataFilePath))
            {
                GenerateStoredDataFile();
            }

            Init_Theme();
            TryOpenLastTextUsed();
        }
        private static void GenerateAllAppFile()
        {
            Directory.CreateDirectory(AppDataPath);
            GenerateStoredDataFile();
            GenerateTheme();
        }
        private static void GenerateTheme()
        {
            Directory.CreateDirectory(ThemesPath);
            GenerateDefaultTheme();
        }
        private static void GenerateDefaultTheme()
        {
            string path_bright = System.IO.Path.Combine(ThemesPath, "defaultTheme_Bright.xml");
            string path_dark = System.IO.Path.Combine(ThemesPath, "defaultTheme_Dark.xml");
            string path_classic = System.IO.Path.Combine(ThemesPath, "classic_tn.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(Theme));

            try
            {
                Theme theme_bright = new(
                "default-bright",
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
                "FF 63 ED 5F",
                "FF 5B 5B 5B",
                "poppins");

                Theme theme_dark = new(
                "default-dark",
                "FF dd dd dd",
                "FF 50 50 50",
                "FF 45 45 45",
                "FF 5e 5e 5e",
                "4C 40 40 40",
                "FF ED 4D 4D",
                "FF ED A7 5F",
                "FF 63 ED 5F",
                "FF dd dd dd",
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
                "FF 63 ED 5F",
                "FF BF BF BF",
                "poppins");

                Theme classic_tn = new(
                "Classic Transparent Notepad",
                "FF dd dd dd",
                "FF 50 50 50",
                "FF 38 38 38",
                "FF 50 50 50",
                "4C 40 40 40",
                "FF ED 4D 4D",
                "FF ED A7 5F",
                "FF fd 7e 44",
                "FF dd dd dd",
                "00 00 00 00",
                "FF 53 53 53",
                "FF fd 7e 44",
                "00 00 00 00",
                "FF 94 94 94",
                "FF B1 B1 B1",
                "FF EA EA EA",
                "FF 77 77 77",
                "FF 58 58 58",
                "FF 35 35 35",
                "FF fd 7e 44",
                "FF BF BF BF",
                "poppins");

                using (Stream writer = new FileStream(path_bright, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(writer, theme_bright);
                }
                using (Stream writer = new FileStream(path_dark, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(writer, theme_dark);
                }
                using (Stream writer = new FileStream(path_classic, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(writer, classic_tn);
                }
            }
            catch (Exception e)
            {
                #if DEBUG
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error while serialize theme into xml file!\nerror: {e}");
                Console.ResetColor();
                #endif
            }
        }
        private static void GenerateTempFiles()
        {
            Directory.CreateDirectory(TempFilePath);
        }
        private static StoredDataFile GenerateStoredDataFile()
        {
            StoredDataFile generated_file = new(
                "none",
                "Bright",
                "poppins",
                24,
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            string json = JsonSerializer.Serialize(generated_file);
            File.WriteAllText(StoredDataFilePath, json);

            return generated_file;
        }
        private static void Init_Theme()
        {
            List<Theme> themes;
            if (TryGetThemeFromXMLFile(out themes))
            {
                var v = from Theme t in themes
                        where t.Theme_Name == StoredDataFile.Selected_Theme
                        select t;

                Console.WriteLine(v.FirstOrDefault().Theme_Name);

                if (v.Any())
                {
                    SetTheme(v.FirstOrDefault());
                    Instance.currentTheme = v.FirstOrDefault();
                }
                else
                {
                    SetTheme(themes[0]);
                    Instance.currentTheme = themes[0];
                }
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


        public static void SetTheme(Theme theme, bool save = false)
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
            MainWindow.panel_border.Background = new
               SolidColorBrush(GetColorFromThemeFileString(theme.Color_Panel));

            MainWindow.PanelColor = GetColorFromThemeFileString(theme.Color_Panel);

            Brush text_color_brush = new
               SolidColorBrush(GetColorFromThemeFileString(theme.Color_Text_Panel_Btns_Text));

            MainWindow.tblc_text_font.Foreground = text_color_brush;
            MainWindow.tblc_brush_size.Foreground = text_color_brush;
            MainWindow.tblc_text_color.Foreground = text_color_brush;
            MainWindow.tblc_brush_color.Foreground = text_color_brush;

            Style button_style = new Style();
            button_style.TargetType = typeof(Button);
            button_style.BasedOn = MainWindow.Panel_Buttons_Style;
            button_style.Setters.Add(new Setter(
                Button.BackgroundProperty,
                new SolidColorBrush(
                    GetColorFromThemeFileString(theme.Color_Text_Panel_Btns_Bg))));
            button_style.Setters.Add(new Setter(
                Button.BorderBrushProperty,
                new SolidColorBrush(
                    GetColorFromThemeFileString(theme.Color_Text_Panel_Btns_Border))));

            Style text_style = new Style();
            text_style.TargetType = typeof(TextBlock);
            text_style.BasedOn = MainWindow.Panel_ButtonsTextBloc_Style;
            button_style.Setters.Add(new Setter(
                TextBlock.ForegroundProperty,
                new SolidColorBrush(
                    GetColorFromThemeFileString(theme.Color_Text_Panel_Btns_Text))));

            IEnumerable<Button> elements_Panel = 
                FindVisualChilds<Button>(MainWindow)
                .Where(x => x.Tag != null && x.Tag.ToString() == "btn_panel");

            foreach (var item in elements_Panel)
            {
                Console.WriteLine($"button panel: {item}");
                item.Style = button_style;
            }

            IEnumerable<TextBlock> elements_Panel_textbloc =
                FindVisualChilds<TextBlock>(MainWindow)
                .Where(x => x.Tag != null && x.Tag.ToString() == "btn_panel_text");

            foreach (var item in elements_Panel_textbloc)
            {
                item.Style = text_style;
            }

            //MainWindow.Panel_Buttons_Style.Setters.Add(
            //     new Setter(Button.BackgroundProperty,
            //        new SolidColorBrush(GetColorFromThemeFileString(theme.Color_Text_Panel_Btns_Bg))));

            if (save)
            {
                StoredDataFile file = StoredDataFile;
                file.Selected_Theme = theme.Theme_Name;
                SaveStoredData(file);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Saved {theme.Theme_Name} | path: {StoredDataFilePath}");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Loaded {theme.Theme_Name} !");
            Console.ResetColor();
        }
        public static void SetDefaultTypeFont(FontFamily font, bool save = false)
        {
            MainWindow.tbox_mainText.FontFamily = font;

            if (save)
            {
                StoredDataFile file = StoredDataFile;
                file.Font = font.Source;
                SaveStoredData(file);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Saved {font.Source} | path: {StoredDataFilePath}");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Loadeded font: {font.Source} !");
            Console.ResetColor();
        }
        public static void SetDefaultTextZoom(double zoom, bool save = false)
        {
            MainWindow.tbox_mainText.FontSize = zoom;

            if (save)
            {
                StoredDataFile file = StoredDataFile;
                file.LastTextZoom = zoom;
                SaveStoredData(file);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Saved text zoom {zoom} | path: {StoredDataFilePath}");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Loadeded text zoom: {zoom} !");
            Console.ResetColor();
        }
        public static void SetLastSaveEmplacement(string path)
        {
            StoredDataFile file = StoredDataFile;
            file.LastSaveDirectory = path;
            SaveStoredData(file);
        }

        public static IEnumerable<T> FindVisualChilds<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindVisualChilds<T>(ithChild)) yield return childOfChild;
            }
        }

        public static Brush GetBrushFromString(string hex)
        {
            return new SolidColorBrush(GetColorFromThemeFileString(hex));
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

        public static bool TryGetTempFiles_Text(out List<TextFile> text_file_content)
        {
            bool defaultFounded = false;
            List<string> empty_files = new List<string>();
            text_file_content = new List<TextFile>();
            string[] path = Directory.GetFiles(TempFilePath);
            TextFile? file;

            try
            {
                for (int i = 0; i < path.Length; i++)
                {
                    if (path[i].TryPathToTextFile(out file))
                    {
                        if (((TextFile)file).Content.Trim().Length < 1)
                        {
                            empty_files.Add(path[i]);
                        }
                        else
                        {
                            if (System.IO.Path.GetFileName(path[i]).Contains("DEFAULTOPEN")
                                || !defaultFounded)
                            {
                                text_file_content.Insert(0, (TextFile)file);
                                defaultFounded = true;
                            }
                            else
                            {
                                text_file_content.Add((TextFile)file);
                            }
                        }
                    }
                }

                for (int i = 0; i < empty_files.Count; i++)
                {
                    File.Delete(empty_files[i]);
                }

                return true;
            }
            catch (Exception e)
            {
                #if DEBUG
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error while loading text files from temp!\nerror: {e}");
                Console.ResetColor();
                #endif
                return false;
            }
        }
        public static bool TryGetTempFiles_Text_LASTUSED(out TextFile? lastTextFileUsed)
        {
            string[] path = Directory.GetFiles(TempFilePath);

            try
            {
                var value = from p in path
                            where System.IO.Path.GetFileName(p).Contains("DEFAULTOPEN")
                            select p;

                return value.FirstOrDefault().TryPathToTextFile(out lastTextFileUsed);
            }
            catch (Exception e)
            {
                #if DEBUG
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error while loading last text file from temp!\nerror: {e}");
                Console.ResetColor();
                #endif

                lastTextFileUsed = null;
                return false;
            }
        }
        public static UInt32 GetTempFiles_Text_Count()
        {
            return Convert.ToUInt32(Directory.GetFiles(TempFilePath).Length);
        }
        public static bool TryOpenLastTextUsed()
        {
            string path = LastSavedTextFile;

            if (path != "none")
            {
                if (File.Exists(path))
                {
                    MainWindow.tbox_mainText.Text = File.ReadAllText(path);
                    return true;
                }
            }

            if (TryGetTempFiles_Text_LASTUSED(out TextFile? tfile))
            {
                MainWindow.tbox_mainText.Text = ((TextFile)tfile).Content;
                return true;
            }

            return false;
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error while loading themes from xml!\nerror: {e}");
                Console.ResetColor();
                #endif

                return false;
            }
        }
        public static bool TryGetStoredFile(out StoredDataFile file)
        {
            try
            {
                string json = File.ReadAllText(StoredDataFilePath);
                file = JsonSerializer.Deserialize<StoredDataFile>(json);
                return true;
            }
            catch (Exception e)
            {
                #if DEBUG
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error while trying get data-file from xml!\nerror: {e}");
                Console.ResetColor();
                #endif

                file = new StoredDataFile();
                return false;
            }
        }

        public static void QuitApp()
        {
            SaveCurrentTextInTemp();
            Environment.Exit(0);
        }
        public static void SaveCurrentTextInTemp()
        {
            if (MainWindow.tbox_mainText.Text.Trim().Length > 0)
            {
                //if (TryGetTempFiles_Text(out List<TextFile> list))
                //{
                //    int listCount = list.Count;

                //    if (listCount > 30)
                //    {
                //        list.RemoveAt(list.Count);
                //    }

                //    for (int i = 0; i < listCount; i++)
                //    {
                //        if (list[i].Path.Contains("DEFAULTOPEN"))
                //        {
                //            File.Move(list[i].Path, list[i].Path.Remove(0,11));
                //        }
                //    }
                //}
                
                string filename = $"DEFAULTOPEN_tempTextFile__{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}.tntxt";
                string path = System.IO.Path.Combine(TempFilePath, filename);

                File.WriteAllText(path, MainWindow.tbox_mainText.Text);
            }
        }
        public static void SaveStoredData(StoredDataFile file)
        {
            string json = JsonSerializer.Serialize(file);
            File.WriteAllText(StoredDataFilePath, json);

            Console.WriteLine($"Saved File into {StoredDataFilePath}");
        }

        public static void StartZoomTimer()
        {
            if (zoomTimerToken != null)
            {
                zoomTimerToken?.Cancel();
                Console.WriteLine($"STOPED ZOOM TIMER");
            }

            StoredDataFile file = StoredDataFile;
            file.LastTextZoom = MainWindow.tbox_mainText.FontSize;

            zoomTimerToken = new CancellationTokenSource();
            Task.Run(() => ZoomTimer(zoomTimerToken.Token, file));
            Console.WriteLine($"START ZOOM TIMER");
        }
        private static async Task ZoomTimer(CancellationToken cancellationToken, StoredDataFile file_to_save)
        {
            //duration: 1000 (20 * 50);

            for (int i = 0; i < 20; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(50);
                Console.WriteLine($"TIMER VALUE: {i * 50}ms | THREAD: {Thread.CurrentThread.ManagedThreadId}");
            }

            SaveStoredData(file_to_save);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"SAVED DATA FILE INTO: {StoredDataFilePath}");
            Console.ResetColor();
        }

        public static void SetAssociationWithExtension(string Extension, string KeyName, string OpenWith, string FileDescription)
        {
            #if WINDOWS
            try
            {
                RegistryKey baseKey = Registry.ClassesRoot.CreateSubKey(Extension);
                baseKey.SetValue("", KeyName);

                RegistryKey openMethod = Registry.ClassesRoot.CreateSubKey(KeyName);
                openMethod.SetValue("", FileDescription);
                openMethod.CreateSubKey("DefaultIcon").SetValue(@"", "\"" + OpenWith + "\",0");
                
                RegistryKey shell = openMethod.CreateSubKey("Shell");
                shell.CreateSubKey("edit").CreateSubKey("command").SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                shell.CreateSubKey("open").CreateSubKey("command").SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                
                baseKey.Close();
                openMethod.Close();
                shell.Close();
            }
            catch { }
            
            #endif
        }

    }
}
