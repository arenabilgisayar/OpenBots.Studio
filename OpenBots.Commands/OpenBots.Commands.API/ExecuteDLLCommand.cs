﻿using Newtonsoft.Json;
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
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace OpenBots.Commands.API
{
    [Serializable]
	[Category("API Commands")]
	[Description("This command invokes a method of a specific class from a DLL.")]
	public class ExecuteDLLCommand : ScriptCommand, IExecuteDLLCommand
	{
		[Required]
		[DisplayName("DLL File Path")]
		[Description("Enter or Select the path to the DLL File.")]
		[SampleUsage("C:\\temp\\myfile.dll || {vDLLFilePath}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowDLLExplorer", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_FilePath { get; set; }

		[Required]
		[DisplayName("Class Name")]
		[Description("Provide the parent class name of the method to be invoked in the DLL.")]
		[SampleUsage("myNamespace.myClassName || {vClassName}")]
		[Remarks("Namespace should be included")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ClassName { get; set; }

		[Required]
		[DisplayName("Method Name")]
		[Description("Provide the method name to be invoked in the DLL.")]
		[SampleUsage("GetSomething || {vMethodName}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_MethodName { get; set; }

		[DisplayName("Parameters (Optional)")]
		[Description("Select the 'Generate Parameters' button once you have indicated a file, class, and method.")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("GenerateDLLParameters", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public DataTable v_MethodParameters { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Result Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string)} )]
		public string v_OutputUserVariableName { get; set; }

		public ExecuteDLLCommand()
		{
			CommandName = "ExecuteDLLCommand";
			SelectionName = "Execute DLL";
			CommandIcon = Resources.command_run_code;
			CommandEnabled = true;

			v_MethodParameters = new DataTable();
			v_MethodParameters.Columns.Add("Parameter Name");
			v_MethodParameters.Columns.Add("Parameter Value");
			v_MethodParameters.TableName = DateTime.Now.ToString("MethodParameterTable" + DateTime.Now.ToString("MMddyy.hhmmss"));			
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//get file path
			var filePath = v_FilePath.ConvertUserVariableToString(engine);
			var className = v_ClassName.ConvertUserVariableToString(engine);
			var methodName = v_MethodName.ConvertUserVariableToString(engine);

			//if file path does not exist
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("DLL was not found at " + filePath);
			}

			//Load Assembly
			Assembly requiredAssembly = Assembly.LoadFrom(filePath);

			//get type
			Type t = requiredAssembly.GetType(className);

			//get all methods
			MethodInfo[] availableMethods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

			//get method
			MethodInfo m = availableMethods.Where(f => f.ToString() == methodName).FirstOrDefault();

			//create instance
			var instance = requiredAssembly.CreateInstance(className);

			//check for parameters
			var reqdParams = m.GetParameters();

			//handle parameters
			object result;
			if (reqdParams.Length > 0)
			{
				//create parameter list
				var parameters = new List<object>();

				//get parameter and add to list
				foreach (var param in reqdParams)
				{
					//declare parameter name
					var paramName = param.Name;

					//get parameter value
					var requiredParameterValue = v_MethodParameters.AsEnumerable()
																   .Where(rws => rws.Field<string>("Parameter Name") == paramName)
																   .Select(rws => rws.Field<string>("Parameter Value"))
																   .FirstOrDefault()
																   .ConvertUserVariableToString(engine);

					dynamic parseResult;
					//check namespace and convert
					if (param.ParameterType.IsArray)
					{
						parseResult = requiredParameterValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					}
					else
					{
						switch (param.ParameterType.FullName)
						{
							case "System.Int32":
								parseResult = int.Parse(requiredParameterValue);
								break;
							case "System.Int64":
								parseResult = long.Parse(requiredParameterValue);
								break;
							case "System.Double":
								parseResult = double.Parse(requiredParameterValue);
								break;
							case "System.Boolean":
								parseResult = bool.Parse(requiredParameterValue);
								break;
							case "System.Decimal":
								parseResult = decimal.Parse(requiredParameterValue);
								break;
							case "System.Single":
								parseResult = float.Parse(requiredParameterValue);
								break;
							case "System.Char":
								parseResult = char.Parse(requiredParameterValue);
								break;
							case "System.String":
								parseResult = requiredParameterValue;
								break;
							case "System.DateTime":
								parseResult = DateTime.Parse(requiredParameterValue);
								break;
							default:
								throw new NotImplementedException("Only system parameter types are supported!");
						}
					}
					parameters.Add(parseResult);
				}

				//invoke
				result = m.Invoke(instance, parameters.ToArray());
			}
			else
			{
				//invoke
				result = m.Invoke(instance, null);
			}

			//check return type
			var returnType = result.GetType();

			//check namespace
			if (returnType.Namespace != "System" || returnType.IsArray)
			{
				//conversion of type is required due to type being a complex object

				//set json settings
				JsonSerializerSettings settings = new JsonSerializerSettings();
				settings.Error = (serializer, err) => {
					err.ErrorContext.Handled = true;
				};

				//set indent
				settings.Formatting = Formatting.Indented;

				//convert to json
				result = JsonConvert.SerializeObject(result, settings);
			}

			//store result in variable
			result.ToString().StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ClassName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MethodName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDataGridViewGroupFor("v_MethodParameters", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Call Method '{v_MethodName}' in '{v_ClassName}' - Store Result in '{v_OutputUserVariableName}']";
		}
	}
}
