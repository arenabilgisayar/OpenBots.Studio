using System;
using System.Activities;

namespace TextXtractor.Activities
{
    public interface ISubmitFileResult
    {
        OutArgument<string> Status { get; set; }
        OutArgument<Guid> TaskID { get; set; }
    }
}