using System.Windows.Data;
using System.Windows.Markup;

namespace TransparentNotePad.CustomControls
{
    [NameScopeProperty("themeColorType", typeof(ThemeColorType))]
    public class CustomTextBlock : System.Windows.Controls.TextBlock, IThemable
    {
        public ThemeColorType themeColorType { get; set; }

        public override void EndInit()
        {
            base.EndInit();
            ThemeManager.onThemeChanged += ApplyTheme;
        }

        public void ApplyTheme(Theme theme)
        {
            this.FontFamily = theme.GetFontFamily();
            this.Foreground = theme.GetBrushColorOf(themeColorType);
        }
    }
}
