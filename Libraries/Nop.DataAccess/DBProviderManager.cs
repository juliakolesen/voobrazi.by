using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Web.Hosting;

namespace NopSolutions.NopCommerce.DataAccess
{
    public abstract partial class DBProviderManager<T> where T : BaseDBProvider
    {
        #region Fields

        protected static bool s_Initialized;
        protected static Exception s_InitializeException;
        protected static object s_lock;
        protected static DBProviderCollection<T> s_Providers;
        private static T s_Provider;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the DBProviderManager class.
        /// </summary>
        static DBProviderManager()
        {
            s_lock = new object();
            s_Initialized = false;
            s_InitializeException = null;
        }

        #endregion

        #region Methods

        private static void Initialize()
        {
            if (s_Initialized)
            {
                if (s_InitializeException != null)
                {
                    throw s_InitializeException;
                }
            }
            else
            {
                if (s_InitializeException != null)
                {
                    throw s_InitializeException;
                }
                lock (s_lock)
                {
                    if (s_Initialized)
                    {
                        if (s_InitializeException != null)
                        {
                            throw s_InitializeException;
                        }
                    }
                    else
                    {
                        try
                        {
                            DBProviderSection section = GetSection<T>();
                            if (((section.DefaultProvider == null) || (section.Providers == null) || (section.Providers.Count < 1)))
                            {
                                throw new ProviderException("Default provider not specified");
                            }
                            s_Providers = new DBProviderCollection<T>();
                            if (HostingEnvironment.IsHosted)
                            {
                                ProvidersHelper.InstantiateProviders(section.Providers, s_Providers, typeof(T));
                            }
                            else
                            {
                                foreach (ProviderSettings settings in section.Providers)
                                {
                                    Type c = Type.GetType(settings.Type, true, true);
                                    if (!typeof(T).IsAssignableFrom(c))
                                    {
                                        throw new ArgumentException(string.Format("Provider must implement {0} type",
                                            new object[] { typeof(T).ToString() }));
                                    }
                                    T provider = (T)Activator.CreateInstance(c);
                                    NameValueCollection parameters = settings.Parameters;
                                    NameValueCollection config = new NameValueCollection(parameters.Count, StringComparer.Ordinal);
                                    foreach (string str2 in parameters)
                                    {
                                        config[str2] = parameters[str2];
                                    }
                                    provider.Initialize(settings.Name, config);
                                    s_Providers.Add(provider);
                                }
                            }
                            s_Provider = (T)s_Providers[section.DefaultProvider];
                            if (s_Provider == null)
                            {
                                throw new ConfigurationErrorsException(string.Format("Default provider not found. {0}. Line number: {1}",
                                    section.ElementInformation.Properties["defaultProvider"].Source,
                                    section.ElementInformation.Properties["defaultProvider"].LineNumber));
                            }
                            s_Providers.SetReadOnly();
                        }
                        catch (Exception exception)
                        {
                            s_InitializeException = exception;
                            throw;
                        }
                        s_Initialized = true;
                    }
                }
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Returns the configuration section.
        /// </summary>
        /// <returns>The ConfigurationSection object.</returns>
        protected static DBProviderSection GetSection<T>() where T : BaseDBProvider
        {
            string sectionName = (typeof(T).GetCustomAttributes(typeof(DBProviderSectionNameAttribute),
                true)[0] as DBProviderSectionNameAttribute).SectionName;
            return (DBProviderSection)ConfigurationManager.GetSection(sectionName);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets a reference to the default provider for the application.
        /// </summary>
        public static T Provider
        {
            get
            {
                Initialize();
                return s_Provider;
            }
        }
        /// <summary>
        /// Gets a collection of the providers for the application.
        /// </summary>
        public static DBProviderCollection<T> Providers
        {
            get
            {
                Initialize();
                return s_Providers;
            }
        }

        #endregion
    }
}
