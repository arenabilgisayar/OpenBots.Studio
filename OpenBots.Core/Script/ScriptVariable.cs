﻿using System;

namespace OpenBots.Core.Script
{
    [Serializable]
    public class ScriptVariable
    {
        /// <summary>
        /// name that will be used to identify the variable
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// type of the variable or current index
        /// </summary>
        public Type VariableType { get; set; }
        
        /// <summary>
        /// value of the variable or current index
        /// </summary>
        public object VariableValue { get; set; }
    }
}
