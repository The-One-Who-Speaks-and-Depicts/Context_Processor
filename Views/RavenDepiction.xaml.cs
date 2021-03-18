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

        //create variables for buttons
        private Button deleteButton;
        private Button localizationButton;

        //creates strings for localization
        private string deletionLocalized = "Удалить";
        private string localizationLocalized = "ru";

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
            // initialize buttons
            deleteButton = this.FindControl<Button>("DeleteBtn");
            deleteButton.Content = deletionLocalized;
            localizationButton = this.FindControl<Button>("LocalizationBtn");
            localizationButton.Content = localizationLocalized;            
        }

        // gets units from RavenDB
        public List<Unit> RavenGet()
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

        public void DeleteUnit(object sender, RoutedEventArgs e)
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
        }

        //  localization changes
        public void Localize (object sender, RoutedEventArgs e)
        {
            if (localizationLocalized == "ru")
            {
                localizationLocalized = "en";
                localizationButton.Content = localizationLocalized;
                deletionLocalized = "Delete";
                deleteButton.Content = deletionLocalized;
            }
            else
            {
                localizationLocalized = "ru";
                localizationButton.Content = localizationLocalized;
                deletionLocalized = "Удалить";
                deleteButton.Content = deletionLocalized;
            }
        }
    }
}