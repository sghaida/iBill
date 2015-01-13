using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class PhoneCallExclusionsDataMapper : DataAccess<PhoneCallExclusion>
    {
        /// <summary>
        ///     This function is responsible for translating the database falgs of the properties: EntityType, ZeroCost and
        ///     AutoMark into readable strings.
        ///     It is called from every overriden Get function.
        ///     The values get mapped from and to a set of GLOBAL values, they can be found in the GLOBALS class under
        ///     PhoneCallExclusion.
        /// </summary>
        /// <param name="exclusions"></param>
        private void MapDataToReadable(ref List<PhoneCallExclusion> exclusions)
        {
            try
            {
                exclusions = exclusions
                    .Select
                    (item =>
                    {
                        item.ExclusionType = LookUpExclusionType(item.ExclusionType);
                        item.AutoMark = LookUpAutoMark(item.AutoMark);
                        item.ZeroCost = LookUpZeroCost(item.ZeroCost);
                        return item;
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Map the single-character flag to a readable string.
        ///     For example: Map "S" to "Source", and "D" to "Destination".
        ///     These values are defined in the  GLOBALS.PhoneCallExclusion.Type enum
        /// </summary>
        /// <param name="exclusionType">one character string</param>
        /// <returns>string</returns>
        private string LookUpExclusionType(string exclusionType, bool reverseLookup = false)
        {
            var DEFAULT = Globals.PhoneCallExclusion.Type.Default;
            var source = Globals.PhoneCallExclusion.Type.Source;
            var destination = Globals.PhoneCallExclusion.Type.Destination;

            if (false == reverseLookup)
            {
                if (false == string.IsNullOrEmpty(exclusionType))
                {
                    if (exclusionType == source.Value())
                    {
                        return source.Description();
                    }
                    if (exclusionType == destination.Value())
                    {
                        return destination.Description();
                    }
                }

                return DEFAULT.Description();
            }
            if (false == string.IsNullOrEmpty(exclusionType))
            {
                if (exclusionType == source.Description())
                {
                    return source.Value();
                }
                if (exclusionType == destination.Description())
                {
                    return destination.Value();
                }
            }

            return source.Value();
        }

        /// <summary>
        ///     Map the single-character flag to a readable string.
        ///     For example: Map "Y" to "Yes", and "N" to "No".
        ///     These values are defined in the  GLOBALS.PhoneCallExclusion.ZeroCost enum
        /// </summary>
        /// <param name="zeroCost">one character string</param>
        /// <returns>string</returns>
        private string LookUpZeroCost(string zeroCost, bool reverseLookup = false)
        {
            var DEFAULT = Globals.PhoneCallExclusion.ZeroCost.Default;
            var yes = Globals.PhoneCallExclusion.ZeroCost.Yes;
            var no = Globals.PhoneCallExclusion.ZeroCost.No;

            if (false == reverseLookup)
            {
                if (false == string.IsNullOrEmpty(zeroCost))
                {
                    if (zeroCost == yes.Value())
                    {
                        return yes.Description();
                    }
                    if (zeroCost == no.Value())
                    {
                        return no.Description();
                    }
                }

                return DEFAULT.Description();
            }
            if (false == string.IsNullOrEmpty(zeroCost))
            {
                if (zeroCost == yes.Description())
                {
                    return yes.Value();
                }
                if (zeroCost == no.Description())
                {
                    return no.Value();
                }
            }

            return no.Value();
        }

        /// <summary>
        ///     Map the single-character flag to a readable string.
        ///     For example: Map "B" to "Business", and "P" to "Personal".
        ///     These values are defined in the  GLOBALS.PhoneCallExclusion.AutoMark enum
        /// </summary>
        /// <param name="autoMarkType">one character string</param>
        /// <returns>string</returns>
        private string LookUpAutoMark(string autoMarkType, bool reverseLookup = false)
        {
            var DEFAULT = Globals.PhoneCallExclusion.AutoMark.Default;
            var business = Globals.PhoneCallExclusion.AutoMark.Business;
            var personal = Globals.PhoneCallExclusion.AutoMark.Personal;

            if (false == reverseLookup)
            {
                if (false == string.IsNullOrEmpty(autoMarkType))
                {
                    if (autoMarkType == business.Value())
                    {
                        return business.Description();
                    }
                    if (autoMarkType == personal.Value())
                    {
                        return personal.Description();
                    }
                }

                return DEFAULT.Description();
            }
            if (false == string.IsNullOrEmpty(autoMarkType))
            {
                if (autoMarkType == business.Description())
                {
                    return business.Value();
                }
                if (autoMarkType == personal.Description())
                {
                    return personal.Value();
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Given an exclusion subject, which could be a Phone-Number or a SipAccount, clean it from the leading '+' character,
        ///     if it exists.
        /// </summary>
        /// <param name="exclusionSubject">Exclusion subject string</param>
        /// <returns>Cleaned version of the same exclusion subject string</returns>
        private string CleanExclusionSubject(string exclusionSubject)
        {
            var cleanedupVersion = string.Empty;
            var unhealthyCharacters = new char['+'];

            if (!string.IsNullOrEmpty(exclusionSubject))
            {
                cleanedupVersion = exclusionSubject.Trim().Trim('+');

                if (cleanedupVersion.StartsWith("0")) //Also holds when it starts with "00" !!
                    cleanedupVersion = Convert.ToString(Convert.ToInt64(cleanedupVersion));
            }

            return cleanedupVersion;
        }

        /// <summary>
        ///     Given a Site's ID, return all it's exclusions.
        /// </summary>
        /// <param name="siteId">Site.ID (int)</param>
        /// <returns>List of PhoneCallExclusion objects</returns>
        public List<PhoneCallExclusion> GetBySiteId(int siteId)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("SiteID", siteId);

            try
            {
                return Get(condition, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Site's ID, return all it's exclusions that are defined of the type "SOURCE".
        /// </summary>
        /// <param name="siteId">Site.ID (int)</param>
        /// <returns>List of PhoneCallExclusion objects</returns>
        public List<PhoneCallExclusion> GetSourcesBySiteId(int siteId)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("SiteID", siteId);
            condition.Add("EntityType", "S");

            try
            {
                return Get(condition, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Site's ID, return all it's exclusions that are defined of the type "DESTINATON".
        /// </summary>
        /// <param name="siteId">Site.ID (int)</param>
        /// <returns>List of PhoneCallExclusion objects</returns>
        public List<PhoneCallExclusion> GetDestinationsBySiteId(int siteId)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("SiteID", siteId);
            condition.Add("EntityType", "D");

            try
            {
                return Get(condition, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override int Insert(PhoneCallExclusion newExclusionObject, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            // NULL value check
            if (null == newExclusionObject)
            {
                throw new Exception("PhoneCallExclusions#Inser: Cannot insert null objects.");
            }

            try
            {
                newExclusionObject.ExclusionSubject = CleanExclusionSubject(newExclusionObject.ExclusionSubject);

                newExclusionObject.ExclusionType = LookUpExclusionType(newExclusionObject.ExclusionType, true);
                newExclusionObject.AutoMark = LookUpAutoMark(newExclusionObject.AutoMark, true);
                newExclusionObject.ZeroCost = LookUpZeroCost(newExclusionObject.ZeroCost, true);

                return base.Insert(newExclusionObject, dataSourceName, dataSource);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override bool Update(PhoneCallExclusion existingExclusionObject, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            // NULL value check
            if (null == existingExclusionObject)
            {
                throw new Exception("PhoneCallExclusions#Update: Cannot update NULL phone call exclusion objects.");
            }

            try
            {
                existingExclusionObject.ExclusionSubject =
                    CleanExclusionSubject(existingExclusionObject.ExclusionSubject);

                existingExclusionObject.ExclusionType = LookUpExclusionType(existingExclusionObject.ExclusionType, true);
                existingExclusionObject.AutoMark = LookUpAutoMark(existingExclusionObject.AutoMark, true);
                existingExclusionObject.ZeroCost = LookUpZeroCost(existingExclusionObject.ZeroCost, true);

                return base.Update(existingExclusionObject, dataSourceName, dataSource);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override PhoneCallExclusion GetById(long id, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            PhoneCallExclusion exclusion = null;

            try
            {
                exclusion = base.GetById(id, dataSourceName, dataSource);

                if (exclusion != null)
                {
                    var temporaryList = new List<PhoneCallExclusion> {exclusion};
                    MapDataToReadable(ref temporaryList);
                    exclusion = temporaryList.First();
                }

                return exclusion;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<PhoneCallExclusion> Get(Expression<Func<PhoneCallExclusion, bool>> predicate,
            string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            List<PhoneCallExclusion> exclusions = null;

            try
            {
                exclusions = base.Get(predicate, dataSourceName, dataSource).ToList();

                if (exclusions != null && exclusions.Count > 0)
                {
                    MapDataToReadable(ref exclusions);
                }

                return exclusions;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<PhoneCallExclusion> Get(Dictionary<string, object> whereConditions, int limit = 25,
            string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            List<PhoneCallExclusion> exclusions = null;

            try
            {
                exclusions = base.Get(whereConditions, limit, dataSourceName, dataSource).ToList();

                if (exclusions != null && exclusions.Count > 0)
                {
                    MapDataToReadable(ref exclusions);
                }

                return exclusions;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<PhoneCallExclusion> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            List<PhoneCallExclusion> exclusions = null;

            try
            {
                exclusions = base.GetAll(dataSourceName, dataSource).ToList();

                if (exclusions != null && exclusions.Count > 0)
                {
                    MapDataToReadable(ref exclusions);
                }

                return exclusions;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<PhoneCallExclusion> GetAll(string sqlQuery)
        {
            List<PhoneCallExclusion> exclusions = null;

            try
            {
                exclusions = base.GetAll(sqlQuery).ToList();

                if (exclusions != null && exclusions.Count > 0)
                {
                    MapDataToReadable(ref exclusions);
                }

                return exclusions;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}