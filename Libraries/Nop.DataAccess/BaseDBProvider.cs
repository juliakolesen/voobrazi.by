using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess
{
    /// <summary>
    /// Provides a base class for abstract provider classes
    /// </summary>
    [DBProviderSectionName("NotDefined")]
    public partial class BaseDBProvider : ProviderBase
    {
        // TODO - add DBCommand override methods
    }
}
