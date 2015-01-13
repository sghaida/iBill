namespace LyncBillingBase.Reports
{
    public class TopDestinationCountries
    {
        public string CountryName { private set; get; }
        public int CallsCount { private set; get; }
        public decimal CallsCost { private set; get; }
        public decimal CallsDuration { private set; get; }
    }
}