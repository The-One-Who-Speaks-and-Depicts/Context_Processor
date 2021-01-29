using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace Context_Processor.Views
{
    public class RavenDepiction : Window
    {
        /*public async void RavenChange(object sender, RoutedEventArgs e)
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
                // do something
              }
            }
        }*/

        public RavenDepiction()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}