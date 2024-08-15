Create a page object model for the `Login Page` .

Page Name: LoginPage
Elements:
- Username input field with selector '#username'
- Password input field with selector '#password'
- Login button with selector 'button[type='submit']'
Methods:
- Login method that takes username and password as parameters, fills in the fields, and clicks the login button.

---

using Microsoft.Playwright;

public class LoginPage : BasePage
{
    private readonly IPage _page;

    public LoginPage(IPage page)
    {
        _page = page;
    }

    private ILocator UsernameInput => _page.Locator("#username");
    private ILocator PasswordInput => _page.Locator("#password");
    private ILocator LoginButton => _page.Locator("button[type='submit']");

    public async Task LoginAsync(string username, string password)
    {
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }
}