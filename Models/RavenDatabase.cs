using Raven.Client.Documents;

namespace Context_Processor.Models
{
    public class RavenDatabase : DocumentStore
    {
        private static RavenDatabase ravenDatabase { get; set; }


        private RavenDatabase()
        {

        }

        public static RavenDatabase getInstance()
        {
            if (ravenDatabase is null)
            {
                var store = new RavenDatabase
                {
                    Urls = new string[]{"http://localhost:8080"},
                    Database = "UnitsDB"
                };
                store.Initialize();
                return store;
            }
            return ravenDatabase;
        }
    }
}
