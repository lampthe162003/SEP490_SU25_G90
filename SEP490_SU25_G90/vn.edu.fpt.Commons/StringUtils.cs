
namespace SEP490_SU25_G90.vn.edu.fpt.Commons
{
    public static class StringUtils
    {
        internal static object GetFirstName(string? fullName)
        {
            var parts = fullName?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts != null && parts.Length > 0 ? parts[0] : null;
        }

        internal static object GetLastName(string? fullName)
        {
            var parts = fullName?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts != null && parts.Length > 1 ? parts[^1] : null;
        }

        internal static object GetMiddleName(string? fullName)
        {
            var parts = fullName?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts != null && parts.Length > 2 ? string.Join(" ", parts.Skip(1).Take(parts.Length - 2)) : null;
        }

        internal static string Generate6DigitCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
