using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Context_Processor.Views
{
    public class RavenDepictionDesign : UserControl
    {
        public RavenDepictionDesign()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}