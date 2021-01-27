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
using System.Text.RegularExpressions;
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
        //create variables for operating with menu items
        private MenuItem databaseEditingMenu;        

        //create variables for operating with textblocks
        private TextBlock unitTextBlock;
        private TextBlock semanticsTextBlock;
        private TextBlock contextsAmountTextBlock;
        private TextBlock sourceTextBlock;
        private TextBlock contextTextBlock;
        private TextBlock analysisBasementTextBlock;
        private TextBlock analysisTextBlock;
        private TextBlock finalTextBlock;


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
        private Button localizationButton;

        //create variables for operating with text boxes
        private TextBox unitField;
        private TextBox semanticsField;
        private NumericUpDown contextsAmountField;
        private TextBox sourceField;
        private TextBox contextField;
        private TextBox analysisBasementField;
        private TextBox analysisField;
        private TextBox finalField;

        //create variables for operating with localizations
        private string currentLocalization = "ru";
        private string unitLocalized = "Единица";
        private string insertionButtonLocalized = "Внести";
        private string semanticsLocalized = "Семантика";
        private string contextsAmountLocalized = "Количество контекстов";
        private string sourceLocalized = "Источник контекста";
        private string contextLocalized = "Контекст";
        private string contextButtonLocalized = "Добавить контекст";
        private string basementLocalized = "Предмет анализа";
        private string analysisLocalized = "Анализ";
        private string finalLocalized = "Итог";
        private string XMLLocalized = "Внести итоговое значение (XML)";
        private string HTMLLocalized = "Внести итоговое значение (HTML)";
        private string RavenLocalized = "Внести итоговое значение (RavenDB)";
        private string erasingLocalized = "Стереть все поля";
        private string localizationLocalized = "en";
        private string messageLocalized = "Сообщение программы";
        private string addingContextsLocalized = "Хотите ли добавить другие контексты?";
        private string changingContextsLocalized = "Хотите ли изменить источник контекста?";
        private string fileChangeLocalized = "Хотите ли добавить единицу в существующий файл?";
        private string successLocalized = "Единица добавлена";
        private string failureLocalized = "Пустое имя файла, единица не может быть добавлена";
        private string XMLErrorLocalized = "Ошибка записи в XML-файл, рекомендуется проверить правильность постановки тэгов";
        private string databaseMenuLocalized = "Внести изменения в базу данных";

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
                ContentTitle = messageLocalized,
                ContentMessage = addingContextsLocalized,
                Icon = Icon.Plus,
                Style = Style.UbuntuLinux
                });
            ButtonResult result = await contextsAdditionFinalizeWindow.Show();
            if (result == ButtonResult.Yes) 
            {
                contextField.Text = "";            
                var sourceChangeWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentTitle = messageLocalized,
                ContentMessage = changingContextsLocalized,
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
                try 
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
                        ContentTitle = messageLocalized,
                        ContentMessage = fileChangeLocalized,
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
                        ContentTitle = messageLocalized,
                        ContentMessage = successLocalized,
                        Icon = Icon.Plus,
                        Style = Style.UbuntuLinux
                        });
                    await successWindow.Show();
                }
                catch (XmlException)
                {
                    var errorWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = messageLocalized,
                    ContentMessage = XMLErrorLocalized,
                    Icon = Icon.Plus,
                    Style = Style.UbuntuLinux
                    });
                    await errorWindow.Show();
                }
                finally 
                {
                    RenewForm();
                }
            }
            else 
            {
                var errorWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = messageLocalized,
                    ContentMessage = failureLocalized,
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
                    ContentTitle = messageLocalized,
                    ContentMessage = successLocalized,
                    Icon = Icon.Plus,
                    Style = Style.UbuntuLinux
                    });
            await successWindow.Show();
            RenewForm();
        }

        //insertion of a unit to the RavenDB
        public async void RavenInsert(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            using (var store = new DocumentStore
            {
                Urls = new string[] {"http://localhost:8080"},
                Database = "UnitsDB"
            })
            {
                store.Initialize();
                var unitName = Regex.Replace(Regex.Match(finalField.Text, @"<unit>.*<\/unit>").Value, @"<\/{0,1}unit>", "");
                var unitSemantics = Regex.Replace(Regex.Match(finalField.Text, @"<semantics>.*<\/semantics>").Value, @"<\/{0,1}semantics>", "");
                var contextsAmount = Regex.Replace(Regex.Match(finalField.Text, @"<contextsAmount>.*<\/contextsAmount>").Value, @"<\/{0,1}contextsAmount>", "");
                var separatedContexts = Regex.Matches(finalField.Text, @"<link>.*<\/link>");
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
                var basement = Regex.Replace(Regex.Match(finalField.Text, @"<basement>.*<\/basement>").Value, @"<\/{0,1}basement>", "");
                var analysis = Regex.Replace(Regex.Match(finalField.Text, @"<analysis>.*<\/analysis>").Value, @"<\/{0,1}analysis>", "");
                using (var session = store.OpenSession())
                {
                    var unit = new Unit
                      {
                        name = unitName,
                        semantics = unitSemantics,
                        contextsAmount = contextsAmount,
                        contexts = contextList,
                        basement = basement,
                        analysis = analysis,
                      };
                      session.Store(unit);
                      session.SaveChanges();
                }
            }
            var successWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = messageLocalized,
                    ContentMessage = successLocalized,
                    Icon = Icon.Plus,
                    Style = Style.UbuntuLinux
                    });
            await successWindow.Show();
            RenewForm();
            this.IsEnabled = true;
        }

        public async void RavenChange(object sender, RoutedEventArgs e)
        {
            var store = new DocumentStore 
            {
                Urls = new string[]{"http://localhost:8080"},
                Database = "UnitsDB"
            };
            store.Initialize();
            using (var session = store.OpenSession())
            {
              var units = session.Query<Unit>();
              foreach (var unit in units) 
              {
                var successWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = messageLocalized,
                    ContentMessage = unit.id,
                    Icon = Icon.Plus,
                    Style = Style.UbuntuLinux
                    });
            await successWindow.Show();
              }
            }
        }

        public void Localize(object sender, RoutedEventArgs e) 
        {
            if (currentLocalization == "ru")
            {
                currentLocalization = "en";
                localizationLocalized = "ru";
                localizationButton.Content = localizationLocalized;
                unitLocalized = "Unit";
                insertionButtonLocalized = "Insert";
                semanticsLocalized = "Semantics";
                contextsAmountLocalized = "Contexts amount";
                sourceLocalized = "Context source";
                contextLocalized = "Context";
                contextButtonLocalized = "Add context";
                basementLocalized = "Analysis ground";
                analysisLocalized = "Analysis";
                finalLocalized = "Result";
                XMLLocalized = "Insert result (XML)";
                HTMLLocalized = "Insert result (HTML)";
                RavenLocalized = "Insert result (RavenDB)";
                erasingLocalized = "Erase";
                messageLocalized = "Program message";
                addingContextsLocalized = "Would you like to add more contexts?";
                changingContextsLocalized = "Would you like to change the source of the contexts?";
                fileChangeLocalized = "Would you like to add unit into the existing file?";
                successLocalized = "Unit inserted";
                failureLocalized = "Void file name, unit may not be inserted";
                XMLErrorLocalized = "XML file record error; it is recommended to check, whether tags are opened and closed successfully";
                databaseMenuLocalized = "Edit database";
                unitTextBlock.Text = unitLocalized;                
                semanticsTextBlock.Text = semanticsLocalized;
                contextsAmountTextBlock.Text = contextsAmountLocalized;
                sourceTextBlock.Text = sourceLocalized;
                contextTextBlock.Text = contextLocalized;
                analysisBasementTextBlock.Text = basementLocalized;
                analysisTextBlock.Text = analysisLocalized;
                finalTextBlock.Text = finalLocalized;
                unitInsertButton.Content = insertionButtonLocalized;
                semanticInsertButton.Content = insertionButtonLocalized;
                contextsAmountInsertionButton.Content = insertionButtonLocalized;
                contextInsertionButton.Content = contextButtonLocalized;
                analysisBasementInsertionButton.Content = insertionButtonLocalized;
                analysisInsertionButton.Content = insertionButtonLocalized;
                XMLInsertionButton.Content = XMLLocalized;
                HTMLInsertionButton.Content = HTMLLocalized;
                databaseInsertionButton.Content = RavenLocalized;
                erasingButton.Content = erasingLocalized;
                databaseEditingMenu.Header = databaseMenuLocalized;
            }
            else 
            {
                currentLocalization = "ru";
                localizationLocalized = "en";
                localizationButton.Content = localizationLocalized;
                unitLocalized = "Единица";
                insertionButtonLocalized = "Внести";
                semanticsLocalized = "Семантика";
                contextsAmountLocalized = "Количество контекстов";
                sourceLocalized = "Источник контекста";
                contextLocalized = "Контекст";
                contextButtonLocalized = "Добавить контекст";
                basementLocalized = "Предмет анализа";
                analysisLocalized = "Анализ";
                finalLocalized = "Итог";
                XMLLocalized = "Внести итоговое значение (XML)";
                HTMLLocalized = "Внести итоговое значение (HTML)";
                RavenLocalized = "Внести итоговое значение (RavenDB)";
                erasingLocalized = "Стереть все поля";
                messageLocalized = "Сообщение программы";
                addingContextsLocalized = "Хотите ли добавить другие контексты?";
                changingContextsLocalized = "Хотите ли изменить источник контекста?";
                fileChangeLocalized = "Хотите ли добавить единицу в существующий файл?";
                successLocalized = "Единица добавлена";
                failureLocalized = "Пустое имя файла, единица не может быть добавлена";
                XMLErrorLocalized = "Ошибка записи в XML-файл, рекомендуется проверить правильность постановки тэгов";
                databaseMenuLocalized = "Внести изменения в базу данных";
                unitTextBlock.Text = unitLocalized;                
                semanticsTextBlock.Text = semanticsLocalized;
                contextsAmountTextBlock.Text = contextsAmountLocalized;
                sourceTextBlock.Text = sourceLocalized;
                contextTextBlock.Text = contextLocalized;
                analysisBasementTextBlock.Text = basementLocalized;
                analysisTextBlock.Text = analysisLocalized;
                finalTextBlock.Text = finalLocalized;
                unitInsertButton.Content = insertionButtonLocalized;
                semanticInsertButton.Content = insertionButtonLocalized;
                contextsAmountInsertionButton.Content = insertionButtonLocalized;
                contextInsertionButton.Content = contextButtonLocalized;
                analysisBasementInsertionButton.Content = insertionButtonLocalized;
                analysisInsertionButton.Content = insertionButtonLocalized;
                XMLInsertionButton.Content = XMLLocalized;
                HTMLInsertionButton.Content = HTMLLocalized;
                databaseInsertionButton.Content = RavenLocalized;
                erasingButton.Content = erasingLocalized;
                databaseEditingMenu.Header = databaseMenuLocalized;
            }
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this); 

            //initialize textblocks and default inscriptions
            unitTextBlock = this.FindControl<TextBlock>("UnitBlock");            
            unitTextBlock.Text = unitLocalized;
            semanticsTextBlock = this.FindControl<TextBlock>("SemanticsBlock");
            semanticsTextBlock.Text = semanticsLocalized;
            contextsAmountTextBlock = this.FindControl<TextBlock>("AmountBlock");
            contextsAmountTextBlock.Text = contextsAmountLocalized;
            sourceTextBlock = this.FindControl<TextBlock>("SourceBlock");
            sourceTextBlock.Text = sourceLocalized;
            contextTextBlock = this.FindControl<TextBlock>("ContextBlock");
            contextTextBlock.Text = contextLocalized;
            analysisBasementTextBlock = this.FindControl<TextBlock>("BasementBlock");
            analysisBasementTextBlock.Text = basementLocalized;
            analysisTextBlock = this.FindControl<TextBlock>("AnalysisBlock");
            analysisTextBlock.Text = analysisLocalized;
            finalTextBlock = this.FindControl<TextBlock>("FinalBlock");
            finalTextBlock.Text = finalLocalized;


            //initialize buttons and inscriptions
            unitInsertButton = this.FindControl<Button>("UnitBtn");
            unitInsertButton.Content = insertionButtonLocalized;
            semanticInsertButton = this.FindControl<Button>("SemBtn");
            semanticInsertButton.Content = insertionButtonLocalized;
            contextsAmountInsertionButton = this.FindControl<Button>("NumBtn");
            contextsAmountInsertionButton.Content = insertionButtonLocalized;
            contextInsertionButton = this.FindControl<Button>("ContextBtn");
            contextInsertionButton.Content = contextButtonLocalized;
            analysisBasementInsertionButton = this.FindControl<Button>("BasementBtn");
            analysisBasementInsertionButton.Content = insertionButtonLocalized;
            analysisInsertionButton = this.FindControl<Button>("AnalysisBtn");
            analysisInsertionButton.Content = insertionButtonLocalized;
            XMLInsertionButton = this.FindControl<Button>("XmlBtn");
            XMLInsertionButton.Content = XMLLocalized;
            HTMLInsertionButton = this.FindControl<Button>("HtmlBtn");
            HTMLInsertionButton.Content = HTMLLocalized;
            databaseInsertionButton = this.FindControl<Button>("RavenBtn");
            databaseInsertionButton.Content = RavenLocalized;
            erasingButton = this.FindControl<Button>("EraseBtn");
            erasingButton.Content = erasingLocalized;
            localizationButton = this.FindControl<Button>("LocalizationBtn");
            localizationButton.Content = localizationLocalized;

            //initialize text boxes
            unitField = this.FindControl<TextBox>("UnitTextBox");
            semanticsField = this.FindControl<TextBox>("SemanticsTextBox");
            contextsAmountField = this.FindControl<NumericUpDown>("NumeralBox");
            sourceField = this.FindControl<TextBox>("SourceTextBox");
            contextField = this.FindControl<TextBox>("ContextTextBox");
            analysisBasementField = this.FindControl<TextBox>("BasementTextBox");
            analysisField = this.FindControl<TextBox>("AnalysisTextBox");
            finalField = this.FindControl<TextBox>("FinalTextBox");

            //Initializing menus
            databaseEditingMenu = this.FindControl<MenuItem>("DatabaseEditingMenu");
            databaseEditingMenu.Header = databaseMenuLocalized;
        }
        
    }
}