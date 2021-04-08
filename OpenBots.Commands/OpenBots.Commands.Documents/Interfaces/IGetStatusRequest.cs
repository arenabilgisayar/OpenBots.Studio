using System;
using System.Activities;

namespace TextXtractor.Activities
{
    public interface IGetStatusRequest : IRequest
    {
        InArgument<Guid> TaskID { get; set; }

        InArgument<bool> AwaitCompletion { get; set; }

        InArgument<int> TimeoutInSeconds { get; set; }
    }
}