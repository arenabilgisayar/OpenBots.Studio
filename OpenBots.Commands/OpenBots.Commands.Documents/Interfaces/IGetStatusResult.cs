using System.Activities;

namespace TextXtractor.Activities
{
    public interface IGetStatusResult
    {
        OutArgument<string> Status { get; set; }
    }
}