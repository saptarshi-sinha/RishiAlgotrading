using KiteConnect;

namespace KiteConnectSample
{
    public class CsvFileObjects
    {
        public string DateTime { get; set; }
        public string Strike1 { get; set; }
        public string Strike2 { get; set; }
        public string LTPDiff { get; set; }
        public long lotSize { get; set; }
    }


    public class CsvInputFileObjectsPivot
    {
      
        public string Strike1 { get; set; }
        public string Strike2 { get; set; }
        public decimal Pivot { get; set; }
        public string Index { get; set; }
        public decimal PivotDelta { get; set; }
        public int MaxlotPerPivot { get; set; }
        public int MaxLotSize { get; set; }
        public int lotSize { get; set; }
        public Kite kite { get; set; }

    }
    public class CsvInputFileArbitrage
    {
        public string Index { get; set; }
        public string CallStrike { get; set; }
        public string PutStrike { get; set; }

        public decimal SpotIndexPrice { get; set; }
        public string CallInstrumentId { get; set; }
        public string PutInstrumentId { get; set; }
        public string FutureInstrumentId { get; set; }
        public decimal FutureIndexPrice { get; set; }
        public decimal CallPrice { get; set; }
        public decimal PutPrice { get; set; }
        public decimal Arbitagedelta { get; set; }
        public long StrikePrice { get; set; }
        public int Tradecount { get; set; }
    }
    public class CsvInputFileSqoff
    {

        public string Strike1 { get; set; }
        public string Strike2 { get; set; }
        public decimal SqoffDelta{ get; set; }
        public decimal SqoffPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SqoffPercent { get; set; }
        public string Broker { get; set; }
        public string TransType { get; set; }
        public int NoofLots { get; set; }
        //public int lotSize { get; set; }
        public Kite kite { get; set; }

    }

    public class TrriggerObject
    {
        public string Stockname { get; set; }
        public decimal Currprice { get; set; }
        public string Buyorsell { get; set; }
        public string expiryDate { get; set; }
        public string threadname { get; set; }
    }

}