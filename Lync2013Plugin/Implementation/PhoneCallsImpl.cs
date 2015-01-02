using Lync2013Plugin.Interfaces;
using LyncBillingBase.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lync2013Plugin.Implementation
{
    class PhoneCallsImpl : IPhoneCall
    {
        private Helpers PhoneCallHelper = new Helpers();

        public PhoneCall SetCallType(PhoneCall phoneCall)
        {
            string srcCountry = string.Empty;
            string dstCountry = string.Empty;
            string srcCallType = string.Empty;
            string dstCallType = string.Empty;
            string srcDIDdsc = string.Empty;
            string dstDIDdsc = string.Empty;

            long ngnDialingCode = 0;
            string ngnCallType = string.Empty;
            string ngnDstCountry = string.Empty;

            char[] destinationNumberLeadingChars = new char[2] { '0', '0' };

            PhoneCall thisCall = phoneCall; 

            PhoneCallHelper.MatchDID(thisCall.SourceNumberUri, out srcDIDdsc, sipAccount: thisCall.SourceUserUri);
            PhoneCallHelper.MatchDID(thisCall.DestinationNumberUri, out dstDIDdsc, sipAccount: thisCall.DestinationUserUri);

            //Set SourceNumberDialing Prefix and source country
            thisCall.Marker_CallFrom = PhoneCallHelper.GetDialingPrefixInfo(PhoneCallHelper.FixNumberType(thisCall.SourceNumberUri), out srcCallType, out srcCountry, sipAccount: thisCall.ChargingParty, did: srcDIDdsc);

            //Set DestinationNumber Dialing Prefix and destination country
            thisCall.Marker_CallTo = PhoneCallHelper.GetDialingPrefixInfo(PhoneCallHelper.FixNumberType(thisCall.DestinationNumberUri), out dstCallType, out dstCountry, thisCall.ToGateway, sipAccount: thisCall.DestinationUserUri, did: dstDIDdsc);


            //Handle the phoneCall in case it is an NGN
            bool isNgnFlag = PhoneCallHelper.GetNGNDialingInfo(thisCall.DestinationNumberUri, srcCountry, out ngnDialingCode, out ngnDstCountry, out ngnCallType);

            if (isNgnFlag == true)
            {
                dstCallType = ngnCallType;
                dstCountry = ngnDstCountry;
                thisCall.Marker_CallTo = ngnDialingCode;
            }


            //Assign the updated global variables to the phonecall object
            thisCall.Marker_CallType = dstCallType;
            thisCall.Marker_CallToCountry = dstCountry;

             //Incoming Call
            if (string.IsNullOrEmpty(thisCall.SourceUserUri) || !PhoneCallHelper.IsValidEmail(thisCall.SourceUserUri))
            {
                thisCall.Marker_CallType = "INCOMING-CALL";
                thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                return thisCall;
            }

            //Voice Mail
            if (thisCall.SourceUserUri == thisCall.DestinationUserUri || thisCall.SourceNumberUri == thisCall.DestinationNumberUri)
            {
                thisCall.Marker_CallType = "VOICE-MAIL";
                thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                return thisCall;
            }

            //Check if the call is lync to lync or site to site or lync call accros the site
            if (!string.IsNullOrEmpty(dstDIDdsc))
            {
                //TODO:  IF source number uri is null check if the user site could be resolved from activedirectoryUsers table
                //       IF yes put the source site from activedirectoryUsers table instead of the soyrce did site
               
                if (!string.IsNullOrEmpty(srcDIDdsc))
                    thisCall.Marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                else
                    thisCall.Marker_CallType = "UNKNOWN-TO-" + dstDIDdsc;

                thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == "SITE-TO-SITE").TypeID;
               
                thisCall.Marker_CallTo = 0;
                thisCall.Marker_CallToCountry = null;

                return thisCall;
               
            }

            //FAIL SAFE for LYNC TO LYNC CALLS
            if (string.IsNullOrEmpty(thisCall.FromGateway) && string.IsNullOrEmpty(thisCall.ToGateway) && string.IsNullOrEmpty(thisCall.FromMediationServer) && string.IsNullOrEmpty(thisCall.ToMediationServer))
            {
                thisCall.Marker_CallType = "LYNC-TO-LYNC";
                thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                thisCall.Marker_CallTo = 0;
                thisCall.Marker_CallToCountry = null;

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
                    if (!string.IsNullOrEmpty(dstDIDdsc))
                    {
                        if (!string.IsNullOrEmpty(srcDIDdsc))
                            thisCall.Marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                        else
                            thisCall.Marker_CallType = "TO-" + dstDIDdsc;

                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == "SITE-TO-SITE").TypeID;

                        thisCall.Marker_CallTo = 0;
                        thisCall.Marker_CallToCountry = null;

                        return thisCall;
                    }

                    if (dstCallType == "fixedline")
                    {
                        thisCall.Marker_CallType = "NATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    else if (dstCallType == "gsm")
                    {
                        thisCall.Marker_CallType = "NATIONAL-MOBILE";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    else if (dstCallType == "NGN")
                    {
                        thisCall.Marker_CallType = "NGN";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    else if (dstCallType == "TOLL-FREE")
                    {
                        thisCall.Marker_CallType = "TOLL-FREE";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    else if (dstCallType == "PUSH-TO-TALK")
                    {
                        thisCall.Marker_CallType = "PUSH-TO-TALK";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    else
                    {
                        thisCall.Marker_CallType = "NATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(dstDIDdsc))
                    {
                        if (!string.IsNullOrEmpty(srcDIDdsc))
                            thisCall.Marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                        else
                            thisCall.Marker_CallType = "TO-" + dstDIDdsc;

                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == "SITE-TO-SITE").TypeID;

                        thisCall.Marker_CallTo = 0;
                        thisCall.Marker_CallToCountry = null;

                        return thisCall;
                    }

                    if (dstCallType == "fixedline")
                    {
                        thisCall.Marker_CallType = "INTERNATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    else if (dstCallType == "gsm")
                    {
                        thisCall.Marker_CallType = "INTERNATIONAL-MOBILE";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    else if (dstCallType == "NGN")
                    {
                        thisCall.Marker_CallType = "NGN";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    else if (dstCallType == "TOLL-FREE")
                    {
                        thisCall.Marker_CallType = "TOLL-FREE";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    else if (dstCallType == "PUSH-TO-TALK")
                    {
                        thisCall.Marker_CallType = "PUSH-TO-TALK";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                    else
                    {
                        thisCall.Marker_CallType = "INTERNATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                        return PhoneCallHelper.UpdateChargingPartyField(thisCall);
                    }
                }
            }


            // Handle the sourceNumber uri sip account and the destination uri is a sip account also 
            // For the calls which doesnt have source number uri or destination number uri to bable to identify which site
            if (!string.IsNullOrEmpty(thisCall.SourceUserUri) && 
                !string.IsNullOrEmpty(thisCall.DestinationUserUri) &&
                PhoneCallHelper.IsValidEmail(thisCall.DestinationUserUri)) 
            {
                if (PhoneCallHelper.IsIMEmail(thisCall.DestinationUserUri))
                {
                    thisCall.Marker_CallType = "LYNC-TO-IM";
                    thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                    thisCall.Marker_CallTo = 0;
                    thisCall.Marker_CallToCountry = null;
                }
                else
                {
                    thisCall.Marker_CallType = "LYNC-TO-LYNC";
                    thisCall.Marker_CallTypeID = Repo.callTypes.Find(type => type.Name == thisCall.Marker_CallType).TypeID;

                    thisCall.Marker_CallTo = 0;
                    thisCall.Marker_CallToCountry = null;
                }

                return thisCall;
            }

            thisCall.Marker_CallType = "N/A";
            thisCall.Marker_CallTypeID = 0;

            return thisCall;

        }

        public PhoneCall ApplyRate(PhoneCall thisCall)
        {
            string Marker_CallToCountry = thisCall.Marker_CallToCountry;
            string DestinationNumberUri = thisCall.DestinationNumberUri;
            string  ToGateway = thisCall.ToGateway;


            if (!string.IsNullOrEmpty(ToGateway))
            {
                // Check if we can apply the rates for this phone-call
                var gateway = Repo.gateways.Find(g => g.Name == ToGateway);

                if (gateway != null)
                {

                    var rates = (from keyValuePair in Repo.ratesPerGatway where keyValuePair.Key == gateway.ID select keyValuePair.Value).SingleOrDefault<List<Rates_International>>() ?? (new List<Rates_International>());

                    var ngnRates = (from KeyValuePair in Repo.ngnRatesPerGateway where KeyValuePair.Key == gateway.ID select KeyValuePair.Value).SingleOrDefault<List<RateForNGN>>() ?? (new List<RateForNGN>());

                    if (Repo.ListofChargeableCallTypes.Contains(Convert.ToInt32(thisCall.Marker_CallTypeID)) && (ngnRates.Count > 0 || rates.Count > 0))
                    {
                        //Apply the rate for this phone call

                        var rate = (from r in rates where r.ISO3CountryCode == Marker_CallToCountry select r).SingleOrDefault<Rates_International>();

                        var ngnRate = (from r in ngnRates
                                       where r.NumberingPlanForNGN.ISO3CountryCode == Marker_CallToCountry && Regex.IsMatch(DestinationNumberUri.Trim('+'), r.NumberingPlanForNGN.DialingCode.ToString())
                                       select r).SingleOrDefault<RateForNGN>();

                        //if the call is of type national/international MOBILE then apply the Mobile-Rate, otherwise apply the Fixedline-Rate
                        if (ngnRate != null && Repo.ListOfNGNIDs.Contains(Convert.ToInt32(thisCall.Marker_CallTypeID)))
                        {
                            thisCall.Marker_CallCost = Math.Ceiling(Convert.ToDecimal(thisCall.Duration) / 60) * ngnRate.Rate;
                        }
                        else if (rate != null)
                        {
                            string sourceCountryCodeISO3 = "N/A";

                            if (thisCall.Marker_CallFrom > 0)
                                sourceCountryCodeISO3 = (Repo.numberingPlan.Find(item => item.DialingPrefix == thisCall.Marker_CallFrom)).ISO3CountryCode;

                            //In case it's a national call
                            if (sourceCountryCodeISO3 == thisCall.Marker_CallToCountry)
                            {
                                var nationalRates = Repo.GetNationalRates(gateway, sourceCountryCodeISO3);

                                var matchedRate = nationalRates.Find(item => item.DialingCode == thisCall.Marker_CallTo);

                                if (matchedRate != null && matchedRate.Rate != 0)
                                {
                                    thisCall.Marker_CallCost = Math.Ceiling(Convert.ToDecimal(thisCall.Duration) / 60) * matchedRate.Rate;
                                }
                                else
                                {
                                    //Fail Safe
                                    thisCall.Marker_CallCost = Math.Ceiling(Convert.ToDecimal(thisCall.Duration) / 60) * rate.FixedLineRate;
                                }
                            }
                            //else: international call
                            else
                            {
                                if (Repo.ListOfFixedLinesIDs.Contains(Convert.ToInt32(thisCall.Marker_CallTypeID)))
                                {
                                    thisCall.Marker_CallCost = Math.Ceiling(Convert.ToDecimal(thisCall.Duration) / 60) * rate.FixedLineRate;
                                }
                                else if (Repo.ListOfMobileLinesIDs.Contains(Convert.ToInt32(thisCall.Marker_CallTypeID)))
                                {
                                    thisCall.Marker_CallCost = Math.Ceiling(Convert.ToDecimal(thisCall.Duration) / 60) * rate.MobileLineRate;
                                }
                            }//end-inner-if

                        }//end-outer-if

                    }//end-if

                }//end-if

            }
            else { thisCall.Marker_CallCost = 0; }

            return thisCall;
        }

        public PhoneCall ApplyExceptions(PhoneCall thisCall)
        {
            
            string ChargingParty = thisCall.ChargingParty.ToLower();

            User userInfo = Repo.users.FirstOrDefault(user => user.SipAccount.ToLower() == ChargingParty);

            Site site = new Site();

            if (thisCall.Marker_CallTypeID.In(1, 2, 3, 4, 5, 6, 19, 21, 22, 24))
            {

                if (userInfo != null)
                {
                    site = Repo.sites.Find(item => item.Name == userInfo.SiteName);
                }


                if (site != null && !string.IsNullOrEmpty(site.Name))
                {
                    //Format the charging party, destination number, and destination user uri
                    string formattedChargingParty = PhoneCallHelper.ReturnEmptyIfNull(thisCall.ChargingParty).ToString().Trim('+').ToLower();
                    string formattedDestinationNumber = PhoneCallHelper.ReturnEmptyIfNull(thisCall.DestinationNumberUri).ToString().Trim('+');
                    string formattedDestinationUserUri = PhoneCallHelper.ReturnEmptyIfNull(thisCall.DestinationUserUri).ToString().Trim('+').ToLower();


                    //Get all the Site Exceptions
                    var thisSiteExceptions = Repo.phoneCallsExclusions.Where(item => item.SiteID == site.ID).ToList();


                    //Check if there is a source excpetion that applies to this phone call
                    var srcSiteExceptions = thisSiteExceptions.Find(
                            item =>
                                (item.ExclusionType == 'S'.ToString()) &&
                                (Regex.IsMatch(formattedChargingParty, @"^" + item.ExclusionSubject.ToLower()))
                            );

                    //Check if there is a destination exception that applies to this phone call
                    var dstSiteExceptions = thisSiteExceptions.Find(
                            item =>
                                (item.ExclusionType == 'D'.ToString()) &&
                                (
                                    Regex.IsMatch(formattedDestinationNumber, @"^" + item.ExclusionSubject) ||
                                    Regex.IsMatch(formattedDestinationUserUri, @"^" + item.ExclusionSubject.ToLower())
                                )
                            );


                    //If any exception case applies continue
                    if (srcSiteExceptions != null || dstSiteExceptions != null)
                    {
                        if (srcSiteExceptions != null)
                        {
                            if (srcSiteExceptions.ZeroCost == 'Y'.ToString())
                            {
                                thisCall.Marker_CallCost = Convert.ToDecimal(0);
                            }

                            if (srcSiteExceptions.AutoMark == 'B'.ToString())
                            {
                                thisCall.UI_CallType = @"Business";
                                thisCall.UI_MarkedOn = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                thisCall.UI_UpdatedByUser = @"LogParser@ccc.gr";
                            }
                            else if (srcSiteExceptions.AutoMark == 'P'.ToString())
                            {
                                thisCall.UI_CallType = @"Personal";
                                thisCall.UI_MarkedOn = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                thisCall.UI_UpdatedByUser = @"LogParser@ccc.gr";
                            }
                        }

                        //Make Dst Number Cost = 0
                        if (dstSiteExceptions != null)
                        {
                            if (dstSiteExceptions.ZeroCost == 'Y'.ToString())
                            {
                                thisCall.Marker_CallCost = Convert.ToDecimal(0);
                            }

                            if (dstSiteExceptions.AutoMark == 'B'.ToString())
                            {
                                thisCall.UI_CallType = @"Business";
                                thisCall.UI_MarkedOn = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                thisCall.UI_UpdatedByUser = @"LogParser@ccc.gr";
                            }
                            else if (dstSiteExceptions.AutoMark == 'P'.ToString())
                            {
                                thisCall.UI_CallType = @"Personal";
                                thisCall.UI_MarkedOn = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                thisCall.UI_UpdatedByUser = @"LogParser@ccc.gr";
                            }
                        }

                        return thisCall;
                    }//end-if

                }//end-sitename-if-statement

            }//end-calltypeid-if-statement

            return thisCall;
        }

        
    }
}
