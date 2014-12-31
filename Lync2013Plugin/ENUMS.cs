using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lync2013Plugin
{
    public class ENUMS
    {

        /// <summary>
        /// This Enum is needed to Translate DataReader for the Phone calls importer in the backend
        /// Please dont delete
        /// </summary>
        public enum PhoneCalls
        {
            [Description("PhoneCallsTableName")]
            PhoneCallsTableName,
            [Description("SessionIdTime")]
            SessionIdTime,
            [Description("SessionIdSeq")]
            SessionIdSeq,
            [Description("ResponseTime")]
            ResponseTime,
            [Description("SessionEndTime")]
            SessionEndTime,
            [Description("SourceUserUri")]
            SourceUserUri,
            [Description("SourceNumberUri")]
            SourceNumberUri,
            [Description("DestinationUserUri")]
            DestinationUserUri,
            [Description("DestinationNumberUri")]
            DestinationNumberUri,
            [Description("FromMediationServer")]
            FromMediationServer,
            [Description("ToMediationServer")]
            ToMediationServer,
            [Description("FromGateway")]
            FromGateway,
            [Description("ToGateway")]
            ToGateway,
            [Description("SourceUserEdgeServer")]
            SourceUserEdgeServer,
            [Description("DestinationUserEdgeServer")]
            DestinationUserEdgeServer,
            [Description("ServerFQDN")]
            ServerFQDN,
            [Description("PoolFQDN")]
            PoolFQDN,
            [Description("OnBehalf")]
            OnBehalf,
            [Description("ReferredBy")]
            ReferredBy,
            [Description("ChargingParty")]
            ChargingParty,
            [Description("Duration")]
            Duration,
            [Description("Marker_CallFrom")]
            Marker_CallFrom,
            [Description("Marker_CallTo")]
            Marker_CallTo,
            [Description("Marker_CallToCountry")]
            Marker_CallToCountry,
            [Description("Marker_CallType")]
            Marker_CallType,
            [Description("Marker_CallTypeID")]
            Marker_CallTypeID,
            [Description("Marker_CallCost")]
            Marker_CallCost,
            [Description("UI_MarkedOn")]
            UI_MarkedOn,
            [Description("UI_UpdatedByUser")]
            UI_UpdatedByUser,
            [Description("UI_AssignedByUser")]
            UI_AssignedByUser,
            [Description("UI_AssignedToUser")]
            UI_AssignedToUser,
            [Description("UI_AssignedOn,")]
            UI_AssignedOn,
            [Description("UI_CallType")]
            UI_CallType,
            [Description("AC_DisputeStatus")]
            AC_DisputeStatus,
            [Description("AC_DisputeResolvedOn")]
            AC_DisputeResolvedOn,
            [Description("AC_IsInvoiced")]
            AC_IsInvoiced,
            [Description("ac_InvoiceDate")]
            AC_InvoiceDate,
            [Description("Exclude")]
            Exclude,
            [Description("CalleeURI")]
            CalleeURI

        }

        public enum CallMarkerStatus
        {
            [Description("CallMarkerStatus")]
            TableName,
            [Description("markerId")]
            MarkerId,
            [Description("phoneCallsTable")]
            PhoneCallsTable,
            [Description("type")]
            Type,
            [Description("timestamp")]
            Timestamp
        }

        public string GetDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] descAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descAttributes != null && descAttributes.Length > 0)
                return descAttributes[0].Description;
            else
                return value.ToString();
        }

        public IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T is not of System.Enum Type");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

    }
}
