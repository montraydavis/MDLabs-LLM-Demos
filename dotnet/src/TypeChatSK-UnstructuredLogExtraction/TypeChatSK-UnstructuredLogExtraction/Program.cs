var app = LogExtractionApp<AppLogSchema>.CreateInstance();

var appLogs = """
    192.168.1.10 - - [11/Aug/2024:15:45:32 +0000] "GET / HTTP/1.1" 200 5123 "-" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36"

    192.168.1.11 - - [11/Aug/2024:15:46:12 +0000] "GET /account/login HTTP/1.1" 200 2315 "-" "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:91.0) Gecko/20100101 Firefox/91.0"
        Log: AccountController.Login page loaded successfully.
        Log: Displaying login form to user.

    192.168.1.12 - - [11/Aug/2024:15:46:55 +0000] "POST /account/login HTTP/1.1" 401 953 "http://financeapp.com/account/login" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36"
        Stack Trace:
            at FinanceApp.AuthService.ValidateCredentials(User user) in /src/FinanceApp/AuthService.cs:line 54
            at FinanceApp.AccountController.Login(LoginModel model) in /src/FinanceApp/AccountController.cs:line 32
            at Microsoft.AspNetCore.Mvc.ActionResult.ExecuteResult(ActionContext context) in /src/Microsoft.AspNetCore.Mvc.ActionResult.cs:line 128
        Log: Failed login attempt for user ID 12345.

    192.168.1.13 - - [11/Aug/2024:15:47:22 +0000] "POST /account/login HTTP/1.1" 200 3125 "http://financeapp.com/account/login" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36"
        Log: AccountController.Login - User ID 12345 credentials validated successfully.
        Log: User 12345 successfully logged in.
        Log: Redirecting user 12345 to /account/home.

    192.168.1.14 - - [11/Aug/2024:15:48:02 +0000] "GET /account/balance HTTP/1.1" 403 1567 "http://financeapp.com/account/login" "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:91.0) Gecko/20100101 Firefox/91.0"
        Stack Trace:
            at FinanceApp.AuthorizationService.CheckPermissions(User user, String resource) in /src/FinanceApp/AuthorizationService.cs:line 78
            at FinanceApp.AccountController.GetBalance(int accountId) in /src/FinanceApp/AccountController.cs:line 42
            at Microsoft.AspNetCore.Mvc.ActionResult.ExecuteResult(ActionContext context) in /src/Microsoft.AspNetCore.Mvc.ActionResult.cs:line 128
        Log: Unauthorized access attempt to /account/balance by user ID 12345.

    192.168.1.15 - - [11/Aug/2024:15:48:45 +0000] "GET /account/balance HTTP/1.1" 200 3125 "http://financeapp.com/account/home" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36"
        Log: User 12345 accessed account balance successfully.
        Log: Account balance for account ID 12345 retrieved: $10,500.00.

    192.168.1.16 - - [11/Aug/2024:15:49:30 +0000] "GET /api/transactions?account=12345 HTTP/1.1" 500 1240 "-" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36"
        Stack Trace:
            at FinanceApp.TransactionService.GetTransactions(int accountId) in /src/FinanceApp/TransactionService.cs:line 89
            at FinanceApp.Api.TransactionController.GetTransactions(int accountId) in /src/FinanceApp/Api/TransactionController.cs:line 39
            at Microsoft.AspNetCore.Mvc.ActionResult.ExecuteResult(ActionContext context) in /src/Microsoft.AspNetCore.Mvc.ActionResult.cs:line 128
        Exception: System.NullReferenceException: Object reference not set to an instance of an object.
            at FinanceApp.TransactionService.GetTransactions(int accountId) in /src/FinanceApp/TransactionService.cs:line 89
        Log: Error retrieving transactions for account ID 12345.

    192.168.1.17 - - [11/Aug/2024:15:50:15 +0000] "GET /api/transactions?account=54321 HTTP/1.1" 200 3120 "-" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36"
        Log: Transactions for account ID 54321 retrieved successfully.
        Log: Number of transactions returned: 25.

    192.168.1.18 - - [11/Aug/2024:15:50:45 +0000] "POST /account/transfer HTTP/1.1" 503 634 "http://financeapp.com/account/transfer" "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:91.0) Gecko/20100101 Firefox/91.0"
        Stack Trace:
            at FinanceApp.TransferService.TransferFunds(TransferRequest request) in /src/FinanceApp/TransferService.cs:line 123
            at FinanceApp.AccountController.Transfer(TransferModel model) in /src/FinanceApp/AccountController.cs:line 55
            at Microsoft.AspNetCore.Mvc.ActionResult.ExecuteResult(ActionContext context) in /src/Microsoft.AspNetCore.Mvc.ActionResult.cs:line 128
        Exception: System.Net.Http.HttpRequestException: Service Unavailable
            at FinanceApp.TransferService.TransferFunds(TransferRequest request) in /src/FinanceApp/TransferService.cs:line 123
        Log: Transfer service unavailable. User 12345's transfer request failed.

    192.168.1.19 - - [11/Aug/2024:15:51:10 +0000] "POST /account/transfer HTTP/1.1" 200 2750 "http://financeapp.com/account/transfer" "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:91.0) Gecko/20100101 Firefox/91.0"
        Log: Transfer request received from user ID 12345.
        Log: Transfer of $500.00 from account 12345 to account 67890 completed successfully.

    192.168.1.20 - - [11/Aug/2024:15:51:55 +0000] "POST /account/deposit HTTP/1.1" 409 1025 "http://financeapp.com/account/deposit" "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:91.0) Gecko/20100101 Firefox/91.0"
        Stack Trace:
            at FinanceApp.DepositService.ProcessDeposit(DepositRequest request) in /src/FinanceApp/DepositService.cs:line 101
            at FinanceApp.AccountController.Deposit(DepositModel model) in /src/FinanceApp/AccountController.cs:line 70
            at Microsoft.AspNetCore.Mvc.ActionResult.ExecuteResult(ActionContext context) in /src/Microsoft.AspNetCore.Mvc.ActionResult.cs:line 128
        Exception: System.InvalidOperationException: Duplicate transaction detected.
            at FinanceApp.DepositService.ProcessDeposit(DepositRequest request) in /src/FinanceApp/DepositService.cs:line 101
        Log: Duplicate deposit attempt detected for transaction ID 98765.

    192.168.1.21 - - [11/Aug/2024:15:52:45 +0000] "POST /account/deposit HTTP/1.1" 200 2450 "http://financeapp.com/account/deposit" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36"
        Log: Deposit request received from user ID 12345.
        Log: Deposit of $1000.00 into account 12345 completed successfully.

    192.168.1.22 - - [11/Aug/2024:15:53:25 +0000] "GET /account/summary HTTP/1
    """;

var logs = await app.RunAsync("🧠 > Thinking ...", appLogs);

Console.WriteLine();
Console.WriteLine(logs);