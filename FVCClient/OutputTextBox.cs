using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
  
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

using ITS;
using ITS.MaterialModule;

namespace FVCClient
{
    public class OutputTextBox:ITSRichTextBox
    {
        protected bool _isQuestion = false;

        double offsetDebug = 0;

        public OutputTextBox(RichTextBox textBox):base(textBox)
        { 
            
        }

        private bool needAutoScroll(RichTextBox rich, ref double offset)
        {
            if (rich.VerticalOffset >= offset)
            {
                rich.ScrollToEnd();
                offset = rich.VerticalOffset;
                return true;
            }
            else
                return false;
        } 

        protected List<Image> LoadImages(Question quest)
        {
            List<Image> images = new List<Image>();
            foreach(var f in quest.ImageFilePaths)
            {
                BitmapImage bi = new BitmapImage(new Uri(f));// (new Uri(@"C:\SimpleImage.jpg"));
                Image image = new Image();
                image.Source = bi;
                images.Add(image);
            }
            return images;
        }

        /// <summary>
        /// 需要完善，如果Question里面有图片，则必须添加
        /// 图片进来。
        /// </summary>
        /// <param name="quest"></param>
        public void AddQuestion(Question quest)
        {
            string str = "---------------------------------------------------------------\n";
            string txt = quest.Content + "(问题等级为：" + quest.Level + ")";
            Paragraph p = new Paragraph(new Run(str+txt));
            p.FontSize = 16;
            p.LineHeight = 5;
            p.Foreground = Brushes.Blue;
            _textBox.Document.Blocks.Add(p);

            if(quest.HasImageFiles())
            {
                List<Image> images = LoadImages(quest);
                foreach(var image in images)
                {
                    InlineUIContainer container = new InlineUIContainer(image);
                    Paragraph paragraph = new Paragraph(container);
                    image.Width = 100;
                    _textBox.Document.Blocks.Add(paragraph);

                }

            }
 
            needAutoScroll(_textBox, ref offsetDebug);
        }
          

        public void OnChooseCourse(string question)
        {
            string str = "------------------------------------------------------------------------\n"; 
            Paragraph p = new Paragraph(new Run(str + question));
            p.FontSize = 16;
            p.LineHeight = 5;
            p.Foreground = Brushes.Blue;
            _textBox.Document.Blocks.Add(p);

            needAutoScroll(_textBox, ref offsetDebug);
        } 
         
    }
}
