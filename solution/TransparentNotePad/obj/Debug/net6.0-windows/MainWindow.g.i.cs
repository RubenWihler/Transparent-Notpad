﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9023AFD4EC68823036155193FD8844F67BDF41DC"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using FontAwesome.WPF;
using FontAwesome.WPF.Converters;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TransparentNotePad;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Converters;
using Xceed.Wpf.Toolkit.Core;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Mag.Converters;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace TransparentNotePad {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 158 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border brd_main;
        
        #line default
        #line hidden
        
        
        #line 163 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal TransparentNotePad.PaintCanvas paint_area;
        
        #line default
        #line hidden
        
        
        #line 167 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbox_mainText;
        
        #line default
        #line hidden
        
        
        #line 170 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border display_panel;
        
        #line default
        #line hidden
        
        
        #line 175 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid panel;
        
        #line default
        #line hidden
        
        
        #line 176 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border panel_border;
        
        #line default
        #line hidden
        
        
        #line 177 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl Panel_Items_textMode;
        
        #line default
        #line hidden
        
        
        #line 178 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_panel_text_switchMode;
        
        #line default
        #line hidden
        
        
        #line 187 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_panel_text_option;
        
        #line default
        #line hidden
        
        
        #line 198 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblc_text_font;
        
        #line default
        #line hidden
        
        
        #line 200 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbbox_Panels_FontSelector;
        
        #line default
        #line hidden
        
        
        #line 205 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblc_text_color;
        
        #line default
        #line hidden
        
        
        #line 208 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.ColorPicker colpicker_text_color;
        
        #line default
        #line hidden
        
        
        #line 214 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_panel_text_save;
        
        #line default
        #line hidden
        
        
        #line 223 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_panel_text_saveas;
        
        #line default
        #line hidden
        
        
        #line 233 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_panel_text_openInFileExplorer;
        
        #line default
        #line hidden
        
        
        #line 242 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_panel_text_OpenTextDoc;
        
        #line default
        #line hidden
        
        
        #line 252 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl Panel_Items_drawMode;
        
        #line default
        #line hidden
        
        
        #line 253 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_panel_draw_switchMode;
        
        #line default
        #line hidden
        
        
        #line 262 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_panel_draw_option;
        
        #line default
        #line hidden
        
        
        #line 272 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_panel_draw_NormalBrush;
        
        #line default
        #line hidden
        
        
        #line 281 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_panel_draw_EraseBrush;
        
        #line default
        #line hidden
        
        
        #line 294 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblc_brush_size;
        
        #line default
        #line hidden
        
        
        #line 296 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider slider_brush_size;
        
        #line default
        #line hidden
        
        
        #line 303 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblc_eraser_size;
        
        #line default
        #line hidden
        
        
        #line 305 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider slider_eraserSize;
        
        #line default
        #line hidden
        
        
        #line 312 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblc_brush_color;
        
        #line default
        #line hidden
        
        
        #line 316 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.ColorPicker cpick_brushColorPicker;
        
        #line default
        #line hidden
        
        
        #line 322 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_panel_draw_Clear;
        
        #line default
        #line hidden
        
        
        #line 336 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid upBar;
        
        #line default
        #line hidden
        
        
        #line 342 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider slider_winOpacity;
        
        #line default
        #line hidden
        
        
        #line 343 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_quit;
        
        #line default
        #line hidden
        
        
        #line 350 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_minimise;
        
        #line default
        #line hidden
        
        
        #line 357 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_top;
        
        #line default
        #line hidden
        
        
        #line 367 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_fullscreen;
        
        #line default
        #line hidden
        
        
        #line 377 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_DisplayPanel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TransparentNotePad;V1.0.0.0;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 20 "..\..\..\MainWindow.xaml"
            ((TransparentNotePad.MainWindow)(target)).PreviewMouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.Window_PreviewMouseWheel);
            
            #line default
            #line hidden
            
            #line 21 "..\..\..\MainWindow.xaml"
            ((TransparentNotePad.MainWindow)(target)).Drop += new System.Windows.DragEventHandler(this.On_WinDrop);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\MainWindow.xaml"
            ((TransparentNotePad.MainWindow)(target)).DragOver += new System.Windows.DragEventHandler(this.On_WinDrop);
            
            #line default
            #line hidden
            
            #line 23 "..\..\..\MainWindow.xaml"
            ((TransparentNotePad.MainWindow)(target)).SizeChanged += new System.Windows.SizeChangedEventHandler(this.OnWinResize);
            
            #line default
            #line hidden
            
            #line 25 "..\..\..\MainWindow.xaml"
            ((TransparentNotePad.MainWindow)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            
            #line 25 "..\..\..\MainWindow.xaml"
            ((TransparentNotePad.MainWindow)(target)).KeyUp += new System.Windows.Input.KeyEventHandler(this.Window_KeyUp);
            
            #line default
            #line hidden
            
            #line 25 "..\..\..\MainWindow.xaml"
            ((TransparentNotePad.MainWindow)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.PreviewKeyDow);
            
            #line default
            #line hidden
            return;
            case 2:
            this.brd_main = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.paint_area = ((TransparentNotePad.PaintCanvas)(target));
            return;
            case 4:
            this.tbox_mainText = ((System.Windows.Controls.TextBox)(target));
            
            #line 167 "..\..\..\MainWindow.xaml"
            this.tbox_mainText.KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            
            #line 167 "..\..\..\MainWindow.xaml"
            this.tbox_mainText.KeyUp += new System.Windows.Input.KeyEventHandler(this.Window_KeyUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.display_panel = ((System.Windows.Controls.Border)(target));
            
            #line 170 "..\..\..\MainWindow.xaml"
            this.display_panel.MouseMove += new System.Windows.Input.MouseEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.panel = ((System.Windows.Controls.Grid)(target));
            
            #line 175 "..\..\..\MainWindow.xaml"
            this.panel.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.DragWindow);
            
            #line default
            #line hidden
            return;
            case 7:
            this.panel_border = ((System.Windows.Controls.Border)(target));
            return;
            case 8:
            this.Panel_Items_textMode = ((System.Windows.Controls.ItemsControl)(target));
            return;
            case 9:
            this.btn_panel_text_switchMode = ((System.Windows.Controls.Button)(target));
            
            #line 178 "..\..\..\MainWindow.xaml"
            this.btn_panel_text_switchMode.Click += new System.Windows.RoutedEventHandler(this.btn_switchMode_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btn_panel_text_option = ((System.Windows.Controls.Button)(target));
            
            #line 187 "..\..\..\MainWindow.xaml"
            this.btn_panel_text_option.Click += new System.Windows.RoutedEventHandler(this.btn_option_Click_1);
            
            #line default
            #line hidden
            return;
            case 11:
            this.tblc_text_font = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            this.cmbbox_Panels_FontSelector = ((System.Windows.Controls.ComboBox)(target));
            
            #line 202 "..\..\..\MainWindow.xaml"
            this.cmbbox_Panels_FontSelector.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbbox_Panels_FontSelector_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 13:
            this.tblc_text_color = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 14:
            this.colpicker_text_color = ((Xceed.Wpf.Toolkit.ColorPicker)(target));
            
            #line 211 "..\..\..\MainWindow.xaml"
            this.colpicker_text_color.SelectedColorChanged += new System.Windows.RoutedPropertyChangedEventHandler<System.Nullable<System.Windows.Media.Color>>(this.OnTextColorChanged);
            
            #line default
            #line hidden
            return;
            case 15:
            this.btn_panel_text_save = ((System.Windows.Controls.Button)(target));
            
            #line 214 "..\..\..\MainWindow.xaml"
            this.btn_panel_text_save.Click += new System.Windows.RoutedEventHandler(this.btn_TextSave_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.btn_panel_text_saveas = ((System.Windows.Controls.Button)(target));
            
            #line 223 "..\..\..\MainWindow.xaml"
            this.btn_panel_text_saveas.Click += new System.Windows.RoutedEventHandler(this.btn_TextSaveAs_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            this.btn_panel_text_openInFileExplorer = ((System.Windows.Controls.Button)(target));
            
            #line 233 "..\..\..\MainWindow.xaml"
            this.btn_panel_text_openInFileExplorer.Click += new System.Windows.RoutedEventHandler(this.btn_ViewInExplorerClick);
            
            #line default
            #line hidden
            return;
            case 18:
            this.btn_panel_text_OpenTextDoc = ((System.Windows.Controls.Button)(target));
            
            #line 242 "..\..\..\MainWindow.xaml"
            this.btn_panel_text_OpenTextDoc.Click += new System.Windows.RoutedEventHandler(this.btn_OpenTextDoc_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            this.Panel_Items_drawMode = ((System.Windows.Controls.ItemsControl)(target));
            return;
            case 20:
            this.btn_panel_draw_switchMode = ((System.Windows.Controls.Button)(target));
            
            #line 253 "..\..\..\MainWindow.xaml"
            this.btn_panel_draw_switchMode.Click += new System.Windows.RoutedEventHandler(this.btn_switchMode_Click);
            
            #line default
            #line hidden
            return;
            case 21:
            this.btn_panel_draw_option = ((System.Windows.Controls.Button)(target));
            
            #line 262 "..\..\..\MainWindow.xaml"
            this.btn_panel_draw_option.Click += new System.Windows.RoutedEventHandler(this.btn_option_Click_1);
            
            #line default
            #line hidden
            return;
            case 22:
            this.btn_panel_draw_NormalBrush = ((System.Windows.Controls.Button)(target));
            
            #line 272 "..\..\..\MainWindow.xaml"
            this.btn_panel_draw_NormalBrush.Click += new System.Windows.RoutedEventHandler(this.OnPenButtonClick);
            
            #line default
            #line hidden
            return;
            case 23:
            this.btn_panel_draw_EraseBrush = ((System.Windows.Controls.Button)(target));
            
            #line 281 "..\..\..\MainWindow.xaml"
            this.btn_panel_draw_EraseBrush.Click += new System.Windows.RoutedEventHandler(this.OnEraserBtn_Click);
            
            #line default
            #line hidden
            return;
            case 24:
            this.tblc_brush_size = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 25:
            this.slider_brush_size = ((System.Windows.Controls.Slider)(target));
            
            #line 300 "..\..\..\MainWindow.xaml"
            this.slider_brush_size.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.BrushSizeChanged);
            
            #line default
            #line hidden
            return;
            case 26:
            this.tblc_eraser_size = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 27:
            this.slider_eraserSize = ((System.Windows.Controls.Slider)(target));
            
            #line 309 "..\..\..\MainWindow.xaml"
            this.slider_eraserSize.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.EraserSizeChanged);
            
            #line default
            #line hidden
            return;
            case 28:
            this.tblc_brush_color = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 29:
            this.cpick_brushColorPicker = ((Xceed.Wpf.Toolkit.ColorPicker)(target));
            
            #line 319 "..\..\..\MainWindow.xaml"
            this.cpick_brushColorPicker.SelectedColorChanged += new System.Windows.RoutedPropertyChangedEventHandler<System.Nullable<System.Windows.Media.Color>>(this.ColorPicker_SelectedColorChanged);
            
            #line default
            #line hidden
            return;
            case 30:
            this.btn_panel_draw_Clear = ((System.Windows.Controls.Button)(target));
            
            #line 322 "..\..\..\MainWindow.xaml"
            this.btn_panel_draw_Clear.Click += new System.Windows.RoutedEventHandler(this.On_BtnClear_Click);
            
            #line default
            #line hidden
            return;
            case 31:
            this.upBar = ((System.Windows.Controls.Grid)(target));
            
            #line 336 "..\..\..\MainWindow.xaml"
            this.upBar.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.DragWindow);
            
            #line default
            #line hidden
            return;
            case 32:
            this.slider_winOpacity = ((System.Windows.Controls.Slider)(target));
            
            #line 342 "..\..\..\MainWindow.xaml"
            this.slider_winOpacity.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.Slider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 33:
            this.btn_quit = ((System.Windows.Controls.Button)(target));
            
            #line 343 "..\..\..\MainWindow.xaml"
            this.btn_quit.Click += new System.Windows.RoutedEventHandler(this.btn_quit_Click);
            
            #line default
            #line hidden
            return;
            case 34:
            this.btn_minimise = ((System.Windows.Controls.Button)(target));
            
            #line 350 "..\..\..\MainWindow.xaml"
            this.btn_minimise.Click += new System.Windows.RoutedEventHandler(this.btn_minimise_Click);
            
            #line default
            #line hidden
            return;
            case 35:
            this.btn_top = ((System.Windows.Controls.Button)(target));
            
            #line 357 "..\..\..\MainWindow.xaml"
            this.btn_top.Click += new System.Windows.RoutedEventHandler(this.btn_top_Click);
            
            #line default
            #line hidden
            return;
            case 36:
            this.btn_fullscreen = ((System.Windows.Controls.Button)(target));
            
            #line 367 "..\..\..\MainWindow.xaml"
            this.btn_fullscreen.Click += new System.Windows.RoutedEventHandler(this.btn_fullscreen_Click);
            
            #line default
            #line hidden
            return;
            case 37:
            this.btn_DisplayPanel = ((System.Windows.Controls.Button)(target));
            
            #line 377 "..\..\..\MainWindow.xaml"
            this.btn_DisplayPanel.Click += new System.Windows.RoutedEventHandler(this.btn_DisplayPanel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

