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

        //user can add multiple contexts, so actually we have to understand this:
        //is it the first addition or not?
        private bool isFirstContextInserted = false;

        //react to context addition
        public async void ContextInsert(object sender, RoutedEventArgs e) 
        {
            if (!isFirstContextInserted) 
            {
                finalField.Text += "<contexts>\n    ";
            }                        
            var contextsAdditionFinalizeWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentTitle = "Program message",
                ContentMessage = "Do you want to insert more contexts?",
                Icon = Icon.Plus,
                Style = Style.UbuntuLinux
                });
            ButtonResult result = await contextsAdditionFinalizeWindow.Show();
            if (result == ButtonResult.Yes) 
            {
                var sourceChangeWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentTitle = "Program message",
                ContentMessage = "Do you want to change context source?",
                Icon = Icon.Plus,
                Style = Style.UbuntuLinux
                });
                result = await sourceChangeWindow.Show();
                if (result == ButtonResult.Yes)
                {
                    finalField.Text += "ASYNC ALL THE WAY";
                }
                else
                {
                    finalField.Text += "GO AHEAD FACE THE LEAD JOIN THE DEAD";
                }
            }
            else 
            {
                finalField.Text += "HIGHER!";
            }
            isFirstContextInserted = true;
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