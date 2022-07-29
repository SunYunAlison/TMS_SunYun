using System;
using System.Collections.Generic;
using System.Configuration;


namespace MicronTMS.Helper
{
    public class DbHelper
    {
        public static Dictionary<string, string> ConnectionStringList
        {

            get { return _connectionStringList == null ? new Dictionary<string, string>() : _connectionStringList; }
            set
            {
                _connectionStringList = value;
            }
        }

        public static Dictionary<string, string> _connectionStringList { get; set; }

        public static Dictionary<string, string> list { get; set; }

        static DbHelper()
        {

            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            list = new Dictionary<string, string>();
            if (connections.Count != 0)
            {
                foreach (ConnectionStringSettings connection in connections)
                {
                    string name = connection.Name;
                    string connectionString = connection.ConnectionString;
                    if (String.IsNullOrEmpty(GetConnectionStringByName(name)))
                        list.Add(name, connectionString);
                }
                ConnectionStringList = list;
            }

        }

        public static string GetConnectionStringByName(string name)
        {

            string item;
            if (!ConnectionStringList.TryGetValue(name, out item))
                return null;

            if (item.IndexOf("Pooling") <= 0)
                item = item + "; Pooling=false";

            return item;
        }
    }
}