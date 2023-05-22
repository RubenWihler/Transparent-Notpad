using System.Windows.Media;

namespace TransparentNotePad
{
    /// <summary>
    /// Represent a <see cref="ThemeColor"/> of an <see cref="Theme"/>
    /// </summary>
    public enum ThemeColorType
    {
        Primary, Secondary, Tertiary,
        Validate, Warning, Alert,
        PanelBackground, PanelText, PanelButtonText, PanelController,
        TopBarBackground,
        DesktopModePanelBackground, DesktopModePanelText,
        EditorBackground,
        NoteEditorBackground, NoteHeaderBackground, NoteHeaderText
    }
    /// <summary>
    /// Represent a <see cref="VariableThemeColor"/> of an <see cref="Theme"/>
    /// </summary>
    public enum VariableColorType
    {
        PanelBrushButtonIcon,
        DesktopModePanelToolButtonsIcon
    }
    /// <summary>
    /// Represent a <see cref="ButtonGroupThemeSettings"/> of an <see cref="Theme"/>
    /// </summary>
    public enum ButtonGroupType
    {
        Panel, 
        DesktopModePanel, DesktopModePanelQuit,
        TopBarButtonClose, TopBarButtonMinimise, TopBarButtonMaximize, TopBarButtonHidePanel, TopBarButtonTop,
        NoteHeader
    }
    /// <summary>
    /// Represent a <see cref="ColorPickerThemeSettings"/> of an <see cref="Theme"/>
    /// </summary>
    public enum ColorPickerType
    {
        Panel,
        DesktopModePanel,
        NotePanel
    }

    /// <summary>
    /// Data structure that represent theme's settings for a button
    /// </summary>
    [System.Serializable]
    public struct ButtonGroupThemeSettings
    {
        public ButtonGroupThemeSettings(ThemeColor backgroundColor, ThemeColor hoverColor, ThemeColor borderColor, ThemeColor contentColor, ThemeColor glowColor, int borderThickness, int glowOpacity)
        {
            BackgroundColor = backgroundColor;
            HoverColor = hoverColor;
            BorderColor = borderColor;
            ContentColor = contentColor;
            GlowColor = glowColor;
            BorderThickness = borderThickness;
            GlowOpacity = glowOpacity;
        }

        public ThemeColor BackgroundColor { get; set; }
        public ThemeColor HoverColor { get; set; }
        public ThemeColor BorderColor { get; set; }
        public ThemeColor ContentColor { get; set; }
        public ThemeColor GlowColor { get; set; }

        public int BorderThickness { get; set; }
        public double GlowOpacity { get; set; }
    }

    /// <summary>
    /// Data structure that represent theme's settings for a color picker
    /// </summary>
    [System.Serializable]
    public struct ColorPickerThemeSettings
    {
        public ColorPickerThemeSettings(ThemeColor backgroundColor, ThemeColor borderColor, ThemeColor dropDownBackgroundColor, ThemeColor dropDownBorderColor, ThemeColor headerBackgroundColor, ThemeColor headerForegroundColor, ThemeColor tabBackgroundColor, ThemeColor tabForegroundColor, ThemeColor glowColor, int borderThickness, int glowOpacity)
        {
            BackgroundColor = backgroundColor;
            BorderColor = borderColor;
            DropDownBackgroundColor = dropDownBackgroundColor;
            DropDownBorderColor = dropDownBorderColor;
            HeaderBackgroundColor = headerBackgroundColor;
            HeaderForegroundColor = headerForegroundColor;
            TabBackgroundColor = tabBackgroundColor;
            TabForegroundColor = tabForegroundColor;
            GlowColor = glowColor;
            BorderThickness = borderThickness;
            GlowOpacity = glowOpacity;
        }

        public ThemeColor BackgroundColor { get; set; }
        public ThemeColor BorderColor { get; set; }

        public ThemeColor DropDownBackgroundColor { get; set; }
        public ThemeColor DropDownBorderColor { get; set; }

        public ThemeColor HeaderBackgroundColor { get; set; }
        public ThemeColor HeaderForegroundColor { get; set; }

        public ThemeColor TabBackgroundColor { get; set; }
        public ThemeColor TabForegroundColor { get; set; }

        public ThemeColor GlowColor { get; set; }

        public int BorderThickness { get; set; }
        public int GlowOpacity { get; set; }
    }

    /// <summary>
    /// Data structure that represent a Theme that store graphical setting like colors, fonts etc
    /// </summary>
    [System.Serializable]
    public struct Theme
    {
        public const string DEFAULT_FONT_FAMILY = "Poppins";

        /*----------- Global -----------*/
        public string ThemeName { get; set; }
        public string GlobalTextFont { get; set; }
        public ThemeColor GlobalTextColor { get; set; }

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

        /*----------- Top bar -----------*/
        public ThemeColor TopBarBackgroundColor { get; set; }
        public ButtonGroupThemeSettings TopBarButtonClose { get; set; }
        public ButtonGroupThemeSettings TopBarButtonMinimise { get; set; }
        public ButtonGroupThemeSettings TopBarButtonMaximise { get; set; }
        public ButtonGroupThemeSettings TopBarButtonHidePanel { get; set; }
        public ButtonGroupThemeSettings TopBarButtonTop { get; set; }

        /*----------- Panel -----------*/
        public ThemeColor PanelBackgroundColor { get; set; }
        public ThemeColor PanelTextColor { get; set; }
        public ThemeColor PanelButtonTextColor { get; set; }
        public ButtonGroupThemeSettings PanelButtons { get; set; }
        public ColorPickerThemeSettings PanelColorPicker { get; set; }
        public ThemeColor PanelControllerColor { get; set; }
        public VariableThemeColor PanelBrushButtonIcon { get; set; }

        /*----------- Editor -----------*/
        public ThemeColor EditorBackgroundColor { get; set; }

        /*----------- Desktop Mode Panel -----------*/
        public ThemeColor DesktopModePanelBackgroundColor { get; set; }
        public ThemeColor DesktopModePanelTextColor { get; set; }
        public ButtonGroupThemeSettings DesktopModePanelButtons { get; set; }
        public ButtonGroupThemeSettings DesktopModePanelCloseButton { get; set; }
        public ColorPickerThemeSettings DesktopModePanelColorPicker { get; set; }
        public VariableThemeColor DesktopModePanelToolButtonIcon { get; set; }

        /*----------- Note -----------*/
        public ThemeColor NoteEditorBackgroundColor { get; set; }
        public ThemeColor NoteHeaderBackgroundColor { get; set; }
        public ThemeColor NoteHeaderTextColor { get; set; }
        public ButtonGroupThemeSettings NoteHeaderButtons { get; set; }
        public ColorPickerThemeSettings NotePanelColorPicker { get; set; }

        public Theme(string themeName, string globalTextFont, ThemeColor globalTextColor, ThemeColor primaryColor, ThemeColor secondaryColor, ThemeColor tertiaryColor, ThemeColor validateColor, ThemeColor warningColor, ThemeColor alertColor, ThemeColor topBarBackgroundColor, ButtonGroupThemeSettings topBarButtonClose, ButtonGroupThemeSettings topBarButtonMinimise, ButtonGroupThemeSettings topBarButtonMaximise, ButtonGroupThemeSettings topBarButtonHidePanel, ButtonGroupThemeSettings topBarButtonTop, ThemeColor panelBackgroundColor, ThemeColor panelTextColor, ThemeColor panelButtonTextColor, ButtonGroupThemeSettings panelButtons, ColorPickerThemeSettings panelColorPicker, ThemeColor panelControllerColor, VariableThemeColor panelBrushButtonIcon, ThemeColor editorBackgroundColor, ThemeColor desktopModePanelBackgroundColor, ThemeColor desktopModePanelTextColor, ButtonGroupThemeSettings desktopModePanelButtons, ButtonGroupThemeSettings desktopModePanelCloseButton, ColorPickerThemeSettings desktopModePanelColorPicker, VariableThemeColor desktopModePanelToolButtonIcon, ThemeColor noteEditorBackgroundColor, ThemeColor noteHeaderBackgroundColor, ThemeColor noteHeaderTextColor, ButtonGroupThemeSettings noteHeaderButtons, ColorPickerThemeSettings notePanelColorPicker)
        {
            ThemeName = themeName;
            GlobalTextFont = globalTextFont;
            GlobalTextColor = globalTextColor;
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
            TertiaryColor = tertiaryColor;
            ValidateColor = validateColor;
            WarningColor = warningColor;
            AlertColor = alertColor;
            TopBarBackgroundColor = topBarBackgroundColor;
            TopBarButtonClose = topBarButtonClose;
            TopBarButtonMinimise = topBarButtonMinimise;
            TopBarButtonMaximise = topBarButtonMaximise;
            TopBarButtonHidePanel = topBarButtonHidePanel;
            TopBarButtonTop = topBarButtonTop;
            PanelBackgroundColor = panelBackgroundColor;
            PanelTextColor = panelTextColor;
            PanelButtonTextColor = panelButtonTextColor;
            PanelButtons = panelButtons;
            PanelColorPicker = panelColorPicker;
            PanelControllerColor = panelControllerColor;
            PanelBrushButtonIcon = panelBrushButtonIcon;
            EditorBackgroundColor = editorBackgroundColor;
            DesktopModePanelBackgroundColor = desktopModePanelBackgroundColor;
            DesktopModePanelTextColor = desktopModePanelTextColor;
            DesktopModePanelButtons = desktopModePanelButtons;
            DesktopModePanelCloseButton = desktopModePanelCloseButton;
            DesktopModePanelColorPicker = desktopModePanelColorPicker;
            DesktopModePanelToolButtonIcon = desktopModePanelToolButtonIcon;
            NoteEditorBackgroundColor = noteEditorBackgroundColor;
            NoteHeaderBackgroundColor = noteHeaderBackgroundColor;
            NoteHeaderTextColor = noteHeaderTextColor;
            NoteHeaderButtons = noteHeaderButtons;
            NotePanelColorPicker = notePanelColorPicker;
        }

        public override string ToString()
        {
            return ThemeName;
        }

        public static Theme[] GetDefaultThemes()
        {
            ThemeColor tcolor_clear         = new ThemeColor(0x00, 0x00, 0x00, 0x00);
            ThemeColor tcolor_pur_white     = new ThemeColor(0xff, 0xff, 0xff, 0xFF);
            ThemeColor tcolor_white_01      = new ThemeColor(246, 246, 246, 0xFF);
            ThemeColor tcolor_white_02      = new ThemeColor(236, 236, 236, 0xFF);
            ThemeColor tcolor_white_03      = new ThemeColor(224, 224, 224, 0xFF);
            ThemeColor tcolor_white_04      = new ThemeColor(195, 195, 195, 0xFF);

            ThemeColor tcolor_light_black   = new ThemeColor(0x13, 0x13, 0x13, 0xFF);
            ThemeColor tcolor_light_grey    = new ThemeColor(0xA5, 0xA5, 0xA5, 0xFF);
            ThemeColor tcolor_grey          = new ThemeColor(0x55, 0x55, 0x55, 0xFF);
            ThemeColor tcolor_grey_02       = new ThemeColor(0x60, 0x60, 0x60, 0xFF);
            ThemeColor tcolor_grey_03       = new ThemeColor(0x65, 0x65, 0x65, 0xFF);
            ThemeColor tcolor_grey_04       = new ThemeColor(0x75, 0x75, 0x75, 0xFF);
            ThemeColor tcolor_dark_grey_01  = new ThemeColor(0x50, 0x50, 0x50, 0xFF);
            ThemeColor tcolor_dark_grey_02  = new ThemeColor(0x45, 0x45, 0x45, 0xFF);
            ThemeColor tcolor_dark_grey_03  = new ThemeColor(0x40, 0x40, 0x40, 0xFF);
            ThemeColor tcolor_dark_grey_04  = new ThemeColor(0x38, 0x38, 0x38, 0xFF);
            
            ThemeColor tcolor_mat_orange    = new ThemeColor(0xFD, 0x7E, 0x44, 0xFF);
            ThemeColor tcolor_dark_orange   = new ThemeColor(255, 154, 19, 0xFF);
            ThemeColor tcolor_mat_green     = new ThemeColor(0x75, 0xFF, 0x56, 0xFF);
            ThemeColor tcolor_mat_red       = new ThemeColor(0xfd, 0x44, 0x44, 0xFF);
            ThemeColor tcolor_mat_blue      = new ThemeColor(0x44, 0x89, 0xFD, 0xFF);
            ThemeColor tcolor_mat_yellow    = new ThemeColor(0xFF, 0xB7, 0x1F, 0xFF);
            ThemeColor tcolor_mat_purple    = new ThemeColor(0x6D, 0x6C, 0xFF, 0xFF);
            ThemeColor tcolor_mat_pink      = new ThemeColor(0xFF, 0x55, 0xD0, 0xFF);
            ThemeColor tcolor_mat_cyan      = new ThemeColor(0x40, 0xFF, 0xBA, 0xFF);

            return new Theme[]
            {
                /*Dark-Orange*/
                new Theme()
                {
                    ThemeName = "Dark-Orange",
                    GlobalTextFont = "poppins",
                    GlobalTextColor = tcolor_mat_orange,
                    PrimaryColor = tcolor_mat_orange,
                    SecondaryColor = tcolor_mat_orange,
                    TertiaryColor = tcolor_mat_orange,
                    ValidateColor = tcolor_mat_green,
                    WarningColor = tcolor_mat_orange,
                    AlertColor = tcolor_mat_red,
                    TopBarBackgroundColor = tcolor_dark_grey_02,
                    TopBarButtonClose = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMinimise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_yellow,
                        HoverColor = new ThemeColor(0xFF, 0xD1, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMaximise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_orange,
                        HoverColor = new ThemeColor(0xFF, 0x8D, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonHidePanel = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_orange,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonTop = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_orange,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelBackgroundColor = tcolor_dark_grey_02,
                    PanelTextColor = tcolor_mat_orange,
                    PanelButtonTextColor = tcolor_mat_orange,
                    PanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_orange,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_orange,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_orange
                    },
                    PanelControllerColor = tcolor_grey,
                    PanelBrushButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_grey_02,
                            tcolor_mat_orange
                        }
                    },
                    EditorBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelTextColor = tcolor_mat_orange,
                    DesktopModePanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_dark_grey_01,
                        HoverColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_orange,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelCloseButton = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey_02,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_orange,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_orange
                    },
                    DesktopModePanelToolButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_light_grey,
                            tcolor_mat_orange
                        }
                    },
                    NoteEditorBackgroundColor = tcolor_dark_grey_04,
                    NoteHeaderBackgroundColor = tcolor_dark_grey_02,
                    NoteHeaderTextColor = tcolor_mat_orange,
                    NoteHeaderButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_orange,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    NotePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_orange,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_orange
                    },
                },
                /*Dark-Deep-Blue*/
                new Theme()
                {
                    ThemeName = "Dark-Blue",
                    GlobalTextFont = "poppins",
                    GlobalTextColor = tcolor_mat_blue,
                    PrimaryColor = tcolor_mat_blue,
                    SecondaryColor = tcolor_mat_blue,
                    TertiaryColor = tcolor_mat_blue,
                    ValidateColor = tcolor_mat_green,
                    WarningColor = tcolor_mat_orange,
                    AlertColor = tcolor_mat_red,
                    TopBarBackgroundColor = tcolor_dark_grey_02,
                    TopBarButtonClose = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMinimise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_yellow,
                        HoverColor = new ThemeColor(0xFF, 0xD1, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMaximise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_blue,
                        HoverColor = new ThemeColor(0x59, 0x97, 0xFF, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonHidePanel = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_blue,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonTop = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_blue,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelBackgroundColor = tcolor_dark_grey_02,
                    PanelTextColor = tcolor_mat_blue,
                    PanelButtonTextColor = tcolor_mat_blue,
                    PanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_blue,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_blue,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_blue
                    },
                    PanelControllerColor = tcolor_grey,
                    PanelBrushButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_grey_02,
                            tcolor_mat_blue
                        }
                    },
                    EditorBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelTextColor = tcolor_mat_blue,
                    DesktopModePanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_dark_grey_01,
                        HoverColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_blue,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelCloseButton = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey_02,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_blue,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_blue
                    },
                    DesktopModePanelToolButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_light_grey,
                            tcolor_mat_blue
                        }
                    },
                    NoteEditorBackgroundColor = tcolor_dark_grey_04,
                    NoteHeaderBackgroundColor = tcolor_dark_grey_02,
                    NoteHeaderTextColor = tcolor_mat_blue,
                    NoteHeaderButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_blue,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    NotePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_blue,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_blue
                    },
                },
                /*Dark-Purple*/
                new Theme()
                {
                    ThemeName = "Dark-Purple",
                    GlobalTextFont = "poppins",
                    GlobalTextColor = tcolor_mat_purple,
                    PrimaryColor = tcolor_mat_purple,
                    SecondaryColor = tcolor_mat_purple,
                    TertiaryColor = tcolor_mat_purple,
                    ValidateColor = tcolor_mat_green,
                    WarningColor = tcolor_mat_orange,
                    AlertColor = tcolor_mat_red,
                    TopBarBackgroundColor = tcolor_dark_grey_02,
                    TopBarButtonClose = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMinimise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_yellow,
                        HoverColor = new ThemeColor(0xFF, 0xD1, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMaximise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_purple,
                        HoverColor = new ThemeColor(0x68, 0x66, 0xFF, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonHidePanel = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_purple,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonTop = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_purple,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelBackgroundColor = tcolor_dark_grey_02,
                    PanelTextColor = tcolor_mat_purple,
                    PanelButtonTextColor = tcolor_mat_purple,
                    PanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_purple,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_purple,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_purple
                    },
                    PanelControllerColor = tcolor_grey,
                    PanelBrushButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_grey_02,
                            tcolor_mat_purple
                        }
                    },
                    EditorBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelTextColor = tcolor_mat_purple,
                    DesktopModePanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_dark_grey_01,
                        HoverColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_purple,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelCloseButton = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey_02,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_purple,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_purple
                    },
                    DesktopModePanelToolButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_light_grey,
                            tcolor_mat_purple
                        }
                    },
                    NoteEditorBackgroundColor = tcolor_dark_grey_04,
                    NoteHeaderBackgroundColor = tcolor_dark_grey_02,
                    NoteHeaderTextColor = tcolor_mat_purple,
                    NoteHeaderButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_purple,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    NotePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_purple,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_purple
                    },
                },
                /*Dark-Yellow*/
                new Theme()
                {
                    ThemeName = "Dark-Yellow",
                    GlobalTextFont = "poppins",
                    GlobalTextColor = tcolor_mat_yellow,
                    PrimaryColor = tcolor_mat_yellow,
                    SecondaryColor = tcolor_mat_yellow,
                    TertiaryColor = tcolor_mat_yellow,
                    ValidateColor = tcolor_mat_green,
                    WarningColor = tcolor_mat_orange,
                    AlertColor = tcolor_mat_red,
                    TopBarBackgroundColor = tcolor_dark_grey_02,
                    TopBarButtonClose = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMinimise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_yellow,
                        HoverColor = new ThemeColor(0xFF, 0xD1, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMaximise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_yellow,
                        HoverColor = new ThemeColor(0xFF, 0xCC, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonHidePanel = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_yellow,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonTop = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_yellow,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelBackgroundColor = tcolor_dark_grey_02,
                    PanelTextColor = tcolor_mat_yellow,
                    PanelButtonTextColor = tcolor_mat_yellow,
                    PanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_yellow,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_yellow,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_yellow
                    },
                    PanelControllerColor = tcolor_grey,
                    PanelBrushButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_grey_02,
                            tcolor_mat_yellow
                        }
                    },
                    EditorBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelTextColor = tcolor_mat_yellow,
                    DesktopModePanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_dark_grey_01,
                        HoverColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_yellow,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelCloseButton = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey_02,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_yellow,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_yellow
                    },
                    DesktopModePanelToolButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_light_grey,
                            tcolor_mat_yellow
                        }
                    },
                    NoteEditorBackgroundColor = tcolor_dark_grey_04,
                    NoteHeaderBackgroundColor = tcolor_dark_grey_02,
                    NoteHeaderTextColor = tcolor_mat_yellow,
                    NoteHeaderButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_yellow,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    NotePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_yellow,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_yellow
                    },
                },
                /*Dark-Green*/
                new Theme()
                {
                    ThemeName = "Dark-Green",
                    GlobalTextFont = "poppins",
                    GlobalTextColor = tcolor_mat_green,
                    PrimaryColor = tcolor_mat_green,
                    SecondaryColor = tcolor_mat_green,
                    TertiaryColor = tcolor_mat_green,
                    ValidateColor = tcolor_mat_green,
                    WarningColor = tcolor_mat_orange,
                    AlertColor = tcolor_mat_red,
                    TopBarBackgroundColor = tcolor_dark_grey_02,
                    TopBarButtonClose = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMinimise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_yellow,
                        HoverColor = new ThemeColor(0xFF, 0xD1, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMaximise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_green,
                        HoverColor = new ThemeColor(0x93, 0xFF, 0x7C, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonHidePanel = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_green,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonTop = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_green,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelBackgroundColor = tcolor_dark_grey_02,
                    PanelTextColor = tcolor_mat_green,
                    PanelButtonTextColor = tcolor_mat_green,
                    PanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_green,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_green,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_green
                    },
                    PanelControllerColor = tcolor_grey,
                    PanelBrushButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_grey_02,
                            tcolor_mat_green
                        }
                    },
                    EditorBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelTextColor = tcolor_mat_green,
                    DesktopModePanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_dark_grey_01,
                        HoverColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_green,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelCloseButton = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey_02,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_green,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_green
                    },
                    DesktopModePanelToolButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_light_grey,
                            tcolor_mat_green
                        }
                    },
                    NoteEditorBackgroundColor = tcolor_dark_grey_04,
                    NoteHeaderBackgroundColor = tcolor_dark_grey_02,
                    NoteHeaderTextColor = tcolor_mat_green,
                    NoteHeaderButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_green,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    NotePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_green,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_green
                    },
                },
                /*Dark-Red*/
                new Theme()
                {
                    ThemeName = "Dark-Red",
                    GlobalTextFont = "poppins",
                    GlobalTextColor = tcolor_mat_red,
                    PrimaryColor = tcolor_mat_red,
                    SecondaryColor = tcolor_mat_red,
                    TertiaryColor = tcolor_mat_red,
                    ValidateColor = tcolor_mat_green,
                    WarningColor = tcolor_mat_orange,
                    AlertColor = tcolor_mat_red,
                    TopBarBackgroundColor = tcolor_dark_grey_02,
                    TopBarButtonClose = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMinimise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_yellow,
                        HoverColor = new ThemeColor(0xFF, 0xD1, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMaximise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonHidePanel = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_red,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonTop = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_red,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelBackgroundColor = tcolor_dark_grey_02,
                    PanelTextColor = tcolor_mat_red,
                    PanelButtonTextColor = tcolor_mat_red,
                    PanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_red,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_red,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_red
                    },
                    PanelControllerColor = tcolor_grey,
                    PanelBrushButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_grey_02,
                            tcolor_mat_red
                        }
                    },
                    EditorBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelTextColor = tcolor_mat_red,
                    DesktopModePanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_dark_grey_01,
                        HoverColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_red,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelCloseButton = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey_02,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_red,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_red
                    },
                    DesktopModePanelToolButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_light_grey,
                            tcolor_mat_red
                        }
                    },
                    NoteEditorBackgroundColor = tcolor_dark_grey_04,
                    NoteHeaderBackgroundColor = tcolor_dark_grey_02,
                    NoteHeaderTextColor = tcolor_mat_red,
                    NoteHeaderButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_red,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    NotePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_red,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_red
                    },
                },
                /*Dark-Pink*/
                new Theme()
                {
                    ThemeName = "Dark-Pink",
                    GlobalTextFont = "poppins",
                    GlobalTextColor = tcolor_mat_pink,
                    PrimaryColor = tcolor_mat_pink,
                    SecondaryColor = tcolor_mat_pink,
                    TertiaryColor = tcolor_mat_pink,
                    ValidateColor = tcolor_mat_green,
                    WarningColor = tcolor_mat_orange,
                    AlertColor = tcolor_mat_red,
                    TopBarBackgroundColor = tcolor_dark_grey_02,
                    TopBarButtonClose = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMinimise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_yellow,
                        HoverColor = new ThemeColor(0xFF, 0xD1, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMaximise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_pink,
                        HoverColor = new ThemeColor(0xFF, 0x75, 0xD9, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonHidePanel = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_pink,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonTop = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_pink,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelBackgroundColor = tcolor_dark_grey_02,
                    PanelTextColor = tcolor_mat_pink,
                    PanelButtonTextColor = tcolor_mat_pink,
                    PanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_pink,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_pink,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_pink
                    },
                    PanelControllerColor = tcolor_grey,
                    PanelBrushButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_grey_02,
                            tcolor_mat_pink
                        }
                    },
                    EditorBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelTextColor = tcolor_mat_pink,
                    DesktopModePanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_dark_grey_01,
                        HoverColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_pink,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelCloseButton = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_pink,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey_02,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_pink,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_pink
                    },
                    DesktopModePanelToolButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_light_grey,
                            tcolor_mat_pink
                        }
                    },
                    NoteEditorBackgroundColor = tcolor_dark_grey_04,
                    NoteHeaderBackgroundColor = tcolor_dark_grey_02,
                    NoteHeaderTextColor = tcolor_mat_pink,
                    NoteHeaderButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_pink,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    NotePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_pink,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_pink
                    },
                },
                /*Dark-Cyan*/
                new Theme()
                {
                    ThemeName = "Dark-Cyan",
                    GlobalTextFont = "poppins",
                    GlobalTextColor = tcolor_mat_cyan,
                    PrimaryColor = tcolor_mat_cyan,
                    SecondaryColor = tcolor_mat_cyan,
                    TertiaryColor = tcolor_mat_cyan,
                    ValidateColor = tcolor_mat_green,
                    WarningColor = tcolor_mat_orange,
                    AlertColor = tcolor_mat_red,
                    TopBarBackgroundColor = tcolor_dark_grey_02,
                    TopBarButtonClose = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMinimise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_yellow,
                        HoverColor = new ThemeColor(0xFF, 0xD1, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMaximise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_cyan,
                        HoverColor = new ThemeColor(0x79, 0xFF, 0xCF, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonHidePanel = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_cyan,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonTop = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_cyan,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelBackgroundColor = tcolor_dark_grey_02,
                    PanelTextColor = tcolor_mat_cyan,
                    PanelButtonTextColor = tcolor_mat_cyan,
                    PanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_cyan,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_cyan,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_cyan
                    },
                    PanelControllerColor = tcolor_grey,
                    PanelBrushButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_grey_02,
                            tcolor_mat_cyan
                        }
                    },
                    EditorBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelBackgroundColor = tcolor_dark_grey_04,
                    DesktopModePanelTextColor = tcolor_mat_cyan,
                    DesktopModePanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_dark_grey_01,
                        HoverColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_cyan,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelCloseButton = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_cyan,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey_02,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_cyan,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_cyan
                    },
                    DesktopModePanelToolButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_light_grey,
                            tcolor_mat_cyan
                        }
                    },
                    NoteEditorBackgroundColor = tcolor_dark_grey_04,
                    NoteHeaderBackgroundColor = tcolor_dark_grey_02,
                    NoteHeaderTextColor = tcolor_mat_cyan,
                    NoteHeaderButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        HoverColor = tcolor_grey_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_cyan,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    NotePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_cyan,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_cyan
                    },
                },
                
                /*Light-Blue*/
                new Theme()
                {
                    ThemeName = "Light-Blue",
                    GlobalTextFont = "poppins",
                    GlobalTextColor = tcolor_dark_grey_04,
                    PrimaryColor = tcolor_mat_blue,
                    SecondaryColor = tcolor_mat_blue,
                    TertiaryColor = tcolor_mat_blue,
                    ValidateColor = tcolor_mat_green,
                    WarningColor = tcolor_mat_orange,
                    AlertColor = tcolor_mat_red,
                    TopBarBackgroundColor = tcolor_white_03,
                    TopBarButtonClose = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_white_02,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMinimise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_blue,
                        HoverColor = new ThemeColor(0x59, 0x97, 0xFF, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_white_02,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonMaximise = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_blue,
                        HoverColor = new ThemeColor(0x59, 0x97, 0xFF, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_white_02,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonHidePanel = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_white_02,
                        HoverColor = tcolor_white_01,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_blue,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    TopBarButtonTop = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_white_02,
                        HoverColor = tcolor_white_01,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_blue,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelBackgroundColor = tcolor_white_03,
                    PanelTextColor = tcolor_mat_blue,
                    PanelButtonTextColor = tcolor_mat_blue,
                    PanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_white_02,
                        HoverColor = tcolor_white_01,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_blue,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    PanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_white_02,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_blue,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_blue
                    },
                    PanelControllerColor = tcolor_white_04,
                    PanelBrushButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_grey_02,
                            tcolor_mat_blue
                        }
                    },
                    EditorBackgroundColor = tcolor_white_04,
                    DesktopModePanelBackgroundColor = tcolor_white_03,
                    DesktopModePanelTextColor = tcolor_mat_blue,
                    DesktopModePanelButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_white_02,
                        HoverColor = tcolor_white_01,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_blue,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelCloseButton = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_mat_red,
                        HoverColor = new ThemeColor(0xFF, 0x59, 0x59, 0xFF),
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_light_black,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    DesktopModePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey_02,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_blue,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_blue
                    },
                    DesktopModePanelToolButtonIcon = new VariableThemeColor()
                    {
                        Variants = new ThemeColor[]
                        {
                            tcolor_light_grey,
                            tcolor_mat_blue
                        }
                    },
                    NoteEditorBackgroundColor = tcolor_white_04,
                    NoteHeaderBackgroundColor = tcolor_white_03,
                    NoteHeaderTextColor = tcolor_mat_blue,
                    NoteHeaderButtons = new ButtonGroupThemeSettings()
                    {
                        BackgroundColor = tcolor_white_02,
                        HoverColor = tcolor_white_01,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        ContentColor = tcolor_mat_blue,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0
                    },
                    NotePanelColorPicker = new ColorPickerThemeSettings()
                    {
                        BackgroundColor = tcolor_grey,
                        BorderColor = tcolor_clear,
                        BorderThickness = 0,
                        DropDownBackgroundColor= tcolor_grey,
                        DropDownBorderColor = tcolor_clear,
                        GlowColor = tcolor_clear,
                        GlowOpacity = 0,
                        HeaderBackgroundColor = tcolor_dark_grey_03,
                        HeaderForegroundColor = tcolor_mat_blue,
                        TabBackgroundColor = tcolor_dark_grey_03,
                        TabForegroundColor = tcolor_mat_blue
                    },
                },
            };
        }
    }

    public static class ThemeExtension
    {
        /// <summary>
        /// Get the ThemeColor of an theme, represented by an ThemeColorType
        /// </summary>
        /// <param name="theme">the targeted theme</param>
        /// <param name="colorType">the targeted ThemeColorTpe that going to define the ThemeColor to return</param>
        /// <returns></returns>
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

                case ThemeColorType.DesktopModePanelBackground:
                    return theme.DesktopModePanelBackgroundColor;

                case ThemeColorType.DesktopModePanelText:
                    return theme.DesktopModePanelTextColor;

                case ThemeColorType.NoteEditorBackground:
                    return theme.NoteEditorBackgroundColor;

                case ThemeColorType.NoteHeaderBackground:
                    return theme.NoteHeaderBackgroundColor;

                case ThemeColorType.NoteHeaderText:
                    return theme.NoteHeaderTextColor;

                default:
                    return theme.PrimaryColor;
            }
        }
        /// <summary>
        /// Same as <see cref="GetThemeColorOf(Theme, ThemeColorType)"/> but convert the ThemeColor to a color
        /// </summary>
        /// <param name="theme">the targeted theme</param>
        /// <param name="colorType">the targeted ThemeColorTpe that going to define the color to return</param>
        /// <returns></returns>
        public static Color GetColorOf(this Theme theme, ThemeColorType colorType)
        {
            return theme.GetThemeColorOf(colorType).ToColor();
        }
        /// <summary>
        /// Same as <see cref="GetThemeColorOf(Theme, ThemeColorType)"/> but convert the ThemeColor to a brush
        /// </summary>
        /// <param name="theme">the targeted theme</param>
        /// <param name="colorType">the targeted ThemeColorTpe that going to define the brush's color to return</param>
        /// <returns></returns>
        public static SolidColorBrush GetBrushColorOf(this Theme theme, ThemeColorType colorType)
        {
            return theme.GetThemeColorOf(colorType).ToBrush();
        }

        /// <summary>
        /// Get the ButtonGroupThemeSettings of an theme, represented by an ButtonGroupType
        /// </summary>
        /// <param name="theme">the targeted theme</param>
        /// <param name="buttonGroupType">the targeted ButtonGroupType that going to define the <see cref="ButtonGroupThemeSettings"/> to return</param>
        /// <returns></returns>
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

                case ButtonGroupType.DesktopModePanelQuit:
                    return theme.DesktopModePanelCloseButton;

                case ButtonGroupType.NoteHeader:
                    return theme.NoteHeaderButtons;

                default:
                    return theme.PanelButtons;
            }
        }

        /// <summary>
        /// Get the <see cref="ColorPickerThemeSettings"/> of an theme, represented by an <see cref="ColorPickerType"/>
        /// </summary>
        /// <param name="theme">the targeted theme</param>
        /// <param name="colorPickerType">the targeted <see cref="ColorPickerType"/> that going to define the <see cref="ColorPickerThemeSettings"/> to return</param>
        /// <returns></returns>
        public static ColorPickerThemeSettings GetColorPickerThemeSettingsOf(this Theme theme, ColorPickerType colorPickerType)
        {
            switch (colorPickerType)
            {
                case ColorPickerType.Panel:
                    return theme.PanelColorPicker;

                case ColorPickerType.DesktopModePanel:
                    return theme.DesktopModePanelColorPicker;

                case ColorPickerType.NotePanel:
                    return theme.NotePanelColorPicker;

                default:
                    return theme.PanelColorPicker;
            }
        }

        /// <summary>
        /// Get the UI font family of an Theme
        /// </summary>
        /// <param name="theme">the targeted theme</param>
        /// <returns></returns>
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
