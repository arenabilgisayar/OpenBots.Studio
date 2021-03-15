using System.Data;

namespace OpenBots.NetCore.Core.Infrastructure
{
    public interface IConditionCommand
    {
        DataTable v_ActionParameterTable { get; set; }
    }
}
