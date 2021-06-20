using System;
using KiteConnect;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Linq;
using IIFLAPI;
using System.Net.Http;
using APIBridge;
using System.Text;

namespace KiteConnectSample
{
    class Program
    {
        // instances of Kite and Ticker
        static Ticker ticker;
        static Kite kite;

        // Initialize key and secret of your app
        static string MyAPIKey = ConfigurationManager.AppSettings["APIKey"];
        static string MySecret = ConfigurationManager.AppSettings["APISecret"];
        //static string MyUserId = "XV5542";

        // persist these data in settings or db or file
        static string MyPublicToken = "abcdefghijklmnopqrstuvwxyz";
        static string MyAccessToken = "abcdefghijklmnopqrstuvwxyz";

        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
        static  void Main(string[] args)
        {
            try
            {
                decimal totalbidprice = 123.50m;

               


                //string temp = totalperceinc <= 0 ? totalperceinc : +totalperceinc;
                OptionsData._broker = "iifl";
                //Login();
                //LoginKite lkite = new LoginKite();
                //lkite.Login();
                //initTicker();
                //RunCreditSpread();
                //OptionsData.BNLongStraddleStrategyLive();
                //OptionsData.BNShortStraddleStrategyLive(null);
                //OptionsData.ShortStraddleStrategyLive()
                //OptionsData.BNLongStrangleStrategyMultiStrikeLive();
                //RunSquareOffDelta();
                //OptionsData.BNLongStraddleStrategyMultiStrikeLive();
                //SellStock();
                //SellBuyStock();
                //RunBNShortStrangle();
                //RunScanforArbitrage();
                string instrumentId = "NIFTY 50";
                //RunOHLCBNOptionStrategy();
                //OptionsData.GetHistoricalData(instrumentId);


                //ParseWebhookData();
                OptionsData.BankNifty5minsScalperStrategy();
                //RunCITRiggerSquareOff();

                //RunShortSquareOffDelta();
                //RunScan();
                //BNOptionData();
                //OptionsData.Teststocksdeveloper();
                //OptionsData.LogintoKite("XV5542", "rishi321", "777777");

                //RunBNShortStraddle();


                //LoginIIFL();

                //int lotsize = Convert.ToInt32(optiondata.GetLotSize(kite, strikeprice1));



                //Console.WriteLine("Total Threads Running:" + lstthread.Count);



                //PositionResponse posresponse = kite.GetPositions();
                //List<Holding> holding = kite.GetHoldings();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        private static void RunOHLCBNOptionStrategy()
        {
            object obj= null;
            OptionsData.BankNiftyOHLCOptionStrategy(obj);

        }

        

        static void GetOptionGreeks(string accesstoken)
        {
            string socketurl= string.Format("wss://ws.kite.trade?api_key={0}&access_token={1}",MyAPIKey, MyAccessToken);
            //using (var ws = new))
            //{

            //}
        }
        //static string filename = string.Format("CI-Trigger-{0}.txt", DateTime.Now.ToString("dd-MM-yyyy"));
        ////static string webhookfilepath1 = @"C:\Algotrade\webhook\Trigger";
        //static string webhookfilepath = string.Format(@"C:\Algotrade\webhook\Trigger\{0}", filename);
        //static string webhookfolderwatch = string.Format(@"C:\Algotrade\webhook\Trigger");
        //static FileSystemWatcher fileSystemwatcher = new FileSystemWatcher(webhookfolderwatch);
        static int noofcalls = 0;
        public static void ParseWebhookData()
        {          
            string filename = string.Format("CI-Trigger-{0}.txt", DateTime.Now.ToString("dd-MM-yyyy"));
            string webhookfilepath = string.Format(@"C:\Algotrade\webhook\Trigger\{0}",filename);
            string webhookfolderwatch = string.Format(@"C:\Algotrade\webhook\Trigger");

            FileSystemWatcher fileSystemwatcher = new FileSystemWatcher(webhookfolderwatch);
            fileSystemwatcher.EnableRaisingEvents = true;
            fileSystemwatcher.Path = webhookfolderwatch;
            fileSystemwatcher.IncludeSubdirectories = false;
            fileSystemwatcher.Filter = "CI-Trigger*.txt";

            fileSystemwatcher.NotifyFilter = NotifyFilters.Attributes
                                   | NotifyFilters.CreationTime
                                   | NotifyFilters.DirectoryName
                                   | NotifyFilters.FileName
                                   | NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.Security
                                   | NotifyFilters.Size;

            fileSystemwatcher.Changed += FileSystemwatcher_Changed;
            fileSystemwatcher.Created += FileSystemwatcher_Changed;
           // fileSystemwatcher.Renamed += FileSystemwatcher_Changed;
            //fileSystemwatcher.Deleted += FileSystemwatcher_Changed;
            fileSystemwatcher.Error += FileSystemwatcher_Error;
            Console.WriteLine("Inside ParseWebhookData ,Press enter to exit.");
            Console.ReadLine();


        }

        private static void FileSystemwatcher_Error(object sender, ErrorEventArgs e)
        {
            //throw new NotImplementedException();
            string filename = string.Format("CI-TriggerError-{0}.txt", DateTime.Now.ToString("dd-MM-yyyy"));
            string webhookerrorfilepath = string.Format(@"C:\Algotrade\webhook\Trigger\{0}", filename);
            File.AppendAllText(webhookerrorfilepath, "Error");
      
            

        }
        public static object _synclock = new object();
        private static void FileSystemwatcher_Changed(object sender, FileSystemEventArgs e)
        {

            noofcalls++;
            string filename = string.Format("CI-Trigger-{0}.txt", DateTime.Now.ToString("dd-MM-yyyy"));
            //string webhookfilepath = e.FullPath;
            string webhookfilepath = string.Format(@"C:\Algotrade\webhook\Trigger\{0}", filename);
            OptionsData._broker = "iifl";
            string filename1 = string.Format(@"C:\Algotrade\webhook\Trigger\TriggerTime-{0}.txt", DateTime.Now.ToString("dd-MM-yyyy"));
            string content = string.Format("TriggerReceived-{0}-{1}\r\n", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), noofcalls);
            lock (_synclock)
            {
                File.AppendAllText(filename1, content);
                //if (e.ChangeType == WatcherChangeTypes.Changed)
                {
                    lock (_synclock)
                    {

                        //check if file is open or not

                        try
                        {
                            FileInfo file = new FileInfo(webhookfilepath);

                            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                            {

                                stream.Close();
                            }
                        }
                        catch (IOException iox)
                        {
                            //the file is unavailable because it is:
                            //still being written to
                            //or being processed by another thread
                            //or does not exist (has already been processed)
                            //return true;
                            Console.WriteLine(iox.Message);
                        }


                        string[] readtriggerfile = File.ReadAllLines(webhookfilepath);
                        OptionsData.ParseWebhookData(readtriggerfile);
             
                        
                        
                        //Console.WriteLine("Completed webhook parse method");
                    }
                }
            }
                        
        }
        private static void FileSystemwatcher_Created(object sender, FileSystemEventArgs e)
        {
            noofcalls++;
            string filename = string.Format("CI-Trigger-{0}.txt", DateTime.Now.ToString("dd-MM-yyyy"));
            string webhookfilepath = string.Format(@"C:\Algotrade\webhook\Trigger\{0}", filename);
            OptionsData._broker = "iifl";
            string filename1 = string.Format(@"C:\Algotrade\webhook\Trigger\TriggerTime-{0}.txt", DateTime.Now.ToString("dd-MM-yyyy"));
            string content = string.Format("TriggerReceived-{0}-{1}\r\n", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), noofcalls);
            
            string[] readtriggerfile = File.ReadAllLines(webhookfilepath);
            OptionsData.ParseWebhookData(readtriggerfile);
            


        }
        static void BuyStock()
        {
            string strikeprice1 = "NIFTY2082011600CE";
            string strikeprice2 = "NIFTY2082011550CE";

            try
            {
                //get data 
                Dictionary<string, Quote> quotes1 = kite.GetQuote(InstrumentId: new string[] { "NFO:" + strikeprice1 });
                Dictionary<string, Quote> quotes2 = kite.GetQuote(InstrumentId: new string[] { "NFO:" + strikeprice2 });

                KeyValuePair<string, Quote> kvpquote1 = quotes1.ElementAt(0);
                KeyValuePair<string, Quote> kvpquote2 = quotes2.ElementAt(0);

                List<DepthItem> lstbids1 = kvpquote1.Value.Bids;
                List<DepthItem> lstoffers1 = kvpquote1.Value.Offers;
                List<DepthItem> lstbids2 = kvpquote2.Value.Bids;
                List<DepthItem> lstoffers2 = kvpquote2.Value.Offers;
                int lotsize = 75;

                ///kite.GetQuote()
                Dictionary<string, dynamic> response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                       TradingSymbol: strikeprice1,
                       TransactionType: Constants.TRANSACTION_TYPE_BUY,
                       Quantity: lotsize*3,
                       Validity: Constants.VALIDITY_DAY,
                       Price: lstoffers1[0].Price,
                       OrderType: Constants.ORDER_TYPE_LIMIT,
                       ProductType: Constants.PRODUCT_NRML);

                // kite.CancelOrder()

                Dictionary<string, dynamic> response2 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                       TradingSymbol: strikeprice2,
                       TransactionType: Constants.TRANSACTION_TYPE_SELL,
                       Quantity: lotsize*3,
                       Validity: Constants.VALIDITY_DAY,
                       Price: lstbids2[0].Price,
                       OrderType: Constants.ORDER_TYPE_LIMIT,                       
                       ProductType: Constants.PRODUCT_NRML);

                dynamic status = "";
           
                dynamic dictresp = "";
                bool ret = response2.TryGetValue("data", out dictresp);
                Dictionary<string, object> keyvalues = (Dictionary<string, object>)dictresp;
                string orderid = keyvalues["order_id"].ToString();

                //List<Order> lstorder1 = kite.GetOrderHistory(orderid);
                List<Order> lstorder = kite.GetOrders();
                Order orderfind3 = lstorder.Find(f => f.OrderId == orderid);
                if (orderfind3.Status.ToLower() != "complete")
                {
                    Thread.Sleep(1000);
                    //try 10 times with 1 sec difference if the 2nd order is not successfule
                    for (int k = 0; k < 10; ++k)
                    {
                        Dictionary<string, dynamic> response3 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NFO,
                             TradingSymbol: strikeprice2,
                             TransactionType: Constants.TRANSACTION_TYPE_SELL,
                              Quantity: lotsize*2,
                              Price: lstbids2[0].Price,
                              Validity: Constants.VALIDITY_DAY,
                              OrderType: Constants.ORDER_TYPE_LIMIT,
                              ProductType: Constants.PRODUCT_NRML);



                        //
                        dynamic dictresp3 = "";
                        bool ret3 = response1.TryGetValue("data", out dictresp3);
                        Dictionary<string, object> keyvalues3 = (Dictionary<string, object>)dictresp3;
                        string orderid3 = keyvalues["order_id"].ToString();
                        List<Order> lstorder3 = kite.GetOrders();
                        orderfind3 = lstorder.Find(f => f.OrderId == orderid);
                        if (orderfind3.Status.ToLower() == "success")
                        {
                            break;
                        }
                        Console.WriteLine(k + " try");
                        Thread.Sleep(1000);
                    }

                }

                if (orderfind3.Status.ToLower() == "complete")
                {
                    Console.WriteLine("Both Orders Executed");
                }
                else
                {
                    Console.WriteLine(orderfind3.StatusMessage);
                }
                

                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            
        }

        static void SellBuyStock()
        {
            Kite kite = new Kite();
            //kite.GetPositions()
            OptionsData.GetZerodhaNewInteractiveAccessToken();
            kite.SetAccessToken(OptionsData._zerodhanewAccessToken);
            Dictionary<string, dynamic> response1 = kite.PlaceOrder(Exchange: Constants.EXCHANGE_NSE,
                   TradingSymbol: "AMARAJABAT",
                   TransactionType: Constants.TRANSACTION_TYPE_BUY,
                   Quantity: 1,
                   Price: 700.0m,
                   Validity: Constants.VALIDITY_DAY,
                   OrderType: Constants.ORDER_TYPE_LIMIT,
                   ProductType: Constants.PRODUCT_CNC, UserId:"XV5542");

            dynamic dictresp = "";
            bool ret = response1.TryGetValue("data", out dictresp);
            Dictionary<string, object> keyvalues = (Dictionary<string, object>)dictresp;
            string orderid = keyvalues["order_id"].ToString();

            List<Order> lstorder1 = kite.GetOrderHistory(orderid);
            List<Order> lstorder = kite.GetOrders();
            Order orderfind = lstorder.Find(f => f.OrderId == orderid);

            //Order or1 = lstorder.Find(i1 => i1.OrderId == "845524");

            //foreach (Order order in lstorder)
            //{
            //    Console.WriteLine(order.OrderId + "," + order.AveragePrice + "," + order.Tradingsymbol);
            //    List<Order> lstorder2= kite.GetOrderHistory(order.OrderId);
            //}

            // kite.GetOrderTrades()
            int i = 0;
            //foreach (KeyValuePair<string, dynamic> kvp in response1)
            //{
            //    if (i == 0)
            //    {
            //        string key = kvp.Key;
            //        string value = kvp.Value;
            //    }
            //    else
            //    {
                
            //    }
            //   // Console.WriteLine(key + "," + value);
            //    ++i;
            //}

        }



        public static  void Login()
        {

            kite = new Kite(MyAPIKey, Debug: false);

            // For handling 403 errors

            kite.SetSessionExpiryHook(OnTokenExpire);

            // Initializes the login flow            

            string redirectURL = string.Format("http://{0}:{1}/", IPAddress.Loopback, GetRandomUnusedPort());

            Console.WriteLine("Redirect URL: " + redirectURL);

            // Generate kite login url with the above redirect url
            string loginURL = string.Format("{0}&redirect_url={1}", kite.GetLoginURL(), redirectURL);

            Console.WriteLine("Login URL: " + loginURL);

            // Start local http server for catching the redirect url
            var http = new HttpListener();
            http.Prefixes.Add(redirectURL);
            http.Start();


            // Launch default browser with login url
            Process.Start(new ProcessStartInfo(loginURL)
            { UseShellExecute = true });

            // Wait until login is complete and redirect url is launched
            var context =  http.GetContext();

            // Send a response back to browser
            var response = context.Response;
            string responseString = string.Format("Authentication Successful");
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>
            {
                responseOutput.Close();
                http.Stop();
                Console.WriteLine("HTTP server stopped.");
            });

            // Collect request token from the redirect url
            var requestToken = context.Request.QueryString.Get("request_token");
            //
            try
            {
                initSession(requestToken);
            }
            catch (Exception e)
            {
                // Cannot continue without proper authentication
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
            //
            kite.SetAccessToken(MyAccessToken);
            
            // Initialize ticker

           
            //buy something


            //OptionsData optiondatobj = new OptionsData();
            //optiondatobj.GetOptionsData(kite);

            // Holdings

            /* List<Holding> holdings = kite.GetHoldings();

             Console.WriteLine("TradingSymbol T1Quantity");
             Console.WriteLine("------------------------");

             foreach (Holding holding in holdings)
             {
                 Console.WriteLine(holding.TradingSymbol + " " + holding.T1Quantity);
             } 
             Console.WriteLine(Utils.JsonSerialize(holdings[0]));

             // Get quotes of upto 200 scrips

             Dictionary<string, Quote> quotes = kite.GetQuote(InstrumentId: new string[] { "NSE:INFY","NSE:LUPIN" });
             //kite.
             Console.WriteLine(Utils.JsonSerialize(quotes));

             // Get all GTTs

             List<GTT> gtts = kite.GetGTTs();
             Console.WriteLine(gtts.Count > 0 ? Utils.JsonSerialize(gtts[0]) : "");

             // Get GTT by Id

             GTT gtt = kite.GetGTT(99691);
             Console.WriteLine(Utils.JsonSerialize(gtt));

             // Cacncel GTT by Id

             var gttCancelResponse = kite.CancelGTT(1582);
             Console.WriteLine(Utils.JsonSerialize(gttCancelResponse));

             // Place GTT

             GTTParams gttParams = new GTTParams();
             gttParams.TriggerType = Constants.GTT_TRIGGER_OCO;
             gttParams.Exchange = "NSE";
             gttParams.TradingSymbol = "SBIN";
             gttParams.LastPrice = 288.9m;

             List<decimal> triggerPrices = new List<decimal>();
             triggerPrices.Add(260m);
             triggerPrices.Add(320m);
             gttParams.TriggerPrices = triggerPrices;

             // Only sell is allowed for OCO or two-leg orders.
             // Single leg orders can be buy or sell order.
             // Passing a last price is mandatory.
             // A stop-loss order must have trigger and price below last price and target order must have trigger and price above last price.
             // Only limit order type  and CNC product type is allowed for now.

             GTTOrderParams order1Params = new GTTOrderParams();
             order1Params.OrderType = Constants.ORDER_TYPE_LIMIT;
             order1Params.Price = 250m;
             order1Params.Product = Constants.PRODUCT_CNC;
             order1Params.TransactionType = Constants.TRANSACTION_TYPE_SELL;
             order1Params.Quantity = 0;

             GTTOrderParams order2Params = new GTTOrderParams();
             order2Params.OrderType = Constants.ORDER_TYPE_LIMIT;
             order2Params.Price = 320m;
             order2Params.Product = Constants.PRODUCT_CNC;
             order2Params.TransactionType = Constants.TRANSACTION_TYPE_SELL;
             order2Params.Quantity = 1;

             // Target or upper trigger
             List<GTTOrderParams> ordersList = new List<GTTOrderParams>();
             ordersList.Add(order1Params);
             ordersList.Add(order2Params);
             gttParams.Orders = ordersList;

             var placeGTTResponse = kite.PlaceGTT(gttParams);
             Console.WriteLine(Utils.JsonSerialize(placeGTTResponse));

             var modifyGTTResponse = kite.ModifyGTT(407301, gttParams);
             Console.WriteLine(Utils.JsonSerialize(modifyGTTResponse));

             // Positions

             PositionResponse positions = kite.GetPositions();
             Console.WriteLine(Utils.JsonSerialize(positions.Net[0]));

             kite.ConvertPosition(
                 Exchange: Constants.EXCHANGE_NSE,
                 TradingSymbol: "ASHOKLEY",
                 TransactionType: Constants.TRANSACTION_TYPE_BUY,
                 PositionType: Constants.POSITION_DAY,
                 Quantity: 1,
                 OldProduct: Constants.PRODUCT_MIS,
                 NewProduct: Constants.PRODUCT_CNC
             );


             // Instruments

             List<Instrument> instruments = kite.GetInstruments();
             Console.WriteLine(Utils.JsonSerialize(instruments[0]));

             // Get quotes of upto 200 scrips

             Dictionary<string, Quote> quotes1 = kite.GetQuote(InstrumentId: new string[] { "NSE:INFY", "NSE:ASHOKLEY" });
             Console.WriteLine(Utils.JsonSerialize(quotes1));

             // Get OHLC and LTP of upto 200 scrips

             Dictionary<string, OHLC> ohlcs = kite.GetOHLC(InstrumentId: new string[] { "NSE:INFY", "NSE:ASHOKLEY" });
             Console.WriteLine(Utils.JsonSerialize(ohlcs));

             // Get LTP of upto 200 scrips

             Dictionary<string, LTP> ltps = kite.GetLTP(InstrumentId: new string[] { "NSE:INFY", "NSE:ASHOKLEY" });
             Console.WriteLine(Utils.JsonSerialize(ltps));

             // Trigger Range

             Dictionary<string, TrigerRange> triggerRange = kite.GetTriggerRange(
                 InstrumentId: new string[] { "NSE:ASHOKLEY" },
                 TrasactionType: Constants.TRANSACTION_TYPE_BUY
             );
             Console.WriteLine(Utils.JsonSerialize(triggerRange));

             // Get all orders

             List<Order> orders = kite.GetOrders();
             Console.WriteLine(Utils.JsonSerialize(orders[0]));

             // Get order by id

             List<Order> orderinfo = kite.GetOrderHistory("1234");
             Console.WriteLine(Utils.JsonSerialize(orderinfo[0]));

             // Place sell order

             Dictionary<string, dynamic> response1 = kite.PlaceOrder(
                 Exchange: Constants.EXCHANGE_CDS,
                 TradingSymbol: "USDINR17AUGFUT",
                 TransactionType: Constants.TRANSACTION_TYPE_SELL,
                 Quantity: 1,
                 Price: 64.0000m,
                 OrderType: Constants.ORDER_TYPE_MARKET,
                 Product: Constants.PRODUCT_MIS
             );
             Console.WriteLine("Order Id: " + response1["data"]["order_id"]);

             // Place buy order

             kite.PlaceOrder(
                 Exchange: Constants.EXCHANGE_CDS,
                 TradingSymbol: "USDINR17AUGFUT",
                 TransactionType: Constants.TRANSACTION_TYPE_BUY,
                 Quantity: 1,
                 Price: 63.9000m,
                 OrderType: Constants.ORDER_TYPE_LIMIT,
                 Product: Constants.PRODUCT_MIS
             );

             // Cancel order by id

             kite.CancelOrder("1234");

             //BO LIMIT order placing

             kite.PlaceOrder(
                 Exchange: Constants.EXCHANGE_NSE,
                 TradingSymbol: "ASHOKLEY",
                 TransactionType: Constants.TRANSACTION_TYPE_BUY,
                 Quantity: 1,
                 Price: 115,
                 Product: Constants.PRODUCT_MIS,
                 OrderType: Constants.ORDER_TYPE_LIMIT,
                 Validity: Constants.VALIDITY_DAY,
                 SquareOffValue: 2,
                 StoplossValue: 2,
                 Variety: Constants.VARIETY_BO
             );

             // BO LIMIT exiting

             kite.CancelOrder(
                 OrderId: "1234",
                 Variety: Constants.VARIETY_BO,
                 ParentOrderId: "5678"
             );

             // BO SL order placing

             kite.PlaceOrder(
                 Exchange: Constants.EXCHANGE_NSE,
                 TradingSymbol: "ASHOKLEY",
                 TransactionType: Constants.TRANSACTION_TYPE_BUY,
                 Quantity: 1,
                 Price: 117,
                 Product: Constants.PRODUCT_MIS,
                 OrderType: Constants.ORDER_TYPE_SL,
                 Validity: Constants.VALIDITY_DAY,
                 SquareOffValue: 2,
                 StoplossValue: 2,
                 TriggerPrice: 117.5m,
                 Variety: Constants.VARIETY_BO
             );

             // BO SL exiting

             kite.CancelOrder(
                OrderId: "1234",
                Variety: Constants.VARIETY_BO,
                ParentOrderId: "5678"
            );

             // CO LIMIT order placing

             kite.PlaceOrder(
                 Exchange: Constants.EXCHANGE_NSE,
                 TradingSymbol: "ASHOKLEY",
                 TransactionType: Constants.TRANSACTION_TYPE_BUY,
                 Quantity: 1,
                 Price: 115.5m,
                 Product: Constants.PRODUCT_MIS,
                 OrderType: Constants.ORDER_TYPE_LIMIT,
                 Validity: Constants.VALIDITY_DAY,
                 TriggerPrice: 116.5m,
                 Variety: Constants.VARIETY_CO
             );

             // CO LIMIT exiting

             kite.CancelOrder(
                OrderId: "1234",
                Variety: Constants.VARIETY_BO,
                ParentOrderId: "5678"
            );

             // CO MARKET order placing

             kite.PlaceOrder(
                 Exchange: Constants.EXCHANGE_NSE,
                 TradingSymbol: "ASHOKLEY",
                 TransactionType: Constants.TRANSACTION_TYPE_BUY,
                 Quantity: 1,
                 Product: Constants.PRODUCT_MIS,
                 OrderType: Constants.ORDER_TYPE_MARKET,
                 Validity: Constants.VALIDITY_DAY,
                 TriggerPrice: 116.5m,
                 Variety: Constants.VARIETY_CO
             );

             // CO MARKET exiting

             kite.CancelOrder(
                 OrderId: "1234",
                 Variety: Constants.VARIETY_BO,
                 ParentOrderId: "5678"
             );

             // Trades

             List<Trade> trades = kite.GetOrderTrades("1234");
             Console.WriteLine(Utils.JsonSerialize(trades[0]));

             // Margins

             UserMargin commodityMargins = kite.GetMargins(Constants.MARGIN_COMMODITY);
             UserMargin equityMargins = kite.GetMargins(Constants.MARGIN_EQUITY);

             // Historical Data With Dates

             List<Historical> historical = kite.GetHistoricalData(
                 InstrumentToken: "5633",
                 FromDate: new DateTime(2016, 1, 1, 12, 50, 0),   // 2016-01-01 12:50:00 AM
                 ToDate: new DateTime(2016, 1, 1, 13, 10, 0),    // 2016-01-01 01:10:00 PM
                 Interval: Constants.INTERVAL_MINUTE,
                 Continuous: false
             );
             Console.WriteLine(Utils.JsonSerialize(historical[0]));

             // Mutual Funds Instruments

             List<MFInstrument> mfinstruments = kite.GetMFInstruments();
             Console.WriteLine(Utils.JsonSerialize(mfinstruments[0]));

             // Mutual funds get all orders

             List<MFOrder> mforders = kite.GetMFOrders();
             Console.WriteLine(Utils.JsonSerialize(mforders[0]));

             // Mutual funds get order by id

             MFOrder mforder = kite.GetMFOrders(OrderId: "1234");
             Console.WriteLine(Utils.JsonSerialize(mforder));

             // Mutual funds place order

             kite.PlaceMFOrder(
                 TradingSymbol: "INF174K01LS2",
                 TransactionType: Constants.TRANSACTION_TYPE_BUY,
                 Amount: 20000
             );

             // Mutual funds cancel order by id

             kite.CancelMFOrder(OrderId: "1234");

             // Mutual Funds get all SIPs

             List<MFSIP> mfsips = kite.GetMFSIPs();
             Console.WriteLine(Utils.JsonSerialize(mfsips[0]));

             // Mutual Funds get SIP by id

             MFSIP sip = kite.GetMFSIPs("63429");
             Console.WriteLine(Utils.JsonSerialize(sip));

             // Mutual Funds place SIP order

             kite.PlaceMFSIP(
                 TradingSymbol: "INF174K01LS2",
                 Amount: 1000,
                 InitialAmount: 5000,
                 Frequency: "monthly",
                 InstalmentDay: 1,
                 Instalments: -1 // -1 means infinite
             );

             // Mutual Funds modify SIP order

             kite.ModifyMFSIP(
                 SIPId: "1234",
                 Amount: 1000,
                 Frequency: "monthly",
                 InstalmentDay: 1,
                 Instalments: 10,
                 Status: "paused"
             );

             kite.CancelMFSIP(SIPId: "1234");

             // Mutual Funds Holdings

             List<MFHolding> mfholdings = kite.GetMFHoldings();
             Console.WriteLine(Utils.JsonSerialize(mfholdings[0]));

             Console.ReadKey();

             // Disconnect from ticker
             */
            //ticker.Close();
        }

        public static void LoginIIFL()
        {
            try
            {
                IIFLConnect iiflconnect = new IIFLConnect();
                IIFLUser user = iiflconnect.GenerateSessionMarket();
                iiflconnect.SetAccessToken(user.AccessToken);
                Dictionary<string, dynamic> dictinstrument = iiflconnect.ParseTradingSymbol("BANKNIFTY24Sep202025000CE");
                Dictionary<string, dynamic> dictinstrument1 = iiflconnect.ParseTradingSymbol("BANKNIFTY24Sep202026000CE");

                IIFLInstrument instrumentData = iiflconnect.GetInstrumentID("marketdata.optionsymbol", dictinstrument);
                IIFLInstrument instrumentData1 = iiflconnect.GetInstrumentID("marketdata.optionsymbol", dictinstrument1);
                iiflconnect.GetQuote(instrumentData.ExchangeInstrumentID.ToString());
                //call interactive
                iiflconnect.SetAccessToken("");
                IIFLUser user1 = iiflconnect.GenerateSessionInteractive();
                iiflconnect.SetAccessToken(user1.AccessToken);


                Dictionary<string, dynamic> dictorder = iiflconnect.PlaceOrder(Exchange: "NSEFO",
                                                                             Quantity: Convert.ToInt32(instrumentData.LotSize) * 8,
                                                                              ExchangeInstrumentId: instrumentData.ExchangeInstrumentID.ToString(),
                                                                              ProductType: IIFLConstants.PRODUCTTYPE_NRML,
                                                                                 OrderType: IIFLConstants.ORDER_TYPE_LIMIT,
                                                                                 TransactionType: IIFLConstants.ORDERSIDE_SELL,
                                                                                  Validity: IIFLConstants.TIMEINFORCE_DAY,
                                                                                  Price: 50m,
                                                                                  StoplossValue: 0,
                                                                                  DisclosedQuantity: 0);
                int i = 10;
                //place order
                //Dictionary<string, dynamic> dictorder1 = iiflconnect.PlaceOrder(Exchange: "NSEFO",
                //                                                             Quantity: Convert.ToInt32(instrumentData1.LotSize)*19,
                //                                                              ExchangeInstrumentId: instrumentData1.ExchangeInstrumentID.ToString(),
                //                                                              ProductType: IIFLConstants.PRODUCTTYPE_NRML,
                //                                                                 OrderType: IIFLConstants.ORDER_TYPE_LIMIT,
                //                                                                 TransactionType: IIFLConstants.ORDERSIDE_BUY,
                //                                                                  Validity: IIFLConstants.TIMEINFORCE_DAY,
                //                                                                  Price: 108,
                //                                                                  StoplossValue: 0,
                //                                                                  DisclosedQuantity: 0);
                //place order

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void BNOptionData()
        {

            CsvInputFileObjectsPivot csvinputfileobj = new CsvInputFileObjectsPivot();
            DirectoryInfo dirinfo = new DirectoryInfo(@"C:\Algotrade\inputNiftyBNcsvfile");
            FileInfo[] fiinfoarr = dirinfo.GetFiles();

            List<Thread> lstthread = new List<Thread>();
            OptionsData optiondata = new OptionsData();
            //Read all the info from csv file
            int i = 0;
            string dirpath = @"C:\Algotrade\scalperoutput";
            DirectoryInfo directory = new DirectoryInfo(dirpath);
            //delete existing files
            foreach (FileInfo file in directory.GetFiles())
            {
                //file.Delete();
            }
           
            try
            {
                foreach (FileInfo fiinfo in fiinfoarr)
                {                   

                    List<CsvInputFileObjectsPivot> lstfileobj = optiondata.CsvFileInputData(fiinfo.FullName);
                    Console.WriteLine(fiinfo.FullName);

                    Thread[] tharr = new Thread[lstfileobj.Count];
                    
                    foreach (CsvInputFileObjectsPivot csvInputFileObjects in lstfileobj)
                    {
                        if(fiinfo.Name.Contains("BN"))
                        {
                            csvInputFileObjects.Index = "BANKNIFTY";
                        }
                        else
                        {
                            csvInputFileObjects.Index = "NIFTY";
                        }
                        //csvInputFileObjects.kite = kite;
                        tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.BankNiftyStrikeOptionData));
                        tharr[i].Name = "Thread" + i.ToString();
                        tharr[i].Start(csvInputFileObjects);
                        lstthread.Add(tharr[i]);

                        ++i;
                        Console.WriteLine("count:" + i);
                        Thread.Sleep(200);//adding sleep so that each thread starttime is different and all threads dont call the API in the same time

                    }
                }

                Console.WriteLine("Total Theads Running BN Options:" + lstthread.Count);

                //squareoff 

                for (int j = 0; j < lstthread.Count; j++)
                {
                    lstthread[j].Join();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                File.AppendAllText(@"C:\Algotrade\scalperoutput\mainErrorLog.txt", ex.StackTrace);
            }
           
            Console.WriteLine("Completed");
        }

        public static void RunBNShortStraddle()
        {

            CsvInputFileObjectsPivot csvinputfileobj = new CsvInputFileObjectsPivot();
            DirectoryInfo dirinfo = new DirectoryInfo(@"C:\Algotrade\inputcsvfile");
            FileInfo[] fiinfoarr = dirinfo.GetFiles();

            List<Thread> lstthread = new List<Thread>();
            OptionsData optiondata = new OptionsData();
            //Read all the info from csv file
            int i = 0;
            string dirpath = @"C:\Algotrade\scalperoutput";
            DirectoryInfo directory = new DirectoryInfo(dirpath);
            //delete existing files
            foreach (FileInfo file in directory.GetFiles())
            {
                //file.Delete();
            }

            try
            {
                foreach (FileInfo fiinfo in fiinfoarr)
                {

                    List<CsvInputFileObjectsPivot> lstfileobj = optiondata.CsvFileInputData(fiinfo.FullName);

                    Thread[] tharr = new Thread[lstfileobj.Count];
                    foreach (CsvInputFileObjectsPivot csvInputFileObjects in lstfileobj)
                    {
                        csvInputFileObjects.kite = kite;
                        tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.BNShortStraddleStrategyLive));
                        tharr[i].Name = "Thread" + i.ToString();
                        tharr[i].Start(csvInputFileObjects);
                        lstthread.Add(tharr[i]);

                        ++i;
                        //Thread.Sleep(200);//adding sleep so that each thread starttime is different and all threads dont call the API in the same time

                    }
                }

                Console.WriteLine("Total Theads Running BN ShortStraddle:" + lstthread.Count);

                //squareoff 

                for (int j = 0; j < lstthread.Count; j++)
                {
                    lstthread[j].Join();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                File.AppendAllText(@"C:\Algotrade\scalperoutput\mainErrorLog.txt", ex.StackTrace);
            }

            Console.WriteLine("Completed");
        }

        public static void RunCreditSpread()
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            CsvInputFileObjectsPivot csvinputfileobj = new CsvInputFileObjectsPivot();
            DirectoryInfo dirinfo = new DirectoryInfo(@"C:\Algotrade\inputcsvfile");
            FileInfo[] fiinfoarr = dirinfo.GetFiles();
           
                

            List<Thread> lstthread = new List<Thread>();
            OptionsData optiondata = new OptionsData();
            //Read all the info from csv file
            int i = 0;
            //OptionsData.GetIIFLMarketAccessToken();
            //OptionsData.GetIIFLInteractiveAccessToken();
            //string outputpivotfilename = @"C:\Algotrade\inputcsvfile\outputpivot.csv";
            string dirpath = @"C:\Algotrade\scalperoutput";
            DirectoryInfo directory = new DirectoryInfo(dirpath);
            //delete existing files
            foreach (FileInfo file in directory.GetFiles())
            {
                //file.Delete();
            }
            if(OptionsData._broker=="iifl")
            {
                //OptionsData.GetIIFLInteractiveAccessToken();
                //OptionsData.GetIIFLMarketAccessToken();
            }
            try
            {
                foreach (FileInfo fiinfo in fiinfoarr)
                {
                    List<CsvInputFileObjectsPivot> lstfileobj = optiondata.CreateMultiPivots(fiinfo.FullName, 0.20m, kite);

                    #region multipivotfile
                    //optiondata.CreditDebitSpreadStrategy(kite, strikeprice1, strikeprice2, startpivot, deltapivot, maxlots, lotsize);

                    //List<CsvFileObjects> lstfileobj = optiondata.CsvFileRead(@"C:\Algotrade\Algostrategy\Nifty11600_11700CE.csv");
                    //optiondata.CreditDebitSpreadStrategy(kite, strikeprice1, strikeprice2, startpivot, deltapivot, maxlots, lotsize);
                    //File.AppendAllText(outputpivotfilename,"Strike1, Strike2,Pivot, PivotDelta ,lotSize, MaxLotSize\n");
                    //foreach (CsvInputFileObjectsPivot csvInputFileObjects in lstfileobj)
                    //{
                    //    string content = string.Format("{0},{1},{2},{3},{4},{5}\n", csvInputFileObjects.Strike1, csvInputFileObjects.Strike2, csvInputFileObjects.Pivot, csvInputFileObjects.PivotDelta, csvInputFileObjects.lotSize, csvInputFileObjects.MaxLotSize);
                    //    File.AppendAllText(outputpivotfilename, content);
                    //}
                    #endregion

                    Thread[] tharr = new Thread[lstfileobj.Count];
                    foreach (CsvInputFileObjectsPivot csvInputFileObjects in lstfileobj)
                    {
                        csvInputFileObjects.kite = kite;
                        tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.CreditSpreadStrategyLive));
                        tharr[i].Name = "Thread" + i.ToString();
                        tharr[i].Start(csvInputFileObjects);
                        lstthread.Add(tharr[i]);

                        ++i;
                        //Thread.Sleep(200);//adding sleep so that each thread starttime is different and all threads dont call the API in the same time

                    }
                }

                Console.WriteLine("Total Theads Running CreditSpread:" + lstthread.Count);

                //squareoff 

                for (int j = 0; j < lstthread.Count; j++)
                {
                    lstthread[j].Join();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                File.AppendAllText(@"C:\Algotrade\scalperoutput\mainErrorLog.txt", ex.StackTrace);
            }

            //for (int j = 0; j < tharr.Length; i++)
            //{
            //    tharr[i].Join();
            //}
            Console.WriteLine("Completed");

        }

        public static void RunSquareOffDelta()
        {
            
            //squareoff
            CsvInputFileSqoff csvinputsqoffobj = new CsvInputFileSqoff();
            DirectoryInfo dirinfo1 = new DirectoryInfo(@"C:\Algotrade\squareofffile");
            
            List<Thread> lstthread = new List<Thread>();
            OptionsData optiondata = new OptionsData();

            FileInfo[] fiinfoarr1 = dirinfo1.GetFiles();
            //Read all the info from csv file
            string inputfilepath = @"C:\Algotrade\squareofffile\Sq_Off_file.csv";

            //foreach (FileInfo fiinfo in fiinfoarr1)
            {
                List<CsvInputFileSqoff> lstfileobj = optiondata.CsvFileReadSqOff(inputfilepath);
                //optiondata.CreditDebitSpreadStrategy(kite, strikeprice1, strikeprice2, startpivot, deltapivot, maxlots, lotsize);

                int i = 0;
                Thread[] tharr = new Thread[lstfileobj.Count];
                foreach (CsvInputFileSqoff csvInputFileSqOff in lstfileobj)
                {
                    //csvInputFileSqOff.kite = kite;
                    if (csvInputFileSqOff.SqoffPrice == 0)
                    {
                        tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.BNLongSquareOffPercentTrail));
                        tharr[i].Name = "Thread" + i.ToString();
                        tharr[i].Start(csvInputFileSqOff);
                        lstthread.Add(tharr[i]);
                    }
                    else
                    {
                        tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.BNLongSquareOffDelta));
                        tharr[i].Name = "Thread" + i.ToString();
                        tharr[i].Start(csvInputFileSqOff);
                        lstthread.Add(tharr[i]);

                    }
                    ++i;
                    Thread.Sleep(2000);//adding sleep so that each thread starttime is different and all threads dont call the API in the same time

                }

                for (int j = 0; j < lstthread.Count; j++)
                {
                    lstthread[j].Join();
                }

                Console.WriteLine("Completed");
            }


        }

        public static void RunCITRiggerSquareOff()
        {

            //squareoff
            CsvInputFileSqoff csvinputsqoffobj = new CsvInputFileSqoff();
            DirectoryInfo dirinfo1 = new DirectoryInfo(@"C:\Algotrade\squareofffile");

            List<Thread> lstthread = new List<Thread>();
            OptionsData optiondata = new OptionsData();

            FileInfo[] fiinfoarr1 = dirinfo1.GetFiles();
            //Read all the info from csv file
            string inputfilepath = @"C:\Algotrade\squareofffile\sqoff_CITrigger.csv";

            //foreach (FileInfo fiinfo in fiinfoarr1)
            {
                List<CsvInputFileSqoff> lstfileobj = optiondata.CsvFileReadSqOff(inputfilepath);
                //optiondata.CreditDebitSpreadStrategy(kite, strikeprice1, strikeprice2, startpivot, deltapivot, maxlots, lotsize);

                int i = 0;
                Thread[] tharr = new Thread[lstfileobj.Count];
                foreach (CsvInputFileSqoff csvInputFileSqOff in lstfileobj)
                {
                    //csvInputFileSqOff.kite = kite;
                    if (csvInputFileSqOff.StrategyType.ToLower() == "credit")
                    {
                        tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.ChartInkTriggerCreditSquareOff));
                        tharr[i].Name = "Thread" + i.ToString();
                        tharr[i].Start(csvInputFileSqOff);
                        lstthread.Add(tharr[i]);
                    }
                    else if(csvInputFileSqOff.StrategyType.ToLower() =="debit")
                    {
                        tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.ChartInkTriggerDebitSquareOff));
                        tharr[i].Name = "Thread" + i.ToString();
                        tharr[i].Start(csvInputFileSqOff);
                        lstthread.Add(tharr[i]);

                    }
                    else if (csvInputFileSqOff.StrategyType.ToLower() == "fut")
                    {
                        tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.ChartInkTriggerFutureSquareOff));
                        tharr[i].Name = "Thread" + i.ToString();
                        tharr[i].Start(csvInputFileSqOff);
                        lstthread.Add(tharr[i]);

                    }
                    ++i;
                    Thread.Sleep(2000);//adding sleep so that each thread starttime is different and all threads dont call the API in the same time

                }

                for (int j = 0; j < lstthread.Count; j++)
                {
                    lstthread[j].Join();
                }
                Console.ReadLine();
                Console.WriteLine("Completed");
            }


        }

        public static void RunShortSquareOffDelta()
        {

            //squareoff
            CsvInputFileSqoff csvinputsqoffobj = new CsvInputFileSqoff();
            DirectoryInfo dirinfo1 = new DirectoryInfo(@"C:\Algotrade\squareofffile");

            List<Thread> lstthread = new List<Thread>();
            OptionsData optiondata = new OptionsData();

            FileInfo[] fiinfoarr1 = dirinfo1.GetFiles();
            //Read all the info from csv file
            string inputfilepath = @"C:\Algotrade\squareofffile\Sq_Off_file1.csv";

            //foreach (FileInfo fiinfo in fiinfoarr1)
            {
                List<CsvInputFileSqoff> lstfileobj = optiondata.CsvFileReadSqOff(inputfilepath);
                //optiondata.CreditDebitSpreadStrategy(kite, strikeprice1, strikeprice2, startpivot, deltapivot, maxlots, lotsize);

                int i = 0;
                Thread[] tharr = new Thread[lstfileobj.Count];
                foreach (CsvInputFileSqoff csvInputFileSqOff in lstfileobj)
                {
                    //csvInputFileSqOff.kite = kite;


                    tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.BNShortSquareOffDelta));
                    tharr[i].Name = "Thread" + i.ToString();
                    tharr[i].Start(csvInputFileSqOff);
                    lstthread.Add(tharr[i]);
               
                    ++i;
                    Thread.Sleep(2000);//adding sleep so that each thread starttime is different and all threads dont call the API in the same time

                }

                for (int j = 0; j < lstthread.Count; j++)
                {
                    lstthread[j].Join();
                }

                Console.WriteLine("Completed");
            }


        }

        public static void RunBNShortStrangle()
        {

            //squareoff
            CsvInputFileSqoff csvinputsqoffobj = new CsvInputFileSqoff();
            DirectoryInfo dirinfo1 = new DirectoryInfo(@"C:\Algotrade\squareofffile");

            List<Thread> lstthread = new List<Thread>();
            OptionsData optiondata = new OptionsData();

            FileInfo[] fiinfoarr1 = dirinfo1.GetFiles();
            //Read all the info from csv file
            string inputfilepath = @"C:\Algotrade\squareofffile\Sq_Off_file.csv";

            //foreach (FileInfo fiinfo in fiinfoarr1)
            {
                List<CsvInputFileSqoff> lstfileobj = optiondata.CsvFileReadSqOff(inputfilepath);
                //optiondata.CreditDebitSpreadStrategy(kite, strikeprice1, strikeprice2, startpivot, deltapivot, maxlots, lotsize);

                //int i = 0;
                Thread[] tharr = new Thread[12];
                int bnstrikedelta = 600;
                try
                {
                    for (int i = 0; i < 2; ++i)
                    {
                        //csvInputFileSqOff.kite = kite;
                   
                        tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.BNLongStrangleStrategyLive));
                        tharr[i].Name = "Thread" + i.ToString();
                        tharr[i].Start(bnstrikedelta);
                        lstthread.Add(tharr[i]);
                        bnstrikedelta += 100;

                        Thread.Sleep(3000);//adding sleep so that each thread starttime is different and all threads dont call the API in the same time

                    }

                    int niftystrikedelta = 400;
                    for (int i = 0; i < 2; ++i)
                    {
                        //csvInputFileSqOff.kite = kite;

                        tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.NiftyShortStrangleStrategyLive));
                        tharr[i].Name = "Thread" + i.ToString();
                        tharr[i].Start(niftystrikedelta);
                        lstthread.Add(tharr[i]);
                        niftystrikedelta += 100;

                        Thread.Sleep(3000);//adding sleep so that each thread starttime is different and all threads dont call the API in the same time

                    }

                    for (int j = 0; j < lstthread.Count; j++)
                    {
                        lstthread[j].Join();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Completed");
            }


        }
        /// <summary>
        /// Zero Loss Strategy
        /// </summary>
        public static void RunScanforArbitrage()
        {

            //List<Thread> lstthread = new List<Thread>();
            Thread[] tharr = new Thread[2];
            object inputobj = "";
            //tharr[0] = new Thread(new ParameterizedThreadStart(OptionsData.ZerolossStrategyScanner));
            //tharr[0].Name = "Thread0";
            //tharr[0].Start(inputobj);

            //tharr[0] = new Thread(new ParameterizedThreadStart(OptionsData.BankNiftyOHLCOptionStrategy));
            //tharr[0].Name = "Thread1";
            //tharr[0].Start(inputobj);
            //lstthread.Add(tharr[1]);
            //OptionsData.ZerolossStrategyScanner(inputobj);
            OptionsData.BankNiftyOHLCOptionStrategy(inputobj);
            //OptionsData.BankNiftyOHLCOptionStrategy(inputobj);

            for (int j = 0; j < 2; j++)
            {
                tharr[j].Join();
            }
        }
        public static void RunScan()
        {
            CsvInputFileObjectsPivot csvinputfileobj = new CsvInputFileObjectsPivot();
            DirectoryInfo dirinfo = new DirectoryInfo(@"C:\Algotrade\scan");
            FileInfo[] fiinfoarr = dirinfo.GetFiles();

            List<Thread> lstthread = new List<Thread>();
            OptionsData optiondata = new OptionsData();
            //Read all the info from csv file
            int i = 0;
            try
            {
                foreach (FileInfo fiinfo in fiinfoarr)
                {

                    List<CsvInputFileObjectsPivot> lstinputfileobj = optiondata.CsvFileInputData(fiinfo.FullName);
                    OptionsData options = new OptionsData();
                    List<Instrument> lstinstrument = OptionsData.GetAllLotSize(kite);

                    Thread[] tharr = new Thread[lstinputfileobj.Count];

                    string filename2path = @"C:\Algotrade\outputfile\OI_25_08_20.txt";
                    string filename2header = "Timestamp,Strikeprice,LastPrice, OIdata,totalVolume,Lotsize,Totallot\n";
                    File.AppendAllText(filename2path, filename2header);
                    Stopwatch watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                    //while (!(DateTime.Now.Hour == 15 && DateTime.Now.Minute == 30))
                    {
                        int count = 0;

                        foreach (CsvInputFileObjectsPivot csvInputFileObjects in lstinputfileobj)
                        {
                            int lotsize = OptionsData.GetLotSize(kite, csvInputFileObjects.Strike1, lstinstrument);

                            csvInputFileObjects.kite = kite;
                            csvInputFileObjects.lotSize = lotsize;
                            //tharr[i] = new Thread(new ParameterizedThreadStart(OptionsData.OIVolumeInfo));
                            OptionsData.OIVolumeInfo(csvInputFileObjects);                            
                            //tharr[i].Name = "Thread" + i.ToString();
                            //tharr[i].Start(csvInputFileObjects);
                            //lstthread.Add(tharr[i]);

                            //++i;
                            //Thread.Sleep(1000);//adding sleep so that each thread starttime is different and all threads dont call the API in the same time
                            ++count;
                            if (count % 100 == 0)
                                Console.WriteLine(count);
                            //Thread.Sleep(500);
                        }
                        watch.Stop();
                        Console.WriteLine("Elapsed minutes" + (watch.Elapsed));
                        //Thread.Sleep(60*60*1000);
                    }
                }

                //Console.WriteLine("Total Theads Running :" + lstthread.Count);



                //for (int j = 0; j < lstthread.Count; j++)
                //{
                //    lstthread[j].Join();
                //}

                Console.WriteLine("Completed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void initSession(string requestToken)
        {
            //Console.WriteLine("Goto " + kite.GetLoginURL());
            //Console.WriteLine("Enter request token: ");
            //string requestToken = Console.ReadLine();
            User user = kite.GenerateSession(requestToken, MySecret);

            Console.WriteLine(Utils.JsonSerialize(user));

            MyAccessToken = user.AccessToken;
            MyPublicToken = user.PublicToken;
        }

        private static void initTicker()
        {
            ticker = new Ticker(MyAPIKey, MyAccessToken);

            ticker.OnTick += OnTick;
            ticker.OnReconnect += OnReconnect;
            ticker.OnNoReconnect += OnNoReconnect;
            ticker.OnError += OnError;
            ticker.OnClose += OnClose;
            ticker.OnConnect += OnConnect;
            ticker.OnOrderUpdate += OnOrderUpdate;

            ticker.EnableReconnect(Interval: 5, Retries: 50);
            ticker.Connect();

            // Subscribing to NIFTY50 and setting mode to LTP
            ticker.Subscribe(Tokens: new UInt32[] { 256265 });
            ticker.SetMode(Tokens: new UInt32[] { 256265 }, Mode: Constants.MODE_LTP);
        }

        private static void OnTokenExpire()
        {
            Console.WriteLine("Need to login again");
        }

        private static void OnConnect()
        {
            Console.WriteLine("Connected ticker");
        }

        private static void OnClose()
        {
            Console.WriteLine("Closed ticker");
        }

        private static void OnError(string Message)
        {
            Console.WriteLine("Error: " + Message);
        }

        private static void OnNoReconnect()
        {
            Console.WriteLine("Not reconnecting");
        }

        private static void OnReconnect()
        {
            Console.WriteLine("Reconnecting");
        }

        private static void OnTick(Tick TickData)
        {
            Console.WriteLine("Tick " + Utils.JsonSerialize(TickData));
        }

        private static void OnOrderUpdate(Order OrderData)
        {
            Console.WriteLine("OrderUpdate " + Utils.JsonSerialize(OrderData));
        }
    }
}
