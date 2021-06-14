using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIFLAPI
{
    public class IIFLConstants
    {  
        //Exchange Segment
        public const string NSECM = "1";
        public const string NSEFO = "2";
        public const string NSECD = "3";
        public const string BSECM = "11";
        public const string BSEFO = "12";
        public const string BSECD = "13";
        public const string NCDEX = "21";
        public const string MSEICM = "41";
        public const string MSEIFO = "42";
        public const string MSEICD = "43";

        //series
        public const string SERIES_OPTSTK = "OPTSTK";
        public const string SERIES_OPTDIX = "OPTIDX";
        public const string SERIES_FUTSTK = "FUTSTK";
        public const string SERIES_FUTIDX = "FUTIDX";
        //MessageCode
        public const string MESSAGECODE_TOUCHLINE = "1501";
        public const string MESSAGECODE_MARKETDATA = "1502";
        public const string MESSAGECODE_INDEXDATA = "1504";
        public const string MESSAGECODE_CANDLEDATA = "1505";
        public const string MESSAGECODE_OPENINT = "1510";
        
        //Order Type
        public const string ORDER_TYPE_MARKET = "MARKET";
        public const string ORDER_TYPE_LIMIT = "LIMIT";
        public const string ORDER_TYPE_SLLIMIT = "STOPLIMIT";
        public const string ORDER_TYPE_STOPMARKET = "STOPMARKET";

        //Timeinforce
        public const string TIMEINFORCE_DAY = "DAY";
        public const string TIMEINFORCE_IOC = "IOC";

        //product type
        public const string PRODUCTTYPE_CO = "CO";
        public const string PRODUCTTYPE_CNC = "CNC";
        public const string PRODUCTTYPE_MIS = "MIS";
        public const string PRODUCTTYPE_NRML = "NRML";

        //order side

        public const string ORDERSIDE_BUY = "BUY";
        public const string ORDERSIDE_SELL = "SELL";

    }
}
