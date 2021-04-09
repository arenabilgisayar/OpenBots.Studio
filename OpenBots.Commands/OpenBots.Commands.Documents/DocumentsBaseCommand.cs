using OpenBots.Commands.Documents.Interfaces;
using OpenBots.Commands.Documents.Library;
using OpenBots.Commands.Documents.Models;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace OpenBots.Commands.Documents
{
    public abstract class DocumentsBaseCommand : ScriptCommand, IRequest
    {
        [Required]
        [DisplayName("Username")]
        [Description("Username for the Openbots Documents Service.")]
        [SampleUsage("OBUser || {vUsername}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_Username { get; set; }

        [Required]
        [DisplayName("Password")]
        [Description("Password for the Openbots Documents Service.")]
        [SampleUsage("{vPassword}")]
        [Remarks("Password input must be a SecureString variable.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(SecureString) })]
        public string v_Password { get; set; }

        [DisplayName("TenantId (Optional)")]
        [Description("TenantId for the Openbots Documents Service.")]
        [SampleUsage("123456789 || {vTenantId}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_TenantId { get; set; } //long?

        [DisplayName("ApiKey (Optional)")]
        [Description("ApiKey for the Openbots Documents Service.")]
        [SampleUsage("123-456-789 || {vApiKey}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_ApiKey { get; set; }

        //[Category("Input")]
        //[DisplayName("Proxy Server")]
        //[Description("Proxy Server to use")]
        //public InArgument<string> ProxyServer { get; set; }

        public DocumentsBaseCommand()
        {
            CommandEnabled = false;
            CommandIcon = Resources.command_files;
        }

        protected DocumentProcessingService CreateService(IAutomationEngineInstance engine)
        {
            var vTenantId = v_TenantId.ConvertUserVariableToString(engine);
            var vApiKey = v_ApiKey.ConvertUserVariableToString(engine);

            var ds = new DocumentProcessingService();

            if (!string.IsNullOrEmpty(vTenantId))
                ds.TenantId = long.Parse(vTenantId).ToString();

            //ds.Proxy = ProxyServer.Get(context);
            ds.ApiKey = vApiKey;
            ds.MachineName = Environment.MachineName;
            return ds;
        }

        protected DocumentProcessingService CreateAuthenticatedService(IAutomationEngineInstance engine)
        {
            var vUsername = v_Username.ConvertUserVariableToString(engine);
            var vPassword = ((SecureString)v_Password.ConvertUserVariableToObject(engine, nameof(v_Password), this)).ConvertSecureStringToString();

            DocumentProcessingService ds = CreateService(engine);
            AuthenticationRequest req = new AuthenticationRequest();
            req.UserNameOrEmailAddress = vUsername;
            req.Password = vPassword;
            ds.Authenticate(req);
            return ds;
        }
    }
}
