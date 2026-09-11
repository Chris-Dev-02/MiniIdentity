using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniIdentity.Enterprise.Configurations
{
    /// <summary>
    /// Defines the default configuration settings for MiniIdentity.
    /// These settings currently apply globally and may become client-specific
    /// when client support is introduced.
    /// </summary>
    public static class DefaultIdentityConfiguration
    {
        public static bool EmailMustBeVerified { get; set; } = false;
        public static int MaximumFailedLoginAttempts { get; set; } = 5;
        public static int LockoutTime { get; set; } = 60;
        public static bool AllowsMultipleSessions { get; set; } = false;
        public static bool ShouldRevokeLastSession { get; set; } = false;
    }
}
