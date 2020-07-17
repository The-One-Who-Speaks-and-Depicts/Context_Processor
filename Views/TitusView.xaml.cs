using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.DTO;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Context_Processor.Views
{
    public class TitusView : UserControl
    {        

        //create variables for operating with buttons
        private Button unitInsertButton;
        private Button semanticInsertButton;
        private Button contextsAmountInsertionButton;
        private Button contextInsertionButton;
        private Button analysisBasementInsertionButton;
        private Button analysisInsertionButton;
        private Button databaseInsertionButton;

        //create variables for operating with text boxes
        private TextBox unitField;
        private TextBox semanticsField;
        private NumericUpDown contextsAmountField;
        private TextBox sourceField;
        private TextBox contextField;
        private TextBox analysisBasementField;
        private TextBox analysisField;
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
                finalField.Text += "<contexts>    ";
            }
            finalField.Text += "<link>" + "<context>" + contextField.Text + "</context><source>" + sourceField.Text + "</source></link>\n";                        
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
                contextField.Text = "";            
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
                    sourceField.Text = "";
                }
            }
            else 
            {
                finalField.Text += "</contexts>\n";
                sourceField.IsReadOnly = true;
                contextField.IsReadOnly = true;
                contextInsertionButton.IsEnabled = false;                
                analysisBasementField.IsReadOnly = false;
                analysisBasementInsertionButton.IsEnabled = true;
            }
            isFirstContextInserted = true;
        }

        //add basement for analysis to the final field
        public void BasementInsert(object sender, RoutedEventArgs e) 
        {
            finalField.Text += "Analysis basement: " + analysisBasementField.Text + ";\n";
            analysisBasementField.IsReadOnly = true;
            analysisBasementInsertionButton.IsEnabled = false;
            analysisField.IsReadOnly = false;
            analysisInsertionButton.IsEnabled = true;
        }

        //add analysis to the final field
        public void AnalysisInsert(object sender, RoutedEventArgs e)
        {
            finalField.Text += "Analysis: " + analysisField.Text + ";\n";
            analysisField.IsReadOnly = true;
            analysisInsertionButton.IsEnabled = false;
            finalField.IsReadOnly = false;
            databaseInsertionButton.IsEnabled = true;            
        }


        public void SaveDocument(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(finalField.Text);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            // Save the document to a file and auto-indent the output.
            XmlWriter writer = XmlWriter.Create(filePath, settings);
            doc.Save(writer);
        }

        //insertion of a unit to the database
        //currently, a user-chosen XML-file
        public async void DatabaseInsert(object sender, RoutedEventArgs e)
        {
            string filePath = await new SaveFileDialog().ShowAsync((Window)this.VisualRoot);
            if (!File.Exists(filePath)) 
            {
                if (!filePath.EndsWith(".xml"))
                {
                    filePath += ".xml";
                    SaveDocument(filePath);
                }
                SaveDocument(filePath);                
            }
            else 
            {                
                var fileFoundWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentTitle = "Program message",
                ContentMessage = "Do you want to write into existing file?",
                Icon = Icon.Plus,
                Style = Style.UbuntuLinux
                });
                var result = await fileFoundWindow.Show();
                if (result == ButtonResult.Yes) 
                {
                    SaveDocument(filePath);
                }
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
            analysisBasementInsertionButton = this.FindControl<Button>("BasementBtn");
            analysisInsertionButton = this.FindControl<Button>("AnalysisBtn");
            databaseInsertionButton = this.FindControl<Button>("FinalBtn");
            //initialize text boxes
            unitField = this.FindControl<TextBox>("UnitTextBox");
            semanticsField = this.FindControl<TextBox>("SemanticsTextBox");
            contextsAmountField = this.FindControl<NumericUpDown>("NumeralBox");
            sourceField = this.FindControl<TextBox>("SourceTextBox");
            contextField = this.FindControl<TextBox>("ContextTextBox");
            analysisBasementField = this.FindControl<TextBox>("BasementTextBox");
            analysisField = this.FindControl<TextBox>("AnalysisTextBox");
            finalField = this.FindControl<TextBox>("FinalTextBox");
        }
        
    }
}