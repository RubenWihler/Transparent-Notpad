using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Effects;
using Xceed.Wpf.Toolkit;

namespace TransparentNotePad.CustomControls
{
    [NameScopeProperty("colorPickerType", typeof(ColorPickerType))]
    public class CustomColorPicker : ColorPicker, IThemable
    {
        public ColorPickerType colorPickerType { get; set; }

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
            var cpSettings = theme.GetColorPickerThemeSettingsOf(colorPickerType);

            this.FontFamily = theme.GetFontFamily();
            this.Background = cpSettings.BackgroundColor.ToBrush();
            this.BorderBrush = cpSettings.BorderColor.ToBrush();
            this.DropDownBackground = cpSettings.DropDownBackgroundColor.ToBrush();
            this.DropDownBorderBrush = cpSettings.DropDownBorderColor.ToBrush();
            this.DropDownBorderThickness = new System.Windows.Thickness(cpSettings.BorderThickness);
            this.HeaderBackground = cpSettings.HeaderBackgroundColor.ToBrush();
            this.HeaderForeground = cpSettings.HeaderForegroundColor.ToBrush();
            this.TabBackground = cpSettings.TabBackgroundColor.ToBrush();
            this.TabForeground = cpSettings.TabForegroundColor.ToBrush();
            this.Effect = new DropShadowEffect();
            this.Effect.SetValue(DropShadowEffect.ColorProperty, cpSettings.GlowColor.ToColor());
            this.Effect.SetValue(DropShadowEffect.OpacityProperty, Convert.ToDouble(cpSettings.GlowOpacity));
            this.SelectedColor = theme.GlobalTextColor.ToColor();
        }
    }
}
