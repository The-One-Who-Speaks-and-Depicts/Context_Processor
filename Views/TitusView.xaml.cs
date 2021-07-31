using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Context_Processor.Models;
using Context_Processor.ServiceFunctions;
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
using System.Net.Http;
using Raven.Client;
using Raven.Client.Documents;

namespace Context_Processor.Views
{
    public class TitusView : UserControl
    {
        //create variables for operating with menu items
        private MenuItem databaseEditingMenu;
        private MenuItem conversionMenu;        

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
        private string localization = "ru";
        private string insertionButtonLocalized;
        private string messageLocalized;
        private string addingContextsLocalized;
        private string changingContextsLocalized;
        private string fileChangeLocalized;
        private string successLocalized;
        private string failureLocalized;
        private string XMLErrorLocalized;
        private string ravenFailureLocalized;

        //create database
        private static IDocumentStore store;
                

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

        public string XMLToHTML(string XML)
        {
            XML = Regex.Replace(XML, @"<analyzedUnit>", "<div class=\"analyzedUnit\">" + unitTextBlock.Text + ": ");
            XML = Regex.Replace(XML, @"<\/analyzedUnit>", "</div>");
            XML = Regex.Replace(XML, @"<semantics>", "<div class=\"semantics\">" + semanticsTextBlock.Text + ": ");
            XML = Regex.Replace(XML, @"<\/semantics>", "</div>");
            XML = Regex.Replace(XML, @"<contextsAmount>", "<div class=\"contextsAmount\">" + contextsAmountTextBlock.Text + ": ");
            XML = Regex.Replace(XML, @"<\/contextsAmount>", "</div>");
            XML = Regex.Replace(XML, @"<contexts>", "<div class=\"contexts\">" + contextTextBlock.Text);
            XML = Regex.Replace(XML, @"<\/contexts>", "</div>");
            XML = Regex.Replace(XML, @"<link>", "<div class=\"link\">");
            XML = Regex.Replace(XML, @"<\/link>", "</div>");
            XML = Regex.Replace(XML, @"<context>", "<div class=\"context\">");
            XML = Regex.Replace(XML, @"<\/context>", "</div>");
            XML = Regex.Replace(XML, @"<source>", "<div class=\"source\">[");
            XML = Regex.Replace(XML, @"<\/source>", "]</div>");
            XML = Regex.Replace(XML, @"<basement>", "<div class=\"basement\">" + analysisBasementTextBlock.Text);
            XML = Regex.Replace(XML, @"<\/basement>", "</div>");
            XML = Regex.Replace(XML, @"<analysis>", "<div class=\"analysis\">" + analysisTextBlock.Text);
            XML = Regex.Replace(XML, @"<\/analysis>", "</div>");
            return XML;
        }


        public void SaveDocument(string filePath)
        { 
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<database>" + finalField.Text + "</database>");
            doc.Save(filePath);           
        }

        public void SaveHTMLDocument(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<div class=\"database\">" + XMLToHTML(finalField.Text) + "</div>");
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

        public void RewriteHTMLDocument(string filePath)
        {
            
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
                    bool success = false;
                    if (!filePath.EndsWith(".xml"))
                    {
                        filePath += ".xml";
                    }
                    if (!File.Exists(filePath)) 
                    {                        
                        SaveDocument(filePath);
                        success = true;                
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
                            success = true;
                        }
                    }
                    if (success)
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

        // copying units from RavenDB to the
        // user-chosen XML-file

        public async void RavenToXML (object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.InitialFileName = "New_Unit.xml";
            string filePath = await saveDialog.ShowAsync((Window)this.VisualRoot);
            this.IsEnabled = false;
            if (!String.IsNullOrEmpty(filePath))
            {
                try 
                {
                    bool success = false;
                    if (!filePath.EndsWith(".xml"))
                    {
                        filePath += ".xml";
                    }
                    var store = new DocumentStore 
                    {
                        Urls = new string[]{"http://localhost:8080"},
                        Database = "UnitsDB"
                    };
                    store.Initialize();
                    List<string> units_from_db = new List<string>();
                    using (var session = store.OpenSession())
                    {
                        List<Unit> units = session.Advanced.RawQuery<Unit>("from Units").ToList();                        
                        foreach (var unit in units)
                        {
                            string unit_to_add = "<analyzedUnit>";
                            unit_to_add += "<unit>" + unit.name + "</unit>\n";
                            unit_to_add += "<semantics>" + unit.semantics + "</semantics>\n";
                            unit_to_add += "<contextsAmount>" + unit.contextsAmount + "</contextsAmount>\n";
                            unit_to_add += "<contexts>\n";
                            foreach (Context context in unit.contexts)
                            {
                                unit_to_add += "<link>" + "<context>" + context.text + "</context><source>" + context.source + "</source></link>\n";
                            }
                            unit_to_add += "</contexts>\n";
                            unit_to_add += "<basement>" + unit.basement + "</basement>\n";
                            unit_to_add += "<analysis>" + unit.analysis + "</analysis>\n";
                            unit_to_add += "</analyzedUnit>";
                            units_from_db.Add(unit_to_add);
                        }  
                    }    
                    if (!File.Exists(filePath)) 
                    {                        
                        XmlDocument doc = new XmlDocument();
                        string units_to_parse = "";
                        foreach (var unit in units_from_db)
                        {
                            units_to_parse += unit;
                        }
                        doc.LoadXml("<database>" + units_to_parse + "</database>");
                        doc.Save(filePath); 
                        success = true;                
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
                            XDocument doc = XDocument.Load(filePath);
                            List<XElement> els = new List<XElement>();
                            foreach (var unit in units_from_db)
                            {
                                els.Add(XElement.Parse(unit));
                            }
                            XElement parentElement = doc.Descendants("analyzedUnit").LastOrDefault();
                            foreach (var element in els)
                            {
                                if (parentElement != null) parentElement.AddAfterSelf(element);
                            }                            
                            doc.Save(filePath);
                            success = true;
                        }
                    }
                    if (success)
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
                catch (Raven.Client.Exceptions.RavenException)
                {                
                    var failureWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = messageLocalized,
                    ContentMessage = ravenFailureLocalized,
                    Icon = Icon.Plus,
                    Style = Style.UbuntuLinux
                    });
                    await failureWindow.Show();
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
            var saveDialog = new SaveFileDialog();
            saveDialog.InitialFileName = "New_Unit.html";
            string filePath = await saveDialog.ShowAsync((Window)this.VisualRoot);
            this.IsEnabled = false;
            if (!String.IsNullOrEmpty(filePath))
            {
                try 
                {
                    bool success = false;
                    if (!filePath.EndsWith(".html"))
                    {
                        filePath += ".html";
                    }
                    if (!File.Exists(filePath)) 
                    {                        
                        SaveHTMLDocument(filePath);
                        success = true;                
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
                            RewriteHTMLDocument(filePath);
                            success = true;
                        }
                    }
                    if (success)
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

        //insertion of a unit to the RavenDB
        public async void RavenInsert(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            var failureWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = messageLocalized,
                    ContentMessage = ravenFailureLocalized,
                    Icon = Icon.Plus,
                    Style = Style.UbuntuLinux
                });
            try 
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
            catch (Raven.Client.Exceptions.RavenException)
            {         
                await failureWindow.Show();
            }
            catch (NullReferenceException)
            {
                await failureWindow.Show();
            }
            catch (HttpRequestException)
            {
                await failureWindow.Show();
            }
            catch (InvalidOperationException)
            {
                await failureWindow.Show();
            }
            this.IsEnabled = true;                    
        }


        // opening an additional window for deleting, editing, and converting units in RavenDB
        public async void RavenEdit(object sender, RoutedEventArgs e)
        {            
            var failureWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = messageLocalized,
                    ContentMessage = ravenFailureLocalized,
                    Icon = Icon.Plus,
                    Style = Style.UbuntuLinux
                    });
            try
            {
                var ravenDepiction = new RavenDepiction();
                ravenDepiction.Show();
            }
            catch (InvalidOperationException)
            {                
                await failureWindow.Show();
            }
            catch(HttpRequestException)
            {
                await failureWindow.Show();
            }
            catch (Raven.Client.Exceptions.RavenException)
            {
                await failureWindow.Show();
            }                             
        }

        public void Localize(object sender, RoutedEventArgs e) 
        {
            if (localization == "en")
            {
                localization = "ru";
                unitTextBlock.Text = "Unit";
                insertionButtonLocalized = "Insert";
                semanticsTextBlock.Text = "Semantics";
                contextsAmountTextBlock.Text = "Contexts amount";
                sourceTextBlock.Text= "Context source";
                contextTextBlock.Text = "Context";
                contextInsertionButton.Content = "Add context";
                analysisBasementTextBlock.Text = "Analysis ground";
                analysisTextBlock.Text = "Analysis";
                finalTextBlock.Text = "Result";
                XMLInsertionButton.Content = "Insert result (XML)";
                HTMLInsertionButton.Content = "Insert result (HTML)";
                databaseInsertionButton.Content = "Insert result (RavenDB)";
                erasingButton.Content = "Erase";
                messageLocalized = "Program message";
                addingContextsLocalized = "Would you like to add more contexts?";
                changingContextsLocalized = "Would you like to change the source of the contexts?";
                fileChangeLocalized = "Would you like to add unit into the existing file?";
                successLocalized = "Unit inserted";
                failureLocalized = "Void file name, unit may not be inserted";
                XMLErrorLocalized = "XML file record error; it is recommended to check, whether tags are opened and closed successfully";
                databaseEditingMenu.Header = "Edit database";
                ravenFailureLocalized = "Impossible to connect to RavenDB";
                conversionMenu.Header = "Convert";                
            }
            else 
            {
                localization = "en";
                unitTextBlock.Text = "Единица";
                insertionButtonLocalized = "Внести";
                semanticsTextBlock.Text = "Семантика";
                contextsAmountTextBlock.Text = "Количество контекстов";
                sourceTextBlock.Text = "Источник контекста";
                contextTextBlock.Text = "Контекст";
                contextInsertionButton.Content = "Добавить контекст";
                analysisBasementTextBlock.Text = "Предмет анализа";
                analysisTextBlock.Text = "Анализ";
                finalTextBlock.Text = "Итог";
                XMLInsertionButton.Content = "Внести итоговое значение (XML)";
                HTMLInsertionButton.Content = "Внести итоговое значение (HTML)";
                databaseInsertionButton.Content = "Внести итоговое значение (RavenDB)";
                erasingButton.Content = "Стереть все поля";                
                messageLocalized = "Сообщение программы";
                addingContextsLocalized = "Хотите ли добавить другие контексты?";
                changingContextsLocalized = "Хотите ли изменить источник контекста?";
                fileChangeLocalized = "Хотите ли добавить единицу в существующий файл?";
                successLocalized = "Единица добавлена";
                failureLocalized = "Пустое имя файла, единица не может быть добавлена";
                XMLErrorLocalized = "Ошибка записи в XML-файл, рекомендуется проверить правильность постановки тэгов";
                databaseEditingMenu.Header = "Внести изменения в базу данных";
                ravenFailureLocalized = "Невозможно соединиться с RavenDB";
                conversionMenu.Header = "Конвертировать";                               
            }
                localizationButton.Content = localization;                
                unitInsertButton.Content = insertionButtonLocalized;
                semanticInsertButton.Content = insertionButtonLocalized;
                contextsAmountInsertionButton.Content = insertionButtonLocalized;
                analysisBasementInsertionButton.Content = insertionButtonLocalized;
                analysisInsertionButton.Content = insertionButtonLocalized;                
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this); 

            //initialize textblocks and default inscriptions
            unitTextBlock = this.FindControl<TextBlock>("UnitBlock"); 
            semanticsTextBlock = this.FindControl<TextBlock>("SemanticsBlock");
            contextsAmountTextBlock = this.FindControl<TextBlock>("AmountBlock");
            sourceTextBlock = this.FindControl<TextBlock>("SourceBlock");
            contextTextBlock = this.FindControl<TextBlock>("ContextBlock");
            analysisBasementTextBlock = this.FindControl<TextBlock>("BasementBlock");
            analysisTextBlock = this.FindControl<TextBlock>("AnalysisBlock");
            finalTextBlock = this.FindControl<TextBlock>("FinalBlock");

            //initialize buttons and inscriptions
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
            localizationButton = this.FindControl<Button>("LocalizationBtn");

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
            conversionMenu = this.FindControl<MenuItem>("ConversionMenu");

            // set localization
            Localize(new object(), new RoutedEventArgs());

            // Initializing RavenDB store
            try
            {
                store = RavenHelper.EnsureUnitsDBExists();
            }
            catch (InvalidOperationException)
            {

            }
            catch (HttpRequestException)
            {

            }
            catch (Raven.Client.Exceptions.RavenException)
            {

            }
            
        }
        
    }
}