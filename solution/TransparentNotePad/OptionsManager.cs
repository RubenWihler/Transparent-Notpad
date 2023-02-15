using System;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using TransparentNotePad.SaveSystem;

namespace TransparentNotePad
{
    /// <summary>
    /// Provide Static Methods and Propreties for managing options
    /// </summary>
    public static class OptionsManager
    {
        class OptionLoadOperation
        {
            public Task<bool> Operation;
            public bool Pending { get; set; } = true;
            public bool Result { get; set; } = false;
            public bool OpenLastText { get; set; }
            public OptionFile OptionFile { get; set; }
            private CancellationTokenSource? _cancellationTokenSource;

            public OptionLoadOperation(){}

            public void Execute()
            {
                this.Pending = true;
                this.Result = false;
                _cancellationTokenSource = new CancellationTokenSource();
                var task = Task.Run(() => Operation = ExecuteTask(_cancellationTokenSource.Token));

                Console.WriteLine($"New Options Loading Operation Exetuting -> {this}");
                Console.WriteLine($"Cancellation Token : {_cancellationTokenSource.Token}");
                Console.WriteLine($"Task id : {task.Id}");
            }
            public void Cancel()
            {
                Console.WriteLine($"Option Loading Operation Cancelled -> {this}");
                _cancellationTokenSource?.Cancel();
            }

            private async Task<bool> ExecuteTask(CancellationToken cancellationToken)
            {
                while (Pending)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Result = false;
                        Pending = false;
                        break;
                    }

                    await Task.Delay(100);

                    if (TryExecute())
                    {
                        Result = true;
                        Pending = false;
                        break;
                    }
                }

                return Result;
            }
            private bool TryExecute()
            {
                if (Manager.Instance == null || Manager.InstanceOfMainWindow == null) return false;
                var instanceOfMainWindow = Manager.InstanceOfMainWindow;

                var option_file = this.OptionFile;
                var fontFamilyConverter = new FontFamilyConverter();
                var fontfamily = (FontFamily)fontFamilyConverter.ConvertFromString(option_file.EditorFont)!;

                fontfamily ??= (FontFamily)fontFamilyConverter.ConvertFromString(Theme.DEFAULT_FONT_FAMILY)!;

                //load theme
                if (!ThemeManager.LoadThemeByName(this.OptionFile.SelectedTheme))
                {
                    option_file.SelectedTheme = OptionFile.GetDefault().SelectedTheme;
                    if (ThemeManager.LoadThemeByName(this.OptionFile.SelectedTheme)) return false;
                }

                instanceOfMainWindow.tbox_mainText.FontFamily = fontfamily;
                instanceOfMainWindow.tbox_mainText.FontSize = option_file.EditorZoom;

                if (OpenLastText && option_file.LastTextFileSaved != "none")
                {
                    _ = Manager.TryOpenTextFromFile(option_file.LastTextFileSaved);
                }

                return true;
            }
        }

        private static OptionFile? _currentOptionFile;
        private static OptionLoadOperation? _currentOperation;

        /// <summary>
        /// The current application option
        /// </summary>
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

        /// <summary>
        /// Apply option and set it as current
        /// </summary>
        /// <param name="optionFile"></param>
        /// <param name="openLastText">open the <see cref="OptionFile.LastTextFileSaved"/> in the main window</param>
        public static void LoadOptions(OptionFile optionFile, bool openLastText = false)
        {
            _currentOperation?.Cancel();
            _currentOperation = new OptionLoadOperation()
            {
                OptionFile = optionFile,
                OpenLastText = openLastText
            };

            _currentOperation.Execute();

            _currentOptionFile = optionFile;
        }
        /// <summary>
        /// Save the option in OptionFile and set it as current
        /// </summary>
        /// <param name="optionFile"></param>
        /// <returns></returns>
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
