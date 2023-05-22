using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace TransparentNotePad.SaveSystem
{
    /// <summary>
    /// Provide Static Methods and Propreties for saving or read file
    /// </summary>
    public static class SaveManager
    {
        public const string APP_DIRECTORY_NAME = "Transparent-Notepad";
        public const string TEMPORARY_DIRECTORY_NAME = "TemporaryFiles";
        public const string TEMPORARY_TEXT_DIRECTORY_NAME = "TemporaryText";
        public const string THEMES_DIRECTORY_NAME = "Themes";
        public const string OPTIONS_FILE_NAME = "options.json";
        public const string THEME_FILE_EXTENSION = ".json";

        private static bool _fileChecked;
        private static string _customTextTemporaryFilePath = string.Empty;

        #region PATHS

        public static string DefaultTemporaryTextDirectoryPath
        {
            get
            {
                return System.IO.Path.Combine(TemporaryDirectoryPath, TEMPORARY_TEXT_DIRECTORY_NAME);
            }
        }

        /// <summary>
        /// return appdata/roaming path
        /// </summary>
        private static string AppDataPath
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    APP_DIRECTORY_NAME);
            }
        }
        /// <summary>
        /// Return appdata/roaming/APP_DIRECTORY_NAME/THEMES_DIRECTORY_NAME
        /// </summary>
        private static string ThemesPath
        {
            get
            {
                return Path.Combine(AppDataPath, THEMES_DIRECTORY_NAME);
            }
        }
        /// <summary>
        /// return appdata/roaming/APP_DIRECTORY_NAME/OPTIONS_FILE_NAME
        /// </summary>
        private static string OptionsFilePath
        {
            get
            {
                return Path.Combine(AppDataPath, OPTIONS_FILE_NAME);
            }
        }

        /// <summary>
        /// return appdata/roaming/TEMPORARY_DIRECTORY_NAME
        /// </summary>
        private static string TemporaryDirectoryPath
        {
            get
            {
                return Path.Combine(AppDataPath, TEMPORARY_DIRECTORY_NAME);
            }
        }
        /// <summary>
        /// return by default appdata/roaming/TEMPORARY_DIRECTORY_NAME/TEMPORARY_TEXT_DIRECTORY_NAME.
        /// But if there is a custom path in the current <see cref="OptionFile"/> it going to use <see cref="OptionFile.TemporaryTextFilePath"/>
        /// </summary>
        public static string TemporaryTextDirectoryPath
        {
            get
            {
                if (Directory.Exists(OptionsManager.CurrentOptionFile.TemporaryTextFilePath))
                {
                    return OptionsManager.CurrentOptionFile.TemporaryTextFilePath;
                }

                return Path.Combine(TemporaryDirectoryPath, TEMPORARY_TEXT_DIRECTORY_NAME);
            }
        }

        #endregion

        public static void SetCustomTextTemporaryFilePath(string newPath)
        {
            _customTextTemporaryFilePath = newPath;
        }

        /// <summary>
        /// Check all necessary files and create it if they not exists
        /// </summary>
        public static void CheckFiles()
        {
            if (_fileChecked) return;

            if (!Directory.Exists(AppDataPath))
            {
                Directory.CreateDirectory(AppDataPath);
            }

            if (!Directory.Exists(ThemesPath))
            {
                Directory.CreateDirectory(ThemesPath);
                GenerateDefaultThemes();
            }

            if (GetAllThemeFromFiles(false).Count == 0)
            {
                GenerateDefaultThemes();
            }

            if (!Directory.Exists(TemporaryDirectoryPath))
            {
                Directory.CreateDirectory(TemporaryDirectoryPath);
            }

            if (!Directory.Exists(DefaultTemporaryTextDirectoryPath))
            {
                Directory.CreateDirectory(DefaultTemporaryTextDirectoryPath);
            }

            if (!File.Exists(OptionsFilePath))
            {
                OptionsManager.CurrentOptionFile = GenerateDefaultOptionFile();
            }

            _fileChecked = true;
        }

        /// <summary>
        /// Save a Serializable object in a file
        /// </summary>
        /// <typeparam name="T">Serializable object</typeparam>
        /// <param name="t">object</param>
        /// <param name="path">the file path</param>
        /// <returns>the result of the opperation</returns>
        public static bool TrySaveToJsonFile<T>(T t, string path, bool fileCheck = true) where T : new()
        {
            try
            {
                if (fileCheck) CheckFiles();
                var json_serialize_options = new JsonSerializerOptions();
                json_serialize_options.WriteIndented = true;
                string json = JsonSerializer.Serialize<T>(t, json_serialize_options);
                File.WriteAllText(path, json);
                return true;
            }
            catch (Exception e)
            {
                //$"Error while trying save an object of type: {t?.GetType()} in a json file with path: {path}!\r\nerror: {e}".LogError();   
            }

            return false;
        }
        /// <summary>
        /// Get an serializable object from a json file
        /// </summary>
        /// <typeparam name="T">the serializable object type</typeparam>
        /// <param name="path">the json file path to get the object from</param>
        /// <param name="t">the object</param>
        /// <returns>the result of the opperation</returns>
        public static bool TryGetJsonFile<T>(string path, out T t, bool checkFile = true) where T : new()
        {
            if (File.Exists(path))
            {
                try
                {
                    if (checkFile) CheckFiles();
                    string json = File.ReadAllText(path);
                    t = JsonSerializer.Deserialize<T>(json)!;
                    return true;
                }
                catch (Exception e)
                {
                    $"Error while trying get data-file from json!\r\nerror: {e}".LogError();
                }
            }

            t = default!;
            return false;
        }

        /// <summary>
        /// Save an text to temporary text file [<see cref="TemporaryTextDirectoryPath"/>]
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        public static void SaveTextToTemporaryFile(string text, Encoding encoding = null!)
        {
            if (text.Trim().Length < 1) return;

            encoding ??= Encoding.UTF8;

            var temporary_text_files = GetAllTemporaryTextFile();

            while (temporary_text_files.Count > 30)
            {
                temporary_text_files.RemoveAt(temporary_text_files.Count - 1);
            }

            for (int i = 0; i < temporary_text_files.Count; i++)
            {
                if (!temporary_text_files[i].Path.Contains("DEFAULTOPEN", StringComparison.CurrentCultureIgnoreCase))
                    continue;

                string old_path = temporary_text_files[i].Path;
                string file_dir = Path.GetDirectoryName(old_path)!;
                string file_name = Path.GetFileName(old_path);

                file_name = file_name.Remove(0, 11);

                string new_path = Path.Combine(file_dir, file_name);

                File.Move(old_path, new_path);
            }

            string filename = $"DEFAULTOPEN_tmp_text_file__{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}h_{DateTime.Now.Minute}m_{DateTime.Now.Second}s.tntxt";
            string path = Path.Combine(TemporaryTextDirectoryPath, filename);

            Console.WriteLine(path);
            File.WriteAllText(path, text, encoding);
        }

        /// <summary>
        /// Save a text in a file
        /// </summary>
        /// <param name="text">the text to save</param>
        /// <param name="path">the path of the file</param>
        /// <param name="encoding">the encoding [default = <see cref="Encoding.UTF8"/>]</param>
        /// <returns></returns>
        public static bool TrySaveTextFile(string text, string path, Encoding encoding = null!)
        {
            encoding ??= Encoding.UTF8;

            try
            {
                File.WriteAllText(path, text, encoding);
                return true;
            }
            catch (Exception e)
            {
                #if DEBUG
                $"error while save file at {path} \r\n error: {e}".LogError();
                #endif
            }

            return false;
        }
        /// <summary>
        /// Open a <see cref="SaveFileDialog "/> and save target text into selected file
        /// </summary>
        /// <param name="text">file content</param>
        /// <param name="dialog">the dialog</param>
        /// <param name="title">the dialog title</param>
        /// <param name="defaultExtension">the dialog file extension</param>
        /// <param name="filter">the dialog filter</param>
        /// <returns>the result of the opperation</returns>
        public static bool TrySaveTextFileAs(string text, out SaveFileDialog dialog,
            string title = "Save text to a file", string defaultExtension = "tntxt",
            string filter = "Text Files(*.txt)|*.txt|All(*.*)|*.tntxt|transparent notepad file(*.tntxt*)|*")
        {

            var file_name = text.Split('\n')[0];

            dialog = new SaveFileDialog
            {
                FileName = file_name,
                InitialDirectory = OptionsManager.CurrentOptionFile.FileSavePath,
                Title = title,

                CheckFileExists = false,
                CheckPathExists = true,

                DefaultExt = defaultExtension,
                Filter = filter,
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == true)
            {
                return TrySaveTextFile(text, dialog.FileName);
            }

            return false;
        }

        /// <summary>
        /// Return all file content from temporary text file [<see cref="TemporaryTextDirectoryPath"/>]
        /// </summary>
        /// <returns></returns>
        public static List<TextFile> GetAllTemporaryTextFile()
        {
            CheckFiles();
            var default_file_founded = false;
            var empty_files = new List<string>();
            var text_files = new List<TextFile>();
            var paths = Directory.GetFiles(TemporaryTextDirectoryPath);

            foreach (string path in paths)
            {
                //skip if the path is incorrect
                if (!path.TryPathToTextFile(out TextFile? text_file))
                {
                    continue;
                }

                //add the file to the empty_file list
                if (((TextFile)text_file!).Content.Trim().Length < 1)
                {
                    empty_files.Add(path);
                    continue;
                }

                //check if the filename contains "DEFAULTOPEN"
                //and set it a the first position of the list
                if (Path.GetFileName(path).Contains("DEFAULTOPEN") || !default_file_founded)
                {
                    text_files.Insert(0, (TextFile)text_file);
                    default_file_founded = true;
                    continue;
                }

                text_files.Add((TextFile)text_file);
            }

            //delete all empty file
            for (int i = 0; i < empty_files.Count; i++)
            {
                File.Delete(empty_files[i]);
            }

            return text_files;
        }

        /// <summary>
        /// Return all theme from appdata file [<see cref="ThemesPath"/>]
        /// </summary>
        /// <returns></returns>
        public static List<Theme> GetAllThemeFromFiles(bool checkFile = true)
        {
            if (checkFile) CheckFiles();
            var themeList = new List<Theme>();
            var paths = Directory.GetFiles(ThemesPath);

            for (int i = 0; i < paths.Length; i++)
            {
                if (TryGetJsonFile<Theme>(paths[i], out var theme, false))
                {
                    themeList.Add(theme);
                }
            }
            
            return themeList;
        }
        /// <summary>
        /// Return an <see cref="OptionFile"/> from appdata file [<see cref="OptionsFilePath"/>]
        /// </summary>
        /// <returns></returns>
        public static OptionFile GetOptionFileFromFile()
        {
            if (TryGetJsonFile<OptionFile>(OptionsFilePath, out var sdFile))
            {
                return sdFile;
            }
            else
            {
                return GenerateDefaultOptionFile();
            }
        }
        
        /// <summary>
        /// Save an <see cref="OptionFile"/> into a json file in appdata [<see cref="OptionsFilePath"/>]
        /// </summary>
        /// <param name="optionFile"></param>
        /// <returns></returns>
        public static bool SaveOptionFile(OptionFile optionFile)
        {
            return TrySaveToJsonFile(optionFile, OptionsFilePath);
        }
        /// <summary>
        /// Create a new json file with the <see cref="Theme.ThemeName"/> as name that contain a <see cref="Theme"/>, in appdata [<see cref="ThemesPath"/>]
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public static bool SaveNewThemeFile(Theme theme)
        {
            var path = Path.Combine(ThemesPath, $"{theme.ThemeName}{THEME_FILE_EXTENSION}");
            return TrySaveToJsonFile(theme, path);
        }

        /// <summary>
        /// Create a file that contain <see cref="OptionFile.GetDefault()"/> in appdata [<see cref="OptionsFilePath"/>]
        /// </summary>
        /// <returns></returns>
        private static OptionFile GenerateDefaultOptionFile()
        {
            var defaultStoredDataFile = OptionFile.GetDefault();
            _ = TrySaveToJsonFile(defaultStoredDataFile, OptionsFilePath, false);
            return defaultStoredDataFile;
        }
        /// <summary>
        /// Create files for each theme from <see cref="Theme.GetDefaultThemes()"/> into appdata [<see cref="ThemesPath"/>]
        /// </summary>
        private static void GenerateDefaultThemes()
        {
            var default_themes = Theme.GetDefaultThemes();

            foreach (Theme theme in default_themes)
            {
                var path = Path.Combine(ThemesPath, $"{theme.ThemeName}.json");
                _ = TrySaveToJsonFile(theme, path, false);
            }
        }
    }
}
