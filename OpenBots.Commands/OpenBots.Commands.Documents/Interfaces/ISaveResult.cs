using System.Activities;

namespace TextXtractor.Activities
{
    public interface ISaveResult
    {
        OutArgument<bool> HasFailedOrError { get; set; }
        OutArgument<bool> IsCompleted { get; set; }
        OutArgument<string> Status { get; set; }
    }
}