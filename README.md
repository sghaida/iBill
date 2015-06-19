# iBill

A multi-featured billing, invoicing and reporting application written in C# and ASP.NET. It manages distributed instances of the Microsoft Lync telephony services.

iBill is capable of billing and invoicing any multi-sites setup of Microsoft Lync, billing multiple telephony gateways across the whole system. Provides a hierarchy of system roles: Users, User Delegates, Site Accountants, Site Admins, and System Admins. Provides consumption statistics for users, sites and overall usage across all the setup. Provides utilities for accountants to export costs reports, and perform financial charging procedures.

#### OVERVIEW

iBill consists of the following sub-projects:

  * **[ORM](CCC.ORM/):** The Object-Relational-Mapper and Data Access Layer. This project is the base for all of iBill. iBill uses this project to communicate with the database.
  * **[Lync Billing Base](LyncBillingBase/):** The ORM-related classes. Data Models, Data Mappers, and the Repository. This project is the single interface between the UI and the ORM-layer.
  * **[Lync Billing UI](LyncBillingUI/):** The User Interface project, which is written in ASP.NET.
  * **[UTILS](CCC.UTILS/):** A collection of utilities used in the application such as:
    * MS Active Directory Query Connector.
    * MS Outlook Connector.
    * Mail Sender.
    * Misc. helper-functions.

There are 3 other projects in iBill, they are responsible for providing plugins that connect to different telephony services, such as: MS Lync, Asterisk DB, and Cisco Call Manager. Plugins are expected to query a service's database/backend, and then import it's phone calls data into it's respective iBill database-table(s).

  * **[Phone Calls Processor](PhoneCallsProcessor/):** This project defines the interface(s) required from new plugins to implement, in order to communicate with the plugins-loader in iBill.
  * **[Phone Calls Processor Loader](PhoneCallsProcessorLoader/):** This is the plugins-loader. It basically loads the various plugins, and executes their data processing (importing, source-country discovery, cost marking, etc...) functions in the backend.
  * **[Lync 2013 Plugin](Lync2013Plugin/):** This is the plugin that imports data (phone calls, gateways, etc...) from MS Lync 2013 LcsCDR database into iBill, and then processes it.


#### INSTALLATION

  * Dependencies:
    * Install all the NuGet packages listed in the Technologies section below. They are not included in the project.
  * Database: 
    * Work in progress...


#### TECHNOLOGIES

  * C# 4.5
  * .NET Framework 4.5
  * Antlr.3.5.0.2
  * Ext.NET.3.1.0
  * Ext.NET.Utilities.2.5.0
  * FastMember.1.0.0.11
  * iTextSharp.5.5.5
  * iTextSharp-LGPL.4.1.6
  * itextsharp.xmlworker.5.5.5
  * Microsoft.AspNet.FriendlyUrls.1.0.2
  * Microsoft.AspNet.FriendlyUrls.Core.1.0.2
  * Microsoft.AspNet.Web.Optimization.1.1.3
  * Microsoft.Exchange.WebServices.2.2
  * Microsoft.Web.Infrastructure.1.0.0.0
  * Modernizr.2.8.3
  * mongocsharpdriver.1.9.2
  * Newtonsoft.Json.6.0.8
  * Respond.1.4.2
  * Transformer.NET.2.1.1
  * WebActivatorEx.2.0.6
  * WebGrease.1.6.0


#### DEVELOPERS

  * [Saddam Abu Ghaida](https://github.com/sghaida)
  * [Ahmad Alhour](https://github.com/aalhour)

