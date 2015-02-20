using System;
using System.Collections.Generic;
using Microsoft.Exchange.WebServices.Data;

namespace CCC.UTILS.Outlook
{
    public class ExchangeWebServices
    {
        private List<OutlookContact> _outlookContacts;

        private readonly Uri _uri = new Uri(@"https://internalmail.ccc.gr/EWS/Exchange.asmx");

        public ExchangeService ExService { get; private  set; }

        public List<OutlookContact> OutlookContacts {
            get
            {
                _outlookContacts = new List<OutlookContact>();

                string businessPhone1 = null;
                string businessPhone2 = null;
                string homePhone1 = null; 
                string homePhone2 = null; 
                string otherPhone = null; 
                string primaryPhone = null;
                string mobilePhone = null; 

                foreach ( var v in ExService.FindItems( WellKnownFolderName.Contacts , new ItemView( 100000000 ) ) )
                {
                    Contact contact = v as Contact;

                    if ( contact != null )
                    {
                        var phones = contact.PhoneNumbers;
                        
                        OutlookContact oContact = new OutlookContact();

                        if ( phones != null )
                        {
                            if (contact.DisplayName != null)
                                oContact.Name = contact.DisplayName;
                           
                            if ( phones.TryGetValue( PhoneNumberKey.BusinessPhone , out businessPhone1 ) )
                                oContact.BusinessPhone1 = businessPhone1;

                            if ( phones.TryGetValue( PhoneNumberKey.BusinessPhone2 , out businessPhone2 ) )
                                oContact.BusinessPhone2 = businessPhone2;

                            if ( phones.TryGetValue( PhoneNumberKey.HomePhone , out homePhone1 ) )
                                oContact.HomePhone1 = homePhone1;

                            if ( phones.TryGetValue( PhoneNumberKey.HomePhone , out homePhone2 ) )
                                oContact.HomePhone2 = homePhone2;

                            if ( phones.TryGetValue( PhoneNumberKey.OtherTelephone , out otherPhone ) )
                                oContact.OtherPhone = otherPhone;

                            if ( phones.TryGetValue( PhoneNumberKey.PrimaryPhone , out primaryPhone ) )
                                oContact.PrimaryPhone = primaryPhone;


                            if ( phones.TryGetValue( PhoneNumberKey.MobilePhone , out mobilePhone ) )
                                oContact.MobilePhone = mobilePhone;
                        }

                        if (businessPhone1 == null && businessPhone2 == null && homePhone1 == null && homePhone2 == null &&
                            otherPhone == null && primaryPhone == null && mobilePhone == null)
                        {
                            continue;
                        }
                        _outlookContacts.Add( oContact );
                    }
                }
                return _outlookContacts; 
            }
        }
        
        public ExchangeWebServices(string username, string password, string domain)
        {
            WebCredentials credentials = new WebCredentials( username , password , domain );

            ExService = new ExchangeService( ExchangeVersion.Exchange2013 );
            ExService.Url = _uri;
            ExService.Credentials = credentials;
        }

    }
}
