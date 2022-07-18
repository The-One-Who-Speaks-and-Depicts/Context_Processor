using System;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.Documents.Operations;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System.Threading.Tasks;

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
