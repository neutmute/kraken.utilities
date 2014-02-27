using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core
{
   
    public interface IDateTimeProvider
    {
        DateTime Now {get;}
    }


    public class DateTimeProviderActual : IDateTimeProvider
    {
        public DateTime  Now
        {
	        get { return DateTime.Now; }
        }
    }
    

    public class DateTimeProviderFrozon : IDateTimeProvider
    {
        private readonly DateTime _frozenTime;

        public DateTimeProviderFrozon(DateTime frozenTime)
        {
            _frozenTime = frozenTime;
        }

        public DateTime Now
        {
            get { return _frozenTime; }
        }
    }

    public static class SystemDate
    {
        private static IDateTimeProvider _dateTimeProvider;

        static SystemDate()
        {
            Reset();
        }

        public static void SetDateTimeProvider(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public static void Reset()
        {
            _dateTimeProvider = new DateTimeProviderActual();
        }

        public static DateTime Now {get { return _dateTimeProvider.Now; }}
    }
}
