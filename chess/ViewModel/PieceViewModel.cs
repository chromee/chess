using System;
using System.ComponentModel;

namespace chess.ViewModel
{
    public class PieceViewModel : INotifyPropertyChanged
    {
        private PieceColor pieceColorVal;
        public PieceColor PieceColor
        {
            get { return pieceColorVal; }
            set
            {
                pieceColorVal = value;
                NotifyPropertyChanged("PieceColor");
            }
        }

        private PieceType pieceTypeVal;
        public PieceType PieceType
        {
            get { return pieceTypeVal; }
            set
            {
                pieceTypeVal = value;
                NotifyPropertyChanged("PieceType");
            }
        }

        private Vector2 positionVal;
        public Vector2 Position
        {
            get { return positionVal; }
            set
            {
                positionVal = value;
                NotifyPropertyChanged("Position");
            }
        }

        private Vector2 imageVal;
        public Vector2 Image
        {
            get { return imageVal; }
            set
            {
                imageVal = value;
                NotifyPropertyChanged("Image");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
