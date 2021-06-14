using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using APIBridge;
using System.Collections;
using System.Threading;
using System.Configuration;

namespace IIFLAPI
{

    public class IIFLConnect : IAPIBridge
    {

        private static  string _root = "https://ttblaze.iifl.com";
        private  string _login = "https://ttblaze.iifl.com/auth/login";
        //interactive API
        //string IntMyAPIKey = "f33b529d1451b74526a640";
        //string IntMySecret = "Nywb610$jQ";
        string IntMyAPIKey = ConfigurationManager.AppSettings["IIFLIntAPIKey"];
        string IntMySecret = ConfigurationManager.AppSettings["IIFLIntAPISecret"];

        //MarketDataAPI
        //string MyAPIKey = "162acff6999b23969a7665";
        //string MySecret = "Mtmx865#dj";
        string MyAPIKey = ConfigurationManager.AppSettings["IIFLAPIKey"];
        string MySecret = ConfigurationManager.AppSettings["IIFLAPISecret"];
        private string _accessToken;
        private string _apiType;

        //static string UserID = "PRMA0101";
        // static string Password = "RamaKrishna7";
        private bool _enableLogging = false;

        public  IIFLConnect()
        {

        }
        private static readonly Dictionary<string, string> _routes = new Dictionary<string, string>
        {
            ["parameters"] = "/parameters",
            ["api.token"] = "/session/token",
            ["api.refresh"] = "/session/refresh_token",
            ["marketdata.quote"] = "/marketdata/instruments/quotes",
            ["searchinstruments"] = "/search/instruments",
            ["interactive.login"] = "/interactive/user/session",
            ["marketdata.login"] = "/apimarketdata/auth/login",
            ["marketdata.instruments"] = "/apimarketdata/intruments/master",
            ["orders.place"] = "/interactive/orders",
            ["marketdata.optionsymbol"] = "/apimarketdata/instruments/instrument/optionSymbol",
            ["marketdata.futsymbol"] = "/apimarketdata/instruments/instrument/futureSymbol",
            ["interactive.orderstatus"] = "/interactive/orders",
            ["marketdata.historical"]= "/apimarketdata/instruments/ohlc"

        };

        private void AddExtraHeaders(ref HttpWebRequest Req)
        {       

            //Req.Headers.Add("Content-Type", "application/json");
            if(!String.IsNullOrEmpty(_accessToken))
            Req.Headers.Add("Authorization", _accessToken);
        }

        private dynamic Request(string Route, string Method,  Dictionary<string, dynamic> Params = null)
        {
            HttpWebRequest request;
            string url = _root + _routes[Route];
            //string paramString = String.Join("&", Params.Select(x => Utils.BuildParam(x.Key, x.Value)));
            string strJson= JsonConvert.SerializeObject(Params);
            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            if (Method.ToUpper() == "POST" || Method.ToUpper() == "PUT")
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;

                request.Method = Method;
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";

                //request.ContentLength = paramString.Length;
                request.ContentLength = bytes.Length;
                //foreach (KeyValuePair<string, dynamic> kvp in Params)
                //{
                //    request.Headers.Add(kvp.Key, kvp.Value);
                //}
                //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                //if (_enableLogging)
                //Console.WriteLine("DEBUG: " + Method + " " + url + "\n" + paramString);
                AddExtraHeaders(ref request);
                List<KeyValuePair<string, dynamic>> paramarr = Params.ToList();
                try
                {
                    using (Stream webStream = request.GetRequestStream())
                    {
                        webStream.Write(bytes, 0, bytes.Count());
                    }
                }
                catch (Exception ex)
                {
                    string errortext = string.Format("{0},{1},{2}\r\n", DateTime.Now, ex.Message, ex.StackTrace);
                    Console.WriteLine(errortext);
                    string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                    File.AppendAllText(filename, errortext);
                }


                //using (StreamWriter requestWriter = new StreamWriter(webStream))

            }
            else
            {
                string paramString = String.Join("&", Params.Select(x => Utils.BuildParam(x.Key, x.Value)));

                request = (HttpWebRequest)WebRequest.Create(url + "?" + paramString);
                request.AllowAutoRedirect = true;
                request.Method = Method;
                // if (_enableLogging) Console.WriteLine("DEBUG: " + Method + " " + url + "?" + paramString);
                AddExtraHeaders(ref request);
            }
            WebResponse webResponse;

            try
            {
                webResponse = request.GetResponse();                
            }
            catch (WebException e)
            {
                if (e.Response is null)
                    throw e;

                webResponse = e.Response;
            }


            using (Stream webStream = webResponse.GetResponseStream())
            {
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    //if (_enableLogging) Console.WriteLine("DEBUG: " + (int)((HttpWebResponse)webResponse).StatusCode + " " + response + "\n");

                    HttpStatusCode status = ((HttpWebResponse)webResponse).StatusCode;

                    if (webResponse.ContentType.Contains("application/json"))
                    {
                        Dictionary<string, dynamic> responseDictionary = Utils.JsonDeserialize(response);

                        if (status != HttpStatusCode.OK)
                        {
                            string errorType = "GeneralException";
                            string message = "";

                            if (responseDictionary.ContainsKey("error_type"))
                                errorType = responseDictionary["error_type"];

                            if (responseDictionary.ContainsKey("message"))
                                message = responseDictionary["message"];


                        }

                        return responseDictionary;
                    }
                    else if (webResponse.ContentType == "text/csv")
                        return Utils.ParseCSV(response);
                    else
                        throw new Exception
                            ("Unexpected content type " + webResponse.ContentType + " " + response);
                }
                
            }
          
        }//end of func

        public static  HttpResponseMessage Request1(string Route, string Method, Dictionary<string, string> Params = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient  request;
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = _root + _routes[Route];
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/www-x-form-urlencoded"));
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    List<KeyValuePair<string, dynamic>> postData = new List<KeyValuePair<string, dynamic>>();
                    //postData.Add()
                    //foreach (KeyValuePair<string, string> kvp in Params)
                    //{
                    //    postData.Add(new KeyValuePair<string, string>(kvp.Key, kvp.Value));
                    //}
                    //postData.Add(new KeyValuePair<string, string>("Content-Type", "application/json"));
                    Dictionary<string, string> Params1 = new Dictionary<string, string>();
                    //foreach (KeyValuePair<string,dynamic> kvp in Params)
                    //{
                    //    Params1.Add(kvp.Key, kvp.Value);

                    //}

                    FormUrlEncodedContent content = new FormUrlEncodedContent(Params);

                    response =  client.PostAsync("", content).Result;
                    //string jsonstring = await response.Content.ReadAsStringAsync();
                    //object responsedata = JsonConvert.DeserializeObject(jsonstring);

                    //string accesstoken = ((dynamic)responsedata).result.token;
                }
               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return response;
        }//end of func

        public void EnableLogging(bool enableLogging)
        {
            _enableLogging = enableLogging;
        }

        //public User GenerateSession(string RequestToken, string AppSecret)
        //{
        //    //string checksum = Utils.SHA256(_apiKey + RequestToken + AppSecret);

        //    var param = new Dictionary<string, string>
        //    {
        //        {"appKey", MyAPIKey},
        //        {"secretKey",MySecret },
        //        {"source", "WEBAPI"}

        //    };

        //    var userData = Post("api.token", param);

        //    return new User(userData);
        //}
        /// <summary>
        /// Set the access token
        /// </summary>
        /// <param name="AccessToken"></param>
        public void SetAccessToken(string AccessToken)
        {
            //if (ApiType.ToLower() == "marketdata")
            {
                this._accessToken = AccessToken;
            }
            
        }

        public IIFLUser GenerateSessionMarket()
        {
            //string checksum = Utils.SHA256(_apiKey + RequestToken + AppSecret);

            var param = new Dictionary<string, dynamic>
            {                
                {"appKey", MyAPIKey},
                {"secretKey",MySecret },
                {"source", "WEBAPI"}
            };

            var userData = Post("marketdata.login", param);

            return new IIFLUser(userData);
        }

        public Dictionary<string,dynamic> ParseTradingSymbol(string tradingsymbol)
        {
            Dictionary<string, dynamic> dictoptionParam = new Dictionary<string, dynamic>();
            //get symbol, expirtydate, optiontype(CE/PE) , strikeprice in order to make the call to get the instrumentid
            
            //look for firscharacter and exract 7 characters
            int intpos = tradingsymbol.IndexOfAny("0123456789".ToCharArray());
            string expiryDate = "";
            if (intpos > 0)
            {
                expiryDate = tradingsymbol.Substring(intpos, 9);
            }
            else
            {
                dictoptionParam = null;
                return dictoptionParam;
            }


            int strikelen = tradingsymbol.Length - (intpos + 9);
            string strikeprice = tradingsymbol.Substring(intpos + 9, strikelen-2);
            string symbol = tradingsymbol.Substring(0, intpos);
            string series = (symbol.ToUpper() == "NIFTY" || symbol.ToUpper() == "BANKNIFTY") ? IIFLConstants.SERIES_OPTDIX : IIFLConstants.SERIES_OPTSTK;

            string optionType = tradingsymbol.Substring(tradingsymbol.Length - 2, 2);
            string exchangeSegment = IIFLConstants.NSEFO;

            dictoptionParam.Add("exchangeSegment", exchangeSegment);
            dictoptionParam.Add("series", series);
            dictoptionParam.Add("symbol", symbol);
            dictoptionParam.Add("expiryDate", expiryDate);
            dictoptionParam.Add("optionType", optionType);
            dictoptionParam.Add("strikePrice", strikeprice);



            return dictoptionParam;


        }

        public Dictionary<string, dynamic> ParseTradingSymbolFUT(string tradingsymbol,string expiryDate="")
        {
            Dictionary<string, dynamic> dictoptionParam = new Dictionary<string, dynamic>();
       


            string symbol = tradingsymbol;
            string series = (symbol.ToString().ToUpper().Contains("NIFTY") || symbol.ToString().ToUpper().Contains("NIFTYBANK")) ? IIFLConstants.SERIES_FUTIDX : IIFLConstants.SERIES_FUTSTK;

            string optionType = tradingsymbol.Substring(tradingsymbol.Length - 2, 2);
            string exchangeSegment = IIFLConstants.NSEFO;

            dictoptionParam.Add("exchangeSegment", exchangeSegment);
            dictoptionParam.Add("series", series);
            dictoptionParam.Add("symbol", symbol);
            dictoptionParam.Add("expiryDate", expiryDate);            



            return dictoptionParam;


        }
        
        public IIFLUser GenerateSessionInteractive()
        {
            //string checksum = Utils.SHA256(_apiKey + RequestToken + AppSecret);

            var param = new Dictionary<string, dynamic>
            {
                {"appKey", IntMyAPIKey},
                {"secretKey",IntMySecret },
                {"source", "WEBAPI"}
            };

            var userData = Post("interactive.login", param);

            return new IIFLUser(userData);
        }
        //public static async Task<HttpResponseMessage> Login1()
        //{
        //    //string checksum = Utils.SHA256(_apiKey + RequestToken + AppSecret);

        //    var param = new Dictionary<string, string>
        //    {
        //        //{"userID",UserID },
        //        //{"password", Password },
        //        {"appKey", MyAPIKey},
        //         {"source", "WEBAPI"},
        //        {"secretKey",MySecret }

        //    };
        //    HttpResponseMessage userData = await Post1("marketdata.login",param);
        //    return userData;

        //    //return new User(userData);
        //}

        private dynamic Post(string Route, Dictionary<string, dynamic> Params = null)
        {
            return Request(Route, "POST", Params);
        }



        private dynamic Get(string Route, Dictionary<string, dynamic> Params = null)
        {
            return Request(Route, "Get", Params);
        }
        private static HttpResponseMessage Post1(string Route, Dictionary<string, string> Params = null)
        {
            HttpResponseMessage httpresp = Request1(Route, "POST", Params);
            return httpresp;
        }
        public IIFLHistorical GetHistoricalQuote(string Route,Dictionary<string,dynamic> Params=null)
        {
            IIFLHistorical iiflhistobj;
            Dictionary<string, dynamic> histdata= Request(Route, "Get", Params);
            if(histdata.ContainsKey("result"))
            {
                return  new IIFLHistorical(histdata);
            }
            return new IIFLHistorical(histdata); 
        }
        public IIFLInstrument GetInstrumentID(string Route, Dictionary<string, dynamic> Params = null)
        {
            Dictionary<string, dynamic> instrumentdata = new Dictionary<string, dynamic>();
            IIFLInstrument iiflintrumentobj;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                try
                {
                    instrumentdata = Request(Route, "Get", Params);
                    if (instrumentdata.ContainsKey("result"))
                    {
                        iiflintrumentobj = new IIFLInstrument(instrumentdata);
                        break;
                    }
                    else
                    {                       

                        string symbol = Params["symbol"];
                        string expirydate = Params["expiryDate"];
                        string optiontype = "";
                        string strikeprice = "";
                        if (!Route.Contains("marketdata.futsymbol"))
                        {
                            strikeprice = Params["strikePrice"];
                            if (symbol.EndsWith("PE") || symbol.EndsWith("CE"))
                            {
                                optiontype = Params["optionType"];
                            }
                        }
                        string fullsymbolcat = string.Format("{0}{1}{2}{3}", symbol, expirydate, optiontype, strikeprice);

                        string errortext = string.Format("{0}-{1},Failed at line no 370: Method GetQuote(),retry-{2},{3}\r\n", DateTime.Now, fullsymbolcat, i, strikeprice);
                        Console.WriteLine(errortext);
                        string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                        File.AppendAllText(filename, errortext);
                    }
                }
                catch(Exception ex)
                {
                    string symbol = Params["symbol"];
                    string expirydate = Params["expiryDate"];
                    string optiontype = Params["optionType"];
                    string strikeprice = Params["strikePrice"];              

                    string fullsymbolcat = string.Format("{0}{1}{2}{3}", symbol, expirydate, optiontype, strikeprice);

                    string errortext = string.Format("{0},{1},{2},{3}\r\n", DateTime.Now, ex.Message, ex.StackTrace,fullsymbolcat);
                    Console.WriteLine(errortext);
                    string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                    File.AppendAllText(filename, errortext);
                    throw ex;
                }
                
            }
            return new IIFLInstrument(instrumentdata);

        }
        public IIFLInstrument GetFutureInstrumentID(string Route, Dictionary<string, dynamic> Params = null)
        {
            Dictionary<string, dynamic> instrumentdata = new Dictionary<string, dynamic>();
            IIFLInstrument iiflintrumentobj;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                try
                {
                    instrumentdata = Request(Route, "Get", Params);
                    if (instrumentdata.ContainsKey("result"))
                    {
                        iiflintrumentobj = new IIFLInstrument(instrumentdata);
                        break;
                    }
                    else
                    {

                        string symbol = Params["symbol"];
                        string expirydate = Params["expiryDate"];                        

                        string fullsymbolcat = string.Format("{0}{1}", symbol, expirydate);

                        string errortext = string.Format("{0}-{1},Failed at line no 370: Method GetQuote(),retry-{2},{3}\r\n", DateTime.Now, fullsymbolcat, i, symbol);
                        Console.WriteLine(errortext);
                        string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                        File.AppendAllText(filename, errortext);
                    }
                }
                catch (Exception ex)
                {
                    string symbol = Params["symbol"];
                    string expirydate = Params["expiryDate"];
                    string optiontype = Params["optionType"];
                    string strikeprice = Params["strikePrice"];

                    string fullsymbolcat = string.Format("{0}{1}{2}{3}", symbol, expirydate, optiontype, strikeprice);

                    string errortext = string.Format("{0},{1},{2},{3}\r\n", DateTime.Now, ex.Message, ex.StackTrace, fullsymbolcat);
                    Console.WriteLine(errortext);
                    string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                    File.AppendAllText(filename, errortext);
                    throw ex;
                }

            }
            return new IIFLInstrument(instrumentdata);

        }

        public Dictionary<string, dynamic> GetQuote(string InstrumentID)
        {
            string json = "{" +
               "\"instruments\": [" +
               "{" +
               "\"exchangeSegment\":" + IIFLConstants.NSEFO + "," +
               "\"exchangeInstrumentID\":" + InstrumentID +
               "}" +
               "]," +
               "\"xtsMessageCode\":" + IIFLConstants.MESSAGECODE_MARKETDATA + "," +
                   "\"publishFormat\": \"JSON\"" +
              "}";
            var param = new Dictionary<string, dynamic>();
            //param.Add("i", InstrumentId);
            Dictionary<string, dynamic> Params1 =JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
            Dictionary<string, dynamic> quotes = new Dictionary<string, dynamic>();
            for (int i = 0; i < 10; i++)
            {
                //Thread.Sleep(200);
                Dictionary<string, dynamic> quoteData = Request("marketdata.quote", "POST", Params1);
                if (quoteData.ContainsKey("result"))
                {
                    try
                    {
                        quotes.Add(InstrumentID, new IIFLQuote(quoteData));
                    }
                    catch (Exception ex)
                    {
                   
                        string errortext = string.Format("{0},{1},{2}\r\n", DateTime.Now, ex.Message, ex.StackTrace);
                        Console.WriteLine(errortext);
                        string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                        File.AppendAllText(filename, errortext);
                        throw ex;
                    }
                    break;
                }
                else
                {
                    string errortext = string.Format("{0}-InstrumentID:{1},Failed at line no 411: Method GetQuote(),retry:{2}", DateTime.Now, InstrumentID, i);
                      
                    Console.WriteLine(errortext);
                    string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                    using (FileStream fs = new FileStream(filename, FileMode.Append))
                    {
                        string data = DateTime.Now.ToString() + "-" + errortext;
                        byte[] info = new UTF8Encoding(true).GetBytes(data);
                        fs.Write(info, 0, info.Length);
                    }
                        //File.AppendAllText(filename, errortext);
                }
                
            }         
            return quotes;

            

        }

        public Dictionary<string, dynamic> GetQuoteBank(string InstrumentID)
        {
            string json = "{" +
               "\"instruments\": [" +
               "{" +
               "\"exchangeSegment\":" + IIFLConstants.NSECM + "," +
               "\"exchangeInstrumentID\":\"" + InstrumentID +"\"" +
               "}" +
               "]," +
               "\"xtsMessageCode\":" + IIFLConstants.MESSAGECODE_INDEXDATA + "," +
                   "\"publishFormat\": \"JSON\"" +
              "}";
            var param = new Dictionary<string, dynamic>();
            //param.Add("i", InstrumentId);
            Dictionary<string, dynamic> Params1 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
            Dictionary<string, dynamic> quotes = new Dictionary<string, dynamic>();
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                Dictionary<string, dynamic> quoteData = Request("marketdata.quote", "POST", Params1);
                if (quoteData.ContainsKey("result"))
                {
                    try
                    {
                        quotes.Add(InstrumentID, new IIFLBankQuote(quoteData));
                    }
                    catch (Exception ex)
                    {

                        string errortext = string.Format("{0},{1},{2}\r\n", DateTime.Now, ex.Message, ex.StackTrace);
                        Console.WriteLine(errortext);
                        string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                        File.AppendAllText(filename, errortext);
                        throw ex;
                    }
                    break;
                }
                else
                {
                    string errortext = string.Format("{0}-InstrumentID:{1},Failed at line no 411: Method GetQuote(),retry:{2}", DateTime.Now, InstrumentID, i);

                    Console.WriteLine(errortext);
                    string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                    File.AppendAllText(filename, errortext);
                }

            }
            return quotes;



        }
        public Dictionary<string, dynamic> PlaceOrder(
            string Exchange,
            int Quantity,
            string TradingSymbol=null,
            string TransactionType = null,            
            decimal? Price = null,
            string ProductType = null,
            string OrderType = null,
            string Validity = null,
            string ExchangeInstrumentId = null,//for IIFL 
            int? DisclosedQuantity = null,
            decimal? TriggerPrice = null,
            decimal? SquareOffValue = null,
            decimal? StoplossValue = null,            
            string Variety = "",
            string Tag = "")
        {

            var param = new Dictionary<string, dynamic>();

            Utils.AddIfNotNull(param, "exchangeSegment", Exchange);
            Utils.AddIfNotNull(param, "exchangeInstrumentID", ExchangeInstrumentId);
            Utils.AddIfNotNull(param, "productType", ProductType);
            Utils.AddIfNotNull(param, "orderType", OrderType);
            Utils.AddIfNotNull(param, "orderSide", TransactionType);
            Utils.AddIfNotNull(param, "timeInForce", Validity);
            Utils.AddIfNotNull(param, "disclosedQuantity", DisclosedQuantity.ToString());
            Utils.AddIfNotNull(param, "orderQuantity", Quantity.ToString());
            Utils.AddIfNotNull(param, "limitPrice", Price.ToString());
            Utils.AddIfNotNull(param, "stopPrice", StoplossValue.ToString());
            Utils.AddIfNotNull(param, "orderUniqueIdentifier", Tag);

            return Post("orders.place", param);
        }
        //public string PlaceOrder()
        //{
        //    throw new NotImplementedException();
        //}

        public Dictionary<string, dynamic> GetOrderStatus(string  orderid)
        {
            string orderstatus = "";

            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
            Utils.AddIfNotNull(param, "appOrderID", orderid);
            Dictionary<string, dynamic> order = new Dictionary<string, dynamic>();
            order.Add("OrderId", orderid);
            order.Add("Status", "");
            order.Add("ExchangeOrderID", "");
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                try
                {
                    Dictionary<string, dynamic> orderdata = Get("interactive.orderstatus", param);
                    if (orderdata.ContainsKey("result"))
                    {
                        IIFLOrder iiflorder = new IIFLOrder(orderdata);
                        if (iiflorder.OrderStatus.ToLower() == "new")
                        {
                            orderstatus = "pending";
                        }
                        else if (iiflorder.OrderStatus.ToLower() == "pendingnew")
                        {
                            orderstatus = "pending";
                        }
                        else if (iiflorder.OrderStatus.ToLower() == "filled")
                        {
                            orderstatus = "complete";
                        }
                        else
                        {
                            orderstatus = iiflorder.OrderStatus;
                        }

                        order["ExchangeOrderID"] = iiflorder.ExchangeOrderID;
                        order["Status"] = orderstatus;
                        if (orderstatus != "pending")
                        {
                            break;
                        }
                        else
                        {
                            string errortext = string.Format("{0}-OrderID:{1},: Method GetOrderStatus() Line 512,retry:{2}\r\n",
                                                        DateTime.Now, orderid, i);
                            Console.WriteLine(errortext);
                            string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                            File.AppendAllText(filename, errortext);
                        }

                    }
                    else
                    {
                        string errortext = string.Format("{0}-OrderID:{1},Failed at line no 521: Method GetOrderStatus(),retry:{2}\r\n",
                                                        DateTime.Now, orderid, i);
                        Console.WriteLine(errortext);
                        string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                        File.AppendAllText(filename, errortext);
                    }
                }
                catch(Exception ex)
                {
                    string errortext = string.Format("{0},OrderId:{1},{2},{3}\r\n", DateTime.Now,orderid, ex.Message, ex.StackTrace);
                    Console.WriteLine(errortext);
                    string filename = string.Format(@"C:\Algotrade\scalperoutput\errorlog{0}.txt", Thread.CurrentThread.Name);
                    File.AppendAllText(filename, errortext);
                    throw ex;
                }
               
            }

            return order;
        }
        public string GetQuote()
        {
            throw new NotImplementedException();
        }

        public string CancelOrder()
        {
            throw new NotImplementedException();
        }
    }//class
}//namespace


        
    



