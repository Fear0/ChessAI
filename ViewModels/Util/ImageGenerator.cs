using ChessAI.Model.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;

using System.Windows.Media.Imaging;
using System.IO;

using System.Net.Mime;
using ChessAI.Model.util.Pieces;
using System.Windows;

namespace ChessAI.ViewModels.Util
{
    internal class ImageGenerator
    {

        public static Image GeneratePieceImage(char pieceChar, char color)
        {
            Image image = new Image() { IsHitTestVisible = false };
            image.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);
            image.Tag = pieceChar + ", " +  color;

            string path = "";
         
            switch(pieceChar)
            {
                case 'p':
                    if (color == 'w')
                    {

                        path = "\\Images\\Chess_plt45.png";
                        //path = "\\ChessAI\\Images\\Chess_plt60.png";
                    }
                    else
                    {
                        path = "\\Images\\Chess_pdt45.png";
                    }
                    break;
                case 'r':
                    if (color == 'w')
                    {

                        path = "\\Images\\Chess_rlt45.png";
                        //path = "\\ChessAI\\Images\\Chess_plt60.png";
                    }
                    else
                    {
                        path = "\\Images\\Chess_rdt45.png";
                    }
                    break;
                case 'b':
                    if (color == 'w')
                    {

                        path = "\\Images\\Chess_blt45.png";
                        //path = "\\ChessAI\\Images\\Chess_plt60.png";
                    }
                    else
                    {
                        path = "\\Images\\Chess_bdt45.png";
                    }
                    break;
                case 'n':
                    if (color == 'w')
                    {

                        path = "\\Images\\Chess_nlt45.png";
                        //path = "\\ChessAI\\Images\\Chess_plt60.png";
                    }
                    else
                    {
                        path = "\\Images\\Chess_ndt45.png";
                    }
                    break;
                case 'k':
                    if (color == 'w')
                    {

                        path = "\\Images\\Chess_klt45.png";
                        //path = "\\ChessAI\\Images\\Chess_plt60.png";
                    }
                    else
                    {
                        path = "\\Images\\Chess_kdt45.png";
                    }
                    break;
                case 'q':
                    if (color == 'w')
                    {

                        path = "\\Images\\Chess_qlt45.png";
                        //path = "\\ChessAI\\Images\\Chess_plt60.png";
                    }
                    else
                    {
                        path = "\\Images\\Chess_qdt45.png";
                    }
                    break;

            }
         
            
            image.Source = new BitmapImage(new Uri(path,UriKind.Relative));
            //image.Source = null;
            return image;
        }
    }
}
