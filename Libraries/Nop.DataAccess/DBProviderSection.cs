using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using System.Configuration;
using System.Web;
using System.Security.Permissions;

namespace NopSolutions.NopCommerce.DataAccess
{
    /// <summary>
    /// This class provides a way to programmatically access and modify the provider section of a configuration file. This class cannot be inherited.
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed partial class DBProviderSection : ConfigurationSection
    {
        #region Fields

        private static readonly ConfigurationProperty _propDefaultProvider = new ConfigurationProperty("defaultProvider", typeof(string));
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
        private static readonly ConfigurationProperty _propProviders = new ConfigurationProperty("providers", typeof(ProviderSettingsCollection), null, ConfigurationPropertyOptions.None);
        
        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the DBProviderConfiguration class.
        /// </summary>
        static DBProviderSection()
        {
            _properties.Add(_propProviders);
            _properties.Add(_propDefaultProvider);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the default provider
        /// </summary>
        [ConfigurationProperty("defaultProvider")]
        public string DefaultProvider
        {
            get
            {
                return (string)base[_propDefaultProvider];
            }
            set
            {
                base[_propDefaultProvider] = value;
            }
        }

        /// <summary>
        /// Gets the collection of properties.
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        /// <summary>
        /// Gets a ProviderSettingsCollection object of ProviderSettings elements.
        /// </summary>
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get
            {
                return (ProviderSettingsCollection)base[_propProviders];
            }
        }
        #endregion
    }
}

