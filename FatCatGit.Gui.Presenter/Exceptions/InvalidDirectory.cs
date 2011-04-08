using System;
using System.Runtime.Serialization;

namespace FatCatGit.Gui.Presenter.Exceptions
{
    [Serializable]
    public class InvalidDirectoryException : Exception
    {
        public InvalidDirectoryException()
        {
        }

        public InvalidDirectoryException(string message) : base(message)
        {
        }

        public InvalidDirectoryException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidDirectoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}