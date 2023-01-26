using System.Windows.Controls;
using System.Windows.Markup;

namespace TransparentNotePad.CustomControls
{
    [NameScopeProperty("themeColorType", typeof(ThemeColorType))]
    public class CustomBorder : Border, IThemable
    {
        public ThemeColorType themeColorType { get; set; }

        public override void EndInit()
        {
            base.EndInit();
            ThemeManager.onThemeChanged += ApplyTheme;
        }

        public void ApplyTheme(Theme theme)
        {
            this.Background = theme.GetBrushColorOf(themeColorType);
        }
    }
}
