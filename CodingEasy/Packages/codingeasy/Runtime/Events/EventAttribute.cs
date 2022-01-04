using System;

namespace Events
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EventAttribute: Attribute
    {
        
    }
}