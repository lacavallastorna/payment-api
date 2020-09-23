<title>Swagger Demo Payment API</title>

<style type="text/css"> body {	font-family: Trebuchet MS, sans-serif;	font-size: 15px;	color: #444;	margin-right: 24px; } h1	{	font-size: 25px; } h2	{	font-size: 20px; } h3	{	font-size: 16px;	font-weight: bold; } hr	{	height: 1px;	border: 0;	color: #ddd;	background-color: #ddd; } .app-desc { clear: both; margin-left: 20px; } .param-name { width: 100%; } .license-info { margin-left: 20px; } .license-url { margin-left: 20px; } .model { margin: 0 0 0px 20px; } .method { margin-left: 20px; } .method-notes	{	margin: 10px 0 20px 0;	font-size: 90%;	color: #555; } pre { padding: 10px; margin-bottom: 2px; } .http-method { text-transform: uppercase; } pre.get { background-color: #0f6ab4; } pre.post { background-color: #10a54a; } pre.put { background-color: #c5862b; } pre.delete { background-color: #a41e22; } .huge	{	color: #fff; } pre.example { background-color: #f3f3f3; padding: 10px; border: 1px solid #ddd; } code { white-space: pre; } .nickname { font-weight: bold; } .method-path { font-size: 1.5em; background-color: #0f6ab4; } .up { float:right; } .parameter { width: 500px; } .param { width: 500px; padding: 10px 0 0 20px; font-weight: bold; } .param-desc { width: 700px; padding: 0 0 0 20px; color: #777; } .param-type { font-style: italic; } .param-enum-header { width: 700px; padding: 0 0 0 60px; color: #777; font-weight: bold; } .param-enum { width: 700px; padding: 0 0 0 80px; color: #777; font-style: italic; } .field-label { padding: 0; margin: 0; clear: both; } .field-items	{	padding: 0 0 15px 0;	margin-bottom: 15px; } .return-type { clear: both; padding-bottom: 10px; } .param-header { font-weight: bold; } .method-tags { text-align: right; } .method-tag { background: none repeat scroll 0% 0% #24A600; border-radius: 3px; padding: 2px 10px; margin: 2px; color: #FFF; display: inline-block; text-decoration: none; } </style>

# Swagger Demo Payment API

<div class="app-desc">Swagger Demo Payment API</div>

<div class="app-desc">More information: <a href="http://www.tudorin.net">http://www.tudorin.net</a></div>

<div class="app-desc">Contact Info: <a href="wtudorin@gmail.com">wtudorin@gmail.com</a></div>

<div class="app-desc">Version: v1</div>

<div class="license-info">Open License</div>

<div class="license-url">http://www.tudorin.net</div>

## Access

## 

 [ Jump to [Models](<#__Models>) ] ### Table of Contents 

<div class="method-summary"></div>

#### [Account](<#Account>)

- [`<span class="http-method">get</span> /api/account/balance/{accountId}`](<#apiAccountBalanceAccountIdGet>)
- [`<span class="http-method">post</span> /api/account/create`](<#apiAccountCreatePost>)

<!-- -->

#### [Deposit](<#Deposit>)

- [`<span class="http-method">post</span> /api/deposit/create`](<#apiDepositCreatePost>)

<!-- -->

#### [Payment](<#Payment>)

- [`<span class="http-method">put</span> /api/payment/cancel`](<#apiPaymentCancelPut>)
- [`<span class="http-method">post</span> /api/payment/create`](<#apiPaymentCreatePost>)
- [`<span class="http-method">put</span> /api/payment/process`](<#apiPaymentProcessPut>)

<!-- -->

# 

<div class="method"><a name="apiAccountBalanceAccountIdGet"></a><div class="method-path"><a class="up" href="#__Methods">Up</a><precode language="" precodenum="0"></precode></div><div class="method-summary">Gets the Account Balance and list of Payment Transactions (<span class="nickname">apiAccountBalanceAccountIdGet</span>)</div><div class="method-notes"></div><h3 class="field-label">Path parameters</h3><div class="field-items"><div class="param">accountId (required)</div><div class="param-desc"><span class="param-type">Path Parameter</span> — The Id of the Account for which the request is performed. format: int32</div>¨NBSP;</div><!-- field-items --><h3 class="field-label">Return type</h3><div class="return-type"><a href="#AccountBalanceResultDto">AccountBalanceResultDto</a></div><!--Todo: process Response Object and its headers, schema, examples --><h3 class="field-label">Example data</h3><div class="example-data-content-type">Content-Type: application/json</div><precode language="" precodenum="1"></precode><h3 class="field-label">Produces</h3> This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. <ul><li><code>text/plain</code></li><li><code>application/json</code></li><li><code>text/json</code></li></ul><h3 class="field-label">Responses</h3><h4 class="field-label">200</h4> Success <a href="#AccountBalanceResultDto">AccountBalanceResultDto</a><h4 class="field-label">400</h4> Bad Request <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">404</h4> Not Found <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">default</h4> Error <a href="#ProblemDetails">ProblemDetails</a></div>

<!-- method -->

---

<div class="method"><a name="apiAccountCreatePost"></a><div class="method-path"><a class="up" href="#__Methods">Up</a><precode language="" precodenum="2"></precode></div><div class="method-summary">Creates a new Account (<span class="nickname">apiAccountCreatePost</span>)</div><div class="method-notes"></div><h3 class="field-label">Consumes</h3> This API call consumes the following media types via the <span class="header">Content-Type</span> request header: <ul><li><code>application/json</code></li><li><code>text/json</code></li><li><code>application/*+json</code></li></ul><h3 class="field-label">Request body</h3><div class="field-items"><div class="param">body <a href="#AccountInsertDto">AccountInsertDto</a> (optional)</div><div class="param-desc"><span class="param-type">Body Parameter</span> — </div></div><!-- field-items --><h3 class="field-label">Return type</h3><div class="return-type"><a href="#AccountInsertResultDto">AccountInsertResultDto</a></div><!--Todo: process Response Object and its headers, schema, examples --><h3 class="field-label">Example data</h3><div class="example-data-content-type">Content-Type: application/json</div><precode language="" precodenum="3"></precode><h3 class="field-label">Produces</h3> This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. <ul><li><code>text/plain</code></li><li><code>application/json</code></li><li><code>text/json</code></li></ul><h3 class="field-label">Responses</h3><h4 class="field-label">201</h4> Success <a href="#AccountInsertResultDto">AccountInsertResultDto</a><h4 class="field-label">500</h4> Server Error <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">default</h4> Error <a href="#ProblemDetails">ProblemDetails</a></div>

<!-- method -->

---

# 

<div class="method"><a name="apiDepositCreatePost"></a><div class="method-path"><a class="up" href="#__Methods">Up</a><precode language="" precodenum="4"></precode></div><div class="method-summary">Creates a Deposit Transaction for the given Account (<span class="nickname">apiDepositCreatePost</span>)</div><div class="method-notes"></div><h3 class="field-label">Consumes</h3> This API call consumes the following media types via the <span class="header">Content-Type</span> request header: <ul><li><code>application/json</code></li><li><code>text/json</code></li><li><code>application/*+json</code></li></ul><h3 class="field-label">Request body</h3><div class="field-items"><div class="param">body <a href="#TransactionInsertDto">TransactionInsertDto</a> (optional)</div><div class="param-desc"><span class="param-type">Body Parameter</span> — Json object with transaction details, such as AccountID, Amount and Date </div></div><!-- field-items --><h3 class="field-label">Return type</h3><div class="return-type"><a href="#TransactionResultDto">TransactionResultDto</a></div><!--Todo: process Response Object and its headers, schema, examples --><h3 class="field-label">Example data</h3><div class="example-data-content-type">Content-Type: application/json</div><precode language="" precodenum="5"></precode><h3 class="field-label">Produces</h3> This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. <ul><li><code>text/plain</code></li><li><code>application/json</code></li><li><code>text/json</code></li></ul><h3 class="field-label">Responses</h3><h4 class="field-label">201</h4> Success <a href="#TransactionResultDto">TransactionResultDto</a><h4 class="field-label">400</h4> Bad Request <a href="#TransactionResultDto">TransactionResultDto</a><h4 class="field-label">404</h4> Not Found <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">500</h4> Server Error <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">default</h4> Error <a href="#ProblemDetails">ProblemDetails</a></div>

<!-- method -->

---

# 

<div class="method"><a name="apiPaymentCancelPut"></a><div class="method-path"><a class="up" href="#__Methods">Up</a><precode language="" precodenum="6"></precode></div><div class="method-summary">Attempts to Cancel a Payment with a Status of Pending. If successful the Status of the Payment Transaction is changed to Closed and the ClosedComment field updated with specified Reason text. (<span class="nickname">apiPaymentCancelPut</span>)</div><div class="method-notes"></div><h3 class="field-label">Consumes</h3> This API call consumes the following media types via the <span class="header">Content-Type</span> request header: <ul><li><code>application/json</code></li><li><code>text/json</code></li><li><code>application/*+json</code></li></ul><h3 class="field-label">Request body</h3><div class="field-items"><div class="param">body <a href="#TransactionCancelDto">TransactionCancelDto</a> (optional)</div><div class="param-desc"><span class="param-type">Body Parameter</span> — Json object with transaction details, such as AccountID, and Transaction Id and Reason for Cancellation </div></div><!-- field-items --><h3 class="field-label">Return type</h3><div class="return-type"><a href="#TransactionResultDto">TransactionResultDto</a></div><!--Todo: process Response Object and its headers, schema, examples --><h3 class="field-label">Example data</h3><div class="example-data-content-type">Content-Type: application/json</div><precode language="" precodenum="7"></precode><h3 class="field-label">Produces</h3> This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. <ul><li><code>text/plain</code></li><li><code>application/json</code></li><li><code>text/json</code></li></ul><h3 class="field-label">Responses</h3><h4 class="field-label">200</h4> Success <a href="#TransactionResultDto">TransactionResultDto</a><h4 class="field-label">400</h4> Bad Request <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">404</h4> Not Found <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">500</h4> Server Error <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">default</h4> Error <a href="#ProblemDetails">ProblemDetails</a></div>

<!-- method -->

---

<div class="method"><a name="apiPaymentCreatePost"></a><div class="method-path"><a class="up" href="#__Methods">Up</a><precode language="" precodenum="8"></precode></div><div class="method-summary">Creates a new Payment Request. If successful the transaction is created with a Status of Pending (<span class="nickname">apiPaymentCreatePost</span>)</div><div class="method-notes"></div><h3 class="field-label">Consumes</h3> This API call consumes the following media types via the <span class="header">Content-Type</span> request header: <ul><li><code>application/json</code></li><li><code>text/json</code></li><li><code>application/*+json</code></li></ul><h3 class="field-label">Request body</h3><div class="field-items"><div class="param">body <a href="#TransactionInsertDto">TransactionInsertDto</a> (optional)</div><div class="param-desc"><span class="param-type">Body Parameter</span> — Json object with transaction details, such as AccountID, Amount and Date </div></div><!-- field-items --><h3 class="field-label">Return type</h3><div class="return-type"><a href="#TransactionResultDto">TransactionResultDto</a></div><!--Todo: process Response Object and its headers, schema, examples --><h3 class="field-label">Example data</h3><div class="example-data-content-type">Content-Type: application/json</div><precode language="" precodenum="9"></precode><h3 class="field-label">Produces</h3> This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. <ul><li><code>text/plain</code></li><li><code>application/json</code></li><li><code>text/json</code></li></ul><h3 class="field-label">Responses</h3><h4 class="field-label">201</h4> Success <a href="#TransactionResultDto">TransactionResultDto</a><h4 class="field-label">400</h4> Bad Request <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">404</h4> Not Found <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">500</h4> Server Error <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">default</h4> Error <a href="#ProblemDetails">ProblemDetails</a></div>

<!-- method -->

---

<div class="method"><a name="apiPaymentProcessPut"></a><div class="method-path"><a class="up" href="#__Methods">Up</a><precode language="" precodenum="10"></precode></div><div class="method-summary">Attempts to change the status of a Payment Transaction to Processed which if successful will decrease accordingly the Account Balance. (<span class="nickname">apiPaymentProcessPut</span>)</div><div class="method-notes"></div><h3 class="field-label">Consumes</h3> This API call consumes the following media types via the <span class="header">Content-Type</span> request header: <ul><li><code>application/json</code></li><li><code>text/json</code></li><li><code>application/*+json</code></li></ul><h3 class="field-label">Request body</h3><div class="field-items"><div class="param">body <a href="#TransactionProcessDto">TransactionProcessDto</a> (optional)</div><div class="param-desc"><span class="param-type">Body Parameter</span> — Json object with transaction details, such as AccountID, and Transaction Id </div></div><!-- field-items --><h3 class="field-label">Return type</h3><div class="return-type"><a href="#TransactionResultDto">TransactionResultDto</a></div><!--Todo: process Response Object and its headers, schema, examples --><h3 class="field-label">Example data</h3><div class="example-data-content-type">Content-Type: application/json</div><precode language="" precodenum="11"></precode><h3 class="field-label">Produces</h3> This API call produces the following media types according to the <span class="header">Accept</span> request header; the media type will be conveyed by the <span class="header">Content-Type</span> response header. <ul><li><code>text/plain</code></li><li><code>application/json</code></li><li><code>text/json</code></li></ul><h3 class="field-label">Responses</h3><h4 class="field-label">200</h4> Success <a href="#TransactionResultDto">TransactionResultDto</a><h4 class="field-label">400</h4> Bad Request <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">404</h4> Not Found <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">500</h4> Server Error <a href="#ErrorResponseDto">ErrorResponseDto</a><h4 class="field-label">default</h4> Error <a href="#ProblemDetails">ProblemDetails</a></div>

<!-- method -->

---

## 

 [ Jump to [Methods](<#__Methods>) ] ### Table of Contents

1. [`AccountBalanceResultDto`](<#AccountBalanceResultDto>)
2. [`AccountInsertDto`](<#AccountInsertDto>)
3. [`AccountInsertResultDto`](<#AccountInsertResultDto>)
4. [`DepositTransactionResultDto`](<#DepositTransactionResultDto>)
5. [`ErrorResponseDto`](<#ErrorResponseDto>)
6. [`PaymentTransactionResultDto`](<#PaymentTransactionResultDto>)
7. [`ProblemDetails`](<#ProblemDetails>)
8. [`TransactionCancelDto`](<#TransactionCancelDto>)
9. [`TransactionInsertDto`](<#TransactionInsertDto>)
10. [`TransactionProcessDto`](<#TransactionProcessDto>)
11. [`TransactionResultDto`](<#TransactionResultDto>)

<!-- -->

<div class="model"><h3><a name="AccountBalanceResultDto"><code>AccountBalanceResultDto</code></a><a class="up" href="#__Models">Up</a></h3><div class="field-items"><div class="param">accountId (optional)</div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">openingBalance (optional)</div><div class="param-desc"><span class="param-type"><a href="#double">Double</a></span> format: double</div><div class="param">processedPaymentsBalance (optional)</div><div class="param-desc"><span class="param-type"><a href="#double">Double</a></span> format: double</div><div class="param">pendingdPaymentsBalance (optional)</div><div class="param-desc"><span class="param-type"><a href="#double">Double</a></span> format: double</div><div class="param">closingBalance (optional)</div><div class="param-desc"><span class="param-type"><a href="#double">Double</a></span> format: double</div><div class="param">deposits (optional)</div><div class="param-desc"><span class="param-type"><a href="#DepositTransactionResultDto">array[DepositTransactionResultDto]</a></span></div><div class="param">payments (optional)</div><div class="param-desc"><span class="param-type"><a href="#PaymentTransactionResultDto">array[PaymentTransactionResultDto]</a></span></div></div><!-- field-items --></div>

<div class="model"><h3><a name="AccountInsertDto"><code>AccountInsertDto</code></a><a class="up" href="#__Models">Up</a></h3><div class="field-items"><div class="param">name </div><div class="param-desc"><span class="param-type"><a href="#string">String</a></span></div></div><!-- field-items --></div>

<div class="model"><h3><a name="AccountInsertResultDto"><code>AccountInsertResultDto</code></a><a class="up" href="#__Models">Up</a></h3><div class="field-items"><div class="param">accountId (optional)</div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">name (optional)</div><div class="param-desc"><span class="param-type"><a href="#string">String</a></span></div></div><!-- field-items --></div>

<div class="model"><h3><a name="DepositTransactionResultDto"><code>DepositTransactionResultDto</code></a><a class="up" href="#__Models">Up</a></h3><div class="field-items"><div class="param">transactionStatus (optional)</div><div class="param-desc"><span class="param-type"><a href="#string">String</a></span></div><div class="param">closedReason (optional)</div><div class="param-desc"><span class="param-type"><a href="#string">String</a></span></div><div class="param">id (optional)</div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">accountId (optional)</div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">amount (optional)</div><div class="param-desc"><span class="param-type"><a href="#double">Double</a></span> format: double</div><div class="param">date (optional)</div><div class="param-desc"><span class="param-type"><a href="#DateTime">Date</a></span> format: date-time</div></div><!-- field-items --></div>

<div class="model"><h3><a name="ErrorResponseDto"><code>ErrorResponseDto</code></a><a class="up" href="#__Models">Up</a></h3><div class="field-items"><div class="param">message (optional)</div><div class="param-desc"><span class="param-type"><a href="#string">String</a></span></div></div><!-- field-items --></div>

<div class="model"><h3><a name="PaymentTransactionResultDto"><code>PaymentTransactionResultDto</code></a><a class="up" href="#__Models">Up</a></h3><div class="field-items"><div class="param">id (optional)</div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">accountId (optional)</div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">amount (optional)</div><div class="param-desc"><span class="param-type"><a href="#double">Double</a></span> format: double</div><div class="param">date (optional)</div><div class="param-desc"><span class="param-type"><a href="#DateTime">Date</a></span> format: date-time</div><div class="param">transactionStatus (optional)</div><div class="param-desc"><span class="param-type"><a href="#string">String</a></span></div><div class="param">closedReason (optional)</div><div class="param-desc"><span class="param-type"><a href="#string">String</a></span></div></div><!-- field-items --></div>

<div class="model"><h3><a name="ProblemDetails"><code>ProblemDetails</code></a><a class="up" href="#__Models">Up</a></h3><div class="field-items"></div><!-- field-items --></div>

<div class="model"><h3><a name="TransactionCancelDto"><code>TransactionCancelDto</code></a><a class="up" href="#__Models">Up</a></h3><div class="field-items"><div class="param">accountId </div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">transactionId </div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">reason (optional)</div><div class="param-desc"><span class="param-type"><a href="#string">String</a></span></div></div><!-- field-items --></div>

<div class="model"><h3><a name="TransactionInsertDto"><code>TransactionInsertDto</code></a><a class="up" href="#__Models">Up</a></h3><div class="field-items"><div class="param">accountId </div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">amount </div><div class="param-desc"><span class="param-type"><a href="#double">Double</a></span> format: double</div><div class="param">date </div><div class="param-desc"><span class="param-type"><a href="#DateTime">Date</a></span> format: date-time</div></div><!-- field-items --></div>

<div class="model"><h3><a name="TransactionProcessDto"><code>TransactionProcessDto</code></a><a class="up" href="#__Models">Up</a></h3><div class="field-items"><div class="param">accountId </div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">transactionId </div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div></div><!-- field-items --></div>

<div class="model"><h3><a name="TransactionResultDto"><code>TransactionResultDto</code></a><a class="up" href="#__Models">Up</a></h3><div class="field-items"><div class="param">transactionStatus (optional)</div><div class="param-desc"><span class="param-type"><a href="#string">String</a></span></div><div class="param">closedReason (optional)</div><div class="param-desc"><span class="param-type"><a href="#string">String</a></span></div><div class="param">id (optional)</div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">accountId (optional)</div><div class="param-desc"><span class="param-type"><a href="#integer">Integer</a></span> format: int32</div><div class="param">amount (optional)</div><div class="param-desc"><span class="param-type"><a href="#double">Double</a></span> format: double</div><div class="param">date (optional)</div><div class="param-desc"><span class="param-type"><a href="#DateTime">Date</a></span> format: date-time</div></div><!-- field-items --></div>

