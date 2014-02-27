using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Kraken.Core
{
    public delegate void BlankMethod();

    /// <summary>
    /// Encapsulates some code in a transaction scope - captures databases and WCF services
    /// </summary>
    public static class TransactionManager
    {
        public static void Capture(BlankMethod daoConsumingMethod)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                daoConsumingMethod();
                scope.Complete();
            }
        }
    }
}
