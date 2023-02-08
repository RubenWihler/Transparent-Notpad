using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransparentNotePad.Commands
{
    public class HideMultipleUiElementsCommand : ICommand
    {
        public System.Windows.UIElement[] Elements { get; set; }

        public HideMultipleUiElementsCommand(System.Windows.UIElement[] elements)
        {
            this.Elements = elements;
        }

        public void Execute()
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i].Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public void Undo()
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i].Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
