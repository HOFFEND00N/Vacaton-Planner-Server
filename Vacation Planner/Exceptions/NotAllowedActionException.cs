using System;

namespace VacationPlanner.Exceptions
{
    public class NotAllowedActionException : Exception
    {
        public NotAllowedActionException(string message) : base(message)
        {
        }
    }
}