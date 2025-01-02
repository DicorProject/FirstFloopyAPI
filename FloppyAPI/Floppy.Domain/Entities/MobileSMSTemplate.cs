using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floppy.Domain.Entities
{
	public class MobileSMSTemplate
	{
		public int Id { get; set; }
		public string TemplateName { get; set; }
		public string TemplateID { get; set; }
		public string TemplateContent { get; set; }
	}

}
