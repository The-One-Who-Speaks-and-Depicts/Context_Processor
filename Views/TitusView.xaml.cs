using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.DTO;
using System.Threading.Tasks;

namespace Context_Processor.Views
{
    public class TitusView : UserControl
    {
        //create variables for operating with buttons
        private Button unitInsertButton;
        private Button semanticInsertButton;
        private Button contextsAmountInsertionButton;
        private Button contextInsertionButton;

        //create variables for operating with text boxes
        private TextBox unitField;
        private TextBox semanticsField;
        private NumericUpDown contextsAmountField;
        private TextBox sourceField;
        private TextBox contextField;
        private TextBox finalField;

        public TitusView()
        {
            InitializeComponent();
        }

        //add analyzed unit to the final field
        public void UnitInsert(object sender, RoutedEventArgs e)
        {
            finalField.Text += "Unit: " + unitField.Text + ";\n"; 
            unitField.IsReadOnly = true;
            semanticsField.IsReadOnly = false;
            unitInsertButton.IsEnabled = false;
            semanticInsertButton.IsEnabled = true;
        }

        //add analyzed unit sematics to the final field
        public void SemanticsInsert(object sender, RoutedEventArgs e) 
        {
            finalField.Text += "Semantics: " + semanticsField.Text + ";\n";
            semanticsField.IsReadOnly = true;
            semanticInsertButton.IsEnabled = false;
            contextsAmountField.IsReadOnly = false;
            contextsAmountInsertionButton.IsEnabled = true;            
        }

        //add analyzed unit amount of contexts to the final field
        public void ContextsAmountInsert(object sender, RoutedEventArgs e) 
        {
            finalField.Text += "Contexts amount: " + contextsAmountField.Text + ";\n";
            contextsAmountField.IsReadOnly = true;
            contextsAmountInsertionButton.IsEnabled = false;
            sourceField.IsReadOnly = false;
            contextField.IsReadOnly = false;
            contextInsertionButton.IsEnabled = true;
        }

        //react to context addition
        public async void ContextInsert(object sender, RoutedEventArgs e) 
        {
            var msBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentTitle = "Title",
                ContentMessage = "Message",
                Icon = Icon.Plus,
                Style = Style.UbuntuLinux
            });
            ButtonResult result = await msBoxStandardWindow.Show();
            if (result == ButtonResult.Yes) 
            {
                finalField.Text += "ASYNC ALL THE WAY";
            }
            else 
            {
                finalField.Text += "HIGHER!";
            }
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            //initialize buttons
            unitInsertButton = this.FindControl<Button>("UnitBtn");
            semanticInsertButton = this.FindControl<Button>("SemBtn");
            contextsAmountInsertionButton = this.FindControl<Button>("NumBtn");
            contextInsertionButton = this.FindControl<Button>("ContextBtn");
            //initialize text boxes
            unitField = this.FindControl<TextBox>("UnitTextBox");
            semanticsField = this.FindControl<TextBox>("SemanticsTextBox");
            contextsAmountField = this.FindControl<NumericUpDown>("NumeralBox");
            sourceField = this.FindControl<TextBox>("SourceTextBox");
            contextField = this.FindControl<TextBox>("ContextTextBox");
            finalField = this.FindControl<TextBox>("FinalTextBox");
        }
        
    }
}