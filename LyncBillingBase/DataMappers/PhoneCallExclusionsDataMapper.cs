using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.Helpers;
using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class PhoneCallExclusionsDataMapper : DataAccess<PhoneCallExclusion>
    {
        /// <summary>
        /// This function is responsible for translating the database falgs of the properties: EntityType, ZeroCost and AutoMark into readable strings.
        /// It is called from every overriden Get function.
        /// The values get mapped from and to a set of GLOBAL values, they can be found in the GLOBALS class under PhoneCallExclusion.
        /// </summary>
        /// <param name="exclusions"></param>
        private void MapDataToReadable(ref List<PhoneCallExclusion> exclusions)
        {
            try
            {
                exclusions = exclusions
                        .Select<PhoneCallExclusion, PhoneCallExclusion>
                        (item =>
                        {
                            item.ExclusionType = this.LookUpExclusionType(item.ExclusionType);
                            item.AutoMark = this.LookUpAutoMark(item.AutoMark);
                            item.ZeroCost = this.LookUpZeroCost(item.ZeroCost);
                            return item;
                        })
                        .ToList<PhoneCallExclusion>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Map the single-character flag to a readable string.
        /// For example: Map "S" to "Source", and "D" to "Destination".
        /// These values are defined in the  GLOBALS.PhoneCallExclusion.Type enum
        /// </summary>
        /// <param name="exclusionType">one character string</param>
        /// <returns>string</returns>
        private string LookUpExclusionType(string exclusionType)
        {
            var DEFAULT = GLOBALS.PhoneCallExclusion.Type.Default;
            var SOURCE = GLOBALS.PhoneCallExclusion.Type.Source;
            var DESTINATION = GLOBALS.PhoneCallExclusion.Type.Destination;

            if(false == string.IsNullOrEmpty(exclusionType))
            {
                if(exclusionType == SOURCE.Value())
                {
                    return SOURCE.Description();
                }
                else if (exclusionType == DESTINATION.Value())
                {
                    return DESTINATION.Description();
                }
            }

            return DEFAULT.Description();
        }

        /// <summary>
        /// Map the single-character flag to a readable string.
        /// For example: Map "Y" to "Yes", and "N" to "No".
        /// These values are defined in the  GLOBALS.PhoneCallExclusion.ZeroCost enum
        /// </summary>
        /// <param name="zeroCost">one character string</param>
        /// <returns>string</returns>
        private string LookUpZeroCost(string zeroCost)
        {
            var DEFAULT = GLOBALS.PhoneCallExclusion.ZeroCost.Default;
            var YES = GLOBALS.PhoneCallExclusion.ZeroCost.Yes;
            var NO = GLOBALS.PhoneCallExclusion.ZeroCost.No;

            if(false == string.IsNullOrEmpty(zeroCost))
            {
                if(zeroCost == YES.Value())
                {
                    return YES.Description();
                }
                else if(zeroCost == NO.Value())
                {
                    return NO.Description();
                }
            }

            return DEFAULT.Description();
        }

        /// <summary>
        /// Map the single-character flag to a readable string.
        /// For example: Map "B" to "Business", and "P" to "Personal".
        /// These values are defined in the  GLOBALS.PhoneCallExclusion.AutoMark enum
        /// </summary>
        /// <param name="autoMarkType">one character string</param>
        /// <returns>string</returns>
        private string LookUpAutoMark(string autoMarkType)
        {
            var DEFAULT = GLOBALS.PhoneCallExclusion.AutoMark.Default;
            var BUSINESS = GLOBALS.PhoneCallExclusion.AutoMark.Business;
            var PERSONAL = GLOBALS.PhoneCallExclusion.AutoMark.Personal;

            if(false == string.IsNullOrEmpty(autoMarkType))
            {
                if(autoMarkType == BUSINESS.Value())
                {
                    return BUSINESS.Description();
                }
                else if(autoMarkType == PERSONAL.Value())
                {
                    return PERSONAL.Description();
                }
            }
            
            return DEFAULT.Description();
        }

        /// <summary>
        /// Given an exclusion subject, which could be a Phone-Number or a SipAccount, clean it from the leading '+' character, if it exists.
        /// </summary>
        /// <param name="exclusionSubject">Exclusion subject string</param>
        /// <returns>Cleaned version of the same exclusion subject string</returns>
        private string CleanExclusionSubject(string exclusionSubject)
        {
            string cleanedupVersion = string.Empty;
            char[] unhealthyCharacters = new char['+'];

            if (!string.IsNullOrEmpty(exclusionSubject))
            {
                cleanedupVersion = exclusionSubject.Trim().Trim('+');

                if (cleanedupVersion.StartsWith("0")) //Also holds when it starts with "00" !!
                    cleanedupVersion = Convert.ToString(Convert.ToInt64(cleanedupVersion));
            }

            return cleanedupVersion;
        }


        /// <summary>
        /// Given a Site's ID, return all it's exclusions.
        /// </summary>
        /// <param name="SiteID">Site.ID (int)</param>
        /// <returns>List of PhoneCallExclusion objects</returns>
        public List<PhoneCallExclusion> GetBySiteID(int SiteID)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("SiteID", SiteID);

            try
            {
                return this.Get(whereConditions: condition, limit: 0).ToList<PhoneCallExclusion>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Site's ID, return all it's exclusions that are defined of the type "SOURCE".
        /// </summary>
        /// <param name="SiteID">Site.ID (int)</param>
        /// <returns>List of PhoneCallExclusion objects</returns>
        public List<PhoneCallExclusion> GetSourcesBySiteID(int SiteID)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("SiteID", SiteID);
            condition.Add("EntityType", "S");

            try
            {
                return this.Get(whereConditions: condition, limit: 0).ToList<PhoneCallExclusion>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Site's ID, return all it's exclusions that are defined of the type "DESTINATON".
        /// </summary>
        /// <param name="SiteID">Site.ID (int)</param>
        /// <returns>List of PhoneCallExclusion objects</returns>
        public List<PhoneCallExclusion> GetDestinationsBySiteID(int SiteID)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("SiteID", SiteID);
            condition.Add("EntityType", "D");

            try
            {
                return this.Get(whereConditions: condition, limit: 0).ToList<PhoneCallExclusion>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override int Insert(PhoneCallExclusion newExclusionObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            // NULL value check
            if(null == newExclusionObject)
            {
                throw new Exception("PhoneCallExclusions#Inser: Cannot insert null objects.");
            }

            try
            {
                newExclusionObject.ExclusionSubject = CleanExclusionSubject(newExclusionObject.ExclusionSubject);

                return base.Insert(newExclusionObject, dataSourceName, dataSource);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override bool Update(PhoneCallExclusion existingExclusionObject, Dictionary<string, object> whereConditions = null, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            // NULL value check
            if(null == existingExclusionObject)
            {
                throw new Exception("PhoneCallExclusions#Update: Cannot update NULL phone call exclusion objects.");
            }

            try
            {
                existingExclusionObject.ExclusionSubject = CleanExclusionSubject(existingExclusionObject.ExclusionSubject);

                return base.Update(dataObject: existingExclusionObject, whereConditions: whereConditions, dataSourceName: dataSourceName, dataSourceType: dataSource);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override PhoneCallExclusion GetById(long id, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            PhoneCallExclusion exclusion = null;

            try
            {
                exclusion = base.GetById(id, dataSourceName, dataSource, IncludeDataRelations);

                if(exclusion != null)
                {
                    var temporaryList = new List<PhoneCallExclusion>() { exclusion };
                    MapDataToReadable(ref temporaryList);
                    exclusion = temporaryList.First();
                }

                return exclusion;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<PhoneCallExclusion> Get(System.Linq.Expressions.Expression<Func<PhoneCallExclusion, bool>> predicate, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            List<PhoneCallExclusion> exclusions = null;

            try
            { 
                exclusions = base.Get(predicate, dataSourceName, dataSource, IncludeDataRelations).ToList<PhoneCallExclusion>();

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


        public override IEnumerable<PhoneCallExclusion> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            List<PhoneCallExclusion> exclusions = null;

            try
            { 
                exclusions = base.Get(whereConditions, limit, dataSourceName, dataSource, IncludeDataRelations).ToList<PhoneCallExclusion>();

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


        public override IEnumerable<PhoneCallExclusion> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            List<PhoneCallExclusion> exclusions = null;

            try
            { 
                exclusions = base.GetAll(dataSourceName, dataSource, IncludeDataRelations).ToList<PhoneCallExclusion>();

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


        public override IEnumerable<PhoneCallExclusion> GetAll(string sql)
        {
            List<PhoneCallExclusion> exclusions = null;

            try
            { 
                exclusions = base.GetAll(sql).ToList<PhoneCallExclusion>();

                if(exclusions != null && exclusions.Count > 0)
                {
                    MapDataToReadable(ref exclusions);
                }

                return exclusions;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }

}
