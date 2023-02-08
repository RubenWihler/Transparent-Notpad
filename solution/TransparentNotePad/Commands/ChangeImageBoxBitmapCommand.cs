namespace TransparentNotePad.Commands
{
    public class ChangeImageBoxBitmapCommand : ICommand
    {
        public CustomControls.ImageBox ImageBox { get; set; }
        public System.Drawing.Bitmap OldBitmap { get; set; }
        public System.Drawing.Bitmap NewBitmap { get; set; }

        public ChangeImageBoxBitmapCommand(CustomControls.ImageBox imageBox, System.Drawing.Bitmap bitmap)
        {
            this.ImageBox = imageBox;
            this.OldBitmap = bitmap;

            //is not yet defined
            this.NewBitmap = bitmap;
        }

        public void Execute()
        {
            this.OldBitmap = this.ImageBox.Bitmap;
            this.ImageBox.Bitmap = this.NewBitmap;
        }
        public void Undo()
        {
            this.ImageBox.Bitmap = this.OldBitmap;
        }
    }
}
