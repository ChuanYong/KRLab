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
using System.Windows.Shapes;

namespace FVCClient
{
    public delegate void FormulaInputConfirm(string txt);
    /// <summary>
    /// FormulaEditorDialog.xaml 的交互逻辑
    /// </summary>
    public partial class FormulaEditorDialog : Window
    {
        public event FormulaInputConfirm onConfirm;
        public TextBox FormulaTextBox
        {
            get { return FormulaInputBox; }
        }
        public FormulaEditorDialog()
        {
            InitializeComponent();
        }

        private void OnFormulaInput(object sender,RoutedEventArgs e)
        {
            string f = FormulaInputBox.Text;
            onConfirm(f);
            this.Close();
        }

        private void OnFormulaCancel(object sender,RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        { 
            base.OnClosed(e);
        }

    }
}
