using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Commands.Documents.Models
{
    public enum TaskStatusTypes : int
    {
        Created = 1,
        InProgress = 2,
        Processed = 3,
        AwaitingVerification = 4,
        Verified = 5,
        Completed = 6,
        MarkedForDeletion = 7,
        Error = 8,
        Abandoned = 9
    }
}
