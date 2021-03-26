﻿using Microsoft.Office.Interop.Outlook;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Outlook.Application;

namespace OpenBots.Commands.Outlook
{
	[Serializable]
	[Category("Outlook Commands")]
	[Description("This command sends an email with optional attachment(s) in Outlook.")]

	public class SendOutlookEmailCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Recipient(s)")]
		[Description("Enter the email address(es) of the recipient(s).")]
		[SampleUsage("test@test.com || test@test.com;test2@test.com || {vEmail} || {vEmail1};{vEmail2} || {vEmails}")]
		[Remarks("Multiple recipient email addresses should be delimited by a semicolon (;).")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_Recipients { get; set; }

		[Required]
		[DisplayName("Email Subject")]
		[Description("Enter the subject of the email.")]
		[SampleUsage("Hello || {vSubject}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_Subject { get; set; }

		[Required]
		[DisplayName("Email Body")]
		[Description("Enter text to be used as the email body.")]
		[SampleUsage("Dear John, ... || {vBody}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_Body { get; set; }

		[Required]
		[DisplayName("Email Body Type")]
		[PropertyUISelectionOption("Plain")]
		[PropertyUISelectionOption("HTML")]
		[Description("Select the email body format.")]
		[Remarks("")]
		public string v_BodyType { get; set; }

		[DisplayName("Attachment File Path(s) (Optional)")]
		[Description("Enter the file path(s) of the file(s) to attach.")]
		[SampleUsage(@"C:\temp\myFile.xlsx || {vFile} || C:\temp\myFile1.xlsx;C:\temp\myFile2.xlsx || {vFile1};{vFile2} || {vFiles}")]
		[Remarks("This input is optional. Multiple attachments should be delimited by a semicolon (;).")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_Attachments { get; set; }

		public SendOutlookEmailCommand()
		{
			CommandName = "SendOutlookEmailCommand";
			SelectionName = "Send Outlook Email";
			CommandEnabled = true;
			CommandIcon = Resources.command_smtp;

			v_BodyType = "Plain";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vRecipients = v_Recipients.ConvertUserVariableToString(engine);
			var vAttachment = v_Attachments.ConvertUserVariableToString(engine);
			var vSubject = v_Subject.ConvertUserVariableToString(engine);
			var vBody = v_Body.ConvertUserVariableToString(engine);
			var splitRecipients = vRecipients.Split(';');

			Application outlookApp = new Application();
			MailItem mail = (MailItem)outlookApp.CreateItem(OlItemType.olMailItem);
			AddressEntry currentUser = outlookApp.Session.CurrentUser.AddressEntry;
			if (currentUser.Type == "EX")
			{
				ExchangeUser manager = currentUser.GetExchangeUser().GetExchangeUserManager();

				foreach (var t in splitRecipients)
					mail.Recipients.Add(t.ToString());

				mail.Recipients.ResolveAll();

				mail.Subject = vSubject;

				if (v_BodyType == "HTML")
					mail.HTMLBody = vBody;
				else
					mail.Body = vBody;
 
				if (!string.IsNullOrEmpty(vAttachment))
				{
					var splitAttachments = vAttachment.Split(';');
					foreach (var attachment in splitAttachments)
						mail.Attachments.Add(attachment);
				}
				mail.Send();
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Recipients", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Subject", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Body", this, editor, 100, 300));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_BodyType", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Attachments", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [To '{v_Recipients}' - Subject '{v_Subject}']";
		}
	}
}