# gkash-client-tcp-softpos-windows (Offline payment)
This project serves as a sample that demonstrates how to communicate with the Gkash SoftPOS APP and perform offline payments.

## Installation

Install as [Nuget Package](https://www.nuget.org/packages/ClientSocketConnection)

```
Install-Package ClientSocketConnection
```

## Usage
SDK Initialization
| ClientSocket | Description | Type |
| --- | --- | --- |
| loginUsername | SoftPOS login username | string |
| password | SoftPOS login password | string |
| dataCallback | IDataCallback interface of SDK, refer section IDataCallback | IDataCallback |
| certPath | The path of the cert in your device | string |
| environment | Pass true to enable production environment, default is false | bool |
| logger | To enable logging for investigate SDK error (Optional) | ILogger |

Initialize without logging
```csharp
//Implement IDataCallback to your class and pass into ClientSocket
ClientSocket client = new ClientSocket(loginUsername, password, dataCallback, certPath, environment);
```
Initialize with logging
```csharp
var serilogLogger = new LoggerConfiguration()
   .MinimumLevel.Debug()
   .WriteTo.Console()
   .WriteTo.File(@"C:\GkashClientLog\log.txt", shared: true, rollingInterval: RollingInterval.Month).CreateLogger();
                    
var logger = new  SerilogLoggerFactory(serilogLogger).CreateLogger<Form1>();

ClientSocket client = new ClientSocket(Username, Password, dataCallback, certPath, Environment, logger);
```

## Request Payment

When calling this function, SDK will send a request to SoftPOS to perform transaction.

```csharp
public class PaymentRequestDto
{
    public decimal Amount { get; set; }
    public string Email { get; set; }
    public string MobileNo { get; set; }
    public string ReferenceNo { get; set; }
    public int PaymentType { get; set; }
    public bool PreAuth { get; set; }
}
```

| PaymentType |
| --- |
| EWALLET_SCAN_PAYMENT |
| TAPTOPHONE_PAYMENT |
| Card_Payment |
| MAYBANK_QR |
| GRABPAY_QR |
| TNG_QR |
| GKASH_QR |
| BOOST_QR |
| WECHAT_QR |
| SHOPEE_PAY_QR |
| ALIPAY_QR |
| ATOME_QR |
| MCASH_QR |
| DUITNOW_QR |

- `Amount`: the amount to be paid.
- `Email`: the email of the customer making the payment. (Optional)
- `MobileNo`: the mobile number of the customer making the payment. (Optional)
- `ReferenceNo`: a reference number for the payment request. (Optional)
- `PaymentType`: the type of payment to be made. Please refer the table below
- `PreAuth`: a boolean value indicating whether the payment should be a pre-authorization or not. (Optional)

Request payment example:
```csharp
client.requestPayment("10.00","customeremail@test.com","0123456789","refnum123", PaymentType.Card_Payment, false);
```

## Data Callback

Implement IDataCallback to your class.

This class holds the Transaction event raised by Gkash SDK that may need to show the user.

| TransactionEventCallback | Description |
| --- | --- |
| DISPLAYING_QR | Displaying QR |
| SCANNING_QR | Ready to scan QR |
| READY_TO_READ_CARD | Ready to read card |
| INPUT_PIN | Please key in PIN |
| RETRIEVING_PAYMENT_STATUS | Payment request sent to server and waiting for response |
| QUERY_STATUS | Querying eWallet QR status |
| CANCEL_PAYMENT | Cancel payment |
| INVALID_METHOD | Your account do not support this payment method |
| INVALID_AMOUNT | Invalid amount |
| DEVICE_OFFLINE | ME30S Device is offline |
| NO_CARD_DETECTED_TIMEOUT | Card not found timeout |
| NO_PIN_DETECTED_TIMEOUT | PIN not found timeout |
| TRY_AGAIN | Connection to terminal has been disconnected, please re-login and try again |
| DONE | Transaction is complete |

This class holds the Socket Connectivity between SDK and SoftPOS that may need to show the user.

| SocketConnectivityCallback | Description |
| --- | --- |
| ONLINE | Login Successful |
| CONNECTED | Connected to terminal |
| DISCONNECTED | Connection has disconnected |
| HOST_NOT_FOUND | Failed to connect to the terminal. Check if the terminal's server mode is enabled. |
| LOGIN_FAIL | Login fail |
| RETRIEVE_IP_FAIL | Get the terminal's IP address failed. Please retry login. |
| RETRIEVE_KEY_FAIL | Get the merchant’s key failed. Please retry login. |

This callback function return current transaction's cart Id.
| CurrentTransactionCartId | Description |
| --- | --- |
| CartId | Current transaction’s cart Id |

This callback function return query error message.
| QueryTransMessage | Description |
| --- | --- |
| Message | Error occurred when querying transaction |

This callback function return transaction status.
| TransactionStatus | Description | Data type |
| --- | --- | --- |
| MID | MID | String |
| TID | TID | String |
| CardNo | Card number | String |
| CartID | Merchant reference number | String |
| Status | To indicate this transaction is success or fail | String |
| AuthIDResponse | Authorization code | String |
| TransferAmount | Amount of this transaction | String |
| TransferCurrency | Currency of the amount | String |
| TransferDate | To indicate when this transaction had transferred | String |
| RemID | Gkash Transaction’s ID | String |
| TraceNo | Trace number | String |
| ResponseOrderNumber | This transaction’s order number | String |
| ApplicationId | Application ID | String |
| Message | Description of the transaction’s status | String |
| TxType | Transaction type | String |
| Method | Transaction method | String |

## Setup Gkash Business settings

Make sure to turn on Server mode in Gkash Business settings before initializing the SDK.

![ss](https://github.com/gkashmy/gkash-client-tcp-softpos-windows/assets/72077476/039f3517-12db-468d-b800-aee0293a1361)

## License
[Apache 2.0](https://choosealicense.com/licenses/apache-2.0/)
