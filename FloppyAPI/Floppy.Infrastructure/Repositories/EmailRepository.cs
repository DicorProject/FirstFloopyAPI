using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class EmailRepository: IEmailRepository
    {
        private readonly ApplicationDbContext _context; 
        private readonly ILogger<EmailRepository> _logger;  
        public EmailRepository(ApplicationDbContext context,ILogger<EmailRepository> logger)
        {
            _context = context;
            _logger = logger;   
        }
        #region GetSMTPDetailsById
        public SmtpMaster GetBySmtpId(int smtpId)
        {
            try
            {
                var smtpData = _context.SmtpMaster.FirstOrDefault(x => x.Id == smtpId);
                return smtpData;

            }
            catch(Exception ex) 
            {
				Console.WriteLine(ex.Message);
				return null;
            }
        }
        #endregion

        #region GetEmailTemplateByDocumentType
        public TemplateMaster GetTemplateMasterAsyncByType(string templateType)
        {
            try
            {
                var data = _context.TemplateMaster
                    .FirstOrDefault(x => x.Documenttype == templateType && x.Templatetype == "Email");
                return data;
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
				return null;
            }
        }
		#endregion

		#region GetMessageTemplateMasterAsyncByType
		public async Task<TemplateMaster> GetMessageTemplateMasterAsyncByType(string templateType)
        {
            try
            {
                var data = await _context.TemplateMaster
                    .FirstOrDefaultAsync(x => x.Documenttype == templateType && x.Templatetype == "message");
                return data;
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
				return null;
            }

        }

		#endregion

		#region GetMobileSMSTemplate
		public async Task<MobileSMSTemplate> GetMobileSMSTemplate(string templateName)
		{
			try
			{
				var data = await _context.MobileSMSTemplate
					.FirstOrDefaultAsync(x => x.TemplateName == templateName);
				return data;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}

		}

		#endregion
	}
}
