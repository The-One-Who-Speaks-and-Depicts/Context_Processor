using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Context_Processor.Models;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using Raven.Client;
using Raven.Client.Documents;

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
        private Button XMLInsertionButton;
        private Button HTMLInsertionButton;
        private Button databaseInsertionButton;
        private Button erasingButton;

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
            finalField.Text += "<analyzedUnit>\n";
            finalField.Text += "<unit>" + unitField.Text + "</unit>\n"; 
            unitField.IsReadOnly = true;
            semanticsField.IsReadOnly = false;
            unitInsertButton.IsEnabled = false;
            semanticInsertButton.IsEnabled = true;
        }

        //add analyzed unit sematics to the final field
        public void SemanticsInsert(object sender, RoutedEventArgs e) 
        {
            finalField.Text += "<semantics>" + semanticsField.Text + "</semantics>\n";
            semanticsField.IsReadOnly = true;
            semanticInsertButton.IsEnabled = false;
            contextsAmountField.IsReadOnly = false;
            contextsAmountInsertionButton.IsEnabled = true;            
        }

        //add analyzed unit amount of contexts to the final field
        public void ContextsAmountInsert(object sender, RoutedEventArgs e) 
        {
            finalField.Text += "<contextsAmount>" + contextsAmountField.Text + "</contextsAmount>\n";
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
                finalField.Text += "<contexts>\n";
            }
            finalField.Text += "<link>" + "<context>" + contextField.Text + "</context><source>" + sourceField.Text + "</source></link>\n";                        
            this.IsEnabled = false;
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
            this.IsEnabled = true;
            isFirstContextInserted = true;
        }

        //add basement for analysis to the final field
        public void BasementInsert(object sender, RoutedEventArgs e) 
        {
            finalField.Text += "<basement>" + analysisBasementField.Text + "</basement>\n";
            analysisBasementField.IsReadOnly = true;
            analysisBasementInsertionButton.IsEnabled = false;
            analysisField.IsReadOnly = false;
            analysisInsertionButton.IsEnabled = true;
        }

        //add analysis to the final field
        public void AnalysisInsert(object sender, RoutedEventArgs e)
        {
            finalField.Text += "<analysis>" + analysisField.Text + "</analysis>\n";
            finalField.Text += "</analyzedUnit>";
            analysisField.IsReadOnly = true;
            analysisInsertionButton.IsEnabled = false;
            finalField.IsReadOnly = false;
            erasingButton.IsEnabled = true;
            XMLInsertionButton.IsEnabled = true;
            HTMLInsertionButton.IsEnabled = true;
            databaseInsertionButton.IsEnabled = true;            
        }


        public void SaveDocument(string filePath)
        { 
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<database>" + finalField.Text + "</database>");
            doc.Save(filePath);           
        }

        public void RewriteDocument(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            XElement el = XElement.Parse(finalField.Text);
            XElement parentElement = doc.Descendants("analyzedUnit").LastOrDefault();
            if (parentElement != null) parentElement.AddAfterSelf(el);
            doc.Save(filePath);

        }

        public void RenewForm()
        {
            unitField.Text = "";
            semanticsField.Text = "";
            contextsAmountField.Text = "";
            sourceField.Text = "";
            contextField.Text = "";
            analysisBasementField.Text = "";
            analysisField.Text = "";
            finalField.Text = "";
            finalField.IsReadOnly = true;
            erasingButton.IsEnabled = false;
            XMLInsertionButton.IsEnabled = false;
            HTMLInsertionButton.IsEnabled = false;
            databaseInsertionButton.IsEnabled = false;
            unitField.IsReadOnly = false;
            unitInsertButton.IsEnabled = true;
            isFirstContextInserted = false;
        }

        public void EraseAllFields(object sender, RoutedEventArgs e)
        {
            RenewForm();
        }

        //insertion of a unit to the
        //user-chosen XML-file
        public async void XMLInsert(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.InitialFileName = "New_Unit.xml";
            string filePath = await saveDialog.ShowAsync((Window)this.VisualRoot);
            this.IsEnabled = false;
            if (!String.IsNullOrEmpty(filePath))
            {
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
                        RewriteDocument(filePath);
                    }
                }
                var successWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Program message",
                    ContentMessage = "Unit is inserted",
                    Icon = Icon.Plus,
                    Style = Style.UbuntuLinux
                    });
                await successWindow.Show();
                RenewForm();                
            }
            else 
            {
                var errorWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Program message",
                    ContentMessage = "There is no file name, unit is not inserted",
                    Icon = Icon.Plus,
                    Style = Style.UbuntuLinux
                    });
            }
            this.IsEnabled = true;
            
        }

        //insertion of a unit to the
        //user-chosen HTML-file
        public async void HTMLInsert (object sender, RoutedEventArgs e)
        {
            var successWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Program message",
                    ContentMessage = "Unit is inserted",
                    Icon = Icon.Plus,
                    Style = Style.UbuntuLinux
                    });
            await successWindow.Show();
            RenewForm();
        }

        //insertion of a unit to the RavenDB
        public async void RavenInsert(object sender, RoutedEventArgs e)
        {
            using (var store = new DocumentStore
            {
                Urls = new string[] {"http://localhost:8080"},
                Database = "UnitsDB"
            })
            {
                store.Initialize();
                // TODO: via Regex get all this
                var unitName = "";
                var unitSemantics = "";
                var contextsAmount = "";
                var contextList = new List<Context>();
                var context = new Context 
                {
                    source = "[]",
                    text = "",
                };
                contextList.Add(context);
                var basement = "";
                var analysis = "";
                using (var session = store.OpenSession())
                {
                    var unit = new Unit
                      {
                        name = "",
                        semantics = "",
                        contextsAmount = "",
                        contexts = contextList,
                        basement = "",
                        analysis = "",
                      };
                      session.Store(unit);
                      session.SaveChanges();
                }
            }
            var successWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Program message",
                    ContentMessage = "Unit is inserted",
                    Icon = Icon.Plus,
                    Style = Style.UbuntuLinux
                    });
            await successWindow.Show();
            RenewForm();
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
            XMLInsertionButton = this.FindControl<Button>("XmlBtn");
            HTMLInsertionButton = this.FindControl<Button>("HtmlBtn");
            databaseInsertionButton = this.FindControl<Button>("RavenBtn");

            erasingButton = this.FindControl<Button>("EraseBtn");
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