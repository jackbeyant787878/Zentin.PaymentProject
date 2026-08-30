using Microsoft.Extensions.DependencyInjection;

namespace Zentin.Payment.Notification.Email
{
    internal class EmailOptionsExtension : INoticeOptionsExtension
    {
        private readonly Action<EmailOptions> configure;

        public EmailOptionsExtension(Action<EmailOptions> configure)
        {
            this.configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddOptions<EmailOptions>().Configure(configure);
            services.AddTransient<IEmailProvider, EmailProvider>();
        }
    }
}
