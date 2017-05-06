using System;

namespace Depot.Api.Models
{
    public class Entry
    {
        public string Key { get; protected set; }
        public string Value { get; protected set; }

        protected Entry()
        {
        }

        public Entry(string key, string value)
        {
             Key = key;
             Value = value;
        }       
    }
}