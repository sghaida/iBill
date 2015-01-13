using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Lync2013Plugin
{
    public class Enums
    {
        public enum CallMarkerStatus
        {
            [Description("CallMarkerStatus")] TableName,
            [Description("markerId")] MarkerId,
            [Description("phoneCallsTable")] PhoneCallsTable,
            [Description("type")] Type,
            [Description("timestamp")] Timestamp
        }

        /// <summary>
        ///     This Enum is needed to Translate DataReader for the Phone calls importer in the backend
        ///     Please dont delete
        /// </summary>
        public enum PhoneCalls
        {
            [Description("PhoneCallsTableName")] PhoneCallsTableName,
            [Description("SessionIdTime")] SessionIdTime,
            [Description("SessionIdSeq")] SessionIdSeq,
            [Description("ResponseTime")] ResponseTime,
            [Description("SessionEndTime")] SessionEndTime,
            [Description("SourceUserUri")] SourceUserUri,
            [Description("SourceNumberUri")] SourceNumberUri,
            [Description("DestinationUserUri")] DestinationUserUri,
            [Description("DestinationNumberUri")] DestinationNumberUri,
            [Description("FromMediationServer")] FromMediationServer,
            [Description("ToMediationServer")] ToMediationServer,
            [Description("FromGateway")] FromGateway,
            [Description("ToGateway")] ToGateway,
            [Description("SourceUserEdgeServer")] SourceUserEdgeServer,
            [Description("DestinationUserEdgeServer")] DestinationUserEdgeServer,
            [Description("ServerFQDN")] ServerFqdn,
            [Description("PoolFQDN")] PoolFqdn,
            [Description("OnBehalf")] OnBehalf,
            [Description("ReferredBy")] ReferredBy,
            [Description("ChargingParty")] ChargingParty,
            [Description("Duration")] Duration,
            [Description("Marker_CallFrom")] MarkerCallFrom,
            [Description("Marker_CallTo")] MarkerCallTo,
            [Description("Marker_CallToCountry")] MarkerCallToCountry,
            [Description("Marker_CallType")] MarkerCallType,
            [Description("Marker_CallTypeID")] MarkerCallTypeId,
            [Description("Marker_CallCost")] MarkerCallCost,
            [Description("UI_MarkedOn")] UiMarkedOn,
            [Description("UI_UpdatedByUser")] UiUpdatedByUser,
            [Description("UI_AssignedByUser")] UiAssignedByUser,
            [Description("UI_AssignedToUser")] UiAssignedToUser,
            [Description("UI_AssignedOn,")] UiAssignedOn,
            [Description("UI_CallType")] UiCallType,
            [Description("AC_DisputeStatus")] AcDisputeStatus,
            [Description("AC_DisputeResolvedOn")] AcDisputeResolvedOn,
            [Description("AC_IsInvoiced")] AcIsInvoiced,
            [Description("ac_InvoiceDate")] AcInvoiceDate,
            [Description("Exclude")] Exclude,
            [Description("CalleeURI")] CalleeUri
        }

        public string GetDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descAttributes =
                (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof (DescriptionAttribute), false);

            if (descAttributes != null && descAttributes.Length > 0)
                return descAttributes[0].Description;
            return value.ToString();
        }

        public IEnumerable<T> EnumToList<T>()
        {
            var enumType = typeof (T);

            if (enumType.BaseType != typeof (Enum))
                throw new ArgumentException("T is not of System.Enum Type");

            var enumValArray = Enum.GetValues(enumType);
            var enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T) Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
    }
}