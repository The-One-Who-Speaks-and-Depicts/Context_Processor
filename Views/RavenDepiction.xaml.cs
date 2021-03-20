using System;
using System.Collections.Generic;
using System.Text;
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
                editTextBox.Text = unitForEditing.name;
            }            
        }

        // save edited unit in DB
        public void EditUnit(object sender, RoutedEventArgs e)
        {

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
            }
            localizationButton.Content = localization;
        }
    }
}