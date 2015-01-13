using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text.RegularExpressions;
using Lync2013Plugin.Interfaces;
using LyncBillingBase.DataModels;

namespace Lync2013Plugin.Implementation
{
    internal class PhoneCallsImpl : IPhoneCall
    {
        private readonly Helpers _phoneCallHelper = new Helpers();

        public PhoneCall SetCallType(PhoneCall phoneCall)
        {
            string srcCountry;
            string dstCountry;
            string srcCallType;
            string dstCallType;
            string srcDiDdsc;
            string dstDiDdsc;

            long ngnDialingCode;
            string ngnCallType;
            string ngnDstCountry;

            //var destinationNumberLeadingChars = new char[2] {'0', '0'};

            var thisCall = phoneCall;

            _phoneCallHelper.MatchDid(thisCall.SourceNumberUri, out srcDiDdsc, thisCall.SourceUserUri);
            _phoneCallHelper.MatchDid(thisCall.DestinationNumberUri, out dstDiDdsc, thisCall.DestinationUserUri);

            //Set SourceNumberDialing Prefix and source country
            thisCall.MarkerCallFrom =
                _phoneCallHelper.GetDialingPrefixInfo(_phoneCallHelper.FixNumberType(thisCall.SourceNumberUri),
                    out srcCallType, out srcCountry, sipAccount: thisCall.ChargingParty, did: srcDiDdsc);

            //Set DestinationNumber Dialing Prefix and destination country
            thisCall.MarkerCallTo =
                _phoneCallHelper.GetDialingPrefixInfo(_phoneCallHelper.FixNumberType(thisCall.DestinationNumberUri),
                    out dstCallType, out dstCountry, thisCall.ToGateway, thisCall.DestinationUserUri, dstDiDdsc);


            //Handle the phoneCall in case it is an NGN
            var isNgnFlag = _phoneCallHelper.GetNgnDialingInfo(thisCall.DestinationNumberUri, srcCountry,
                out ngnDialingCode, out ngnDstCountry, out ngnCallType);

            if (isNgnFlag)
            {
                dstCallType = ngnCallType;
                dstCountry = ngnDstCountry;
                thisCall.MarkerCallTo = ngnDialingCode;
            }


            //Assign the updated global variables to the phonecall object
            thisCall.MarkerCallType = dstCallType;
            thisCall.MarkerCallToCountry = dstCountry;

            //Incoming Call
            if (string.IsNullOrEmpty(thisCall.SourceUserUri) || !_phoneCallHelper.IsValidEmail(thisCall.SourceUserUri))
            {
                thisCall.MarkerCallType = "INCOMING-CALL";
                thisCall.MarkerCallTypeId = Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                return thisCall;
            }

            //Voice Mail
            if (thisCall.SourceUserUri == thisCall.DestinationUserUri ||
                thisCall.SourceNumberUri == thisCall.DestinationNumberUri)
            {
                thisCall.MarkerCallType = "VOICE-MAIL";
                thisCall.MarkerCallTypeId = Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                return thisCall;
            }

            //Check if the call is lync to lync or site to site or lync call accros the site
            if (!string.IsNullOrEmpty(dstDiDdsc))
            {
               
                //IF yes put the source site from activedirectoryUsers table instead of the soyrce did site

                if (!string.IsNullOrEmpty(srcDiDdsc))
                    thisCall.MarkerCallType = srcDiDdsc + "-TO-" + dstDiDdsc;
                else
                    thisCall.MarkerCallType = "UNKNOWN-TO-" + dstDiDdsc;

                thisCall.MarkerCallTypeId = Repo.CallTypes.Find(type => type.Name == "SITE-TO-SITE").TypeId;

                thisCall.MarkerCallTo = 0;
                thisCall.MarkerCallToCountry = null;

                return thisCall;
            }

            //FAIL SAFE for LYNC TO LYNC CALLS
            if (string.IsNullOrEmpty(thisCall.FromGateway) && string.IsNullOrEmpty(thisCall.ToGateway) &&
                string.IsNullOrEmpty(thisCall.FromMediationServer) && string.IsNullOrEmpty(thisCall.ToMediationServer))
            {
                thisCall.MarkerCallType = "LYNC-TO-LYNC";
                thisCall.MarkerCallTypeId = Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                thisCall.MarkerCallTo = 0;
                thisCall.MarkerCallToCountry = null;

                return thisCall;
            }

            //Check if the call is went through gateway which means national or international call
            // To Gateway or to mediation server should be set to be able to be able to consider this call as external
            // There is a bug some calls went ththrough pst but the gateways is null (NADER TO INVISTAGATE) : we could apply default rates for those calls
            if (!string.IsNullOrEmpty(thisCall.SourceUserUri) &&
                (!string.IsNullOrEmpty(thisCall.ToGateway) || !string.IsNullOrEmpty(thisCall.ToMediationServer)) &&
                !string.IsNullOrEmpty(thisCall.DestinationNumberUri))
            {
                if (srcCountry == dstCountry)
                {
                    if (!string.IsNullOrEmpty(dstDiDdsc))
                    {
                        if (!string.IsNullOrEmpty(srcDiDdsc))
                            thisCall.MarkerCallType = srcDiDdsc + "-TO-" + dstDiDdsc;
                        else
                            thisCall.MarkerCallType = "TO-" + dstDiDdsc;

                        thisCall.MarkerCallTypeId = Repo.CallTypes.Find(type => type.Name == "SITE-TO-SITE").TypeId;

                        thisCall.MarkerCallTo = 0;
                        thisCall.MarkerCallToCountry = null;

                        return thisCall;
                    }

                    if (dstCallType == "fixedline")
                    {
                        thisCall.MarkerCallType = "NATIONAL-FIXEDLINE";
                        thisCall.MarkerCallTypeId =
                            Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                        return _phoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    if (dstCallType == "gsm")
                    {
                        thisCall.MarkerCallType = "NATIONAL-MOBILE";
                        thisCall.MarkerCallTypeId =
                            Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                        return _phoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    if (dstCallType == "NGN")
                    {
                        thisCall.MarkerCallType = "NGN";
                        thisCall.MarkerCallTypeId =
                            Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                        return _phoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    if (dstCallType == "TOLL-FREE")
                    {
                        thisCall.MarkerCallType = "TOLL-FREE";
                        thisCall.MarkerCallTypeId =
                            Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                        return _phoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    if (dstCallType == "PUSH-TO-TALK")
                    {
                        thisCall.MarkerCallType = "PUSH-TO-TALK";
                        thisCall.MarkerCallTypeId =
                            Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                        return _phoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    thisCall.MarkerCallType = "NATIONAL-FIXEDLINE";
                    thisCall.MarkerCallTypeId =
                        Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                    return _phoneCallHelper.UpdateChargingPartyField(thisCall);
                }

                if (!string.IsNullOrEmpty(dstDiDdsc))
                {
                    if (!string.IsNullOrEmpty(srcDiDdsc))
                        thisCall.MarkerCallType = srcDiDdsc + "-TO-" + dstDiDdsc;
                    else
                        thisCall.MarkerCallType = "TO-" + dstDiDdsc;

                    thisCall.MarkerCallTypeId = Repo.CallTypes.Find(type => type.Name == "SITE-TO-SITE").TypeId;

                    thisCall.MarkerCallTo = 0;
                    thisCall.MarkerCallToCountry = null;

                    return thisCall;
                }

                if (dstCallType == "fixedline")
                {
                    thisCall.MarkerCallType = "INTERNATIONAL-FIXEDLINE";
                    thisCall.MarkerCallTypeId =
                        Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                    return _phoneCallHelper.UpdateChargingPartyField(thisCall);
                }
                if (dstCallType == "gsm")
                {
                    thisCall.MarkerCallType = "INTERNATIONAL-MOBILE";
                    thisCall.MarkerCallTypeId =
                        Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                    return _phoneCallHelper.UpdateChargingPartyField(thisCall);
                }
                if (dstCallType == "NGN")
                {
                    thisCall.MarkerCallType = "NGN";
                    thisCall.MarkerCallTypeId =
                        Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                    return _phoneCallHelper.UpdateChargingPartyField(thisCall);
                }
                if (dstCallType == "TOLL-FREE")
                {
                    thisCall.MarkerCallType = "TOLL-FREE";
                    thisCall.MarkerCallTypeId =
                        Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                    return _phoneCallHelper.UpdateChargingPartyField(thisCall);
                }
                if (dstCallType == "PUSH-TO-TALK")
                {
                    thisCall.MarkerCallType = "PUSH-TO-TALK";
                    thisCall.MarkerCallTypeId =
                        Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                    return _phoneCallHelper.UpdateChargingPartyField(thisCall);
                }
                thisCall.MarkerCallType = "INTERNATIONAL-FIXEDLINE";
                thisCall.MarkerCallTypeId = Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                return _phoneCallHelper.UpdateChargingPartyField(thisCall);
            }


            // Handle the sourceNumber uri sip account and the destination uri is a sip account also 
            // For the calls which doesnt have source number uri or destination number uri to bable to identify which site
            if (!string.IsNullOrEmpty(thisCall.SourceUserUri) &&
                !string.IsNullOrEmpty(thisCall.DestinationUserUri) &&
                _phoneCallHelper.IsValidEmail(thisCall.DestinationUserUri))
            {
                if (_phoneCallHelper.IsImEmail(thisCall.DestinationUserUri))
                {
                    thisCall.MarkerCallType = "LYNC-TO-IM";
                    thisCall.MarkerCallTypeId =
                        Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                    thisCall.MarkerCallTo = 0;
                    thisCall.MarkerCallToCountry = null;
                }
                else
                {
                    thisCall.MarkerCallType = "LYNC-TO-LYNC";
                    thisCall.MarkerCallTypeId =
                        Repo.CallTypes.Find(type => type.Name == thisCall.MarkerCallType).TypeId;

                    thisCall.MarkerCallTo = 0;
                    thisCall.MarkerCallToCountry = null;
                }

                return thisCall;
            }

            thisCall.MarkerCallType = "N/A";
            thisCall.MarkerCallTypeId = 0;

            return thisCall;
        }

        public PhoneCall ApplyRate(PhoneCall thisCall)
        {
            var markerCallToCountry = thisCall.MarkerCallToCountry;
            var destinationNumberUri = thisCall.DestinationNumberUri;
            var toGateway = thisCall.ToGateway;


            if (!string.IsNullOrEmpty(toGateway))
            {
                // Check if we can apply the rates for this phone-call
                var gateway = Repo.Gateways.Find(g => g.Name == toGateway);

                if (gateway != null)
                {
                    var rates =
                        (from keyValuePair in Repo.RatesPerGatway
                            where keyValuePair.Key == gateway.Id
                            select keyValuePair.Value).SingleOrDefault<List<RatesInternational>>() ??
                        (new List<RatesInternational>());

                    var ngnRates =
                        (from keyValuePair in Repo.NgnRatesPerGateway
                            where keyValuePair.Key == gateway.Id
                            select keyValuePair.Value).SingleOrDefault<List<RateForNgn>>() ?? (new List<RateForNgn>());

                    if (Repo.ListofChargeableCallTypes.Contains(Convert.ToInt32(thisCall.MarkerCallTypeId)) &&
                        (ngnRates.Count > 0 || rates.Count > 0))
                    {
                        //Apply the rate for this phone call

                        var rate =
                            (from r in rates where r.Iso3CountryCode == markerCallToCountry select r)
                                .SingleOrDefault<RatesInternational>();

                        var ngnRate = (from r in ngnRates
                            where
                                r.NumberingPlanForNgn.Iso3CountryCode == markerCallToCountry &&
                                Regex.IsMatch(destinationNumberUri.Trim('+'), r.NumberingPlanForNgn.DialingCode)
                            select r).SingleOrDefault<RateForNgn>();

                        //if the call is of type national/international MOBILE then apply the Mobile-Rate, otherwise apply the Fixedline-Rate
                        if (ngnRate != null && Repo.ListOfNgniDs.Contains(Convert.ToInt32(thisCall.MarkerCallTypeId)))
                        {
                            thisCall.MarkerCallCost = Math.Ceiling(Convert.ToDecimal(thisCall.Duration)/60)*
                                                       ngnRate.Rate;
                        }
                        else if (rate != null)
                        {
                            var sourceCountryCodeIso3 = "N/A";

                            if (thisCall.MarkerCallFrom > 0)
                            {
                                var numPlan =
                                    Repo.NumberingPlan.AsParallel()
                                        .First(item => item.DialingPrefix == thisCall.MarkerCallFrom);

                                if (numPlan != null)
                                    sourceCountryCodeIso3 = numPlan.Iso3CountryCode;
                            }

                            //In case it's a national call
                            if (sourceCountryCodeIso3 == thisCall.MarkerCallToCountry)
                            {
                                var nationalRates = Repo.GetNationalRates(gateway, sourceCountryCodeIso3);

                                var matchedRate = nationalRates.Find(item => item.DialingCode == thisCall.MarkerCallTo);

                                if (matchedRate != null && matchedRate.Rate != 0)
                                {
                                    thisCall.MarkerCallCost = Math.Ceiling(Convert.ToDecimal(thisCall.Duration)/60)*
                                                               matchedRate.Rate;
                                }
                                else
                                {
                                    //Fail Safe
                                    thisCall.MarkerCallCost = Math.Ceiling(Convert.ToDecimal(thisCall.Duration)/60)*
                                                               rate.FixedLineRate;
                                }
                            }
                            //else: international call
                            else
                            {
                                if (Repo.ListOfFixedLinesIDs.Contains(Convert.ToInt32(thisCall.MarkerCallTypeId)))
                                {
                                    thisCall.MarkerCallCost = Math.Ceiling(Convert.ToDecimal(thisCall.Duration)/60)*
                                                               rate.FixedLineRate;
                                }
                                else if (Repo.ListOfMobileLinesIDs.Contains(Convert.ToInt32(thisCall.MarkerCallTypeId)))
                                {
                                    thisCall.MarkerCallCost =
                                        Math.Ceiling(Convert.ToDecimal(thisCall.Duration)/60)*rate.MobileLineRate;
                                }
                            } //end-inner-if
                        } //end-outer-if
                    } //end-if
                } //end-if
            }
            else
            {
                thisCall.MarkerCallCost = 0;
            }

            return thisCall;
        }

        public PhoneCall ApplyExceptions(PhoneCall thisCall)
        {
            var chargingParty = thisCall.ChargingParty.ToLower();

            var userInfo = Repo.Users.Find(user => user.SipAccount.ToLower() == chargingParty);

            var site = new Site();

            if (thisCall.MarkerCallTypeId.In(1, 2, 3, 4, 5, 6, 19, 21, 22, 24))
            {
                if (userInfo != null)
                {
                    site = Repo.Sites.Find(item => item.Name == userInfo.SiteName);
                }

                if (site != null && !string.IsNullOrEmpty(site.Name))
                {
                    //Format the charging party, destination number, and destination user uri
                    var formattedChargingParty =
                        _phoneCallHelper.ReturnEmptyIfNull(thisCall.ChargingParty).ToString().Trim('+').ToLower();
                    var formattedDestinationNumber =
                        _phoneCallHelper.ReturnEmptyIfNull(thisCall.DestinationNumberUri).ToString().Trim('+');
                    var formattedDestinationUserUri =
                        _phoneCallHelper.ReturnEmptyIfNull(thisCall.DestinationUserUri).ToString().Trim('+').ToLower();


                    //Get all the Site Exceptions
                    var thisSiteExceptions = Repo.PhoneCallsExclusions.Where(item => item.SiteId == site.Id).ToList();


                    //Check if there is a source excpetion that applies to this phone call
                    //var srcSiteExceptions = thisSiteExceptions.FirstOrDefault(
                    //        item =>
                    //            (item.ExclusionType == 'S'.ToString()) &&
                    //            (Regex.IsMatch(formattedChargingParty, @"^" + item.ExclusionSubject.ToLower()))
                    //        );

                    var srcSiteExceptions = Repo.PhoneCallsExclusions.Find(
                        item =>
                            (item.SiteId == site.Id) &&
                            (item.ExclusionType == 'S'.ToString()) &&
                            (Regex.IsMatch(formattedChargingParty, @"^" + item.ExclusionSubject.ToLower())));

                    //Check if there is a destination exception that applies to this phone call
                    //var dstSiteExceptions = thisSiteExceptions.FirstOrDefault(
                    //        item =>
                    //            (item.ExclusionType == 'D'.ToString()) &&
                    //            (
                    //                Regex.IsMatch(formattedDestinationNumber, @"^" + item.ExclusionSubject) ||
                    //                Regex.IsMatch(formattedDestinationUserUri, @"^" + item.ExclusionSubject.ToLower())
                    //            )
                    //        );

                    var dstSiteExceptions = Repo.PhoneCallsExclusions.Find(
                        item =>
                            (item.SiteId == site.Id) &&
                            (item.ExclusionType == 'D'.ToString()) &&
                            (
                                Regex.IsMatch(formattedDestinationNumber, @"^" + item.ExclusionSubject) ||
                                Regex.IsMatch(formattedDestinationUserUri, @"^" + item.ExclusionSubject.ToLower())
                                ));


                    //If any exception case applies continue
                    if (srcSiteExceptions != null || dstSiteExceptions != null)
                    {
                        if (srcSiteExceptions != null)
                        {
                            if (srcSiteExceptions.ZeroCost == 'Y'.ToString())
                            {
                                thisCall.MarkerCallCost = Convert.ToDecimal(0);
                            }

                            if (srcSiteExceptions.AutoMark == 'B'.ToString())
                            {
                                thisCall.UiCallType = @"Business";
                                thisCall.UiMarkedOn = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                thisCall.UiUpdatedByUser = @"LogParser@ccc.gr";
                            }
                            else if (srcSiteExceptions.AutoMark == 'P'.ToString())
                            {
                                thisCall.UiCallType = @"Personal";
                                thisCall.UiMarkedOn = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                thisCall.UiUpdatedByUser = @"LogParser@ccc.gr";
                            }
                        }

                        //Make Dst Number Cost = 0
                        if (dstSiteExceptions != null)
                        {
                            if (dstSiteExceptions.ZeroCost == 'Y'.ToString())
                            {
                                thisCall.MarkerCallCost = Convert.ToDecimal(0);
                            }

                            if (dstSiteExceptions.AutoMark == 'B'.ToString())
                            {
                                thisCall.UiCallType = @"Business";
                                thisCall.UiMarkedOn = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                thisCall.UiUpdatedByUser = @"LogParser@ccc.gr";
                            }
                            else if (dstSiteExceptions.AutoMark == 'P'.ToString())
                            {
                                thisCall.UiCallType = @"Personal";
                                thisCall.UiMarkedOn = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                thisCall.UiUpdatedByUser = @"LogParser@ccc.gr";
                            }
                        }

                        return thisCall;
                    } //end-if
                } //end-sitename-if-statement
            } //end-calltypeid-if-statement

            return thisCall;
        }

        public PhoneCall FixEmptyDates(PhoneCall thisCall)
        {
            //Fix DateMin Casting to SQL DateMin
            if (thisCall.SessionEndTime == DateTime.MinValue)
                thisCall.SessionEndTime = SqlDateTime.MinValue.Value;

            if (thisCall.ResponseTime == DateTime.MinValue)
                thisCall.ResponseTime = SqlDateTime.MinValue.Value;

            if (thisCall.AcDisputeResolvedOn == DateTime.MinValue)
                thisCall.AcDisputeResolvedOn = SqlDateTime.MinValue.Value;

            if (thisCall.AcInvoiceDate == DateTime.MinValue)
                thisCall.AcInvoiceDate = SqlDateTime.MinValue.Value;

            if (thisCall.UiAssignedOn == DateTime.MinValue)
                thisCall.UiAssignedOn = SqlDateTime.MinValue.Value;

            if (thisCall.UiMarkedOn == DateTime.MinValue)
                thisCall.UiMarkedOn = SqlDateTime.MinValue.Value;

            return thisCall;
        }

        public PhoneCall ProcessPhoneCall(PhoneCall phoneCall)
        {
            //Set Initial Charging Party Part
            if (!string.IsNullOrEmpty(phoneCall.ReferredBy))
            {
                phoneCall.ChargingParty = phoneCall.ReferredBy;
            }
            else if (!string.IsNullOrEmpty(phoneCall.SourceUserUri))
            {
                phoneCall.ChargingParty = phoneCall.SourceUserUri;
            }

            phoneCall = SetCallType(phoneCall);
            phoneCall = ApplyRate(phoneCall);
            phoneCall = ApplyExceptions(phoneCall);

            return phoneCall;
        }
    }
}