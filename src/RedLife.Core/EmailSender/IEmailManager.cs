using RedLife.Authorization.Users;
using RedLife.Core.Donations;

namespace RedLife.Core.EmailSender
{
    public interface IEmailManager
    {
        public void SendMailToBloodDonor(User bloodDonor, Donation donation);
    }
}
