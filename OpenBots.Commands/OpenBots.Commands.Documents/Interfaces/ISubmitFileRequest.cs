using System;

namespace OpenBots.Commands.Documents.Interfaces
{
    public interface ISubmitFileRequest : IRequest
    {
        string v_AssignedTo { get; set; }
        string v_CaseNumber { get; set; }
        string v_CaseType { get; set; }
        string v_Description { get; set; }
        string v_DueDate { get; set; } //DateTime
        string v_FilePath { get; set; }

        string v_QueueName { get; set; }

        string v_Name { get; set; }

    }

    public interface IRequest
    {
        
        string v_TenantId { get; set; } //long?

       
        string v_Username { get; set; }

       
        string v_Password { get; set; }

       
        string v_ApiKey { get; set; }
    }
}