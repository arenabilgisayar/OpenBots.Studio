using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using System.Collections.Generic;
using System.Reflection;
using OBScriptVariable = OpenBots.Core.Script.ScriptVariable;
using RSScript = Microsoft.CodeAnalysis.Scripting.Script;

namespace OpenBots.UI.Models
{
    public class ScriptContext
    {
        public List<OBScriptVariable> ScriptVariables { get; set; }
        public List<ScriptArgument> ScriptArguments { get; set; }
        public List<ScriptElement> ScriptElements { get; set; }
        public Dictionary<string, AssemblyReference> ImportedNamespaces { get; set; }
        public List<Assembly> AssembliesList { get; set; }
        public List<string> NamespacesList { get; set; }
        public RSScript EngineScript { get; set; }
        public ScriptState EngineScriptState { get; set; }

        public ScriptContext()
        {
            ScriptVariables = new List<OBScriptVariable>();
            ScriptArguments = new List<ScriptArgument>();
            ScriptElements = new List<ScriptElement>();
            ImportedNamespaces = new Dictionary<string, AssemblyReference>(ScriptDefaultNamespaces.DefaultNamespaces);

            AssembliesList = NamespaceMethods.GetAssemblies(ImportedNamespaces);
            NamespacesList = NamespaceMethods.GetNamespacesList(ImportedNamespaces);

            EngineScript = CSharpScript.Create("", ScriptOptions.Default.WithReferences(AssembliesList)
                                                                        .WithImports(NamespacesList));

            EngineScriptState = null;
        }

        public ScriptContext(List<OBScriptVariable> scriptVariables, List<ScriptArgument> scriptArguments, List<ScriptElement> scriptElements,
            Dictionary<string, AssemblyReference> importedNamespaces, RSScript engineScript, ScriptState engineScriptState)
        {
            ScriptVariables = scriptVariables;
            ScriptArguments = scriptArguments;
            ScriptElements = scriptElements;
            ImportedNamespaces = importedNamespaces;

            AssembliesList = NamespaceMethods.GetAssemblies(ImportedNamespaces);
            NamespacesList = NamespaceMethods.GetNamespacesList(ImportedNamespaces);

            EngineScript = engineScript;
            EngineScriptState = engineScriptState;
        }

        public void CreateEngineScript()
        {
            EngineScript = CSharpScript.Create("", ScriptOptions.Default.WithReferences(AssembliesList)
                                                                        .WithImports(NamespacesList));
        }
    }
}
