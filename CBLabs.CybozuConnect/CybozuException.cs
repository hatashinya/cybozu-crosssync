using System;

namespace CBLabs.CybozuConnect
{
    public class CybozuException : Exception
    {
        public readonly string Code;

        public CybozuException(string message)
            : base(message)
        {
        }

        public CybozuException(string message, string code)
            : base(message)
        {
            Code = code;
        }
    }
}
