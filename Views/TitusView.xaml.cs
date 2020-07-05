using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Context_Processor.Views
{
    public class TitusView : UserControl
    {
        public TitusView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}