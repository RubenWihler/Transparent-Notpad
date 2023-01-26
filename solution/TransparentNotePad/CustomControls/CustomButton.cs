using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Effects;

namespace TransparentNotePad.CustomControls
{
    [NameScopeProperty("buttonGroupType", typeof(ButtonGroupType))]
    public class CustomButton : Button, IThemable
    {
        public ButtonGroupType buttonGroupType { get; set; }

        public override void EndInit()
        {
            base.EndInit();
            ThemeManager.onThemeChanged += ApplyTheme;
        }

        public void ApplyTheme(Theme theme)
        {
            var buttonGroupSettings = theme.GetButtonGroupThemeSettingsOf(buttonGroupType);
            var contentForgroundBrush = buttonGroupSettings.ContentColor.ToBrush();

            this.Background = buttonGroupSettings.BackgroundColor.ToBrush();
            this.BorderBrush = buttonGroupSettings.BorderColor.ToBrush();
            this.BorderThickness = new Thickness(buttonGroupSettings.BorderThickness);
            this.Effect.SetValue(DropShadowEffect.ColorProperty, buttonGroupSettings.GlowColor);
            this.Effect.SetValue(DropShadowEffect.OpacityProperty, buttonGroupSettings.GlowOpacity);

            for (int i = 0; i < this.VisualChildrenCount; i++)
            {
                this.GetVisualChild(i).SetValue(ForegroundProperty, contentForgroundBrush);
            }
        }
    }
}
