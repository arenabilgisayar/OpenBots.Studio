using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextXtractor.Activities.Model;

namespace OpenBots.Commands.Documents
{
    public abstract class BaseActivity : CodeActivity, IRequest
    {
        [Category("Input")]
        [DisplayName("Openbots Documents Username")]
        [RequiredArgument]
        [Description("Username for the Openbots Documents Service")]
        public InArgument<string> Username { get; set; }

        [Category("Input")]
        [DisplayName("Openbots Documents Password")]
        [RequiredArgument]
        [Description("Password for the Openbots Documents Service")]
        public InArgument<string> Password { get; set; }

        [Category("Input")]
        [DisplayName("Openbots Documents TenantId")]
        [Description("TenantId for the Openbots Documents Service")]
        public InArgument<long?> TenantId { get; set; }

        [Category("Input")]
        [DisplayName("Openbots Documents ApiKey")]
        [Description("ApiKey for the Openbots Documents Service")]
        public InArgument<string> ApiKey { get; set; }

        //[Category("Input")]
        //[DisplayName("Proxy Server")]
        //[Description("Proxy Server to use")]
        //public InArgument<string> ProxyServer { get; set; }

        protected DocumentProcessingService CreateService(CodeActivityContext context)
        {
            var ds = new DocumentProcessingService();
            if (TenantId.Get(context).HasValue)
                ds.TenantId = TenantId.Get(context).Value.ToString();


            //ds.Proxy = ProxyServer.Get(context);
            ds.ApiKey = ApiKey.Get(context);
            ds.MachineName = Environment.MachineName;
            return ds;
        }

        protected DocumentProcessingService CreateAuthenticatedService(CodeActivityContext context)
        {
            DocumentProcessingService ds = CreateService(context);
            AuthenticationRequest req = new AuthenticationRequest();
            req.UserNameOrEmailAddress = Username.Get(context);
            req.Password = Password.Get(context);
            ds.Authenticate(req);
            return ds;
        }

    }

}
