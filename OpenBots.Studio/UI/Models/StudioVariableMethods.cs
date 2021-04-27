using Microsoft.CodeAnalysis.Scripting;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
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

            if (existingVariable != null && (existingVariable.Type.GetRealTypeName() == varType.GetRealTypeName() || existingVariable.Type.GetRealTypeName() == $"Nullable<{varType.GetRealTypeName()}>"))
            {
                if (scriptContext.EngineScriptState == null)
                    scriptContext.EngineScriptState = await scriptContext.EngineScript.RunAsync();

                string script = $"{varName} = {code};";

                scriptContext.EngineScriptState = await scriptContext.EngineScriptState
                   .ContinueWithAsync(script, ScriptOptions.Default
                   .WithReferences(scriptContext.AssembliesList)
                   .WithImports(scriptContext.NamespacesList));
            }
            else if(existingVariable != null)
            {
                var existingtype = existingVariable.Type.GetRealTypeName();
                var newType = varType.GetRealTypeName();

                await RemoveVariable(varName, scriptContext);
                await AddVariable(varName, varType, code, scriptContext);
            }
            else
                await AddVariable(varName, varType, code, scriptContext);
        }

        public async static Task RemoveVariable(string varName, ScriptContext scriptContext)
        {
            var existingVariable = scriptContext.EngineScriptState.Variables.Where(x => x.Name == varName).LastOrDefault();

            if (existingVariable != null)
            {
                scriptContext.EngineScriptState.Variables.Remove(existingVariable);

                if (scriptContext.EngineScriptState == null)
                    scriptContext.EngineScriptState = await scriptContext.EngineScript.RunAsync();

                string script = $"GC.Collect();";

                scriptContext.EngineScriptState = await scriptContext.EngineScriptState
                   .ContinueWithAsync(script, ScriptOptions.Default
                   .WithReferences(scriptContext.AssembliesList)
                   .WithImports(scriptContext.NamespacesList));
            }
        }
    }
}
