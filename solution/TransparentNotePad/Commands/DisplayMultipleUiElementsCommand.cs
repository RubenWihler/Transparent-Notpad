namespace TransparentNotePad.Commands
{
    public class DisplayMultipleUiElementsCommand : ICommand
    {
        public System.Windows.UIElement[] Elements { get; set; }

        public DisplayMultipleUiElementsCommand(System.Windows.UIElement[] elements)
        {
            this.Elements = elements;
        }

        public void Execute()
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i].Visibility = System.Windows.Visibility.Visible;
            }
        }

        public void Undo()
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i].Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
