using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransparentNotePad
{
    [Serializable]
    public struct Theme
    {
        public Theme(
            string tname,
            string color_DefaultText,
            string color_UpBar,
            string color_Panel,
            string color_DisplayPanel,
            string color_TextArea,
            string color_Btn_Quit,
            string color_Btn_Minimise,
            string color_Btn_Maximise,
            string color_Btn_Top,
            string color_Text_Opacity_lbl,
            string color_Text_Panel_Btns_Bg,
            string color_Text_Panel_Btns_Text,
            string color_Text_Panel_Btns_Border,
            string color_General_Bright_1,
            string color_General_Bright_2,
            string color_General_Bright_3,
            string color_General_Dark_1,
            string color_General_Dark_2,
            string color_General_Dark_3,
            string color_Btn_Brush_Active,
            string color_Btn_Brush_Disable,
            string font_UI_General,
            string default_PaintColor,
            string color_Btn_ShowHidePanel)
        {
            Theme_Name = tname;
            Color_DefaultText = color_DefaultText;
            Color_UpBar = color_UpBar;
            Color_Panel = color_Panel;
            Color_DisplayPanel = color_DisplayPanel;
            Color_TextArea = color_TextArea;
            Color_Btn_Quit = color_Btn_Quit;
            Color_Btn_Minimise = color_Btn_Minimise;
            Color_Btn_Maximise = color_Btn_Maximise;
            Color_Btn_Top = color_Btn_Top;
            Color_Text_Opacity_lbl = color_Text_Opacity_lbl;
            Color_Text_Panel_Btns_Bg = color_Text_Panel_Btns_Bg;
            Color_Text_Panel_Btns_Text = color_Text_Panel_Btns_Text;
            Color_Text_Panel_Btns_Border = color_Text_Panel_Btns_Border;
            Color_General_Bright_1 = color_General_Bright_1;
            Color_General_Bright_2 = color_General_Bright_2;
            Color_General_Bright_3 = color_General_Bright_3;
            Color_General_Dark_1 = color_General_Dark_1;
            Color_General_Dark_2 = color_General_Dark_2;
            Color_General_Dark_3 = color_General_Dark_3;
            Color_Btn_Brush_Active = color_Btn_Brush_Active;
            Color_Btn_Brush_Disable = color_Btn_Brush_Disable;
            Font_UI_General = font_UI_General;
            Default_PaintColor = default_PaintColor;
            Color_Btn_ShowHidePanel = color_Btn_ShowHidePanel;
        }

        public String Theme_Name { get; set; }
        public String Color_DefaultText { get; set; }
        public String Color_UpBar { get; set; }
        public String Color_Panel { get; set; }
        public String Color_DisplayPanel { get; set; }
        public String Color_TextArea { get; set; }

        public String Color_Btn_Quit { get; set; }
        public String Color_Btn_Minimise { get; set; }
        public String Color_Btn_Maximise { get; set; }
        public String Color_Btn_Top { get; set; }
        public string Color_Btn_ShowHidePanel { get; set; }

        public String Color_Text_Opacity_lbl { get; set; }
        public String Color_Text_Panel_Btns_Bg { get; set; }
        public String Color_Text_Panel_Btns_Text { get; set; }
        public String Color_Text_Panel_Btns_Border { get; set; }

        public String Color_General_Bright_1 { get; set; }
        public String Color_General_Bright_2 { get; set; }
        public String Color_General_Bright_3 { get; set; }
        public String Color_General_Dark_1 { get; set; }
        public String Color_General_Dark_2 { get; set; }
        public String Color_General_Dark_3 { get; set; }

        public String Color_Btn_Brush_Active { get; set; }
        public String Color_Btn_Brush_Disable { get; set; }

        public string Font_UI_General { get; set; }

        public string Default_PaintColor { get; set; }
    }
}
