using System.Windows.Controls;
using System.Windows.Media;

namespace ChessAI.ViewModels
{
    public class IconViewModel : ViewModelBase
    {
        private readonly Image _image;

        public ImageSource pieceImage => _image.Source;
        public IconViewModel(Image pieceImage)
        {
            this._image = pieceImage;
            
        }
    }
}