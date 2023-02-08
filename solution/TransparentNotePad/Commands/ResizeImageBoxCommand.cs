namespace TransparentNotePad.Commands
{
    public class ResizeImageBoxCommand : ICommand
    {
        public CustomControls.ImageBox ImageBox { get; set; }
        public System.Windows.Point OldSize { get; set; }
        public System.Windows.Point NewSize { get; set; }
        
        public ResizeImageBoxCommand(CustomControls.ImageBox imageBox, System.Windows.Point size)
        {
            this.ImageBox = imageBox;
            this.NewSize = size;

            // no yet defined
            this.OldSize = size;
        }

        public void Execute()
        {
            this.OldSize = this.ImageBox.Size;
            this.ImageBox.Resize(this.NewSize);
        }
        public void Undo()
        {
            this.ImageBox.Resize(this.OldSize);
        }
    }
}
