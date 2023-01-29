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
using System.Runtime.CompilerServices;
using FontFamily = System.Windows.Media.FontFamily;
using Brush = System.Windows.Media.Brush;
using TransparentNotePad.SaveSystem;

namespace TransparentNotePad
{
    public class Manager
    {
        private static Manager _instance = null!;

        private OptionWindow _optionWindow;
        private MainWindow _mainWindow;
        private List<Postit> _postitsInstances = new List<Postit>();
        
        #region /*---------- Proprety ----------*/

        public static Manager Instance
        {
            get
            {
                return _instance;
            }
        }
        public static OptionWindow OptionWin
        {
            get
            {
                return Instance._optionWindow;
            }
        }
        public static MainWindow InstanceOfMainWindow
        {
            get
            {
                return Instance._mainWindow;
            }
        }
        public static List<Postit> NotesInstances
        {
            get
            {
                return Instance._postitsInstances;
            }
        }
        public static bool isOpened_OptionWin
        {
            get
            {
                if (TryGetWindowIsActive<OptionWindow>(Instance._optionWindow, out bool result)) return result;
                return false;
            }
        }

        #endregion

        public Manager(MainWindow mainWindow)
        {
            _instance = this;
            this._mainWindow = mainWindow;
            SaveManager.CheckFiles();
            
            OptionsManager.LoadOptions(OptionsManager.CurrentOptionFile, true);
            this._optionWindow = new OptionWindow();
        }

        #region |------------ Windows Management ------------|

        public static bool TryOpenWindow<T>(T win) where T : Window
        {
            if (win != null)
            {
                win.IsEnabled = true;
                win.Activate();
                win.Show();
                return true;
            }

            return false;
        }
        public static bool TryCloseWindow<T>(T win) where T : Window
        {
            if (win != null)
            {
                win.Hide();
                return true;
            }

            return false;
        }
        public static bool TryGetWindowIsActive<T>(T win, out bool result) where T : Window
        {
            if (win != null)
            {
                result = win.IsActive;
                return true;
            }

            result = false;
            return false;
        }

        #endregion

        #region |------------ Try Get Values From Files ------------|

        public static bool TryOpenText(string text)
        {
            try
            {
                InstanceOfMainWindow.tbox_mainText.Text = text;
                return true;
            }
            catch (Exception e)
            {
                $"Error while loading text \r\n error message : \r\n{e}".LogError();
            }

            return false;
        }
        public static bool TryOpenTextFromFile(string path)
        {
            bool error;
            string error_exception;

            if (!File.Exists(path))
            {
                error = true;
                error_exception = $"File {path} doesn't exist !";
            }
            else
            {
                try
                {
                    return TryOpenText(File.ReadAllText(path));
                }
                catch (Exception e)
                {
                    error = true;
                    error_exception = e.Message;
                }
            }

            if (error)
            {
                $"Error while loading text from file : \"{path}\" \r\n error message : \r\n{error_exception}".LogError();
            }

            return false;
        }

        #endregion

        #region |------------ Quit App and Save Methods ------------|

        public static void QuitApp()
        {
            if (InstanceOfMainWindow.FileSaved) InstanceOfMainWindow.Save();
            else SaveManager.SaveTextToTemporaryFile(InstanceOfMainWindow.tbox_mainText.Text);

            Environment.Exit(0);
        }
        
        #endregion

        #region |------------ Utils ------------|

        public static void SetAssociationWithExtension(string Extension, string KeyName, string OpenWith, string FileDescription)
        {
            #if WINDOWS

            if (!OptionsManager.CurrentOptionFile.TntxtFileInitialized)
            {
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

                    OptionFile tosave_file = OptionsManager.CurrentOptionFile;
                    tosave_file.TntxtFileInitialized = true;
                    OptionsManager.SaveOptions(tosave_file);
                }
                catch (Exception e)
                {
                    $"Error while trying to setup '.tntxt' file icon ! error : {e}".LogError();
                }
            }
            #endif
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

        #endregion
    }

    public static class ManagerExtension
    {
        public static void LogError(this object message)
        {
            #if DEBUG
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error has occurred!\nerror: {message}");
            Console.ResetColor();
            #else
            
            MessageBox.Show(message.ToString(), "An error has occurred !", MessageBoxButton.OK, MessageBoxImage.Error);

            #endif
        }
    }
}
