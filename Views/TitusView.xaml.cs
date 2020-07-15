using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace Context_Processor.Views
{
    public class TitusView : UserControl
    {
        private Button unitInsertButton;
        private Button semanticInsertButton;
        private TextBox finalField;

        public TitusView()
        {
            InitializeComponent();
        }

        public void UnitInsert(object sender, RoutedEventArgs e)
        {          
            /*String transferrable = "<b><i>" + label1.Text + "</i></b>: "; 
            transferrable += "<i>" + richTextBox1.Text + "</i>";
            transferrable += ".<br>\n";
            richTextBox5.Text += transferrable;*/
            unitInsertButton.IsEnabled = false;
            semanticInsertButton.IsEnabled = true;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            unitInsertButton = this.FindControl<Button>("UnitBtn");
            semanticInsertButton = this.FindControl<Button>("SemBtn");
            finalField = this.FindControl<TextBox>("FinalTextBox");
        }
        
    }
}