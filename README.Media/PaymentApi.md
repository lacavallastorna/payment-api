# Swagger Demo Payment API

<div class="app-desc">Swagger Demo Payment API</div>

<div class="app-desc">More information: [http://www.tudorin.net](http://www.tudorin.net)</div>

<div class="app-desc">Contact Info: [wtudorin@gmail.com](wtudorin@gmail.com)</div>

<div class="app-desc">Version: v1</div>

<div class="license-info">Open License</div>

<div class="license-url">http://www.tudorin.net</div>

## Access ## <a name="__Methods">Methods</a> [ Jump to [Models](#__Models) ] ### Table of Contents #### [Account](#Account) * [`<span class="http-method">get</span> /api/account/balance/{accountId}`](#apiAccountBalanceAccountIdGet) * [`<span class="http-method">post</span> /api/account/create`](#apiAccountCreatePost) #### [Deposit](#Deposit) * [`<span class="http-method">post</span> /api/deposit/create`](#apiDepositCreatePost) #### [Payment](#Payment) * [`<span class="http-method">put</span> /api/payment/cancel`](#apiPaymentCancelPut) * [`<span class="http-method">post</span> /api/payment/create`](#apiPaymentCreatePost) * [`<span class="http-method">put</span> /api/payment/process`](#apiPaymentProcessPut) # <a name="Account">Account</a>

<div class="method"><a name="apiAccountBalanceAccountIdGet"></a>

<div class="method-path">[Up](#__Methods) get /api/account/balance/{accountId}</div>

<div class="method-summary">Gets the Account Balance and list of Payment Transactions (<span class="nickname">apiAccountBalanceAccountIdGet</span>)</div>

### Path parameters

<div class="field-items">

<div class="param">accountId (required)</div>

<div class="param-desc"><span class="param-type">Path Parameter</span> — The Id of the Account for which the request is performed. format: int32</div>

</div>

### Return type

<div class="return-type">[AccountBalanceResultDto](#AccountBalanceResultDto)</div>

### Example data

<div class="example-data-content-type">Content-Type: application/json</div>

{ "accountId" : 0, "processedPaymentsBalance" : 1.4658129805029452, "payments" : [ { "date" : "2000-01-23T04:56:07.000+00:00", "accountId" : 2, "amount" : 4.145608029883936, "closedReason" : "closedReason", "transactionStatus" : "transactionStatus", "id" : 3 }, { "date" : "2000-01-23T04:56:07.000+00:00", "accountId" : 2, "amount" : 4.145608029883936, "closedReason" : "closedReason", "transactionStatus" : "transactionStatus", "id" : 3 } ], "pendingdPaymentsBalance" : 5.962133916683182, "closingBalance" : 5.637376656633329, "openingBalance" : 6.027456183070403, "deposits" : [ { "date" : "2000-01-23T04:56:07.000+00:00", "accountId" : 7, "amount" : 9.301444243932576, "closedReason" : "closedReason", "transactionStatus" : "transactionStatus", "id" : 2 }, { "date" : "2000-01-23T04:56:07.000+00:00", "accountId" : 7, "amount" : 9.301444243932576, "closedReason" : "closedReason", "transactionStatus" : "transactionStatus", "id" : 2 } ] } ### Produces This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. * `text/plain` * `application/json` * `text/json` ### Responses #### 200 Success [AccountBalanceResultDto](#AccountBalanceResultDto) #### 400 Bad Request [ErrorResponseDto](#ErrorResponseDto) #### 404 Not Found [ErrorResponseDto](#ErrorResponseDto) #### default Error [ProblemDetails](#ProblemDetails)</div>

* * *

<div class="method"><a name="apiAccountCreatePost"></a>

<div class="method-path">[Up](#__Methods) post /api/account/create</div>

<div class="method-summary">Creates a new Account (<span class="nickname">apiAccountCreatePost</span>)</div>

### Consumes This API call consumes the following media types via the <span class="header">Content-Type</span> request header: * `application/json` * `text/json` * `application/*+json` ### Request body

<div class="field-items">

<div class="param">body [AccountInsertDto](#AccountInsertDto) (optional)</div>

<div class="param-desc"><span class="param-type">Body Parameter</span> —</div>

</div>

### Return type

<div class="return-type">[AccountInsertResultDto](#AccountInsertResultDto)</div>

### Example data

<div class="example-data-content-type">Content-Type: application/json</div>

{ "accountId" : 0, "name" : "name" } ### Produces This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. * `text/plain` * `application/json` * `text/json` ### Responses #### 201 Success [AccountInsertResultDto](#AccountInsertResultDto) #### 500 Server Error [ErrorResponseDto](#ErrorResponseDto) #### default Error [ProblemDetails](#ProblemDetails)</div>

* * * # <a name="Deposit">Deposit</a>

<div class="method"><a name="apiDepositCreatePost"></a>

<div class="method-path">[Up](#__Methods) post /api/deposit/create</div>

<div class="method-summary">Creates a Deposit Transaction for the given Account (<span class="nickname">apiDepositCreatePost</span>)</div>

### Consumes This API call consumes the following media types via the <span class="header">Content-Type</span> request header: * `application/json` * `text/json` * `application/*+json` ### Request body

<div class="field-items">

<div class="param">body [TransactionInsertDto](#TransactionInsertDto) (optional)</div>

<div class="param-desc"><span class="param-type">Body Parameter</span> — Json object with transaction details, such as AccountID, Amount and Date</div>

</div>

### Return type

<div class="return-type">[TransactionResultDto](#TransactionResultDto)</div>

### Example data

<div class="example-data-content-type">Content-Type: application/json</div>

{ "date" : "2000-01-23T04:56:07.000+00:00", "accountId" : 6, "amount" : 1.4658129805029452, "closedReason" : "closedReason", "transactionStatus" : "transactionStatus", "id" : 0 } ### Produces This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. * `text/plain` * `application/json` * `text/json` ### Responses #### 201 Success [TransactionResultDto](#TransactionResultDto) #### 400 Bad Request [TransactionResultDto](#TransactionResultDto) #### 404 Not Found [ErrorResponseDto](#ErrorResponseDto) #### 500 Server Error [ErrorResponseDto](#ErrorResponseDto) #### default Error [ProblemDetails](#ProblemDetails)</div>

* * * # <a name="Payment">Payment</a>

<div class="method"><a name="apiPaymentCancelPut"></a>

<div class="method-path">[Up](#__Methods) put /api/payment/cancel</div>

<div class="method-summary">Attempts to Cancel a Payment with a Status of Pending. If successful the Status of the Payment Transaction is changed to Closed and the ClosedComment field updated with specified Reason text. (<span class="nickname">apiPaymentCancelPut</span>)</div>

### Consumes This API call consumes the following media types via the <span class="header">Content-Type</span> request header: * `application/json` * `text/json` * `application/*+json` ### Request body

<div class="field-items">

<div class="param">body [TransactionCancelDto](#TransactionCancelDto) (optional)</div>

<div class="param-desc"><span class="param-type">Body Parameter</span> — Json object with transaction details, such as AccountID, and Transaction Id and Reason for Cancellation</div>

</div>

### Return type

<div class="return-type">[TransactionResultDto](#TransactionResultDto)</div>

### Example data

<div class="example-data-content-type">Content-Type: application/json</div>

{ "date" : "2000-01-23T04:56:07.000+00:00", "accountId" : 6, "amount" : 1.4658129805029452, "closedReason" : "closedReason", "transactionStatus" : "transactionStatus", "id" : 0 } ### Produces This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. * `text/plain` * `application/json` * `text/json` ### Responses #### 200 Success [TransactionResultDto](#TransactionResultDto) #### 400 Bad Request [ErrorResponseDto](#ErrorResponseDto) #### 404 Not Found [ErrorResponseDto](#ErrorResponseDto) #### 500 Server Error [ErrorResponseDto](#ErrorResponseDto) #### default Error [ProblemDetails](#ProblemDetails)</div>

* * *

<div class="method"><a name="apiPaymentCreatePost"></a>

<div class="method-path">[Up](#__Methods) post /api/payment/create</div>

<div class="method-summary">Creates a new Payment Request. If successful the transaction is created with a Status of Pending (<span class="nickname">apiPaymentCreatePost</span>)</div>

### Consumes This API call consumes the following media types via the <span class="header">Content-Type</span> request header: * `application/json` * `text/json` * `application/*+json` ### Request body

<div class="field-items">

<div class="param">body [TransactionInsertDto](#TransactionInsertDto) (optional)</div>

<div class="param-desc"><span class="param-type">Body Parameter</span> — Json object with transaction details, such as AccountID, Amount and Date</div>

</div>

### Return type

<div class="return-type">[TransactionResultDto](#TransactionResultDto)</div>

### Example data

<div class="example-data-content-type">Content-Type: application/json</div>

{ "date" : "2000-01-23T04:56:07.000+00:00", "accountId" : 6, "amount" : 1.4658129805029452, "closedReason" : "closedReason", "transactionStatus" : "transactionStatus", "id" : 0 } ### Produces This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. * `text/plain` * `application/json` * `text/json` ### Responses #### 201 Success [TransactionResultDto](#TransactionResultDto) #### 400 Bad Request [ErrorResponseDto](#ErrorResponseDto) #### 404 Not Found [ErrorResponseDto](#ErrorResponseDto) #### 500 Server Error [ErrorResponseDto](#ErrorResponseDto) #### default Error [ProblemDetails](#ProblemDetails)</div>

* * *

<div class="method"><a name="apiPaymentProcessPut"></a>

<div class="method-path">[Up](#__Methods) put /api/payment/process</div>

<div class="method-summary">Attempts to change the status of a Payment Transaction to Processed which if successful will decrease accordingly the Account Balance. (<span class="nickname">apiPaymentProcessPut</span>)</div>

### Consumes This API call consumes the following media types via the <span class="header">Content-Type</span> request header: * `application/json` * `text/json` * `application/*+json` ### Request body

<div class="field-items">

<div class="param">body [TransactionProcessDto](#TransactionProcessDto) (optional)</div>

<div class="param-desc"><span class="param-type">Body Parameter</span> — Json object with transaction details, such as AccountID, and Transaction Id</div>

</div>

### Return type

<div class="return-type">[TransactionResultDto](#TransactionResultDto)</div>

### Example data

<div class="example-data-content-type">Content-Type: application/json</div>

{ "date" : "2000-01-23T04:56:07.000+00:00", "accountId" : 6, "amount" : 1.4658129805029452, "closedReason" : "closedReason", "transactionStatus" : "transactionStatus", "id" : 0 } ### Produces This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. * `text/plain` * `application/json` * `text/json` ### Responses #### 200 Success [TransactionResultDto](#TransactionResultDto) #### 400 Bad Request [ErrorResponseDto](#ErrorResponseDto) #### 404 Not Found [ErrorResponseDto](#ErrorResponseDto) #### 500 Server Error [ErrorResponseDto](#ErrorResponseDto) #### default Error [ProblemDetails](#ProblemDetails)</div>

* * * ## <a name="__Models">Models</a> [ Jump to [Methods](#__Methods) ] ### Table of Contents 1\. [`AccountBalanceResultDto`](#AccountBalanceResultDto) 2\. [`AccountInsertDto`](#AccountInsertDto) 3\. [`AccountInsertResultDto`](#AccountInsertResultDto) 4\. [`DepositTransactionResultDto`](#DepositTransactionResultDto) 5\. [`ErrorResponseDto`](#ErrorResponseDto) 6\. [`PaymentTransactionResultDto`](#PaymentTransactionResultDto) 7\. [`ProblemDetails`](#ProblemDetails) 8\. [`TransactionCancelDto`](#TransactionCancelDto) 9\. [`TransactionInsertDto`](#TransactionInsertDto) 10\. [`TransactionProcessDto`](#TransactionProcessDto) 11\. [`TransactionResultDto`](#TransactionResultDto)

<div class="model">### <a name="AccountBalanceResultDto">`AccountBalanceResultDto`</a> [Up](#__Models)

<div class="field-items">

<div class="param">accountId (optional)</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">openingBalance (optional)</div>

<div class="param-desc"><span class="param-type">[Double](#double)</span> format: double</div>

<div class="param">processedPaymentsBalance (optional)</div>

<div class="param-desc"><span class="param-type">[Double](#double)</span> format: double</div>

<div class="param">pendingdPaymentsBalance (optional)</div>

<div class="param-desc"><span class="param-type">[Double](#double)</span> format: double</div>

<div class="param">closingBalance (optional)</div>

<div class="param-desc"><span class="param-type">[Double](#double)</span> format: double</div>

<div class="param">deposits (optional)</div>

<div class="param-desc"><span class="param-type">[array[DepositTransactionResultDto]](#DepositTransactionResultDto)</span></div>

<div class="param">payments (optional)</div>

<div class="param-desc"><span class="param-type">[array[PaymentTransactionResultDto]](#PaymentTransactionResultDto)</span></div>

</div>

</div>

<div class="model">### <a name="AccountInsertDto">`AccountInsertDto`</a> [Up](#__Models)

<div class="field-items">

<div class="param">name</div>

<div class="param-desc"><span class="param-type">[String](#string)</span></div>

</div>

</div>

<div class="model">### <a name="AccountInsertResultDto">`AccountInsertResultDto`</a> [Up](#__Models)

<div class="field-items">

<div class="param">accountId (optional)</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">name (optional)</div>

<div class="param-desc"><span class="param-type">[String](#string)</span></div>

</div>

</div>

<div class="model">### <a name="DepositTransactionResultDto">`DepositTransactionResultDto`</a> [Up](#__Models)

<div class="field-items">

<div class="param">transactionStatus (optional)</div>

<div class="param-desc"><span class="param-type">[String](#string)</span></div>

<div class="param">closedReason (optional)</div>

<div class="param-desc"><span class="param-type">[String](#string)</span></div>

<div class="param">id (optional)</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">accountId (optional)</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">amount (optional)</div>

<div class="param-desc"><span class="param-type">[Double](#double)</span> format: double</div>

<div class="param">date (optional)</div>

<div class="param-desc"><span class="param-type">[Date](#DateTime)</span> format: date-time</div>

</div>

</div>

<div class="model">### <a name="ErrorResponseDto">`ErrorResponseDto`</a> [Up](#__Models)

<div class="field-items">

<div class="param">message (optional)</div>

<div class="param-desc"><span class="param-type">[String](#string)</span></div>

</div>

</div>

<div class="model">### <a name="PaymentTransactionResultDto">`PaymentTransactionResultDto`</a> [Up](#__Models)

<div class="field-items">

<div class="param">id (optional)</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">accountId (optional)</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">amount (optional)</div>

<div class="param-desc"><span class="param-type">[Double](#double)</span> format: double</div>

<div class="param">date (optional)</div>

<div class="param-desc"><span class="param-type">[Date](#DateTime)</span> format: date-time</div>

<div class="param">transactionStatus (optional)</div>

<div class="param-desc"><span class="param-type">[String](#string)</span></div>

<div class="param">closedReason (optional)</div>

<div class="param-desc"><span class="param-type">[String](#string)</span></div>

</div>

</div>

<div class="model">### <a name="ProblemDetails">`ProblemDetails`</a> [Up](#__Models)</div>

<div class="model">### <a name="TransactionCancelDto">`TransactionCancelDto`</a> [Up](#__Models)

<div class="field-items">

<div class="param">accountId</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">transactionId</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">reason (optional)</div>

<div class="param-desc"><span class="param-type">[String](#string)</span></div>

</div>

</div>

<div class="model">### <a name="TransactionInsertDto">`TransactionInsertDto`</a> [Up](#__Models)

<div class="field-items">

<div class="param">accountId</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">amount</div>

<div class="param-desc"><span class="param-type">[Double](#double)</span> format: double</div>

<div class="param">date</div>

<div class="param-desc"><span class="param-type">[Date](#DateTime)</span> format: date-time</div>

</div>

</div>

<div class="model">### <a name="TransactionProcessDto">`TransactionProcessDto`</a> [Up](#__Models)

<div class="field-items">

<div class="param">accountId</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">transactionId</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

</div>

</div>

<div class="model">### <a name="TransactionResultDto">`TransactionResultDto`</a> [Up](#__Models)

<div class="field-items">

<div class="param">transactionStatus (optional)</div>

<div class="param-desc"><span class="param-type">[String](#string)</span></div>

<div class="param">closedReason (optional)</div>

<div class="param-desc"><span class="param-type">[String](#string)</span></div>

<div class="param">id (optional)</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">accountId (optional)</div>

<div class="param-desc"><span class="param-type">[Integer](#integer)</span> format: int32</div>

<div class="param">amount (optional)</div>

<div class="param-desc"><span class="param-type">[Double](#double)</span> format: double</div>

<div class="param">date (optional)</div>

<div class="param-desc"><span class="param-type">[Date](#DateTime)</span> format: date-time</div>

</div>

</div>