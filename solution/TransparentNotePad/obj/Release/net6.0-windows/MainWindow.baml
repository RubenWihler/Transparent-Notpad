   M S B A M L   `   `   `  ���� M  ITransparentNotePad, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:TransparentNotePadTransparentNotePad  U/clr-namespace:TransparentNotePad.CustomControls!TransparentNotePad.CustomControls  R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel #    TransparentNotePad.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:TransparentNotePad     Jcustomcontrols/clr-namespace:TransparentNotePad.CustomControls     Hscm8clr-namespace:System.ComponentModel;assembly=WindowsBase     ] YXceed.Wpf.Toolkit.NET5, Version=4.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  -     � AllowsTransparency$
  True�� � WindowStyle$
 None=� � Icon$&  ./Resources/Icon/icon_orange.png�� � Title$ Transparent Notpad��$	��448��$	��800�����	#00000000 � WindowStartupLocation$ CenterScreen=�$	��385��$	��485��$��False�� J� 	AllowDrop$
 True�� � 
ResizeMode$ CanResizeWithGrip=� � 	Resources    familyCollection �� Source   System.Windows.Media.Fonts	   SystemFontFamilies
 �� Source �� SortDescriptions+  %System.ComponentModel.SortDescription   PropertyName   	Direction  UpRightButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value �� 
TargetName  PanelButtonTextViewBox  PanelTextViewBox  PanelButtonText  PanelTextBlock7   1TransparentNotePad.CustomControls.CustomTextBlock  PanelButtonTextIcon"  FontAwesome.WPF.ImageAwesome  PanelButton �� RelativeSource �� Path  Panel_BrushButton 	 
Panel_Grid � RowDefinitions � ColumnDefinitions 
 Panel_ComboBox  Rounded Button  Circle Button  
DMP_button  DMP_buttonIcon  	DMP_Label  
DMP_Slider  DMP_ColorPicker#  Xceed.Wpf.Toolkit.ColorPicker��%�  &	        &	 ;     &	 W    &	 �    &	 !    &	 �    &	 �    &	 o    &	 r    &		 �    &	
 	    &	 4    &	 9	    &	  
    &	 ?    &	 �    &	 q    &	 �    ��  �� #
 Z	    $ Source��$ 	Ascending=��� " ��.���  �@�OverridesDefaultStyle$
 True����  � ��$ 5q���  � �� �� " ��.R��� $��border��$ 5��$��0q�#��z�����#404040._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ��� $ border�� � �� �#040404�� " (�.���  � ��$ 50����  � ��$ 30����  � ��$ 10����  � ��$ 10���� " (�.���  � ��$ 80����  � ��$ 50����  � ��$ 25����  � ��$ 25���� " ��.���  �@�Tag$ btn_panel_text����  � ��$ Poppins)���  � ��$ 11(��� "  .���  �@�Tag$ btn_panel_text����  � ��$ Poppins)���  � ��$ 11(��� "  .���  �@�Tag$ btn_panel_draw_brushIcons����  �@ 
Foreground �	#FF484848��  � ��$ 15,15,15,15q��� " ��.���  �@�Tag$ 	btn_panel����  �@�OverridesDefaultStyle$
 True����  � ��$ 5,7,5,7q���  � �� �	#FF484848��  � �� �	#FFE6E6E6��  � �� �	#FF484848��  � ��$	 200����  � ��$ 50����  � ��$ 61����  � ��$ 23����  � �� �� " ��.R��� $��border��$��2q�$ 4��#��z��#��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  ��  �� *Self+$ HoverBackground� � ���� " ��.���  �@�Tag$ 	btn_panel����  �@�OverridesDefaultStyle$
 True����  � ��$ 	31,7,31,7q���  � �� �	#FF484848��  � �� �	#FFE6E6E6��  � �� �	#FF484848��  � ��$ 50����  � ��$ 50����  � ��$ 12����  � ��$ 15����  �@�Cursor$
 Hand����  � �� �� " ��.R��� $��border��$��1q�$ 4��#��z��#��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  ��  �� *Self+$ HoverBackground� � ���� " ��.���  � �� �� .R��  �� �� �� �� �� �� �� �� �� �� ��  �� �� �� " ��.���  � ��$ 
5,10,5,9.7q��� " ��.���  �@�Cursor$
 Hand����  � �� �� " ��.R��� $��border��$��0q�$ 4��#��z��#��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  ��  �� *Self+$ HoverBackground� � ���� " ��.���  � �� �� " ��.R��� $��border��$��0q�$ 50��#��z��#��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  ��  �� *Self+$ HoverBackground� � ���� " ��.���  �@�Tag$ DMP_btn����  �@�OverridesDefaultStyle$
 True����  � ��$ 7,7,7,7q���  � �� �	#FF484848��  � �� �	#FFE6E6E6��  � �� �	#FF484848��  � ��$ 50����  � ��$ 50����  � ��$ 12����  � ��$ 15����  �@�Cursor$
 Hand����  � �� �� " ��.R��� $��border��$��1q�$ 4��#��z��#��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  ��  �� *Self+$ HoverBackground� � ���� "  .���  �@�Tag$ btn_panel_draw_brushIcons����  �@ 
Foreground �	#FF484848��  � ��$ 15,15,15,15q���  �@J�IsHitTestVisible$ False���� " ��.���  �@�Tag$ 	DMP_label����  � �� �	#FFE6E6E6��  � ��$ 2,2,2,2q���  � ��$ Poppins)��� " ��.���  �@�Tag$ 
DMP_slider����  � ��$ 5,0,5,0q���  � ��$ Center=��� "  .���  �@�Tag$ DMP_color_Picker����  � ��$ 5,0,5,0q���  � ��$ Center=�.��4   .TransparentNotePad.CustomControls.CustomBorder  -     Name$ brd_main��$ 15,15,15,15��$��0q� �� Padding$ 4q����	#4CFFFFFF$
}�True��$
 True��   themeColorType$ EditorBackground=�._�� 
������  J� Opacity$	 255Q�$��-4,-4,-4,-4q�$
}�True��.+�� $
}�True��$
 True����Z� .+�� ���	#00000000$��2��$��2��$
 True��.+��� ���	#00000000$ 	0,0,30,30��$
 True��._�$   TransparentNotePad.PaintCanvas  -     Name$ 
paint_area��$��0,33,0,0q����	#00000000$
 True���� -     ��Name$  tbox_mainText��$��10,58,10,10q�! �� TextWrapping$
! Wrap=�$$��Welcome to Transparent-Notepad�����	#00000000���	#00000000" �� SelectionBrush" �	#FF6BB8FF$��24(�$��0,0,0,0q�$��False��# �� 
AcceptsTab$
# True��$ �� AcceptsReturn$
$ True��$ False��% �� SelectionTextBrush% Z�   -   $ display_panel��4   .TransparentNotePad.CustomControls.CustomBorder&   	MouseDown$& display_panel_MouseDown��'   Drop$' On_DropOnWindow��$	 4.5��
���   �$��0q�$��611,157,0,129q����	#FFA4A4A4$��41��( � Cursor$
( Hand��$
|�True��) K� ZIndex$) 1��$��1��$
��Left=�$��14��$
 True��$ PanelController=�* J� Effect* 3	  -System.Windows.Media.Effects.DropShadowEffect	  + 	  
BlurRadius$+ 10Q�, 	  Opacity$	, 0.4Q�- 	  	Direction$- -89.8Q�� -   . �Name$. panel�����	#00000000$	��150��$
 True��$��	618,0,0,0q� �� $
��273* ��� $
��176* �.+�  -   $ panel_border��$ 	0,15,15,0��
���   �$��0q����	#FFBFBFBF$ PanelBackground=�$��2���� -   / ��Name$/ Panel_Items_textMode��#��[	 $��
10,32,10,0q�$��2��.@�4
   .TransparentNotePad.CustomControls.CustomButton
  -	   0 
 Name$%0 btn_panel_text_openAsNoteWindow��4   .TransparentNotePad.CustomControls.CustomButton1   Click$1 btn_note_Click_1��2 
  buttonGroupType$2 Panel=�$��0��$��0��$��Option��#���  $
( Hand��3 � ToolTip$3 Create new Note [Ctr+N]������ .R�(� #��[ .��  4   themeColorType$4 PanelButtonText=�#��[ 
New Note
  -
   $0 btn_panel_text_option��$1 btn_option_Click_1��$2 Panel=�$��1��$��0��$��Option��#���  $
( Hand��$3 Open option window������ .R�(� #��[ .��  $4 PanelButtonText=�#��[ Option
  -   $0 btn_panel_text_switchMode��$1 btn_switchMode_Click��$2 Panel=�$��0��$��1��$��Paint��#���  $
( Hand��$3 Set to Paint mode������ .R�(� #��[ .��  $4 PanelButtonText=�#��[ Paint
  -   $ 0 btn_panel_text_DesktopMode��$1 btn_desktopMode_Click��$2 Panel=�$��1��$��1��$��Option��#���  $
( Hand��$3 Set to Desktop mode������ .R�(� #��[ .��  $4 PanelButtonText=�#��[ 	Desktop(� $��0��$��3��#��[ .��  -   5  Name$5 tblc_text_font��$4 	PanelText=�#��[ 
Text Font:6   0TransparentNotePad.CustomControls.CustomComboBox  -   6  Name$ 6 cmbbox_Panels_FontSelector��6   0TransparentNotePad.CustomControls.CustomComboBox7   SelectionChanged$17 +cmbbox_Panels_FontSelector_SelectionChanged��8   themeColorType$8 	PanelText=�#��� 
 $��1��$��3��$��Center=����� #
 [  (� $��0��$��4��#��[ .��  -   $5 tblc_text_color��$4 	PanelText=�#��[ Text Color:9   3TransparentNotePad.CustomControls.CustomColorPicker  -   9  Name$9 colpicker_text_color��9   3TransparentNotePad.CustomControls.CustomColorPicker:   SelectedColorChanged$: OnTextColorChanged��;   colorPickerType$; Panel=�$��Center=�$��1��$��4��
  -   $0 btn_panel_text_save��$1 btn_TextSave_Click��$2 Panel=�$��0��$��8��$
��Save��#���  $
( Hand��$'3 !Save the text into a file [Ctr+S]������ .R�(� #��[ .��  $4 PanelButtonText=�#��[ Save
  -   $0 btn_panel_text_saveas��$1 btn_TextSaveAs_Click��$2 Panel=�$��0��$��9��$
��Save��#���  $
( Hand��$13 +Save the text into a new file [Ctr+Shift+S]������ .R�(� #��[ .��  $4 PanelButtonText=�#��[ 	Save As
  -   $'0 !btn_panel_text_openInFileExplorer��$1 btn_ViewInExplorerClick��$2 Panel=�$��1��$��9��$
��Save��#���  $
( Hand��$.3 (Reveal the current file in file explorer������ .R�(� #��[ .��  $4 PanelButtonText=�#��[ 
Explorer
  -   $ 0 btn_panel_text_OpenTextDoc��$1 btn_OpenTextDoc_Click��$2 Panel=�$��1��$��8��$
��Save��#���  $
( Hand��$3 Open a text file������ .R�(� #��[ .��  $4 PanelButtonText=�#��[ Open�� -   $/ Panel_Items_drawMode��#��[	 $��
10,32,10,0q�$��2��.@�
  -   $%0 btn_panel_draw_openAsNoteWindow��$1 btn_note_Click_1��$2 Panel=�$��0��$��0��$��Option��#���  $
( Hand��$3 Create new Note [Ctr+N]������ .R�(� #��[ .��  $4 PanelButtonText=�#��[ 
New Note
  -   $0 btn_panel_draw_option��$1 btn_option_Click_1��$2 Panel=�$��1��$��0��$��Option��#��[ $
( Hand��$3 Open option window������ .R�(� #��[ .��  $4 PanelButtonText=�#��[ Option
  -   $0 btn_panel_draw_switchMode��$1 btn_switchMode_Click��$2 Panel=�$��0��$��1��$��Paint��#���  $
( Hand��$3 Set to Text mode������ .R�(� #��[ .��  $4 PanelButtonText=�#��[ Text
  -   $ 0 btn_panel_draw_DesktopMode��$1 btn_desktopMode_Click��$2 Panel=�$��1��$��1��$��Option��#���  $
( Hand��$3 Set to Desktop mode������ .R�(� #��[ .��  $4 PanelButtonText=�#��[ 	Desktop
  -   $ 0 btn_panel_draw_NormalBrush��$1 OnPenButtonClick��$2 Panel=�$��0��$��3��$��Option��#��[ $
( Hand��$3 Draw Tool [D]������ .R�(� #��[ .��  <   Icon$< Pencil=�#��[ 
  -   $0 btn_panel_draw_EraseBrush��$1 OnEraserBtn_Click��$2 Panel=�$��1��$��3��$��Option��#��[ $
( Hand��$3 Eraser Tool [E]������ .R�(� #��[ .��  $< Eraser=�#��[ (� $��0��$��4��#��[ .��  -   $5 tblc_brush_size��$4 	PanelText=�#��[ Brush Size:�� -   = ��Name$= slider_brush_size��$��Center=�> � Maximum$	> 100Q�? � Minimum$? 1Q�$��1��$��4��(� $��0��$��5��#��[ .��  -   $5 tblc_eraser_size��$4 	PanelText=�#��[ Eraser Size:�� -   $= slider_eraserSize��$��Center=�$	> 100Q�$? 1Q�$��1��$��5��@ � Value$@ 28Q�(� $��0��$��6��#��[ .��  -    $5 tblc_brush_color��$4 	PanelText=�#��[ Brush Color:  -!   $9 cpick_brushColorPicker��$&:  ColorPicker_SelectedColorChanged��$; Panel=�$��Center=�$��1��$��6��
  -"   $0 btn_panel_draw_Clear��$1 On_BtnClear_Click��$2 Panel=�$��1��$��7��$��Option��#��[ ���	#FFFF7B5E$
( Hand��$3 Clear all elements������ .R�(� #��[ .��  #��[ $4 PanelButtonText=����	#FF313131Clear  -#   $ dm_PaintCanvas�����	#01FFFFFFA J� IsHitTestVisible$A False��$
 True��.+�4   .TransparentNotePad.CustomControls.MovablePanel  -$   B  Name$B brd_DesktopModePanel��4   .TransparentNotePad.CustomControls.MovablePanelC   
MouseEnter$%C brd_DesktopModePanel_MouseEnter��D   
MouseLeave$%D brd_DesktopModePanel_MouseLeave��$  DesktopModePanelBackground=����	#FF444444$��95��$	��384��$ 10��$
��Left=�$��Center=�$
A True��$( Arrow��$) 11��._�� .+�
  -%   $0 DMP_btn_Extend��$1 DMP_btn_Extend_Click��$2 DesktopModePanel=�$��0,180,-12,180q�$��Right=�$��24��#��[ .��(� .��2   ,TransparentNotePad.CustomControls.CustomIcon  -&   E  Name$E DMP_icon_Extend��$< ArrowCircleRight=�#��[ F   themeColorType$F DesktopModePanelText=�� $��	8,25,8,25q� �� $��5* ��� $	��41* ��� $��7* � �� -'   G ��Name$G DMP_gridDef_Base��$	��32* ��� -(   $G DMP_gridDef_Extend��$��0 �.+�=   7TransparentNotePad.CustomControls.DesktopModeToolButton  -)   H  Name$H DMP_btn_PointerTool��=   7TransparentNotePad.CustomControls.DesktopModeToolButtonI   Click$I DMP_btn_Tools_Click��#��[ $��25��$��70��$��0��$��0��$3 desktop cursor��J   Tool$J Cursor=�$��Center=�$��Center=�$��0,0,0,0q�$��70��$��25��.��(� .��  -*   $E DM_ToolIcon_Cursor��$< MousePointer=�#��[ $F DesktopModePanelText=�� $��1��$��0��$��79��$��79�� �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* � �� �� .+�  -+   $H DMP_btn_DrawTool��$I DMP_btn_Tools_Click��#��[ $��0��$��0��$3 	draw tool��$	J Pen=�$��25��$��25��$��Center=�$��Center=�.��(� .��  -,   $E DM_ToolIcon_Pencil��$< Pencil=�#��[ $F DesktopModePanelText=�  --   $H DMP_btn_EraseTool��$I DMP_btn_Tools_Click��#��[ $��0��$��1��$J Eraser=�$��25��$��25��$��Center=�$��Center=�.��(� .��  -.   $E DM_ToolIcon_Eraser��$< Eraser=�#��[ $F DesktopModePanelText=�  -/   $H DMP_btn_ImageBoxTool��$I DMP_btn_Tools_Click��#��[ $��1��$��0��$3 image box tool��$J ImageBox=�$��25��$��25��$��Center=�$��Center=�.��(� .��  -0   $E DM_ToolIcon_ImageBox��$< Image=�#��[ $F DesktopModePanelText=�  -1   $H DMP_btn_TextTool��$I DMP_btn_Tools_Click��#��[ $��1��$��1��$3 text box tool��$
J Text=�$��25��$��25��$��Center=�$��Center=�.��(� .��  -2   $E DM_ToolIcon_Text��$
< Font=�#��[ $F DesktopModePanelText=�  -3   $H DMP_btn_RectBorderTool��$I DMP_btn_Tools_Click��#��[ $��2��$��0��$3 rectangle tool��$J 
RectBorder=�$��25��$��25��$��Center=�$��Center=�.��(� .��  -4   $E DM_ToolIcon_RectBorder��$< SquareOutline=�#��[ $F DesktopModePanelText=�  -5   $H DMP_btn_RectFillTool��$I DMP_btn_Tools_Click��#��[ $��2��$��1��$3 filled rectangle tool��$J RectFill=�$��25��$��25��$��Center=�$��Center=�.��(� .��  -6   $E DM_ToolIcon_RectFill��$< Square=�#��[ $F DesktopModePanelText=�  -7   $H DMP_btn_CircleBorderTool��$I DMP_btn_Tools_Click��#��[ $��3��$��0��$3 circle tool��$J CircleBorder=�$��25��$��25��$��Center=�$��Center=�.��(� .��  -8   $E DM_ToolIcon_CircleBorder��$< CircleOutline=�#��[ $F DesktopModePanelText=�  -9   $H DMP_btn_CircleFillTool��$I DMP_btn_Tools_Click��#��[ $��3��$��1��$3 filled circle tool��$J 
CircleFill=�$��25��$��25��$��Center=�$��Center=�.��(� .��  -:   $E DM_ToolIcon_CircleFill��$< Circle=�#��[ $F DesktopModePanelText=�  -;   $H DMP_btn_ArrowTool��$I DMP_btn_Tools_Click��#��[ $��4��$��1��$3 
arrow tool��$J Arrow=�$��25��$��25��$��Center=�$��Center=�.��(� .��  -<   $E DM_ToolIcon_Arrow��$< 
ArrowRight=�#��[ $F DesktopModePanelText=�  -=   $H DMP_btn_LineTool��$I DMP_btn_Tools_Click��#��[ $��4��$��0��$3 	line tool��$
J Line=�$��25��$��25��$��Center=�$��Center=�.��(� .��  ->   $E DM_ToolIcon_Line��$< Minus=�#��[ $F DesktopModePanelText=�
  -?   $0 DMP_btn_Clear��$1 DMP_btn_Clear_Click��#��[ $��6��$��1��$3 Clear all elements��$2 DesktopModePanel=�$��25��$��25��$��Center=�$��Center=�.��(� .��  -@   $E DM_ToolIcon_Clear��$< Trash=�#��[ $F DesktopModePanelText=�
  $1 DMP_Close_Click��$��2��$��0��$��25��$��70�����	#FFED4D4D#��[ $3 exit desktop mode��$2 DesktopModePanelQuit=�$��Center=�$��Center=�.��(� $��5q�.��  $< Close=�K   
ForegroundK �	#ff333333� -A   $. DMP_grid_Extended��$��1��$��1��$ 0Q�$A False�� �� �� �� �� �� �� ��  �� �� .+�(� $��0��$��0��.��3   -TransparentNotePad.CustomControls.CustomLabel  -B   L  Name$L DM_lbl_brushSize��M   themeColorType$M DesktopModePanelText=�#��[ $��brush size:���� -C   $= DMP_slider_brushSize��$��0��$��1��#��[ $? 1Q�$	> 100Q�$@ 10Q�(� $��1��$��0��.��  -D   $L DM_lbl_eraserSize��$M DesktopModePanelText=�#��[ $��eraser size:���� -E   $= DMP_slider_eraserSize��$��1��$��1��#��[ $? 1Q�$	> 200Q�$@ 50Q�(� $��2��$��0��.��  -F   $L DM_lbl_Color��$M DesktopModePanelText=�#��[ $��color:��  -G   $9 DMP_colorPicker��$*: $DMP_colorPicker_SelectedColorChanged��$; DesktopModePanel=�$��2��$��1��#��[   -H   $ upBar��N   MouseLeftButtonDown$N 
DragWindow�����	#FFDADADA$��34��$	��Top=�$ 	15,15,0,0��$ TopBarBackground=�._�� -I   $. 
upBar_grid��$��0,1,0,1q�.+��� -J   $= slider_winOpacity��$
��Left=�$��13,0,0,0q�$��Center=�$	��176��$
 True�����	#FFD05B5B
���   �$@ 5Q�$	> 255Q�$3 window's opacity����Z� 
  -K   $0 btn_quit��$1 btn_quit_Click��$2 TopBarButtonClose=�$��0,5,10,5q����	#FFED4D4D���	#FF272727$
( Hand��$��X��$��18(�$��	Tw Cen MT)�$
��Bold"����	#FF171717#��[ $��Right=�$��22��$3 Close�� ��%:   '��       �� " ��.���  �@��CornerRadius$ 20��
  -L   $0 btn_minimise��$1 btn_minimise_Click��$2 TopBarButtonMinimise=�$��0,5,37,5q����	#FFEDA75F���	#FF272727$
( Hand��$	��—��$��	Tw Cen MT)����	#FF171717$
��Bold"�$��14(�#��[ $��Right=�$��22��$3 Minimise�� ��%:   '��       �� " ��.���  �@��CornerRadius$ 20��
  -M   $0 btn_fullscreen��$1 btn_fullscreen_Click��$2 TopBarButtonMaximize=�$��0,5,64,5q����	#FF63ED5F���	#FF272727$
( Hand��$��+��$��	Tw Cen MT)����	#FF171717$��18(�$
��Bold"�$��Right=�$��22��$3 	Maximise �� ��%9   '��       �� " ��.���  �@��CornerRadius$ 5������ O �� ResourceKey$O UpRightButtonStyle��
  -N   $0 btn_top��$1 btn_top_Click��$2 TopBarButtonTop=�$��0,5,98,5q����	#FF5F61ED���	#FF272727$
( Hand��$��Top  ✓��$��	Tw Cen MT)����	#FF171717$
��Bold"�$��14(�$��Right=�$��72��$93 3Put the window above the other applications [Ctr+T]�� ��%9   '��       �� " ��.���  �@��CornerRadius$ 6������ $O UpRightButtonStyle��
  -O   $0 btn_DisplayPanel��$1 btn_DisplayPanel_Click��$2 TopBarButtonHidePanel=�$��	0,5,180,5q����	#FFF94AAA���	#FF272727$
( Hand��$��
Hide Panel��$��	Tw Cen MT)����	#FF171717$
��Bold"�$��14(�$��Right=�$��89��$&3  Hide the right side panel[Ctr+P]�� ��%9   '��       �� " ��.���  �@��CornerRadius$ 6������ $O UpRightButtonStyle��  