using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class PhoneCallExclusionsDataMapper : DataAccess<PhoneCallExclusion>
    {
        /// <summary>
        /// 
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
                            item.AutoMark = this.LookUpAutoMark(item.AutoMark);
                            item.ZeroCost = this.LookUpZeroCost(item.ZeroCost);
                            item.EntityType = this.LookUpExclusionType(item.EntityType);
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
        /// 
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        private string LookUpExclusionType(string exceptionType)
        {
            switch(exceptionType)
            {
                case "S": return "Source";
                case "D": return "Destination";
                default: return "N/A";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zeroCost"></param>
        /// <returns></returns>
        private string LookUpZeroCost(string zeroCost)
        {
            switch(zeroCost)
            {
                case "Y": return "Yes";
                case "N": return "No";
                default: return "N/A";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="autoMarkType"></param>
        /// <returns></returns>
        private string LookUpAutoMark(string autoMarkType)
        {
            switch(autoMarkType)
            {
                case "B": return "Business";
                case "P": return "Personal";
                default: return "DISABLED";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneCallExclusionBodyText"></param>
        /// <returns></returns>
        private string CleanTheExceptionBody(string phoneCallExclusionBodyText)
        {
            string cleanedupVersion = string.Empty;
            char[] unhealthyCharacters = new char['+'];

            if (!string.IsNullOrEmpty(phoneCallExclusionBodyText))
            {
                cleanedupVersion = phoneCallExclusionBodyText.Trim().Trim('+');

                if (cleanedupVersion.StartsWith("0")) //Also holds when it starts with "00" !!
                    cleanedupVersion = Convert.ToString(Convert.ToInt64(cleanedupVersion));
            }

            return cleanedupVersion;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteID"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="SiteID"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="SiteID"></param>
        /// <returns></returns>
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


        public override PhoneCallExclusion GetById(long id, string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default, bool IncludeDataRelations = true)
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


        public override IEnumerable<PhoneCallExclusion> Get(System.Linq.Expressions.Expression<Func<PhoneCallExclusion, bool>> predicate, string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default, bool IncludeDataRelations = true)
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


        public override IEnumerable<PhoneCallExclusion> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default, bool IncludeDataRelations = true)
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


        public override IEnumerable<PhoneCallExclusion> GetAll(string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default, bool IncludeDataRelations = true)
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
