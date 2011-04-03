namespace FatCatGit.Configuration
{
    public static class ConfigurationSettings
    {
        private static GlobalConfiguration _global;
        public static GlobalConfiguration Global
        {
            get { return _global ?? Configuration.Instance; }
            set { _global = value; }
        }
    }
}