using System.Windows.Media;

namespace TransparentNotePad
{
    [System.Serializable]
    public struct ThemeColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public ThemeColor(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }
        public SolidColorBrush ToBrush()
        {
            return new SolidColorBrush(ToColor());
        }
    }

    [System.Serializable]
    public struct VariableThemeColor
    {
        public VariableThemeColor(ThemeColor[] variants)
        {
            Variants = variants;
        }

        public ThemeColor[] Variants { get; set; }
    }
}
