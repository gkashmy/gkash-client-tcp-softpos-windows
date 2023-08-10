using ClientSocketConnection;
using ClientSocketConnection.model;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Windows.Forms;
using static ClientSocketConnection.model.SocketStatus;
using static ClientSocketConnection.model.TransResult;

namespace TcpSDKDemo
{
    public partial class Form1 : Form, IDataCallback
    {
        private ClientSocket client;
        private PaymentType paymentType = PaymentType.CARD_PAYMENT;
        public Form1()
        {
            InitializeComponent();
        }

        public void TransactionResult(TransactionStatus result)
        {
            //your payment result
            if (label8.InvokeRequired)
            {
                label8.Invoke(new MethodInvoker(delegate
                {
                    label8.Text = result.RemID;
                    label11.Text = result.Status;
                    label12.Text = result.CartID;
                    label18.Text = result.TransferCurrency + result.TransferAmount;
                    label19.Text = result.TransferDate;
                }));
            }
            else
            {
                label8.Text = result.RemID.ToString();
            }
        }
        public void QueryTransactionResult(TransactionStatus result)
        {
            //your payment result
            if (label8.InvokeRequired)
            {
                label8.Invoke(new MethodInvoker(delegate
                {
                    label8.Text = result.RemID;
                    label11.Text = result.Status;
                    label12.Text = result.CartID;
                    label18.Text = result.TransferCurrency + result.TransferAmount;
                }));
            }
            else
            {
                label8.Text = result.RemID.ToString();
            }
        }

        public void QueryTransMessage(string message)
        {
            // handle error
        }

        public void CurrentTransactionCartId(string cartId)
        {
            // current transaction cart Id
            if (textBox8.InvokeRequired)
            {
                textBox8.Invoke(new MethodInvoker(delegate { textBox8.Text = cartId; }));
            }
            else
            {
                textBox8.Text = cartId;
            }
        }

        public void TransactionEventCallback(TransactionEventCallback transactionEventCallback)
        {
            if (label17.InvokeRequired)
            {
                label17.Invoke(new MethodInvoker(delegate { label17.Text = transactionEventCallback.ToString(); }));
            }
            else
            {
                label17.Text = transactionEventCallback.ToString();
            }
        }

        public void SocketStatusCallback(SocketConnectivityCallback socketConnectivityCallback)
        {
            if (label10.InvokeRequired)
            {
                if (socketConnectivityCallback == SocketConnectivityCallback.CONNECTED || socketConnectivityCallback == SocketConnectivityCallback.RECONNECTING)
                {
                    label10.Invoke(new MethodInvoker(delegate { label10.Text = socketConnectivityCallback.ToString() + " " + client.GetIpAddress(); }));
                }
                else
                {
                    label10.Invoke(new MethodInvoker(delegate { label10.Text = socketConnectivityCallback.ToString(); }));
                }

            }
            else
            {
                if (socketConnectivityCallback == SocketConnectivityCallback.CONNECTED || socketConnectivityCallback == SocketConnectivityCallback.RECONNECTING)
                {
                    label10.Text = socketConnectivityCallback.ToString() + " " + client.GetIpAddress();
                }
                else
                {
                    label10.Text = socketConnectivityCallback.ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            var serilogLogger = new LoggerConfiguration()
             .MinimumLevel.Debug()
             .WriteTo.Console()
             .WriteTo.File(@"C:\GkashClientLog\log.txt", shared: true, rollingInterval: RollingInterval.Month)
             .CreateLogger();
            var logger = new SerilogLoggerFactory(serilogLogger).CreateLogger<Form1>();

            //var loggerFactory = LoggerFactory.Create(builder => builder.AddSimpleConsole(options => {
            //    options.SingleLine = true;
            //    options.TimestampFormat = "hh:mm:ss.fff";
            //}));
            //Microsoft.Extensions.Logging.ILogger _logger = loggerFactory.CreateLogger<Form1>();
            
            string certPath = "C://t1clientcert/t1clientcert.pfx";

            bool isProduction = checkBox2.Checked;

            client = new ClientSocket(username, password, this, certPath, isProduction, logger);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //email, mobile no, refrence no is optional input
            string amount = textBox3.Text;
            string email = textBox4.Text;
            string mobileNo = textBox5.Text;
            string referenceNo = textBox6.Text;

            PaymentRequestDto requestDto = new PaymentRequestDto()
            {
                Amount = amount,
                Email = email,
                MobileNo = mobileNo,
                ReferenceNo = referenceNo,
                PaymentType = paymentType,
                PreAuth = checkBox1.Checked
            };

            if (label8.InvokeRequired)
            {
                label8.Invoke((MethodInvoker)(() => label8.Text = " Waiting payment response"));
                label11.Invoke((MethodInvoker)(() => label11.Text = ""));
                label12.Invoke((MethodInvoker)(() => label12.Text = ""));
                label18.Invoke((MethodInvoker)(() => label18.Text = ""));
                label19.Invoke((MethodInvoker)(() => label19.Text = ""));
            }
            else
            {
                label11.Text = "";
                label12.Text = "";
                label18.Text = "";
                label19.Text = "";
                label8.Text = " Waiting payment response";
            }

            if (client != null)
            {
                client.RequestPayment(requestDto);
            }
        }
        public void UpdateUI(object msg)
        {
            if (label8.InvokeRequired)
            {
                label8.Invoke((MethodInvoker)(() => label8.Text = msg.ToString()));
            }
            else
            {
                label8.Text = msg.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox7.Text = "EWALLET SCAN PAYMENT";
            paymentType = PaymentType.EWALLET_SCAN_PAYMENT;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox7.Text = "TAP-TO-PHONE PAYMENT";
            paymentType = PaymentType.TAPTOPHONE_PAYMENT;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox7.Text = "CARD PAYMENT";
            paymentType = PaymentType.CARD_PAYMENT;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox7.Text = "MAYBANK QR";
            paymentType = PaymentType.MAYBANK_QR;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox7.Text = "TnG QR";
            paymentType = PaymentType.TNG_QR;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox7.Text = "Gkash QR";
            paymentType = PaymentType.GKASH_QR;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox7.Text = "Boost QR";
            paymentType = PaymentType.BOOST_QR;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox7.Text = "GrabPay QR";
            paymentType = PaymentType.GRABPAY_QR;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            textBox7.Text = "DuitNow QR";
            paymentType = PaymentType.DUITNOW_QR;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox7.Text = "MCash QR";
            paymentType = PaymentType.MCASH_QR;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox7.Text = "Atome QR";
            paymentType = PaymentType.ATOME_QR;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox7.Text = "AliPay QR";
            paymentType = PaymentType.ALIPAY_QR;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            textBox7.Text = "Wechat QR";
            paymentType = PaymentType.WECHAT_QR;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox7.Text = "ShopeePay QR";
            paymentType = PaymentType.SHOPEE_PAY_QR;

        }

        private void button19_Click(object sender, EventArgs e)
        {

            if (client != null)
            {
                //your query result
                TransactionStatus status = client.QueryStatus(textBox8.Text);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            client.CancelPayment();
        }
    }
}