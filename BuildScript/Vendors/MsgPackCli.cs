using System;
using System.Collections.Generic;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
    class MsgPackCli : ThirdParty
    {
        public MsgPackCli(ProjectFile project, PlatformType platform, Configuration configuration)
            : base(project, platform, configuration)
        {
            project.ReferenceAssembly("MsgPack", @"%(VendorsDir)msgpack/cli/net35-client/MsgPack.dll");
        }
    }
}
