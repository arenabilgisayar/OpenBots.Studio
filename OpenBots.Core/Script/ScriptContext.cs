using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using OBScriptVariable = OpenBots.Core.Script.ScriptVariable;
using RSScript = Microsoft.CodeAnalysis.Scripting.Script;

namespace OpenBots.Core.Script
{
    public class ScriptContext
    {
        public List<OBScriptVariable> Variables { get; set; }
        public List<ScriptArgument> Arguments { get; set; }
        public List<ScriptElement> Elements { get; set; }
        public Dictionary<string, AssemblyReference> ImportedNamespaces { get; set; }
        public List<Assembly> AssembliesList { get; set; }
        public List<string> NamespacesList { get; set; }
        public RSScript EngineScript { get; set; }
        public ScriptState EngineScriptState { get; set; }
        public string GuidPlaceholder { get; set; }


        public ScriptContext()
        {
            Variables = new List<OBScriptVariable>();
            Arguments = new List<ScriptArgument>();
            Elements = new List<ScriptElement>();
            ImportedNamespaces = new Dictionary<string, AssemblyReference>(ScriptDefaultNamespaces.DefaultNamespaces);

            AssembliesList = NamespaceMethods.GetAssemblies(ImportedNamespaces);
            NamespacesList = NamespaceMethods.GetNamespacesList(ImportedNamespaces);

            EngineScript = CSharpScript.Create("", ScriptOptions.Default.WithReferences(AssembliesList)
                                                                    .WithImports(NamespacesList));
     
            GuidPlaceholder = $"v{Guid.NewGuid()}".Replace("-", "");
        }
    }
}
