﻿using System;

namespace CCC.ORM.DataAttributes
{
    /// <summary>
    ///     This attribute tells the Repository that it's associated property resembles a Database Table ID Column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IsIDFieldAttribute : Attribute
    {
        public IsIDFieldAttribute(bool status = true)
        {
            Status = status;
        }

        public bool Status { get; private set; }
    }
}