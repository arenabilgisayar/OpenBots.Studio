﻿using OpenBots.Core.Enums;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Studio.Utilities;
using OpenBots.UI.Forms.Supplement_Forms;
using OpenBots.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.CodeDom.Compiler;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        #region Variable/Argument Tab Events
        private void dgvVariablesArguments_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //creates a list of all existing variable/argument names to check against, prior to creating a new one
                DataGridView dgv = (DataGridView)sender;

                _preEditVarArgName = dgv.Rows[e.RowIndex].Cells[0].Value?.ToString();

                _existingVarArgSearchList = new List<string>();
                _existingVarArgSearchList.AddRange(_scriptContext.Arguments.Select(arg => arg.ArgumentName).ToList());
                _existingVarArgSearchList.AddRange(_scriptContext.Variables.Select(var => var.VariableName).ToList());
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private async void dgvVariablesArguments_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;
                var nameCell = dgv.Rows[e.RowIndex].Cells[0];
                var typeCell = dgv.Rows[e.RowIndex].Cells[1];
                var valueCell = dgv.Rows[e.RowIndex].Cells[2];

                //variable/argument name column
                if (e.ColumnIndex == 0)
                {
                    var cellValue = nameCell.Value;

                    CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");

                    //deletes an empty row if it's created without assigning values
                    if ((cellValue == null && _preEditVarArgName != null) ||
                        (cellValue != null && string.IsNullOrEmpty(cellValue.ToString().Trim())) || 
                        (cellValue != null && !provider.IsValidIdentifier(cellValue.ToString())))
                    {
                        dgv.Rows.RemoveAt(e.RowIndex);
                        await StudioVariableMethods.ResetEngineVariables(_scriptContext);
                        return;
                    }
                    //removes an empty uncommitted row
                    else if (nameCell.Value == null)
                        return;

                    //trims any space characters before reassigning the value to the cell
                    string variableName = nameCell.Value.ToString().Trim();
                    nameCell.Value = variableName;

                    //prevents user from creating a new variable/argument with an already used name
                    if (_existingVarArgSearchList.Contains(variableName) && variableName != _preEditVarArgName)
                    {
                        Notify($"An Error Occurred: A variable or argument with the name '{variableName}' already exists", Color.Red);
                        dgv.Rows.RemoveAt(e.RowIndex);
                        await StudioVariableMethods.ResetEngineVariables(_scriptContext);
                        return;
                    }
                    //if the variable/argument name is valid, set value cell's readonly as false
                    else
                    {
                        if (_scriptFileExtension == ".obscript")
                        {
                            foreach (DataGridViewCell cell in dgv.Rows[e.RowIndex].Cells)
                                cell.ReadOnly = false;
                        }
                        //if script isn't an obscript, enable the value cell only
                        else
                            dgv.Rows[e.RowIndex].Cells[2].ReadOnly = false;

                        nameCell.Value = variableName.Trim();

                        await StudioVariableMethods.AddVariable(nameCell.Value.ToString(), (Type)typeCell.Value, valueCell.Value?.ToString(), _scriptContext);
                    }
                }

                else if (e.ColumnIndex == 2)
                {
                    try
                    {
                        await StudioVariableMethods.UpdateVariable(nameCell.Value.ToString(), (Type)typeCell.Value, valueCell.Value?.ToString(), _scriptContext);
                        valueCell.Style = new DataGridViewCellStyle { ForeColor = Color.Black };
                    }
                    catch(Exception)
                    {
                        valueCell.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                    }
                }

                //marks the script as unsaved with changes
                if (uiScriptTabControl.SelectedTab != null && !uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                    uiScriptTabControl.SelectedTab.Text += " *";
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariablesArguments_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                if (dgv.Rows[e.RowIndex].Cells[0].Value == null)
                {
                    //when a new row is added, all cells except for the variable/argument name will be readonly until the name is validated
                    foreach (DataGridViewCell cell in dgv.Rows[e.RowIndex].Cells)
                    {
                        if (cell.ColumnIndex != 0)
                            cell.ReadOnly = true;
                    }
                }                
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariablesArguments_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                string varArgName = e.Row.Cells[0].Value?.ToString();
                    //prevents the ProjectPath row from being deleted
                if (varArgName == "ProjectPath" && _scriptFileExtension == ".obscript")
                    e.Cancel = true;
                else if((varArgName == "--PythonVersion" || varArgName == "--MainFunction") && _scriptFileExtension == ".py")
                    e.Cancel = true;
                else
                {
                    //marks the script as unsaved with changes
                    if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                        uiScriptTabControl.SelectedTab.Text += " *";
                }
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private async void dgvVariablesArguments_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            await StudioVariableMethods.ResetEngineVariables(_scriptContext);
        }

        private async void dgvVariablesArguments_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                if (dgv.Name == "dgvVariables")
                    await _scriptContext.ReinitializeEngineScript();

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    var nameCell = row.Cells[0];
                    var typeCell = row.Cells[1];
                    var valueCell = row.Cells[2];

                    if (nameCell.Value == null)
                        continue;

                    string varArgName = nameCell.Value?.ToString();

                    //sets the entire ProjectPath row as readonly
                    if (varArgName == "ProjectPath" && _scriptFileExtension == ".obscript")
                        row.ReadOnly = true;
                    else if ((varArgName == "--PythonVersion" || varArgName == "--MainFunction") && _scriptFileExtension == ".py")
                        row.Cells[0].ReadOnly = true;

                    //adds new type to default list when a script containing non-defaults is loaded
                    if (!_typeContext.DefaultTypes.ContainsKey(((Type)typeCell.Value)?.GetRealTypeName()))
                        _typeContext.DefaultTypes.Add(((Type)typeCell.Value).GetRealTypeName(), (Type)typeCell.Value);

                    //sets Value cell to readonly if the Direction is Out
                    if (row.Cells.Count == 4 && row.Cells["Direction"].Value != null && 
                        ((ScriptArgumentDirection)row.Cells["Direction"].Value == ScriptArgumentDirection.Out || 
                         (ScriptArgumentDirection)row.Cells["Direction"].Value == ScriptArgumentDirection.InOut))
                        row.Cells["ArgumentValue"].ReadOnly = true;

                    try
                    {
                        await StudioVariableMethods.AddVariable(nameCell.Value.ToString(), (Type)typeCell.Value, valueCell.Value?.ToString(), _scriptContext);
                        valueCell.Style = new DataGridViewCellStyle { ForeColor = Color.Black };
                    }
                    catch (Exception)
                    {
                        valueCell.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                    }
                }
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariablesArguments_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                if (dgv.IsCurrentCellDirty)
                {
                    //grab pre-edit value from here
                    if (dgv.CurrentCell.Value is Type)
                        _preEditVarArgType = (Type)dgv.CurrentCell.Value;
                    
                    //this fires the cell value changed handler below
                    dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }

            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private async void dgvVariablesArguments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;
                var nameCell = dgv.Rows[e.RowIndex].Cells[0];
                var typeCell = dgv.Rows[e.RowIndex].Cells[1];
                var valueCell = dgv.Rows[e.RowIndex].Cells[2];

                if (e.RowIndex != -1)
                {
                    var selectedCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    if (selectedCell.Value == null)
                        return;

                    else if (e.RowIndex != -1 && e.ColumnIndex == 3)
                    {
                        //sets value cell to read only if the argument direction is set to Out
                        if ((ScriptArgumentDirection)selectedCell.Value == ScriptArgumentDirection.Out || 
                            (ScriptArgumentDirection)selectedCell.Value == ScriptArgumentDirection.InOut)
                        {
                            dgv.Rows[e.RowIndex].Cells["ArgumentValue"].Value = null;
                            dgv.Rows[e.RowIndex].Cells["ArgumentValue"].ReadOnly = true;
                        }

                        else if ((ScriptArgumentDirection)selectedCell.Value == ScriptArgumentDirection.In)
                            dgv.Rows[e.RowIndex].Cells["ArgumentValue"].ReadOnly = false;
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        if (selectedCell.Value is Type && ((Type)selectedCell.Value).Name == "MoreOptions")
                        {
                            //triggers the type form to open if 'More Options...' is selected
                            frmTypes typeForm = new frmTypes(_typeContext);
                            typeForm.ShowDialog();

                            //adds type to defaults if new, then commits selection to the cell
                            if (typeForm.DialogResult == DialogResult.OK)
                            {
                                if (!_typeContext.DefaultTypes.ContainsKey(typeForm.SelectedType.GetRealTypeName()))
                                {
                                    _typeContext.DefaultTypes.Add(typeForm.SelectedType.GetRealTypeName(), typeForm.SelectedType);
                                    variableType.DataSource = new BindingSource(_typeContext.DefaultTypes, null);
                                    argumentType.DataSource = new BindingSource(_typeContext.DefaultTypes, null);
                                }

                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = typeForm.SelectedType;
                                ((DataGridViewComboBoxCell)typeCell).Value = typeForm.SelectedType;
                            }
                            //returns the cell to its original value
                            else
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = _preEditVarArgType;
                                ((DataGridViewComboBoxCell)typeCell).Value = _preEditVarArgType;
                            }

                            typeForm.Dispose();

                            //necessary hack to force the set value to update
                            SendKeys.Send("{TAB}");
                            SendKeys.Send("+{TAB}");
                        }

                        try
                        {
                            await StudioVariableMethods.UpdateVariable(nameCell.Value.ToString(), (Type)typeCell.Value, valueCell.Value?.ToString(), _scriptContext);
                            valueCell.Style = new DataGridViewCellStyle { ForeColor = Color.Black };
                        }
                        catch (Exception)
                        {
                            valueCell.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariablesArguments_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                if (dgv.Columns.Count == 4)
                {
                    //sets Direction to In by default when a new row is added. Prevents cell from ever being null
                    e.Row.Cells["Direction"].Value = ScriptArgumentDirection.In;
                }

                e.Row.Cells[1].Value = typeof(string);
                
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariables_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                var dgvBindingList = (BindingList<ScriptVariable>)dgv.DataSource;
                var nullScriptVariable = dgvBindingList.Where(x => string.IsNullOrEmpty(x.VariableName)).FirstOrDefault();

                if (nullScriptVariable != null && dgvBindingList.Count > 1)
                    dgvBindingList.Remove(nullScriptVariable);

            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvArguments_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                var dgvBindingList = (BindingList<ScriptArgument>)dgv.DataSource;
                var nullScriptVariable = dgvBindingList.Where(x => string.IsNullOrEmpty(x.ArgumentName)).FirstOrDefault();

                if (nullScriptVariable != null && dgvBindingList.Count > 1)
                    dgvBindingList.Remove(nullScriptVariable);

            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void ResetVariableArgumentBindings()
        {
            dgvVariables.DataSource = new BindingList<ScriptVariable>(_scriptContext.Variables);
            dgvArguments.DataSource = new BindingList<ScriptArgument>(_scriptContext.Arguments);

            TypeMethods.GenerateAllVariableTypes(NamespaceMethods.GetAssemblies(_scriptContext.ImportedNamespaces), _typeContext.GroupedTypes);

            var defaultTypesBinding = new BindingSource(_typeContext.DefaultTypes, null);
            variableType.DataSource = defaultTypesBinding;
            argumentType.DataSource = defaultTypesBinding;

            var importedNameSpacesBinding = new BindingSource(_scriptContext.ImportedNamespaces, null);
            lbxImportedNamespaces.DataSource = importedNameSpacesBinding;

            var allNameSpacesBinding = new BindingSource(_allNamespaces, null);
            cbxAllNamespaces.DataSource = allNameSpacesBinding;
        }

        private void SetVarArgTabControlSettings(ProjectType projectType)
        {
            switch (projectType)
            {
                case ProjectType.OpenBots:
                    splitContainerScript.Panel2Collapsed = false;

                    if (!uiVariableArgumentTabs.TabPages.Contains(variables))
                        uiVariableArgumentTabs.TabPages.Insert(0, variables);
                    if (!uiVariableArgumentTabs.TabPages.Contains(imports))
                        uiVariableArgumentTabs.TabPages.Add(imports);

                    dgvArguments.Columns["argumentType"].ReadOnly = false;
                    dgvArguments.Columns["direction"].ReadOnly = false;
                    break;
                case ProjectType.Python:
                case ProjectType.TagUI:
                case ProjectType.CSScript:
                    if (_isMainScript)
                        splitContainerScript.Panel2Collapsed = false;           
                    else
                        splitContainerScript.Panel2Collapsed = true;

                    uiVariableArgumentTabs.TabPages.Remove(variables);
                    uiVariableArgumentTabs.TabPages.Remove(imports);

                    dgvArguments.Columns["argumentType"].ReadOnly = true;
                    dgvArguments.Columns["direction"].ReadOnly = true;
                    break;
            }

        }

        private void dgvVariablesArguments_KeyDown(object sender, KeyEventArgs e)
        {
            //various advanced keystroke shortcuts for saving, creating new var/arg/elem, shortcut menu, etc
            if (e.Control)
            {
                if (e.Shift)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                            SaveAllFiles();
                            break;
                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                            ClearSelectedListViewItems();
                            if (_selectedTabScriptActions is ListView)
                                SaveToOpenBotsFile(false);
                            else
                                SaveToTextEditorFile(false);
                            break;
                        case Keys.J:
                            OpenArgumentManager();
                            break;
                        case Keys.K:
                            OpenVariableManager();
                            break;
                        case Keys.L:
                            OpenElementManager();
                            break;
                        case Keys.M:
                            shortcutMenuToolStripMenuItem_Click(null, null);
                            break;
                        case Keys.O:
                            aboutOpenBotsToolStripMenuItem_Click(null, null);
                            break;
                    }
                }
            }
        }
        #endregion

        #region Imported Namespaces
        private void cbxAllNamespaces_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var pair = (KeyValuePair<string, AssemblyReference>)cbxAllNamespaces.SelectedItem;
            if (!_scriptContext.ImportedNamespaces.ContainsKey(pair.Key))
            {
                _scriptContext.ImportedNamespaces.Add(pair.Key, pair.Value);
                var importedNameSpacesBinding = new BindingSource(_scriptContext.ImportedNamespaces, null);
                lbxImportedNamespaces.DataSource = importedNameSpacesBinding;

                TypeMethods.GenerateAllVariableTypes(NamespaceMethods.GetAssemblies(_scriptContext.ImportedNamespaces), _typeContext.GroupedTypes);

                //marks the script as unsaved with changes
                if (uiScriptTabControl.SelectedTab != null && !uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                    uiScriptTabControl.SelectedTab.Text += " *";
            }
        }

        private void lbxImportedNamespaces_KeyDown(object sender, KeyEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (e.KeyCode == Keys.Delete)
            {
                List<string> removaList = new List<string>();
                foreach (var item in listBox.SelectedItems)
                {
                    var pair = (KeyValuePair<string, AssemblyReference>)item;
                    removaList.Add(pair.Key);
                }

                removaList.ForEach(x => _scriptContext.ImportedNamespaces.Remove(x));
                var importedNameSpacesBinding = new BindingSource(_scriptContext.ImportedNamespaces, null);
                lbxImportedNamespaces.DataSource = importedNameSpacesBinding;

                TypeMethods.GenerateAllVariableTypes(NamespaceMethods.GetAssemblies(_scriptContext.ImportedNamespaces), _typeContext.GroupedTypes);

                //marks the script as unsaved with changes
                if (uiScriptTabControl.SelectedTab != null && !uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                    uiScriptTabControl.SelectedTab.Text += " *";
            }
            else
            {
                dgvVariablesArguments_KeyDown(null, e);
            }
        }

        private void lbxImportedNamespaces_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        
        private void cbxAllNamespaces_KeyDown(object sender, KeyEventArgs e)
        {
            dgvVariablesArguments_KeyDown(null, e);
        }

        private void cbxAllNamespaces_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        #endregion
    }
}
