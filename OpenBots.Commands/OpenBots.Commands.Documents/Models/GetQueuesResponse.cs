using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Commands.Documents.Models
{
    public class GetQueuesResponse : List<GetQueueResponse>
    {
    }

    public class GetQueueResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

}
