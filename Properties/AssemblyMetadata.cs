using System;

namespace BCT.Properties
{
    class AssemblyFingerprint : Attribute
    {
        private readonly string fingerprint = string.Empty;
        public AssemblyFingerprint() : this( string.Empty ) {}

        public AssemblyFingerprint( string s )
        {
            fingerprint = s;
        }
    }
}
