using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace Context_Processor.Views
{
    public class TitusView : UserControl
    {
        private Button unitInsertButton;
        private Button semanticInsertButton;

        public TitusView()
        {
            InitializeComponent();
        }

        public void UnitInsert(object sender, RoutedEventArgs e)
        {            
            // do insertion
            unitInsertButton.IsEnabled = false;
            semanticInsertButton.IsEnabled = true;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            unitInsertButton = this.FindControl<Button>("UnitBtn");
            semanticInsertButton = this.FindControl<Button>("SemBtn");
            semanticInsertButton.IsEnabled = false;
        }
        
    }
}