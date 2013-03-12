namespace Glass.Mapper.Sc.Razor
{
    public static class ExtensionMethods
    {
        public static RawString RawString(this string target)
        {
            return new RawString(target);
        }

    }
}
