using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Text.RegularExpressions;


namespace FVCClient
{
    public abstract class ITSRichTextBox
    {
        protected RichTextBox _textBox;

        public ITSRichTextBox(RichTextBox textBox)
        {
            _textBox = textBox;
        }

        public void AddWarningText(string txt)
        {
            string warning = "----------------------------警示----------------------------\n";
            warning += txt;

            Paragraph p = new Paragraph(new Run(warning));
            p.FontSize = 14;
            p.LineHeight = 5;
            p.Foreground = Brushes.Red;
            _textBox.Document.Blocks.Add(p); 
        }

        private void AddImageToTextBox(Image img)
        {
            Paragraph parag = new Paragraph();
            parag.Inlines.Add(img);
            _textBox.Document.Blocks.Add(parag);
        }

        private Image ExpressionToImage(byte[] pngBytes)
        {
            MemoryStream strm = new MemoryStream();
            strm.Write(pngBytes, 0, pngBytes.Length);
            strm.Position = 0;

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = strm;
            bi.EndInit();

            Image img = new Image();
            img.Width = bi.Width;
            img.Height = bi.Height;
            img.Source = bi;

            return img;
        }

    }
}
