using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Core;

namespace Kraken.Core.Tests.TestClasses
{
    /// <summary>
    /// Just some object that has lots of data types
    /// </summary>
    public class DataEntity 
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public Decimal Amount { get; set; }

        public bool IsCool { get; set; }

        public string Description { get; set; }

        public static List<DataEntity> GetSeveral()
        {
            List<DataEntity> list = new List<DataEntity>();

            DataEntity de1 = new DataEntity
                                 {
                                     Id = 1,
                                     Created = DateTime.Parse("2000-01-01"),
                                     Amount = 14.23m,
                                     Description = "Holy bat man boat monster",
                                     IsCool = true
                                 };

            DataEntity de2 = new DataEntity
            {
                Id = 1,
                Created = DateTime.Parse("2000-03-01"),
                Amount = 12314.23m,
                Description = "what boy wonder",
                IsCool = false
            };

            DataEntity de3 = new DataEntity
            {
                Id = 1,
                Created = DateTime.Parse("2010-01-01"),
                Amount = 14.213333333m,
                Description = null,
                IsCool = true
            };

            list.Add(de1);
            list.Add(de2);
            list.Add(de3);

            return list;
        }

        #region IObjectDumpable<DataEntity> Members

        public static ObjectDump GetObjectDump(DataEntity a)
        {
            ObjectDump dump = new ObjectDump();

            dump.Headers = new List<string> { "Id", "IsCool", "Description", "Created", "Amount" };

            dump.Data = new List<String>
                          {
                              a.Id.ToString(),
                              a.IsCool.ToString(),
                              a. Description,
                              a.Created.ToString("yyyy-MM-dd"),
                              a.Amount.ToString(),
                          };

            return dump;
        }

        #endregion
    }
}
