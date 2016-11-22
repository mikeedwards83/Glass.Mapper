namespace Glass.Mapper.Diagnostics
{
    public class ModelCounter
    {
        public int ProxyModelsCreated { get; set; }
        public int ModelsRequested { get; set; }
        public int ModelsMapped { get; set; }
        public int CachedModels { get; set; }
        public int ConcreteModelCreated { get; set; }

        public void Reset()
        {
            ProxyModelsCreated = 0;
            ModelsMapped = 0;
            ModelsRequested = 0;
            CachedModels = 0;
            ConcreteModelCreated = 0;
        }
    }
}
