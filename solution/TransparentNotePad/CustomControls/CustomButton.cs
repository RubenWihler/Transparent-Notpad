using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace TransparentNotePad.CustomControls
{
    [NameScopeProperty("buttonGroupType", typeof(ButtonGroupType))]
    public class CustomButton : Button, IThemable
    {
        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register("HoverBackground",
                typeof(Brush),
                typeof(CustomButton),
                new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));

        public ButtonGroupType buttonGroupType { get; set; }
        public Brush HoverBackground
        {
            get { return (Brush)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }

        private ButtonGroupThemeSettings buttonGroupSettings;

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
            buttonGroupSettings = theme.GetButtonGroupThemeSettingsOf(buttonGroupType);
            var contentForgroundBrush = buttonGroupSettings.ContentColor.ToBrush();
            
            this.Background = buttonGroupSettings.BackgroundColor.ToBrush();
            this.BorderBrush = buttonGroupSettings.BorderColor.ToBrush();
            this.BorderThickness = new Thickness(buttonGroupSettings.BorderThickness);
            this.HoverBackground = buttonGroupSettings.HoverColor.ToBrush();
            this.Effect = new DropShadowEffect();
            this.Effect.SetValue(DropShadowEffect.ColorProperty, buttonGroupSettings.GlowColor.ToColor());
            this.Effect.SetValue(DropShadowEffect.OpacityProperty, Convert.ToDouble(buttonGroupSettings.GlowOpacity));
            this.Effect.SetValue(DropShadowEffect.DirectionProperty, Convert.ToDouble(-90));
            this.Effect.SetValue(DropShadowEffect.BlurRadiusProperty, Convert.ToDouble(15));

            this.Foreground = buttonGroupSettings.ContentColor.ToBrush();

            for (int i = 0; i < this.VisualChildrenCount; i++)
            {
                this.GetVisualChild(i).SetValue(ForegroundProperty, contentForgroundBrush);
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.Background = buttonGroupSettings.HoverColor.ToBrush();
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.Background = buttonGroupSettings.BackgroundColor.ToBrush();
        }
    }
}
