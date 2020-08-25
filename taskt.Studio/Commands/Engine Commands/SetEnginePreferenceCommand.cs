﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command allows you to set preferences for engine behavior at runtime.")]
    public class SetEnginePreferenceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Calculation Option")]
        [PropertyUISelectionOption("Enable Automatic Calculations")]
        [PropertyUISelectionOption("Disable Automatic Calculations")]
        [InputSpecification("")]
        [Remarks("")]
        public string v_CalculationOption { get; set; }

        public SetEnginePreferenceCommand()
        {
            CommandName = "SetEnginePreferenceCommand";
            SelectionName = "Set Engine Preference";
            CommandEnabled = true;
            CustomRendering = true;
            v_CalculationOption = "Enable Automatic Calculations";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            switch (v_CalculationOption)
            {
                case "Enable Automatic Calculations":
                    engine.AutoCalculateVariables = true;
                    break;
                case "Disable Automatic Calculations":
                    engine.AutoCalculateVariables = false;
                    break;
                default:
                    throw new NotImplementedException($"The preference '{v_CalculationOption}' is not implemented.");
            }
            //TODO: Add more engine preference options
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_CalculationOption", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Calculation '{v_CalculationOption}']";
        }
    }
}