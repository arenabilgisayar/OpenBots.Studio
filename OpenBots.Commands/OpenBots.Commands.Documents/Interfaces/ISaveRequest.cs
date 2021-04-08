using System;
using System.Activities;

namespace TextXtractor.Activities
{
    public interface ISaveRequest : IRequest
    {
        InArgument<bool> AwaitCompletion { get; set; }
        InArgument<Guid> TaskID { get; set; }
        InArgument<int> TimeoutInSeconds { get; set; }
        InArgument<string> OutputFolder { get; set; }
        InArgument<bool> SavePageImages { get; set; }
        InArgument<bool> SavePageText { get; set; }
    }
}