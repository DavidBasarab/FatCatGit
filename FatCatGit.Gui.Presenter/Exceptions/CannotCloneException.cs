using System;
using System.Runtime.Serialization;

namespace FatCatGit.Gui.Presenter.Exceptions
{
    [Serializable]
    public class CannotCloneException : Exception
    {
        public CannotCloneException()
        {
        }

        public CannotCloneException(string message) : base(message)
        {
        }

        public CannotCloneException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CannotCloneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}