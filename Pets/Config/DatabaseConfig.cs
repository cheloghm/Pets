using System;

namespace Pets.Config
{
    public class DatabaseConfig
    {
        /// <summary>
        /// Optionally provide the full connection string directly.
        /// If set, the BuildConnectionString method will return this string.
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// The host address for the PostgreSQL database.
        /// </summary>
        public string? Host { get; set; }

        /// <summary>
        /// The port for the PostgreSQL database.
        /// </summary>
        public string? Port { get; set; }

        /// <summary>
        /// The database name.
        /// </summary>
        public string? DatabaseName { get; set; }

        /// <summary>
        /// The username used to authenticate with the database.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// The password used to authenticate with the database.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Builds the connection string for PostgreSQL.  
        /// If a full connection string is already provided in ConnectionString, it is returned directly.
        /// Otherwise, it is built from the individual properties.
        /// </summary>
        /// <returns>A PostgreSQL connection string.</returns>
        public string BuildConnectionString()
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                // If the entire connection string is already set, just return it.
                return ConnectionString;
            }

            // Validate that the required properties are provided.
            if (string.IsNullOrEmpty(Host) ||
                string.IsNullOrEmpty(Port) ||
                string.IsNullOrEmpty(DatabaseName) ||
                string.IsNullOrEmpty(Username) ||
                string.IsNullOrEmpty(Password))
            {
                throw new InvalidOperationException("One or more required database configuration values are missing.");
            }

            // Build the connection string for PostgreSQL.
            return $"Host={Host};Port={Port};Database={DatabaseName};Username={Username};Password={Password};";
        }

        /// <summary>
        /// Returns a sanitized version of the connection string with the password hidden.
        /// </summary>
        /// <returns>A sanitized connection string.</returns>
        public string SanitizeConnectionString()
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                // If a full connection string is provided, attempt to replace the password portion with '***'.
                // This is a simple replacement. For a robust solution, consider parsing the connection string properly.
                var sanitized = ConnectionString;
                if (!string.IsNullOrEmpty(Password))
                {
                    sanitized = sanitized.Replace(Password, "***");
                }
                return sanitized;
            }

            // Otherwise, build from individual properties with the password replaced by '***'.
            return $"Host={Host};Port={Port};Database={DatabaseName};Username={Username};Password=***;";
        }
    }
}
