using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mailing.Interfaces
{
    public interface IEmailService
    {
        void Send(string email, string subject, string html);
    }
}
