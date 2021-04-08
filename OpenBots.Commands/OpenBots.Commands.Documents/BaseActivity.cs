using OpenBots.Commands.Documents.Interfaces;
using OpenBots.Commands.Documents.Library;
using OpenBots.Commands.Documents.Models;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenBots.Commands.Documents
{
    public abstract class BaseActivity : ScriptCommand, IRequest
    {
        [Category("Input")]
        [DisplayName("Openbots Documents Username")]
        [Required]
        [Description("Username for the Openbots Documents Service")]
        public string v_Username { get; set; }

        [Category("Input")]
        [DisplayName("Openbots Documents Password")]
        [Required]
        [Description("Password for the Openbots Documents Service")]
        public string v_Password { get; set; }

        [Category("Input")]
        [DisplayName("Openbots Documents TenantId")]
        [Description("TenantId for the Openbots Documents Service")]
        public string v_TenantId { get; set; } //long?

        [Category("Input")]
        [DisplayName("Openbots Documents ApiKey")]
        [Description("ApiKey for the Openbots Documents Service")]
        public string v_ApiKey { get; set; }

        //[Category("Input")]
        //[DisplayName("Proxy Server")]
        //[Description("Proxy Server to use")]
        //public InArgument<string> ProxyServer { get; set; }

        protected DocumentProcessingService CreateService(IAutomationEngineInstance engine)// CodeActivityContext context)
        {
            var vTenantId = v_TenantId.ConvertUserVariableToString(engine);
            var vApiKey = v_ApiKey.ConvertUserVariableToString(engine);

            var ds = new DocumentProcessingService();

            if (!string.IsNullOrEmpty(vTenantId))
            //if (TenantId.Get(context).HasValue)
                ds.TenantId = long.Parse(vTenantId).ToString();


            //ds.Proxy = ProxyServer.Get(context);
            ds.ApiKey = vApiKey;
            ds.MachineName = Environment.MachineName;
            return ds;
        }

        protected DocumentProcessingService CreateAuthenticatedService(IAutomationEngineInstance engine) ///CodeActivityContext context)
        {
            var vUsername = v_Username.ConvertUserVariableToString(engine);
            var vPassword = v_Password.ConvertUserVariableToString(engine);

            DocumentProcessingService ds = CreateService(engine);
            AuthenticationRequest req = new AuthenticationRequest();
            req.UserNameOrEmailAddress = vUsername;
            req.Password = vPassword;
            ds.Authenticate(req);
            return ds;
        }

    }

}
