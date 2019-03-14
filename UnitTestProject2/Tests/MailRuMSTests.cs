namespace TestWebProject
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestWebProject.Pages;
    using TestWebProject.Webdriver;
    using TestWebProject.BO;

    [TestClass]
    public class MailRuMSTests : BaseTest
    {
        private const string LogIn = "testuser.19";
        private const string Password = "testCDP123";
        private const string EmailAddress = "litmarsd@mail.ru";
        private const string EmailSubject = "Subject";
        private const string EmailText = "EmailTestText";

        [TestMethod, TestCategory("Login")]
        public void LoginTest()
        {
            var startPage = new StartPage();
            startPage.Login(new User(LogIn, Password));

            Assert.IsTrue(startPage.LoginSuccessMarker());
        }

        [TestMethod, TestCategory("Login")]
        public void LoginInvalidUserTest()
        {
            var startPage = new StartPage();
            startPage.Login(InvalidUser.CreateTestUser());

            Assert.IsTrue(startPage.IncorrectCredentialsMarker());
        }

        [TestMethod, TestCategory("Login")]
        public void LoginValidUserTest()
        {
            var startPage = new StartPage();
            startPage.Login(ValidUser.CreateTestUser());

            Assert.IsTrue(startPage.LoginSuccessMarker());
        }

        [TestMethod, TestCategory("Login")]
        public void LoginTestUsingTabs()
        {
            var startPage = new StartPage();
            startPage.LoginUsingTabs(new User(LogIn, Password));

            Assert.IsTrue(startPage.LoginSuccessMarker());
        }

        [TestMethod, TestCategory("EmailCreating")]
        public void CreateDraftEmailTest()
        {
            new StartPage().Login(new User(LogIn, Password));
            new InboxPage().GoToNewEmailPage();

            var emailPage = new EmailPage();
            emailPage.CreateANewEmail(new Email(EmailAddress, EmailSubject, EmailText));
            emailPage.SaveAsADraft();
            emailPage.GoToDraftPage();

            Assert.AreEqual(EmailAddress, new DraftPage().GetEmailAddress());
        }

        [TestMethod, TestCategory("VerificationOfTheDraftContent")]
        public void CompareDraftEmailAddressTest()
        {
            new StartPage().Login(new User(LogIn, Password));
            new InboxPage().GoToNewEmailPage();

            var emailPage = new EmailPage();
            emailPage.CreateANewEmail(new Email(EmailAddress, EmailSubject, EmailText));
            emailPage.SaveAsADraft();
            emailPage.GoToDraftPage();

            new DraftPage().OpenEmail();

            Assert.AreEqual(EmailAddress, emailPage.GetDraftEmailAddress());
        }

        [TestMethod, TestCategory("VerificationOfTheDraftContent")]
        public void CompareDraftEmailSubjectTest()
        {
            new StartPage().Login(new User(LogIn, Password));
            new InboxPage().GoToNewEmailPage();

            var emailPage = new EmailPage();
            emailPage.CreateANewEmail(new Email(EmailAddress, EmailSubject, EmailText));
            emailPage.SaveAsADraft();
            emailPage.GoToDraftPage();

            new DraftPage().OpenEmail();

            Assert.AreEqual(EmailSubject, emailPage.GetDraftEmailSubject());
        }

        [TestMethod, TestCategory("VerificationOfTheDraftContent")]
        public void CompareDraftEmailTextTest()
        {
            new StartPage().Login(new User(LogIn, Password));
            new InboxPage().GoToNewEmailPage();

            var emailForm = new EmailPage();
            emailForm.CreateANewEmail(new Email(EmailAddress, EmailSubject, EmailText));
            emailForm.SaveAsADraft();
            emailForm.GoToDraftPage();

            new DraftPage().OpenEmail();

            Assert.IsTrue(emailForm.GetDraftEmailText().Contains(EmailSubject));
        }

        [TestMethod, TestCategory("EmailSending")]
        public void DraftFolderAfterSendingTest()
        {
            new StartPage().Login(new User(LogIn, Password));
            new InboxPage().GoToDraftPage();

            var draftPage = new DraftPage();
            draftPage.DeleteAllDraft();
            draftPage.GoToNewEmailPage();

            var emailPage = new EmailPage();
            emailPage.CreateANewEmail(new Email(EmailAddress, EmailSubject, EmailText));
            emailPage.SaveAsADraft();
            emailPage.GoToDraftPage();
            draftPage.OpenEmail();
            emailPage.SendEmail();
            emailPage.GoToDraftPage();

            Assert.IsFalse(draftPage.DraftEmailExist());
        }

        [TestMethod, TestCategory("EmailSending")]
        public void SendFolderAfterSendingTest()
        {
            new StartPage().Login(new User(LogIn, Password));
            new InboxPage().GoToSentPage();

            var sentPage = new SentPage();
            sentPage.DeleteAllSent();
            sentPage.GoToNewEmailPage();

            var emailPage = new EmailPage();
            emailPage.CreateANewEmail(new Email(EmailAddress, EmailSubject, EmailText));
            emailPage.SaveAsADraft();
            emailPage.GoToDraftPage();

            new DraftPage().OpenEmail();

            emailPage.SendEmail();
            emailPage.GoToSentPage();

            Assert.IsTrue(sentPage.SentEmailExist());
        }

        [TestMethod, TestCategory("Logout")]
        public void LogoutTest()
        {
            var startPage = new StartPage();
            startPage.Login(new User(LogIn, Password));

            new InboxPage().LogOut();

            Assert.IsTrue(startPage.LogoutSuccessMarker());
        }

        [TestMethod, TestCategory("DeleteEmail")]
        public void DragAndDropEmailTest()
        {
            new StartPage().Login(new User(LogIn, Password));
            new InboxPage().GoToSentPage();

            var sentPage = new SentPage();
            sentPage.DeleteAllSent();
            sentPage.GoToNewEmailPage();

            var emailPage = new EmailPage();
            emailPage.CreateANewEmail(new Email(EmailAddress, EmailSubject, EmailText));
            emailPage.SendEmail();
            emailPage.GoToSentPage();
            sentPage.DragAndDropFromSentToDelete();

            Assert.IsFalse(sentPage.SentEmailExist());
        }

        [TestMethod, TestCategory("EmailSending")]
        public void SendAnEmailToRandom()
        {
            var RandomEmail = GetRandomEmailAddress.GetRandomEmail();

            new StartPage().Login(new User(LogIn, Password));
            new InboxPage().GoToSentPage();

            var sentPage = new SentPage();
            sentPage.DeleteAllSent();
            sentPage.GoToNewEmailPage();

            var emailPage = new EmailPage();
            emailPage.CreateANewEmail(new Email(EmailAddress, EmailSubject, EmailText));
            emailPage.SaveAsADraft();
            emailPage.GoToDraftPage();

            new DraftPage().OpenEmail();

            emailPage.SendEmail();
            emailPage.GoToSentPage();

            Assert.IsTrue(sentPage.SentEmailExist());
        }



    }
}
