using System.Collections.Generic;
using System.Configuration;

namespace LyncBillingBase.Conf
{
    public class PhoneCallsTableElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return this["value"].ToString(); }
        }

        [ConfigurationProperty("description")]
        public string Description
        {
            get { return this["description"].ToString(); }
        }
    }


    public class PhoneCallsTablesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PhoneCallsTableElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PhoneCallsTableElement) element).Name;
        }
    }


    public class PhoneCallsTablesSection : ConfigurationSection
    {
        public static string ConfigurationSectionName
        {
            get { return "PhoneCallsTablesSection"; }
        }

        [ConfigurationProperty("PhoneCallsTables")]
        public PhoneCallsTablesCollection PhoneCallsTables
        {
            get { return (PhoneCallsTablesCollection) this["PhoneCallsTables"]; }
        }

        public List<string> PhoneCallsTablesList
        {
            get
            {
                var tablesList = new List<string>();

                foreach (PhoneCallsTableElement el in PhoneCallsTables)
                {
                    tablesList.Add(el.Value);
                }

                return tablesList;
            }
        }
    }
}