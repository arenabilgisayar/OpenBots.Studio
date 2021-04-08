﻿namespace OpenBots.Commands.Documents.Interfaces
{
    public interface ISaveRequest : IRequest
    {
        string v_AwaitCompletion { get; set; } //bool
        string v_TaskId { get; set; } //Guid
        string v_Timeout { get; set; } //int
        string v_OutputFolder { get; set; }
        string v_SavePageImages { get; set; } //bool
        string v_SavePageText { get; set; } //bool
    }
}