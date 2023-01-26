using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TransparentNotePad
{
    public enum ThemeColorType
    {
        Primary, Secondary, Tertiary,
        Validate, Warning, Alert,
        TopBarBackground, PanelBackground, EditorBackground,
        PanelText, PanelButtonText, PanelController
    }

    public enum ButtonGroupType
    {
        Panel, DesktopModePanel, 
        TopBarButtonClose, TopBarButtonMinimise, TopBarButtonMaximize,
        TopBarButtonHidePanel, TopBarButtonTop,
    }

    [System.Serializable]
    public struct ButtonGroupThemeSettings
    {
        public ThemeColor BackgroundColor { get; set; }
        public ThemeColor HoverColor { get; set; }
        public ThemeColor BorderColor { get; set; }
        public ThemeColor ContentColor { get; set; }
        public ThemeColor GlowColor { get; set; }

        public int BorderThickness { get; set; }
        public int GlowOpacity { get; set; }
    }

    [System.Serializable]
    public struct Theme
    {
        public const string DEFAULT_FONT_FAMILY = "Poppins";

        public ThemeColor PrimaryColor { get; set; }
        public ThemeColor SecondaryColor { get; set;}
        public ThemeColor TertiaryColor { get; set; }

        /// <summary>
        /// For Validate/ok color [DEFAULT GREEN]
        /// </summary>
        public ThemeColor ValidateColor { get; set; }
        /// <summary>
        /// For Middly important color [DEFAULT ORANGE]
        /// </summary>
        public ThemeColor WarningColor { get; set; }
        /// <summary>
        /// For Alert/important/cancel color [DEFAULT RED]
        /// </summary>
        public ThemeColor AlertColor { get; set; }

        public ThemeColor TopBarBackgroundColor { get; set; }
        public ThemeColor PanelBackgroundColor { get; set; }
        public ThemeColor EditorBackgroundColor { get; set; }

        public ThemeColor PanelTextColor { get; set; }
        public ThemeColor PanelButtonTextColor { get; set; }
        public ThemeColor PanelControllerColor { get; set; }

        public ButtonGroupThemeSettings PanelButtons { get; set; }
        public ButtonGroupThemeSettings DesktopModePanelButtons { get; set; }

        public ButtonGroupThemeSettings TopBarButtonClose { get; set; }
        public ButtonGroupThemeSettings TopBarButtonMinimise { get; set; }
        public ButtonGroupThemeSettings TopBarButtonMaximise { get; set; }
        public ButtonGroupThemeSettings TopBarButtonHidePanel { get; set; }
        public ButtonGroupThemeSettings TopBarButtonTop { get; set; }

        public string GlobalTextFont { get; set; }
        public ThemeColor GlobalTextColor { get; set; }
    }

    public static class ThemeExtension
    {
        public static ThemeColor GetThemeColorOf(this Theme theme, ThemeColorType colorType)
        {
            switch (colorType)
            {
                case ThemeColorType.Primary:
                    return theme.PrimaryColor;

                case ThemeColorType.Secondary:
                    return theme.SecondaryColor;

                case ThemeColorType.Tertiary:
                    return theme.TertiaryColor;

                case ThemeColorType.Validate:
                    return theme.ValidateColor;

                case ThemeColorType.Warning:
                    return theme.WarningColor;

                case ThemeColorType.Alert:
                    return theme.AlertColor;

                case ThemeColorType.TopBarBackground:
                    return theme.TopBarBackgroundColor;

                case ThemeColorType.PanelBackground:
                    return theme.PanelBackgroundColor;

                case ThemeColorType.EditorBackground:
                    return theme.EditorBackgroundColor;

                case ThemeColorType.PanelText:
                    return theme.PanelTextColor;

                case ThemeColorType.PanelButtonText:
                    return theme.PanelButtonTextColor;

                case ThemeColorType.PanelController:
                    return theme.PanelControllerColor;

                default:
                    return theme.PrimaryColor;
            }
        }
        public static Color GetColorOf(this Theme theme, ThemeColorType colorType)
        {
            return theme.GetThemeColorOf(colorType).ToColor();
        }
        public static SolidColorBrush GetBrushColorOf(this Theme theme, ThemeColorType colorType)
        {
            return theme.GetThemeColorOf(colorType).ToBrush();
        }

        public static ButtonGroupThemeSettings GetButtonGroupThemeSettingsOf(this Theme theme, ButtonGroupType buttonGroupType)
        {
            switch (buttonGroupType)
            {
                case ButtonGroupType.Panel:
                    return theme.PanelButtons;

                case ButtonGroupType.DesktopModePanel:
                    return theme.DesktopModePanelButtons;

                case ButtonGroupType.TopBarButtonClose:
                    return theme.TopBarButtonClose;

                case ButtonGroupType.TopBarButtonMinimise:
                    return theme.TopBarButtonMinimise;

                case ButtonGroupType.TopBarButtonMaximize:
                    return theme.TopBarButtonMaximise;

                case ButtonGroupType.TopBarButtonHidePanel:
                    return theme.TopBarButtonHidePanel;

                case ButtonGroupType.TopBarButtonTop:
                    return theme.TopBarButtonTop;

                default:
                    return theme.PanelButtons;
            }
        }

        public static FontFamily GetFontFamily(this Theme theme)
        {
            var fontFamilyConverter = new FontFamilyConverter();
            var fontfamily = (FontFamily?)fontFamilyConverter.ConvertFromString(theme.GlobalTextFont);

            if (fontfamily == null)
            {
                return (FontFamily)fontFamilyConverter.ConvertFromString(Theme.DEFAULT_FONT_FAMILY)!;
            }
                
            return fontfamily;
        }
    }
}
