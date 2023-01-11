namespace AirlineManager.API.Maintenance
{
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;
    public class AirmanEmailService
    {
        private string apiKey;

        public AirmanEmailService(string apiKey)
        {
            this.apiKey = apiKey;
        }
   
        public async Task NotifyScheduleChange(string email)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("bobbydpblue@gmail.com", "Airline Manager");
            var subject = "New Scheduled Maintenance";
            var to = new EmailAddress(email, "Worker 1");
            var plainTextContent = "Your schedule has been changed, please check your updated schedule.";
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //logger.LogInformation("Attempting to send a maintenance notification email.");
            var response = await client.SendEmailAsync(msg);
            //if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    logger.LogInformation("Email sent!");
            //}
            //else
            //{
            //    logger.LogError("The email did not send.", response.Body);
            //}

        }

    }
}
