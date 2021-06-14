using KiteConnectSample;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScalperUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FillStrategyComboBox();
        }

        private void FillStrategyComboBox()
        {
            comboBox1.Items.Add("LongStrangleStrategy");
            comboBox1.Items.Add("BNOptionsdata");

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView1.
        }

        public delegate void UpdateControl(object text);
        //public delegate void UpdateControl1(object obj);
        public UpdateControl myDelegate;

        public void UpdateControlMethod(object text)
        {
            textBox1.Text = text.ToString();
        }
        static int lots=1;
        public void UpdateControlGridMethod(object datagridobj)
        {
            CsvInputFileSqoff csvfileobj = (CsvInputFileSqoff)datagridobj;
            dataGridView1.Rows.Clear();
            int rowId = dataGridView1.Rows.Add();

            // Grab the new row!
            DataGridViewRow row = dataGridView1.Rows[rowId];


            // Add the data
            row.Cells["DateTime"].Value = System.DateTime.Now;
            row.Cells["Strike1"].Value = csvfileobj.Strike1;
            row.Cells["Strike2"].Value = csvfileobj.Strike2;
            row.Cells["NoofLots"].Value = lots++;

        }
        public void UpdateControlGridView(object datagridobj)
        {
       

            myDelegate = new UpdateControl(UpdateControlGridMethod);

            textBox1.Invoke(myDelegate, new object[] { datagridobj });


        }
        public  void UpdateTextBox(string updatetext)
        {
            //textBox1.Invoke((Action)delegate
            //{
            //    textBox1.Text = updatetext;
            //});
            myDelegate = new UpdateControl(UpdateControlMethod);
            textBox1.Invoke(myDelegate, new object[] { updatetext });
            
            
        }
        
        

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            BNOptionData();
        }
        public  void BNOptionData()
        {

            CsvInputFileObjectsPivot csvinputfileobj = new CsvInputFileObjectsPivot();
            DirectoryInfo dirinfo = new DirectoryInfo(@"C:\Algotrade\inputBNcsvfile");
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
                        //csvInputFileObjects.kite = kite;
                        tharr[i] = new Thread(new ParameterizedThreadStart(BankNiftyStrikeOptionData));
                        tharr[i].Name = "Thread" + i.ToString();
                        tharr[i].Start(csvInputFileObjects);
                        lstthread.Add(tharr[i]);

                        ++i;
                        //Thread.Sleep(200);//adding sleep so that each thread starttime is different and all threads dont call the API in the same time

                    }
                }

                Console.WriteLine("Total Theads Running BN Options:" + lstthread.Count);

                //squareoff 

                for (int j = 0; j < lstthread.Count; j++)
                {
                    //lstthread[j].Join();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                File.AppendAllText(@"C:\Algotrade\scalperoutput\mainErrorLog.txt", ex.StackTrace);
            }

            Console.WriteLine("Completed");
        }

        public   void BankNiftyStrikeOptionData(object csvobj)
        {
            CsvInputFileObjectsPivot csvinputobj = (CsvInputFileObjectsPivot)csvobj;
            //Kite kite = csvinputobj.kite;
            string strikeprice1 = csvinputobj.Strike1;
            string strikeprice2 = csvinputobj.Strike2;

            //create the directory if it doesnt exist

            //csvinputobj.lotSize = lotsize;           
            //delete existing files


            string threadname = Thread.CurrentThread.Name;

            string filename2 = string.Format("{0}-{1}_{2}_{3}_{4}", strikeprice1, strikeprice2, System.DateTime.Now.ToString("dd-MM-yyyy"), "ErrorLog",
                                                                 threadname);
            string filename2path = string.Format(@"C:\Algotrade\scalperoutput\BN_{0}.csv", filename2);
            string filecontent2 = "DateTime,Message,StackTrace,Strike1,Strike2,LineNo\r\n";
            File.AppendAllText(filename2path, filecontent2);

            string filename3 = string.Format("{0}-{1}_{2}_{3}_{4}", strikeprice1, strikeprice2, System.DateTime.Now.ToString("dd-MM-yyyy"), "Continous",
                                                                  threadname);
            string filename3path = string.Format(@"C:\Algotrade\scalperoutput\LiveBN-{0}.csv", filename3);
            string filecontent3 = "Datetime,Bid1,Offer2,Bid2,Offer1,BidSum,OfferSum\r\n";
            File.AppendAllText(filename3path, filecontent3);



            int nooflots = 0;


            Dictionary<string, dynamic> dictinstrumentid = new Dictionary<string, dynamic>()
            {
                {"strikeprice1", strikeprice1},
                {"strikeprice2",strikeprice2 }
            };

            for (int j = 0; j < 2000; ++j)
            {
                //UpdateTextBox(string.Format("Count:{0}",  j+1));
                CsvInputFileSqoff csvobj1= new CsvInputFileSqoff();
                csvobj1.Strike1 = "24000PE";
                csvobj1.Strike2 = "25000CE";

                UpdateControlGridView(csvobj1);
                Thread.Sleep(5000);
            }

            //
            Thread.Sleep(100);
            OptionsData._broker = "iifl";
            Dictionary<string, dynamic> instrumentids = OptionsData.GetInstrumentIDLotsize(ref dictinstrumentid);
            int lotsize = Convert.ToInt32(instrumentids["lotsize"]);
            string instrumentId1 = "";
            string instrumentId2 = "";


            TimeSpan end = new TimeSpan(15, 31, 0);

            int count = 1;

            //run till 03:30PM
            while (new TimeSpan(System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second) > end)
            {
                try
                {
                    //Thread.Sleep(10000);

                    instrumentId1 = Convert.ToString(instrumentids["strikeprice1"]);
                    instrumentId2 = Convert.ToString(instrumentids["strikeprice2"]);

                    Dictionary<string, dynamic> bnquotes = OptionsData.GetQuoteBank("NIFTY BANK");
                    KeyValuePair<string, dynamic> bnkvpquote = bnquotes.ElementAt(0);
                    dynamic currentliveprice = bnkvpquote.Value.LastPrice;

                    Dictionary<string, dynamic> quotes1 = OptionsData.GetQuote(instrumentId1);
                    Dictionary<string, dynamic> quotes2 = OptionsData.GetQuote(instrumentId2);
                    //add nfo 
                    KeyValuePair<string, dynamic> kvpquote1 = quotes1.ElementAt(0);
                    KeyValuePair<string, dynamic> kvpquote2 = quotes2.ElementAt(0);

                    UpdateTextBox(string.Format("{0},{1},{2}", currentliveprice, threadname, count));
                    Thread.Sleep(500);
                    //get bid ask price

                    //dynamic lstbids1 = kvpquote1.Value.Bids;
                    //dynamic lstoffers1 = kvpquote1.Value.Offers;
                    //dynamic lstbids2 = kvpquote2.Value.Bids;
                    //dynamic lstoffers2 = kvpquote2.Value.Offers;
                    //int totaltradedquantity1 = kvpquote1.Value.Quantity;
                    //int totaltradedquantity2 = kvpquote2.Value.Quantity;


                    //decimal bidsum = (lstbids1[0].Price + lstbids2[0].Price);
                    //decimal offersum = (lstoffers1[0].Price + lstoffers2[0].Price);

                    //string filecontent1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", System.DateTime.Now, lstbids1[0].Price, lstoffers1[0].Price, lstbids2[0].Price,
                    //                                                                  lstoffers2[0].Price, bidsum,  offersum, currentliveprice);
                    //File.AppendAllText(filename3path, filecontent1);

                    ////update uithread


                    //UpdateTextBox(string.Format("{0},{1}", currentliveprice, count));

                    //Thread.Sleep(2000);


                }//end of try
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    string filecontent1 = string.Format("{0},{1},{2},{3},{4}\r\n", System.DateTime.Now, ex.Message, ex.StackTrace,
                                                                           strikeprice1, strikeprice2);
                    File.AppendAllText(filename2path, filecontent1);
                }
                count++;
            }



        }//end of function
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("button2 cliked");
        }
    }
}
