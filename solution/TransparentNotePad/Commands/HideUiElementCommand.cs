﻿namespace TransparentNotePad.Commands
{
    public class HideUiElementCommand : ICommand
    {
        public System.Windows.UIElement Element { get; set; }

        public HideUiElementCommand(System.Windows.UIElement element)
        {
            this.Element = element;
        }

        public void Execute()
        {
            Element.Visibility = System.Windows.Visibility.Hidden;
        }

        public void Undo()
        {
            Element.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
