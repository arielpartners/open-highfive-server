#region references

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace HighFive.Server.Services.Utils
{
    public class HighFiveException : Exception
    {
        public HighFiveException()
        {
        }

        public HighFiveException(string message)
            : base(message)
        {
        }

        public HighFiveException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
