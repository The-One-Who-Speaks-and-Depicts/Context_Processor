using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using Raven.Client;
using Raven.Client.Documents;
using Context_Processor.Models;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.DTO;



namespace Context_Processor.Views
{
    public class RavenDepiction : Window
    {
        // create variable for selector of units
        private ComboBox unitsComboBox;

        // create variable for unit depiction
        private TextBox editTextBox;

        //create variables for buttons
        private Button deleteButton;
        private Button localizationButton;
        private Button editButton;
        private Button XMLButton;
        private Button HTMLButton;

        //creates strings for localization
        private string localization = "ru";
        private string messageLocalized;
        private string ravenDeletionLocalized;
        private string regexMatchFailureLocalized;
        private string ravenEditingLocalized;
              

        public RavenDepiction()
        {            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this); 
            // get names for each unit           
            unitsComboBox = this.FindControl<ComboBox>("UnitsComboBox");
            unitsComboBox.Items = RavenGet().Select(unit => unit.name);
            unitsComboBox.SelectionChanged += ChooseUnit;                  
            // initialize buttons
            deleteButton = this.FindControl<Button>("DeleteBtn");
            localizationButton = this.FindControl<Button>("LocalizationBtn");
            editButton = this.FindControl<Button>("EditBtn");
            XMLButton = this.FindControl<Button>("XMLBtn");
            HTMLButton = this.FindControl<Button>("HTMLBtn");
            //initialize text box
            editTextBox = this.FindControl<TextBox>("EditTextBox");
            // localize
            Localize(new object(), new RoutedEventArgs());            
        }

        // gets units from RavenDB
        public List<Unit> RavenGet()
        {
            try 
            {
                var store = new DocumentStore 
                {
                    Urls = new string[]{"http://localhost:8080"},
                    Database = "UnitsDB"
                };
                store.Initialize();
                using (var session = store.OpenSession())
                {
                    List<Unit> units = session.Advanced.RawQuery<Unit>("from Units").ToList();
                    return units;      
                }
            }            
            catch (Raven.Client.Exceptions.RavenException)
            {                
                this.Close();
                return new List<Unit>();
            }            
        }

        // deletes units from RavenDB
        public async void DeleteUnit(object sender, RoutedEventArgs e)
        {
            var store = new DocumentStore 
            {
                Urls = new string[]{"http://localhost:8080"},
                Database = "UnitsDB"
            };
            store.Initialize();
            using (var session = store.OpenSession())
            {                
                Unit unitForDeletion = session.Advanced.RawQuery<Unit>("from Units where exact(name='" + unitsComboBox.SelectedItem + "')").ToList()[0];
                session.Delete(unitForDeletion);
                session.SaveChanges();
            }
            unitsComboBox.Items = RavenGet().Select(unit => unit.name);
            var successWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = messageLocalized,
                        ContentMessage = ravenDeletionLocalized,
                        Style = Style.UbuntuLinux
                        });
            await successWindow.Show();
        }

        // depict chosen unit

        public void ChooseUnit(object sender, SelectionChangedEventArgs args)
        {
            editTextBox.Text = "";
            var store = new DocumentStore 
            {
                Urls = new string[]{"http://localhost:8080"},
                Database = "UnitsDB"
            };
            store.Initialize();
            using (var session = store.OpenSession())
            {                
                Unit unitForEditing = session.Advanced.RawQuery<Unit>("from Units where exact(name='" + unitsComboBox.SelectedItem + "')").ToList()[0];
                editTextBox.Text += "<unit>" + unitForEditing.name + "</unit>\n";
                editTextBox.Text += "<semantics>" + unitForEditing.semantics + "</semantics>\n";
                editTextBox.Text += "<contextsAmount>" + unitForEditing.contextsAmount + "</contextsAmount>\n";
                editTextBox.Text += "<contexts>\n";
                foreach (Context context in unitForEditing.contexts)
                {
                    editTextBox.Text += "<link>" + "<context>" + context.text + "</context><source>" + context.source + "</source></link>\n";
                }
                editTextBox.Text += "</contexts>\n";
                editTextBox.Text += "<basement>" + unitForEditing.basement + "</basement>\n";
                editTextBox.Text += "<analysis>" + unitForEditing.analysis + "</analysis>\n"; 
            }            
        }

        // save edited unit in DB
        public async void EditUnit(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(editTextBox.Text, @"<unit>.*?<\/unit>\n<semantics>.*?<\/semantics>\n<contextsAmount>.*?<\/contextsAmount>\n<contexts>\n(<link><context>.*?<\/context><source>.*?<\/source><\/link>\n)*?<\/contexts>\n<basement>.*?<\/basement>\n<analysis>.*?<\/analysis>"))
            {
                var failureWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = messageLocalized,
                        ContentMessage = regexMatchFailureLocalized,
                        Style = Style.UbuntuLinux
                        });
                await failureWindow.Show();
            }
            else
            {
                var store = new DocumentStore 
                {
                    Urls = new string[]{"http://localhost:8080"},
                    Database = "UnitsDB"
                };
                store.Initialize();
                var unitName = Regex.Replace(Regex.Match(editTextBox.Text, @"<unit>.*<\/unit>").Value, @"<\/{0,1}unit>", "");
                var unitSemantics = Regex.Replace(Regex.Match(editTextBox.Text, @"<semantics>.*<\/semantics>").Value, @"<\/{0,1}semantics>", "");
                var contextsAmount = Regex.Replace(Regex.Match(editTextBox.Text, @"<contextsAmount>.*<\/contextsAmount>").Value, @"<\/{0,1}contextsAmount>", "");
                var separatedContexts = Regex.Matches(editTextBox.Text, @"<link>.*<\/link>");
                var contextList = new List<Context>();
                foreach (Match separatedContext in separatedContexts)
                {
                    var currentContext = Regex.Replace(separatedContext.Value, @"<\/{0,1}link>", "");
                    contextList.Add(new Context 
                        {
                            source = Regex.Replace(Regex.Match(currentContext, @"<source>.*<\/source>").Value, @"<\/{0,1}source>", ""),
                            text = Regex.Replace(Regex.Match(currentContext, @"<context>.*<\/context>").Value, @"<\/{0,1}context>", ""),
                        });
                }
                var basement = Regex.Replace(Regex.Match(editTextBox.Text, @"<basement>.*<\/basement>").Value, @"<\/{0,1}basement>", "");
                var analysis = Regex.Replace(Regex.Match(editTextBox.Text, @"<analysis>.*<\/analysis>").Value, @"<\/{0,1}analysis>", "");
                using (var session = store.OpenSession())
                {
                    var originalUnit = session.Advanced.RawQuery<Unit>("from Units where exact(name='" + unitsComboBox.SelectedItem + "')").ToList()[0];
                    originalUnit.name = unitName;
                    originalUnit.semantics = unitSemantics;
                    originalUnit.contextsAmount = contextsAmount;
                    originalUnit.contexts = contextList;
                    originalUnit.basement = basement;
                    originalUnit.analysis = analysis;
                    session.SaveChanges();
                }
                unitsComboBox.Items = RavenGet().Select(unit => unit.name);
                editTextBox.Text = "";
                var successWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = messageLocalized,
                            ContentMessage = ravenEditingLocalized,
                            Style = Style.UbuntuLinux
                            });
                await successWindow.Show();
            }
        }

        // save unit as XML
        public void SaveToXML(object sender, RoutedEventArgs e)
        {

        }

        //save unit as HTML
        public void SaveToHTML(object sender, RoutedEventArgs e)
        {

        }

        //  localization changes
        public void Localize (object sender, RoutedEventArgs e)
        {
            if (localization == "en")
            {
                localization = "ru";                
                deleteButton.Content = "Delete";                
                editButton.Content = "Edit";
                XMLButton.Content = "Save as XML";
                HTMLButton.Content = "Save as HTML";
                messageLocalized = "Program message";
                ravenDeletionLocalized = "Unit deleted";
                regexMatchFailureLocalized = "No unit chosen, or schema of analysis is violated";
                ravenEditingLocalized = "Unit is edited";              
            }
            else
            {
                localization = "en";
                deleteButton.Content = "Удалить";
                editButton.Content = "Редактировать";
                XMLButton.Content = "Сохранить как XML";
                HTMLButton.Content = "Сохранить как HTML";
                messageLocalized = "Сообщение программы";
                ravenDeletionLocalized = "Единица удалена";
                regexMatchFailureLocalized = "Единица не выбрана, или схема нарушена";
                ravenEditingLocalized = "Единица изменена";
            }
            localizationButton.Content = localization;
        }
    }
}