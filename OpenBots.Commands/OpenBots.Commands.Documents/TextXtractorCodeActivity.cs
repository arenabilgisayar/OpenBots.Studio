using OpenBots.Commands.Documents.Interfaces;
using OpenBots.Core.Command;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenBots.Commands.Documents
{
    public abstract class TextXtractorCodeActivity : ScriptCommand, IRequest
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


        //protected IDocumentService CreateService(CodeActivityContext context)
        //{
        //    // Create an instance of 'AutneticationRequest' to pass authentication credetials to the service.
        //    AuthenticationRequest request = new AuthenticationRequest()
        //    {
        //        UserNameOrEmailAddress = Username.Get(context),  // Prefer using EmailAddress
        //        Password = Password.Get(context)
        //    };

        //    ServiceConnectionSetting serviceSetting = new ServiceConnectionSetting();
        //    serviceSetting.ApiKey = ApiKey.Get(context);

        //    // Pass your TenantId . Ask your administrator incase you dont have it.
        //    long? tenantId = TenantId.Get(context);

        //    if (!tenantId.HasValue || tenantId.Value == 0)
        //    {
        //        IDocumentService service = ServiceFactory.CreateDocumentService(request, null, serviceSetting);
        //        return service;
        //    }
        //    else
        //    {
        //        // Create an instance of the document service class to be able able to process documents.           
        //        IDocumentService service = ServiceFactory.CreateDocumentService(request, tenantId, serviceSetting);
        //        return service;
        //    }
        //}

    }
}
