using System;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.Documents.Operations;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System.Threading.Tasks;

namespace Context_Processor.ServiceFunctions
{
	public static class RavenHelper
	{
		public static IDocumentStore EnsureUnitsDBExists()
		{
			var store = new DocumentStore 
            {
                Urls = new string[]{"http://localhost:8080"},
                Database = "UnitsDB"
            };
		    string database = store.Database;
		    store.Initialize();

		    try
		    {
		        store.Maintenance.ForDatabase(database).Send(new GetStatisticsOperation());
		    }
		    catch (DatabaseDoesNotExistException)
		    {
		        try
		        {
		            store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(database)));
		        }
		        catch (ConcurrencyException)
		        {
		            // The database was already created before calling CreateDatabaseOperation
		        }

		    }
		    return store;
		}
	}
}