Create a page object model for the `Email Compose Page`.

Page Name: EmailComposePage
Elements:
- Recipient Field (selector: '#recipient')
- CC Field (selector: '#cc')
- BCC Field (selector: '#bcc')
- Subject Field (selector: '#subject')
- Message Body (selector: '#message-body')
- Attachment Button (selector: '#attach-file')
- Send Button (selector: '#send-email')
- Draft Button (selector: '#save-draft')
- Formatting Toolbar (selector: '.formatting-toolbar')
  - Bold Button (selector: '.bold-button')
  - Italic Button (selector: '.italic-button')
  - Underline Button (selector: '.underline-button')
Methods:
- ComposeEmail method that takes recipient, subject, and body as parameters and fills in the corresponding fields.
- AddAttachment method that simulates adding an attachment to the email.
- SendEmail method that clicks the Send button.
- SaveDraft method that clicks the Draft button.
- FormatText method that takes a formatting option (bold, italic, or underline) and applies it to the selected text.

---

using Microsoft.Playwright;

public class EmailComposePage : BasePage
{
    private readonly IPage _page;

    public EmailComposePage(IPage page) : base(page)
    {
        _page = page;
    }

    public class EmailForm : BaseElement
    {
        private readonly ILocator _locator;

        public EmailForm(ILocator locator) : base(locator)
        {
            _locator = locator;
        }

        public ILocator RecipientField => _locator.Locator("#recipient");
        public ILocator CCField => _locator.Locator("#cc");
        public ILocator BCCField => _locator.Locator("#bcc");
        public ILocator SubjectField => _locator.Locator("#subject");
        public ILocator MessageBody => _locator.Locator("#message-body");
        public ILocator AttachmentButton => _locator.Locator("#attach-file");
        public ILocator SendButton => _locator.Locator("#send-email");
        public ILocator DraftButton => _locator.Locator("#save-draft");
    }

    public class FormattingToolbar : BaseElement
    {
        private readonly ILocator _locator;

        public FormattingToolbar(ILocator locator) : base(locator)
        {
            _locator = locator;
        }

        public ILocator BoldButton => _locator.Locator(".bold-button");
        public ILocator ItalicButton => _locator.Locator(".italic-button");
        public ILocator UnderlineButton => _locator.Locator(".underline-button");
    }

    public EmailForm EmailForm => new EmailForm(_page.Locator("form"));
    public FormattingToolbar FormattingToolbar => new FormattingToolbar(_page.Locator(".formatting-toolbar"));

    public async Task ComposeEmailAsync(string recipient, string subject, string body)
    {
        await EmailForm.RecipientField.FillAsync(recipient);
        await EmailForm.SubjectField.FillAsync(subject);
        await EmailForm.MessageBody.FillAsync(body);
    }

    public async Task AddAttachmentAsync()
    {
        await EmailForm.AttachmentButton.ClickAsync();
        // Simulating file selection - in a real scenario, you'd need to handle the file dialog
        await _page.WaitForTimeoutAsync(1000); // Simulating delay for file selection
    }

    public async Task SendEmailAsync()
    {
        await EmailForm.SendButton.ClickAsync();
    }

    public async Task SaveDraftAsync()
    {
        await EmailForm.DraftButton.ClickAsync();
    }

    public async Task FormatTextAsync(string formatOption)
    {
        switch (formatOption.ToLower())
        {
            case "bold":
                await FormattingToolbar.BoldButton.ClickAsync();
                break;
            case "italic":
                await FormattingToolbar.ItalicButton.ClickAsync();
                break;
            case "underline":
                await FormattingToolbar.UnderlineButton.ClickAsync();
                break;
            default:
                throw new ArgumentException("Invalid formatting option");
        }
    }
}