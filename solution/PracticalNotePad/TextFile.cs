using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalNotePad
{
    public struct TextFile
    {
        public TextFile(string path, string content)
        {
            Path = path;
            Content = content;
        }

        public string Path { get; set; }
        public string Content { get; set; }
    }

    public static class EXT_TextFile
    {
        public static TextFile PathToTextFile(this string path)
        {
            if (!File.Exists(path))
            {
                return new TextFile(path, String.Empty);
            }
            else
            {
                return new TextFile(path, File.ReadAllText(path));
            }
        }
        public static bool TryPathToTextFile(this string path, out TextFile? result)
        {
            if (!File.Exists(path))
            {
                result = null;
                return false;
            }
            else
            {
                result = new TextFile(path, File.ReadAllText(path));
                return true;
            }
        }
    }
}
