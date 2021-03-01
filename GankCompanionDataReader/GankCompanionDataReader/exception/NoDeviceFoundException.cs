using System;
using System.Collections.Generic;
using System.Text;

namespace GankCompanionDataReader.exception
{
    public class NoDeviceFoundException : Exception
    {
        private static readonly string message = "No devices have been found";
        public NoDeviceFoundException() : base(message)
        {

        }
    }
}
