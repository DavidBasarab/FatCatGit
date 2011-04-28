using FatCatGit.Common.Interfaces;

namespace FatCatGit.CommandLineRunner
{
    public class CommandOutput : Output
    {
        public string Output { get; set; }

        public string ErrorOutput { get; set; }
    }
}