﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TextXtractor.Ocr;

namespace OpenBots.Commands.Image
{
	[Serializable]
	[Category("OCR Commands")]
	[Description("This command extracts text from an image file using Amazon OCR.")]
	public class AmazonOCRCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Image File Path")]
		[Description("Select the image to perform OCR text extraction on.")]
		[SampleUsage(@"C:\temp\myimages.png || {ProjectPath}\myimages.png || {vImageFile}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_FilePath { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output OCR Result Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public AmazonOCRCommand()
		{
			CommandName = "AmazonOCRCommand";
			SelectionName = "Amazon OCR";
			CommandEnabled = true;
			CommandIcon = Resources.command_camera;
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vFilePath = v_FilePath.ConvertUserVariableToString(engine);

			var googleassemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.ToLower().StartsWith("g")).ToList();
			//byte[] img = File.ReadAllBytes(vFilePath);
			var ocrEngine = OcrFactory.CreateEngine();
			var ocrResult = ocrEngine.PerformRecognition(vFilePath);


			var text = ocrResult.Text; 

			text.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [File '{v_FilePath}' - Store OCR Result in '{v_OutputUserVariableName}']";
		}
	}
}