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
        private ComboBox unitsComboBox;

        public RavenDepiction()
        {            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);            
            unitsComboBox = this.FindControl<ComboBox>("UnitsComboBox");
            unitsComboBox.Items = RavenGet().Select(unit => unit.name);            
        }

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
    }
}