namespace FatCatGit.Configuration
{
    internal class Configuration : GlobalConfiguration
    {
        public string GitExecutableLocation { get; set; }

        public static Configuration Instance
        {
            get { return Nested.InternalInstance; }
        }

        class Nested
        {
            public static readonly Configuration InternalInstance = new Configuration();
        }
    }
}