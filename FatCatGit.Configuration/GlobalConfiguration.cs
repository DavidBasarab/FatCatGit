using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FatCatGit.Configuration
{
    public interface GlobalConfiguration
    {
        string GitExecutableLocation { get; set; }
    }
}
