namespace Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.ConfigureOptionsForType
{
    public class ConfigureOptionsForType : AbstractConfigurationResolverTask
    {
        public override void Execute(ConfigurationResolverArgs args)
        {

            if (args.Result != null)
            {
                args.Result.GetTypeOptions(args.Options);
            }
        }
    }
}
