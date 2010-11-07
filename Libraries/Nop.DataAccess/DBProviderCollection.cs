using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess
{
    public sealed partial class DBProviderCollection<T> : ProviderCollection where T : BaseDBProvider
    {
        /// <summary>
        /// Adds a provider to the collection.
        /// </summary>
        /// <param name="provider">The provider to be added.</param>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            if (!(provider is T))
            {
                throw new ArgumentException(string.Format("Provider must implement {0} type", new object[] { typeof(T).ToString() }), "provider");
            }
            base.Add(provider);
        }

        /// <summary>
        /// Copies the contents of the collection to the given array starting at the specified index.
        /// </summary>
        /// <param name="array">The array to copy the elements of the collection to.</param>
        /// <param name="index">The index of the collection item at which to start the copying process.</param>
        public void CopyTo(T[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the provider with the specified name.
        /// </summary>
        /// <param name="name">The key by which the provider is identified.</param>
        /// <returns>The provider with the specified name.</returns>
        public T this[string name]
        {
            get
            {
                return (T)base[name];
            }
        }
    }
}
