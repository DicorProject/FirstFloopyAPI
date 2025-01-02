using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface IEmailRepository
    {
        SmtpMaster GetBySmtpId(int smtpId);
        TemplateMaster GetTemplateMasterAsyncByType(string templateType);
        Task<MobileSMSTemplate> GetMobileSMSTemplate(string templateName);

	}
}
