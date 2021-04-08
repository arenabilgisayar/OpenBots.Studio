using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Commands.Documents.Models
{
	public class CreateTaskRequest 
	{
		public string Name { get; set; }

		public string Description { get; set; }
		
		public string CaseNumber { get; set; }
		
		public string CaseType { get; set; }

		public string DueOn { get; set; }
	
		public string Status { get; set; }

		public string AssignedTo { get; set; }

		public Guid? TaskQueueId { get; set; }

		public long? OrganizationUnitId { get; set; }

		public long? UserId { get; set; }


	}
}
