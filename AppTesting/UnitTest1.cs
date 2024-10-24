using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
namespace AppTesting
{
    [TestClass]
    public class UnitTest1 : PageTest
    {
        [TestMethod]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        {
            await Page.GotoAsync("http://localhost:7212");
            var connectButton = Page.Locator("button:text('Connect')");
            await Expect(connectButton).ToBeVisibleAsync();


            var emailInput = Page.Locator("input[placeholder='Email']");
            var passwordInput = Page.Locator("input[placeholder='Password']");


            await emailInput.FillAsync("testuser@example.com");
            await passwordInput.FillAsync("password");

            await connectButton.ClickAsync();

            await Expect(Page).ToHaveURLAsync("http://localhost:7212/");

        }
    }
}
