using System;

namespace OpenBots.Commands.Documents.Interfaces
{
    public interface ISubmitFileResult
    {
        string v_Status { get; set; }
        string v_TaskID { get; set; } //Guid
    }
}