namespace Glass.Mapper.Sc.Razor
{
    /// <summary>
    /// Class ExtensionMethods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Raws the string.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>RawString.</returns>
        public static RawString RawString(this string target)
        {
            return new RawString(target);
        }

    }
}
