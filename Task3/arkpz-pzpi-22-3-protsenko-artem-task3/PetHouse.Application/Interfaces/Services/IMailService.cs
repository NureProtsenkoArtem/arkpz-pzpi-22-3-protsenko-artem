using PetHouse.Application.Contracts.Mail;

namespace PetHouse.Application.Interfaces.Services;

public interface IMailService
{
   Task SendEmail(string recieverEmail, string code, EmailSendType sendType);
}