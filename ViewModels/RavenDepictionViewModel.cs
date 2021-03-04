using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Context_Processor.Models;
using Raven.Client;
using Raven.Client.Documents;

namespace Context_Processor.ViewModels
{
    public class RavenDepictionViewModel : ViewModelBase
    {
    	public ObservableCollection<Unit> Units { get; }

        public RavenDepictionViewModel()
        {

        	Units = new ObservableCollection<Unit>(RavenGet());
        }

        public IEnumerable<Unit> RavenGet()
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
              return units;
            }
        }
    }
}