using APIBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace IIFLAPI
{
    public struct IIFLUser
    {
        public IIFLUser(Dictionary<string, dynamic> data) : this()
        {
            try
            {                
                AccessToken = data["result"]["token"];
                UserId = data["result"]["userID"];
                AppVersion = data["result"]["appVersion"];              
            }
            catch (Exception e)
            {
                //throw new DataException("Unable to parse data. " + Utils.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

       
        
        public string AccessToken { get; set; }
        public string UserId { get; }     
        public string AppVersion { get; }
    }

   
    public struct IIFLHistorical
    {
        public IIFLHistorical(Dictionary<string,dynamic>data): this()
        {
            
            try
            {
                Dictionary<string,dynamic> dictData = data["result"];
                KeyValuePair<string,dynamic>kvphist = dictData.ElementAt(2);

                string histarr = kvphist.Value;

                string[] strhistarr = histarr.Split(',');
               
                lsthistorical = new List<IIFLHistorical>();
                for (int i = 0; i < strhistarr.Length; ++i)
                {
                    string[] intcandledata = strhistarr[i].Split('|');
                   
                    int j = 0;

                    IIFLHistorical iiflhistobj = new IIFLHistorical();
                    iiflhistobj.timestamplong = Convert.ToInt32(intcandledata[j]);

                    iiflhistobj.Open = Convert.ToDecimal(intcandledata[++j]);
                    iiflhistobj.High = Convert.ToDecimal(intcandledata[++j]);
                    iiflhistobj.Low = Convert.ToDecimal(intcandledata[++j]);
                    iiflhistobj.Close = Convert.ToDecimal(intcandledata[++j]);
                    iiflhistobj.Volume = Convert.ToInt32(intcandledata[++j]);
                    iiflhistobj.OpenInterest = Convert.ToInt32(intcandledata[++j]);
                    lsthistorical.Add(iiflhistobj);
                    
                }
                
                
                //for (int i = 0; i < dictData.Le; ++i)
                {
                    IIFLHistorical iiflhistorical = new IIFLHistorical();
                    //iiflhistorical.Open = data["result"]["dataresponse"][i]["Open"];
                    //lsthistorical.Add
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<IIFLHistorical> lsthistorical;
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public int timestamplong { get; set; }
        public long Volume { get; set; }
        public int OpenInterest { get; set; }

    }

    public struct IIFLInstrument
    {
        public IIFLInstrument(Dictionary<string, dynamic> data) : this()
        {
            try
            {
                ExchangeSegment = Convert.ToUInt32(data["result"][0]["ExchangeSegment"]);
                ExchangeInstrumentID = Convert.ToUInt32(data["result"][0]["ExchangeInstrumentID"]);
                InstrumentType = Convert.ToUInt32(data["result"][0]["InstrumentType"]);
                Name = "";
                Name = data["result"][0]["Name"];
                DisplayName = data["result"][0]["DisplayName"];
                Description = data["result"][0]["Description"];
                Series = data["result"][0]["Series"];
                InstrumentID = data["result"][0]["InstrumentID"];
                //ContractExpirationString = data["result"][0]["ContractExpirationString"];
                RemainingExpiryDays = data["result"][0]["RemainingExpiryDays"];
                //StrikePrice = Convert.ToUInt32(data["result"][0]["StrikePrice"]);

                LotSize = Convert.ToUInt32(data["result"][0]["LotSize"]);
            }
            catch (Exception e)
            {
                //throw new DataException("Unable to parse data. " + Utils.JsonSerialize(data), HttpStatusCode.OK, e);
            }

        }

        public UInt32 ExchangeSegment { get; set; }
        public UInt32 ExchangeInstrumentID { get; set; }
        public UInt32 InstrumentType { get; set; }
        public string  Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }        
        public string Series { get; set; }
        public long InstrumentID { get; set; }
        public string ContractExpirationString { get; set; }
        public decimal RemainingExpiryDays { get; set; }
        public UInt32 StrikePrice { get; set; }
        public UInt32 LotSize { get; set; }
    }

    public struct IIFLQuote
    {
        public IIFLQuote(Dictionary<string, dynamic> data) : this()
        {
            try
            {
                var ArrayList = data["result"]["listQuotes"];

                var jss = new JavaScriptSerializer();
                var data1 = Utils.JsonDeserialize(ArrayList[0]);
                ExchangeInstrumentID = Convert.ToUInt32(data1["ExchangeInstrumentID"]);
                ExchangeSegment = Convert.ToUInt32(data1["ExchangeSegment"]);
                //Timestamp = Utils.StringToDate(data["timestamp"]);
                LastPrice = Convert.ToUInt32(data1["Touchline"]["LastTradedPrice"]);

                Open = data1["Touchline"]["Open"];
                Close = data1["Touchline"]["Close"];
                Low = data1["Touchline"]["Low"];
                High = data1["Touchline"]["Low"];
                LastQuantity = data1["Touchline"]["TotalTradedQuantity"];
                Bids = new List<IIFLDepthItem>();
                Offers = new List<IIFLDepthItem>();

                if (data1["Bids"] != null)
                {
                    foreach (Dictionary<string, dynamic> bid in data1["Bids"])
                        Bids.Add(new IIFLDepthItem(bid));
                }

                if (data1["Asks"] != null)
                {
                    foreach (Dictionary<string, dynamic> offer in data1["Asks"])
                        Offers.Add(new IIFLDepthItem(offer));
                }
                
            }
            catch (Exception e)
            {
                //throw new DataException("Unable to parse data. " + Utils.JsonSerialize(data), HttpStatusCode.OK, e);
            }
                        

        }

        
        public List<IIFLDepthItem> ToList()
        {
            List<IIFLDepthItem> lstiifdepthitem = Bids.ToList();
            //lstiifluser.ToList()
            return lstiifdepthitem;
        }
        //public var ArrayList;
        public UInt32 ExchangeInstrumentID { get; set; }
        public UInt32 ExchangeSegment { get;  set; }
        public decimal LastPrice { get; set; }
        public int LastQuantity { get; set; }
        public decimal AveragePrice { get; set; }
        public UInt32 Volume { get; set; }
        public UInt32 BuyQuantity { get; set; }
        public UInt32 SellQuantity { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Change { get; set; }
        public decimal LowerCircuitLimit { get; set; }
        public decimal UpperCircuitLimit { get; set; }
        public List<IIFLDepthItem> Bids { get; set; }
        public List<IIFLDepthItem> Offers { get; set; }

        // KiteConnect 3 Fields

        public DateTime? LastTradeTime { get; set; }
        public UInt32 OI { get; set; }
        public UInt32 OIDayHigh { get; set; }
        public UInt32 OIDayLow { get; set; }
        public DateTime? Timestamp { get; set; }
    }
    public struct IIFLBankQuote
    {
        public IIFLBankQuote(Dictionary<string, dynamic> data) : this()
        {
            try
            {
                var ArrayList = data["result"]["listQuotes"];

                var jss = new JavaScriptSerializer();
                var data1 = Utils.JsonDeserialize(ArrayList[0]);
                ExchangeInstrumentID = Convert.ToUInt32(data1["ExchangeInstrumentID"]);
                ExchangeSegment = Convert.ToUInt32(data1["ExchangeSegment"]);
                //Timestamp = Utils.StringToDate(data["timestamp"]);
                LastPrice = Convert.ToUInt32(data1["IndexValue"]);
                PrevDayClose = Convert.ToUInt32(data1["ClosingIndex"]);
                Open = data1["OpeningIndex"];
                High = data1["HighIndexValue"];
                Low = data1["LowIndexValue"];
                //Close = data1["Close"];



            }
            catch (Exception e)
            {
                //throw new DataException("Unable to parse data. " + Utils.JsonSerialize(data), HttpStatusCode.OK, e);
            }


        }
        public UInt32 ExchangeInstrumentID { get; set; }
        public UInt32 ExchangeSegment { get; set; }
        public decimal LastPrice { get; set; }
        public decimal PrevDayClose { get; set; }
        public int Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public UInt32 Volume { get; set; }
        public UInt32 BuyQuantity { get; set; }
        public UInt32 SellQuantity { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Change { get; set; }
        public decimal LowerCircuitLimit { get; set; }
        public decimal UpperCircuitLimit { get; set; }
    }

    public struct IIFLOrder
    {
        public IIFLOrder(Dictionary<string, dynamic> data) : this()
        {
            ArrayList array = data["result"];
            int arraycount = array.Count - 1;
            Dictionary<string, dynamic> data1 = data["result"][arraycount];

            OrderStatus = data1["OrderStatus"];
            OrderId = Convert.ToString(data1["AppOrderID"]);
            ExchangeOrderID = Convert.ToString(data1["ExchangeOrderID"]);



        }

        public string OrderStatus { get; set; }
        public string OrderId { get; set; }
        public string ExchangeOrderID { get; set; }
    }
    public struct IIFLDepthItem
    {
        public IIFLDepthItem(Dictionary<string, dynamic> data)
        {
            Quantity = Convert.ToUInt32(data["Size"]);
            Price = data["Price"];
            Orders = Convert.ToUInt32(data["TotalOrders"]);
        }

        public UInt32 Quantity { get; set; }
        public decimal Price { get; set; }
        public UInt32 Orders { get; set; }
    }

}
