using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD.UnitTests
{
    public class CAdocumentMOQ : IDisposable
    {
        public   HSBCContext cadocumentsContext { get; set; }
        public CAdocumentMOQ()
        {
            var options = new DbContextOptionsBuilder<HSBCContext>().UseInMemoryDatabase(databaseName: "MovieListDatabase")
            .Options;
            //HSBCContext context = null;
            //using (context = new HSBCContext(options))
            cadocumentsContext = new HSBCContext(options);
            cadocumentsContext.Database.EnsureDeleted();
            // Insert seed data into the database using one instance of the contextusing (var context = new MovieDbContext(options))
            //{
            cadocumentsContext.cadocuments.Add(new CAdocument { id=1,department = "Sylvester", avaloqid = "ghg", businesspartner = "test" });
            cadocumentsContext.cadocuments.Add(new CAdocument { id=2,department = "Sylvester", avaloqid = "ghg", businesspartner = "test1" });
            cadocumentsContext.Database.EnsureCreated();
            cadocumentsContext.SaveChanges();
            //}
        }

        public void Dispose()
        {
            cadocumentsContext.Database.EnsureDeleted();
            cadocumentsContext.Dispose();
           // throw new NotImplementedException();
        }

        //private static HSBCContext context()
        //{
        //    var options = new DbContextOptionsBuilder<HSBCContext>().UseInMemoryDatabase(databaseName: "MovieListDatabase")
        //    .Options;
        //    HSBCContext context = null;
        //    using ( context = new HSBCContext(options))
        //    // Insert seed data into the database using one instance of the contextusing (var context = new MovieDbContext(options))
        //    {
        //        context.cadocuments.Add(new CAdocument { department = "Sylvester", avaloqid = "ghg" , businesspartner ="test"});
        //        context.cadocuments.Add(new CAdocument { department = "Sylvester", avaloqid = "ghg", businesspartner = "test1" });
        //        context.SaveChanges();
        //    }
        //    return context;
        //}
    }
}
