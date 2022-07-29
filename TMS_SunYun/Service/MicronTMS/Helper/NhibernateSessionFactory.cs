using MicronTMS.DLL.Entities;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System.Configuration;

namespace MicronTMS.Helper
{
    public class NhibernateSessionFactory
    {
        public ISessionFactory GetSessionFactory(string DB)
        {
            var configBase = Fluently.Configure();
            //for server and vs2013

            string connectionString = ConfigurationManager.ConnectionStrings[DB].ConnectionString;

            var dbConfiguration = configBase.Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString).ShowSql());
                     #region Table and SP Mapping Registration

            var mappings = dbConfiguration.Mappings(m => m.FluentMappings.AddFromAssemblyOf<CMcVersion>());

               #endregion Table Mapping Registration

            var configuration = mappings.BuildConfiguration();
            var sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory;

        }
    }
}