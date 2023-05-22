using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace TransparentNotePad.CustomControls
{
    [NameScopeProperty("themeColorType", typeof(ThemeColorType))]
    public class CustomTextBox : TextBox, IThemable
    {
        public ThemeColorType themeColorType { get; set; }

        public override void EndInit()
        {
            base.EndInit();
            //this._defaultBorderBrush = this.BorderBrush;
            ThemeManager.onThemeChanged += ApplyTheme;
            this.Unloaded += (object sender, RoutedEventArgs e) =>
            {
                ThemeManager.onThemeChanged -= this.ApplyTheme;
            };
        }

        public void ApplyTheme(Theme theme)
        {
            this.FontFamily = theme.GetFontFamily();
            this.Foreground = theme.GetBrushColorOf(themeColorType);
            this.Background = theme.PanelButtons.BackgroundColor.ToBrush();
        }
    }
}
