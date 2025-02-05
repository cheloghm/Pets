using System;

namespace Pets.Utilities
{
    public static class DateTimeUtility
    {
        /// <summary>
        /// Gets the current UTC time.
        /// </summary>
        public static DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// Formats a DateTime into a friendly string.
        /// </summary>
        public static string ToFriendlyString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss 'UTC'");
        }
    }
}
