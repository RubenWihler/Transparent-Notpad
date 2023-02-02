using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace TransparentNotePad.CustomControls
{

    [NameScopeProperty("Tool", typeof(MainWindow.DMTools))]
    public class DesktopModeToolButton : CustomButton
    {
        public MainWindow.DMTools Tool { get; set; }

        protected override void InitEvents()
        {
            ThemeManager.onThemeChanged += ApplyTheme;
            this.Unloaded += (object sender, RoutedEventArgs e) =>
            {
                ThemeManager.onThemeChanged -= this.ApplyTheme;
            };

            this.buttonGroupType = ButtonGroupType.DesktopModePanel;
        }
    }
}
