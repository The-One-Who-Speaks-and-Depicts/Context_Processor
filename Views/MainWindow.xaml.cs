using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace Context_Processor.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void downloadButton_Click(object sender, RoutedEventArgs e) 
        {
            // Handle click here
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            var downloadButton = this.FindControl<Button>("downloadButton");
        }
    }
}