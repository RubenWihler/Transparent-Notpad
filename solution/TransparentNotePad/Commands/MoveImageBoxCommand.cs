namespace TransparentNotePad.Commands
{
    public class MoveImageBoxCommand : ICommand
    {
        public CustomControls.ImageBox ImageBox { get; set; }
        public System.Windows.Point OldPosition { get; set; }
        public System.Windows.Point NewPosition { get; set; }

        public MoveImageBoxCommand(CustomControls.ImageBox imageBox, System.Windows.Point newPosition)
        {
            this.ImageBox = imageBox;
            this.NewPosition = newPosition;

            //is not yet defined
            this.OldPosition = newPosition;
        }

        public void Execute()
        {
            this.OldPosition = this.ImageBox.Position;
            this.ImageBox.MoveTo(this.NewPosition);
        }
        public void Undo()
        {
            this.ImageBox.MoveTo(this.OldPosition);
        }
    }
}
