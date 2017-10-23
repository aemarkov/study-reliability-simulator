using System;
using System.Runtime.Serialization;

namespace SimCore
{
    [Serializable]
    public class GraphParseException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public GraphParseException()
        {
        }

        public GraphParseException(string message) : base(message)
        {
        }

        public GraphParseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GraphParseException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}