using APIBridge;
using KiteConnect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KiteConnectSample
{
    public class LoginKite
    {
        private static Kite kite;
        static string MyAPIKey = ConfigurationManager.AppSettings["APIKey"];
        static string MySecret = ConfigurationManager.AppSettings["APISecret"];
        static string MyPublicToken = "abcdefghijklmnopqrstuvwxyz";
        static string MyAccessToken = "abcdefghijklmnopqrstuvwxyz";
        public void Login()
        {

            kite = new Kite(MyAPIKey, Debug: false);

            // For handling 403 errors

            //kite.SetSessionExpiryHook(OnTokenExpire);

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
            var context = http.GetContext();

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
        }
        private  void initSession(string requestToken)
        {
            //Console.WriteLine("Goto " + kite.GetLoginURL());
            //Console.WriteLine("Enter request token: ");
            //string requestToken = Console.ReadLine();
            User user = kite.GenerateSession(requestToken, MySecret);

            Console.WriteLine(Utils.JsonSerialize(user));

            MyAccessToken = user.AccessToken;
            MyPublicToken = user.PublicToken;
        }

        public  int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
