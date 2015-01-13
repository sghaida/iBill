using System;

namespace CCC.ORM.DataAttributes
{
    /// <summary>
    ///     This attribute tells the Repository that it's associated property is most probably a Table ID Field that is allowed
    ///     to be changed and inserted into the corresponding database table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowIDInsertAttribute : Attribute
    {
        public AllowIDInsertAttribute(bool status = true)
        {
            Status = status;
        }

        public bool Status { get; private set; }
    }
}