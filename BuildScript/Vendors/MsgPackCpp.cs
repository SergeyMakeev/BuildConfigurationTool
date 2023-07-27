using System;
using System.Collections.Generic;
using BCT.Source.Model;


namespace BCT.BuildScript.Vendors
{
    public class MsgPackCpp : ThirdParty
    {
        public MsgPackCpp(ProjectFile project, PlatformType platform, Configuration configuration)
            : base(project, platform, configuration)
        {
            project.IncludePath("%(VendorsDir)msgpack/cpp/include");
        }
    }
}
