using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace TransparentNotePad.SaveSystem
{
    public static class SaveManager
    {
        public const string APP_DIRECTORY_NAME = "Transparent-Notepad";
        public const string TEMPORARY_DIRECTORY_NAME = "TemporaryFiles";
        public const string TEMPORARY_TEXT_DIRECTORY_NAME = "TemporaryText";
        public const string THEMES_DIRECTORY_NAME = "Themes";
        public const string OPTIONS_FILE_NAME = "options.json";
        public const string THEME_FILE_EXTENSION = ".json";

        #region PATHS

        private static string AppDataPath
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    APP_DIRECTORY_NAME);
            }
        }
        private static string ThemesPath
        {
            get
            {
                return Path.Combine(AppDataPath, THEMES_DIRECTORY_NAME);
            }
        }
        private static string OptionsFilePath
        {
            get
            {
                return Path.Combine(AppDataPath, OPTIONS_FILE_NAME);
            }
        }

        private static string TemporaryDirectoryPath
        {
            get
            {
                return Path.Combine(AppDataPath, TEMPORARY_DIRECTORY_NAME);
            }
        }
        private static string TemporaryTextDirectoryPath
        {
            get
            {
                return System.IO.Path.Combine(TemporaryDirectoryPath, TEMPORARY_TEXT_DIRECTORY_NAME);
            }
        }

        #endregion

        public static void CheckFiles()
        {
            if (!Directory.Exists(AppDataPath))
            {
                Directory.CreateDirectory(AppDataPath);
            }

            if (!Directory.Exists(ThemesPath))
            {
                Directory.CreateDirectory(ThemesPath);
                GenerateDefaultThemes();
            }

            if (!Directory.Exists(TemporaryDirectoryPath))
            {
                Directory.CreateDirectory(TemporaryDirectoryPath);
            }

            if (!Directory.Exists(TemporaryTextDirectoryPath))
            {
                Directory.CreateDirectory(TemporaryTextDirectoryPath);
            }

            if (!File.Exists(OptionsFilePath))
            {
                OptionsManager.CurrentOptionFile = GenerateDefaultOptionFile();
            }
        }

        public static bool TrySaveToJsonFile<T>(T t, string path) where T : new()
        {
            try
            {
                var json_serialize_options = new JsonSerializerOptions();
                json_serialize_options.WriteIndented = true;
                string json = JsonSerializer.Serialize<T>(t, json_serialize_options);
                File.WriteAllText(path, json);
                return true;
            }
            catch (Exception e)
            {
                $"Error while trying save an object of type: {t?.GetType()} in a json file with path: {path}!\r\nerror: {e}".LogError();   
            }

            return false;
        }
        public static bool TryGetJsonFile<T>(string path, out T t) where T : new()
        {
            if (File.Exists(path))
            {
                try
                {
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

            File.WriteAllText(path, text, encoding);
        }

        public static List<TextFile> GetAllTemporaryTextFile()
        {
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

        public static List<Theme> GetAllThemeFromFiles()
        {
            CheckFiles();
            var themeList = new List<Theme>();
            var paths = Directory.GetFiles(ThemesPath);

            for (int i = 0; i < paths.Length; i++)
            {
                if (TryGetJsonFile<Theme>(paths[i], out var theme))
                {
                    themeList.Add(theme);
                }
            }
            
            return themeList;
        }
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

        public static bool SaveOptionFile(OptionFile optionFile)
        {
            return TrySaveToJsonFile(optionFile, OptionsFilePath);
        }
        public static bool SaveNewThemeFile(Theme theme)
        {
            var path = Path.Combine(ThemesPath, $"{theme.ThemeName}{THEME_FILE_EXTENSION}");
            return TrySaveToJsonFile(theme, path);
        }

        private static OptionFile GenerateDefaultOptionFile()
        {
            var defaultStoredDataFile = OptionFile.GetDefault();
            _ = TrySaveToJsonFile(defaultStoredDataFile, OptionsFilePath);
            return defaultStoredDataFile;
        }
        private static void GenerateDefaultThemes()
        {
            var default_themes = Theme.GetDefaultThemes();

            foreach (Theme theme in default_themes)
            {
                var path = Path.Combine(ThemesPath, $"{theme.ThemeName}.json");
                _ = TrySaveToJsonFile(theme, path);
            }
        }
    }
}
