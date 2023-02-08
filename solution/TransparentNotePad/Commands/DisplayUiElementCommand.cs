namespace TransparentNotePad.Commands
{
    public class DisplayUiElementCommand : ICommand
    {
        public System.Windows.UIElement Element { get; set; }

        public DisplayUiElementCommand(System.Windows.UIElement element)
        {
            this.Element = element;
        }

        public void Execute()
        {
            Element.Visibility = System.Windows.Visibility.Visible;
        }

        public void Undo()
        {
            Element.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
