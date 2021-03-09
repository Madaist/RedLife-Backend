using Abp.Dependency;
using Abp.Net.Mail;
using RedLife.Authorization.Users;
using RedLife.Core.Donations;

namespace RedLife.Core.EmailSender
{
    public class EmailManager : IEmailManager, ISingletonDependency
    {
        public const string fromAddress = "RedLife <redlife.noreply@gmail.com>";

        private readonly IEmailSender _emailSender;

        public EmailManager(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public void SendMailToBloodDonor(User bloodDonor, Donation donation)
        {
            _emailSender.Send(
                to: bloodDonor.EmailAddress,
                from: fromAddress,
                subject: "Your blood donation just saved a life!",
                body: $"Hello {bloodDonor.Name}, <br><br> You are a hero! <br>" 
                    + "<b>A pacient just received a transfusion with the blood that you donated on " 
                    + donation.Date.ToLocalTime().ToString("yyyy-MM-dd") + ". </b><br>"
                    + "Add another saved life on your list! <br>"
                    + "We are waiting for you to save more.<br><br>"
                    + "You can go in the app and see how your statistics changed.<br>"
                    + "Cheers, <br> Red Life team"
                    + "<br><img src=https://i.ibb.co/yFxCtbg/redlifelogo-removebg-preview.png />"
                ,
                isBodyHtml: true
            );
        }
    }
}
