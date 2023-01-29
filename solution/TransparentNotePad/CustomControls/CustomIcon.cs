using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace TransparentNotePad.CustomControls
{
    [NameScopeProperty("themeColorType", typeof(ThemeColorType))]
    public class CustomIcon : ImageAwesome, IThemable
    {
        public ThemeColorType themeColorType { get; set; }

        public override void EndInit()
        {
            base.EndInit();
            ThemeManager.onThemeChanged += ApplyTheme;
            this.Unloaded += (object sender, RoutedEventArgs e) =>
            {
                ThemeManager.onThemeChanged -= this.ApplyTheme;
            };
        }

        public void ApplyTheme(Theme theme)
        {
            this.Foreground = theme.GetBrushColorOf(themeColorType);
        }
    }
}
