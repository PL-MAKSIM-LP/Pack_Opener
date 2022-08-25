
using System.Drawing;
using System.Windows.Forms;

namespace PackOpenSimulator
{
    internal class CardForm : Form
    {
       public CardForm(Image image)
        {
            this.Height = 700;
            this.Width = 380;
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width,
                                      workingArea.Bottom - Size.Height);

            var picture = new PictureBox
            {
                Name = "pictureBox",
                Size = new Size(380, 600),
                Location = new Point(0, 0),
                Image = image,

            };
            this.Controls.Add(picture);
       }
    }
}
