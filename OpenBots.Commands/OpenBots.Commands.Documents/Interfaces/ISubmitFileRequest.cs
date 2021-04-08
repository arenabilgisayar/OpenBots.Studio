using System;
using System.Activities;

namespace TextXtractor.Activities
{
    public interface ISubmitFileRequest : IRequest
    {
        InArgument<string> AssignedTo { get; set; }
        InArgument<string> CaseNumber { get; set; }
        InArgument<string> CaseType { get; set; }
        InArgument<string> Description { get; set; }
        InArgument<DateTime> DueDate { get; set; }
        InArgument<string> FilePath { get; set; }

        InArgument<string> QueueName { get; set; }

        InArgument<string> Name { get; set; }

    }

    public interface IRequest
    {
        
        InArgument<long?> TenantId { get; set; }

       
        InArgument<string> Username { get; set; }

       
        InArgument<string> Password { get; set; }

       
        InArgument<string> ApiKey { get; set; }
    }
}