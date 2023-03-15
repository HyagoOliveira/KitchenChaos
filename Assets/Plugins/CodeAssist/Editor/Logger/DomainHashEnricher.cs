using System;
using System.Linq;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Configuration;


namespace Meryel.UnityCodeAssist.Editor.Logger
{
    public class DomainHashEnricher : ILogEventEnricher
    {
        static int domainHash;

        static DomainHashEnricher()
        {
            var guid = UnityEditor.GUID.Generate();
            domainHash = guid.GetHashCode();
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "DomainHash", domainHash));
        }
    }

}