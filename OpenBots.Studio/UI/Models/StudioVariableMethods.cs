using Microsoft.CodeAnalysis.Scripting;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenBots.UI.Models
{
    public static class StudioVariableMethods
    {
        public async static Task AddVariable(string varName, Type varType, string code, ScriptContext scriptContext)
        {
            if (string.IsNullOrEmpty(code))
                code = "null";

            if (scriptContext.EngineScriptState == null)
                scriptContext.EngineScriptState = await scriptContext.EngineScript.RunAsync();

            string script = $"{varType.GetRealTypeName()}? {varName} = {code};";

            scriptContext.EngineScriptState = await scriptContext.EngineScriptState
                .ContinueWithAsync(script, ScriptOptions.Default
                .WithReferences(scriptContext.AssembliesList)
                .WithImports(scriptContext.NamespacesList));
        }


        public async static Task UpdateVariable(string varName, Type varType, string code, ScriptContext scriptContext)
        {
            if (string.IsNullOrEmpty(code))
                code = "null";

            var existingVariable = scriptContext.EngineScriptState.Variables.Where(x => x.Name == varName).LastOrDefault();

            if (existingVariable != null && (existingVariable.Type.GetRealTypeName() == varType.GetRealTypeName() || 
                                             existingVariable.Type.GetRealTypeName() == $"Nullable<{varType.GetRealTypeName()}>"))
            {
                if (scriptContext.EngineScriptState == null)
                    scriptContext.EngineScriptState = await scriptContext.EngineScript.RunAsync();

                string script = $"{varName} = {code};";

                scriptContext.EngineScriptState = await scriptContext.EngineScriptState
                   .ContinueWithAsync(script, ScriptOptions.Default
                   .WithReferences(scriptContext.AssembliesList)
                   .WithImports(scriptContext.NamespacesList));
            }
            else if (existingVariable != null)
            {
                var errors = await ResetEngineVariables(scriptContext);
                if (errors.Count > 0)
                    throw errors.Last();
            }
            else
                await AddVariable(varName, varType, code, scriptContext);
        }

        public async static Task<List<Exception>> ResetEngineVariables(ScriptContext scriptContext)
        {
            List<Exception> errors = new List<Exception>();
                
            await scriptContext.ReinitializeEngineScript();

            foreach (var variable in scriptContext.Variables)
            {
                try
                {
                    await AddVariable(variable.VariableName, variable.VariableType, variable.VariableValue?.ToString(), scriptContext);
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            }

            foreach (var argument in scriptContext.Arguments)
            {
                try
                {
                    await AddVariable(argument.ArgumentName, argument.ArgumentType, argument.ArgumentValue.ToString(), scriptContext);
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            }

            return errors;
        }
    }
}
