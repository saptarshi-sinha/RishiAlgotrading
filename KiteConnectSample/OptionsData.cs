using APIBridge;
using com.dakshata.autotrader.api;
using com.dakshata.constants.trading;
using com.dakshata.data.model.common;
using CsvHelper;
using IIFLAPI;
using KiteConnect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KiteConnectSample
{
    public class OptionsData
    {
        public static string _broker = "";
        public static string _accessTokenMarket = "";
        public static string _accessTokenInteractive = "";
        public static string _zerodhanewAccessToken = "";
        public static string zerodhaid = "XV5542";
        public static string zerodhapass = "rishi321";
        public static string zerodhapin = "777777";
        public static List<Instrument> lstInstrument=null;

        public static Kite kite = null;
        public void GetOptionsData(Kite kite)
        {
            //List<Instrument> lstinstrument= kite.GetInstruments("NFO");

            Dictionary<string, Quote> quotes = kite.GetQuote(InstrumentId: new string[] { "NFO:NIFTY2082711400CE" });
            

        }
        public static List<Instrument> GetAllLotSize(Kite kite)
        {
            List<Instrument> instrument = kite.GetInstruments("NFO");
            return instrument;

        }
        public static Dictionary<string,dynamic> GetInstrumentIDLotsize(ref Dictionary<string,dynamic> dictinstruments)
        {
            //Dictionary<string, dynamic> dictinstruments = new Dictionary<string, dynamic>();
            switch(_broker.ToLower())
            {
                case "zerodha":
                    {
                        string strikeprice = dictinstruments["strikeprice1"];
                        if (kite == null)
                        {
                            kite = new Kite("67uleb0ttdjmdp6v", Debug: false);
                            //Kite kite = new Kite();
                            kite.Login();
                            List<Instrument> lstInstrument = GetAllLotSize(kite);
                            Instrument findscript = lstInstrument.Find(i => i.TradingSymbol == strikeprice);
                            dictinstruments.Add("lotsize", findscript.LotSize);
                        }
                        else
                        {
                            if (lstInstrument == null || lstInstrument.Count() == 0)
                            {
                                lstInstrument = GetAllLotSize(kite);
                            }
                            Instrument findscript = lstInstrument.Find(i => i.TradingSymbol == strikeprice);
                            dictinstruments.Add("lotsize", findscript.LotSize);
                        }
                        

                        break;
                    }
                case "iifl":
                case "zerodhanew":      
                    {
                        IIFLConnect iiflConnect = new IIFLConnect();
                        //lock (_sync)
                        {
                            if (_accessTokenMarket == "")
                            {
                                //IIFLUser user = iiflConnect.GenerateSessionMarket();
                                //iiflConnect.SetAccessToken(user.AccessToken, "marketdata");
                                //_accessTokenMarket = user.AccessToken;
                                GetIIFLMarketAccessToken();
                                iiflConnect.SetAccessToken(_accessTokenMarket);
                                //Console.WriteLine("Entered once Line GetInstrumentIDLotsize() 64");

                            }
                            else
                            {
                                iiflConnect.SetAccessToken(_accessTokenMarket);
                            }
                        }
                        Dictionary<string, dynamic> dictnewinstruments = new Dictionary<string, dynamic>();
                        IIFLInstrument instrumentData=new IIFLInstrument();

                        //foreach (KeyValuePair<string, dynamic> kvp in dictinstruments)
                        {
                            Dictionary<string, dynamic> dictParams = iiflConnect.ParseTradingSymbol(dictinstruments["strikeprice1"]);
                            instrumentData = iiflConnect.GetInstrumentID("marketdata.optionsymbol", dictParams);
                            
                            dictnewinstruments.Add("strikeprice1", instrumentData.ExchangeInstrumentID);
                        }
                        dictnewinstruments.Add("lotsize", instrumentData.LotSize);
                        dictinstruments.Clear();
                        foreach(KeyValuePair<string,dynamic>kvp1 in dictnewinstruments)
                        {
                            dictinstruments.Add(kvp1.Key, kvp1.Value);
                        }

                    }
                    break;

                
                   
            }

            return dictinstruments;
        }


        public static Dictionary<string, dynamic> GetInstrumentIDLotsizeFUT(ref Dictionary<string, dynamic> dictinstruments, string expiryDate)
        {
            
                //Dictionary<string, dynamic> dictinstruments = new Dictionary<string, dynamic>();
                switch (_broker.ToLower())
                {
                    case "zerodha":
                        {
                            string strikeprice = dictinstruments["strikeprice1"];
                            if (kite == null)
                            {
                                kite = new Kite("67uleb0ttdjmdp6v", Debug: false);
                                //Kite kite = new Kite();
                                kite.Login();
                                List<Instrument> lstInstrument = GetAllLotSize(kite);
                                Instrument findscript = lstInstrument.Find(i => i.TradingSymbol == strikeprice);
                                dictinstruments.Add("lotsize", findscript.LotSize);
                            }
                            else
                            {
                                if (lstInstrument == null || lstInstrument.Count() == 0)
                                {
                                    lstInstrument = GetAllLotSize(kite);
                                }
                                Instrument findscript = lstInstrument.Find(i => i.TradingSymbol == strikeprice);
                                dictinstruments.Add("lotsize", findscript.LotSize);
                            }


                            break;
                        }
                    case "iifl":
                    {
                        IIFLConnect iiflConnect = new IIFLConnect();
                        //lock (_sync)
                        {
                            if (_accessTokenMarket == "")
                            {
                                //IIFLUser user = iiflConnect.GenerateSessionMarket();
                                //iiflConnect.SetAccessToken(user.AccessToken, "marketdata");
                                //_accessTokenMarket = user.AccessToken;
                                GetIIFLMarketAccessToken();
                                iiflConnect.SetAccessToken(_accessTokenMarket);
                                //Console.WriteLine("Entered once Line GetInstrumentIDLotsize() 64");

                            }
                            else
                            {
                                iiflConnect.SetAccessToken(_accessTokenMarket);
                            }
                        }
                        Dictionary<string, dynamic> dictnewinstruments = new Dictionary<string, dynamic>();
                        IIFLInstrument instrumentData = new IIFLInstrument();
                        
                        try
                        {
                            //foreach (KeyValuePair<string, dynamic> kvp in dictinstruments)
                            {
                                Dictionary<string, dynamic> dictParams = iiflConnect.ParseTradingSymbolFUT(dictinstruments["strikeprice1"], expiryDate);
                                instrumentData = iiflConnect.GetInstrumentID("marketdata.futsymbol", dictParams);

                                dictnewinstruments.Add("strikeprice1", instrumentData.ExchangeInstrumentID);
                                
                            }
                            dictnewinstruments.Add("lotsize", instrumentData.LotSize);
                            dictinstruments.Clear();
                            foreach (KeyValuePair<string, dynamic> kvp1 in dictnewinstruments)
                            {
                                dictinstruments.Add(kvp1.Key, kvp1.Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                        break;

                }           
            

            return dictinstruments;
        }
        public static IAPIBridge GetBrokerObject()
        {
            IAPIBridge ibridgeobj = null;
            switch (_broker.ToLower())
            {
                case "iifl":
                    {
                        ibridgeobj = new IIFLConnect();
                        //IIFLUser user = ibridgeobj.GenerateSessionMarket();
                        break;
                    }
                case "zerodha":
                    {
                        break;
                    }

            }

            return ibridgeobj;
        }
        public static int GetLotSize(string scriptCode)
        {
            int lotSize = 0;
            switch (_broker.ToLower())
            {
                case "zerodha":
                    {
                        Kite kite = new Kite();
                        List<Instrument> lstInstrument = GetAllLotSize(kite);
                        Instrument findscript = lstInstrument.Find(i => i.TradingSymbol == scriptCode);
                        lotSize = Convert.ToInt32(findscript.LotSize);                       
                    }
                    break;

                case "iifl":
                    {

                        break;
                    }
                    
            }//switch
            return lotSize;

        }
        public static int GetLotSize(Kite kite, string scriptccode, List<Instrument> lstinstrument)
        {
            int lotsize = 0;
            Instrument findscript = lstinstrument.Find(i => i.TradingSymbol == scriptccode);
            lotsize = Convert.ToInt32(findscript.LotSize);
            return lotsize;
        }

        public void BuyStock(Kite kite)
        {
            string strikeprice1 = "NFO:NIFTY20AUG10500PE";
            string strikeprice2 = "NFO:NIFTY20AUG10400PE";
            //strikeprice1 = "NSE:AMARAJABAT";
            try
            {
                //get data 
                Dictionary<string, Quote> quotes = kite.GetQuote(InstrumentId: new string[] { strikeprice1, strikeprice2 });
                //Dictionary<string, Quote> quotes1 = kite.GetQuote(InstrumentId: new string[] { strikeprice1, strikeprice2 });

                List<Instrument> instrument = kite.GetInstruments();
                Instrument findscript = instrument.Find(i => i.TradingSymbol == "NIFTY20AUG10400PE");
                long lotsize = findscript.LotSize;

                // get diff of strikeprice 1 and strikeprice2
                Quote quote1;
                bool bresult = quotes.TryGetValue(strikeprice1, out quote1);
                List<DepthItem> lstbids = quote1.Bids;
                List<DepthItem> lstoffers = quote1.Offers;
                ///kite.GetQuote()
                Dictionary<string, dynamic> response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                       TradingSymbol: strikeprice1.Replace("NFO:", ""),
                       TransactionType: Constants.TRANSACTION_TYPE_BUY,
                       Quantity: 75,
                       Price: lstoffers[0].Price,
                       Validity: Constants.VALIDITY_DAY,
                       OrderType: Constants.ORDER_TYPE_LIMIT,
                       Variety: Constants.VARIETY_AMO,
                       ProductType: Constants.PRODUCT_NRML);

                // kite.CancelOrder()

                dynamic status = "";
                dynamic detaildata = "";
                response1.TryGetValue("status", out status);
                bool statusdetail = response1.TryGetValue("data", out detaildata);
                Dictionary<string, object> dictstatusdetail = (Dictionary<string, object>)detaildata;
                if (status == "success")
                {

                }


                foreach (KeyValuePair<string, dynamic> kvp in response1)
                {
                    string key = kvp.Key;
                    string value = kvp.Value;
                    Console.WriteLine(key + "," + value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="csvobj"></param>
        public static void CreditSpreadStrategyWithDummy(object csvobj)
        {
            CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            decimal currentpivot1 = csvinputobj.Pivot;
            decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            int maxlots = csvinputobj.MaxLotSize;
            long lotsize = csvinputobj.lotSize;
            decimal pivotdelta = csvinputobj.PivotDelta;
            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", threadname,startpivot);
            string filename1path = string.Format(@"C:\Algotrade\Dummy{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData",  threadname,startpivot);
            string filename2path = string.Format(@"C:\Algotrade\credit{0}.txt", filename2);
                      
            int nooflots = 0;
            //decimal maxuppivotrange = currentpivot + (pivotdelta * iterations);
            //decimal maxdownpivotrange = currentpivot - (pivotdelta * iterations);
            decimal nextpivotup = startpivot;
            decimal nextpivotdown = startpivot;
            decimal currentpivot = startpivot;
            decimal currentpivot2 = startpivot;
            int sno = 1;

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            //foreach(CsvFileObjects csvobj in lstfileobj)
            {
                try
                {
                    Thread.Sleep(3000);
                    Dictionary<string, Quote> quotes = kite.GetQuote(InstrumentId: new string[] { "NFO:" + strikeprice1, "NFO:" + strikeprice2, "NSE:NIFTY 50" });

                    KeyValuePair<string, Quote> kvpquote1 = quotes.ElementAt(0);
                    KeyValuePair<string, Quote> kvpquote2 = quotes.ElementAt(1);
                    KeyValuePair<string, Quote> kvpnifty = quotes.ElementAt(2);
                    //get bid ask price
                    List<DepthItem> lstbids1 = kvpquote1.Value.Bids;
                    List<DepthItem> lstoffers1 = kvpquote1.Value.Offers;
                    List<DepthItem> lstbids2 = kvpquote2.Value.Bids;
                    List<DepthItem> lstoffers2 = kvpquote2.Value.Offers;



                    decimal ltpdiff1 = Math.Abs(kvpquote1.Value.LastPrice - kvpquote2.Value.LastPrice);
                    //todelete
                    //decimal ltpdiff1 = Convert.ToDecimal(csvobj.LTPDiff);
                    //todelete
                    //decimal ltpdiff1 = Math.Abs(lstbids1[0].Price - lstoffers2[0].Price);

                    if ((nooflots == 0 && ltpdiff1 >= startpivot) ||
                        (ltpdiff1 >= nextpivotup && nextpivotup >= startpivot && Math.Abs(nooflots) < maxlots))
                    {

                        //sell 1 lot
                        //sell 1st strike at bid price and buy 2nd strike one at ask price
                        //if (Math.Abs(bids11 - offers22) >= ltpdiff1)
                        //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                        {
                            //Dictionary<string, dynamic> response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //  TradingSymbol: strikeprice1.Replace("NFO:", ""),
                            //  TransactionType: Constants.TRANSACTION_TYPE_SELL,
                            //  Quantity: lotsize,
                            //  Price: lstbids1[0].Price,
                            //  Validity: Constants.VALIDITY_DAY,
                            //  OrderType: Constants.ORDER_TYPE_LIMIT,
                            //  Variety: Constants.VARIETY_AMO,
                            //  Product: Constants.PRODUCT_NRML);

                            //buy 2nd strike
                            //Dictionary<string, dynamic> response2 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //   TradingSymbol: strikeprice2.Replace("NFO:", ""),
                            //   TransactionType: Constants.TRANSACTION_TYPE_BUY,
                            //    Quantity: lotsize,
                            //    Price: lstoffers2[0].Price,
                            //    Validity: Constants.VALIDITY_DAY,
                            //     OrderType: Constants.ORDER_TYPE_LIMIT,
                            //     Variety: Constants.VARIETY_AMO,
                            //     Product: Constants.PRODUCT_NRML);

                            dynamic status = "success";
                            dynamic status1 = "success";
                            //response1.TryGetValue("status", out status);
                            //response2.TryGetValue("status", out status1);

                            if (status == "success" && status1 == "success")
                            {
                                nooflots -= 1;
                                currentpivot = nextpivotup;
                                nextpivotup = currentpivot + pivotdelta;
                                nextpivotdown = currentpivot - pivotdelta;
                                currentpivot2 = currentpivot;

                                //todelete
                                string writetofile = string.Format("{0},{1},{2},{3},{4},{5}\n", DateTime.Now, ltpdiff1, nooflots, lstbids1[0].Price, lstoffers2[0].Price, "SELL");
                                Console.WriteLine(writetofile);
                                File.AppendAllText(filename1path, writetofile);
                                //
                            }
                            else
                            {
                                //try again which ever script failed try to execute the order
                            }

                            //startpivot = 0;
                        }//end of if
                    }
                    if (ltpdiff1 <= nextpivotdown && nooflots < 0)
                    {
                        //buy 1 lot
                        //buy 1st strike at ask price and sell 2nd strike at bid price
                        //if(Math.Abs(offers11-bids22)<ltpdiff1)
                        //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= nextpivotdown)
                        {
                            //Dictionary<string, dynamic> response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            // TradingSymbol: strikeprice1.Replace("NFO:", ""),
                            // TransactionType: Constants.TRANSACTION_TYPE_BUY,
                            // Quantity: 75,
                            // Price: lstoffers2[0].Price,
                            // Validity: Constants.VALIDITY_DAY,
                            // OrderType: Constants.ORDER_TYPE_LIMIT,
                            // Variety: Constants.VARIETY_AMO,
                            // Product: Constants.PRODUCT_NRML);

                            //sell 2nd strike 
                            //Dictionary<string, dynamic> response2 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //   TradingSymbol: strikeprice2.Replace("NFO:", ""),
                            //   TransactionType: Constants.TRANSACTION_TYPE_SELL,
                            //   Quantity: 75,
                            //   Price: lstbids1[0].Price,
                            //   Validity: Constants.VALIDITY_DAY,
                            //   OrderType: Constants.ORDER_TYPE_LIMIT,
                            //   Variety: Constants.VARIETY_AMO,
                            //   Product: Constants.PRODUCT_NRML);

                            dynamic status = "success";
                            dynamic status1 = "success";
                            //response1.TryGetValue("status", out status);
                            // response2.TryGetValue("status", out status1);

                            if (status == "success" && status1 == "success")
                            {

                                nooflots += 1;
                                currentpivot = nextpivotdown;
                                nextpivotup = currentpivot + pivotdelta;
                                nextpivotdown = currentpivot - pivotdelta;
                                currentpivot2 = currentpivot;
                                startpivot = currentpivot1;

                                
                                string writetofile = string.Format("{0},{1},{2},{3},{4},{5}\n", DateTime.Now, ltpdiff1, nooflots, lstoffers2[0].Price, lstbids1[0].Price, "BUY");
                                Console.WriteLine(writetofile);
                                File.AppendAllText(filename1path, writetofile);
                                //
                            }
                            else
                            {
                                // execute the trade which failed
                            }
                        }//end of if Math.Abs

                    }
                    //string filecontent1 = string.Format("{0},{1},{2},{3}\n", kvpquote1.Value.Timestamp, kvpquote1.Value.LastPrice, kvpquote2.Value.LastPrice, ltpdiff1);

                    string filecontent1 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, kvpquote1.Value.LastPrice, kvpquote2.Value.LastPrice, ltpdiff1);

                    //Console.WriteLine(filecontent);
                    File.AppendAllText(filename2path, filecontent1);
                    sno++;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
                
            }//end of while

        }//end of function

        public static void ShortStraddleStrategyWithDummy(object csvobj)
        {
            CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            Kite kite = csvinputobj.kite;
            decimal currentpivot1 = csvinputobj.Pivot;
            decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            int maxlots = csvinputobj.MaxLotSize;
            decimal pivotdelta = csvinputobj.PivotDelta;
            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}_{6}", threadname, strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", threadname, startpivot);
            string filename1path = string.Format(@"C:\Algotrade\Dummy{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}_{6}", threadname, strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData", threadname, startpivot);
            string filename2path = string.Format(@"C:\Algotrade\credit{0}.txt", filename2);

            int nooflots = 0;
            //decimal maxuppivotrange = currentpivot + (pivotdelta * iterations);
            //decimal maxdownpivotrange = currentpivot - (pivotdelta * iterations);
            decimal nextpivotup = startpivot;
            decimal nextpivotdown = startpivot;
            decimal currentpivot = startpivot;
            decimal currentpivot2 = startpivot;
            int sno = 1;

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            //foreach(CsvFileObjects csvobj in lstfileobj)
            {
                try
                {
                    Thread.Sleep(3000);
                    Dictionary<string, Quote> quotes = kite.GetQuote(InstrumentId: new string[] { "NFO:" + strikeprice1, "NFO:" + strikeprice2, "NSE:NIFTY 50" });

                    KeyValuePair<string, Quote> kvpquote1 = quotes.ElementAt(0);
                    KeyValuePair<string, Quote> kvpquote2 = quotes.ElementAt(1);
                    KeyValuePair<string, Quote> kvpnifty = quotes.ElementAt(2);
                    //get bid ask price
                    List<DepthItem> lstbids1 = kvpquote1.Value.Bids;
                    List<DepthItem> lstoffers1 = kvpquote1.Value.Offers;
                    List<DepthItem> lstbids2 = kvpquote2.Value.Bids;
                    List<DepthItem> lstoffers2 = kvpquote2.Value.Offers;



                    decimal ltpdiff1 = Math.Abs(kvpquote1.Value.LastPrice - kvpquote2.Value.LastPrice);
                    //todelete
                    //decimal ltpdiff1 = Convert.ToDecimal(csvobj.LTPDiff);
                    //todelete
                    //decimal ltpdiff1 = Math.Abs(lstbids1[0].Price - lstoffers2[0].Price);

                    if ((nooflots == 0 && ltpdiff1 >= startpivot) ||
                        (ltpdiff1 >= nextpivotup && nextpivotup >= startpivot && Math.Abs(nooflots) < maxlots))
                    {

                        //sell 1 lot
                        //sell 1st strike at bid price and buy 2nd strike one at ask price
                        //if (Math.Abs(bids11 - offers22) >= ltpdiff1)
                        //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                        {
                            //sell 1st strike
                            //Dictionary<string, dynamic> response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //  TradingSymbol: strikeprice1.Replace("NFO:", ""),
                            //  TransactionType: Constants.TRANSACTION_TYPE_SELL,
                            //  Quantity: 75,
                            //  Price: lstbids1[0].Price,
                            //  Validity: Constants.VALIDITY_DAY,
                            //  OrderType: Constants.ORDER_TYPE_LIMIT,
                            //  Variety: Constants.VARIETY_AMO,
                            //  Product: Constants.PRODUCT_NRML);

                            //buy 2nd strike
                            //Dictionary<string, dynamic> response2 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //   TradingSymbol: strikeprice2.Replace("NFO:", ""),
                            //   TransactionType: Constants.TRANSACTION_TYPE_SELL,
                            //    Quantity: 75,
                            //    Price: lstoffers2[0].Price,
                            //    Validity: Constants.VALIDITY_DAY,
                            //     OrderType: Constants.ORDER_TYPE_LIMIT,
                            //     Variety: Constants.VARIETY_AMO,
                            //     Product: Constants.PRODUCT_NRML);

                            dynamic status = "success";
                            dynamic status1 = "success";
                            //response1.TryGetValue("status", out status);
                            //response2.TryGetValue("status", out status1);

                            if (status == "success" && status1 == "success")
                            {
                                nooflots -= 1;
                                currentpivot = nextpivotup;
                                nextpivotup = currentpivot + pivotdelta;
                                nextpivotdown = currentpivot - pivotdelta;
                                currentpivot2 = currentpivot;

                                //todelete
                                string writetofile = string.Format("{0},{1},{2},{3}\n", DateTime.Now, ltpdiff1, nooflots, "SELL");
                                Console.WriteLine(writetofile);
                                File.AppendAllText(filename1path, writetofile);
                                //
                            }
                            else
                            {
                                //try again which ever script failed try to execute the order
                            }

                            //startpivot = 0;
                        }//end of if
                    }
                    if (ltpdiff1 <= nextpivotdown && nooflots < 0)
                    {
                        //buy 1 lot
                        //buy 1st strike at ask price and sell 2nd strike at bid price
                        //if(Math.Abs(offers11-bids22)<ltpdiff1)
                        //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= nextpivotdown)
                        {
                            //Dictionary<string, dynamic> response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            // TradingSymbol: strikeprice1.Replace("NFO:", ""),
                            // TransactionType: Constants.TRANSACTION_TYPE_BUY,
                            // Quantity: 75,
                            // Price: lstoffers1[0].Price,
                            // Validity: Constants.VALIDITY_DAY,
                            // OrderType: Constants.ORDER_TYPE_LIMIT,
                            // Variety: Constants.VARIETY_AMO,
                            // Product: Constants.PRODUCT_NRML);

                            //sell 2nd strike 
                            //Dictionary<string, dynamic> response2 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //   TradingSymbol: strikeprice2.Replace("NFO:", ""),
                            //   TransactionType: Constants.TRANSACTION_TYPE_BUY,
                            //   Quantity: 75,
                            //   Price: lstbids2[0].Price,
                            //   Validity: Constants.VALIDITY_DAY,
                            //   OrderType: Constants.ORDER_TYPE_LIMIT,
                            //   Variety: Constants.VARIETY_AMO,
                            //   Product: Constants.PRODUCT_NRML);

                            dynamic status = "success";
                            dynamic status1 = "success";
                            //response1.TryGetValue("status", out status);
                            // response2.TryGetValue("status", out status1);

                            if (status == "success" && status1 == "success")
                            {

                                nooflots += 1;
                                currentpivot = nextpivotdown;
                                nextpivotup = currentpivot + pivotdelta;
                                nextpivotdown = currentpivot - pivotdelta;
                                currentpivot2 = currentpivot;
                                startpivot = currentpivot1;

                                //todelete
                                string writetofile = string.Format("{0},{1},{2},{3}\n", DateTime.Now, ltpdiff1, nooflots, "BUY");
                                Console.WriteLine(writetofile);
                                File.AppendAllText(filename1path, writetofile);
                                //
                            }
                            else
                            {
                                // execute the trade which failed
                            }
                        }//end of if Math.Abs

                    }
                    //string filecontent1 = string.Format("{0},{1},{2},{3}\n", kvpquote1.Value.Timestamp, kvpquote1.Value.LastPrice, kvpquote2.Value.LastPrice, ltpdiff1);

                    string filecontent1 = string.Format("{0},{1},{2},{3}\n", kvpquote1.Value.Timestamp, kvpquote1.Value.LastPrice, kvpquote2.Value.LastPrice, ltpdiff1);

                    //Console.WriteLine(filecontent);
                    File.AppendAllText(filename2path, filecontent1);
                    sno++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }//end of while

        }//end of function
        /// <summary>
        /// 
        /// </summary>
        /// <param name="csvobj"></param>
        public static void DebitSpreadStrategyWithDummy(object csvobj)
        {
            CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            Kite kite = csvinputobj.kite;
            decimal currentpivot1 = csvinputobj.Pivot;
            decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            int maxlots = csvinputobj.MaxLotSize;
            decimal pivotdelta = csvinputobj.PivotDelta;
            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", threadname,strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", threadname, startpivot);
            string filename1path = string.Format(@"C:\Algotrade\Dummy{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}",threadname, strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData", threadname, startpivot);
            string filename2path = string.Format(@"C:\Algotrade\credit{0}.txt", filename2);

            int nooflots = 0;
            //decimal maxuppivotrange = currentpivot + (pivotdelta * iterations);
            //decimal maxdownpivotrange = currentpivot - (pivotdelta * iterations);
            decimal nextpivotup = startpivot;
            decimal nextpivotdown = startpivot;
            decimal currentpivot = startpivot;
            decimal currentpivot2 = startpivot;
            int sno = 1;

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            //foreach(CsvFileObjects csvobj in lstfileobj)
            {
                try
                {
                    Thread.Sleep(5000);
                    Dictionary<string, Quote> quotes = kite.GetQuote(InstrumentId: new string[] { "NFO:" + strikeprice1, "NFO:" + strikeprice2, "NSE:NIFTY 50" });

                    KeyValuePair<string, Quote> kvpquote1 = quotes.ElementAt(0);
                    KeyValuePair<string, Quote> kvpquote2 = quotes.ElementAt(1);
                    KeyValuePair<string, Quote> kvpnifty = quotes.ElementAt(2);
                    //get bid ask price
                    List<DepthItem> lstbids1 = kvpquote1.Value.Bids;
                    List<DepthItem> lstoffers1 = kvpquote1.Value.Offers;
                    List<DepthItem> lstbids2 = kvpquote2.Value.Bids;
                    List<DepthItem> lstoffers2 = kvpquote2.Value.Offers;



                    decimal ltpdiff1 = Math.Abs(kvpquote1.Value.LastPrice - kvpquote2.Value.LastPrice);
                    //todelete
                    //decimal ltpdiff1 = Convert.ToDecimal(csvobj.LTPDiff);
                    //todelete
                    //decimal ltpdiff1 = Math.Abs(lstbids1[0].Price - lstoffers2[0].Price);
                    if (ltpdiff1 <= nextpivotdown && nooflots < 0)
                    {
                        //buy 1 lot
                        //buy 1st strike at ask price and sell 2nd strike at bid price
                        //if(Math.Abs(offers11-bids22)<ltpdiff1)
                        //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= nextpivotdown)
                        {
                            //Dictionary<string, dynamic> response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            // TradingSymbol: strikeprice1.Replace("NFO:", ""),
                            // TransactionType: Constants.TRANSACTION_TYPE_BUY,
                            // Quantity: 75,
                            // Price: lstoffers1[0].Price,
                            // Validity: Constants.VALIDITY_DAY,
                            // OrderType: Constants.ORDER_TYPE_LIMIT,
                            // Variety: Constants.VARIETY_AMO,
                            // Product: Constants.PRODUCT_NRML);

                            //sell 2nd strike 
                            //Dictionary<string, dynamic> response2 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //   TradingSymbol: strikeprice2.Replace("NFO:", ""),
                            //   TransactionType: Constants.TRANSACTION_TYPE_SELL,
                            //   Quantity: 75,
                            //   Price: lstbids2[0].Price,
                            //   Validity: Constants.VALIDITY_DAY,
                            //   OrderType: Constants.ORDER_TYPE_LIMIT,
                            //   Variety: Constants.VARIETY_AMO,
                            //   Product: Constants.PRODUCT_NRML);

                            dynamic status = "success";
                            dynamic status1 = "success";
                            //response1.TryGetValue("status", out status);
                            // response2.TryGetValue("status", out status1);

                            if (status == "success" && status1 == "success")
                            {

                                nooflots += 1;
                                currentpivot = nextpivotdown;
                                nextpivotup = currentpivot + pivotdelta;
                                nextpivotdown = currentpivot - pivotdelta;
                                currentpivot2 = currentpivot;
                                startpivot = currentpivot1;

                                //todelete
                                string writetofile = string.Format("{0},{1},{2},{3}\n", DateTime.Now, ltpdiff1, nooflots, "BUY");
                                Console.WriteLine(writetofile);
                                File.AppendAllText(filename1path, writetofile);
                                //
                            }
                            else
                            {
                                // execute the trade which failed
                            }
                        }//end of if Math.Abs

                    }//end of if
                    if ((nooflots == 0 && ltpdiff1 >= startpivot) ||
                        (ltpdiff1 >= nextpivotup && nextpivotup >= startpivot && Math.Abs(nooflots) < maxlots))
                    {

                        //sell 1 lot
                        //sell 1st strike at bid price and buy 2nd strike one at ask price
                        //if (Math.Abs(bids11 - offers22) >= ltpdiff1)
                        //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                        {
                            //Dictionary<string, dynamic> response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //  TradingSymbol: strikeprice1.Replace("NFO:", ""),
                            //  TransactionType: Constants.TRANSACTION_TYPE_SELL,
                            //  Quantity: 75,
                            //  Price: lstbids1[0].Price,
                            //  Validity: Constants.VALIDITY_DAY,
                            //  OrderType: Constants.ORDER_TYPE_LIMIT,
                            //  Variety: Constants.VARIETY_AMO,
                            //  Product: Constants.PRODUCT_NRML);

                            //buy 2nd strike
                            //Dictionary<string, dynamic> response2 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //   TradingSymbol: strikeprice2.Replace("NFO:", ""),
                            //   TransactionType: Constants.TRANSACTION_TYPE_BUY,
                            //    Quantity: 75,
                            //    Price: lstoffers2[0].Price,
                            //    Validity: Constants.VALIDITY_DAY,
                            //     OrderType: Constants.ORDER_TYPE_LIMIT,
                            //     Variety: Constants.VARIETY_AMO,
                            //     Product: Constants.PRODUCT_NRML);

                            dynamic status = "success";
                            dynamic status1 = "success";
                            //response1.TryGetValue("status", out status);
                            //response2.TryGetValue("status", out status1);

                            if (status == "success" && status1 == "success")
                            {
                                nooflots -= 1;
                                currentpivot = nextpivotup;
                                nextpivotup = currentpivot + pivotdelta;
                                nextpivotdown = currentpivot - pivotdelta;
                                currentpivot2 = currentpivot;

                                //todelete
                                string writetofile = string.Format("{0},{1},{2},{3}\n", DateTime.Now, ltpdiff1, nooflots, "SELL");
                                Console.WriteLine(writetofile);
                                File.AppendAllText(filename1path, writetofile);
                                //
                            }
                            else
                            {
                                //try again which ever script failed try to execute the order
                            }

                            //startpivot = 0;
                        }//end of if
                    }
                    
                    //string filecontent1 = string.Format("{0},{1},{2},{3}\n", kvpquote1.Value.Timestamp, kvpquote1.Value.LastPrice, kvpquote2.Value.LastPrice, ltpdiff1);

                    string filecontent1 = string.Format("{0},{1},{2},{3}\n", kvpquote1.Value.Timestamp, kvpquote1.Value.LastPrice, kvpquote2.Value.LastPrice, ltpdiff1);

                    //Console.WriteLine(filecontent);
                    File.AppendAllText(filename2path, filecontent1);
                    sno++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }//end of while

        }//end of function
        public static  void OIVolumeInfo(object csvobj)

        {
            
            CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            Kite kite = csvinputobj.kite;
            decimal currentpivot1 = csvinputobj.Pivot;
            decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = csvinputobj.Strike1;           
            string threadname = Thread.CurrentThread.Name;

            //string filename2 = string.Format("{0}-{1}_{2}_{3}", strikeprice1,  DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData", threadname);
            //string filename2path = string.Format(@"C:\Algotrade\outputfile\OI_25_08_20.txt", filename2);
            string filename2path = @"C:\Algotrade\outputfile\OI_25_08_20.txt";
            //string filename2header = "Timestamp,Strikeprice,LastPrice, OIdata,totalVolume,Lotsize,Totallot\n";
            //File.AppendAllText(filename2path, filename2header);

            int count = 1;
            //decimal maxuppivotrange = currentpivot + (pivotdelta * iterations);
            //decimal maxdownpivotrange = currentpivot - (pivotdelta * iterations);
            decimal nextpivotup = startpivot;
            decimal nextpivotdown = startpivot;
            decimal currentpivot = startpivot;
            decimal currentpivot2 = startpivot;
            int sno = 1;
            long lotsize = csvinputobj.lotSize;

            //while (!(DateTime.Now.Hour==15 && DateTime.Now.Minute==30))
            //foreach(CsvFileObjects csvobj in lstfileobj)
            {
                try
                {
                    
                    Dictionary<string, Quote> quotes1 = kite.GetQuote(InstrumentId: new string[] { "NFO:" + strikeprice1 });
                    //Dictionary<string, Quote> quotes2= kite.GetQuote(InstrumentId: new string[] { "NFO:" + strikeprice2 });
                    KeyValuePair<string, Quote> kvpquote1 = quotes1.ElementAt(0);
                    //KeyValuePair<string, Quote> kvpquote2 = quotes2.ElementAt(0);
                    //KeyValuePair<string, Quote> kvpnifty = quotes.ElementAt(2);
                    //get bid ask price
                    List<DepthItem> lstbids1 = kvpquote1.Value.Bids;
                    List<DepthItem> lstoffers1 = kvpquote1.Value.Offers;
                    //List<DepthItem> lstbids2 = kvpquote2.Value.Bids;
                    //List<DepthItem> lstoffers2 = kvpquote2.Value.Offers;

                    decimal OIdata1= kvpquote1.Value.OI;
                    decimal totalVolume1 = kvpquote1.Value.Volume;

                    //decimal OIdata2 = kvpquote2.Value.OI;
                    //decimal totalVolume2 = kvpquote2.Value.Volume;


                    //decimal ltpdiff1 = Math.Abs(kvpquote1.Value.LastPrice - kvpquote2.Value.LastPrice);


                    //string filecontent1 = string.Format("{0},{1},{2},{3}\n", kvpquote1.Value.Timestamp, kvpquote1.Value.LastPrice, kvpquote2.Value.LastPrice, ltpdiff1);

                    string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6}\n", DateTime.Now.ToString(), strikeprice1, kvpquote1.Value.LastPrice, OIdata1, totalVolume1, lotsize,(totalVolume1 / lotsize));

                    //Console.WriteLine(filecontent1);
                    File.AppendAllText(filename2path, filecontent1);
                    sno++;
                                       
                    //Thread.Sleep(60*60*1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }//end of while

        }//end of function
        public static void DebitStraddleStrategyWithDummy(object csvobj)
        {
            CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            Kite kite = csvinputobj.kite;
            decimal currentpivot1 = csvinputobj.Pivot;
            decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            int maxlots = csvinputobj.MaxLotSize;
            decimal pivotdelta = csvinputobj.PivotDelta;
            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", threadname);
            string filename1path = string.Format(@"C:\Algotrade\Dummy{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData", threadname);
            string filename2path = string.Format(@"C:\Algotrade\credit{0}.txt", filename2);

            int nooflots = 0;
            //decimal maxuppivotrange = currentpivot + (pivotdelta * iterations);
            //decimal maxdownpivotrange = currentpivot - (pivotdelta * iterations);
            decimal nextpivotup = startpivot;
            decimal nextpivotdown = startpivot;
            decimal currentpivot = startpivot;
            decimal currentpivot2 = startpivot;
            int sno = 1;

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            //foreach(CsvFileObjects csvobj in lstfileobj)
            {
                try
                {
                    Thread.Sleep(2000);
                    Dictionary<string, Quote> quotes = kite.GetQuote(InstrumentId: new string[] { "NFO:" + strikeprice1, "NFO:" + strikeprice2, "NSE:NIFTY 50" });

                    KeyValuePair<string, Quote> kvpquote1 = quotes.ElementAt(0);
                    KeyValuePair<string, Quote> kvpquote2 = quotes.ElementAt(1);
                    KeyValuePair<string, Quote> kvpnifty = quotes.ElementAt(2);
                    //get bid ask price
                    List<DepthItem> lstbids1 = kvpquote1.Value.Bids;
                    List<DepthItem> lstoffers1 = kvpquote1.Value.Offers;
                    List<DepthItem> lstbids2 = kvpquote2.Value.Bids;
                    List<DepthItem> lstoffers2 = kvpquote2.Value.Offers;



                    decimal ltpdiff1 = Math.Abs(kvpquote1.Value.LastPrice - kvpquote2.Value.LastPrice);
                    //todelete
                    //decimal ltpdiff1 = Convert.ToDecimal(csvobj.LTPDiff);
                    //todelete
                    //decimal ltpdiff1 = Math.Abs(lstbids1[0].Price - lstoffers2[0].Price);

                    if ((nooflots == 0 && ltpdiff1 <= startpivot) ||
                        (ltpdiff1 <= nextpivotup && nextpivotup <= startpivot && Math.Abs(nooflots) < maxlots))
                    {

                        //sell 1 lot
                        //sell 1st strike at bid price and buy 2nd strike one at ask price
                        //if (Math.Abs(bids11 - offers22) >= ltpdiff1)
                        //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                        {
                            //Dictionary<string, dynamic> response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //  TradingSymbol: strikeprice1.Replace("NFO:", ""),
                            //  TransactionType: Constants.TRANSACTION_TYPE_SELL,
                            //  Quantity: 75,
                            //  Price: lstbids1[0].Price,
                            //  Validity: Constants.VALIDITY_DAY,
                            //  OrderType: Constants.ORDER_TYPE_LIMIT,
                            //  Variety: Constants.VARIETY_AMO,
                            //  Product: Constants.PRODUCT_NRML);

                            //buy 2nd strike
                            //Dictionary<string, dynamic> response2 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //   TradingSymbol: strikeprice2.Replace("NFO:", ""),
                            //   TransactionType: Constants.TRANSACTION_TYPE_BUY,
                            //    Quantity: 75,
                            //    Price: lstoffers2[0].Price,
                            //    Validity: Constants.VALIDITY_DAY,
                            //     OrderType: Constants.ORDER_TYPE_LIMIT,
                            //     Variety: Constants.VARIETY_AMO,
                            //     Product: Constants.PRODUCT_NRML);

                            dynamic status = "success";
                            dynamic status1 = "success";
                            //response1.TryGetValue("status", out status);
                            //response2.TryGetValue("status", out status1);

                            if (status == "success" && status1 == "success")
                            {
                                nooflots += 1;
                                currentpivot = nextpivotup;
                                nextpivotup = currentpivot + pivotdelta;
                                nextpivotdown = currentpivot - pivotdelta;
                                currentpivot2 = currentpivot;

                                //todelete
                                string writetofile = string.Format("{0},{1},{2},{3}\n", sno, ltpdiff1, nooflots, "SELL");
                                Console.WriteLine(writetofile);
                                File.AppendAllText(filename1path, writetofile);
                                //
                            }
                            else
                            {
                                //try again which ever script failed try to execute the order
                            }

                            //startpivot = 0;
                        }//end of if
                    }
                    if (ltpdiff1 >= nextpivotdown && nooflots > 0)
                    {
                        //buy 1 lot
                        //buy 1st strike at ask price and sell 2nd strike at bid price
                        //if(Math.Abs(offers11-bids22)<ltpdiff1)
                        //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= nextpivotdown)
                        {
                            //Dictionary<string, dynamic> response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            // TradingSymbol: strikeprice1.Replace("NFO:", ""),
                            // TransactionType: Constants.TRANSACTION_TYPE_BUY,
                            // Quantity: 75,
                            // Price: lstoffers1[0].Price,
                            // Validity: Constants.VALIDITY_DAY,
                            // OrderType: Constants.ORDER_TYPE_LIMIT,
                            // Variety: Constants.VARIETY_AMO,
                            // Product: Constants.PRODUCT_NRML);

                            //sell 2nd strike 
                            //Dictionary<string, dynamic> response2 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                            //   TradingSymbol: strikeprice2.Replace("NFO:", ""),
                            //   TransactionType: Constants.TRANSACTION_TYPE_SELL,
                            //   Quantity: 75,
                            //   Price: lstbids2[0].Price,
                            //   Validity: Constants.VALIDITY_DAY,
                            //   OrderType: Constants.ORDER_TYPE_LIMIT,
                            //   Variety: Constants.VARIETY_AMO,
                            //   Product: Constants.PRODUCT_NRML);

                            dynamic status = "success";
                            dynamic status1 = "success";
                            //response1.TryGetValue("status", out status);
                            // response2.TryGetValue("status", out status1);

                            if (status == "success" && status1 == "success")
                            {

                                nooflots += 1;
                                currentpivot = nextpivotdown;
                                nextpivotup = currentpivot + pivotdelta;
                                nextpivotdown = currentpivot - pivotdelta;
                                currentpivot2 = currentpivot;
                                startpivot = currentpivot1;

                                //todelete
                                string writetofile = string.Format("{0},{1},{2},{3}\n", sno, ltpdiff1, nooflots, "BUY");
                                Console.WriteLine(writetofile);
                                File.AppendAllText(filename1path, writetofile);
                                //
                            }
                            else
                            {
                                // execute the trade which failed
                            }
                        }//end of if Math.Abs

                    }
                    //string filecontent1 = string.Format("{0},{1},{2},{3}\n", kvpquote1.Value.Timestamp, kvpquote1.Value.LastPrice, kvpquote2.Value.LastPrice, ltpdiff1);

                    string filecontent1 = string.Format("{0},{1},{2},{3}\n", kvpquote1.Value.Timestamp, kvpquote1.Value.LastPrice, kvpquote2.Value.LastPrice, ltpdiff1);

                    //Console.WriteLine(filecontent);
                    File.AppendAllText(filename2path, filecontent1);
                    sno++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }//end of while

        }//end of function

        public static void WriteContenttoFile(string filepath,string filecontent)
        {

        }

        private static object _lockobj = new object();
        /// <summary>
        /// Get the accesstoken or create a accesstoken and store it to a file
        /// </summary>
        public static void GetIIFLMarketAccessToken()
        {
            //Read file from the directory, if file is from current date then read or else create a new key
            //need to lock to avoid race condition
            lock (_lockobj)
            {
                string marketfilename = @"c:\algotrade\key\iiflmarketkey.txt";

                //check if the file is generated today
                if (File.Exists(marketfilename) &&
                    File.GetLastWriteTime(marketfilename).Date == DateTime.Today)
                {
                    //read and set the marketdata key
                    //check if the file is generated today
                    string marketaccesstoken = File.ReadAllText(marketfilename);
                    IIFLConnect iiflconnect = new IIFLConnect();

                    _accessTokenMarket = marketaccesstoken;
                }
                else
                {
                    IIFLConnect iiflconnect = new IIFLConnect();
                    IIFLUser iifluser = iiflconnect.GenerateSessionMarket();
                    File.WriteAllText(marketfilename, iifluser.AccessToken);
                    _accessTokenMarket = iifluser.AccessToken;
                }
            }
                  

        }
        private static object _lockobj1 = new object();

        public static void GetIIFLInteractiveAccessToken()
        {
            //need to lock to avoid race condition
            lock (_lockobj1)
            {
                string interactivefilename = @"c:\algotrade\key\iiflinteractivekey.txt";
                if (File.Exists(interactivefilename) &&
                   File.GetLastWriteTime(interactivefilename).Date == DateTime.Today)
                {
                    string interactiveaccesstoken = File.ReadAllText(interactivefilename);
                    _accessTokenInteractive = interactiveaccesstoken;

                }
                else
                {
                    IIFLConnect iiflconnect = new IIFLConnect();
                    IIFLUser iifluser = iiflconnect.GenerateSessionInteractive();
                    File.WriteAllText(interactivefilename, iifluser.AccessToken);
                    _accessTokenInteractive = iifluser.AccessToken;

                }
            }
        }
        /// <summary>
        /// get the enctoken by logging in to zerodha site programatically
        /// </summary>
        public static void GetZerodhaNewInteractiveAccessToken()
        {
            //need to lock to avoid race condition
            lock (_lockobj1)
            {
                string zkeyfilename = string.Format(@"c:\algotrade\key\zerodhanewkey_{0}.txt", zerodhaid);
                if (File.Exists(zkeyfilename) &&
                   File.GetLastWriteTime(zkeyfilename).Date == DateTime.Today)
                {
                    _zerodhanewAccessToken = File.ReadAllText(zkeyfilename);                    

                }
                else
                {
                    _zerodhanewAccessToken = LogintoKite(zerodhaid, zerodhapass, zerodhapin);
                    File.WriteAllText(zkeyfilename, _zerodhanewAccessToken);

                }
             
            }
        }
        /// <summary>
        /// This strategy is only for Bank Nifty Options
        /// </summary>
        /// <param name="csvobj"></param>
        public static void BNLongStraddleStrategyLive()
        {
            //CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            //decimal currentpivot1 = csvinputobj.Pivot;
            //decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = "NIFTY BANK";
            //string strikeprice2 = csvinputobj.Strike2;

            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;           
            //delete existing files

            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("LST-{0}-{1}_{2}", strikeprice1,  DateTime.Now.ToString("dd-MM-yyyy"), "TradeData");
            string filename1path = string.Format(@"C:\Algotrade\scalperoutput\Live{0}.csv", filename1);
            string filecontent = "DateTime,OrderID,Bid/Ask,InstrumentId,Price,OrderType,LineNo,PivotDown,PivotUp,Maxlots\r\n";

            File.AppendAllText(filename1path, filecontent);

            string filename2 = string.Format("LST-{0}-{1}_{2}", strikeprice1, DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");


            string filename3 = string.Format("LST-{0}-{1}_{2}", strikeprice1, DateTime.Now.ToString("dd-MM-yyyy"), "Continous");
                                                                  
            string filename3path = string.Format(@"C:\Algotrade\scalperoutput\Live-{0}.csv", filename3);
            string filecontent3 = "Datetime,LivePrice,Strike,BuyDeltaPrice,SellDeltaPrice,Quantity1,Quantity2,PercentageInc" + threadname + "\r\n";
            File.AppendAllText(filename3path, filecontent3);
            
            string filename2path = string.Format(@"C:\Algotrade\scalperoutput\credit{0}.csv", filename2);
            string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename2path, filecontent2);


            int nooflots = 0;
            int totallots = 5;
           
            //Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            //{
            //    {"strikeprice1", strikeprice1},
            //    //{"strikeprice2",strikeprice2 }
            //};

           
            Thread.Sleep(100);
            //Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = 25;
            string instrumentId1 = "";
            string instrumentId2 = "";
            int alive = 0;
            decimal totalbuyprice = 0.0m;
            decimal totalbidprice = 0.0m;
            TimeSpan end = new TimeSpan(15, 31, 0);
            int maxlots = 1;
            string strike1 = "";
            string strike2 = "";
            bool bfirst = true;
            int strikedelta = 0;
            //while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            //run till 03:30PM
            while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
            {
                try
                {
                    //Thread.Sleep(1000);

                    //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    //instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);
                    decimal prevdayclose = 22370.0m;
                    
                    Dictionary<string, dynamic> quotes = GetQuoteBank(strikeprice1);
                    KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                    dynamic currentliveprice1 = kvpquote.Value.LastPrice;
                    prevdayclose = kvpquote.Value.PrevDayClose;//Previous day close price

                    long currentliveprice = long.Parse(currentliveprice1.ToString());

                    decimal currpricepercentincval = prevdayclose * 1.015m;
                    decimal currpricepercentdecval = prevdayclose *(1.0m- 0.015m);

                  
                    //if (currentliveprice>= currpricepercentincval || currentliveprice<= currpricepercentdecval)
                    {
                        long currentlivepricetens = currentliveprice  % 100;
                        long currentlivepricewhole = (currentliveprice /100) * 100;

                        long currentlivepricewholenext = currentlivepricewhole + 100;
                       

                        long currstrikeprice = 0;
                        //check for which nearest strike to be taken
                        decimal percentfromlowerstrike = ((currentliveprice-currentlivepricewhole)*1.0m / currentlivepricewhole) * 100.0m;
                        decimal percentfromupperstrike = ((currentlivepricewholenext - currentliveprice)*1.0m / currentlivepricewholenext) * 100.0m;

                        if (nooflots == 0)
                        {
                            if (percentfromlowerstrike < percentfromupperstrike)
                            {
                                currstrikeprice = currentlivepricewhole;
                            }
                            else
                            {
                                currstrikeprice = currentlivepricewholenext;
                            }
                            //give strikeprice input
                            //currstrikeprice = 24000;
                            if(currentlivepricetens>=40 && currentlivepricetens<60)
                            {
                                strikedelta = 150;
                            }

                            strike1 = "BANKNIFTY22Oct2020" + (currstrikeprice- strikedelta) + "PE";
                            strike2 = "BANKNIFTY22Oct2020" + (currstrikeprice + strikedelta) + "CE";


                            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                            {
                                {"strikeprice1", strike1},
                                {"strikeprice2",strike2 }
                            };

                            Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid);
                            lotsize = Convert.ToInt32(instrumentids1["lotsize"]);

                            instrumentId1 = Convert.ToString(instrumentids1["strikeprice1"]);
                            instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                        }//end of if

                        Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                        Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);
                        //add nfo 
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                        //get bid ask price

                        dynamic lstbids1 = kvpquote1.Value.Bids;
                        dynamic lstoffers1 = kvpquote1.Value.Offers;
                        dynamic lstbids2 = kvpquote2.Value.Bids;
                        dynamic lstoffers2 = kvpquote2.Value.Offers;
                        int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                        int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                        totalbidprice = (lstbids1[0].Price + lstbids2[0].Price);

                        decimal percentchange = currentliveprice / prevdayclose;
                        if (DateTime.Now.Hour < 15 && nooflots < maxlots
                              /*&&(percentchange >=1.015m || percentchange <= (1.0m-0.015m))*/)
                        {
                            //buy 1 lot
                            //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                            {
                                //buy 2nd strike at bid price and buy 2nd strike one at ask price
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int i = 0; i < 5; ++i)
                                {
                                    try
                                    {
                                        response1 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_BUY,
                                          lstoffers2[0].Price, (lotsize*totallots),"",Constants.PRODUCT_MIS);

                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strike1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }
                                Dictionary<string, dynamic> orderstatus = new Dictionary<string, dynamic>();
                                //orderstatus = GetOrderStatus(response1);//not required

                                //Thread.Sleep(100);

                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                       "", strike2, lstoffers2[0].Price, "BUY", totaltradedquantity2, "LineNo1138");
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() != "cancelled"
                                //        && orderstatus["Status"].ToLower() != "rejected")
                                {
                                    //sell 1st strike
                                    //Thread.Sleep(2000);
                                    Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                    for (int j = 0; j < 5; ++j)
                                    {
                                        try
                                        {
                                            response2 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_BUY,
                                                                              lstbids1[0].Price, (lotsize*totallots), Constants.PRODUCT_MIS);

                                            string writetofile6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                "", strike1, lstoffers1[0].Price, "BUY", totaltradedquantity1, "LineNo1154");
                                            Console.WriteLine(writetofile6);
                                            File.AppendAllText(filename1path, writetofile6);
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                            strike1, strike2);
                                            File.AppendAllText(filename2path, content);
                                        }
                                        //Thread.Sleep(500);
                                    }

                                    Dictionary<string, dynamic> orderstatus2 = new Dictionary<string, dynamic>();



                                    //if (orderstatus2["Status"].ToLower() != "cancelled"
                                    //  && orderstatus2["Status"].ToLower() != "rejected")
                                    {
                                        nooflots += 1;
                                        totalbuyprice = totalbidprice;

                                        string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, lstoffers1[0].Price, lstoffers2[0].Price,
                                                                       totalbuyprice, nooflots, "BUY", currentliveprice, "LineNo1206");
                                        Console.WriteLine(writetofile);
                                        File.AppendAllText(filename1path, writetofile);
                                        //
                                    }

                                }//end of if

                            }
                        }
                        else if (nooflots > 0 && (totalbidprice >= (totalbuyprice * 1.05m)))
                        {

                            //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= nextpivotdown)
                            {
                                //buy 1 lot
                                //buy 1st strike at ask price and sell 2nd strike at bid price   
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int j1 = 0; j1 < 5; ++j1)
                                {
                                    try
                                    {
                                        response1 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_SELL,
                                                                         lstoffers1[0].Price, (lotsize*totallots), Constants.PRODUCT_MIS);

                                        string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID", "",
                                                                                           strike2, lstbids1[0].Price, totaltradedquantity1, "SELL", "LineNo152");
                                        Console.WriteLine(filecontent5);
                                        File.AppendAllText(filename1path, filecontent5);
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strikeprice1, strike2);
                                        File.AppendAllText(filename2path, content);

                                    }
                                    //Thread.Sleep(500);
                                }//else

                                //Thread.Sleep(100);
                                //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);
                                //string filecontent5 = string.Format("{0},{1},{2},{3},{4}-{5}\r\n", DateTime.Now, response1["result"]["AppOrderID"], 
                                //                                                           instrumentId2,lstoffers1[0].Price, "BUY", "LineNo1220");
                                //Console.WriteLine(filecontent5);
                                //File.AppendAllText(filename1path, filecontent5);

                                //sell 2nd strike 
                                Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                for (int j2 = 0; j2 < 5; ++j2)
                                {
                                    try
                                    {
                                        response2 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_SELL,
                                        lstbids2[0].Price, (lotsize * totallots), "", Constants.PRODUCT_MIS);

                                        string filecontent6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID", "", strike2,
                                                                                    lstbids2[0].Price, "SELL", totaltradedquantity2, "LineNo1284");
                                        Console.WriteLine(filecontent6);
                                        File.AppendAllText(filename1path, filecontent6);
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strikeprice1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }

                                //string filecontent6 = string.Format("{0},{1},{2},{3},{4}\n", DateTime.Now, response2["result"]["AppOrderID"],
                                //                                                     lstbids2[0].Price, "SELL", "LineNo1227");
                                //Console.WriteLine(filecontent6);
                                //File.AppendAllText(filename1path, filecontent6);



                                //if (orderstatus["Status"].ToLower() == "complete" && orderstatus6["Status"].ToLower() == "complete")
                                {
                                    //both legs squared off so reduce the no of lots
                                    nooflots -= 1;

                                    decimal percentageincrease = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100.0m, 2);

                                    string writetofile = string.Format("{0},{1},{2},{3},{4}%,{5},{6},{7}\n", DateTime.Now, lstbids1[0].Price,
                                                                     lstbids2[0].Price, totalbidprice, percentageincrease,
                                                                       "SELL", nooflots, "LineNo1245");
                                    Console.WriteLine(writetofile);
                                    File.AppendAllText(filename1path, writetofile);
                                    //
                                }
                                //else
                                //{
                                //    string writetofile = string.Format("{0},{1},{2},{3},{4},{5}\n", DateTime.Now, lstoffers1[0].Price, lstbids2[0].Price,
                                //                                                  (lstoffers1[0].Price - lstbids2[0].Price), nooflots, "FAILED");
                                //    Console.WriteLine(writetofile);
                                //    File.AppendAllText(filename1path, writetofile);
                                //}
                            }//end of if Math.Abs

                        }
                        else if (nooflots > 0 && DateTime.Now.Hour == 14 && DateTime.Now.Minute >= 45)
                        {
                            //square off everything 

                        }
                        decimal totalperceinc = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);
                        string strtotalpercinc = "";
                        if (totalperceinc >= 0)
                        {
                            strtotalpercinc = "+" + totalperceinc.ToString();
                        }
                        else
                        {
                            strtotalpercinc = totalperceinc.ToString();
                        }

                        string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%\r\n", DateTime.Now, currentliveprice,  
                                          strike2, totalbidprice, totalbuyprice, totaltradedquantity1, 
                                                  totaltradedquantity2,strtotalpercinc);

                        File.AppendAllText(filename3path, filecontent1);

                        if (alive % 250 == 0)
                        {
                            Console.WriteLine(DateTime.Now + "-I am alive -" + alive + "-" +  "," + strikeprice1);
                            string filecontent9 = string.Format("{0},Alive-,{1}\r\n", DateTime.Now, alive);
                            //File.AppendAllText(filename2path, filecontent9);
                            Console.WriteLine(filecontent9);
                        }
                        ++alive;


                    }//end of if block line 1173

                    

                }//end of try
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                           strike1, strike2);
                    File.AppendAllText(filename2path, filecontent1);
                }
            }

        }

      
        public static void BNLongStrangleStrategyLive()
        {
            //CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            //decimal currentpivot1 = csvinputobj.Pivot;
            //decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = "NIFTY BANK";
            //string strikeprice2 = csvinputobj.Strike2;

            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;           
            //delete existing files

            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("BN-LStrang{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "TradeData");
            string filename1path = string.Format(@"C:\Algotrade\scalperoutput\Live{0}.csv", filename1);
            string filecontent = "DateTime,OrderID,Bid/Ask,InstrumentId,Price,OrderType,LineNo,PivotDown,PivotUp,Maxlots\r\n";

            File.AppendAllText(filename1path, filecontent);

            string filename2 = string.Format("BN-LStrang-{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");

            string filename3 = string.Format("BN-LStrangle-{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "Continous");

            string filename3path = string.Format(@"C:\Algotrade\scalperoutput\Live-{0}.csv", filename3);
            string filecontent3 = "Datetime,LivePrice,Strike,BuyDeltaPrice,SellDeltaPrice,Quantity1,Quantity2,PercentageInc" + threadname + "\r\n";
            File.AppendAllText(filename3path, filecontent3);

            string filename2path = string.Format(@"C:\Algotrade\scalperoutput\credit{0}.csv", filename2);
            string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename2path, filecontent2);


            

            //Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            //{
            //    {"strikeprice1", strikeprice1},
            //    //{"strikeprice2",strikeprice2 }
            //};


            Thread.Sleep(100);
            //Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = 25;
            string instrumentId1 = "";
            string instrumentId2 = "";
            int alive = 0;
            decimal totalbuyprice = 0.0m;
            decimal totalbidprice = 0.0m;
            TimeSpan end = new TimeSpan(15, 31, 0);
            
            string strike1 = "";
            string strike2 = "";
            string strike3 = "";
            string strike4 = "";
            bool bfirst = true;
            totalbuyprice = 774.85m;
            int strikedelta = 0;
            int nooflots = 0;
            int totallots = 10;
            int maxlots = 20;

            //int strikedelta2 = 0;
            //while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            //run till 03:30PM
            while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
            {
                try
                {
                    //Thread.Sleep(1000);

                    //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    //instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);
                    decimal prevdayclose = 22370.0m;

                    Dictionary<string, dynamic> quotes = GetQuoteBank(strikeprice1);
                    KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                    dynamic currentliveprice1 = kvpquote.Value.LastPrice;
                    prevdayclose = kvpquote.Value.PrevDayClose;//Previous day close price

                    long currentliveprice = long.Parse(currentliveprice1.ToString());

                    decimal currpricepercentincval = prevdayclose * 1.015m;
                    decimal currpricepercentdecval = prevdayclose * (1.0m - 0.015m);


                    //if (currentliveprice>= currpricepercentincval || currentliveprice<= currpricepercentdecval)
                    {
                        long currentlivepricetens = currentliveprice % 100;
                        long currentlivepricewhole = (currentliveprice / 100) * 100;

                        long currentlivepricewholenext = currentlivepricewhole + 100;


                        long currstrikeprice = 0;
                        //check for which nearest strike to be taken
                        decimal percentfromlowerstrike = ((currentliveprice - currentlivepricewhole) * 1.0m / currentlivepricewhole) * 100.0m;
                        decimal percentfromupperstrike = ((currentlivepricewholenext - currentliveprice) * 1.0m / currentlivepricewholenext) * 100.0m;

                        if (nooflots == 0)
                        {
                           
                            if (percentfromlowerstrike < percentfromupperstrike)
                            {
                                currstrikeprice = currentlivepricewhole;
                            }
                            else
                            {
                                currstrikeprice = currentlivepricewholenext;
                            }
                            //give strikeprice input
                            //currstrikeprice = 24000;
                           

                            strike1 = "BANKNIFTY22Oct2020" + (currstrikeprice - strikedelta) + "PE";
                            strike2 = "BANKNIFTY22Oct2020" + (currstrikeprice + strikedelta) + "CE";                           



                            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                            {
                                {"strikeprice1", strike1},
                                {"strikeprice2",strike2 }                                
                            };

                            Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid);
                            lotsize = Convert.ToInt32(instrumentids1["lotsize"]);

                            instrumentId1 = Convert.ToString(instrumentids1["strikeprice1"]);
                            instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                          
                        }//end of if

                        Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                        Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);
                        //add nfo 
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                        //get bid ask price

                        dynamic lstbids1 = kvpquote1.Value.Bids;
                        dynamic lstoffers1 = kvpquote1.Value.Offers;
                        dynamic lstbids2 = kvpquote2.Value.Bids;
                        dynamic lstoffers2 = kvpquote2.Value.Offers;
                        int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                        int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                        totalbidprice = (lstbids1[0].Price + lstbids2[0].Price);

                        decimal percentchange = currentliveprice / prevdayclose;
                        if (nooflots < maxlots
                              /*&&(percentchange >=1.015m || percentchange <= (1.0m-0.015m))*/)
                        {
                            //buy 1 lot
                            //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                            {
                                //buy 2nd strike at bid price and buy 2nd strike one at ask price
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int i = 0; i < 5; ++i)
                                {
                                    try
                                    {
                                        //response1 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_BUY,
                                         // lstoffers2[0].Price, (lotsize * totallots), "", Constants.PRODUCT_MIS);

                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strike1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }
                                Dictionary<string, dynamic> orderstatus = new Dictionary<string, dynamic>();
                                //orderstatus = GetOrderStatus(response1);//not required

                                //Thread.Sleep(100);

                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                       "", strike2, lstoffers2[0].Price, "BUY", totaltradedquantity2, "LineNo1138");
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() != "cancelled"
                                //        && orderstatus["Status"].ToLower() != "rejected")
                                {
                                    //sell 1st strike
                                    //Thread.Sleep(2000);
                                    Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                    for (int j = 0; j < 5; ++j)
                                    {
                                        try
                                        {
                                            response2 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_BUY,
                                                                              lstbids1[0].Price, (lotsize * totallots), Constants.PRODUCT_MIS);

                                            string writetofile6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                "", strike1, lstoffers1[0].Price, "BUY", totaltradedquantity1, "LineNo1154");
                                            Console.WriteLine(writetofile6);
                                            File.AppendAllText(filename1path, writetofile6);
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                            strike1, strike2);
                                            File.AppendAllText(filename2path, content);
                                        }
                                        //Thread.Sleep(500);
                                    }

                                    Dictionary<string, dynamic> orderstatus2 = new Dictionary<string, dynamic>();



                                    //if (orderstatus2["Status"].ToLower() != "cancelled"
                                    //  && orderstatus2["Status"].ToLower() != "rejected")
                                    {
                                        nooflots += totallots;
                                        totalbuyprice = totalbidprice;

                                        string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, lstoffers1[0].Price, lstoffers2[0].Price,
                                                                       totalbuyprice, nooflots, "BUY", currentliveprice, "LineNo1206");
                                        Console.WriteLine(writetofile);
                                        File.AppendAllText(filename1path, writetofile);
                                        //
                                    }

                                }//end of if

                            }
                        }
                        else if (nooflots > 0 && (totalbidprice >= (totalbuyprice * 1.05m)))
                        {

                            //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= nextpivotdown)
                            {
                                //buy 1 lot
                                //buy 1st strike at ask price and sell 2nd strike at bid price   
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int j1 = 0; j1 < 5; ++j1)
                                {
                                    try
                                    {
                                        response1 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_SELL,
                                                                         lstoffers1[0].Price, (lotsize * totallots), Constants.PRODUCT_MIS);

                                        string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID", "",
                                                                                           strike2, lstbids1[0].Price, totaltradedquantity1, "SELL", "LineNo152");
                                        Console.WriteLine(filecontent5);
                                        File.AppendAllText(filename1path, filecontent5);
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strikeprice1, strike2);
                                        File.AppendAllText(filename2path, content);

                                    }
                                    //Thread.Sleep(500);
                                }//else

                                //Thread.Sleep(100);
                                //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);
                                //string filecontent5 = string.Format("{0},{1},{2},{3},{4}-{5}\r\n", DateTime.Now, response1["result"]["AppOrderID"], 
                                //                                                           instrumentId2,lstoffers1[0].Price, "BUY", "LineNo1220");
                                //Console.WriteLine(filecontent5);
                                //File.AppendAllText(filename1path, filecontent5);

                                //sell 2nd strike 
                                Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                for (int j2 = 0; j2 < 5; ++j2)
                                {
                                    try
                                    {
                                        response2 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_SELL,
                                        lstbids2[0].Price, (lotsize * totallots), "", Constants.PRODUCT_MIS);

                                        string filecontent6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID", "", strike2,
                                                                                    lstbids2[0].Price, "SELL", totaltradedquantity2, "LineNo1284");
                                        Console.WriteLine(filecontent6);
                                        File.AppendAllText(filename1path, filecontent6);
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strikeprice1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }

                                //string filecontent6 = string.Format("{0},{1},{2},{3},{4}\n", DateTime.Now, response2["result"]["AppOrderID"],
                                //                                                     lstbids2[0].Price, "SELL", "LineNo1227");
                                //Console.WriteLine(filecontent6);
                                //File.AppendAllText(filename1path, filecontent6);



                                //if (orderstatus["Status"].ToLower() == "complete" && orderstatus6["Status"].ToLower() == "complete")
                                {
                                    //both legs squared off so reduce the no of lots
                                    nooflots -= 1;

                                    decimal percentageincrease = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100.0m, 2);

                                    string writetofile = string.Format("{0},{1},{2},{3},{4}%,{5},{6},{7}\n", DateTime.Now, lstbids1[0].Price,
                                                                     lstbids2[0].Price, totalbidprice, percentageincrease,
                                                                       "SELL", nooflots, "LineNo1245");
                                    Console.WriteLine(writetofile);
                                    File.AppendAllText(filename1path, writetofile);
                                    break;
                                    //
                                }
                                //else
                                //{
                                //    string writetofile = string.Format("{0},{1},{2},{3},{4},{5}\n", DateTime.Now, lstoffers1[0].Price, lstbids2[0].Price,
                                //                                                  (lstoffers1[0].Price - lstbids2[0].Price), nooflots, "FAILED");
                                //    Console.WriteLine(writetofile);
                                //    File.AppendAllText(filename1path, writetofile);
                                //}
                            }//end of if Math.Abs

                        }
                        else if (nooflots > 0 && DateTime.Now.Hour == 14 && DateTime.Now.Minute >= 45)
                        {
                            //square off everything 

                        }
                        decimal totalperceinc = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);
                        string strtotalpercinc = "";
                        if (totalperceinc >= 0)
                        {
                            strtotalpercinc = "+" + totalperceinc.ToString();
                        }
                        else
                        {
                            strtotalpercinc = totalperceinc.ToString();
                        }

                        string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%\r\n", DateTime.Now, currentliveprice,
                                          strike2, totalbidprice, totalbuyprice, totaltradedquantity1,
                                                  totaltradedquantity2, strtotalpercinc);

                        File.AppendAllText(filename3path, filecontent1);

                        if (alive % 250 == 0)
                        {
                            Console.WriteLine(DateTime.Now + "-I am alive -" + alive + "-" + "," + strikeprice1);
                            string filecontent9 = string.Format("{0},Alive-,{1}\r\n", DateTime.Now, alive);
                            //File.AppendAllText(filename2path, filecontent9);
                            Console.WriteLine(filecontent9);
                        }
                        ++alive;


                    }//end of if block line 1173



                }//end of try
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                           strike1, strike2);
                    File.AppendAllText(filename2path, filecontent1);
                }
            }

        }

        public static void BNLongStraddleStrategyMultiStrikeLive()
        {
            //CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            //decimal currentpivot1 = csvinputobj.Pivot;
            //decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = "NIFTY BANK";
            //string strikeprice2 = csvinputobj.Strike2;

            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;           
            //delete existing files

            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("BN-LStrang{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "TradeData");
            string filename1path = string.Format(@"C:\Algotrade\scalperoutput\Live{0}.csv", filename1);
            string filecontent = "DateTime,OrderID,Bid/Ask,InstrumentId,Price,OrderType,LineNo,PivotDown,PivotUp,Maxlots\r\n";

            File.AppendAllText(filename1path, filecontent);

            string filename2 = string.Format("BN-LStrang-{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");

            string filename3 = string.Format("BN-LStrangle-{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "Continous");

            string filename3path = string.Format(@"C:\Algotrade\scalperoutput\Live-{0}.csv", filename3);
            string filecontent3 = "Datetime,LivePrice,Strike,BuyDeltaPrice,SellDeltaPrice,Quantity1,Quantity2,PercentageInc" + threadname + "\r\n";
            File.AppendAllText(filename3path, filecontent3);

            string filename2path = string.Format(@"C:\Algotrade\scalperoutput\credit{0}.csv", filename2);
            string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename2path, filecontent2);





            //Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            //{
            //    {"strikeprice1", strikeprice1},
            //    //{"strikeprice2",strikeprice2 }
            //};


            Thread.Sleep(100);
            //Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = 25;
            string instrumentId1 = "";
            string instrumentId2 = "";
            string instrumentId3 = "";
            string instrumentId4 = "";
            int alive = 0;
            decimal totalbuyprice = 0.0m;
            decimal totalbidprice = 0.0m;
            TimeSpan end = new TimeSpan(15, 31, 0);

            string strike1 = "";
            string strike2 = "";
            string strike3 = "";
            string strike4 = "";
            bool bfirst = true;
            totalbuyprice = 0.0m;
            int strikedelta = 0;
            int totallots = 10;
            int nooflots = 0;
            int maxlots = 1;

            while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
            {
                try
                {
                    //Thread.Sleep(1000);

                    //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    //instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);
                    //decimal prevdayclose = 22370.0m;

                    Dictionary<string, dynamic> quotes = GetQuoteBank(strikeprice1);
                    KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                    dynamic currentliveprice1 = kvpquote.Value.LastPrice;
                    decimal prevdayclose = kvpquote.Value.PrevDayClose;//Previous day close price

                    long currentliveprice = long.Parse(currentliveprice1.ToString());


                    decimal currpricepercentincval = prevdayclose * 1.015m;
                    decimal currpricepercentdecval = prevdayclose * (1.0m - 0.015m);


                    //if (currentliveprice>= currpricepercentincval || currentliveprice<= currpricepercentdecval)
                    {
                        long currentlivepricetens = currentliveprice % 100;
                        long currentlivepricewhole = (currentliveprice / 100) * 100;

                        long currentlivepricewholenext = currentlivepricewhole + 100;


                        long currstrikeprice = 0;
                        //check for which nearest strike to be taken
                        decimal percentfromlowerstrike = ((currentliveprice - currentlivepricewhole) * 1.0m / currentlivepricewhole) * 100.0m;
                        decimal percentfromupperstrike = ((currentlivepricewholenext - currentliveprice) * 1.0m / currentlivepricewholenext) * 100.0m;

                        if (nooflots == 0)
                        {

                            if (percentfromlowerstrike < percentfromupperstrike)
                            {
                                currstrikeprice = currentlivepricewhole;
                            }
                            else
                            {
                                currstrikeprice = currentlivepricewholenext;
                            }

                            //give strikeprice input
                            //currstrikeprice = 24500;

                            Dictionary<string, string> dictParamStrikes = GetStrikePrices((currstrikeprice).ToString(),
                                             (currstrikeprice).ToString(), _broker,"BANKNIFTY");

                            strike1 = dictParamStrikes["strike1"];
                            strike2 = dictParamStrikes["strike2"];

                            //strike3 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta - 100)) + "CE";
                            //strike4 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta + 100)) + "CE";

                            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                            {
                                {"strikeprice1", strike1},
                                {"strikeprice2",strike2 },
                                //{"strikeprice3",strike3 },
                                //{"strikeprice4",strike4 }
                            };

                            Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid);
                            lotsize = Convert.ToInt32(instrumentids1["lotsize"]);

                            //lotsize = 25;
                            //if (_broker == "iifl")
                            {
                                instrumentId1 = Convert.ToString(instrumentids1["strikeprice1"]);
                                instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                                //instrumentId1 = Convert.ToString(instrumentids1["strikeprice3"]);
                                // instrumentId2 = Convert.ToString(instrumentids1["strikeprice4"]);
                            }
                            

                        }//end of if

                        Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                        Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                        //add nfo 
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                        dynamic lstbids1 = kvpquote1.Value.Bids;
                        dynamic lstoffers1 = kvpquote1.Value.Offers;
                        dynamic lstbids2 = kvpquote2.Value.Bids;
                        dynamic lstoffers2 = kvpquote2.Value.Offers;


                        int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                        int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                        totalbidprice = (lstoffers1[0].Price + lstoffers2[0].Price);

                        if (_broker != "iifl")
                        {
                            Dictionary<string, string> dictParamStrikes1 = GetStrikePrices((currstrikeprice).ToString(),
                                           (currstrikeprice).ToString(), "zerodhanew");
                            strike1 = dictParamStrikes1["strike1"];
                            strike2 = dictParamStrikes1["strike2"];

                        }
                        else
                        {
                            strike1 = instrumentId1;
                            strike2 = instrumentId2;
                        }

                        decimal percentchange = currentliveprice / prevdayclose;
                        if (nooflots < maxlots && totalbidprice <= 530.0m
                              /*&&(percentchange >=1.015m || percentchange <= (1.0m-0.015m))*/)
                        {
                            //buy 1 lot
                            //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                            {
                                //buy 2nd strike at bid price and buy 2nd strike one at ask price
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int i = 0; i < 5; ++i)
                                {

                                    try
                                    {
                                        response1 = PlaceOrderOption(strike1, Constants.TRANSACTION_TYPE_BUY,
                                          lstoffers1[0].Price, (lotsize * totallots), "", Constants.PRODUCT_NRML);
                                        break;

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strike1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }
                                Dictionary<string, dynamic> orderstatus = new Dictionary<string, dynamic>();
                                //orderstatus = GetOrderStatus(response1);//not required

                                //Thread.Sleep(100);

                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                       "", strike1, lstoffers1[0].Price, "BUY",
                                                                                       totaltradedquantity1, "LineNo2082");
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() != "cancelled"
                                //        && orderstatus["Status"].ToLower() != "rejected")
                                {
                                    //sell 1st strike
                                    //Thread.Sleep(2000);
                                    Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                    for (int j = 0; j < 5; ++j)
                                    {
                                        try
                                        {
                                            response2 = PlaceOrderOption(strike2, Constants.TRANSACTION_TYPE_BUY,
                                                                       lstoffers2[0].Price, (lotsize * totallots), Constants.PRODUCT_NRML);

                                            break;

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                            strike1, strike2);
                                            File.AppendAllText(filename2path, content);
                                        }
                                        //Thread.Sleep(500);
                                    }

                                    string writetofile6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                "", strike2, lstoffers2[0].Price, "BUY", totaltradedquantity2, "LineNo2113");
                                    Console.WriteLine(writetofile6);
                                    File.AppendAllText(filename1path, writetofile6);

                                    Dictionary<string, dynamic> orderstatus2 = new Dictionary<string, dynamic>();


                                    //if (orderstatus2["Status"].ToLower() != "cancelled"
                                    //  && orderstatus2["Status"].ToLower() != "rejected")
                                    {
                                        nooflots += totallots;
                                        totalbuyprice = totalbidprice;

                                        string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, lstoffers1[0].Price, lstoffers2[0].Price,
                                                                       totalbuyprice, nooflots, "BUY", currentliveprice, "LineNo1206");
                                        Console.WriteLine(writetofile);
                                        File.AppendAllText(filename1path, writetofile);
                                        break;

                                        //
                                    }

                                }//end of if

                            }
                        }
                        else if (nooflots > 0 && DateTime.Now.Hour == 14 && DateTime.Now.Minute >= 45)
                        {
                            //square off everything 

                        }

                        if (totalbuyprice == 0m)
                        {
                            totalbuyprice = totalbidprice;
                        }

                        decimal totalperceinc = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);
                        string strtotalpercinc = "";
                        if (totalperceinc >= 0)
                        {
                            strtotalpercinc = "+" + totalperceinc.ToString();
                        }
                        else
                        {
                            strtotalpercinc = totalperceinc.ToString();
                        }

                        string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%\r\n", DateTime.Now, currentliveprice,
                                          strike2, totalbidprice, totalbuyprice, totaltradedquantity1,
                                                  totaltradedquantity2, strtotalpercinc);

                        Console.WriteLine(filecontent1);

                        File.AppendAllText(filename3path, filecontent1);

                        if (alive % 250 == 0)
                        {
                            Console.WriteLine(DateTime.Now + "-I am alive -" + alive + "-" + "," + strikeprice1);
                            string filecontent9 = string.Format("{0},Alive-,{1}\r\n", DateTime.Now, alive);
                            //File.AppendAllText(filename2path, filecontent9);
                            //Console.WriteLine(filecontent9);
                        }
                        ++alive;


                    }//end of if block line 1173


                }//end of try
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                           strike1, strike2);
                    File.AppendAllText(filename2path, filecontent1);
                }
            }

        }

        public static void BNLongStrangleStrategyMultiStrikeLive()
        {
            //CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            //decimal currentpivot1 = csvinputobj.Pivot;
            //decimal startpivot = csvinputobj.Pivot;
            string strikeprice = "NIFTY BANK";
            //string strikeprice2 = csvinputobj.Strike2;

            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;           
            //delete existing files

            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("BN-LStrang{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "TradeData");
            string filename1path = string.Format(@"C:\Algotrade\scalperoutput\Live{0}.csv", filename1);
            string filecontent = "DateTime,OrderID,Bid/Ask,InstrumentId,Price,OrderType,LineNo,PivotDown,PivotUp,Maxlots\r\n";

            File.AppendAllText(filename1path, filecontent);

            string filename2 = string.Format("BN-LStrang-{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");

            string filename3 = string.Format("BN-LStrangle-{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "Continous");

            string filename3path = string.Format(@"C:\Algotrade\scalperoutput\Live-{0}.csv", filename3);
            string filecontent3 = "Datetime,LivePrice,Strike,BuyDeltaPrice,SellDeltaPrice,Quantity1,Quantity2,PercentageInc" + threadname + "\r\n";
            File.AppendAllText(filename3path, filecontent3);

            string filename2path = string.Format(@"C:\Algotrade\scalperoutput\credit{0}.csv", filename2);
            string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename2path, filecontent2);


            
            

            //Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            //{
            //    {"strikeprice1", strikeprice1},
            //    //{"strikeprice2",strikeprice2 }
            //};


            Thread.Sleep(100);
            //Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = 25;
            string instrumentId1 = "";
            string instrumentId2 = "";
            string instrumentId3 = "";
            string instrumentId4 = "";
            int alive = 0;
            decimal totalbuyprice = 0.0m;
            decimal totalbidprice = 0.0m;
            TimeSpan end = new TimeSpan(15, 31, 0);
            
            string strike1 = "";
            string strike2 = "";
            string strike3 = "";
            string strike4 = "";
            bool bfirst = true;
            totalbuyprice = 0.0m;
            int strikedelta = 0;

            int totallots = 10;
            int nooflots = 0;
            int maxlots = 20;

            string strikeprice1 = "";
            string strikeprice2 = "";
            long currstrikeprice = 0;

            while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
            {
                try
                {
                    //Thread.Sleep(1000);

                    //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    //instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);
                    
                    Dictionary<string, dynamic> quotes = GetQuoteBank(strikeprice);
                    KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                    dynamic currentliveprice1 = kvpquote.Value.LastPrice;
                    decimal prevdayclose = kvpquote.Value.PrevDayClose;//Previous day close price

                    long currentliveprice = long.Parse(currentliveprice1.ToString());


                    decimal currpricepercentincval = prevdayclose * 1.015m;
                    decimal currpricepercentdecval = prevdayclose * (1.0m - 0.015m);


                    //if (currentliveprice>= currpricepercentincval || currentliveprice<= currpricepercentdecval)
                    {
                        long currentlivepricetens = currentliveprice % 100;
                        long currentlivepricewhole = (currentliveprice / 100) * 100;

                        long currentlivepricewholenext = currentlivepricewhole + 100;


                        
                        //check for which nearest strike to be taken
                        decimal percentfromlowerstrike = ((currentliveprice - currentlivepricewhole) * 1.0m / currentlivepricewhole) * 100.0m;
                        decimal percentfromupperstrike = ((currentlivepricewholenext - currentliveprice) * 1.0m / currentlivepricewholenext) * 100.0m;

                        if (nooflots == 0)
                        {

                            if (percentfromlowerstrike < percentfromupperstrike)
                            {
                                currstrikeprice = currentlivepricewhole;
                            }
                            else
                            {
                                currstrikeprice = currentlivepricewholenext;
                            }
                            //give strikeprice input
                            currstrikeprice = 30250;

                            strikedelta = 250;
                            if (currentlivepricetens >= 40 && currentlivepricetens < 60)
                            {
                                currstrikeprice = currentlivepricewhole + 50;
                                strikedelta = strikedelta - 50;
                            }
                            else
                            {
                                //strikedelta = 200;

                            }

                            //if (_broker == "iifl")
                            {
                                //strike1 = "BANKNIFTY29Oct2020" + (currstrikeprice - strikedelta) + "PE";
                                //strike2 = "BANKNIFTY29Oct2020" + (currstrikeprice + strikedelta) + "CE";
                            }
                            Dictionary<string,string> dictParamStrikes = GetStrikePrices((currstrikeprice - strikedelta).ToString(),
                                             (currstrikeprice + strikedelta).ToString(),"iifl", "BANKNIFTY");

                            strikeprice1 = dictParamStrikes["strike1"];
                            strikeprice2 = dictParamStrikes["strike2"];
                            
                            //strike3 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta - 100)) + "CE";
                            //strike4 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta + 100)) + "CE";

                            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                            {
                                {"strikeprice1", strikeprice1},
                                {"strikeprice2",strikeprice2 },
                                //{"strikeprice3",strike3 },
                                //{"strikeprice4",strike4 }
                            };

                            Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid);
                            lotsize = Convert.ToInt32(instrumentids1["lotsize"]);

                            //lotsize = 25;
                            //if (_broker == "iifl")
                            {
                                instrumentId1 = Convert.ToString(instrumentids1["strikeprice1"]);
                                instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                                //instrumentId1 = Convert.ToString(instrumentids1["strikeprice3"]);
                                // instrumentId2 = Convert.ToString(instrumentids1["strikeprice4"]);
                            }
                            //else
                            {
                                //instrumentId1 = strike1;
                                //instrumentId2 = strike2;

                            }

                        }//end of if

                        Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                        Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                        //add nfo 
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);
                        
                        dynamic lstbids1 = kvpquote1.Value.Bids;
                        dynamic lstoffers1 = kvpquote1.Value.Offers;
                        dynamic lstbids2 = kvpquote2.Value.Bids;
                        dynamic lstoffers2 = kvpquote2.Value.Offers;

                      


                        int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                        int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                        totalbidprice = (lstoffers1[0].Price + lstoffers2[0].Price);

                        if (_broker != "iifl")
                        {
                            Dictionary<string, string> dictParamStrikes1 = GetStrikePrices((currstrikeprice - strikedelta).ToString(),
                                           (currstrikeprice + strikedelta).ToString(), "zerodhanew");
                            strike1 = dictParamStrikes1["strike1"];
                            strike2 = dictParamStrikes1["strike2"];

                        }
                        else
                        {
                            strike1 = instrumentId1;
                            strike2 = instrumentId2;
                        }

                        decimal percentchange = currentliveprice / prevdayclose;

                        if (nooflots < maxlots && totalbidprice <= 450.0m
                              /*&&(percentchange >=1.015m || percentchange <= (1.0m-0.015m))*/)
                        {

                            //take totallots(default 10) lots in one go


                            //buy 1 lot
                            //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                            {
                                //buy 2nd strike at bid price and buy 2nd strike one at ask price
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int i = 0; i < 5; ++i)
                                {

                                    try
                                    {
                                        response1 = PlaceOrderOption(strikeprice: strike1, transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                                              price: lstoffers1[0].Price, lotsize: (lotsize * totallots),
                                                              product: Constants.PRODUCT_NRML);
                                        break;

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strike1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }
                                Dictionary<string, dynamic> orderstatus = new Dictionary<string, dynamic>();
                                //orderstatus = GetOrderStatus(response1);//not required

                                //Thread.Sleep(100);

                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                       "", strike1, lstoffers1[0].Price, "BUY",
                                                                                       totaltradedquantity1, "LineNo2082");
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() != "cancelled"
                                //        && orderstatus["Status"].ToLower() != "rejected")
                                {
                                    //sell 1st strike
                                    //Thread.Sleep(2000);
                                    Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                    for (int j = 0; j < 5; ++j)
                                    {
                                        try
                                        {
                                            response2 = PlaceOrderOption(strikeprice: strike2, transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                                                       price: lstoffers2[0].Price, lotsize: (lotsize * totallots),
                                                                       product: Constants.PRODUCT_NRML);

                                            break;

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                            strike1, strike2);
                                            File.AppendAllText(filename2path, content);
                                        }
                                        //Thread.Sleep(500);
                                    }

                                    string writetofile6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                "", strike2, lstoffers2[0].Price, "BUY",
                                                                                    totaltradedquantity2, "LineNo2113");
                                    Console.WriteLine(writetofile6);
                                    File.AppendAllText(filename1path, writetofile6);

                                    Dictionary<string, dynamic> orderstatus2 = new Dictionary<string, dynamic>();


                                    //if (orderstatus2["Status"].ToLower() != "cancelled"
                                    //  && orderstatus2["Status"].ToLower() != "rejected")
                                    {
                                        nooflots += totallots;
                                        totalbuyprice = totalbidprice;

                                        string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, strikeprice1, strikeprice2,
                                                                        totalbuyprice, nooflots, "BUY", currentliveprice, "LineNo1206");
                                        Console.WriteLine(writetofile);
                                        File.AppendAllText(filename1path, writetofile);
                                        //
                                        int lotdiff = maxlots - nooflots;
                                        if (lotdiff < totallots)
                                        {
                                            int rem = lotdiff % totallots;
                                            if (rem != 0)
                                            {
                                                totallots = lotdiff;
                                            }
                                        }

                                        if (nooflots >= maxlots)
                                        {
                                            break;
                                        }
                                        //break;
                                    }

                                }//end of if

                            }
                        }
                        

                        if(totalbuyprice==0m)
                        {
                            totalbuyprice = totalbidprice;
                        }

                        decimal totalperceinc = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);
                        
                        string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}%\r\n", DateTime.Now, currentliveprice,
                                          strikeprice1,strikeprice2, totalbidprice, totalbuyprice, totaltradedquantity1,
                                                  totaltradedquantity2, totalperceinc);

                        Console.WriteLine(filecontent1);

                        File.AppendAllText(filename3path, filecontent1);

                        if (alive % 250 == 0)
                        {
                            Console.WriteLine(DateTime.Now + "-I am alive -" + alive + "-" + "," + strikeprice1);
                            string filecontent9 = string.Format("{0},Alive-,{1}\r\n", DateTime.Now, alive);
                            //File.AppendAllText(filename2path, filecontent9);
                            //Console.WriteLine(filecontent9);
                        }
                        ++alive;


                    }//end of if block line 1173
                    //Thread.Sleep(60000);//wait for 1 min

                }//end of try
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                           strike1, strike2);
                    File.AppendAllText(filename2path, filecontent1);
                }
            }

        }

        public static void Teststocksdeveloper()
        {
            Dictionary<string, dynamic> response0 = PlaceOrderOption("BANKNIFTY_29-Oct-2020_CE_25000", Constants.TRANSACTION_TYPE_BUY,
                         110, 25);


        }
        public static void BNShortStrangleStrategyLive(object strikeobj)
        {
            //CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            //decimal currentpivot1 = csvinputobj.Pivot;
            //decimal startpivot = csvinputobj.Pivot;
            string strikeprice = "NIFTY BANK";
            //string strikeprice2 = csvinputobj.Strike2;
            int strikedelta = int.Parse(strikeobj.ToString());
            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;           
            //delete existing files
            try
            {
                string threadname = Thread.CurrentThread.Name;
                string filename1 = string.Format("BN-SStrang{0}-{1}-{2}_{3}", DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", strikedelta, threadname);
                string filename1path = string.Format(@"C:\Algotrade\scalperoutput\LiveTrade-{0}.csv", filename1);
                string filecontent = "DateTime,OrderID,Bid-Ask,InstrumentId,Price,OrderType,LineNo,PivotDown,PivotUp,Maxlots\r\n";

                File.AppendAllText(filename1path, filecontent);

                string filename2 = string.Format("BN-SStrang-{0}-{1}_{2}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog", threadname);

                string filename3 = string.Format("BN-SStrangle-{0}-{1}-{2}_{3}", DateTime.Now.ToString("dd-MM-yyyy"), "Continous",strikedelta, threadname);

                string filename3path = string.Format(@"C:\Algotrade\scalperoutput\LiveTest-{0}.csv", filename3);
                string filecontent3 = "Datetime,LivePrice,Strike,BuyDeltaPrice,SellDeltaPrice,Quantity1,Quantity2,PercentageInc" + threadname + "\r\n";
                File.AppendAllText(filename3path, filecontent3);

                string filename2path = string.Format(@"C:\Algotrade\scalperoutput\{0}.csv", filename2);
                string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
                File.AppendAllText(filename2path, filecontent2);





                //Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                //{
                //    {"strikeprice1", strikeprice1},
                //    //{"strikeprice2",strikeprice2 }
                //};


                //Thread.Sleep(100);
                //Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
                int lotsize = 25;
                string instrumentId1 = "";
                string instrumentId2 = "";
                string instrumentId3 = "";
                string instrumentId4 = "";
                int alive = 0;
                decimal totalbuyprice;
                decimal totalbidprice = 1.0m;
                decimal totalofferprice = 0.0m;
                
                string strike1 = "";
                string strike2 = "";
                string strike3 = "";
                string strike4 = "";
                bool bfirst = true;
              
                //int strikedelta = 0;

                int totallots = 10;
                int nooflots = 0;
                int maxlots = 10;

                string strikeprice1 = "";
                string strikeprice2 = "";

                decimal squareoffprice = 0.0m;

                int remaininglots = 0;
                int strikedelta1 = strikedelta;
                TimeSpan end = new TimeSpan(15, 31, 0);
                bool takeposition = false;
              
                decimal maxprice = 0.0m;
                long stoplosslow = 0;
                long stoplosshigh = 0;
                decimal squareoffpercent = 8.0m;
                //decimal stoplosspercent = 15.0m;
                //minimum and maximum Rs price target 
                decimal stoplossprice = 0.0m;
                decimal mintargetprice = 20.0m;
                decimal maxtargetprice = 35.0m;
                decimal stoplossdeltaprice = 35.0m;

                totalbuyprice = 150.0m;

                while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
                {
                    try
                    {
                        //Thread.Sleep(5000);
                        //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                        //instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                        Dictionary<string, dynamic> quotes = GetQuoteBank(strikeprice);
                        KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                        dynamic currentliveprice1 = kvpquote.Value.LastPrice;
                        decimal prevdayclose = kvpquote.Value.PrevDayClose;//Previous day close price

                        long currentliveprice = long.Parse(Math.Round(currentliveprice1).ToString());

                        decimal currpricepercentincval = prevdayclose * 1.015m;
                        decimal currpricepercentdecval = prevdayclose * (1.0m - 0.015m);                  



                        long currentlivepricetens = currentliveprice % 100;
                        long currentlivepricewhole = (currentliveprice / 100) * 100;

                        long currentlivepricewholenext = currentlivepricewhole + 100;
                        //when it nears a whole number +-10 take position


                        long currstrikeprice = 0;
                        //check for which nearest strike to be taken
                        decimal percentfromlowerstrike = ((currentliveprice - currentlivepricewhole) * 1.0m / currentlivepricewhole) * 100.0m;
                        decimal percentfromupperstrike = ((currentlivepricewholenext - currentliveprice) * 1.0m / currentlivepricewholenext) * 100.0m;

                        if (nooflots == 0)
                        {
                            takeposition = false;
                            strikedelta = strikedelta1;

                            if (percentfromlowerstrike < percentfromupperstrike)
                            {
                                currstrikeprice = currentlivepricewhole;
                            }
                            else
                            {
                                currstrikeprice = currentlivepricewholenext;
                            }
                            //give strikeprice input
                            //currstrikeprice = 24500;
                            //strikedelta = 200;
                            //if (currentlivepricetens >= 40 && currentlivepricetens < 60)
                            //{
                            //    currstrikeprice = currentlivepricewhole + 50;
                            //    strikedelta = strikedelta - 50;
                            //    takeposition = true;
                            //    //Console.WriteLine("Short position taken" + currentliveprice + "," + currstrikeprice);
                            //}
                            if (Math.Abs(currstrikeprice - currentliveprice) <= 25)
                            {
                                takeposition = true;
                                //Console.WriteLine("Short position taken" + currentliveprice +","+currstrikeprice);
                                //stoplosslow = currstrikeprice - (strikedelta / 2);
                                //stoplosshigh = currstrikeprice + (strikedelta / 2);
                                stoplosslow = currstrikeprice - 300;
                                stoplosshigh = currstrikeprice + 300;

                            }


                            //if (_broker == "iifl")
                            {
                                //strike1 = "BANKNIFTY29Oct2020" + (currstrikeprice - strikedelta) + "PE";
                                //strike2 = "BANKNIFTY29Oct2020" + (currstrikeprice + strikedelta) + "CE";
                            }
                            Dictionary<string, string> dictParamStrikes = GetStrikePrices((currstrikeprice - strikedelta).ToString(),
                                             (currstrikeprice + strikedelta).ToString(), OptionsData._broker);

                            strikeprice1 = dictParamStrikes["strike1"];
                            strikeprice2 = dictParamStrikes["strike2"];

                            //strike3 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta - 100)) + "CE";
                            //strike4 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta + 100)) + "CE";

                            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                            {
                                {"strikeprice1", strikeprice1},
                                {"strikeprice2",strikeprice2 },
                                //{"strikeprice3",strike3 },
                                //{"strikeprice4",strike4 }
                            };

                            Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid);
                            lotsize = Convert.ToInt32(instrumentids1["lotsize"]);

                            //lotsize = 25;
                            //if (_broker == "iifl")
                            {
                                instrumentId1 = Convert.ToString(instrumentids1["strikeprice1"]);
                                instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                                //instrumentId1 = Convert.ToString(instrumentids1["strikeprice3"]);
                                // instrumentId2 = Convert.ToString(instrumentids1["strikeprice4"]);
                            }
                            //else
                            {
                                //instrumentId1 = strike1;
                                //instrumentId2 = strike2;

                            }

                            if (_broker != "iifl")
                            {
                                Dictionary<string, string> dictParamStrikes1 = GetStrikePrices((currstrikeprice - strikedelta).ToString(),
                                               (currstrikeprice + strikedelta).ToString(), "zerodhanew");
                                strike1 = dictParamStrikes1["strike1"];
                                strike2 = dictParamStrikes1["strike2"];

                            }
                            else
                            {
                                strike1 = instrumentId1;
                                strike2 = instrumentId2;
                            }


                        }//end of if

                        Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                        Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                        //add nfo 
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                        dynamic lstbids1 = kvpquote1.Value.Bids;
                        dynamic lstoffers1 = kvpquote1.Value.Offers;
                        dynamic lstbids2 = kvpquote2.Value.Bids;
                        dynamic lstoffers2 = kvpquote2.Value.Offers;



                        int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                        int totaltradedquantity2 = kvpquote2.Value.LastQuantity;                       

                        decimal percentchange = currentliveprice / prevdayclose;

                       

                        totalbidprice = (lstbids1[0].Price + lstbids2[0].Price);
                        totalofferprice = (lstoffers1[0].Price + lstoffers2[0].Price);
                        //do not take position if difference between bid and offer is more than 1%

                        if((lstbids1[0].Price*1.01m) < lstoffers1[0].Price || (lstbids2[0].Price * 1.01m) < lstoffers2[0].Price)
                        {
                            takeposition = false;
                            Console.WriteLine("Bid/Ask more than 1% difference,banknifty"+strikeprice1+","+strikeprice2);
                        }

                        if (takeposition == true && nooflots==0)
                        {
                            Console.WriteLine("{0},Expected Price:{1},CurrentPrice:{2}", currentliveprice, totalbuyprice, totalbidprice);
                        }

                        if (totalofferprice <= squareoffprice)
                        {
                            //trailflag = true;
                            //trailsquareoffprice = squareoffprice - 2.0m;
                            //currpercent = nextcurrpercent;
                            //nextcurrpercent += percentraildiff;
                        }


                        //Console.WriteLine(currentliveprice);

                        TimeSpan prdiscstarttime = new TimeSpan(9, 30, 0);
                        TimeSpan prdiscendtime = new TimeSpan(9, 44, 0);

                        TimeSpan starttime = new TimeSpan(9, 45, 0);
                        TimeSpan endtime = new TimeSpan(13, 30, 0);
                        TimeSpan squareofftime = new TimeSpan(14, 45, 0);
                        TimeSpan currtime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        //take position after 09:45


                        //discover max price from 9:30 to 9:45 on which the short strangle will be bought
                        //if (nooflots == 0 &&
                        //     currtime >= prdiscstarttime && currtime <= prdiscendtime)
                        //{
                        //    if (totalofferprice >= maxprice)
                        //    {
                        //        maxprice = totalofferprice;
                        //        totalbuyprice = maxprice;
                        //        Console.WriteLine("max price:" + maxprice);
                        //    }
                        //    Thread.Sleep(5000);
                        //}

                        if (nooflots < maxlots && takeposition == true
                            && currtime >= starttime && currtime <= endtime 
                            && totalbidprice >= totalbuyprice)
                        {
                            Console.WriteLine("Short position taken" + currentliveprice + "," + currstrikeprice);
                            //buy 2nd strike at bid price and buy 2nd strike one at ask price
                            Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                            for (int i = 0; i < 5; ++i)
                            {

                                try
                                {
                                    //response1 = PlaceOrderOption(strikeprice: strike1, transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                    //                         price: lstbids1[0].Price, lotsize: (lotsize * totallots),
                                    //                         product: Constants.PRODUCT_MIS);
                                    break;

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                    strike1, strike2);
                                    File.AppendAllText(filename2path, content);
                                }
                                //Thread.Sleep(500);
                            }
                            Dictionary<string, dynamic> orderstatus = new Dictionary<string, dynamic>();
                            //orderstatus = GetOrderStatus(response1);//not required



                            string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                   strikeprice1, strikeprice2, lstoffers1[0].Price, "BUY",
                                                                                   totaltradedquantity1, "LineNo2082");
                            //Console.WriteLine(writetofile1);
                            //File.AppendAllText(filename1path, writetofile1);

                            //if (orderstatus["Status"].ToLower() != "cancelled"
                            //        && orderstatus["Status"].ToLower() != "rejected")
                            {
                                //sell 1st strike
                                //Thread.Sleep(2000);
                                Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                for (int j = 0; j < 5; ++j)
                                {
                                    try
                                    {
                                        //response2 = PlaceOrderOption(strikeprice: strike2, transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                        //                           price: lstbids2[0].Price, lotsize: (lotsize * totallots),
                                        //                           product: Constants.PRODUCT_MIS);

                                        break;

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strike1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }

                                //string writetofile6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                //                                            "", strike2, lstoffers2[0].Price, "SELL", totaltradedquantity2, "LineNo2113");
                                //Console.WriteLine(writetofile6);
                                //File.AppendAllText(filename1path, writetofile6);

                                Dictionary<string, dynamic> orderstatus2 = new Dictionary<string, dynamic>();


                                //if (orderstatus2["Status"].ToLower() != "cancelled"
                                //  && orderstatus2["Status"].ToLower() != "rejected")
                                {
                                    nooflots += totallots;
                                    remaininglots = nooflots;
                                    totalbuyprice = totalbidprice;

                                    string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", DateTime.Now, strikeprice1, strikeprice2,
                                                                   lstbids1[0].Price, lstbids2[0].Price, totalbidprice, nooflots, "SELL", currentliveprice, "LineNo1206");
                                    Console.WriteLine(writetofile);
                                    File.AppendAllText(filename1path, writetofile);
                                    //
                                    int lotdiff = maxlots - nooflots;
                                    if (lotdiff < totallots)
                                    {
                                        int rem = lotdiff % totallots;
                                        if (rem != 0)
                                        {
                                            totallots = lotdiff;
                                        }
                                    }
                                    //break;
                                }

                            }//end of if

                            squareoffprice = totalbidprice * (1.0m - squareoffpercent / 100);
                            stoplossprice = totalbidprice + stoplossdeltaprice;


                            if (squareoffpercent==4.0m)
                            {
                                mintargetprice = mintargetprice / 2;
                                maxtargetprice = maxtargetprice / 2;
                                stoplossdeltaprice = stoplossdeltaprice / 2;
                            }
                            if (totalbidprice - squareoffprice < mintargetprice)
                            {
                                squareoffprice = totalbidprice - mintargetprice;
                            }
                            else if (totalbidprice - squareoffprice > maxtargetprice)
                            {
                                squareoffprice = totalbidprice - maxtargetprice;
                            }
                            stoplossprice = totalbidprice + stoplossdeltaprice;                           

                        }
                        else if (remaininglots > 0 && (totalofferprice <= squareoffprice || currtime>=squareofftime ))//square off
                        {
                            //square off everything 
                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            //Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                            //                            transactiontype: Constants.TRANSACTION_TYPE_BUY,
                            //                             price: lstoffers1[0].Price,
                            //                             lotsize: (lotsize * totallots),
                            //                            validity: Constants.VALIDITY_DAY,
                            //                            product: Constants.PRODUCT_MIS);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                //                                   transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                //                                   price: lstoffers2[0].Price,
                                //                                   lotsize: (lotsize * totallots),
                                //                                   validity: Constants.VALIDITY_DAY,
                                //                                   product: Constants.PRODUCT_MIS);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbuyprice - totalofferprice) * 1.0m / totalbuyprice) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}%,{9},{10}\n", DateTime.Now, strikeprice1, strikeprice2, "AppOrderID",
                                                                    lstoffers1[0].Price, lstoffers2[0].Price, totalofferprice, totallots,
                                                                    percentageincrease, currentliveprice, squareoffprice);
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }

                            }                          
                            squareoffpercent = 4.0m;
                            
                            //stoplossprice = totalbuyprice * (1.0m + stoplosspercent / 100);

                            //decimal totalperceinc1 = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);


                            //string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%\r\n", DateTime.Now, currentliveprice,
                            //                  strike2, totalbidprice, totalbuyprice, totaltradedquantity1,
                            //                          totaltradedquantity2, totalperceinc1);

                            //Console.WriteLine(filecontent1);
                            //File.AppendAllText(filename1path, filecontent1);                            
                            nooflots = remaininglots;                           

                        }//end of if block line 1173
                        else if (remaininglots > 0 && 
                            (currentliveprice <= stoplosslow || currentliveprice >= stoplosshigh || totalofferprice>=stoplossprice)
                            /*totalofferprice >= stoplossprice*/)
                        {
                            //stop loss module
                            //exit if the index has moved by stoploss low /high of the price is more than 40rs from the sell price


                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            //Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                            //                            transactiontype: Constants.TRANSACTION_TYPE_BUY,
                            //                             price: lstoffers1[0].Price,
                            //                             lotsize: (lotsize * totallots),
                            //                            validity: Constants.VALIDITY_DAY,
                            //                            product: Constants.PRODUCT_MIS);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                //                                   transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                //                                   price: lstoffers2[0].Price,
                                //                                   lotsize: (lotsize * totallots),
                                //                                   validity: Constants.VALIDITY_DAY,
                                //                                   product: Constants.PRODUCT_MIS);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbuyprice - totalofferprice) * 1.0m / totalbuyprice) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}%,{9},{10}\n", DateTime.Now, strikeprice1, strikeprice2, "AppOrderID",
                                                                    lstoffers1[0].Price, lstoffers2[0].Price, totalofferprice, totallots,
                                                                    percentageincrease,currentliveprice,stoplossprice);
                                //take the next position above this price and 2% higher
                                totalbuyprice = totalofferprice *1.02m;
                                if (totalofferprice + 2.0m > totalbuyprice)
                                {
                                    totalbuyprice = totalofferprice + 2.0m; //add 2 rs
                                }
                                squareoffpercent = 4.0m;
                                //squareoffprice = totalbuyprice * (1.0m - squareoffpercent / 100);
                                //stoplossprice = totalbuyprice * (1.0m + stoplosspercent / 100);

                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }
                            }
                            nooflots = remaininglots;
                        }

                        if (nooflots > 0)
                        {

                            decimal totalperceinc = Math.Round(((totalbuyprice - totalofferprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);
                            string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%,{8},{9},{10},{11}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                        totalbuyprice, lstoffers1[2].Price, lstoffers2[2].Price,totalofferprice, totalperceinc, currentliveprice,remaininglots, 
                                                        squareoffprice, OptionsData._broker);
                            File.AppendAllText(filename3path, filecontent5);
                            Console.WriteLine(filecontent5);
                        }
                        else
                        {
                            Console.WriteLine(currentliveprice);
                            string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                        totalbuyprice, lstoffers1[2].Price, lstoffers2[2].Price,totalofferprice, remaininglots, currentliveprice, OptionsData._broker);
                            //Console.WriteLine(filecontent5);
                            File.AppendAllText(filename3path, filecontent5);

                        }
                        if (nooflots == 0)
                        {
                            Thread.Sleep(3000);
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }


                    }//end of try
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                               strike1, strike2);
                        File.AppendAllText(filename2path, filecontent1);
                    }
                    finally
                    {
                        Thread.Sleep(3000);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public static void BNLongStrangleStrategyLive(object strikeobj)
        {
            //CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            //decimal currentpivot1 = csvinputobj.Pivot;
            //decimal startpivot = csvinputobj.Pivot;
            string strikeprice = "NIFTY BANK";
            //string strikeprice2 = csvinputobj.Strike2;
            int strikedelta = int.Parse(strikeobj.ToString());
            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;           
            //delete existing files
            try
            {
                string threadname = Thread.CurrentThread.Name;
                string filename1 = string.Format("BN-LStrang{0}-{1}-{2}_{3}", DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", strikedelta, threadname);
                string filename1path = string.Format(@"C:\Algotrade\scalperoutput\LiveTrade-{0}.csv", filename1);
                string filecontent = "DateTime,OrderID,Bid-Ask,InstrumentId,Price,OrderType,LineNo,PivotDown,PivotUp,Maxlots\r\n";

                File.AppendAllText(filename1path, filecontent);

                string filename2 = string.Format("BN-LStrang-{0}-{1}_{2}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog", threadname);

                string filename3 = string.Format("BN-LStrangle-{0}-{1}-{2}_{3}", DateTime.Now.ToString("dd-MM-yyyy"), "Continous", strikedelta, threadname);

                string filename3path = string.Format(@"C:\Algotrade\scalperoutput\LiveTest-{0}.csv", filename3);
                string filecontent3 = "Datetime,LivePrice,Strike,BuyDeltaPrice,SellDeltaPrice,Quantity1,Quantity2,PercentageInc" + threadname + "\r\n";
                File.AppendAllText(filename3path, filecontent3);

                string filename2path = string.Format(@"C:\Algotrade\scalperoutput\{0}.csv", filename2);
                string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
                File.AppendAllText(filename2path, filecontent2);





                //Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                //{
                //    {"strikeprice1", strikeprice1},
                //    //{"strikeprice2",strikeprice2 }
                //};


                //Thread.Sleep(100);
                //Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
                int lotsize = 25;
                string instrumentId1 = "";
                string instrumentId2 = "";
                string instrumentId3 = "";
                string instrumentId4 = "";
                int alive = 0;
                decimal totalbuyprice;
                decimal totalbidprice = 1.0m;
                decimal totalofferprice = 0.0m;

                string strike1 = "";
                string strike2 = "";
                string strike3 = "";
                string strike4 = "";
                bool bfirst = true;

                //int strikedelta = 0;

                int totallots = 10;
                int nooflots = 0;
                int maxlots = 10;

                string strikeprice1 = "";
                string strikeprice2 = "";

                decimal squareoffprice = 0.0m;

                int remaininglots = 0;
                int strikedelta1 = strikedelta;
                TimeSpan end = new TimeSpan(15, 31, 0);
                bool takeposition = false;

                decimal maxprice = 0.0m;
                long stoplosslow = 0;
                long stoplosshigh = 0;
                decimal squareoffpercent = 8.0m;
                //decimal stoplosspercent = 15.0m;
                //minimum and maximum Rs price target 
                decimal stoplossprice = 0.0m;
                decimal mintargetprice = 20.0m;
                decimal maxtargetprice = 35.0m;
                decimal mintargetprice1 = 20.0m;
                decimal maxtargetprice1 = 35.0m;
                decimal stoplossdeltaprice = 35.0m;
                decimal stoplossdeltaprice1 = 35.0m;
                decimal minbuyrice = 150.0m;
                totalbuyprice = 2000.0m;

                while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
                {
                    try
                    {
                        //Thread.Sleep(5000);
                        //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                        //instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                        Dictionary<string, dynamic> quotes = GetQuoteBank(strikeprice);
                        KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                        dynamic currentliveprice1 = kvpquote.Value.LastPrice;
                        decimal prevdayclose = kvpquote.Value.PrevDayClose;//Previous day close price

                        long currentliveprice = long.Parse(Math.Round(currentliveprice1).ToString());

                        decimal currpricepercentincval = prevdayclose * 1.015m;
                        decimal currpricepercentdecval = prevdayclose * (1.0m - 0.015m);



                        long currentlivepricetens = currentliveprice % 100;
                        long currentlivepricewhole = (currentliveprice / 100) * 100;

                        long currentlivepricewholenext = currentlivepricewhole + 100;
                        //when it nears a whole number +-10 take position


                        long currstrikeprice = 0;
                        //check for which nearest strike to be taken
                        decimal percentfromlowerstrike = ((currentliveprice - currentlivepricewhole) * 1.0m / currentlivepricewhole) * 100.0m;
                        decimal percentfromupperstrike = ((currentlivepricewholenext - currentliveprice) * 1.0m / currentlivepricewholenext) * 100.0m;

                        if (nooflots == 0)
                        {
                            takeposition = false;
                            strikedelta = strikedelta1;

                            if (percentfromlowerstrike < percentfromupperstrike)
                            {
                                currstrikeprice = currentlivepricewhole;
                            }
                            else
                            {
                                currstrikeprice = currentlivepricewholenext;
                            }
                            //give strikeprice input
                            //currstrikeprice = 24500;
                            //strikedelta = 200;
                            //if (currentlivepricetens >= 40 && currentlivepricetens < 60)
                            //{
                            //    currstrikeprice = currentlivepricewhole + 50;
                            //    strikedelta = strikedelta - 50;
                            //    takeposition = true;
                            //    //Console.WriteLine("Short position taken" + currentliveprice + "," + currstrikeprice);
                            //}
                            if (Math.Abs(currstrikeprice - currentliveprice) <= 25)
                            {
                                takeposition = true;
                                //Console.WriteLine("Short position taken" + currentliveprice +","+currstrikeprice);
                                //stoplosslow = currstrikeprice - (strikedelta / 2);
                                //stoplosshigh = currstrikeprice + (strikedelta / 2);
                                stoplosslow = currstrikeprice - 300;
                                stoplosshigh = currstrikeprice + 300;

                            }


                            //if (_broker == "iifl")
                            {
                                //strike1 = "BANKNIFTY29Oct2020" + (currstrikeprice - strikedelta) + "PE";
                                //strike2 = "BANKNIFTY29Oct2020" + (currstrikeprice + strikedelta) + "CE";
                            }
                            Dictionary<string, string> dictParamStrikes = GetStrikePrices((currstrikeprice - strikedelta).ToString(),
                                             (currstrikeprice + strikedelta).ToString(), OptionsData._broker);

                            strikeprice1 = dictParamStrikes["strike1"];
                            strikeprice2 = dictParamStrikes["strike2"];

                            //strike3 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta - 100)) + "CE";
                            //strike4 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta + 100)) + "CE";

                            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                            {
                                {"strikeprice1", strikeprice1},
                                {"strikeprice2",strikeprice2 },
                                //{"strikeprice3",strike3 },
                                //{"strikeprice4",strike4 }
                            };

                            Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid);
                            lotsize = Convert.ToInt32(instrumentids1["lotsize"]);

                            //lotsize = 25;
                            //if (_broker == "iifl")
                            {
                                instrumentId1 = Convert.ToString(instrumentids1["strikeprice1"]);
                                instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                                //instrumentId1 = Convert.ToString(instrumentids1["strikeprice3"]);
                                // instrumentId2 = Convert.ToString(instrumentids1["strikeprice4"]);
                            }
                            //else
                            {
                                //instrumentId1 = strike1;
                                //instrumentId2 = strike2;

                            }

                            if (_broker != "iifl")
                            {
                                Dictionary<string, string> dictParamStrikes1 = GetStrikePrices((currstrikeprice - strikedelta).ToString(),
                                               (currstrikeprice + strikedelta).ToString(), "zerodhanew");
                                strike1 = dictParamStrikes1["strike1"];
                                strike2 = dictParamStrikes1["strike2"];

                            }
                            else
                            {
                                strike1 = instrumentId1;
                                strike2 = instrumentId2;
                            }


                        }//end of if

                        Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                        Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                        //add nfo 
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                        dynamic lstbids1 = kvpquote1.Value.Bids;
                        dynamic lstoffers1 = kvpquote1.Value.Offers;
                        dynamic lstbids2 = kvpquote2.Value.Bids;
                        dynamic lstoffers2 = kvpquote2.Value.Offers;



                        int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                        int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                        decimal percentchange = currentliveprice / prevdayclose;



                        totalbidprice = (lstbids1[0].Price + lstbids2[0].Price);
                        totalofferprice = (lstoffers1[0].Price + lstoffers2[0].Price);
                        //do not take position if difference between bid and offer is more than 1%

                        if ((lstbids1[0].Price * 1.01m) < lstoffers1[0].Price || (lstbids2[0].Price * 1.01m) < lstoffers2[0].Price)
                        {
                            takeposition = false;
                            Console.WriteLine("Bid/Ask more than 1% difference,banknifty" + strikeprice1 + "," + strikeprice2);
                        }

                        if (takeposition == true && nooflots == 0)
                        {
                            Console.WriteLine("{0},Expected Price:{1},CurrentPrice:{2}", currentliveprice, totalbuyprice, totalbidprice);
                        }

                        if (totalofferprice <= squareoffprice)
                        {
                            //trailflag = true;
                            //trailsquareoffprice = squareoffprice - 2.0m;
                            //currpercent = nextcurrpercent;
                            //nextcurrpercent += percentraildiff;
                        }


                        //Console.WriteLine(currentliveprice);

                        TimeSpan prdiscstarttime = new TimeSpan(9, 30, 0);
                        TimeSpan prdiscendtime = new TimeSpan(9, 44, 0);

                        TimeSpan starttime = new TimeSpan(9, 45, 0);
                        TimeSpan endtime = new TimeSpan(13, 30, 0);
                        TimeSpan squareofftime = new TimeSpan(14, 45, 0);
                        TimeSpan currtime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        //take position after 09:45


                        //discover max price from 9:30 to 9:45 on which the short strangle will be bought
                        //if (nooflots == 0 &&
                        //     currtime >= prdiscstarttime && currtime <= prdiscendtime)
                        //{
                        //    if (totalofferprice >= maxprice)
                        //    {
                        //        maxprice = totalofferprice;
                        //        totalbuyprice = maxprice;
                        //        Console.WriteLine("max price:" + maxprice);
                        //    }
                        //    Thread.Sleep(5000);
                        //}

                        if (nooflots < maxlots && takeposition == true
                            && currtime >= starttime && currtime <= endtime
                            && (totalofferprice >= minbuyrice && totalofferprice<=totalbuyprice))
                        {
                            Console.WriteLine("Long position taken" + currentliveprice + "," + currstrikeprice);
                            //buy 2nd strike at bid price and buy 2nd strike one at ask price
                            Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                            for (int i = 0; i < 5; ++i)
                            {

                                try
                                {
                                    //response1 = PlaceOrderOption(strikeprice: strike1, transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                    //                         price: lstbids1[0].Price, lotsize: (lotsize * totallots),
                                    //                         product: Constants.PRODUCT_MIS);
                                    break;

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                    strike1, strike2);
                                    File.AppendAllText(filename2path, content);
                                }
                                //Thread.Sleep(500);
                            }
                            Dictionary<string, dynamic> orderstatus = new Dictionary<string, dynamic>();
                            //orderstatus = GetOrderStatus(response1);//not required



                            string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                   strikeprice1, strikeprice2, lstoffers1[0].Price, "BUY",
                                                                                   totaltradedquantity1, "LineNo2082");
                            //Console.WriteLine(writetofile1);
                            //File.AppendAllText(filename1path, writetofile1);

                            //if (orderstatus["Status"].ToLower() != "cancelled"
                            //        && orderstatus["Status"].ToLower() != "rejected")
                            {
                                //sell 1st strike
                                //Thread.Sleep(2000);
                                Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                for (int j = 0; j < 5; ++j)
                                {
                                    try
                                    {
                                        //response2 = PlaceOrderOption(strikeprice: strike2, transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                        //                           price: lstbids2[0].Price, lotsize: (lotsize * totallots),
                                        //                           product: Constants.PRODUCT_MIS);

                                        break;

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strike1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }

                                //string writetofile6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                //                                            "", strike2, lstoffers2[0].Price, "SELL", totaltradedquantity2, "LineNo2113");
                                //Console.WriteLine(writetofile6);
                                //File.AppendAllText(filename1path, writetofile6);

                                Dictionary<string, dynamic> orderstatus2 = new Dictionary<string, dynamic>();


                                //if (orderstatus2["Status"].ToLower() != "cancelled"
                                //  && orderstatus2["Status"].ToLower() != "rejected")
                                {
                                    nooflots += totallots;
                                    remaininglots = nooflots;
                                    totalbuyprice = totalbidprice;

                                    string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", DateTime.Now, strikeprice1, strikeprice2,
                                                                   lstoffers1[0].Price, lstoffers2[0].Price, totalofferprice, nooflots, "BUY", currentliveprice, "LineNo1206");
                                    Console.WriteLine(writetofile);
                                    File.AppendAllText(filename1path, writetofile);
                                    //
                                    int lotdiff = maxlots - nooflots;
                                    if (lotdiff < totallots)
                                    {
                                        int rem = lotdiff % totallots;
                                        if (rem != 0)
                                        {
                                            totallots = lotdiff;
                                        }
                                    }
                                    //break;
                                }

                            }//end of if

                            squareoffprice = totalofferprice * (1.0m + squareoffpercent / 100);
                            stoplossprice = totalofferprice - stoplossdeltaprice;


                            if (squareoffpercent == 4.0m)
                            {
                                mintargetprice1 = mintargetprice / 2;
                                maxtargetprice1 = maxtargetprice / 2;
                                stoplossdeltaprice1 = stoplossdeltaprice / 2;
                            }

                            if ((squareoffprice-totalbidprice) < mintargetprice1)
                            {
                                squareoffprice = totalofferprice + mintargetprice1;
                            }
                            else if ((squareoffprice-totalbidprice) > maxtargetprice1)
                            {
                                squareoffprice = totalofferprice + maxtargetprice1;
                            }
                            stoplossprice = totalofferprice - stoplossdeltaprice1;

                        }
                        else if (remaininglots > 0 && (totalbidprice >= squareoffprice || currtime >= squareofftime))//square off
                        {
                            //square off everything 
                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            //Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                            //                            transactiontype: Constants.TRANSACTION_TYPE_BUY,
                            //                             price: lstoffers1[0].Price,
                            //                             lotsize: (lotsize * totallots),
                            //                            validity: Constants.VALIDITY_DAY,
                            //                            product: Constants.PRODUCT_MIS);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                //                                   transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                //                                   price: lstoffers2[0].Price,
                                //                                   lotsize: (lotsize * totallots),
                                //                                   validity: Constants.VALIDITY_DAY,
                                //                                   product: Constants.PRODUCT_MIS);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}%,{9},{10}\n", DateTime.Now, strikeprice1, strikeprice2, "AppOrderID",
                                                                    lstbids1[0].Price, lstbids2[0].Price, totalbidprice, totallots,
                                                                    percentageincrease, currentliveprice, Math.Round(squareoffprice,2));
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }

                            }
                            squareoffpercent = 4.0m;
                            totalbuyprice = totalbuyprice * 0.98m;
                            //stoplossprice = totalbuyprice * (1.0m + stoplosspercent / 100);

                            //decimal totalperceinc1 = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);


                            //string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%\r\n", DateTime.Now, currentliveprice,
                            //                  strike2, totalbidprice, totalbuyprice, totaltradedquantity1,
                            //                          totaltradedquantity2, totalperceinc1);

                            //Console.WriteLine(filecontent1);
                            //File.AppendAllText(filename1path, filecontent1);                            
                            nooflots = remaininglots;

                        }//end of if block line 1173
                        else if (remaininglots > 0 &&    (totalbidprice <= stoplossprice)
                            /*totalofferprice >= stoplossprice*/)
                        {
                            //stop loss module
                            //exit if the index has moved by stoploss low /high of the price is more than 40rs from the sell price


                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            //Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                            //                            transactiontype: Constants.TRANSACTION_TYPE_BUY,
                            //                             price: lstoffers1[0].Price,
                            //                             lotsize: (lotsize * totallots),
                            //                            validity: Constants.VALIDITY_DAY,
                            //                            product: Constants.PRODUCT_MIS);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                //                                   transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                //                                   price: lstoffers2[0].Price,
                                //                                   lotsize: (lotsize * totallots),
                                //                                   validity: Constants.VALIDITY_DAY,
                                //                                   product: Constants.PRODUCT_MIS);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbidprice-totalbuyprice) * 1.0m / totalbuyprice) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}%,{9},{10}\n", DateTime.Now, strikeprice1, strikeprice2, "AppOrderID",
                                                                    lstbids1[0].Price, lstbids1[0].Price, totalbidprice, totallots,
                                                                    percentageincrease, currentliveprice, Math.Round(stoplossprice,2));
                                //take the next position above this price and 2% higher
                                totalbuyprice = totalbidprice * 0.96m;
                                if (totalbidprice -2.0m < totalbuyprice)
                                {
                                    totalbuyprice = totalbuyprice - 2.0m; //add 2 rs
                                }
                                squareoffpercent = 4.0m;
                                //squareoffprice = totalbuyprice * (1.0m - squareoffpercent / 100);
                                //stoplossprice = totalbuyprice * (1.0m + stoplosspercent / 100);

                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);


                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }
                            }
                            nooflots = remaininglots;
                        }

                        if (nooflots > 0)
                        {

                            decimal totalperceinc = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);
                            string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%,{8},{9},{10},{11}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                        totalbuyprice, lstbids1[0].Price, lstbids2[0].Price, totalbidprice, totalperceinc, currentliveprice, remaininglots,
                                                        squareoffprice, OptionsData._broker);
                            File.AppendAllText(filename3path, filecontent5);
                            Console.WriteLine(filecontent5);
                        }
                        else
                        {
                            Console.WriteLine(currentliveprice);
                            string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                        totalbuyprice, lstbids1[0].Price, lstbids2[0].Price, totalbidprice, remaininglots, currentliveprice, OptionsData._broker);
                            //Console.WriteLine(filecontent5);
                            File.AppendAllText(filename3path, filecontent5);

                        }
                        //if (nooflots == 0)
                        //{
                        //    Thread.Sleep(3000);
                        //}
                        //else
                        //{
                        //    Thread.Sleep(1000);
                        //}


                    }//end of try
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                               strike1, strike2);
                        File.AppendAllText(filename2path, filecontent1);
                    }
                    finally
                    {
                        if (nooflots == 0)
                        {
                            Thread.Sleep(3000);
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        static int stindex = 10;
        public static decimal? CalculateSuperTrendStart(object obj)
        {
         
            IIFLHistorical histdata = (IIFLHistorical)obj;
            decimal[] highpcdiff = new decimal[12];
            decimal[] lowpcdiff = new decimal[12];
            decimal[] highlowdiff = new decimal[12];
            decimal[] calctruerange = new decimal[11];
            decimal[] calcrange = new decimal[12];
            decimal[] arrnewtruerange = new decimal[12];


            ArrayList arrlist = new ArrayList();

            decimal[] maxarray = new decimal[3];
            decimal?[] advstup = new decimal?[12];
            decimal?[] advstdn = new decimal?[12];
            decimal?[] streal = new decimal?[12];
            decimal?[] truerangemax = new decimal?[12];
            decimal?[] hlaverage = new decimal?[12];

            int currindex = histdata.lsthistorical.Count()-1;
            for (int i = 0; i < histdata.lsthistorical.Count(); ++i)
            {

                highpcdiff[i] = Math.Round(histdata.lsthistorical[i].High - histdata.lsthistorical[i].Close,2);
                maxarray[0] = highpcdiff[i];
                lowpcdiff[i] = Math.Round(histdata.lsthistorical[i].Low - histdata.lsthistorical[i].Close,2);
                maxarray[1] = lowpcdiff[i];
                highlowdiff[i] = Math.Round(histdata.lsthistorical[i].High - histdata.lsthistorical[i].Low,2);
                maxarray[2] = highlowdiff[i];
                calctruerange[i] = maxarray.Max();
            }
            //Array arrnewtruerange = Array.CreateInstance(typeof(decimal), 12);
            //calctruerange.CopyTo(arrnewtruerange, 1);
            for (int j = 0; j < calctruerange.Count()-1; ++j)
            {
                arrnewtruerange[j] = calctruerange[j + 1];
            }
            int stmultiplier = 2;
             truerangemax[currindex] = calctruerange.Average();
             hlaverage[currindex] = (histdata.lsthistorical[currindex].High + histdata.lsthistorical[currindex].Low)/2;
            decimal ?bstup = (hlaverage[currindex] + (truerangemax[currindex] * stmultiplier));
            decimal ?bstdown = (hlaverage[currindex] - (truerangemax[currindex] * stmultiplier));


            if(bstup<advstup[stindex] || advstup[stindex] < histdata.lsthistorical[stindex].Close)
            {
                advstup[currindex] = bstup;
            }
            else
            {
                advstup[currindex] = bstdown;
            }

            if(bstdown>advstdn[stindex] || histdata.lsthistorical[stindex].High< advstdn[stindex])
            {
                advstdn[currindex] = bstdown;
            }
            else
            {
                advstdn[currindex] = advstdn[stindex];
            }

            if(advstup[currindex]< histdata.lsthistorical[currindex].Close)
            {
                streal[currindex] = advstup[currindex];
            }
            else
            {
                streal[currindex] = advstdn[currindex];
            }

            Console.WriteLine(streal[currindex]);

            return streal[currindex];

        }


        public static void BankNiftyOHLCOptionStrategy(object obj)
        {

            string expiryDate = "06May2021";
            string threadname = "Thread1";

            string filename = string.Format("dataOHLC{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), threadname);
            //string filename1= string.Format("TradedataOHLC.csv{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), threadname);

            string filenamepath = string.Format(@"C:\Algotrade\scalperoutput\Live-{0}.csv", filename);
            string filename1path = string.Format(@"C:\Algotrade\scalperoutput\Trade-{0}.csv", filename);

            string instrument = "BANKNIFTY";
            string strikeprice = "";

            if(instrument=="BANKNIFTY")
            {
                strikeprice = "NIFTY BANK";
            }
            else if(instrument=="NIFTY")
            {
                strikeprice = "NIFTY 50";
            }

            TimeSpan start = new TimeSpan(09, 30, 0);

            while(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < start)
            {
                Console.WriteLine(DateTime.Now.ToString());
                if(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) >= start)
                {
                    break;
                }
            }


            Dictionary<string, dynamic> quotes = GetQuoteBank(strikeprice);
            KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
            long highindexvalue = Convert.ToUInt32(kvpquote.Value.High);
            long lowindexvalue = Convert.ToUInt32(kvpquote.Value.Low);

            long lowstrikeprice;
            long highstrikeprice;
            //get low strike(PE) and high strike (CE)
            long lowatmstrikeprice = GetatthemoneyStrike(long.Parse(lowindexvalue.ToString()));
            long highatmstrikeprice = GetatthemoneyStrike(long.Parse(highindexvalue.ToString()));
            //get pe strike at mutiples of 500

            long lowrem = lowatmstrikeprice % 200;
            if (lowrem <= 100)
            {
                lowstrikeprice = lowatmstrikeprice - lowrem - 200;
            }
            else
            {
                lowstrikeprice = lowatmstrikeprice - lowrem;

            }

            long highrem = 200-(highatmstrikeprice % 200);
            if (highrem <= 100)
            {
                highstrikeprice = highatmstrikeprice + highrem + 200;
            }
            else
            {
                highstrikeprice = highatmstrikeprice + highrem;
            }

            Dictionary<string, string> dictParamStrikes = GetStrikePrices((lowstrikeprice).ToString(),
                                         (highstrikeprice).ToString(), OptionsData._broker, instrument, expiryDate);

            string putstrike1 = dictParamStrikes["strike1"];
            string callstrike1 = dictParamStrikes["strike2"];

            int lots = 10;
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", putstrike1},
                {"strikeprice2",callstrike1 },
                //{"strikeprice3",strike3 },
                //{"strikeprice4",strike4 }
            };

            Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids1["lotsize"]);
            string putinstrumentid = instrumentids1["strikeprice1"].ToString();
            string callinstrumentid = instrumentids1["strikeprice2"].ToString();


            decimal stoplossdeltaperc = 20;
            decimal sqoffpricedelta = stoplossdeltaperc * 2.0m;
            decimal totalbuyprice = 0.0m;
            decimal sqoffprice = 0.0m;
            decimal stoplossprice = 0.0m;

            bool putpostaken = false;
            bool callpostaken = false;
            decimal putbuyprice = 0.0m;
            decimal callbuyprice = 0.0m;
            decimal callbidsprice = 0.0m;


          

             while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) > start)
            {
                try
                {
                    Dictionary<string, dynamic> quotes1 = GetQuoteBank(strikeprice);
                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    long currentspotprice = Convert.ToUInt32(kvpquote1.Value.LastPrice);
                    Console.WriteLine("High:" + highindexvalue + "," + "Low" + lowindexvalue + ",spot:" + currentspotprice);

                    if (currentspotprice > highindexvalue 
                        && callpostaken == false)
                    {
                        //take call position
                        Console.WriteLine("Call position taken");
                        callpostaken = true;
                        Dictionary<string, dynamic> callquotes = GetQuote(callinstrumentid);
                        KeyValuePair<string, dynamic> kvpcallquote = callquotes.ElementAt(0);
                        dynamic lstofferscall = kvpcallquote.Value.Offers;
                        dynamic lstbidscall = kvpcallquote.Value.Bids;
                        int totaltradedquantitycall = kvpcallquote.Value.LastQuantity;
                        callbuyprice = lstofferscall[0].Price;
                        callbidsprice = lstbidscall[0].Price;
                        totalbuyprice += callbuyprice;
                        sqoffprice += (callbuyprice * (1 + sqoffpricedelta / 100.0m));
                        stoplossprice += (callbuyprice * (1 - (stoplossdeltaperc / 100.0m)));

                        Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();

                        //response1 = PlaceOrderOption(strikeprice: strike1, transactiontype: Constants.TRANSACTION_TYPE_SELL,
                        //                         price: lstbids1[0].Price, lotsize: (lotsize * totallots),
                        //                         product: Constants.PRODUCT_MIS);
                        string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}\r\n", DateTime.Now, callstrike1, "CE",
                                                              lstofferscall[0].Price, lstbidscall[0].Price, "BUY", currentspotprice,highindexvalue,lowindexvalue,
                                                              sqoffprice,stoplossprice);
                        Console.WriteLine(writetofile1);
                        File.AppendAllText(filename1path, writetofile1);

                    }
                    else if (currentspotprice < lowindexvalue && putpostaken == false)
                    {
                        Console.WriteLine("Put position taken");
                        putpostaken = true;
                        Dictionary<string, dynamic> putquotes = GetQuote(putinstrumentid);
                        KeyValuePair<string, dynamic> kvpputquote = putquotes.ElementAt(0);
                        dynamic lstoffersput = kvpputquote.Value.Offers;
                        dynamic lstbidsput = kvpputquote.Value.Bids;
                        int totaltradedquantityput = kvpputquote.Value.LastQuantity;
                        putbuyprice = lstoffersput[0].Price;
                        totalbuyprice += putbuyprice;
                        sqoffprice += (putbuyprice * (1 + sqoffpricedelta / 100.0m));
                        stoplossprice += (putbuyprice * (1 - (stoplossdeltaperc / 100.0m)));
                        Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();

                        //response1 = PlaceOrderOption(strikeprice: strike1, transactiontype: Constants.TRANSACTION_TYPE_SELL,
                        //                         price: lstbids1[0].Price, lotsize: (lotsize * totallots),
                        //                         product: Constants.PRODUCT_MIS);

                        string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", DateTime.Now, putstrike1, "PE",
                                                                  lstoffersput[0].Price, lstbidsput[0].Price, "BUY", currentspotprice,lowindexvalue,sqoffprice,stoplossprice);
                        Console.WriteLine(writetofile1);
                        File.AppendAllText(filename1path, writetofile1);

                    }


                    decimal totalcurrprice = 0.0m;
                    decimal putbidprice = 0.0m;
                    decimal callbidprice = 0.0m;
                    decimal callofferprice = 0.0m;
                    decimal putofferprice = 0.0m;

                    int totaltradedquantity1 = 0, totaltradedquantity2 = 0;
                    if (callpostaken == true)
                    {
                        Dictionary<string, dynamic> callquotes = GetQuote(callinstrumentid);
                        KeyValuePair<string, dynamic> kvpcallquote = callquotes.ElementAt(0);
                        dynamic lstofferscall = kvpcallquote.Value.Offers;
                        dynamic lstbidscall = kvpcallquote.Value.Bids;
                        totaltradedquantity1 = kvpcallquote.Value.LastQuantity;
                        callbidprice = lstbidscall[0].Price;
                        callofferprice = lstofferscall[0].Price;
                        totalcurrprice += callbidprice;
                        string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},", DateTime.Now, callstrike1, callbuyprice,
                                                                 callofferprice, callbidprice, totalcurrprice, currentspotprice);
                        Console.Write(writetofile1);
                        File.AppendAllText(filenamepath, writetofile1);
                        //write 
                    }
                    if (putpostaken == true)
                    {
                        Dictionary<string, dynamic> putquotes = GetQuote(putinstrumentid);
                        KeyValuePair<string, dynamic> kvpputquote = putquotes.ElementAt(0);
                        dynamic lstoffersput = kvpputquote.Value.Offers;
                        dynamic lstbidsput = kvpputquote.Value.Bids;
                        totaltradedquantity2 = kvpputquote.Value.LastQuantity;
                        putbidprice = lstbidsput[0].Price;
                        putofferprice = lstoffersput[0].Price;
                        totalcurrprice += putbidprice;

                        //
                        string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},", DateTime.Now, putstrike1, putbuyprice,
                                                                lstoffersput[0].Price, lstbidsput[0].Price, totalcurrprice, currentspotprice);
                        //
                        Console.Write(writetofile1);
                        File.AppendAllText(filenamepath, writetofile1);

                    }

                    //calculate percentage
                    decimal percentage = 0.0m;
                    if (totalbuyprice > 0)
                    {
                        percentage = Math.Round(((totalcurrprice - totalbuyprice) / totalbuyprice) * 100.0m, 2);
                        string writetofile2 = string.Format("{0}%\r\n", percentage);
                        //
                        Console.WriteLine(writetofile2);
                        File.AppendAllText(filenamepath, writetofile2);
                    }

                    // string writetofile2 = string.Format("{0}%\r\n",percentage);
                    //
                    //Console.WriteLine(writetofile2);
                    //File.AppendAllText(filenamepath, writetofile2);

                    if (totalcurrprice < stoplossprice || totalcurrprice > sqoffprice)
                    {
                        //squreoff everything
                        if (callpostaken == true)
                        {
                            //sqof call
                            string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%\r\n", DateTime.Now, callstrike1, "CE",
                                                                  callofferprice, callbidprice, "SELL", currentspotprice, percentage);
                            Console.WriteLine(writetofile1);
                            File.AppendAllText(filename1path, writetofile1);

                        }
                        if (putpostaken == true)
                        {
                            //sqoffputposition
                            string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%\r\n", DateTime.Now, putstrike1, "PE",
                                                                 putofferprice, putbidprice, "SELL", currentspotprice, percentage);
                            Console.WriteLine(writetofile1);
                            File.AppendAllText(filename1path, writetofile1);
                        }

                        break;
                    }

                    Thread.Sleep(1000);//sleep for 1 second
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    File.AppendAllText(filenamepath, ex.StackTrace);
                }
            }//end of while


        }

        public static void ZerolossStrategyScanner(object obj)
        {

            string putstrike1 = "";
            string callstrike1 = "";
            string strikeprice1 = "BANKNIFTY";
            string strikeprice2 = "NIFTY 50";
            string instrumentId1 = "";
            string instrumentId2 = "";
            string putinstrumentId1 = "";
            string callinstrumentId1 = "";


            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("ZerolossArbitrage{0}{1}", DateTime.Now.ToString("dd-MM-yyyy"), threadname);
            string filename1path = string.Format(@"C:\Algotrade\scalperoutput\LiveScanner-{0}.csv", filename1);
            string filecontent = "DateTime,BidFUTPrice,AskFutPrice, SpotPrice,BidPEPrice,AskPEPrice,BidCEPrice,AskCEPrice,PEStrike,CEStrike,futspotcallpediff,Instrument,Direction\r\n";

            File.AppendAllText(filename1path, filecontent);

            TimeSpan end = new TimeSpan(15, 31, 0);
            string expiryDate = "29Apr2021";

            List<string> lstscannerinstruments = new List<string>();
            lstscannerinstruments.Add("NIFTY");
            lstscannerinstruments.Add("BANKNIFTY");
            decimal tradeentrypricediff = 0.0m;
            int tradecount = 0;
            while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
            {
                //get fut quote
                try
                {
                    foreach (string instrument in lstscannerinstruments)
                    {
                        Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                       {
                            {"strikeprice1", instrument},
                            //{"strikeprice2",strikeprice2 }
                        };

                        //Thread.Sleep(100);
                        Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsizeFUT(ref dictinstrumentid, expiryDate);
                        instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                        //instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                        Dictionary<string, dynamic> quotes = GetQuote(instrumentId1);
                        KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                        dynamic lstoffersfut = kvpquote.Value.Offers;
                        dynamic lstbidsfut = kvpquote.Value.Bids;

                        dynamic currentlivepricefut = kvpquote.Value.LastPrice;

                        if (instrument.ToUpper() == "NIFTY")
                        {
                            strikeprice1 = "NIFTY 50";

                        }
                        else if (instrument.ToUpper() == "BANKNIFTY")
                        {
                            strikeprice1 = "NIFTY BANK";
                        }
                        Dictionary<string, dynamic> dictinstrumentid1 = new Dictionary<string, dynamic>()
                        {
                          {"strikeprice1", strikeprice1}
                        };
                        //get spot price
                        //Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid1);
                        //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                        //instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                        Dictionary<string, dynamic> quotes1 = GetQuoteBank(strikeprice1);
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        dynamic currentlivepricespot = kvpquote1.Value.LastPrice;

                        //get pe ce quote calculate at the money strike price

                        long currstrikeprice = GetatthemoneyStrike(long.Parse(currentlivepricespot.ToString()));
                        Dictionary<string, string> dictParamStrikes = GetStrikePrices((currstrikeprice).ToString(),
                                                     (currstrikeprice).ToString(), "iifl", instrument);

                        putstrike1 = dictParamStrikes["strike1"];
                        callstrike1 = dictParamStrikes["strike2"];

                        Dictionary<string, dynamic> dictoptinstrumentid1 = new Dictionary<string, dynamic>() {
                        {"strikeprice1", putstrike1},
                        {"strikeprice2",callstrike1 }

                        };

                        Dictionary<string, dynamic> instrumentids2 = GetInstrumentIDLotsize(ref dictoptinstrumentid1);
                        putinstrumentId1 = Convert.ToString(instrumentids2["strikeprice1"]);
                        callinstrumentId1 = Convert.ToString(instrumentids2["strikeprice2"]);

                        Dictionary<string, dynamic> putquotes1 = GetQuote(putinstrumentId1);
                        KeyValuePair<string, dynamic> putkvpquote1 = putquotes1.ElementAt(0);
                        dynamic currentputprice = putkvpquote1.Value.LastPrice;
                        dynamic lstoffersput = putkvpquote1.Value.Offers;
                        dynamic lstbidsput = putkvpquote1.Value.Bids;

                        Dictionary<string, dynamic> callquotes1 = GetQuote(callinstrumentId1);
                        KeyValuePair<string, dynamic> callkvpquote1 = callquotes1.ElementAt(0);
                        dynamic currentcallprice = callkvpquote1.Value.LastPrice;
                        dynamic lstofferscall = callkvpquote1.Value.Offers;
                        dynamic lstbidscall = callkvpquote1.Value.Bids;

                        //decimal putcalldiff = Convert.ToDecimal(currentcallprice - currentputprice);
                        //decimal futspotdiff = Convert.ToDecimal(currentlivepricefut - currentlivepricespot);

                        decimal putcalldiff = Convert.ToDecimal(lstofferscall[0].Price - lstbidsput[0].Price);
                        decimal futspotdiff = Convert.ToDecimal(lstbidsfut[0].Price - currentlivepricespot);

                        decimal delta = lstbidsfut[0].Price - currstrikeprice - lstofferscall[0].Price + lstbidsput[0].Price;


                        if (instrument=="NIFTY")
                        {
                            tradeentrypricediff = 6.0m;
                        }
                        else
                        {
                            tradeentrypricediff = 18.0m;
                        }

                        if (futspotdiff > 0)
                        {
                            string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}\r\n", DateTime.Now, lstbidsfut[0].Price, lstoffersfut[0].Price, currentlivepricespot,
                                                                                            lstbidsput[0].Price, lstoffersput[0].Price, lstbidscall[0].Price, lstofferscall[0].Price, putstrike1, 
                                                                                            callstrike1, delta, instrument, "LONG");

                            Console.WriteLine(writetofile1);
                            File.AppendAllText(filename1path, writetofile1);

                            if (delta >= tradeentrypricediff)
                            {
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.WriteLine("ArbitrageTrade Triggered");
                                Console.BackgroundColor = ConsoleColor.Black;

                                //string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", DateTime.Now, currentlivepricefut, currentlivepricespot,
                                //                                                             putstrike1, callstrike1, currentputprice, currentcallprice,
                                //                                                            (futspotdiff - Math.Abs(putcalldiff)), instrument, "LONG");
                                //Console.BackgroundColor = ConsoleColor.Green;
                                //Console.WriteLine(writetofile1);
                                CsvInputFileArbitrage csvfileobj = new CsvInputFileArbitrage();
                                csvfileobj.FutureInstrumentId = instrumentId1;
                                csvfileobj.Index = instrument;
                                csvfileobj.PutStrike = putstrike1;
                                csvfileobj.CallStrike = callstrike1;
                                csvfileobj.PutInstrumentId = putinstrumentId1;
                                csvfileobj.CallInstrumentId = callinstrumentId1;
                                csvfileobj.CallPrice = lstofferscall[0].Price;
                                csvfileobj.PutPrice = lstbidsput[0].Price;
                                csvfileobj.FutureIndexPrice = lstbidsfut[0].Price;
                                csvfileobj.SpotIndexPrice = currentlivepricespot;
                                csvfileobj.Arbitagedelta = delta;
                                csvfileobj.StrikePrice = currstrikeprice;
                                csvfileobj.Tradecount = ++tradecount;
                                Thread tharr = new Thread(new ParameterizedThreadStart(OptionsData.InitiateArbitrageTrade));
                                tharr.Name = "Thread" + tradecount.ToString();
                                tharr.Start(csvfileobj);
                                //InitiateArbitrageTrade(csvfileobj);

                                //File.AppendAllText(filename1path, writetofile1);
                            }
                        }
                        else if (futspotdiff < 0)
                        {
                            string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", DateTime.Now, currentlivepricefut, currentlivepricespot,
                                                                                            putstrike1, callstrike1, currentputprice, currentcallprice,
                                                                                            (Math.Abs(futspotdiff) - Math.Abs(putcalldiff)), instrument, "SHORT");
                            Console.WriteLine(writetofile1);

                            //if ((Math.Abs(futspotdiff) - Math.Abs(putcalldiff)) > 0)
                            {
                                //string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", DateTime.Now, currentlivepricefut, currentlivepricespot,
                                //                                                             putstrike1, callstrike1, currentputprice, currentcallprice,
                                //                                                             (Math.Abs(futspotdiff) - Math.Abs(putcalldiff)), instrument, "SHORT");
                                //Console.BackgroundColor = ConsoleColor.Red;
                                //Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);
                            }
                        }

                    }//end of foreach

                    Thread.Sleep(new TimeSpan(0, 1, 0)); // wait for 15 mins
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("{0},{1}", ex.Message, ex.StackTrace));
                }
            }


        }//end of function

        public static void InitiateArbitrageTrade(object obj)
        {
            CsvInputFileArbitrage csvInputFileArbitrage = (CsvInputFileArbitrage)obj;

            string count = csvInputFileArbitrage.Tradecount.ToString(); ;
            string filename1 = string.Format("ArbitrageTrade{0}_{1}", DateTime.Now.ToString("dd-MM-yyyy"), count);
            string filename1path = string.Format(@"C:\Algotrade\arbitrage\{0}{1}.csv",csvInputFileArbitrage.Index, filename1);
            string filecontent = "DateTime,SpotPrice,OffersFutPrice,BidsFutPrice,CallStrike,OffersCallPrice,BidsCallPrice,BidsPutPrice,OffersPutPrice,Diff\r\n";

            File.AppendAllText(filename1path, filecontent);

            TimeSpan end = new TimeSpan(15, 30, 0);
            string indexstrike = csvInputFileArbitrage.Index;
            string futinstrumentid = csvInputFileArbitrage.FutureInstrumentId;
            string instrument = csvInputFileArbitrage.Index;
            long strikeprice1 = csvInputFileArbitrage.StrikePrice;
            string putinstrumentId = csvInputFileArbitrage.PutInstrumentId;
            string callinstrumentId = csvInputFileArbitrage.CallInstrumentId;
            string callstrike = csvInputFileArbitrage.CallStrike;
            string putstrikeprice = csvInputFileArbitrage.PutStrike;
            decimal spotprice = csvInputFileArbitrage.SpotIndexPrice;
            decimal futprice = csvInputFileArbitrage.FutureIndexPrice;
            decimal callprice = csvInputFileArbitrage.CallPrice;
            decimal putprice = csvInputFileArbitrage.PutPrice;
            decimal arbitragedelta = csvInputFileArbitrage.Arbitagedelta;
            string index = "";

            decimal currfutoptdiff = futprice - (callprice - putprice);
            string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now.ToString(), spotprice, futprice, callstrike,
                  callprice, putprice, currfutoptdiff, arbitragedelta);

            File.AppendAllText(filename1path, writetofile);



            while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
            {
                try
                {

                    //instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                    Dictionary<string, dynamic> quotes = GetQuote(futinstrumentid);
                    KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                    dynamic lstoffersfut = kvpquote.Value.Offers;
                    dynamic lstbidsfut = kvpquote.Value.Bids;

                    dynamic currentlivepricefut = kvpquote.Value.LastPrice;

                    if (instrument.ToUpper() == "NIFTY")
                    {
                        index = "NIFTY 50";

                    }
                    else if (instrument.ToUpper() == "BANKNIFTY")
                    {
                        index = "NIFTY BANK";
                    }
                    Dictionary<string, dynamic> dictinstrumentid1 = new Dictionary<string, dynamic>()
                    {
                          {"strikeprice1", index}
                    };
                    //get spot price
                    //Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid1);
                    //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    //instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                    Dictionary<string, dynamic> quotes1 = GetQuoteBank(index);
                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    dynamic currentlivepricespot = kvpquote1.Value.LastPrice;

                    //get pe ce quote calculate at the money strike price                             



                    Dictionary<string, dynamic> putquotes1 = GetQuote(putinstrumentId);
                    KeyValuePair<string, dynamic> putkvpquote1 = putquotes1.ElementAt(0);
                    dynamic currentputprice = putkvpquote1.Value.LastPrice;
                    dynamic lstoffersput = putkvpquote1.Value.Offers;
                    dynamic lstbidsput = putkvpquote1.Value.Bids;

                    Dictionary<string, dynamic> callquotes1 = GetQuote(callinstrumentId);
                    KeyValuePair<string, dynamic> callkvpquote1 = callquotes1.ElementAt(0);
                    dynamic currentcallprice = callkvpquote1.Value.LastPrice;
                    dynamic lstofferscall = callkvpquote1.Value.Offers;
                    dynamic lstbidscall = callkvpquote1.Value.Bids;


                    //calculate difference between poistion taken and current price
                    decimal putcalldiff = Convert.ToDecimal(lstofferscall[0].Price - lstbidsput[0].Price);
                    decimal futspotdiff = Convert.ToDecimal(lstbidsfut[0].Price - currentlivepricespot);

                    decimal futoptdiff = lstoffersfut[0].Price - strikeprice1 - (lstbidscall[0].Price - lstoffersput[0].Price);
                    //decimal delta = currfutoptdiff - futoptdiff;
                    string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", DateTime.Now.ToString(), currentlivepricespot, lstoffersfut[0].Price,lstbidsfut[0].Price, callstrike,
                        lstofferscall[0].Price,lstbidscall[0].Price, lstbidsput[0].Price,lstoffersput[0].Price, futoptdiff);
                    File.AppendAllText(filename1path, writetofile1);
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
          


        }
        public static long  GetatthemoneyStrike(long currentlivepricespot)
        {
            long currentlivepricetens = currentlivepricespot % 100;
            long currentlivepricewhole = (currentlivepricespot / 100) * 100;

            long currentlivepricewholenext = currentlivepricewhole + 100;
            decimal percentfromlowerstrike = ((currentlivepricespot - currentlivepricewhole) * 1.0m / currentlivepricewhole) * 100.0m;
            decimal percentfromupperstrike = ((currentlivepricewholenext - currentlivepricespot) * 1.0m / currentlivepricewholenext) * 100.0m;

            long currstrikeprice = 0;
            if (currentlivepricetens < 50)
            {
                currstrikeprice = currentlivepricewhole;
            }
            else
            {
                currstrikeprice = currentlivepricewholenext;
            }
            return currstrikeprice;
        }
        public static void NiftyShortStrangleStrategyLive(object strikeobj)
        {
            //CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            //decimal currentpivot1 = csvinputobj.Pivot;
            //decimal startpivot = csvinputobj.Pivot;
            string strikeprice = "NIFTY 50";
            //string strikeprice2 = csvinputobj.Strike2;
            int strikedelta = int.Parse(strikeobj.ToString());
            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;           
            //delete existing files
            try
            {
                string threadname = Thread.CurrentThread.Name;
                string filename1 = string.Format("Nifty-SStrang{0}-{1}-{2}_{3}", DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", strikedelta, threadname);
                string filename1path = string.Format(@"C:\Algotrade\scalperoutput\LiveTrade-{0}.csv", filename1);
                string filecontent = "DateTime,OrderID,Bid-Ask,InstrumentId,Price,OrderType,LineNo,PivotDown,PivotUp,Maxlots\r\n";

                File.AppendAllText(filename1path, filecontent);

                string filename2 = string.Format("Nifty-SStrang-{0}-{1}_{2}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog", threadname);

                string filename3 = string.Format("Nifty-SStrangle-{0}-{1}-{2}_{3}", DateTime.Now.ToString("dd-MM-yyyy"), "Continous", strikedelta, threadname);

                string filename3path = string.Format(@"C:\Algotrade\scalperoutput\LiveTest-{0}.csv", filename3);
                string filecontent3 = "Datetime,LivePrice,Strike,BuyDeltaPrice,SellDeltaPrice,Quantity1,Quantity2,PercentageInc" + threadname + "\r\n";
                File.AppendAllText(filename3path, filecontent3);

                string filename2path = string.Format(@"C:\Algotrade\scalperoutput\{0}.csv", filename2);
                string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
                File.AppendAllText(filename2path, filecontent2);





                //Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                //{
                //    {"strikeprice1", strikeprice1},
                //    //{"strikeprice2",strikeprice2 }
                //};


                //Thread.Sleep(100);
                //Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
                int lotsize = 25;
                string instrumentId1 = "";
                string instrumentId2 = "";
                string instrumentId3 = "";
                string instrumentId4 = "";
                int alive = 0;
                decimal totalbuyprice;
                decimal totalbidprice = 1.0m;
                decimal totalofferprice = 0.0m;

                string strike1 = "";
                string strike2 = "";
                string strike3 = "";
                string strike4 = "";
                bool bfirst = true;

                //int strikedelta = 0;

                int totallots = 10;
                int nooflots = 0;
                int maxlots = 10;

                string strikeprice1 = "";
                string strikeprice2 = "";

                decimal squareoffprice = 0.0m;
                //decimal stoplossprice = 0.0m;
                int remaininglots = 0;
                int strikedelta1 = strikedelta;
                TimeSpan end = new TimeSpan(15, 31, 0);
                bool takeposition = false;

                decimal maxprice = 0.0m;
                long stoplosslow = 0;
                long stoplosshigh = 0;
                decimal squareoffpercent = 8.0m;
                //decimal stoplosspercent = 15.0m;

                decimal stoplossprice = 0.0m;

                decimal mintargetprice = 6.0m;
                decimal maxtargetprice = 11.0m;
                decimal stoplossdeltaprice = 12.0m;

                decimal mintargetprice1 = 6.0m;
                decimal maxtargetprice1 = 11.0m;
                decimal stoplossdeltaprice1 = 12.0m;

                totalbuyprice = 40.0m;

                while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
                {
                    try
                    {
                        //Thread.Sleep(5000);
                        //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                        //instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                        Dictionary<string, dynamic> quotes = GetQuoteBank(strikeprice);
                        KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                        dynamic currentliveprice1 = kvpquote.Value.LastPrice;
                        decimal prevdayclose = kvpquote.Value.PrevDayClose;//Previous day close price

                        long currentliveprice = long.Parse(currentliveprice1.ToString());

                        decimal currpricepercentincval = prevdayclose * 1.015m;
                        decimal currpricepercentdecval = prevdayclose * (1.0m - 0.015m);



                        long currentlivepricetens = currentliveprice % 100;
                        long currentlivepricewhole = (currentliveprice / 100) * 100;

                        long currentlivepricewholenext = currentlivepricewhole + 100;
                        //when it nears a whole number +-10 take position


                        long currstrikeprice = 0;
                        //check for which nearest strike to be taken
                        decimal percentfromlowerstrike = ((currentliveprice - currentlivepricewhole) * 1.0m / currentlivepricewhole) * 100.0m;
                        decimal percentfromupperstrike = ((currentlivepricewholenext - currentliveprice) * 1.0m / currentlivepricewholenext) * 100.0m;

                        if (nooflots == 0)
                        {
                            takeposition = false;
                            strikedelta = strikedelta1;

                            if (percentfromlowerstrike < percentfromupperstrike)
                            {
                                currstrikeprice = currentlivepricewhole;
                            }
                            else
                            {
                                currstrikeprice = currentlivepricewholenext;
                            }
                            //give strikeprice input
                            //currstrikeprice = 24500;
                            //strikedelta = 200;
                            //if (currentlivepricetens >= 40 && currentlivepricetens < 60)
                            //{
                            //    currstrikeprice = currentlivepricewhole + 50;
                            //    strikedelta = strikedelta - 50;
                            //    takeposition = true;
                            //    //Console.WriteLine("Short position taken" + currentliveprice + "," + currstrikeprice);
                            //}
                            if (Math.Abs(currstrikeprice - currentliveprice) <= 25)
                            {
                                takeposition = true;
                                //Console.WriteLine("Short position taken" + currentliveprice +","+currstrikeprice);
                                //stoplosslow = currstrikeprice - (strikedelta / 2);
                                //stoplosshigh = currstrikeprice + (strikedelta / 2);

                                stoplosslow = currstrikeprice - (150);
                                stoplosshigh = currstrikeprice + (150);

                            }


                            //if (_broker == "iifl")
                            {
                                //strike1 = "BANKNIFTY29Oct2020" + (currstrikeprice - strikedelta) + "PE";
                                //strike2 = "BANKNIFTY29Oct2020" + (currstrikeprice + strikedelta) + "CE";
                            }
                            Dictionary<string, string> dictParamStrikes = GetStrikePrices((currstrikeprice - strikedelta).ToString(),
                                             (currstrikeprice + strikedelta).ToString(), "iifl", "NIFTY");

                            strikeprice1 = dictParamStrikes["strike1"];
                            strikeprice2 = dictParamStrikes["strike2"];

                            //strike3 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta - 100)) + "CE";
                            //strike4 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta + 100)) + "CE";

                            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                            {
                                {"strikeprice1", strikeprice1},
                                {"strikeprice2",strikeprice2 },
                                //{"strikeprice3",strike3 },
                                //{"strikeprice4",strike4 }
                            };

                            Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid);
                            lotsize = Convert.ToInt32(instrumentids1["lotsize"]);

                            //lotsize = 25;
                            //if (_broker == "iifl")
                            {
                                instrumentId1 = Convert.ToString(instrumentids1["strikeprice1"]);
                                instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                                //instrumentId1 = Convert.ToString(instrumentids1["strikeprice3"]);
                                // instrumentId2 = Convert.ToString(instrumentids1["strikeprice4"]);
                            }
                            //else
                            {
                                //instrumentId1 = strike1;
                                //instrumentId2 = strike2;

                            }

                            if (_broker != "iifl")
                            {
                                Dictionary<string, string> dictParamStrikes1 = GetStrikePrices((currstrikeprice - strikedelta).ToString(),
                                               (currstrikeprice + strikedelta).ToString(), "zerodhanew");
                                strike1 = dictParamStrikes1["strike1"];
                                strike2 = dictParamStrikes1["strike2"];

                            }
                            else
                            {
                                strike1 = instrumentId1;
                                strike2 = instrumentId2;
                            }


                        }//end of if

                        Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                        Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                        //add nfo 
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                        dynamic lstbids1 = kvpquote1.Value.Bids;
                        dynamic lstoffers1 = kvpquote1.Value.Offers;
                        dynamic lstbids2 = kvpquote2.Value.Bids;
                        dynamic lstoffers2 = kvpquote2.Value.Offers;



                        int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                        int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                        decimal percentchange = currentliveprice / prevdayclose;



                        totalbidprice = (lstbids1[0].Price + lstbids2[0].Price);
                        totalofferprice = (lstoffers1[0].Price + lstoffers2[0].Price);

                        //check if bid and ask difference is more than 1%
                        if ((lstbids1[0].Price * 1.01m) < lstoffers1[0].Price || (lstbids2[0].Price * 1.01m) < lstoffers2[0].Price)
                        {
                            takeposition = false;
                            Console.WriteLine("Bid/Ask more than 1% difference" + strikeprice1 + "," + strikeprice2);
                        }

                        if (takeposition == true && nooflots == 0)
                        {
                            Console.WriteLine("{0},Expected Price:{1},CurrentPrice:{2}", currentliveprice, totalbuyprice, totalbidprice);
                        }


                        //Console.WriteLine(currentliveprice);

                        TimeSpan prdiscstarttime = new TimeSpan(9, 30, 0);
                        TimeSpan prdiscendtime = new TimeSpan(9, 44, 0);

                        TimeSpan starttime = new TimeSpan(9, 45, 0);
                        TimeSpan endtime = new TimeSpan(13, 30, 0);
                        TimeSpan squareofftime = new TimeSpan(14, 45, 0);
                        TimeSpan currtime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        //take position after 09:45


                        //discover max price from 9:30 to 9:45 on which the short strangle will be bought
                        //if (nooflots == 0 &&
                        //     currtime >= prdiscstarttime && currtime <= prdiscendtime)
                        //{
                        //    if (totalofferprice >= maxprice)
                        //    {
                        //        maxprice = totalofferprice;
                        //        totalbuyprice = maxprice;
                        //        Console.WriteLine("max price:" + maxprice);
                        //    }
                        //    Thread.Sleep(5000);
                        //}

                        if (nooflots < maxlots && takeposition == true
                            && currtime >= starttime && currtime <= endtime
                            && totalbidprice >= totalbuyprice)
                        {
                            Console.WriteLine("Short position taken" + currentliveprice + "," + currstrikeprice);
                            //buy 2nd strike at bid price and buy 2nd strike one at ask price
                            Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                            for (int i = 0; i < 5; ++i)
                            {

                                try
                                {
                                    //response1 = PlaceOrderOption(strikeprice: strike1, transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                    //                         price: lstbids1[0].Price, lotsize: (lotsize * totallots),
                                    //                         product: Constants.PRODUCT_MIS);
                                    break;

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                    strike1, strike2);
                                    File.AppendAllText(filename2path, content);
                                }
                                //Thread.Sleep(500);
                            }
                            Dictionary<string, dynamic> orderstatus = new Dictionary<string, dynamic>();
                            //orderstatus = GetOrderStatus(response1);//not required



                            string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                   strikeprice1, strikeprice2, lstoffers1[0].Price, "BUY",
                                                                                   totaltradedquantity1, "LineNo2082");
                            //Console.WriteLine(writetofile1);
                            //File.AppendAllText(filename1path, writetofile1);

                            //if (orderstatus["Status"].ToLower() != "cancelled"
                            //        && orderstatus["Status"].ToLower() != "rejected")
                            {
                                //sell 1st strike
                                //Thread.Sleep(2000);
                                Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                for (int j = 0; j < 5; ++j)
                                {
                                    try
                                    {
                                        //response2 = PlaceOrderOption(strikeprice: strike2, transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                        //                           price: lstbids2[0].Price, lotsize: (lotsize * totallots),
                                        //                           product: Constants.PRODUCT_MIS);

                                        break;

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strike1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }

                                //string writetofile6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                //                                            "", strike2, lstoffers2[0].Price, "SELL", totaltradedquantity2, "LineNo2113");
                                //Console.WriteLine(writetofile6);
                                //File.AppendAllText(filename1path, writetofile6);

                                Dictionary<string, dynamic> orderstatus2 = new Dictionary<string, dynamic>();


                                //if (orderstatus2["Status"].ToLower() != "cancelled"
                                //  && orderstatus2["Status"].ToLower() != "rejected")
                                {
                                    nooflots += totallots;
                                    remaininglots = nooflots;
                                    totalbuyprice = totalbidprice;

                                    string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", DateTime.Now, strikeprice1, strikeprice2,
                                                                   lstbids1[0].Price, lstbids2[0].Price, totalbidprice, nooflots, "SELL", currentliveprice, "LineNo1206");
                                    Console.WriteLine(writetofile);
                                    File.AppendAllText(filename1path, writetofile);
                                    //
                                    int lotdiff = maxlots - nooflots;
                                    if (lotdiff < totallots)
                                    {
                                        int rem = lotdiff % totallots;
                                        if (rem != 0)
                                        {
                                            totallots = lotdiff;
                                        }
                                    }
                                    //break;
                                }

                            }//end of if

                            squareoffprice = totalbidprice * (1.0m - squareoffpercent / 100);


                            if (squareoffpercent == 4.0m)
                            {
                                mintargetprice1 = mintargetprice / 2;
                                maxtargetprice1 = maxtargetprice / 2;
                                stoplossdeltaprice1 = stoplossdeltaprice / 2;
                            }
                            if (totalbidprice - squareoffprice < mintargetprice1)
                            {
                                squareoffprice = totalbidprice - mintargetprice1;
                            }
                            else if (totalbidprice - squareoffprice > maxtargetprice1)
                            {
                                squareoffprice = totalbidprice - maxtargetprice;
                            }
                            stoplossprice = totalbidprice + stoplossdeltaprice1;


                        }
                        else if (remaininglots > 0 && (totalofferprice <= squareoffprice || currtime >= squareofftime))//square off
                        {
                            //square off everything 
                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            //Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                            //                            transactiontype: Constants.TRANSACTION_TYPE_BUY,
                            //                             price: lstoffers1[0].Price,
                            //                             lotsize: (lotsize * totallots),
                            //                            validity: Constants.VALIDITY_DAY,
                            //                            product: Constants.PRODUCT_MIS);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                //                                   transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                //                                   price: lstoffers2[0].Price,
                                //                                   lotsize: (lotsize * totallots),
                                //                                   validity: Constants.VALIDITY_DAY,
                                //                                   product: Constants.PRODUCT_MIS);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbuyprice - totalofferprice) * 1.0m / totalbuyprice) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}%,{9},{10}\n", DateTime.Now, strikeprice1, strikeprice2, "AppOrderID",
                                                                    lstoffers1[0].Price, lstoffers2[0].Price, totalofferprice, totallots,
                                                                    percentageincrease, currentliveprice, squareoffprice);
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }

                            }
                            squareoffpercent = 4.0m;
                            //squareoffprice = totalbuyprice * (1.0m - squareoffpercent / 100);
                            //stoplossprice = totalbuyprice * (1.0m + stoplosspercent / 100);



                            //decimal totalperceinc1 = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);


                            //string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%\r\n", DateTime.Now, currentliveprice,
                            //                  strike2, totalbidprice, totalbuyprice, totaltradedquantity1,
                            //                          totaltradedquantity2, totalperceinc1);

                            //Console.WriteLine(filecontent1);
                            //File.AppendAllText(filename1path, filecontent1);                            
                            nooflots = remaininglots;

                        }//end of if block line 1173
                        else if (remaininglots > 0 &&
                            (currentliveprice <= stoplosslow || currentliveprice >= stoplosshigh || totalofferprice >= stoplossprice)
                            /*totalofferprice >= stoplossprice*/)
                        {
                            //stop loss module

                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            //Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                            //                            transactiontype: Constants.TRANSACTION_TYPE_BUY,
                            //                             price: lstoffers1[0].Price,
                            //                             lotsize: (lotsize * totallots),
                            //                            validity: Constants.VALIDITY_DAY,
                            //                            product: Constants.PRODUCT_MIS);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                //                                   transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                //                                   price: lstoffers2[0].Price,
                                //                                   lotsize: (lotsize * totallots),
                                //                                   validity: Constants.VALIDITY_DAY,
                                //                                   product: Constants.PRODUCT_MIS);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbuyprice - totalofferprice) * 1.0m / totalbuyprice) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}%,{9},{10}\n", DateTime.Now, strikeprice1, strikeprice2, "AppOrderID",
                                                                    lstoffers1[0].Price, lstoffers2[0].Price, totalofferprice, totallots,
                                                                    percentageincrease, currentliveprice,stoplossprice);
                                //take the next position above this price
                                totalbuyprice = totalofferprice * 1.02m;//take next at 1% more
                                if (totalofferprice + 2.0m > totalbuyprice)
                                {
                                    totalbuyprice = totalofferprice + 2.0m; //add 2 rs
                                }
                                squareoffpercent = 4.0m;
                                //squareoffprice = totalbuyprice * (1.0m - squareoffpercent / 100);
                                //stoplossprice = totalbuyprice * (1.0m + stoplosspercent / 100);

                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }
                            }
                            nooflots = remaininglots;
                        }

                        if (nooflots > 0)
                        {

                            decimal totalperceinc = Math.Round(((totalbuyprice - totalofferprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);
                            string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5}%,{6},{7},{8},{9}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                        totalbuyprice, totalofferprice, totalperceinc, currentliveprice, remaininglots,
                                                        squareoffprice, OptionsData._broker);
                            File.AppendAllText(filename3path, filecontent5);
                            Console.WriteLine(filecontent5);
                        }
                        else
                        {
                            Console.WriteLine(currentliveprice);
                            string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5}%,{6},{7}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                        totalbuyprice, totalofferprice, remaininglots, currentliveprice, OptionsData._broker);
                            //Console.WriteLine(filecontent5);
                            File.AppendAllText(filename3path, filecontent5);

                        }
                        if (nooflots == 0)
                        {
                            Thread.Sleep(3000);
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }


                    }//end of try
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                               strike1, strike2);
                        File.AppendAllText(filename2path, filecontent1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void BNIronCondorStrategyLive(object strikeobj)
        {
            //CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            //decimal currentpivot1 = csvinputobj.Pivot;
            //decimal startpivot = csvinputobj.Pivot;
            string strikeprice = "NIFTY BANK";
            //string strikeprice2 = csvinputobj.Strike2;
            int strikedelta = int.Parse(strikeobj.ToString());
            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;           
            //delete existing files
            try
            {
                string threadname = Thread.CurrentThread.Name;
                string filename1 = string.Format("BN-SStrang{0}-{1}-{2}_{3}", DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", strikedelta, threadname);
                string filename1path = string.Format(@"C:\Algotrade\scalperoutput\LiveTrade-{0}.csv", filename1);
                string filecontent = "DateTime,OrderID,Bid-Ask,InstrumentId,Price,OrderType,LineNo,PivotDown,PivotUp,Maxlots\r\n";

                File.AppendAllText(filename1path, filecontent);

                string filename2 = string.Format("BN-SStrang-{0}-{1}_{2}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog", threadname);

                string filename3 = string.Format("BN-SStrangle-{0}-{1}-{2}", DateTime.Now.ToString("dd-MM-yyyy"), "Continous", threadname);

                string filename3path = string.Format(@"C:\Algotrade\scalperoutput\LiveTest-{0}.csv", filename3);
                string filecontent3 = "Datetime,LivePrice,Strike,BuyDeltaPrice,SellDeltaPrice,Quantity1,Quantity2,PercentageInc" + threadname + "\r\n";
                File.AppendAllText(filename3path, filecontent3);

                string filename2path = string.Format(@"C:\Algotrade\scalperoutput\{0}.csv", filename2);
                string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
                File.AppendAllText(filename2path, filecontent2);





                //Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                //{
                //    {"strikeprice1", strikeprice1},
                //    //{"strikeprice2",strikeprice2 }
                //};


                //Thread.Sleep(100);
                //Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
                int lotsize = 25;
                string instrumentId1 = "";
                string instrumentId2 = "";
                string instrumentId3 = "";
                string instrumentId4 = "";
                int alive = 0;
                decimal totalbuyprice;
                decimal totalbidprice = 1.0m;
                decimal totalofferprice = 0.0m;

                string strike1 = "";
                string strike2 = "";
                string strike3 = "";
                string strike4 = "";
                bool bfirst = true;

                //int strikedelta = 0;

                int totallots = 10;
                int nooflots = 0;
                int maxlots = 10;

                string strikeprice1 = "";
                string strikeprice2 = "";

                decimal squareoffprice = 0.0m;
                decimal stoplossprice = 0.0m;
                int remaininglots = 0;
                int strikedelta1 = strikedelta;
                TimeSpan end = new TimeSpan(15, 31, 0);
                bool takeposition = false;
                totalbuyprice = 0.0m;
                decimal maxprice = 0.0m;
                long stoplosslow = 0;
                long stoplosshigh = 0;

                while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
                {
                    try
                    {
                        //Thread.Sleep(5000);
                        //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                        //instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                        Dictionary<string, dynamic> quotes = GetQuoteBank(strikeprice);
                        KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                        dynamic currentliveprice1 = kvpquote.Value.LastPrice;
                        decimal prevdayclose = kvpquote.Value.PrevDayClose;//Previous day close price

                        long currentliveprice = long.Parse(currentliveprice1.ToString());

                        decimal currpricepercentincval = prevdayclose * 1.015m;
                        decimal currpricepercentdecval = prevdayclose * (1.0m - 0.015m);




                        long currentlivepricetens = currentliveprice % 100;
                        long currentlivepricewhole = (currentliveprice / 100) * 100;

                        long currentlivepricewholenext = currentlivepricewhole + 100;
                        //when it nears a whole number +-10 take position


                        long currstrikeprice = 0;
                        //check for which nearest strike to be taken
                        decimal percentfromlowerstrike = ((currentliveprice - currentlivepricewhole) * 1.0m / currentlivepricewhole) * 100.0m;
                        decimal percentfromupperstrike = ((currentlivepricewholenext - currentliveprice) * 1.0m / currentlivepricewholenext) * 100.0m;

                        if (nooflots == 0)
                        {
                            takeposition = false;
                            strikedelta = strikedelta1;

                            if (percentfromlowerstrike < percentfromupperstrike)
                            {
                                currstrikeprice = currentlivepricewhole;
                            }
                            else
                            {
                                currstrikeprice = currentlivepricewholenext;
                            }
                            //give strikeprice input
                            //currstrikeprice = 24500;
                            //strikedelta = 200;
                            //if (currentlivepricetens >= 40 && currentlivepricetens < 60)
                            //{
                            //    currstrikeprice = currentlivepricewhole + 50;
                            //    strikedelta = strikedelta - 50;
                            //    takeposition = true;
                            //    //Console.WriteLine("Short position taken" + currentliveprice + "," + currstrikeprice);
                            //}
                            if (Math.Abs(currstrikeprice - currentliveprice) <= 25)
                            {
                                takeposition = true;
                                //Console.WriteLine("Short position taken" + currentliveprice +","+currstrikeprice);
                                stoplosslow = currstrikeprice - 125;
                                stoplosshigh = currstrikeprice + 125;

                            }


                            //if (_broker == "iifl")
                            {
                                //strike1 = "BANKNIFTY29Oct2020" + (currstrikeprice - strikedelta) + "PE";
                                //strike2 = "BANKNIFTY29Oct2020" + (currstrikeprice + strikedelta) + "CE";
                            }
                            Dictionary<string, string> dictParamStrikes = GetStrikePrices((currstrikeprice - strikedelta).ToString(),
                                             (currstrikeprice + strikedelta).ToString(), "iifl");

                            strikeprice1 = dictParamStrikes["strike1"];
                            strikeprice2 = dictParamStrikes["strike2"];

                            //strike3 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta - 100)) + "CE";
                            //strike4 = "BANKNIFTY22Oct2020" + (currstrikeprice + (strikedelta + 100)) + "CE";

                            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                            {
                                {"strikeprice1", strikeprice1},
                                {"strikeprice2",strikeprice2 },
                                //{"strikeprice3",strike3 },
                                //{"strikeprice4",strike4 }
                            };

                            Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid);
                            lotsize = Convert.ToInt32(instrumentids1["lotsize"]);

                            //lotsize = 25;
                            //if (_broker == "iifl")
                            {
                                instrumentId1 = Convert.ToString(instrumentids1["strikeprice1"]);
                                instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                                //instrumentId1 = Convert.ToString(instrumentids1["strikeprice3"]);
                                // instrumentId2 = Convert.ToString(instrumentids1["strikeprice4"]);
                            }
                            //else
                            {
                                //instrumentId1 = strike1;
                                //instrumentId2 = strike2;

                            }

                            if (_broker != "iifl")
                            {
                                Dictionary<string, string> dictParamStrikes1 = GetStrikePrices((currstrikeprice - strikedelta).ToString(),
                                               (currstrikeprice + strikedelta).ToString(), "zerodhanew");
                                strike1 = dictParamStrikes1["strike1"];
                                strike2 = dictParamStrikes1["strike2"];

                            }
                            else
                            {
                                strike1 = instrumentId1;
                                strike2 = instrumentId2;
                            }

                        }//end of if

                        Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                        Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                        //add nfo 
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                        dynamic lstbids1 = kvpquote1.Value.Bids;
                        dynamic lstoffers1 = kvpquote1.Value.Offers;
                        dynamic lstbids2 = kvpquote2.Value.Bids;
                        dynamic lstoffers2 = kvpquote2.Value.Offers;



                        int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                        int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                        decimal percentchange = currentliveprice / prevdayclose;



                        totalbidprice = (lstbids1[0].Price + lstbids2[0].Price);
                        totalofferprice = (lstoffers1[0].Price + lstoffers2[0].Price);
                        if (takeposition == true)
                        {
                            Console.WriteLine("{0},Expected Price:{1},CurrentPrice:{2}", currentliveprice, totalbuyprice, totalbidprice);
                        }

                        decimal squareoffpercent = 8.0m;
                        decimal stoplosspercent = 15.0m;
                        //Console.WriteLine(currentliveprice);

                        TimeSpan prdiscstarttime = new TimeSpan(9, 30, 0);
                        TimeSpan prdiscendtime = new TimeSpan(9, 44, 0);

                        TimeSpan starttime = new TimeSpan(9, 45, 0);
                        TimeSpan endtime = new TimeSpan(13, 0, 0);
                        TimeSpan currtime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        //take position after 09:45


                        //discover max price from 9:30 to 9:45 on which the short strangle will be bought
                        if (nooflots == 0 &&
                             currtime >= prdiscstarttime && currtime <= prdiscendtime)
                        {
                            if (totalofferprice >= maxprice)
                            {
                                maxprice = totalofferprice;
                                totalbuyprice = maxprice;
                                Console.WriteLine("max price:" + maxprice);
                            }
                            Thread.Sleep(3000);
                        }

                        if (nooflots < maxlots && takeposition == true
                            && currtime >= starttime && currtime <= endtime && totalbidprice >= totalbuyprice)
                        {
                            Console.WriteLine("Short position taken" + currentliveprice + "," + currstrikeprice);
                            //buy 2nd strike at bid price and buy 2nd strike one at ask price
                            Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                            for (int i = 0; i < 5; ++i)
                            {

                                try
                                {
                                    //response1 = PlaceOrderOption(strikeprice: strike1, transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                    //                         price: lstbids1[0].Price, lotsize: (lotsize * totallots),
                                    //                         product: Constants.PRODUCT_MIS);
                                    break;

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                    strike1, strike2);
                                    File.AppendAllText(filename2path, content);
                                }
                                //Thread.Sleep(500);
                            }
                            Dictionary<string, dynamic> orderstatus = new Dictionary<string, dynamic>();
                            //orderstatus = GetOrderStatus(response1);//not required



                            string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                   strikeprice1, strikeprice2, lstoffers1[0].Price, "BUY",
                                                                                   totaltradedquantity1, "LineNo2082");
                            //Console.WriteLine(writetofile1);
                            //File.AppendAllText(filename1path, writetofile1);

                            //if (orderstatus["Status"].ToLower() != "cancelled"
                            //        && orderstatus["Status"].ToLower() != "rejected")
                            {
                                //sell 1st strike
                                //Thread.Sleep(2000);
                                Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                for (int j = 0; j < 5; ++j)
                                {
                                    try
                                    {
                                        //response2 = PlaceOrderOption(strikeprice: strike2, transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                        //                           price: lstbids2[0].Price, lotsize: (lotsize * totallots),
                                        //                           product: Constants.PRODUCT_MIS);

                                        break;

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strike1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }

                                //string writetofile6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                //                                            "", strike2, lstoffers2[0].Price, "SELL", totaltradedquantity2, "LineNo2113");
                                //Console.WriteLine(writetofile6);
                                //File.AppendAllText(filename1path, writetofile6);

                                Dictionary<string, dynamic> orderstatus2 = new Dictionary<string, dynamic>();


                                //if (orderstatus2["Status"].ToLower() != "cancelled"
                                //  && orderstatus2["Status"].ToLower() != "rejected")
                                {
                                    nooflots += totallots;
                                    remaininglots = nooflots;
                                    totalbuyprice = totalbidprice;

                                    string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", DateTime.Now, strikeprice1, strikeprice2,
                                                                   lstbids1[0].Price, lstbids2[0].Price, totalbidprice, nooflots, "SELL", currentliveprice, "LineNo1206");
                                    Console.WriteLine(writetofile);
                                    File.AppendAllText(filename1path, writetofile);
                                    //
                                    int lotdiff = maxlots - nooflots;
                                    if (lotdiff < totallots)
                                    {
                                        int rem = lotdiff % totallots;
                                        if (rem != 0)
                                        {
                                            totallots = lotdiff;
                                        }
                                    }
                                    //break;
                                }

                            }//end of if

                            squareoffprice = totalbidprice * (1.0m - squareoffpercent / 100);
                            stoplossprice = totalbidprice * (1.0m + stoplosspercent / 100);

                        }
                        else if (remaininglots > 0 && (currentliveprice <= stoplosslow || currentliveprice >= stoplosshigh)
                            /*&& totalofferprice <= squareoffprice*/)//square off
                        {
                            //square off everything 
                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            //Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                            //                            transactiontype: Constants.TRANSACTION_TYPE_BUY,
                            //                             price: lstoffers1[0].Price,
                            //                             lotsize: (lotsize * totallots),
                            //                            validity: Constants.VALIDITY_DAY,
                            //                            product: Constants.PRODUCT_MIS);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                //                                   transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                //                                   price: lstoffers2[0].Price,
                                //                                   lotsize: (lotsize * totallots),
                                //                                   validity: Constants.VALIDITY_DAY,
                                //                                   product: Constants.PRODUCT_MIS);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbuyprice - totalofferprice) * 1.0m / totalbuyprice) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}%\n", DateTime.Now, strikeprice1, strikeprice2, "AppOrderID",
                                                                    lstoffers1[0].Price, lstoffers2[0].Price, totalofferprice, totallots,
                                                                    percentageincrease);
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }

                            }


                            //decimal totalperceinc1 = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);


                            //string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%\r\n", DateTime.Now, currentliveprice,
                            //                  strike2, totalbidprice, totalbuyprice, totaltradedquantity1,
                            //                          totaltradedquantity2, totalperceinc1);

                            //Console.WriteLine(filecontent1);
                            //File.AppendAllText(filename1path, filecontent1);                            
                            nooflots = remaininglots;

                        }//end of if block line 1173
                        else if (remaininglots > 0 && totalofferprice >= stoplossprice)
                        {
                            //stop loss module

                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                                                        transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                                         price: lstoffers1[0].Price,
                                                         lotsize: (lotsize * totallots),
                                                        validity: Constants.VALIDITY_DAY,
                                                        product: Constants.PRODUCT_MIS);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                //                                   transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                //                                   price: lstoffers2[0].Price,
                                //                                   lotsize: (lotsize * totallots),
                                //                                   validity: Constants.VALIDITY_DAY,
                                //                                   product: Constants.PRODUCT_MIS);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbuyprice - totalofferprice) * 1.0m / totalbuyprice) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}%\n", DateTime.Now, strikeprice1, strikeprice2, "AppOrderID",
                                                                    lstoffers1[0].Price, lstoffers2[0].Price, totalofferprice, totallots,
                                                                    percentageincrease);
                                //take the next position above this price
                                totalbuyprice = totalofferprice * 1.01m;//take next at 1% more
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }
                            }
                            nooflots = remaininglots;
                        }

                        if (nooflots > 0)
                        {

                            decimal totalperceinc = Math.Round(((totalbuyprice - totalofferprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);
                            string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5}%,{6},{7},{8}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                        totalbuyprice, totalofferprice, totalperceinc, currentliveprice, remaininglots, OptionsData._broker);
                            File.AppendAllText(filename3path, filecontent5);
                            Console.WriteLine(filecontent5);
                        }
                        else
                        {
                            Console.WriteLine(currentliveprice);
                            string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5}%,{6},{7}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                        totalbuyprice, totalofferprice, remaininglots, currentliveprice, OptionsData._broker);
                            //Console.WriteLine(filecontent5);
                            File.AppendAllText(filename3path, filecontent5);

                        }
                        Thread.Sleep(5000);

                    }//end of try
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                               strike1, strike2);
                        File.AppendAllText(filename2path, filecontent1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void LongStrangleStrategyLive()
        {
            //CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            //decimal currentpivot1 = csvinputobj.Pivot;
            //decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = "NIFTY BANK";
            //string strikeprice2 = csvinputobj.Strike2;

            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;           
            //delete existing files

            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("LST-{0}-{1}_{2}", strikeprice1, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData");
            string filename1path = string.Format(@"C:\Algotrade\scalperoutput\Live{0}.csv", filename1);
            string filecontent = "DateTime,OrderID,Bid/Ask,InstrumentId,Price,OrderType,LineNo,PivotDown,PivotUp,Maxlots\r\n";

            File.AppendAllText(filename1path, filecontent);

            string filename2 = string.Format("LST-{0}-{1}_{2}", strikeprice1, DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");


            string filename3 = string.Format("LST-{0}-{1}_{2}", strikeprice1, DateTime.Now.ToString("dd-MM-yyyy"), "Continous");

            string filename3path = string.Format(@"C:\Algotrade\scalperoutput\Live-{0}.csv", filename3);
            string filecontent3 = "Datetime,Bid1,Offer2,Bid2,Offer1,SellDeltaPrice,BuyDeltaPrice," + threadname + "\r\n";
            File.AppendAllText(filename3path, filecontent3);

            string filename2path = string.Format(@"C:\Algotrade\scalperoutput\credit{0}.csv", filename2);
            string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename2path, filecontent2);


            int nooflots = 0;


            //Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            //{
            //    {"strikeprice1", strikeprice1},
            //    //{"strikeprice2",strikeprice2 }
            //};


            Thread.Sleep(100);
            //Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = 25;
            string instrumentId1 = "";
            string instrumentId2 = "";
            int alive = 0;
            decimal totalbuyprice = 0.0m;
            decimal totalbidprice = 0.0m;
            TimeSpan end = new TimeSpan(15, 31, 0);
            int maxlots = 1;
            string strike1 = "";
            string strike2 = "";
            bool bfirst = true;
            //while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            //run till 03:30PM
            while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
            {
                try
                {
                    //Thread.Sleep(1000);

                    //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    //instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);
                    decimal prevdayclose = 22370.0m;

                    Dictionary<string, dynamic> quotes = GetQuoteBank(strikeprice1);
                    KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                    dynamic currentliveprice1 = kvpquote.Value.LastPrice;
                    prevdayclose = kvpquote.Value.PrevDayClose;//Previous day close price

                    long currentliveprice = long.Parse(currentliveprice1.ToString());

                    decimal currpricepercentincval = prevdayclose * 1.015m;
                    decimal currpricepercentdecval = prevdayclose * (1.0m - 0.015m);


                    //if (currentliveprice>= currpricepercentincval || currentliveprice<= currpricepercentdecval)
                    {
                        long currentlivepricetens = currentliveprice % 100;
                        long currentlivepricewhole = (currentliveprice / 100) * 100;

                        long currentlivepricewholenext = currentlivepricewhole + 100;


                        long currstrikeprice = 0;
                        //check for which nearest strike to be taken
                        decimal percentfromlowerstrike = ((currentliveprice - currentlivepricewhole) * 1.0m / currentlivepricewhole) * 100.0m;
                        decimal percentfromupperstrike = ((currentlivepricewholenext - currentliveprice) * 1.0m / currentlivepricewholenext) * 100.0m;

                        if (nooflots == 0)
                        {
                            if (percentfromlowerstrike < percentfromupperstrike)
                            {
                                currstrikeprice = currentlivepricewhole;
                            }
                            else
                            {
                                currstrikeprice = currentlivepricewholenext;
                            }

                            strike1 = "BANKNIFTY15Oct2020" + (currstrikeprice - 300) + "PE";
                            strike2 = "BANKNIFTY15Oct2020" + currstrikeprice + 300 + "CE";


                            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                            {
                                {"strikeprice1", strike1},
                                {"strikeprice2",strike2 }
                            };

                            Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid);
                            lotsize = Convert.ToInt32(instrumentids1["lotsize"]);

                            instrumentId1 = Convert.ToString(instrumentids1["strikeprice1"]);
                            instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                        }//end of if

                        Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                        Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);
                        //add nfo 
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                        //get bid ask price

                        dynamic lstbids1 = kvpquote1.Value.Bids;
                        dynamic lstoffers1 = kvpquote1.Value.Offers;
                        dynamic lstbids2 = kvpquote2.Value.Bids;
                        dynamic lstoffers2 = kvpquote2.Value.Offers;
                        int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                        int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                        totalbidprice = (lstoffers1[0].Price + lstoffers2[0].Price);

                        //if (bfirst)
                        //{
                        //    totalbidprice = 532.5m;
                        //    bfirst = false;
                        //}
                        //decimal percent

                        decimal percentchange = currentliveprice / prevdayclose;
                        if (DateTime.Now.Hour < 15 && nooflots < maxlots
                              /*&&(percentchange >=1.015m || percentchange <= (1.0m-0.015m))*/)
                        {
                            //buy 1 lot
                            //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                            {
                                //buy 2nd strike at bid price and buy 2nd strike one at ask price
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int i = 0; i < 5; ++i)
                                {
                                    try
                                    {
                                        //response1 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_BUY,
                                        //  lstoffers2[0].Price, lotsize);

                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strike1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }
                                Dictionary<string, dynamic> orderstatus = new Dictionary<string, dynamic>();
                                //orderstatus = GetOrderStatus(response1);//not required

                                //Thread.Sleep(100);

                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                       "", strike2, lstoffers2[0].Price, "BUY", totaltradedquantity2, "LineNo1138");
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() != "cancelled"
                                //        && orderstatus["Status"].ToLower() != "rejected")
                                {
                                    //sell 1st strike
                                    //Thread.Sleep(2000);
                                    Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                    for (int j = 0; j < 5; ++j)
                                    {
                                        try
                                        {
                                            //response2 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_BUY,
                                            //                                                     lstbids1[0].Price, lotsize);

                                            string writetofile6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                "", strike1, lstoffers1[0].Price, "BUY", totaltradedquantity1, "LineNo1154");
                                            Console.WriteLine(writetofile6);
                                            File.AppendAllText(filename1path, writetofile6);
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                            strike1, strike2);
                                            File.AppendAllText(filename2path, content);
                                        }
                                        Thread.Sleep(500);
                                    }

                                    Dictionary<string, dynamic> orderstatus2 = new Dictionary<string, dynamic>();



                                    //if (orderstatus2["Status"].ToLower() != "cancelled"
                                    //  && orderstatus2["Status"].ToLower() != "rejected")
                                    {
                                        nooflots += 1;
                                        totalbuyprice = totalbidprice;

                                        string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, lstoffers1[0].Price, lstoffers2[0].Price,
                                                                       totalbuyprice, nooflots, "BUY", currentliveprice, "LineNo1206");
                                        Console.WriteLine(writetofile);
                                        File.AppendAllText(filename1path, writetofile);
                                        //
                                    }

                                }//end of if

                            }
                        }
                        else if (nooflots > 0 && (totalbidprice >= (totalbuyprice * 1.05m)))
                        {

                            //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= nextpivotdown)
                            {
                                //buy 1 lot
                                //buy 1st strike at ask price and sell 2nd strike at bid price   
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int j1 = 0; j1 < 5; ++j1)
                                {
                                    try
                                    {
                                        //response1 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_SELL,
                                        //                                 lstoffers1[0].Price, lotsize);

                                        string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID", "",
                                                                                           strike2, lstbids1[0].Price, totaltradedquantity1, "SELL", "LineNo152");
                                        Console.WriteLine(filecontent5);
                                        File.AppendAllText(filename1path, filecontent5);
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strikeprice1, strike2);
                                        File.AppendAllText(filename2path, content);

                                    }
                                    Thread.Sleep(500);
                                }//else

                                //Thread.Sleep(100);
                                //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);
                                //string filecontent5 = string.Format("{0},{1},{2},{3},{4}-{5}\r\n", DateTime.Now, response1["result"]["AppOrderID"], 
                                //                                                           instrumentId2,lstoffers1[0].Price, "BUY", "LineNo1220");
                                //Console.WriteLine(filecontent5);
                                //File.AppendAllText(filename1path, filecontent5);

                                //sell 2nd strike 
                                Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                for (int j2 = 0; j2 < 5; ++j2)
                                {
                                    try
                                    {
                                        //response2 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_SELL,
                                        //lstbids2[0].Price, lotsize);
                                        string filecontent6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID", "", strike2,
                                                                                    lstbids2[0].Price, "SELL", totaltradedquantity2, "LineNo1284");
                                        Console.WriteLine(filecontent6);
                                        File.AppendAllText(filename1path, filecontent6);
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strikeprice1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }

                                //string filecontent6 = string.Format("{0},{1},{2},{3},{4}\n", DateTime.Now, response2["result"]["AppOrderID"],
                                //                                                     lstbids2[0].Price, "SELL", "LineNo1227");
                                //Console.WriteLine(filecontent6);
                                //File.AppendAllText(filename1path, filecontent6);



                                //if (orderstatus["Status"].ToLower() == "complete" && orderstatus6["Status"].ToLower() == "complete")
                                {
                                    //both legs squared off so reduce the no of lots
                                    nooflots -= 1;

                                    decimal percentageincrease = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100.0m, 2);

                                    string writetofile = string.Format("{0},{1},{2},{3},{4}%,{5},{6},{7}\n", DateTime.Now, lstbids1[0].Price,
                                                                     lstbids2[0].Price, totalbidprice, percentageincrease,
                                                                       "SELL", nooflots, "LineNo1245");
                                    Console.WriteLine(writetofile);
                                    File.AppendAllText(filename1path, writetofile);
                                    //
                                }
                                //else
                                //{
                                //    string writetofile = string.Format("{0},{1},{2},{3},{4},{5}\n", DateTime.Now, lstoffers1[0].Price, lstbids2[0].Price,
                                //                                                  (lstoffers1[0].Price - lstbids2[0].Price), nooflots, "FAILED");
                                //    Console.WriteLine(writetofile);
                                //    File.AppendAllText(filename1path, writetofile);
                                //}
                            }//end of if Math.Abs

                        }
                        else if (nooflots > 0 && DateTime.Now.Hour == 14 && DateTime.Now.Minute >= 45)
                        {
                            //square off everything 

                        }
                        decimal totalperceinc = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);
                        string strtotalpercinc = "";
                        if (totalperceinc >= 0)
                        {
                            strtotalpercinc = "+" + totalperceinc.ToString();
                        }
                        else
                        {
                            strtotalpercinc = totalperceinc.ToString();
                        }

                        string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}\r\n", DateTime.Now, currentliveprice,
                                          strike2, totalbidprice, totalbuyprice, (lstbids1[0].Price + lstbids2[0].Price), totaltradedquantity1,
                                                  totaltradedquantity2, strtotalpercinc);

                        File.AppendAllText(filename3path, filecontent1);

                        if (alive % 250 == 0)
                        {
                            Console.WriteLine(DateTime.Now + "-I am alive -" + alive + "-" + "," + strikeprice1);
                            string filecontent9 = string.Format("{0},Alive-,{1}\r\n", DateTime.Now, alive);
                            //File.AppendAllText(filename2path, filecontent9);
                            Console.WriteLine(filecontent9);
                        }
                        ++alive;


                    }//end of if block line 1173



                }//end of try
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                           strike1, strike2);
                    File.AppendAllText(filename2path, filecontent1);
                }
            }

        }

        static void BuySupertrendStrategyTrigger(object stobj)
        {
            string broker = OptionsData._broker;
            TrriggerObject triggerobj = (TrriggerObject)stobj;

            string expiryDate = triggerobj.expiryDate;
            string threadname = triggerobj.threadname;
            string stockname = triggerobj.Stockname;
            decimal triggerprice = triggerobj.Currprice;

            string filename = string.Format("TriggerCIStrategy{0}-{1}-{2}", DateTime.Now.ToString("dd-MM-yyyy"), stockname,threadname);
            //string filename1= string.Format("TradedataOHLC.csv{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), threadname);

            string filenamepath = string.Format(@"C:\Algotrade\scalperoutput\LiveFUT-{0}.csv", filename);
            string content1 = string.Format("Datetime,StockName,BidPrice,Offerprice,buyprice,lotsize,Squareoffprice,Stoploss,Profit/Loss,percentageDiff\r\n");
            File.AppendAllText(filenamepath, content1);

            string filename1path = string.Format(@"C:\Algotrade\scalperoutput\opt\TradeFUT-{0}.csv", filename);
            
            File.AppendAllText(filename1path, content1);

            TimeSpan end = new TimeSpan(15, 31, 0);
            TimeSpan sqofftime = new TimeSpan(14, 45, 0);

            string instrumentId1 = "";
            int bidofferindex = 0;
            
            bool bfirst = true;
            string strike1;

            long lstrike1 = GetatthemoneyStrike(long.Parse(Math.Round(triggerprice,0).ToString()));
            Dictionary<string, string> dictparams = GetStockCallStrikePrice(lstrike1.ToString(), broker, stockname, expiryDate);

            strike1 = dictparams["strike1"];
            Dictionary<string, dynamic> dictinstruments = new Dictionary<string, dynamic>
            {
                { "strikeprice1",strike1 },
                {"expiryDate",expiryDate }
            };

            IIFLConnect iiflconnect = new IIFLConnect();
            ////////////////////////////////////////////
            
            Dictionary<string, dynamic> dictParams = GetInstrumentIDLotsize(ref dictinstruments);

            instrumentId1 = Convert.ToString(dictParams["strikeprice1"]);
            int lotsize = Convert.ToInt32(dictinstruments["lotsize"]);
            decimal buyprice = 0.0m;
            

            try
            {
                while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
                {
                    Dictionary<string, dynamic> quotes = GetQuote(instrumentId1);
                    KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                    dynamic lstoffersfut = kvpquote.Value.Offers;
                    dynamic lstbidsfut = kvpquote.Value.Bids;

                    if (bfirst)
                    {
                        buyprice = lstoffersfut[bidofferindex].Price;
                        //bfirst = false;
                    }

                    decimal squareoffprice = Math.Round(buyprice + (3000 * 1.0m / lotsize * 1.0m), 2); //target 3000rs                 
                                                                                                       //decimal stoplossprice = triggerprice * (1 - percent / 100);
                    decimal stoplossprice = Math.Round(buyprice - (2000 * 1.0m / lotsize * 1.0m), 2); //max stoploss 5000rs
                    decimal currprice = lstbidsfut[bidofferindex].Price;
                    decimal percentdiff = Math.Round(((currprice - buyprice) * 1.0m / buyprice) * 100, 2);
                    //look for target or stoploss
                    Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();

                    int totallots = 1;

                    if (bfirst)
                    {
                        string filecontent = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}%\r\n", DateTime.Now, strike1, lstbidsfut[bidofferindex].Price, lstoffersfut[bidofferindex].Price, buyprice, lotsize, squareoffprice, stoplossprice, percentdiff);
                        File.AppendAllText(filename1path, filecontent);
                        Console.WriteLine(filecontent);
                        bfirst = false;
                    }
                    if (lstbidsfut[bidofferindex].Price >= squareoffprice)
                    {
                        //target achieved, squreoff order
                        //response2 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_SELL,
                        //lstbidsfut[bidofferindex].Price, (lotsize * totallots), Constants.PRODUCT_MIS);
                        //target achieved
                        decimal profit = (lstbidsfut[bidofferindex].Price - buyprice) * lotsize;
                        string filecontent = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}%\r\n", DateTime.Now, strike1, lstbidsfut[bidofferindex].Price, lstoffersfut[bidofferindex].Price, buyprice, lotsize, squareoffprice, stoplossprice, profit, percentdiff);
                        File.AppendAllText(filename1path, filecontent);
                        Console.WriteLine(filecontent);
                        break;

                    }
                    else if (lstbidsfut[bidofferindex].Price < stoplossprice)
                    {
                        //stoploss
                        decimal loss = (lstbidsfut[bidofferindex].Price - buyprice) * lotsize;
                        string filecontent = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}%\r\n", DateTime.Now, strike1, lstbidsfut[bidofferindex].Price, lstoffersfut[bidofferindex].Price, buyprice, lotsize, squareoffprice, stoplossprice, loss, percentdiff);

                        File.AppendAllText(filename1path, filecontent);
                        Console.WriteLine(filecontent);
                        break;

                    }
                    else if (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) >= sqofftime)
                    {
                        //square off everything at 2:45PM
                        decimal profitorloss = (lstbidsfut[bidofferindex].Price - buyprice) * lotsize;
                        string filecontent = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}%\r\n", DateTime.Now, strike1, lstbidsfut[bidofferindex].Price, lstoffersfut[bidofferindex].Price, buyprice, lotsize, squareoffprice, stoplossprice, profitorloss, percentdiff);
                        File.AppendAllText(filename1path, filecontent);
                        Console.WriteLine(filecontent);
                        break;
                    }
                    else
                    {


                        string filecontent = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}%\r\n", DateTime.Now, strike1, lstbidsfut[bidofferindex].Price, lstoffersfut[bidofferindex].Price, buyprice, lotsize, squareoffprice, stoplossprice, percentdiff);
                        //File.AppendAllText(filenamepath, filecontent);
                        Console.WriteLine(filecontent);

                    }

                    //response2 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_SELL,
                    //lstbids2[0].Price, (lotsize * totallots), "", Constants.PRODUCT_MIS);

                    Thread.Sleep(1000);
                    //instrumentData = iiflconnect.GetInstrumentID("marketdata.futsymbol", dictParams);


                }//end of while
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                string path = string.Format(@"C:\Algotrade\webhook\Trigger\ChartinkTrigger{0}", DateTime.Now.ToString("dd-MM-yyyy"));
                File.AppendAllText(path, ex.StackTrace);
            }

            
            
        }//end of BuySupertrendStrategyTrigger


       

        public static void BankNifty5minsScalperStrategy()
        {
            try
            {
                string broker = OptionsData._broker;
                //TrriggerObject triggerobj = (TrriggerObject)stobj;
                string expiryDate = "24Jun2021";
                string filename = string.Format("BN_5minScalperStrategy_{0}", DateTime.Now.ToString("dd-MM-yyyy"));
                //string filename1= string.Format("TradedataOHLC.csv{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), threadname);

                string filenamepath = string.Format(@"C:\Algotrade\scalperoutput\Live-{0}.csv", filename);
                string content1 = string.Format("Datetime,Askprice,Bidprice,lots,buy/sellprice,targetprice,stoploss,percentDiff\r\n");
                File.AppendAllText(filenamepath, content1);

                string filename1path = string.Format(@"C:\Algotrade\scalperoutput\TradeBNScalper-{0}.csv", filename);
                File.AppendAllText(filename1path, content1);

                TimeSpan starttime = new TimeSpan(9, 55, 0);
                TimeSpan end = new TimeSpan(15, 31, 0);
                TimeSpan first5min = new TimeSpan(10, 00, 0);
                TimeSpan next5min = new TimeSpan(10, 05, 0);
                TimeSpan stoptillalert = new TimeSpan(14, 0, 0);
                TimeSpan sqofftime = new TimeSpan(15, 15, 0);
                //TimeSpan nine15 = new TimeSpan(9, 15, 0);

                string instrumentId1 = "";
                int bidofferindex = 0;
                decimal percent = 0.5m;
                bool bfirst = true;

                Dictionary<string, dynamic> dictinstruments = new Dictionary<string, dynamic>
               {
                 { "strikeprice1","BANKNIFTY" },
                 {"expiryDate",expiryDate }
               };

                //get the high of the index and wait till any candle breaks the high


                IIFLConnect iiflconnect = new IIFLConnect();
                ////
                Dictionary<string, dynamic> dictParams = GetInstrumentIDLotsizeFUT(ref dictinstruments, expiryDate);
                instrumentId1 = Convert.ToString(dictinstruments["strikeprice1"]);
                int lotsize = Convert.ToInt32(dictParams["lotsize"]);
                decimal buyprice = 1.0m;

                //mark the high of the alert candle



                //get the high and low of the first 5 mins candle
                decimal lastprice = 0.0m;
                decimal first5minhigh = 0.0m;
                decimal first5minlow = 0.0m;

                decimal next5minhigh = 0.0m;
                decimal next5minlow = 0.0m;
                bool first = true;

                while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) <= starttime)
                {
                    //start the strategy at 9:15AM
                    if (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) >= starttime)
                    {
                        break;
                    }
                }

                while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) <= first5min)
                {
                    try
                    {
                        Dictionary<string, dynamic> quotes = GetQuote(instrumentId1);
                        KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                        lastprice = kvpquote.Value.LastPrice;
                        if (first)
                        {
                            first5minhigh = lastprice;
                            first5minlow = lastprice;
                            first = false;

                        }
                        if (lastprice > first5minhigh)
                        {
                            first5minhigh = lastprice;
                        }
                        else if (lastprice < first5minlow)
                        {
                            first5minlow = lastprice;
                        }
                        Console.WriteLine(string.Format("{0}Current Candle High{1}, Current Candle Low{2}", DateTime.Now.ToString("HH:mm:ss"),
                            first5minhigh, first5minlow));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }//end of while
               


                Console.WriteLine(string.Format("first Candle High{0}, First Candle Low{1}", first5minhigh, first5minlow));
                //look for the alert candle that breaks the low of the first 5 mins candle
                bool lowbroken = true;
                int lowbrokenmin = 0;
                int minutetorunmore;
                lastprice = 0.0m;
                first = true;

                while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) > first5min
                    && new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) <= stoptillalert)
                {
                    try
                    {
                        Dictionary<string, dynamic> quotes = GetQuote(instrumentId1);
                        KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                        lastprice = kvpquote.Value.LastPrice;

                        if (first)
                        {
                            next5minhigh = first5minhigh;
                            next5minlow = first5minlow;
                            first = false;

                        }

                        if (lastprice > next5minhigh)
                        {
                            next5minhigh = lastprice;

                        }
                        else if (lastprice < first5minlow)
                        {
                            next5minlow = lastprice;
                        }

                        if (next5minlow < first5minlow
                            && lowbroken == false)
                        {
                            lowbroken = true;
                            lowbrokenmin = DateTime.Now.Minute;
                            minutetorunmore = 5 - (lowbrokenmin % 5);
                            DateTime newruntime = DateTime.Now.AddMinutes(minutetorunmore);
                            stoptillalert = new TimeSpan(newruntime.Hour, newruntime.Minute, newruntime.Second);
                        }//end of if
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                if (lowbroken)
                {
                    Console.WriteLine(string.Format("Alert Candle High{0}, AlertCandle Low{1}", next5minhigh, next5minlow));
                }



                //write the code for taking a trade at alert candle high buy

                //take position after next5minhigh
                decimal targetprice = 0.0m;
                decimal stoplossprice = 0.0m;
                bool takepos = false;
                while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
                {
                    try {
                        Dictionary<string, dynamic> quotes = GetQuote(instrumentId1);
                        KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                        lastprice = kvpquote.Value.LastPrice;
                        dynamic lstoffersfut = kvpquote.Value.Offers;
                        dynamic lstbidsfut = kvpquote.Value.Bids;
                        decimal bidprice = lstbidsfut[bidofferindex].Price;
                        decimal askprice = lstoffersfut[bidofferindex].Price;

                        decimal percentdiff = Math.Round(((bidprice - buyprice) * 1.0m / buyprice) * 100.0m, 2);

                        if (lastprice > next5minhigh && takepos == false)
                        {
                            Console.WriteLine("Position Taken");
                            buyprice = lstoffersfut[bidofferindex].Price;
                            targetprice = buyprice + 50.0m;
                            stoplossprice = buyprice - 30.0m;
                            //take position
                            string filecontent = string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", askprice, bidprice, lotsize, buyprice, targetprice, stoplossprice, percentdiff);
                            File.AppendAllText(filenamepath, filecontent);
                            takepos = true;

                        }


                        string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", askprice, bidprice, lotsize, bidprice, targetprice, stoplossprice, percentdiff);
                        Console.WriteLine(filecontent1);

                        //File.AppendAllText(filename1path, filecontent);
                        if (takepos == true)
                        {
                            if (bidprice >= targetprice)
                            {
                                string filecontent = string.Format("{0},{1},{2},{3},{4},{5},{6}-TargetHit\r\n", askprice, bidprice, lotsize, bidprice, targetprice, stoplossprice, percentdiff);
                                File.AppendAllText(filename1path, filecontent);
                                break;

                            }
                            else if (bidprice < stoplossprice)
                            {
                                //stoploss hit
                                string filecontent = string.Format("{0},{1},{2},{3},{4},{5},{6}-Stoploss Hit\r\n", askprice, bidprice, lotsize, bidprice, targetprice, stoplossprice, percentdiff);
                                File.AppendAllText(filename1path, filecontent);
                                break;
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    }//end of while

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }//end of BuySupertrendStrategyTrigger

        /// <summary>
        /// In this strategy we will mark the high and low of nifty 1st 5 mins green candle
        /// pick nifty 50 stocks and if the 1st 5 mins candle is green then buy on the breakout of the high
        /// do this for around 10 stocks
        /// 
        /// do not take a trade if the 1st 5mins candle is more than 0.75% 
        /// do reverse for 5 mins red candle
        /// </summary>
        public static void NiftyStocks5minsbreakoutStrategy()
        {
            string instrumentId = "NIFTY";
            Dictionary<string, dynamic> dicthistdata = GetHistoricalDataParameters(5, instrumentId);

        }
        public static void SellSupertrendStrategyTrigger(object stobj)
        {

        }
        static int lastindexparsed = 0;
        public static void ParseWebhookData(object obj)
        {
            string expirydate = "24Jun2021";
            string [] filetriggercontents = (string [])obj;
            //int tharrcount = 0;
            try
            {
                //Parse stocks and prices
                List<string> alertname = filetriggercontents.Where(x => x.Contains("alert_name")).ToList();
                List<string> stocklist = filetriggercontents.Where(x => x.Contains("stocks")).ToList();
                List<string> pricetrigger = filetriggercontents.Where(x => x.Contains("trigger_prices")).ToList();

                string[] triggerstocklist = new string[50];
                string[] triggerpricelist = new string[50];
                int stockcountindex = stocklist.Count();

                List<Thread> lsttharr = new List<Thread>();
                for (int j = lastindexparsed; j < stockcountindex; ++j)
                {
                    int index = stocklist[j].IndexOf("stocks");
                    string triggerdatetime = stocklist[j].Substring(0, index - 2);
                    DateTime triggertime = new DateTime();
                    try
                    {
                         triggertime = DateTime.Parse(triggerdatetime);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        string path = string.Format(@"C:\Algotrade\webhook\Trigger\ChartinkTrigger{0}", DateTime.Now.ToString("dd-MM-yyyy"));
                        File.AppendAllText(path, ex.StackTrace);
                    }
                    TimeSpan ts = new TimeSpan(triggertime.Hour, triggertime.Minute, triggertime.Second);
                    TimeSpan starttime = new TimeSpan(9, 30, 0);
                    if (ts > starttime)
                    {
                        //ignore alerts that came before 9:45AM
                        int stocklen = stocklist[j].Length;
                        string stockarr = stocklist[j].Substring(index + 7);
                        triggerstocklist = stockarr.Split(',');
                        int stockcount = triggerstocklist.Count();

                        //tharr = new Thread[stockcount];
                    }

                    int pricetriggercount = pricetrigger.Count();
                    int priceindex = pricetrigger[j].IndexOf("trigger_prices");
                    string triggerdatetime1 = stocklist[j].Substring(0, priceindex - 2);
                    DateTime triggertime1 = new DateTime();
                    try
                    {
                         triggertime1 = DateTime.Parse(triggerdatetime1);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        string path = string.Format(@"C:\Algotrade\webhook\Trigger\ChartinkTrigger{0}", DateTime.Now.ToString("dd-MM-yyyy"));
                        File.AppendAllText(path, ex.StackTrace);
                    }
                    TimeSpan ts1 = new TimeSpan(triggertime.Hour, triggertime.Minute, triggertime.Second);
                    TimeSpan starttime1 = new TimeSpan(9, 30, 0);
                    if (ts1 > starttime1)
                    {
                        int pricelen = pricetrigger[j].Length;
                        string pricearr = pricetrigger[j].Substring(priceindex + 15);
                        triggerpricelist = pricearr.Split(',');
                        int triggerpricelistcount = triggerpricelist.Count();
                        for (int l = 0; l < triggerpricelistcount; ++l)
                        {
                            TrriggerObject tobj = new TrriggerObject();
                            tobj.Buyorsell = "buy";
                            tobj.Stockname = triggerstocklist[l];
                            tobj.expiryDate = expirydate;
                            tobj.Currprice = Convert.ToDecimal(triggerpricelist[l]);
                            tobj.threadname = "Thread" + j.ToString();

                            //if (alertname[j].ToLower().Contains("buy"))
                            {
                                Thread th = new Thread(new ParameterizedThreadStart(OptionsData.BuySupertrendStrategyTrigger));
                                th.Start(tobj);
                                th.IsBackground = true;
                                lsttharr.Add(th);
                            }
                            //else if(alertname[j].ToLower().Contains("sell"))
                            //{

                            //}              

                           // BuySupertrendStrategyTrigger(tobj);

                        }
                    }                

                }//end of for
                TimeSpan end = new TimeSpan(15, 15, 0);
                lastindexparsed = stocklist.Count();
                Console.WriteLine("Last Index Parsed:" + lastindexparsed);
                //while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
                {
                    for (int j = 0; j < lsttharr.Count; j++)
                    {
                       // lsttharr[j].Join();
                    }
                    Console.WriteLine("Completed Parsewebhookdata");
                    //Console.ReadLine();
                }
            }//try
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                string path = string.Format(@"C:\Algotrade\webhook\Trigger\ChartinkTrigger{0}", DateTime.Now.ToString("dd-MM-yyyy"));
                File.AppendAllText(path, ex.StackTrace);
            }

        }
        public static void BNShortStraddleStrategyLive(object csvobj)
        {
            CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            //decimal currentpivot1 = csvinputobj.Pivot;
            //decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = "NIFTY BANK";
            //string strikeprice2 = csvinputobj.Strike2;
            //create the directory if it doesnt exist
            
            
            //csvinputobj.lotSize = lotsize;           
            //delete existing files

            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("BN-SSTRANG-{0}-{1}_{2}", strikeprice1, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData");
            string filename1path = string.Format(@"C:\Algotrade\scalperoutput\Live{0}.csv", filename1);
            string filecontent = "DateTime,OrderID,Bid/Ask,InstrumentId,Price,OrderType,LineNo,PivotDown,PivotUp,Maxlots\r\n";

            File.AppendAllText(filename1path, filecontent);

            string filename2 = string.Format("BN-SSTRANG-{0}-{1}_{2}", strikeprice1, DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");


            string filename3 = string.Format("BN-SSTRANG-{0}-{1}_{2}", strikeprice1, DateTime.Now.ToString("dd-MM-yyyy"), "Continous");

            string filename3path = string.Format(@"C:\Algotrade\scalperoutput\Live-{0}.csv", filename3);
            string filecontent3 = "Datetime,Bid1,Offer2,Bid2,Offer1,SellDeltaPrice,BuyDeltaPrice," + threadname + "\r\n";
            File.AppendAllText(filename3path, filecontent3);

            string filename2path = string.Format(@"C:\Algotrade\scalperoutput\credit{0}.csv", filename2);
            string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename2path, filecontent2);


            int nooflots = 0;
            int totallots = 3;

            //Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            //{
            //    {"strikeprice1", strikeprice1},
            //    //{"strikeprice2",strikeprice2 }
            //};


            Thread.Sleep(100);
            //Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = 25;
            string instrumentId1 = "";
            string instrumentId2 = "";
            int alive = 0;
            decimal totalbuyprice = 0.0m;
            decimal totalbidprice = 0.0m;
            TimeSpan end = new TimeSpan(15, 31, 0);
            int maxlots = 1;
            string strike1 = "";
            string strike2 = "";
            bool bfirst = true;
            
            //while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            //run till 03:30PM
            while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
            {
                try
                {
                    //Thread.Sleep(1000);

                    //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    //instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);
                    decimal prevdayclose = 22370.0m;

                    Dictionary<string, dynamic> quotes = GetQuoteBank(strikeprice1);
                    KeyValuePair<string, dynamic> kvpquote = quotes.ElementAt(0);
                    dynamic currentliveprice1 = kvpquote.Value.LastPrice;
                    prevdayclose = kvpquote.Value.PrevDayClose;//Previous day close price

                    long currentliveprice = long.Parse(currentliveprice1.ToString());

                    decimal currpricepercentincval = prevdayclose * 1.015m;
                    decimal currpricepercentdecval = prevdayclose * (1.0m - 0.015m);


                    //if (currentliveprice>= currpricepercentincval || currentliveprice<= currpricepercentdecval)
                    {
                        long currentlivepricetens = currentliveprice % 100;
                        long currentlivepricewhole = (currentliveprice / 100) * 100;

                        long currentlivepricewholenext = currentlivepricewhole + 100;


                        long currstrikeprice = 0;
                        //check for which nearest strike to be taken
                        decimal percentfromlowerstrike = ((currentliveprice - currentlivepricewhole) * 1.0m / currentlivepricewhole) * 100.0m;
                        decimal percentfromupperstrike = ((currentlivepricewholenext - currentliveprice) * 1.0m / currentlivepricewholenext) * 100.0m;

                        if (nooflots == 0)
                        {
                            if (percentfromlowerstrike < percentfromupperstrike)
                            {
                                currstrikeprice = currentlivepricewhole;
                            }
                            else
                            {
                                currstrikeprice = currentlivepricewholenext;
                            }
                            currstrikeprice = 23000;

                            strike1 = "BANKNIFTY15Oct2020" + (currstrikeprice ) + "PE";
                            strike2 = "BANKNIFTY15Oct2020" + currstrikeprice + "CE";


                            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
                            {
                                {"strikeprice1", strike1},
                                {"strikeprice2",strike2 }
                            };

                            Dictionary<string, dynamic> instrumentids1 = GetInstrumentIDLotsize(ref dictinstrumentid);
                            lotsize = Convert.ToInt32(instrumentids1["lotsize"]);

                            instrumentId1 = Convert.ToString(instrumentids1["strikeprice1"]);
                            instrumentId2 = Convert.ToString(instrumentids1["strikeprice2"]);
                        }//end of if

                        Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                        Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);
                        //add nfo 
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                        //get bid ask price

                        dynamic lstbids1 = kvpquote1.Value.Bids;
                        dynamic lstoffers1 = kvpquote1.Value.Offers;
                        dynamic lstbids2 = kvpquote2.Value.Bids;
                        dynamic lstoffers2 = kvpquote2.Value.Offers;
                        int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                        int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                        totalbidprice = (lstoffers1[0].Price + lstoffers2[0].Price);

                        
                        decimal percentchange = currentliveprice / prevdayclose;
                        if (DateTime.Now.Hour < 15 && nooflots < maxlots
                              /*&&(percentchange >=1.015m || percentchange <= (1.0m-0.015m))*/)
                        {
                            //buy 1 lot
                            //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                            {
                                //buy 2nd strike at bid price and buy 2nd strike one at ask price
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int i = 0; i < 5; ++i)
                                {
                                    try
                                    {
                                        response1 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_SELL,
                                          lstbids2[0].Price, lotsize);

                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strike1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }
                                Dictionary<string, dynamic> orderstatus = new Dictionary<string, dynamic>();
                                //orderstatus = GetOrderStatus(response1);//not required

                                //Thread.Sleep(100);

                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                       "", strike2, lstoffers2[0].Price, "BUY", totaltradedquantity2, "LineNo1138");
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() != "cancelled"
                                //        && orderstatus["Status"].ToLower() != "rejected")
                                {
                                    //sell 1st strike
                                    //Thread.Sleep(2000);
                                    Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                    for (int j = 0; j < 5; ++j)
                                    {
                                        try
                                        {
                                            response2 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_SELL,
                                                                                                 lstbids1[0].Price, (lotsize*totallots));

                                            string writetofile6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID",
                                                                                "", strike1, lstoffers1[0].Price, "BUY", totaltradedquantity1, "LineNo1154");
                                            Console.WriteLine(writetofile6);
                                            File.AppendAllText(filename1path, writetofile6);
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                            strike1, strike2);
                                            File.AppendAllText(filename2path, content);
                                        }
                                        Thread.Sleep(500);
                                    }

                                    Dictionary<string, dynamic> orderstatus2 = new Dictionary<string, dynamic>();



                                    //if (orderstatus2["Status"].ToLower() != "cancelled"
                                    //  && orderstatus2["Status"].ToLower() != "rejected")
                                    {
                                        nooflots += 1;
                                        totalbuyprice = totalbidprice;

                                        string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, lstoffers1[0].Price, lstoffers2[0].Price,
                                                                       totalbuyprice, nooflots, "BUY", currentliveprice, "LineNo1206");
                                        Console.WriteLine(writetofile);
                                        File.AppendAllText(filename1path, writetofile);
                                        //
                                    }

                                }//end of if

                            }
                        }
                        else if (nooflots > 0 && (totalbidprice <= (totalbuyprice * 0.95m)))
                        {

                            //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= nextpivotdown)
                            {
                                //buy 1 lot
                                //buy 1st strike at ask price and sell 2nd strike at bid price   
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int j1 = 0; j1 < 5; ++j1)
                                {
                                    try
                                    {
                                        response1 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_BUY,
                                                                         lstoffers1[0].Price, (lotsize*totallots));

                                        string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID", "",
                                                                                           strike2, lstbids1[0].Price, totaltradedquantity1, "SELL", "LineNo152");
                                        Console.WriteLine(filecontent5);
                                        File.AppendAllText(filename1path, filecontent5);
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strikeprice1, strike2);
                                        File.AppendAllText(filename2path, content);

                                    }
                                    Thread.Sleep(500);
                                }//else

                                //Thread.Sleep(100);
                                //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);
                                //string filecontent5 = string.Format("{0},{1},{2},{3},{4}-{5}\r\n", DateTime.Now, response1["result"]["AppOrderID"], 
                                //                                                           instrumentId2,lstoffers1[0].Price, "BUY", "LineNo1220");
                                //Console.WriteLine(filecontent5);
                                //File.AppendAllText(filename1path, filecontent5);

                                //sell 2nd strike 
                                Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                for (int j2 = 0; j2 < 5; ++j2)
                                {
                                    try
                                    {
                                        response2 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_BUY,
                                        lstoffers2[0].Price, (lotsize* totallots));
                                        string filecontent6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now, "AppOrderID", "", strike2,
                                                                                    lstoffers2[0].Price, "SELL", totaltradedquantity2, "LineNo1284");
                                        Console.WriteLine(filecontent6);
                                        File.AppendAllText(filename1path, filecontent6);
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strikeprice1, strike2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }

                                //string filecontent6 = string.Format("{0},{1},{2},{3},{4}\n", DateTime.Now, response2["result"]["AppOrderID"],
                                //                                                     lstbids2[0].Price, "SELL", "LineNo1227");
                                //Console.WriteLine(filecontent6);
                                //File.AppendAllText(filename1path, filecontent6);



                                //if (orderstatus["Status"].ToLower() == "complete" && orderstatus6["Status"].ToLower() == "complete")
                                {
                                    //both legs squared off so reduce the no of lots
                                    nooflots -= 1;

                                    decimal percentageincrease = Math.Round(((totalbuyprice-totalbidprice) * 1.0m / totalbuyprice * 1.0m) * 100.0m, 2);

                                    string writetofile = string.Format("{0},{1},{2},{3},{4}%,{5},{6},{7}\n", DateTime.Now, lstoffers1[0].Price,
                                                                     lstoffers2[0].Price, totalbidprice, percentageincrease,
                                                                       "SELL", nooflots, "LineNo1245");
                                    Console.WriteLine(writetofile);
                                    File.AppendAllText(filename1path, writetofile);
                                    //
                                }
                                //else
                                //{
                                //    string writetofile = string.Format("{0},{1},{2},{3},{4},{5}\n", DateTime.Now, lstoffers1[0].Price, lstbids2[0].Price,
                                //                                                  (lstoffers1[0].Price - lstbids2[0].Price), nooflots, "FAILED");
                                //    Console.WriteLine(writetofile);
                                //    File.AppendAllText(filename1path, writetofile);
                                //}
                            }//end of if Math.Abs

                        }
                        else if (nooflots > 0 && DateTime.Now.Hour == 14 && DateTime.Now.Minute >= 45)
                        {
                            //square off everything 

                        }
                        decimal totalperceinc = Math.Round(((totalbuyprice-totalbidprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);
                        string strtotalpercinc = "";
                        if (totalperceinc >= 0)
                        {
                            strtotalpercinc = "+" + totalperceinc.ToString();
                        }
                        else
                        {
                            strtotalpercinc = totalperceinc.ToString();
                        }

                        string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}%\r\n", DateTime.Now, currentliveprice,
                                          strike2, totalbidprice, totalbuyprice, totaltradedquantity1,
                                                  totaltradedquantity2, strtotalpercinc);

                        File.AppendAllText(filename3path, filecontent1);

                        if (alive % 250 == 0)
                        {
                            Console.WriteLine(DateTime.Now + "-I am alive -" + alive + "-" + "," + strikeprice1);
                            string filecontent9 = string.Format("{0},Alive-,{1}\r\n", DateTime.Now, alive);
                            //File.AppendAllText(filename2path, filecontent9);
                            Console.WriteLine(filecontent9);
                        }
                        ++alive;


                    }//end of if block line 1173



                }//end of try
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                           strike1, strike2);
                    File.AppendAllText(filename2path, filecontent1);
                }
            }

        }
        private static object _synclock = new object();//sync object
        public static void BankNiftyStrikeOptionData(object csvobj)
        {
            CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            string indextype = csvinputobj.Index;
            //Kite kite = csvinputobj.kite;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;

            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;         
            //delete existing files

           
            string threadname = Thread.CurrentThread.Name;

            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}",indextype, strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog",
                                                                 threadname);
            string filename2path = string.Format(@"C:\Algotrade\indexOIData\{0}.csv", filename2);
            string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename2path, filecontent2);

            string filename3 = string.Format("{0}-OI_{1}_{2}_{3}_{4}_{5}",indextype, strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "Continous",
                                                                  threadname);
            string filename3path = string.Format(@"C:\Algotrade\indexOIData\{0}.csv", filename3);
            string filecontent3 = "Datetime,BidSum,OfferSum,LivePrice,Quantity1,Quanity2,PEOI,CEOI\r\n";
            File.AppendAllText(filename3path, filecontent3);

           

            int nooflots = 0;

            Dictionary<string, string> dictParamStrikes1 = ConvertStrikePricesfromIIFL(strikeprice1, strikeprice2, "zerodha", indextype);
            string strike1 = dictParamStrikes1["strike1"];
            string strike2 = dictParamStrikes1["strike2"];

            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strike1},
                {"strikeprice2",strike2 }
            };

           
            //
            Thread.Sleep(200);
            //Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            //int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            string instrumentId1 = "";
            string instrumentId2 = "";
         

            TimeSpan end = new TimeSpan(15, 31, 0);

            
            //run till 03:30PM
            while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
            {
                try
                {                   


                    //instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    //instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                    instrumentId1 = Convert.ToString(dictinstrumentid["strikeprice1"]);
                    instrumentId2 = Convert.ToString(dictinstrumentid["strikeprice2"]);

                    string index = (indextype == "BANKNIFTY") ? "NIFTY BANK" : "NIFTY 50";
                    Dictionary<string, dynamic> bnquotes = GetQuoteBank(index);
                    KeyValuePair<string, dynamic> bnkvpquote = bnquotes.ElementAt(0);
                    dynamic currentliveprice = bnkvpquote.Value.LastPrice;

                    Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                    Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);
                    //add nfo 
                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                    //get bid ask price

                    dynamic lstbids1 = kvpquote1.Value.Bids;
                    dynamic lstoffers1 = kvpquote1.Value.Offers;
                    dynamic lstbids2 = kvpquote2.Value.Bids;
                    dynamic lstoffers2 = kvpquote2.Value.Offers;
                    long totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                    long totaltradedquantity2 = kvpquote2.Value.LastQuantity;
                    long OI = kvpquote1.Value.OI;
                    long OI1 = kvpquote2.Value.OI;

                    decimal bidaskdiff1 = (lstbids1[0].Price - lstoffers2[0].Price);
                    decimal bidaskdiff2 = (lstoffers1[0].Price - lstbids2[0].Price);

                    string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now,(lstbids1[0].Price + lstbids2[0].Price),  
                                                                                      (lstoffers1[0].Price + lstoffers2[0].Price), currentliveprice,totaltradedquantity1,totaltradedquantity2, OI,OI1);
                    File.AppendAllText(filename3path, filecontent1);

                    //update uithread


                    Thread.Sleep(30000);
                    //Thread.Sleep(2000);


                }//end of try
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                           strikeprice1, strikeprice2);
                    File.AppendAllText(filename2path, filecontent1);
                }
            }



        }//end of function
        public static void CreditSpreadStrategyLive(object csvobj)
        {
            CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            decimal currentpivot1 = csvinputobj.Pivot;
            decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;

            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;           
            //delete existing files

            int maxlots = csvinputobj.MaxLotSize;
            decimal pivotdelta = csvinputobj.PivotDelta;
            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData",
                                                                 threadname, startpivot);
            string filename1path = string.Format(@"C:\Algotrade\scalperoutput\Live{0}.csv", filename1);
            string filecontent = "DateTime,OrderID,Bid/Ask,InstrumentId,Price,OrderType,LineNo,PivotDown,PivotUp,Maxlots\r\n";

            File.AppendAllText(filename1path, filecontent);

            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog",
                                                                  threadname, startpivot);
            string filename2path = string.Format(@"C:\Algotrade\scalperoutput\credit{0}.csv", filename2);
            string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename2path, filecontent2);

            string filename3 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "Continous",
                                                                  threadname, startpivot);
            string filename3path = string.Format(@"C:\Algotrade\scalperoutput\Live-{0}.csv", filename3);
            string filecontent3 = "Datetime,Bid1,Offer2,Bid2,Offer1,SellDeltaPrice,BuyDeltaPrice," + threadname + "\r\n";
            File.AppendAllText(filename3path, filecontent3);

            


            int nooflots = 0;

            decimal nextpivotup = startpivot;
            decimal nextpivotdown = startpivot;
            decimal currentpivot = startpivot;
            decimal currentpivot2 = startpivot;

            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1},
                {"strikeprice2",strikeprice2 }
            };

            //stockdeveloper test
            //Dictionary<string, dynamic> response0 = PlaceOrderOption("BANKNIFTY_29-Oct-2020_CE_21500", Constants.TRANSACTION_TYPE_BUY,
                                           // 110, 25);
            
            //
            Thread.Sleep(100);
            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            string instrumentId1 = "";
            string instrumentId2 = "";
            int alive = 0;

            TimeSpan end = new TimeSpan(15, 31, 0);

            //while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            //run till 03:30PM
            while (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
            {
                try
                {
                    Thread.Sleep(1000);

                    instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                    lock (_synclock)
                    {
                        Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                        Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);
                        //add nfo 
                        KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                        KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                        //get bid ask price

                        dynamic lstbids1 = kvpquote1.Value.Bids;
                        dynamic lstoffers1 = kvpquote1.Value.Offers;
                        dynamic lstbids2 = kvpquote2.Value.Bids;
                        dynamic lstoffers2 = kvpquote2.Value.Offers;
                        int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                        int totaltradedquantity2 = kvpquote2.Value.LastQuantity;


                        decimal bidaskdiff1 = (lstbids1[0].Price - lstoffers2[0].Price);
                        decimal bidaskdiff2 = (lstoffers1[0].Price - lstbids2[0].Price);


                        if ((nooflots == 0 && bidaskdiff1 >= startpivot) ||
                            (bidaskdiff1 >= nextpivotup && nextpivotup >= startpivot 
                            && Math.Abs(nooflots) < maxlots))
                        {
                            //buy 1 lot

                            //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                            {
                                //buy 2nd strike at bid price and buy 2nd strike one at ask price
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int i = 0; i < 5; ++i)
                                {
                                    try
                                    {
                                        response1 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_BUY,
                                            lstoffers2[0].Price, lotsize);

                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strikeprice1, strikeprice2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    //Thread.Sleep(500);
                                }
                                Dictionary<string, dynamic> orderstatus = new Dictionary<string, dynamic>();
                                //orderstatus = GetOrderStatus(response1);//not required

                                //Thread.Sleep(100);

                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}\r\n", DateTime.Now, response1["result"]["AppOrderID"],
                                                                                       "", strikeprice2, lstoffers2[0].Price, "BUY", totaltradedquantity2, "LineNo1138", nextpivotdown, nextpivotup, maxlots);
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() != "cancelled"
                                //        && orderstatus["Status"].ToLower() != "rejected")
                                {
                                    //sell 1st strike
                                    //Thread.Sleep(2000);
                                    Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                    for (int j = 0; j < 5; ++j)
                                    {
                                        try
                                        {
                                            response2 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_SELL,
                                                                                                   lstbids1[0].Price, lotsize);

                                            string writetofile6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}\r\n", DateTime.Now, response2["result"]["AppOrderID"],
                                                                                "", strikeprice1, lstbids1[0].Price, "SELL", totaltradedquantity1, "LineNo1154", nextpivotdown, nextpivotup, maxlots);
                                            Console.WriteLine(writetofile6);
                                            File.AppendAllText(filename1path, writetofile6);
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                            strikeprice1, strikeprice2);
                                            File.AppendAllText(filename2path, content);
                                        }
                                        Thread.Sleep(500);
                                    }

                                    Dictionary<string, dynamic> orderstatus2 = new Dictionary<string, dynamic>();

                                    //for (int k = 0; k < 5; k++)
                                    //{
                                    //    try
                                    //    {

                                    //        //orderstatus2 = GetOrderStatus(response2);
                                    //        break;
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        Console.WriteLine(ex.Message);
                                    //        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                    //                                        strikeprice1, strikeprice2);
                                    //        File.AppendAllText(filename2path, content);
                                    //    }
                                    //    //Thread.Sleep(500);
                                    //}


                                    //string writetofile2 = string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", DateTime.Now, response2["result"]["AppOrderID"],
                                    //                                          "", strikeprice1, lstbids1[0].Price, "SELL", "LineNo1191");
                                    //Console.WriteLine(writetofile2);
                                    //File.AppendAllText(filename1path, writetofile2);

                                    //if (orderstatus2["Status"].ToLower() != "cancelled"
                                    //  && orderstatus2["Status"].ToLower() != "rejected")
                                    {
                                        nooflots -= 1;
                                        currentpivot = nextpivotup;
                                        nextpivotup = currentpivot + pivotdelta;
                                        nextpivotdown = currentpivot - pivotdelta;
                                        currentpivot2 = currentpivot;

                                        string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", DateTime.Now, lstbids1[0].Price, lstoffers2[0].Price,
                                                                       (lstbids1[0].Price - lstoffers2[0].Price), nooflots, "SELL", "LineNo1206",
                                                                       nextpivotdown, nextpivotup, maxlots);
                                        Console.WriteLine(writetofile);
                                        File.AppendAllText(filename1path, writetofile);
                                        //
                                    }
                                    //else
                                    {
                                        //Dictionary<string, dynamic> quotes11 = GetQuote(instrumentId2);
                                        ////add nfo
                                        //KeyValuePair<string, dynamic> kvpquote11 = quotes11.ElementAt(0);

                                        ////get bid ask price
                                        //dynamic lstbids11 = kvpquote11.Value.Bids;
                                        //dynamic lstoffers11 = kvpquote11.Value.Offers;
                                        ////reverse the first order if the second leg fails to execute
                                        //Dictionary<string, dynamic> response4 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_SELL,
                                        //                                            lstbids11[0].Price, lotsize);

                                        ////Thread.Sleep(200);

                                        ////Dictionary<string, dynamic> orderstatus4 = GetOrderStatus(response4);
                                        //string filecontent4 = string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", DateTime.Now, response4["result"]["AppOrderID"], "", 
                                        //                                                                 strikeprice2,lstoffers11[0].Price, "REVERSE", "SELL");
                                        //Console.WriteLine(filecontent4);
                                        //File.AppendAllText(filename1path, filecontent4);
                                    }

                                }//end of if

                            }
                        }
                        else if (bidaskdiff2 <= nextpivotdown && nooflots < 0)
                        {

                            //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= nextpivotdown)
                            {
                                //buy 1 lot
                                //buy 1st strike at ask price and sell 2nd strike at bid price   
                                Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();
                                for (int j1 = 0; j1 < 5; ++j1)
                                {
                                    try
                                    {
                                        response1 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_BUY,
                                                                           lstoffers1[0].Price, lotsize);

                                        string filecontent5 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}\r\n", DateTime.Now, response1["result"]["AppOrderID"], "",
                                                                                           strikeprice2, lstoffers1[0].Price,totaltradedquantity1, "BUY", "LineNo152", nextpivotdown, nextpivotup, maxlots);
                                        Console.WriteLine(filecontent5);
                                        File.AppendAllText(filename1path, filecontent5);
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strikeprice1, strikeprice2);
                                        File.AppendAllText(filename2path, content);

                                    }
                                    Thread.Sleep(500);
                                }//else

                                //Thread.Sleep(100);
                                //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);
                                //string filecontent5 = string.Format("{0},{1},{2},{3},{4}-{5}\r\n", DateTime.Now, response1["result"]["AppOrderID"], 
                                //                                                           instrumentId2,lstoffers1[0].Price, "BUY", "LineNo1220");
                                //Console.WriteLine(filecontent5);
                                //File.AppendAllText(filename1path, filecontent5);

                                //sell 2nd strike 
                                Dictionary<string, dynamic> response2 = new Dictionary<string, dynamic>();
                                for (int j2 = 0; j2 < 5; ++j2)
                                {
                                    try
                                    {
                                        response2 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_SELL,
                                                                                                    lstbids2[0].Price, lotsize);
                                        string filecontent6 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}\r\n", DateTime.Now, response2["result"]["AppOrderID"], "", strikeprice2,
                                                                                    lstbids2[0].Price, "SELL", totaltradedquantity2,"LineNo1284", nextpivotdown, nextpivotup, maxlots);
                                        Console.WriteLine(filecontent6);
                                        File.AppendAllText(filename1path, filecontent6);
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                        strikeprice1, strikeprice2);
                                        File.AppendAllText(filename2path, content);
                                    }
                                    Thread.Sleep(500);
                                }

                                //string filecontent6 = string.Format("{0},{1},{2},{3},{4}\n", DateTime.Now, response2["result"]["AppOrderID"],
                                //                                                     lstbids2[0].Price, "SELL", "LineNo1227");
                                //Console.WriteLine(filecontent6);
                                //File.AppendAllText(filename1path, filecontent6);



                                //if (orderstatus["Status"].ToLower() == "complete" && orderstatus6["Status"].ToLower() == "complete")
                                {
                                    //both legs squared off so reduce the no of lots
                                    nooflots += 1;
                                    currentpivot = nextpivotdown;
                                    nextpivotup = currentpivot + pivotdelta;
                                    nextpivotdown = currentpivot - pivotdelta;
                                    currentpivot2 = currentpivot;
                                    startpivot = currentpivot1;


                                    string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n", DateTime.Now, lstoffers1[0].Price,
                                                                     lstbids2[0].Price, (lstoffers1[0].Price - lstbids2[0].Price), nooflots,
                                                                       "BUY", "LineNo1245", nextpivotdown, nextpivotup, maxlots);
                                    Console.WriteLine(writetofile);
                                    File.AppendAllText(filename1path, writetofile);
                                    //
                                }
                                //else
                                //{
                                //    string writetofile = string.Format("{0},{1},{2},{3},{4},{5}\n", DateTime.Now, lstoffers1[0].Price, lstbids2[0].Price,
                                //                                                  (lstoffers1[0].Price - lstbids2[0].Price), nooflots, "FAILED");
                                //    Console.WriteLine(writetofile);
                                //    File.AppendAllText(filename1path, writetofile);
                                //}
                            }//end of if Math.Abs

                        }

                        string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", DateTime.Now, lstbids1[0].Price, lstoffers2[0].Price, lstbids2[0].Price, lstoffers1[0].Price, (lstbids1[0].Price - lstoffers2[0].Price),
                                                                  (lstoffers1[0].Price - lstbids2[0].Price));
                        File.AppendAllText(filename3path, filecontent1);

                        if (alive % 250 == 0)
                        {
                            Console.WriteLine(DateTime.Now + "-I am alive -" + alive + "-" + threadname + "," + strikeprice1);
                            string filecontent9 = string.Format("{0},Alive-,{1}\r\n", DateTime.Now, alive);
                            File.AppendAllText(filename2path, filecontent9);
                        }
                        ++alive;
                    }//end of lock block

                }//end of try
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                                           strikeprice1, strikeprice2);
                    File.AppendAllText(filename2path, filecontent1);
                }
            }         
           


        }//end of function
        public static void CreditSpreadStrategyLive_Old(object csvobj) 
        {
            CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            Kite kite = csvinputobj.kite;
            decimal currentpivot1 = csvinputobj.Pivot;
            decimal startpivot = csvinputobj.Pivot;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
          
            //csvinputobj.lotSize = lotsize;           
            
            int maxlots = csvinputobj.MaxLotSize;
            decimal pivotdelta = csvinputobj.PivotDelta;
            string threadname = Thread.CurrentThread.Name;
            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", 
                                                                 threadname, startpivot);
            string filename1path = string.Format(@"C:\Algotrade\scalperoutput\Live{0}.csv", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData", 
                                                                  threadname, startpivot);
            string filename2path = string.Format(@"C:\Algotrade\scalperoutput\credit{0}.csv", filename2);
            string filecontent = "DateTime,OrderID,InstrumentID,Bid1,Ask2,Difference,NoofLots,OrderType\n";
            File.AppendAllText(filename1path, filecontent);
            
            int nooflots = -1;
            //decimal maxuppivotrange = currentpivot + (pivotdelta * iterations);
            //decimal maxdownpivotrange = currentpivot - (pivotdelta * iterations);
            decimal nextpivotup = startpivot;
            decimal nextpivotdown = startpivot;
            decimal currentpivot = startpivot;
            decimal currentpivot2 = startpivot;
        
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1},
                {"strikeprice2",strikeprice2 }
            };

            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            string instrumentId1 = "";
            string instrumentId2 = "";

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                try
                {
                    Thread.Sleep(1000);
                    //Dictionary<string, Quote> quotes1 = kite.GetQuote(InstrumentId: new string[] { "NFO:" + strikeprice1  });
                    //Dictionary<string, Quote> quotes2 = kite.GetQuote(InstrumentId: new string[] { "NFO:" + strikeprice2 });
                    instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);
                    Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                    Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);
                    //add nfo
                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);
                    
                    //get bid ask price
                                  
                    dynamic lstbids1 = kvpquote1.Value.Bids;
                    dynamic lstoffers1 = kvpquote1.Value.Offers;
                    dynamic lstbids2 = kvpquote2.Value.Bids;
                    dynamic lstoffers2 = kvpquote2.Value.Offers;
                    int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                    int totaltradedquantity2 = kvpquote2.Value.LastQuantity;



                    decimal bidaskdiff1 = Math.Abs(lstbids1[0].Price - lstoffers2[0].Price);
                    decimal bidaskdiff2 = Math.Abs(lstoffers1[0].Price - lstbids2[0].Price);

                    if ((nooflots == 0 && bidaskdiff1 >= startpivot) ||
                        (bidaskdiff1 >= nextpivotup && nextpivotup >= startpivot && Math.Abs(nooflots) < maxlots))
                    {
                        //buy 1 lot

                        //if (Math.Abs(lstbids1[0].Price - lstoffers2[0].Price) >= nextpivotup)
                        {
                            //buy 2nd strike at bid price and buy 2nd strike one at ask price

                           
                            Dictionary<string, dynamic> response1 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_BUY, 
                                                                                lstoffers2[0].Price, lotsize);

                           
                            //Thread.Sleep(100);
                            Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);
                            string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6}\n", DateTime.Now, orderstatus["OrderId"], instrumentId2,
                                                              orderstatus["Status"], lstoffers2[0].Price, "BUY","LineNo1103");

                            File.AppendAllText(filename1path, writetofile1);

                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                //sell 1st strike
                                Dictionary<string, dynamic> response2 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_SELL,
                                                                                      lstbids1[0].Price, lotsize);

                     
                               Thread.Sleep(5000);
                                Dictionary<string, dynamic> orderstatus2 = GetOrderStatus(response2);

                                string writetofile2 = string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", DateTime.Now, orderstatus2["OrderId"], instrumentId1,
                                                                            orderstatus2["Status"], lstbids1[0].Price, "SELL", "LineNo1118");

                                File.AppendAllText(filename1path, writetofile2);

                                //if (orderstatus2["Status"].ToLower() != "complete")
                                //{
                                //    //Thread.Sleep(1000);
                                //    //try 10 times with 1 sec difference if the 2nd order is not successfule
                                //    for (int k = 0; k < 10; ++k)
                                //    {
                                //        Dictionary<string, dynamic> quotes11 = GetQuote(instrumentId1);
                                //        //add nfo
                                //        KeyValuePair<string, dynamic> kvpquote11 = quotes11.ElementAt(0);
                                //        //KeyValuePair<string, Quote> kvpnifty = quotes.ElementAt(2);
                                //        //get bid ask price

                                //        dynamic lstbids11 = kvpquote11.Value.Bids;
                                //        dynamic lstoffers11 = kvpquote11.Value.Offers;
                                //        Dictionary<string, dynamic> response3 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_SELL,
                                //                                                             lstbids11[0].Price, lotsize);
                                        

                                //        //Thread.Sleep(200);
                                //        orderstatus2.Clear();
                                //        orderstatus2 = GetOrderStatus(response3);
                                //        string writetofile3 = string.Format("{0},{1},{2},{3},{4}-{5},{6},{7}\n", DateTime.Now, orderstatus2["OrderId"], instrumentId1,
                                //                                                orderstatus2["Status"], lstbids11[0].Price, "SELL", k,"LineNo1144");

                                //        File.AppendAllText(filename1path, writetofile3);                                                                

                                //        if (orderstatus2["Status"].ToLower() == "complete")
                                //        {
                                //            File.AppendAllText(filename1path, "break called LineNo1184\n");
                                //            break;
                                          
                                //        }

                                //        //Thread.Sleep(1000);
                                //    }//for

                                //}
                                //                                

                                if (orderstatus2["Status"].ToLower() != "cancelled" &&
                                    orderstatus2["Status"].ToLower() != "rejected")
                                {
                                    nooflots -= 1;
                                    currentpivot = nextpivotup;
                                    nextpivotup = currentpivot + pivotdelta;
                                    nextpivotdown = currentpivot - pivotdelta;
                                    currentpivot2 = currentpivot;

                                    string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", DateTime.Now, lstbids1[0].Price, lstoffers2[0].Price, 
                                                                   (lstbids1[0].Price - lstoffers2[0].Price), nooflots, "SELL","LineNo1171");
                                    File.AppendAllText(filename1path, writetofile);
                                    //
                                }
                                else
                                {
                                    Dictionary<string, dynamic> quotes11 = GetQuote(instrumentId2);
                                    //add nfo
                                    KeyValuePair<string, dynamic> kvpquote11 = quotes11.ElementAt(0);

                                    //get bid ask price
                                    dynamic lstbids11 = kvpquote11.Value.Bids;
                                    dynamic lstoffers11 = kvpquote11.Value.Offers;
                                    //reverse the first order if the second leg fails to execute
                                    Dictionary<string, dynamic> response4 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_SELL,
                                                                                lstbids11[0].Price, lotsize);

                                    //Thread.Sleep(200);
                                    Dictionary<string, dynamic> orderstatus4 = GetOrderStatus(response4);
                                    string filecontent4 = string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", DateTime.Now, orderstatus4["OrderId"], instrumentId2,
                                                                                orderstatus4["Status"], lstoffers11[0].Price, "REVERSE", "SELL");

                                    File.AppendAllText(filename1path, filecontent4);

                                    if (orderstatus4["Status"].ToLower() != "complete")
                                    {
                                        //loop till it gets commpleted
                                        for (int k = 0; k < 10; ++k)
                                        {
                                            Dictionary<string, dynamic> quotes12 = GetQuote(instrumentId2);
                                            //add nfo
                                            KeyValuePair<string, dynamic> kvpquote12 = quotes11.ElementAt(0);
                                            
                                            dynamic lstbids12 = kvpquote11.Value.Bids;
                                            dynamic lstoffers12 = kvpquote11.Value.Offers;
                                            Dictionary<string, dynamic> response3 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_SELL,
                                                                                                 lstbids12[0].Price, lotsize);
                                            //Thread.Sleep(200);
                                            Dictionary<string, dynamic>  orderstatus5 = GetOrderStatus(response3);
                                            string filecontent5 = string.Format("{0},{1},{2},{3},{4}-{5},{6},{7}\n", DateTime.Now, orderstatus5["OrderId"], instrumentId2,
                                                                                                 orderstatus5["Status"], lstbids12[0].Price, "SELL", k, "LineNo1215");
                                            //Console.WriteLine(filecontent5);
                                            File.AppendAllText(filename1path, filecontent5);                                           

                                            if (orderstatus5["Status"].ToLower() == "complete")
                                            {
                                                File.AppendAllText(filename1path, "break called LineNo1219\n");
                                                break;
                                            }

                                            Thread.Sleep(200);
                                        }//for
                                    }
                                }
                               
                            }//end of if
                            
                        }
                    }
                    else if (bidaskdiff2 <= nextpivotdown && nooflots < 0)
                    {

                        //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= nextpivotdown)
                        {
                            //buy 1 lot
                            //buy 1st strike at ask price and sell 2nd strike at bid price                 

                            Dictionary<string, dynamic> response1 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_BUY, 
                                                                    lstoffers1[0].Price, lotsize);
                           
                            //Thread.Sleep(100);
                            Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);
                            string filecontent5 = string.Format("{0},{1},{2},{3},{4}-{5},{6}\n", DateTime.Now, orderstatus["OrderId"], instrumentId2,
                                                                                                orderstatus["Status"], lstoffers1[0].Price, "BUY", "LineNo1247");
                            File.AppendAllText(filename1path, filecontent5);

                            //Dictionary<string, dynamic> orderstatus5 = new Dictionary<string, dynamic>();
                            if (orderstatus["Status"].Tolower() != "complete")
                            {

                                //try 10 times with 1 sec difference if the 2nd order is not successfule
                                for (int k = 0; k < 10; ++k)
                                {
                                    Dictionary<string, dynamic> quotes11 = GetQuote(instrumentId1);
                                    //add nfo

                                    KeyValuePair<string, dynamic> kvpquote11 = quotes11.ElementAt(0);
                                    //KeyValuePair<string, Quote> kvpnifty = quotes.ElementAt(2);
                                    //get bid ask price

                                    dynamic lstbids11 = kvpquote11.Value.Bids;
                                    dynamic lstoffers11 = kvpquote11.Value.Offers;
                                    Dictionary<string, dynamic> response3 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_BUY,
                                                                                         lstoffers11[0].Price, lotsize);

                                    //Thread.Sleep(200);
                                    orderstatus.Clear();
                                    orderstatus = GetOrderStatus(response3);
                                    string filecontent3 = string.Format("{0},{1},{2},{3},{4}-{5},{6}\n", DateTime.Now, orderstatus["OrderId"],
                                                                            orderstatus["Status"], lstbids11[0].Price, "BUY", k, "LineNo1269");

                                    File.AppendAllText(filename1path, filecontent3);                                   

                                   
                                    if (orderstatus["Status"].ToLower() == "complete")
                                    {
                                        File.AppendAllText(filename1path, "break called Lineno 1276\n");
                                        break;

                                    }

                                    //Thread.Sleep(1000);
                                }//for
                            }//end of if

                            //sell 2nd strike
                            Dictionary<string, dynamic> orderstatus6 = new Dictionary<string, dynamic>();
                            if (orderstatus["Status"].ToLower() == "complete")
                            {
                                Dictionary<string, dynamic> response2 = PlaceOrderOption(instrumentId2, Constants.TRANSACTION_TYPE_SELL,
                                                                                          lstbids2[0].Price, lotsize);
                                
                                //Thread.Sleep(200);
                                 orderstatus6 = GetOrderStatus(response2);

                                if (orderstatus6["Status"].Tolower() != "complete")
                                {

                                    //try 10 times with 1 sec difference if the 2nd order is not successfule
                                    for (int k = 0; k < 10; ++k)
                                    {
                                        Dictionary<string, dynamic> quotes11 = GetQuote(instrumentId1);
                                        //add nfo
                                        KeyValuePair<string, dynamic> kvpquote11 = quotes11.ElementAt(0);
                                        //KeyValuePair<string, Quote> kvpnifty = quotes.ElementAt(2);
                                        //get bid ask price

                                        dynamic lstbids11 = kvpquote11.Value.Bids;
                                        dynamic lstoffers11 = kvpquote11.Value.Offers;
                                        Dictionary<string, dynamic> response3 = PlaceOrderOption(instrumentId1, Constants.TRANSACTION_TYPE_BUY,
                                                                                             lstoffers11[0].Price, lotsize);

                                        Thread.Sleep(200);
                                        orderstatus6.Clear();
                                        orderstatus6 = GetOrderStatus(response3);
                                        string writetofile3 = string.Format("{0},{1},{2},{3},{4}-{5},{6}\n", DateTime.Now, orderstatus6["OrderId"],
                                                                                orderstatus6["Status"], lstoffers11[0].Price, "BUY", k, "LineNo1319");

                                        File.AppendAllText(filename1path, writetofile3);

                                        if (orderstatus["Status"].ToLower() == "complete")
                                        {
                                            File.AppendAllText(filename1path, "break called Lineno 1319\n");
                                            break;

                                        }

                                        Thread.Sleep(200);
                                    }//for
                                }//end of if
                            }//end of if

                            if (orderstatus["Status"].ToLower() == "complete" && orderstatus6["Status"].ToLower() == "complete")
                            {
                                //both legs squared off so reduce the no of lots
                                nooflots += 1;
                                currentpivot = nextpivotdown;
                                nextpivotup = currentpivot + pivotdelta;
                                nextpivotdown = currentpivot - pivotdelta;
                                currentpivot2 = currentpivot;
                                startpivot = currentpivot1;


                                string writetofile = string.Format("{0},{1},{2},{3},{4},{5},{6}\n", DateTime.Now, lstoffers1[0].Price, lstbids2[0].Price, 
                                                                                    (lstoffers1[0].Price - lstbids2[0].Price), nooflots, "BUY","LineNo1340");
                                Console.WriteLine(writetofile);
                                File.AppendAllText(filename1path, writetofile);
                                //
                            }
                            else
                            {
                                string writetofile = string.Format("{0},{1},{2},{3},{4},{5}\n", DateTime.Now, lstoffers1[0].Price, lstbids2[0].Price, 
                                                                              (lstoffers1[0].Price - lstbids2[0].Price), nooflots, "FAILED");
                                Console.WriteLine(writetofile);
                                File.AppendAllText(filename1path, writetofile);
                            }
                        }//end of if Math.Abs

                       
                    }

                    //string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6}\n", DateTime.Now, lstbids1[0].Price, lstoffers2[0].Price, (lstbids1[0].Price - lstoffers2[0].Price), lstoffers1[0].Price, 
                                                         //lstbids2[0].Price,(lstoffers1[0].Price - lstbids2[0].Price));
                  
                    //File.AppendAllText(filename2path, filecontent1);
                }//end of try
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string filecontent1 = string.Format("{0},{1}\n", DateTime.Now,ex.Message);
                    File.AppendAllText(filename2path, filecontent1);
                }
            }

        }//end of function

        //private  static object _sync = new object();
        public static Dictionary<string,dynamic> GetOrderStatus(Dictionary<string, dynamic> dictorders)
        {
            Dictionary<string, dynamic> order = new Dictionary<string, dynamic>();
                

            switch (_broker.ToLower())
            {
                case "iifl":
                    {
                        IIFLConnect iiflConnect = new IIFLConnect();
                        //lock (_sync)
                        {
                            if (_accessTokenInteractive == "")
                            {
                                //IIFLUser user = iiflConnect.GenerateSessionInteractive();
                                //iiflConnect.SetAccessToken(user.AccessToken, "interactive");//set access token for interactive
                                //_accessTokenInteractive = user.AccessToken;
                                //write the token in a file so that any new instance can read the token
                                GetIIFLInteractiveAccessToken();
                                iiflConnect.SetAccessToken(_accessTokenInteractive);
                                //Console.WriteLine("Entered once Line GetOrderStatus() 1806");


                            }
                            else
                            {
                                iiflConnect.SetAccessToken(_accessTokenInteractive);
                            }
                        }
                        string orderid = Convert.ToString(dictorders["result"]["AppOrderID"]);
                        order = iiflConnect.GetOrderStatus(orderid);
                        break;
                    }
                case "zerodha":
                    {
                        dynamic dictresp2 = "";
                        Kite kite = new Kite();
                        bool ret2 = dictorders.TryGetValue("data", out dictresp2);
                        Dictionary<string, object> keyvalues2 = (Dictionary<string, object>)dictresp2;
                        string orderid2 = keyvalues2["order_id"].ToString();
                        List<Order> lstorder2 = kite.GetOrders();
                        Order orderfind2 = lstorder2.Find(f => f.OrderId == orderid2);
                        break;
                    }
            }

            return order;
        }
        /// <summary>
        /// get strike price for a specific broker
        /// </summary>
        public static Dictionary<string,string> GetStrikePrices(string putstrike,string callstrike,string broker,string index="BANKNIFTY",string expiry="29Apr2021")
        {
            //index= BANKNIFTY,NIFTY
            Dictionary<string, string> param = new Dictionary<string, string>();
            string strike1 = "";string strike2 = "";
            switch (broker.ToLower())
            {
                case "zerodhanew":
                case "zerodha":
                    {
                        strike1 = index + "21111" + putstrike + "PE";
                        strike2 = index + "21111" + callstrike + "CE";
                        param.Add("strike1", strike1);
                        param.Add("strike2", strike2);
                        break;
                    }
                case "iifl":
                    {
                        strike1 = index + expiry + putstrike + "PE";
                        strike2 = index + expiry + callstrike + "CE";
                        param.Add("strike1", strike1);
                        param.Add("strike2", strike2);
                        break;
                    }

            }
            return param;


        }

        public static Dictionary<string, string> GetStockCallStrikePrice(string callstrike, string broker, string stock, string expiry)
        {
            //index= BANKNIFTY,NIFTY
            Dictionary<string, string> param = new Dictionary<string, string>();
            string strike1 = ""; 
            switch (broker.ToLower())
            {
                case "zerodhanew":
                case "zerodha":
                    {
                        strike1 = stock + "21111" + callstrike + "CE";
                        param.Add("strike1", strike1);
                        
                        break;
                    }
                case "iifl":
                    {
                        strike1 = stock + expiry + callstrike + "CE";                       
                        param.Add("strike1", strike1);                        
                        break;
                    }

            }
            return param;


        }

        public static Dictionary<string, string> ConvertStrikePricesfromIIFL(string putstrike, string callstrike, string broker,string index="BANKNIFTY")
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            string strike1 = ""; string strike2 = "";
            int indexpos = putstrike.IndexOfAny("0123456789".ToCharArray());
            string putstrikenumber = putstrike.Substring(indexpos + 9, 5);
            int indexpos1 = callstrike.IndexOfAny("0123456789".ToCharArray());
            string callstrikenumber = callstrike.Substring(indexpos1 + 9, 5);
            switch (broker.ToLower())
            {
                //BANKNIFTY29Oct202023900PE
                //get put
               
                case "zerodhanew":
                    { 
                        strike1 = index+"21114" + putstrikenumber + "PE";
                        strike2 = index+ "21114" + callstrikenumber + "CE";
                        param.Add("strike1", strike1);
                        param.Add("strike2", strike2);
                        break;
                    }
                case "zerodha":
                    {
                        strike1 = index + "21121" + putstrikenumber + "PE";
                        strike2 = index + "21121" + callstrikenumber + "CE";
                        param.Add("strike1", strike1);
                        param.Add("strike2", strike2);
                        break;
                    }
                case "finvasia":
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
                

            }
            return param;


        }
        public static Dictionary<string, dynamic> PlaceOrderOption(string strikeprice,
                                             string transactiontype, decimal price, int lotsize,
                                              string stocksdeveloperaccountId = "",
                                             string product = Constants.PRODUCT_NRML,
                                             string validity = Constants.VALIDITY_DAY)
        {
            Dictionary<string, dynamic> response1 = new Dictionary<string, dynamic>();

            switch (_broker.ToLower())
            {

                case "zerodha":
                    {
                        Kite kite = new Kite();
                        response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                                              TradingSymbol: strikeprice,
                                              TransactionType: transactiontype,
                                               Quantity: lotsize,
                                               Price: price,
                                               Validity: validity,
                                               OrderType: Constants.ORDER_TYPE_LIMIT,
                                               ProductType: product);
                        break;
                    }
                case "zerodhanew":
                    {
                        Kite kite = new Kite();

                        if (_zerodhanewAccessToken == "")
                        {
                            GetZerodhaNewInteractiveAccessToken();
                            kite.SetAccessToken(_zerodhanewAccessToken);
                        }
                        else
                        {
                            kite.SetAccessToken(_zerodhanewAccessToken);
                        }

                        response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                                              TradingSymbol: strikeprice,
                                              TransactionType: transactiontype,
                                               Quantity: lotsize,
                                               Price: price,
                                               Validity: validity,
                                               OrderType: Constants.ORDER_TYPE_MARKET,
                                               Variety: Constants.VARIETY_REGULAR,
                                               ProductType: product,
                                               UserId: zerodhaid);
                        break;
                    }
                case "iifl":
                    {
                        IIFLConnect iiflConnect = new IIFLConnect();
                        //lock (_sync)
                        {
                            if (_accessTokenInteractive == "")
                            {
                                //IIFLUser user = iiflConnect.GenerateSessionInteractive();
                                //iiflConnect.SetAccessToken(user.AccessToken, "interactive");//set access token for interactive
                                //_accessTokenInteractive = user.AccessToken;
                                GetIIFLInteractiveAccessToken();
                                iiflConnect.SetAccessToken(_accessTokenInteractive);
                                //Console.WriteLine("Entered once Line PlaceOrderOption() 1866");
                            }
                            else
                            {
                                iiflConnect.SetAccessToken(_accessTokenInteractive);
                            }
                        }

                        response1 = iiflConnect.PlaceOrder(Exchange: "NSEFO",
                                                 ExchangeInstrumentId: strikeprice,
                                                 TransactionType: transactiontype,
                                                  Quantity: lotsize,
                                                  Price: 0,
                                                  Validity: validity,
                                                  OrderType: IIFLConstants.ORDER_TYPE_MARKET,
                                                  ProductType: product,
                                                  DisclosedQuantity: 0,
                                                  StoplossValue: 0);
                        break;
                    }
                case "stocksdeveloperfinvasia":
                    {
                        IAutoTrader autoTrader = AutoTrader.CreateInstance("32a3b8a7-b5e7-42d5-bd36-e4bacc5f0c80", "https://stocksdeveloper.in:9017");
                        IOperationResponse<string> r5 = autoTrader.PlaceRegularOrder("FA26840", "NSE", strikeprice,
                                                     (transactiontype=="BUY"?TradeType.BUY:TradeType.SELL), OrderType.LIMIT,
                                                     ProductType.NORMAL, lotsize,
                                                     float.Parse(price.ToString()), 0);
                        //Console.WriteLine("Message: {0}", r5.Message);
                        //Console.WriteLine("Result: {0}", r5.Result);
                        response1.Add("AppOrderID", r5.CommandId);
                        response1.Add("Status", r5.Message);
                        response1.Add("Error", r5.Error);
                        break;
                    }
                case "stocksdeveloperzerodha":
                    {
                        IAutoTrader autoTrader = AutoTrader.CreateInstance("42e541fd-a895-4d4a-aaa8-64351497ca63", "https://stocksdeveloper.in:9017");
                        //IOperationResponse<string> r5 = autoTrader.PlaceAdvancedOrder(variety: Variety.REGULAR, pseudoAccount: "XV5542", 
                        //                      exchange: "NSE", symbol: strikeprice,
                        //                             tradeType: TradeType.BUY, orderType: OrderType.LIMIT,
                        //                             productType: ProductType.INTRADAY, quantity: lotsize,
                        //                             price: float.Parse(price.ToString()), IsAmo: true);

                        IOperationResponse<string> r6 = autoTrader.PlaceRegularOrder("XV5542", "NSE", strikeprice,
                                                     (transactiontype == "BUY" ? TradeType.BUY : TradeType.SELL), OrderType.LIMIT,
                                                     ProductType.NORMAL, lotsize,
                                                     float.Parse(price.ToString()), 0);

                        response1.Add("AppOrderID", r6.CommandId);
                        response1.Add("Status", r6.Message);
                        response1.Add("Error", r6.Error);

                        break;
                    }
                default:
                    {
                        Console.WriteLine("Inside default");
                        break;
                    }
            }

            return response1;

        }

        
        public static Dictionary<string,dynamic>GetQuote(string instrumentId)
        {
            Dictionary<string, dynamic> dictQuote = new Dictionary<string, dynamic>();


            switch (_broker.ToLower())
            {
                case "zerodha":
                    {
                        if (kite == null)
                        {
                            kite = new Kite("67uleb0ttdjmdp6v", Debug: false);                            
                            kite.Login();                     
                            Dictionary<string, Quote> kitequote = kite.GetQuote(InstrumentId: new string[] { "NFO:" + instrumentId });
                            foreach (KeyValuePair<string, Quote> kvp in kitequote)
                            {
                                dictQuote.Add(kvp.Key, kvp.Value);
                            }
                        }
                        else
                        {
                            Dictionary<string, Quote> kitequote = kite.GetQuote(InstrumentId: new string[] { "NFO:" + instrumentId });
                            foreach (KeyValuePair<string, Quote> kvp in kitequote)
                            {
                                dictQuote.Add(kvp.Key, kvp.Value);
                            }
                        }
                        break;
                    }
                case "iifl":
                case "zerodhanew":           
                    {
                        IIFLConnect iiflConnect = new IIFLConnect();
                        //lock (_sync)
                        {
                            if (_accessTokenMarket == "")
                            {

                                //IIFLUser user = iiflConnect.GenerateSessionMarket();
                                //iiflConnect.SetAccessToken(user.AccessToken, "marketdata");
                                //_accessTokenMarket = user.AccessToken;
                                GetIIFLMarketAccessToken();
                                iiflConnect.SetAccessToken(_accessTokenMarket);
                                //Console.WriteLine("Entered once GetQuote(), Line 1923");

                            }
                            else
                            {
                                iiflConnect.SetAccessToken(_accessTokenMarket);
                            }
                        }
                        dictQuote = iiflConnect.GetQuote(instrumentId);
                        break;
                    }
              
            }

            return dictQuote;
        }

        static int histindex = 0;

        public static Dictionary<string, dynamic> GetHistoricalData(string instrumentId)
        {

            Dictionary<string, dynamic> dictParamhist = new Dictionary<string, dynamic>();
            switch (_broker.ToLower())
            {
                case "iifl":
                    {
                        lock (_synclock)
                        {
                            IIFLConnect iiflConnect = new IIFLConnect();

                            if (_accessTokenInteractive == "")
                            {
                                //IIFLUser user = iiflConnect.GenerateSessionInteractive();
                                //iiflConnect.SetAccessToken(user.AccessToken, "interactive");//set access token for interactive
                                //_accessTokenInteractive = user.AccessToken;
                                GetIIFLMarketAccessToken();
                                iiflConnect.SetAccessToken(_accessTokenMarket);
                                //Console.WriteLine("Entered once Line PlaceOrderOption() 1866");
                            }
                            else
                            {
                                iiflConnect.SetAccessToken(_accessTokenInteractive);
                            }

                            //get the start and the end time for the parameter
                            if (histindex == 0)
                            {
                                Dictionary<string, dynamic> Params = GetHistoricalDataParameters(15, instrumentId);                                
                                IIFLHistorical historicalquote = iiflConnect.GetHistoricalQuote("marketdata.historical", Params);
                                CalculateSuperTrendStart(historicalquote);
                            }
                            if(histindex==0)
                            {
                                
                            }



                        }
                        break;
                    }

                 
            }
            return dictParamhist;
        }


        public static Dictionary<string,dynamic>GetHistoricalDataParameters(int minutes,string instrumentId)
        {
            Dictionary<string, dynamic> dictParam = new Dictionary<string, dynamic>();
            //get the time
            DateTime currtime = new DateTime(2021, 4, 30, 15, 30, 0);
            //get starttime 8 * minutes back
            int second = minutes * 60;
             int rem = 0;
            if (currtime.Minute % minutes != 0)
            {
                 rem = currtime.Minute % minutes;
                //remsec = rem * 60;


            }
            DateTime starttime = currtime.AddMinutes(-(minutes * 11 + rem));
            DateTime newstarttime = new DateTime();
            if (starttime.Hour<9)
            {

                TimeSpan timediff = currtime - starttime;
                TimeSpan ts = new TimeSpan(9, 15, 0);
                TimeSpan startspan = new TimeSpan(starttime.Hour, starttime.Minute, starttime.Second);

                TimeSpan tsdiff = ts-startspan ;
                double durationminutes = tsdiff.TotalMinutes;
                double histdatacount = durationminutes / minutes;
                int year = starttime.Year;
                // set tiem time to 3:30PM of previous day
                newstarttime = new DateTime(starttime.Year, starttime.Month, starttime.Day, 15, 30, 0).AddDays(-1).AddMinutes(-durationminutes);
                starttime = newstarttime;



            }
            DateTime endtime = currtime;
            dictParam.Add("exchangeSegment", "NSECM");
            dictParam.Add("exchangeInstrumentID", instrumentId);
            dictParam.Add("startTime", starttime.ToString("MMM dd yyyy HHmm00"));
            dictParam.Add("endTime", endtime.ToString("MMM dd yyyy HHmm00"));  
            dictParam.Add("compressionValue", second.ToString());
            return dictParam;
        }



        public static Dictionary<string, dynamic> GetQuoteBank(string instrumentId)
        {
            Dictionary<string, dynamic> dictQuote = new Dictionary<string, dynamic>();


            switch (_broker.ToLower())
            {
                case "zerodha":
                    {
                        lock (_synclock)
                        {
                            if (kite == null)
                            {

                                kite = new Kite("67uleb0ttdjmdp6v", Debug: false);

                                kite.Login();

                                Dictionary<string, Quote> kitequote = kite.GetQuote(InstrumentId: new string[] { "NSE:" + instrumentId });
                                foreach (KeyValuePair<string, Quote> kvp in kitequote)
                                {
                                    dictQuote.Add(kvp.Key, kvp.Value);
                                }

                            }

                            else
                            {
                                //Console.WriteLine("MyAccessToken:" + kite.MyAccessToken + "," + Thread.CurrentThread.Name);
                                kite.GetQuote(InstrumentId: new string[] { "NSE:" + instrumentId });
                                Dictionary<string, Quote> kitequote = kite.GetQuote(InstrumentId: new string[] { "NSE:" + instrumentId });
                                foreach (KeyValuePair<string, Quote> kvp in kitequote)
                                {
                                    dictQuote.Add(kvp.Key, kvp.Value);
                                }
                            }
                        }//lock
                        break;
                    }
                case "iifl":
                case "zerodhanew":
                    {
                        IIFLConnect iiflConnect = new IIFLConnect();
                        //lock (_sync)
                        {
                            if (_accessTokenMarket == "")
                            {

                                //IIFLUser user = iiflConnect.GenerateSessionMarket();
                                //iiflConnect.SetAccessToken(user.AccessToken, "marketdata");
                                //_accessTokenMarket = user.AccessToken;
                                GetIIFLMarketAccessToken();
                                iiflConnect.SetAccessToken(_accessTokenMarket);
                                //Console.WriteLine("Entered once GetQuote(), Line 1923");

                            }
                            else
                            {
                                iiflConnect.SetAccessToken(_accessTokenMarket);
                            }
                        }
                        dictQuote = iiflConnect.GetQuoteBank(instrumentId);
                        break;
                    }
                case "stocksdeveloperzerodha":
                    {
                        IIFLConnect iiflConnect = new IIFLConnect();
                        //lock (_sync)
                        {
                            if (_accessTokenMarket == "")
                            {

                                //IIFLUser user = iiflConnect.GenerateSessionMarket();
                                //iiflConnect.SetAccessToken(user.AccessToken, "marketdata");
                                //_accessTokenMarket = user.AccessToken;
                                GetIIFLMarketAccessToken();
                                iiflConnect.SetAccessToken(_accessTokenMarket);
                                //Console.WriteLine("Entered once GetQuote(), Line 1923");

                            }
                            else
                            {
                                iiflConnect.SetAccessToken(_accessTokenMarket);
                            }
                        }
                        dictQuote = iiflConnect.GetQuoteBank(instrumentId);
                        break;
                    }
            }

            return dictQuote;
        }

        public static void CreditSpreadSquareOffPrice(object csvobj)
        {
            CsvInputFileSqoff csvinputobj = (CsvInputFileSqoff)csvobj;
            Kite kite = csvinputobj.kite;
            decimal squareoffdelta = csvinputobj.SqoffDelta;
            string strikeprice1 = csvinputobj.Strike1;            
            int nooflots = csvinputobj.NoofLots ;
            decimal sqoffprice = csvinputobj.SqoffPrice;
            string transtype = csvinputobj.TransType.ToLower();

            //int lotsize = csvinputobj.lotSize;


            string threadname = Thread.CurrentThread.Name;

            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}", strikeprice1, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", threadname, sqoffprice);
            string filename1path = string.Format(@"C:\Algotrade\squareofffile\SquareOffDelta{0}.csv", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}", strikeprice1,  DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData", threadname, sqoffprice);
            string filename2path = string.Format(@"C:\Algotrade\squareofffile\creditsq{0}.csv", filename2);
            string content1 = string.Format("DateTime,Offer,Bid,Diff,Lots,TranType\n");
            File.AppendAllText(filename1path, content1);
            //int nooflots = 0;
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1}               
            };
            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            int sno = 1;
            while (nooflots > 0)
            {
                //Thread.Sleep(200);
                string instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);                

                Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);

               
                KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);

                //get bid ask price
                dynamic lstbids1 = kvpquote1.Value.Bids;
                dynamic lstoffers1 = kvpquote1.Value.Offers;

                decimal ltp = kvpquote1.Value.LastPrice;

                if (ltp <= squareoffdelta && transtype == "buy" && nooflots > 0)
                {

                    Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strikeprice1,
                                                transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                                 price: lstoffers1[0].Price, lotsize: lotsize,
                                                validity: Constants.VALIDITY_DAY);
                    //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);
                    //record the orderid so that we can view the status later
                    string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, response1["result"]["AppOrderID"],
                                          lstoffers1[0].Price, "BUY");
                    Console.WriteLine(writetofile2);
                    File.AppendAllText(filename1path, writetofile2);
                    nooflots -= 1;

                }
                else if (ltp >= squareoffdelta && transtype == "sell" && nooflots > 0)
                {
                    Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strikeprice1,
                                                   transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                                    price: lstbids1[0].Price, lotsize: lotsize,
                                                   validity: Constants.VALIDITY_DAY);
                    //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                    string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, response1["result"]["AppOrderID"],
                                          lstbids1[0].Price, "SELL");
                    Console.WriteLine(writetofile2);
                    File.AppendAllText(filename1path, writetofile2);
                    nooflots -= 1;

                }

                sno++;
            }

        }//end of function

        public static void CreditSpreadSquareOffDelta(object csvobj)
        {
            CsvInputFileSqoff csvinputobj = (CsvInputFileSqoff)csvobj;
            Kite kite = csvinputobj.kite;
            decimal squareoffdelta = csvinputobj.SqoffDelta;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            int nooflots = csvinputobj.NoofLots * -1;

            //int lotsize = csvinputobj.lotSize;


            string threadname = Thread.CurrentThread.Name;

            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", threadname, squareoffdelta);
            string filename1path = string.Format(@"C:\Algotrade\squareofffile\SquareOffDelta{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData", threadname, squareoffdelta);
            string filename2path = string.Format(@"C:\Algotrade\squareofffile\creditsq{0}.txt", filename2);
            string content1 = string.Format("DateTime,Offer,Bid,Diff,Lots,TranType\n");
            File.AppendAllText(filename1path, content1);
            //int nooflots = 0;
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1},
                {"strikeprice2",strikeprice2 }
            };
            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            int sno = 1;
            while (nooflots < 0)
            {
                //Thread.Sleep(200);
                string instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                string instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                //add nfo


                KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                //get bid ask price
                dynamic lstbids1 = kvpquote1.Value.Bids;
                dynamic lstoffers1 = kvpquote1.Value.Offers;
                dynamic lstbids2 = kvpquote2.Value.Bids;
                dynamic lstoffers2 = kvpquote2.Value.Offers;

                decimal bidaskdiff = Math.Abs(lstoffers1[0].Price - lstbids2[0].Price);

                if (bidaskdiff <= squareoffdelta && nooflots < 0)
                {

                    //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= squareoffdelta)
                    {
                        //buy 1 lot
                        //buy 1st strike at ask price and sell 2nd strike at bid price

                        Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strikeprice1,
                                                    transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                                     price: lstoffers1[0].Price, 
                                                     lotsize: lotsize,
                                                    validity: Constants.VALIDITY_DAY);
                        //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                        string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, response1["result"]["AppOrderID"],
                                                            lstoffers2[0].Price, "BUY");
                        Console.WriteLine(writetofile2);
                        File.AppendAllText(filename1path, writetofile2);

                        //sell 2nd strike 
                        //if (orderstatus["Status"].ToLower() == "complete")
                        {
                            Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strikeprice2,
                                                               transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                                               price:lstbids2[0].Price,
                                                               lotsize: lotsize, 
                                                               validity: Constants.VALIDITY_DAY);
                            //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);


                            string writetofile1 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, response2["result"]["AppOrderID"],
                                                                lstbids2[0].Price, "SELL");
                            Console.WriteLine(writetofile1);
                            File.AppendAllText(filename1path, writetofile1);

                            //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                            {
                                nooflots += 1;
                                

                            }
                           
                        }//end of if
                    }//end of if Math.Abs

                }
                string filecontent2 = string.Format("{0},{1},{2},{3},{4},{5}\n", kvpquote1.Value.Timestamp, 
                          lstbids1[0].Price, lstoffers1[0].Price, lstbids2[0].Price, lstoffers2[0].Price, 
                          Math.Abs(lstoffers1[0].Price - lstbids2[0].Price));

                //Console.WriteLine(filecontent);
                File.AppendAllText(filename2path, filecontent2);
                sno++;
            }

        }//end of function
        public static void BNLongSquareOffPercentTrail(object csvobj)
        {
            CsvInputFileSqoff csvinputobj = (CsvInputFileSqoff)csvobj;
            //Kite kite = csvinputobj.kite;
            decimal purchaseprice = csvinputobj.PurchasePrice;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            int nooflots = csvinputobj.NoofLots;
            decimal totalbuyprice = purchaseprice;
            
            //int lotsize = csvinputobj.lotSize;

            string strike1 = "";
            string strike2 = "";

            string threadname = Thread.CurrentThread.Name;

            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", 
                                                   threadname, totalbuyprice);
            string filename1path = string.Format(@"C:\Algotrade\squareoffoutput\SquareOffDelta{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData", 
                                                   threadname, totalbuyprice);
            string filename2path = string.Format(@"C:\Algotrade\squareoffoutput\creditsq{0}.txt", filename2);
            string content1 = string.Format("DateTime,Offer,Bid,Diff,Lots,TranType\n");
            File.AppendAllText(filename1path, content1);

            string filename3 = string.Format("{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");
            string filename3path = string.Format(@"C:\Algotrade\squareoffoutput\BN-LStrangSqoff_{0}.txt", filename2);
            string filecontent3 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename3path, filecontent3);

            //int nooflots = 0;
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1},
                {"strikeprice2",strikeprice2 }
            };

            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            int sno = 1;
            bool trailflag = false;
            decimal currpercent = 1;//start trailing from x%
            decimal nextcurrpercent = csvinputobj.SqoffPercent;
            decimal trailsqoffprice = 0.0m;

            int totallots = 10; //totallots to square of at one time
            int remaininglots = nooflots;
            decimal stoplosspercent = 20.0m;
            decimal stoplossprice = totalbuyprice * (1 - (stoplosspercent * 0.01m));

            while (remaininglots > 0)
            {
                try
                {
                    //Thread.Sleep(200);
                    string instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    string instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                    Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                    Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                   
                    if (remaininglots < totallots)
                    {
                        if (remaininglots % totallots != 0)
                        {
                            totallots = remaininglots;
                        }
                    }

                    //add nfo


                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                    //get bid ask price
                    dynamic lstbids1 = kvpquote1.Value.Bids;
                    dynamic lstoffers1 = kvpquote1.Value.Offers;
                    dynamic lstbids2 = kvpquote2.Value.Bids;
                    dynamic lstoffers2 = kvpquote2.Value.Offers;

                    decimal totalbidprice = Math.Abs(lstbids1[0].Price + lstbids2[0].Price);
                    decimal sqoffprice = totalbuyprice * (1.0m + 0.01m * (nextcurrpercent)); //sqqoff at 3% from previous price

                    if (_broker == "zerodhanew")
                    {
                        Dictionary<string, string> dictParamStrikes1 = GetStrikePrices(strikeprice1.ToString(), strikeprice2.ToString(), "zerodhanew");
                        strike1 = dictParamStrikes1["strike1"];
                        strike2 = dictParamStrikes1["strike2"];

                    }
                    else
                    {
                        strike1 = instrumentId1;
                        strike2 = instrumentId2;
                    }

                    int percentraildiff = 2;

                    //once 5%,3% is reached trigger the trail
                    if (totalbidprice >= sqoffprice)
                    {
                        trailflag = true;
                        trailsqoffprice = totalbuyprice * (1.0m + 0.01m * (nextcurrpercent - percentraildiff));
                        currpercent = nextcurrpercent;
                        nextcurrpercent += percentraildiff;
                    }

                    //while (nooflots > 0)
                    //{

                    if ((totalbidprice <= trailsqoffprice && nooflots > 0 && trailflag == true) ||
                        (DateTime.Now.Hour >= 14 && DateTime.Now.Minute >= 45 && trailflag == true))
                    {

                        //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= squareoffdelta)
                        {
                            //buy 1 lot
                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                                                        transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                                         price: lstbids1[0].Price,
                                                         lotsize: (lotsize * totallots),
                                                        validity: Constants.VALIDITY_DAY);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            string writetofile2 = string.Format("{0},{1},{2},{3},{4},{5}\n", DateTime.Now, strikeprice1, "AppOrderID",
                                                                                lstbids1[0].Price, totallots, "SELL");
                            Console.WriteLine(writetofile2);
                            File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                                                   transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                                                   price: lstbids2[0].Price,
                                                                   lotsize: (lotsize * totallots),
                                                                   validity: Constants.VALIDITY_DAY);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5}\n", DateTime.Now, strikeprice2, "AppOrderID",
                                                                    lstbids2[0].Price,totallots, "SELL");
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                   
                                    nextcurrpercent = currpercent;//reset to current
                                    if (remaininglots <= 0)
                                    {
                                        break; //exit the loop
                                    }

                                }
                                

                            }//end of if
                        }//end of if Math.Abs
                        

                    }
                    else if (totalbidprice < stoplossprice)
                    {
                        //exit when the stoploss is hit
                        //buy 1 lot
                        //buy 1st strike at ask price and sell 2nd strike at bid price

                        //Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                        //                            transactiontype: Constants.TRANSACTION_TYPE_SELL,
                        //                             price: lstbids1[0].Price,
                        //                             lotsize: (lotsize * totallots),
                        //                            validity: Constants.VALIDITY_DAY);
                        //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                        string writetofile2 = string.Format("{0},{1},{2},{3},{4}\n", DateTime.Now, strikeprice1, "AppOrderID",
                                                                            lstbids1[0].Price, "SELL");
                        Console.WriteLine(writetofile2);
                        File.AppendAllText(filename1path, writetofile2);

                        //sell 2nd strike 


                        //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                        //                                   transactiontype: Constants.TRANSACTION_TYPE_SELL,
                        //                                   price: lstbids2[0].Price,
                        //                                   lotsize: (lotsize * totallots),
                        //                                   validity: Constants.VALIDITY_DAY);
                        //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                        decimal percentageincrease = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100.0m, 2);
                        string writetofile1 = string.Format("{0},{1},{2},{3},{4}\n", DateTime.Now, strikeprice2, "AppOrderID",
                                                            lstbids2[0].Price, "SELL");
                        Console.WriteLine(writetofile1);
                        File.AppendAllText(filename1path, writetofile1);
                        remaininglots -= totallots;
                        //break;
                        if (remaininglots <= 0)
                        {
                            break; //exit the loop
                        }


                    }
                    //}//end of while loop

                    decimal totalperceinc = Math.Round(((totalbidprice - totalbuyprice) * 1.0m / totalbuyprice * 1.0m) * 100, 2);
                    string filecontent2 = string.Format("{0},{1},{2},{3},{4},{5}%,{6}%,{7},{8}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                totalbuyprice, totalbidprice, totalperceinc, currpercent, remaininglots,OptionsData._broker);

                    Console.WriteLine(filecontent2);
                    File.AppendAllText(filename2path, filecontent2);
                    Thread.Sleep(5000);
                    sno++;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                    strikeprice1, strikeprice2);
                    File.AppendAllText(filename3path, content);
                }
            }

        }//end of function

        public static void BNShortSquareOffDelta(object csvobj)
        {
            CsvInputFileSqoff csvinputobj = (CsvInputFileSqoff)csvobj;
            //Kite kite = csvinputobj.kite;
            decimal squareoffprice = csvinputobj.SqoffPrice;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            int nooflots = csvinputobj.NoofLots;
            //int lotsize = csvinputobj.lotSize;


            string threadname = Thread.CurrentThread.Name;

            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData",
                                                      threadname, squareoffprice);
            string filename1path = string.Format(@"C:\Algotrade\squareoffoutput\SSquareOffDelta{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData",
                                                      threadname, squareoffprice);
            string filename2path = string.Format(@"C:\Algotrade\squareoffoutput\SS_{0}.txt", filename2);
            string content1 = string.Format("DateTime,Offer,Bid,Diff,Lots,TranType\n");
            File.AppendAllText(filename1path, content1);

            string filename3 = string.Format("{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");
            string filename3path = string.Format(@"C:\Algotrade\squareoffoutput\BN-SStrangSqoff_{0}.txt", filename2);
            string filecontent3 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename3path, filecontent3);

            //int nooflots = 0;
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1},
                {"strikeprice2",strikeprice2 }
            };
            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            int sno = 1;

            string strike1 = "";
            string strike2 = "";

            int totallots = 10; //totallots to square of at one time
            int remaininglots = nooflots;

            while (remaininglots > 0)
            {
                try
                {
                    //Thread.Sleep(200);
                    string instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    string instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                    Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                    Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                    //int rem = totallots % remaininglots;
                    if (remaininglots < totallots)
                    {
                        if (remaininglots % totallots != 0)
                        {
                            totallots = remaininglots;
                        }
                    }

                    //add nfo
                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                    //get bid ask price
                    dynamic lstbids1 = kvpquote1.Value.Bids;
                    dynamic lstoffers1 = kvpquote1.Value.Offers;
                    dynamic lstbids2 = kvpquote2.Value.Bids;
                    dynamic lstoffers2 = kvpquote2.Value.Offers;

                    decimal totalaskprice = Math.Abs(lstoffers1[2].Price + lstoffers2[2].Price);

                    int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                    int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                    //Get broker specific strike price
                    if (_broker.ToLower() == "zerodhanew")
                    {
                        Dictionary<string, string> dictParamStrikes1 = ConvertStrikePricesfromIIFL(strikeprice1, strikeprice2, "zerodhanew");
                        strike1 = dictParamStrikes1["strike1"];
                        strike2 = dictParamStrikes1["strike2"];

                    }
                    else if (_broker.ToLower() == "iifl")
                    {
                        strike1 = instrumentId1;
                        strike2 = instrumentId2;
                    }

                    if (totalaskprice<= squareoffprice /*&& remaininglots > 0*/)
                    {

                        //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= squareoffdelta)
                        {
                            //buy 1 lot

                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                                                        transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                                         price: lstoffers1[2].Price,
                                                         lotsize: (lotsize * totallots),
                                                        validity: Constants.VALIDITY_DAY);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                                                   transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                                                   price: lstoffers2[2].Price,
                                                                   lotsize: (lotsize * totallots),
                                                                   validity: Constants.VALIDITY_DAY);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((squareoffprice-totalaskprice) * 1.0m / squareoffprice * 1.0m) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}\n", DateTime.Now, strikeprice1, "AppOrderID",
                                                                    lstoffers1[2].Price, lstoffers2[2].Price, squareoffprice, remaininglots, totalaskprice,
                                                                    percentageincrease);
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }

                            }//end of if
                        }//end of if


                    }
                    decimal totalperceinc = Math.Round(((squareoffprice-totalaskprice) * 1.0m / squareoffprice * 1.0m) * 100, 2);
                    string filecontent2 = string.Format("{0},{1},{2},{3},{4},{5},{6}%,{7}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                               totalaskprice, squareoffprice, nooflots, totalperceinc, _broker);

                    Console.WriteLine(filecontent2);
                    File.AppendAllText(filename2path, filecontent2);
                    Thread.Sleep(5000);
                    sno++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                    strikeprice1, strikeprice2);
                    File.AppendAllText(filename3path, content);
                }
            }

        }//end of function

        public static void BNLongBuyDelta(object csvobj)
        {
            CsvInputFileSqoff csvinputobj = (CsvInputFileSqoff)csvobj;
            //Kite kite = csvinputobj.kite;
            decimal buyprice = csvinputobj.PurchasePrice;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            int nooflots = csvinputobj.NoofLots;
            //int lotsize = csvinputobj.lotSize;


            string threadname = Thread.CurrentThread.Name;

            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData",
                                                      threadname, buyprice);
            string filename1path = string.Format(@"C:\Algotrade\squareoffoutput\SquareOffDelta{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData",
                                                      threadname, buyprice);
            string filename2path = string.Format(@"C:\Algotrade\squareoffoutput\creditsq{0}.txt", filename2);
            string content1 = string.Format("DateTime,Offer,Bid,Diff,Lots,TranType\n");
            File.AppendAllText(filename1path, content1);

            string filename3 = string.Format("{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");
            string filename3path = string.Format(@"C:\Algotrade\squareoffoutput\BN-LStrangSqoff_{0}.txt", filename2);
            string filecontent3 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename3path, filecontent3);

            //int nooflots = 0;
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1},
                {"strikeprice2",strikeprice2 }
            };
            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            int sno = 1;

            string strike1 = "";
            string strike2 = "";

            int totallots = 10; //totallots to square of at one time
            int remaininglots = nooflots;
            int maxlots = 20;

            while (nooflots < maxlots)
            {
                try
                {
                    //Thread.Sleep(200);
                    string instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    string instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                    Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                    Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                    //int rem = totallots % remaininglots;
                    if (remaininglots < totallots)
                    {
                        if (remaininglots % totallots != 0)
                        {
                            totallots = remaininglots;
                        }
                    }

                    //add nfo
                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                    //get bid ask price
                    dynamic lstbids1 = kvpquote1.Value.Bids;
                    dynamic lstoffers1 = kvpquote1.Value.Offers;
                    dynamic lstbids2 = kvpquote2.Value.Bids;
                    dynamic lstoffers2 = kvpquote2.Value.Offers;

                    decimal totalbidprice = Math.Abs(lstbids1[0].Price + lstbids2[0].Price);

                    int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                    int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                    //Get broker specific strike price
                    if (_broker.ToLower() == "zerodhanew")
                    {
                        Dictionary<string, string> dictParamStrikes1 = ConvertStrikePricesfromIIFL(strikeprice1, strikeprice2, "zerodhanew");
                        strike1 = dictParamStrikes1["strike1"];
                        strike2 = dictParamStrikes1["strike2"];

                    }
                    else if (_broker.ToLower() == "iifl")
                    {
                        strike1 = instrumentId1;
                        strike2 = instrumentId2;
                    }

                    if (totalbidprice <= buyprice /*&& remaininglots > 0*/)
                    {

                        //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= squareoffdelta)
                        {
                            //buy 1 lot

                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                                                        transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                                         price: lstbids1[0].Price,
                                                         lotsize: (lotsize * totallots),
                                                        validity: Constants.VALIDITY_DAY);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                                                   transactiontype: Constants.TRANSACTION_TYPE_BUY,
                                                                   price: lstbids2[0].Price,
                                                                   lotsize: (lotsize * totallots),
                                                                   validity: Constants.VALIDITY_DAY);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbidprice - buyprice) * 1.0m / buyprice * 1.0m) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}\n", DateTime.Now, strikeprice1, strikeprice2, "AppOrderID",
                                                                    lstbids1[0].Price, lstbids2[0].Price, buyprice, totallots, totalbidprice,
                                                                    percentageincrease);
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);


                                nooflots += totallots;
                                //break;
                                //
                                int lotdiff = maxlots - nooflots;
                                if (lotdiff < totallots)
                                {
                                    int rem = lotdiff % totallots;
                                    if (rem != 0)
                                    {
                                        totallots = lotdiff;
                                    }
                                }

                                if (nooflots >= maxlots)
                                {
                                    break;
                                }


                            }//end of if
                        }//end of if


                    }
                    decimal totalperceinc = Math.Round(((totalbidprice - buyprice) * 1.0m / buyprice * 1.0m) * 100, 2);
                    string filecontent2 = string.Format("{0},{1},{2},{3},{4},{5},{6}%,{7}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                               totalbidprice, buyprice, nooflots, totalperceinc, _broker);

                    Console.WriteLine(filecontent2);
                    File.AppendAllText(filename2path, filecontent2);
                    sno++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                    strikeprice1, strikeprice2);
                    File.AppendAllText(filename3path, content);
                }
            }

        }//end of function
        public static void BNLongSquareOffDelta(object csvobj)
        {
            CsvInputFileSqoff csvinputobj = (CsvInputFileSqoff)csvobj;
            //Kite kite = csvinputobj.kite;
            decimal squareoffprice = csvinputobj.SqoffPrice;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            int nooflots = csvinputobj.NoofLots;
            //int lotsize = csvinputobj.lotSize;


            string threadname = Thread.CurrentThread.Name;

            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", 
                                                      threadname, squareoffprice);
            string filename1path = string.Format(@"C:\Algotrade\squareoffoutput\SquareOffDelta{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData", 
                                                      threadname, squareoffprice);
            string filename2path = string.Format(@"C:\Algotrade\squareoffoutput\creditsq{0}.txt", filename2);
            string content1 = string.Format("DateTime,Offer,Bid,Diff,Lots,TranType\n");
            File.AppendAllText(filename1path, content1);

            string filename3 = string.Format("{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");
            string filename3path = string.Format(@"C:\Algotrade\squareoffoutput\BN-LStrangSqoff_{0}.txt", filename2);
            string filecontent3 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename3path, filecontent3);

            //int nooflots = 0;
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1},
                {"strikeprice2",strikeprice2 }
            };
            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            int sno = 1;

            string strike1 = "";
            string strike2 = "";

            int totallots = 10; //totallots to square of at one time
            int remaininglots = nooflots;

            while (remaininglots > 0)
            {
                try
                {
                    //Thread.Sleep(200);
                    string instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    string instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                    Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                    Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                    //int rem = totallots % remaininglots;
                    if (remaininglots < totallots)
                    {
                        if (remaininglots % totallots != 0)
                        {
                            totallots = remaininglots;
                        }
                    }

                    //add nfo
                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                    //get bid ask price
                    dynamic lstbids1 = kvpquote1.Value.Bids;
                    dynamic lstoffers1 = kvpquote1.Value.Offers;
                    dynamic lstbids2 = kvpquote2.Value.Bids;
                    dynamic lstoffers2 = kvpquote2.Value.Offers;

                    decimal totalbidprice = Math.Abs(lstbids1[0].Price + lstbids2[0].Price);

                    int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                    int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                    //Get broker specific strike price
                    if (_broker.ToLower() == "zerodhanew")
                    {
                        Dictionary<string, string> dictParamStrikes1 = ConvertStrikePricesfromIIFL(strikeprice1, strikeprice2, "zerodhanew");
                        strike1 = dictParamStrikes1["strike1"];
                        strike2 = dictParamStrikes1["strike2"];

                    }
                    else if (_broker.ToLower() == "iifl")
                    {
                        strike1 = instrumentId1;
                        strike2 = instrumentId2;
                    }
                    if(squareoffprice==0)
                    {
                        Console.WriteLine("Square of price cannot be zero");
                        throw new Exception("Square off price is zero");
                    }

                    if (totalbidprice >= squareoffprice /*&& nooflots > 0*/)
                    {

                        //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= squareoffdelta)
                        {
                      

                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                                                        transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                                         price: lstbids1[0].Price,
                                                         lotsize: (lotsize * totallots),
                                                        validity: Constants.VALIDITY_DAY);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                                                   transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                                                   price: lstbids2[0].Price,
                                                                   lotsize: (lotsize * totallots),
                                                                   validity: Constants.VALIDITY_DAY);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbidprice - squareoffprice) * 1.0m / squareoffprice * 1.0m) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n", DateTime.Now, strikeprice1,strikeprice2, "AppOrderID",
                                                                    lstbids1[0].Price, lstbids2[0].Price, squareoffprice, totallots, totalbidprice,
                                                                    percentageincrease);
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }

                            }//end of if
                        }//end of if
                       

                    }
                    decimal totalperceinc = Math.Round(((totalbidprice - squareoffprice) * 1.0m / squareoffprice * 1.0m) * 100, 2);
                    string filecontent2 = string.Format("{0},{1},{2},{3},{4},{5},{6}%,{7}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                               totalbidprice, squareoffprice, nooflots, totalperceinc, _broker);

                    Console.WriteLine(filecontent2);
                    File.AppendAllText(filename2path, filecontent2);
                    Thread.Sleep(5000);
                    sno++;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                    strikeprice1, strikeprice2);
                    File.AppendAllText(filename3path, content);
                }
            }

        }//end of function
        public static void ChartInkTriggerCreditSquareOff(object csvobj)
        {
            CsvInputFileSqoff csvinputobj = (CsvInputFileSqoff)csvobj;
            //Kite kite = csvinputobj.kite;
            decimal squareoffprice = csvinputobj.SqoffPrice;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            ///string strikeprice3 = csvinputobj.Strike3;
            decimal purchaseprice = csvinputobj.PurchasePrice;
            decimal sqoffpercent = csvinputobj.SqoffPercent;
            decimal stoplossprice = csvinputobj.StopPrice;
            if (sqoffpercent != 0.0m)
            {
                squareoffprice = purchaseprice * (1 - (sqoffpercent / 100.0m));
            }
            if (purchaseprice!=0.0m)
            {
                squareoffprice = purchaseprice;

            }

            int nooflots = csvinputobj.NoofLots;
            //int lotsize = csvinputobj.lotSize;
            if (strikeprice2 == string.Empty)
            {
                Console.WriteLine("Future trading");
            }

            string threadname = Thread.CurrentThread.Name;

            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData",
                                                      threadname);
            string filename1path = string.Format(@"C:\Algotrade\squareoffoutput\CICreditSquareOffDelta{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData",
                                                      threadname);
            string filename2path = string.Format(@"C:\Algotrade\squareoffoutput\creditsq{0}.txt", filename2);
            string content1 = string.Format("DateTime,Offer,Bid,Diff,Lots,TranType\n");
            File.AppendAllText(filename1path, content1);

            string filename3 = string.Format("{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");
            string filename3path = string.Format(@"C:\Algotrade\squareoffoutput\CI-Creditsqoff_{0}.txt", filename2);
            string filecontent3 = "DateTime,Message,StackTrace,Strike1,Strike2,Strike3,LineNo\r\n";
            File.AppendAllText(filename3path, filecontent3);

            //int nooflots = 0;
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1},
                {"strikeprice2",strikeprice2 }
            };
            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            int sno = 1;

            string strike1 = "";
            string strike2 = "";

            int totallots = 10; //totallots to square of at one time
            int remaininglots = nooflots;

            while (remaininglots > 0)
            {
                try
                {
                    //Thread.Sleep(200);
                    string instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    string instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                    Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                    Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                    //int rem = totallots % remaininglots;
                    if (remaininglots < totallots)
                    {
                        if (remaininglots % totallots != 0)
                        {
                            totallots = remaininglots;
                        }
                    }

                    //add nfo
                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);
                    int bidofferindex = 0;
                    //get bid ask price
                    dynamic lstbids1 = kvpquote1.Value.Bids;
                    dynamic lstoffers1 = kvpquote1.Value.Offers;
                    dynamic lstbids2 = kvpquote2.Value.Bids;
                    dynamic lstoffers2 = kvpquote2.Value.Offers;

                    decimal totalbidofferprice = Math.Abs(lstoffers1[bidofferindex].Price + lstbids2[bidofferindex].Price);

                    int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                    int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                    //Get broker specific strike price
                    if (_broker.ToLower() == "zerodhanew")
                    {
                        Dictionary<string, string> dictParamStrikes1 = ConvertStrikePricesfromIIFL(strikeprice1, strikeprice2, "zerodhanew");
                        strike1 = dictParamStrikes1["strike1"];
                        strike2 = dictParamStrikes1["strike2"];

                    }
                    else if (_broker.ToLower() == "iifl")
                    {
                        strike1 = instrumentId1;
                        strike2 = instrumentId2;
                    }
                    if (squareoffprice == 0)
                    {
                        Console.WriteLine("Square of price cannot be zero");
                        throw new Exception("Square off price is zero");
                    }

                    if (totalbidofferprice <= squareoffprice /*&& nooflots > 0*/)
                    {                       

                           string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                                                                lstoffers1[bidofferindex].Price, "buy");
                            Console.WriteLine(writetofile2);
                            File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                //                                   transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                //                                   price: lstbids2[bidofferindex].Price,
                                //                                   lotsize: (lotsize * totallots),
                                //                                   validity: Constants.VALIDITY_DAY);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbidofferprice - squareoffprice) * 1.0m / squareoffprice * 1.0m) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}\n", DateTime.Now, strikeprice1, strikeprice2, 
                                                                    lstbids1[0].Price, lstbids2[0].Price, squareoffprice, totallots, totalbidofferprice,
                                                                    percentageincrease, "TargetHit");
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }

                            }//end of if
                        


                    }
                    else if (totalbidofferprice >= stoplossprice /*&& nooflots > 0*/)
                    {
                       


                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            //Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                            //                            transactiontype: Constants.TRANSACTION_TYPE_SELL,
                            //                             price: lstoffer1[0].Price,
                            //                             lotsize: (lotsize * totallots),
                            //                            validity: Constants.VALIDITY_DAY);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                                                                lstoffers1[bidofferindex].Price, "sell");
                            Console.WriteLine(writetofile2);
                            File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                //                                   transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                //                                   price: lstbids2[bidofferindex].Price,
                                //                                   lotsize: (lotsize * totallots),
                                //                                   validity: Constants.VALIDITY_DAY);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbidofferprice - stoplossprice) * 1.0m / stoplossprice * 1.0m) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n", DateTime.Now, strikeprice1, strikeprice2, "AppOrderID",
                                                                    lstbids1[0].Price, lstbids2[0].Price, stoplossprice, totallots, totalbidofferprice,
                                                                    percentageincrease, "StoplossHit");
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }

                            }//end of if
                        


                    }
                    decimal totalperceinc = Math.Round(((totalbidofferprice - squareoffprice) * 1.0m / squareoffprice * 1.0m) * 100, 2);
                    string filecontent2 = string.Format("{0},{1},{2},{3},{4},{5},{6}%,{7}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                               totalbidofferprice, squareoffprice, nooflots, totalperceinc, _broker);

                    Console.WriteLine(filecontent2);
                    File.AppendAllText(filename2path, filecontent2);
                    Thread.Sleep(5000);
                    sno++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                    strikeprice1, strikeprice2);
                    File.AppendAllText(filename3path, content);
                }
            }

        }//end of function

        public static void ChartInkTriggerDebitSquareOff(object csvobj)
        {
            CsvInputFileSqoff csvinputobj = (CsvInputFileSqoff)csvobj;
            //Kite kite = csvinputobj.kite;
            decimal squareoffprice = csvinputobj.SqoffPrice;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            string strikeprice3 = csvinputobj.Strike3;
            int nooflots = csvinputobj.NoofLots;
            //int lotsize = csvinputobj.lotSize;
            string targetstoplossflag = csvinputobj.Status;
            if (strikeprice2 == string.Empty)
            {
                Console.WriteLine("Future trading");
            }

            string threadname = Thread.CurrentThread.Name;

            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData",
                                                      threadname, squareoffprice);
            string filename1path = string.Format(@"C:\Algotrade\squareoffoutput\CISquareOffDelta{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData",
                                                      threadname, squareoffprice);
            string filename2path = string.Format(@"C:\Algotrade\squareoffoutput\creditsq{0}.txt", filename2);
            string content1 = string.Format("DateTime,Offer,Bid,Diff,Lots,TranType\n");
            File.AppendAllText(filename1path, content1);

            string filename3 = string.Format("{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");
            string filename3path = string.Format(@"C:\Algotrade\squareoffoutput\CI-Creditsqoff_{0}.txt", filename2);
            string filecontent3 = "DateTime,Message,StackTrace,Strike1,Strike2,Strike3,LineNo\r\n";
            File.AppendAllText(filename3path, filecontent3);

            //int nooflots = 0;
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1},
                {"strikeprice2",strikeprice2 }
            };
            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            int sno = 1;

            string strike1 = "";
            string strike2 = "";

            int totallots = 10; //totallots to square of at one time
            int remaininglots = nooflots;

            while (remaininglots > 0)
            {
                try
                {
                    //Thread.Sleep(200);
                    string instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    string instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                    Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                    Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                    //int rem = totallots % remaininglots;
                    if (remaininglots < totallots)
                    {
                        if (remaininglots % totallots != 0)
                        {
                            totallots = remaininglots;
                        }
                    }

                    //add nfo
                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);
                    int bidofferindex = 0;
                    //get bid ask price
                    dynamic lstbids1 = kvpquote1.Value.Bids;
                    dynamic lstoffers1 = kvpquote1.Value.Offers;
                    dynamic lstbids2 = kvpquote2.Value.Bids;
                    dynamic lstoffers2 = kvpquote2.Value.Offers;

                    decimal totalbidofferprice = Math.Abs(lstoffers1[bidofferindex].Price + lstbids2[bidofferindex].Price);

                    int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                    int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                    //Get broker specific strike price
                    if (_broker.ToLower() == "zerodhanew")
                    {
                        Dictionary<string, string> dictParamStrikes1 = ConvertStrikePricesfromIIFL(strikeprice1, strikeprice2, "zerodhanew");
                        strike1 = dictParamStrikes1["strike1"];
                        strike2 = dictParamStrikes1["strike2"];

                    }
                    else if (_broker.ToLower() == "iifl")
                    {
                        strike1 = instrumentId1;
                        strike2 = instrumentId2;
                    }
                    if (squareoffprice == 0)
                    {
                        Console.WriteLine("Square of price cannot be zero");
                        throw new Exception("Square off price is zero");
                    }

                    if (totalbidofferprice <= squareoffprice /*&& nooflots > 0*/)
                    {

                        


                            //buy 1st strike at ask price and sell 2nd strike at bid price

                            //Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                            //                            transactiontype: Constants.TRANSACTION_TYPE_SELL,
                            //                             price: lstbids1[0].Price,
                            //                             lotsize: (lotsize * totallots),
                            //                            validity: Constants.VALIDITY_DAY);
                            //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                            //string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                            //                                    lstoffers2[0].Price, "sell");
                            //Console.WriteLine(writetofile2);
                            //File.AppendAllText(filename1path, writetofile2);

                            //sell 2nd strike 
                            //if (orderstatus["Status"].ToLower() == "complete")
                            {
                                Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                                                                   transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                                                   price: lstbids2[0].Price,
                                                                   lotsize: (lotsize * totallots),
                                                                   validity: Constants.VALIDITY_DAY);
                                //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                                decimal percentageincrease = Math.Round(((totalbidofferprice - squareoffprice) * 1.0m / squareoffprice * 1.0m) * 100.0m, 2);
                                string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n", DateTime.Now, strikeprice1, strikeprice2, "AppOrderID",
                                                                    lstbids1[0].Price, lstbids2[0].Price, squareoffprice, totallots, totalbidofferprice,
                                                                    percentageincrease);
                                Console.WriteLine(writetofile1);
                                File.AppendAllText(filename1path, writetofile1);

                                //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                                {
                                    remaininglots -= totallots;
                                    //break;
                                }

                            }//end of if
                        


                    }
                    decimal totalperceinc = Math.Round(((totalbidofferprice - squareoffprice) * 1.0m / squareoffprice * 1.0m) * 100, 2);
                    string filecontent2 = string.Format("{0},{1},{2},{3},{4},{5},{6}%,{7}\n", DateTime.Now, strikeprice1, strikeprice2,
                                                               totalbidofferprice, squareoffprice, nooflots, totalperceinc, _broker);

                    Console.WriteLine(filecontent2);
                    File.AppendAllText(filename2path, filecontent2);
                    Thread.Sleep(5000);
                    sno++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string content = string.Format("{0},{1},{2},{3},{4}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                    strikeprice1, strikeprice2);
                    File.AppendAllText(filename3path, content);
                }
            }

        }//end of function

        public static void ChartInkTriggerFutureSquareOff(object csvobj)
        {
            CsvInputFileSqoff csvinputobj = (CsvInputFileSqoff)csvobj;
            //Kite kite = csvinputobj.kite;
            decimal squareoffprice = csvinputobj.SqoffPrice;
            string strikeprice1 = csvinputobj.Strike1;
            
            int nooflots = csvinputobj.NoofLots;
            //int lotsize = csvinputobj.lotSize;
            string targetstoplossflag = csvinputobj.Status;
            decimal stoplossprice = csvinputobj.StopPrice;
            string expiryDate = csvinputobj.ExpiryDate;
            string threadname = Thread.CurrentThread.Name;

            string filename1 = string.Format("{0}-{1}_{2}_{3}", strikeprice1, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData",
                                                      threadname);
            string filename1path = string.Format(@"C:\Algotrade\squareoffoutput\CISquareOffFUT{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}", strikeprice1, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData",
                                                      threadname);
            string filename2path = string.Format(@"C:\Algotrade\squareoffoutput\creditsq{0}.txt", filename2);
            string content1 = string.Format("DateTime,Offer,Bid,Diff,Lots,TranType\n");
            File.AppendAllText(filename1path, content1);

            string filename3 = string.Format("{0}-{1}", DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog");
            string filename3path = string.Format(@"C:\Algotrade\squareoffoutput\CI-Creditsqoff_{0}.txt", filename2);
            string filecontent3 = "DateTime,Message,StackTrace,Strike1,Strike2,Strike3,LineNo\r\n";
            File.AppendAllText(filename3path, filecontent3);

            //int nooflots = 0;
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1}
               
            };
            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsizeFUT(ref dictinstrumentid, expiryDate);

            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            int sno = 1;

            string strike1 = "";
            string strike2 = "";

            int totallots = 10; //totallots to square of at one time
            int remaininglots = nooflots;

            while (remaininglots > 0)
            {
                try
                {
                    //Thread.Sleep(200);
                    string instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    //string instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                    Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                    //Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                    //int rem = totallots % remaininglots;
                    if (remaininglots < totallots)
                    {
                        if (remaininglots % totallots != 0)
                        {
                            totallots = remaininglots;
                        }
                    }

                    //add nfo
                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    //KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);
                    int bidofferindex = 0;
                    //get bid ask price
                    dynamic lstbids1 = kvpquote1.Value.Bids;
                    dynamic lstoffers1 = kvpquote1.Value.Offers;
                    //dynamic lstbids2 = kvpquote2.Value.Bids;
                    //dynamic lstoffers2 = kvpquote2.Value.Offers;

                    decimal totalbidofferprice = Math.Abs(lstbids1[bidofferindex].Price);

                    int totaltradedquantity1 = kvpquote1.Value.LastQuantity;
                    //int totaltradedquantity2 = kvpquote2.Value.LastQuantity;

                    //Get broker specific strike price


                    strike1 = instrumentId1;
                        //strike2 = instrumentId2;
                    
                    if (squareoffprice == 0)
                    {
                        Console.WriteLine("Square of price cannot be zero");
                        throw new Exception("Square off price is zero");
                    }

                    if (totalbidofferprice >= squareoffprice /*&& nooflots > 0*/)
                    {                        

                      
                        //if (orderstatus["Status"].ToLower() == "complete")
                        {
                            //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                            //                                   transactiontype: Constants.TRANSACTION_TYPE_SELL,
                            //                                   price: totalbidofferprice,
                            //                                   lotsize: (lotsize * totallots),
                            //                                   validity: Constants.VALIDITY_DAY);
                            //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                            decimal percentageincrease = Math.Round(((totalbidofferprice - squareoffprice) * 1.0m / squareoffprice * 1.0m) * 100.0m, 2);
                            decimal profit = (totalbidofferprice - squareoffprice) * lotsize * nooflots;
                            string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\n", DateTime.Now, strikeprice1,
                                                                lstbids1[0].Price, totalbidofferprice, squareoffprice, totallots,
                                                                profit, "Target Hit");
                            Console.WriteLine("Target Hit");
                            Console.WriteLine(writetofile1);
                            File.AppendAllText(filename1path, writetofile1);

                            //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                            {
                                remaininglots -= totallots;
                                //break;
                            }

                        }//end of if



                    }//end of if
                    else if (totalbidofferprice <= stoplossprice /*&& nooflots > 0*/)
                    {



                        //buy 1st strike at ask price and sell 2nd strike at bid price

                        //Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strike1,
                        //                            transactiontype: Constants.TRANSACTION_TYPE_SELL,
                        //                             price: lstoffer1[0].Price,
                        //                             lotsize: (lotsize * totallots),
                        //                            validity: Constants.VALIDITY_DAY);
                        //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                        string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, "AppOrderId",
                                                            lstoffers1[bidofferindex].Price, "sell");
                        Console.WriteLine(writetofile2);
                        File.AppendAllText(filename1path, writetofile2);

                        //sell 2nd strike 
                        //if (orderstatus["Status"].ToLower() == "complete")
                        {
                            //Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strike2,
                            //                                   transactiontype: Constants.TRANSACTION_TYPE_SELL,
                            //                                   price: lstbids2[bidofferindex].Price,
                            //                                   lotsize: (lotsize * totallots),
                            //                                   validity: Constants.VALIDITY_DAY);
                            //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);

                            decimal percentageincrease = Math.Round(((totalbidofferprice - stoplossprice) * 1.0m / stoplossprice * 1.0m) * 100.0m, 2);
                            string writetofile1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\n", DateTime.Now, strikeprice1,
                                                                lstbids1[0].Price, totalbidofferprice, squareoffprice, totallots, 
                                                                percentageincrease,"Stoploss Hit");
                            Console.WriteLine(writetofile1);
                            File.AppendAllText(filename1path, writetofile1);

                            //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                            {
                                remaininglots -= totallots;
                                break;
                            }

                        }//end of if



                    }//else
                    decimal totalperceinc = Math.Round(((totalbidofferprice - squareoffprice) * 1.0m / squareoffprice * 1.0m) * 100, 2);
                    string filecontent2 = string.Format("{0},{1},{2},{3},{4},{5}%,{6}\n", DateTime.Now, strikeprice1,
                                                               totalbidofferprice, squareoffprice, nooflots, totalperceinc, _broker);

                    Console.WriteLine(filecontent2);
                    File.AppendAllText(filename2path, filecontent2);
                    Thread.Sleep(5000);
                    sno++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string content = string.Format("{0},{1},{2},{3}\r\n", DateTime.Now, ex.Message, ex.StackTrace,
                                                    strikeprice1);
                    File.AppendAllText(filename3path, content);
                }
            }

        }//end of function

        public static void BNShortStraddleSquareOffPercent(object csvobj)
        {
            CsvInputFileSqoff csvinputobj = (CsvInputFileSqoff)csvobj;
            Kite kite = csvinputobj.kite;
            decimal squareoffdelta = csvinputobj.SqoffDelta;
            decimal totalbuyprice = squareoffdelta;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;
            int nooflots = csvinputobj.NoofLots;

            //int lotsize = csvinputobj.lotSize;


            string threadname = Thread.CurrentThread.Name;

            string filename1 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData", threadname, squareoffdelta);
            string filename1path = string.Format(@"C:\Algotrade\squareofffile\SquareOffDelta{0}.txt", filename1);
            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}_{5}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "ContinousData", threadname, squareoffdelta);
            string filename2path = string.Format(@"C:\Algotrade\squareofffile\creditsq{0}.txt", filename2);
            string content1 = string.Format("DateTime,Offer,Bid,Diff,Lots,TranType\n");
            File.AppendAllText(filename1path, content1);
            //int nooflots = 0;
            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1},
                {"strikeprice2",strikeprice2 }
            };
            Dictionary<string, dynamic> instrumentids = GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            int sno = 1;
            TimeSpan end = new TimeSpan(15, 31, 0);
           

            while (nooflots > 0 && new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) < end)
            {
                //Thread.Sleep(200);
                string instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                string instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                Dictionary<string, dynamic> quotes1 = GetQuote(instrumentId1);
                Dictionary<string, dynamic> quotes2 = GetQuote(instrumentId2);

                //add nfo


                KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                //get bid ask price
                dynamic lstbids1 = kvpquote1.Value.Bids;
                dynamic lstoffers1 = kvpquote1.Value.Offers;
                dynamic lstbids2 = kvpquote2.Value.Bids;
                dynamic lstoffers2 = kvpquote2.Value.Offers;

                decimal bidaskdiff = Math.Abs(lstoffers1[0].Price - lstoffers2[0].Price);
                //decimal sqoffprice = squareoffdelta * 1.05m; //qoff at 5% from previous price
                ///trail percentage 
                
                if (bidaskdiff <= squareoffdelta && nooflots > 0)
                {

                    //if (Math.Abs(lstoffers1[0].Price - lstbids2[0].Price) <= squareoffdelta)
                    {
                        //buy 1 lot
                        //buy 1st strike at ask price and sell 2nd strike at bid price

                        Dictionary<string, dynamic> response1 = PlaceOrderOption(strikeprice: strikeprice1,
                                                    transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                                     price: lstbids1[0].Price,
                                                     lotsize: (lotsize * nooflots),
                                                    validity: Constants.VALIDITY_DAY);
                        //Dictionary<string, dynamic> orderstatus = GetOrderStatus(response1);

                        string writetofile2 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, response1["result"]["AppOrderID"],
                                                            lstoffers2[0].Price, "BUY");
                        Console.WriteLine(writetofile2);
                        File.AppendAllText(filename1path, writetofile2);

                        //sell 2nd strike 
                        //if (orderstatus["Status"].ToLower() == "complete")
                        {
                            Dictionary<string, dynamic> response2 = PlaceOrderOption(strikeprice: strikeprice2,
                                                               transactiontype: Constants.TRANSACTION_TYPE_SELL,
                                                               price: lstbids2[0].Price,
                                                               lotsize: (lotsize * nooflots),
                                                               validity: Constants.VALIDITY_DAY);
                            //Dictionary<string, dynamic> orderstatus1 = GetOrderStatus(response2);


                            string writetofile1 = string.Format("{0},{1},{2},{3}\n", DateTime.Now, response2["result"]["AppOrderID"],
                                                                lstbids2[0].Price, "SELL");
                            Console.WriteLine(writetofile1);
                            File.AppendAllText(filename1path, writetofile1);

                            //if (orderstatus["Status"].ToLower() == "complete" &&  orderstatus1["Status"].ToLower() == "complete")
                            {
                                nooflots -= 1;
                                break;

                            }

                        }//end of if
                    }//end of if Math.Abs

                }
                string filecontent2 = string.Format("{0},{1},{2},{3},{4},{5}\n", kvpquote1.Value.Timestamp,
                          lstbids1[0].Price, lstoffers1[0].Price, lstbids2[0].Price, lstoffers2[0].Price,
                          Math.Abs(lstoffers1[0].Price - lstbids2[0].Price));

                //Console.WriteLine(filecontent);
                File.AppendAllText(filename2path, filecontent2);
                sno++;
            }

        }//end of function
        public void CreditDebitSpreadStrategyTest(Kite kite, string strikeprice1, string strikeprice2,
                                             decimal startpivot, decimal pivotdelta,int maxlots)
        {
            decimal currentpivot1 = startpivot;
            string filename1 = string.Format("{0}-{1}_{2}_{3}", strikeprice1, strikeprice2, DateTime.Now.ToString("dd-MM-yyyy"), "TradeData");
            string filename1path = string.Format(@"C:\Algotrade\creditspread{0}.txt", filename1);
            //decimal pivotdelta = 2.5m;
            //int iterations = 4;
            int nooflots = 0;
            //decimal maxuppivotrange = currentpivot + (pivotdelta * iterations);
            //decimal maxdownpivotrange = currentpivot - (pivotdelta * iterations);
            decimal nextpivotup = startpivot;
            decimal nextpivotdown = startpivot;
            decimal currentpivot = startpivot;
            decimal currentpivot2 = startpivot;

            int sno = 1;
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {

                
                Dictionary<string, Quote> quotes = kite.GetQuote(InstrumentId: new string[] { "NFO:" + strikeprice1, "NFO:" + strikeprice2, "NSE:NIFTY 50" });
                KeyValuePair<string, Quote> kvpquote1 = quotes.ElementAt(0);
                KeyValuePair<string, Quote> kvpquote2 = quotes.ElementAt(1);
                KeyValuePair<string, Quote> kvpnifty = quotes.ElementAt(2);
                Console.WriteLine(sno+ ","+kvpquote1.Value.LastPrice + "," + kvpquote2.Value.LastPrice);
                sno++;
                Thread.Sleep(1000);
            }
        }

        public List<CsvInputFileObjectsPivot> CreateMultiPivots(string csvinputfile,decimal pivotspread, Kite kite=null)
        {
            List<CsvInputFileObjectsPivot> lstinputfileobj = CsvFileInputData(csvinputfile);
            List<CsvInputFileObjectsPivot> lstmultipivot = new List<CsvInputFileObjectsPivot>();
            int pivotiteration = 3;
            foreach(CsvInputFileObjectsPivot csvinput in lstinputfileobj)
            {

                //int lotSize = Convert.ToInt32(GetLotSize(kite, csvinput.Strike1));
                
                int noofpivotrows =  csvinput.MaxlotPerPivot;
                int maxlot = csvinput.MaxLotSize / csvinput.MaxlotPerPivot;
                
                for (int i = 0; i < noofpivotrows; ++i)
                {
                    CsvInputFileObjectsPivot csvinputmulti = new CsvInputFileObjectsPivot();

                    //% will make the value repeat every n iteration
                    decimal pivotvalue = (i % pivotiteration) * pivotspread;
                    csvinputmulti.Pivot = csvinput.Pivot + pivotvalue;
                

                    //csvinputmulti.kite = csvinput.kite;
                    csvinputmulti.Strike1 = csvinput.Strike1;
                    csvinputmulti.Strike2 = csvinput.Strike2;                    
                    csvinputmulti.MaxlotPerPivot = 1;
                    csvinputmulti.MaxLotSize = maxlot;
                    csvinputmulti.PivotDelta = csvinput.PivotDelta + pivotvalue;

                    lstmultipivot.Add(csvinputmulti);


                }
            }
            //int noofpivots = csvinput.


            return lstmultipivot;
        }
        public void CreditDebitSpreadLTPDiff(Kite kite)
        {

            string strikeprice1 = "NFO:NIFTY20AUG10500PE";
            string strikeprice2 = "NFO:NIFTY20AUG10400PE";

            string strikeprice3 = "NFO:NIFTY20AUG11400CE";
            string strikeprice4 = "NFO:NIFTY20AUG11500CE";

            string strikeprice5 = "NFO:NIFTY20AUG11600CE";
            //string strikeprice6 = "NFO:NIFTY20AUG11600CE";

            string filename1 = string.Format("{0}-{1}_{2}", strikeprice1.Replace("NFO:", ""), strikeprice2.Replace("NFO:", ""),
                    DateTime.Now.ToString("dd-MM-yyyy"));
            string filename2 = string.Format("{0}-{1}_{2}", strikeprice3.Replace("NFO:", ""), strikeprice4.Replace("NFO:", ""),
                    DateTime.Now.ToString("dd-MM-yyyy"));

            string filename3 = string.Format("{0}-{1}_{2}", strikeprice4.Replace("NFO:", ""), strikeprice5.Replace("NFO:", ""),
                    DateTime.Now.ToString("dd-MM-yyyy"));


            Dictionary<string, Quote> quotes1 = kite.GetQuote(InstrumentId: new string[] { strikeprice1, strikeprice2 });


            string filename1path = string.Format(@"C:\Algotrade\creditspread\{0}.csv", filename1);
            string filename2path = string.Format(@"C:\Algotrade\creditspread\{0}.csv", filename2);
            string filename3path = string.Format(@"C:\Algotrade\creditspread\{0}.csv", filename3);
            string content1 = string.Format("DateTime,{0},{1},LTPDiff,NIFTY\n", strikeprice1.Replace("NFO:", ""), strikeprice2.Replace("NFO:", ""));
            string content2 = string.Format("DateTime,{0},{1},LTPDiff,NIFTY\n", strikeprice3.Replace("NFO:", ""), strikeprice4.Replace("NFO:", ""));
            string content3 = string.Format("DateTime,{0},{1},LTPDiff,NIFTY\n", strikeprice4.Replace("NFO:", ""), strikeprice5.Replace("NFO:", ""));

            //File.Delete(filename1path);
            //File.Delete(filename2path);
            //File.Delete(filename3path);
            //
            File.AppendAllText(filename1path, content1);
            File.AppendAllText(filename2path, content2);
            File.AppendAllText(filename2path, content3);

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                //get data 
                Dictionary<string, Quote> quotes = kite.GetQuote(InstrumentId: new string[] { strikeprice1, strikeprice2, strikeprice3, strikeprice4, strikeprice5, "NSE:NIFTY 50" });


                // get diff of strikeprice 1 and strikeprice2
                KeyValuePair<string, Quote> kvpquote1 = quotes.ElementAt(0);
                KeyValuePair<string, Quote> kvpquote2 = quotes.ElementAt(1);
                decimal ltpdiff1 = Math.Abs(kvpquote1.Value.LastPrice - kvpquote2.Value.LastPrice);


                // get diff of strikeprice 3 and strikeprice4
                KeyValuePair<string, Quote> kvpquote3 = quotes.ElementAt(2);
                KeyValuePair<string, Quote> kvpquote4 = quotes.ElementAt(3);
                decimal ltpdiff2 = Math.Abs(kvpquote3.Value.LastPrice - kvpquote4.Value.LastPrice);


                // get diff of strikeprice 5 and strikeprice6
                KeyValuePair<string, Quote> kvpquote5 = quotes.ElementAt(4);
                KeyValuePair<string, Quote> kvpquote6 = quotes.ElementAt(5);
                decimal ltpdiff3 = Math.Abs(kvpquote4.Value.LastPrice - kvpquote5.Value.LastPrice);

                string filecontent1 = string.Format("{0},{1},{2},{3},{4}\n", kvpquote1.Value.Timestamp.Value.ToString("dd-MM-yyyy hh:mm:ss"),
                                       kvpquote1.Value.LastPrice, kvpquote2.Value.LastPrice, ltpdiff1, kvpquote6.Value.LastPrice);
                string filecontent2 = string.Format("{0},{1},{2},{3},,{4}\n", kvpquote3.Value.Timestamp.Value.ToString("dd-MM-yyyy hh:mm:ss"),
                                        kvpquote3.Value.LastPrice, kvpquote4.Value.LastPrice, ltpdiff2, kvpquote6.Value.LastPrice);
                string filecontent3 = string.Format("{0},{1},{2},{3},{4}\n", kvpquote5.Value.Timestamp.Value.ToString("dd-MM-yyyy hh:mm:ss"),
                                       kvpquote4.Value.LastPrice, kvpquote5.Value.LastPrice, ltpdiff3, kvpquote6.Value.LastPrice);



                File.AppendAllText(filename1path, filecontent1);
                File.AppendAllText(filename2path, filecontent2);
                File.AppendAllText(filename3path, filecontent3);

                Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss") + "," + ltpdiff1.ToString() + "," + kvpquote6.Value.LastPrice);
                Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss") + "," + ltpdiff2.ToString() + "," + kvpquote6.Value.LastPrice);
                Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss") + "," + ltpdiff3.ToString() + "," + kvpquote6.Value.LastPrice);


                //create a copy of the file every hour
                if (DateTime.Now.Minute == 30 && DateTime.Now.Hour != 9)
                {
                    string fileCopy1path = string.Format(@"C:\Algotrade\creditspread\{0}_{1}.csv", filename1, DateTime.Now.ToString("hh_mm"));
                    string fileCopy2path = string.Format(@"C:\Algotrade\creditspread\{0}_{1}.csv", filename2, DateTime.Now.ToString("hh_mm"));
                    string fileCopy3path = string.Format(@"C:\Algotrade\creditspread\{0}_{1}.csv", filename3, DateTime.Now.ToString("hh_mm"));
                    File.Copy(filename1path, fileCopy1path);
                    File.Copy(filename2path, fileCopy2path);
                    File.Copy(filename3path, fileCopy3path);
                }

                Thread.Sleep(1000 * 60);
            }


        }
        public List<CsvInputFileSqoff> CsvFileReadSqOff(string csvfilepath)
        {
            List<CsvInputFileSqoff> lstcsvobj = new List<CsvInputFileSqoff>();
            try
            {
                using (TextReader reader = File.OpenText(csvfilepath))
                {
                    CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                    csv.Configuration.Delimiter = ",";
                    //csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.MissingFieldFound = null;
                    while (csv.Read())
                    {
                        CsvInputFileSqoff csvrecord = csv.GetRecord<CsvInputFileSqoff>();
                        lstcsvobj.Add(csvrecord);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return lstcsvobj;
            }
            return lstcsvobj;
        }

        public static string LogintoKite(string username, string password, string pin)
        {
            Kite kitenew = new Kite();
            Dictionary<string,dynamic> dictparam =kitenew.LoginToKiteWithPin(username, password, pin);
            string enctoken = dictparam["enctoken"].ToString();
            return enctoken;

            //File.AppendAllText(@"")
        }

        public List<CsvInputFileObjectsPivot> CsvFileInputData(string csvfilepath)
        {
            List<CsvInputFileObjectsPivot> lstcsvobj = new List<CsvInputFileObjectsPivot>();
            try
            {
                using (TextReader reader = File.OpenText(csvfilepath))
                {
                    CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                    csv.Configuration.Delimiter = ",";
                    //csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.MissingFieldFound = null;
                    while (csv.Read())
                    {
                        CsvInputFileObjectsPivot csvrecord = csv.GetRecord<CsvInputFileObjectsPivot>();
                        lstcsvobj.Add(csvrecord);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return lstcsvobj;
            }
            return lstcsvobj;
        }



    }//end of class
}
