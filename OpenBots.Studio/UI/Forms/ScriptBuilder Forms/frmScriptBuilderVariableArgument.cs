﻿using OpenBots.Core.Common;
using OpenBots.Core.Script;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        #region Variable/Argument Events
        private void dgvArguments_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                e.Row.Cells["Direction"].Value = ScriptArgumentDirection.In;
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariablesArguments_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //creates a list of all existing variable/argument names prior to creating a new one
                DataGridView dgv = (DataGridView)sender;

                _preEditVarArgName = dgv.Rows[e.RowIndex].Cells[0].Value?.ToString();

                _existingVarArgSearchList = new List<string>();
                _existingVarArgSearchList.AddRange(_scriptArguments.Select(arg => arg.ArgumentName).ToList());
                _existingVarArgSearchList.AddRange(_scriptVariables.Select(var => var.VariableName).ToList());
                _existingVarArgSearchList.AddRange(Common.GenerateSystemVariables().Select(var => var.VariableName).ToList());
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariablesArguments_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                if (e.ColumnIndex == 0)
                {
                    var cellValue = dgv.Rows[e.RowIndex].Cells[0].Value;
                    //deletes an empty row if it's created without assigning values
                    if ((cellValue == null && _preEditVarArgName != null) ||
                        (cellValue != null && string.IsNullOrEmpty(cellValue.ToString().Trim())))
                    {
                        dgv.Rows.RemoveAt(e.RowIndex);
                        return;
                    }
                    else if (dgv.Rows[e.RowIndex].Cells[0].Value == null)
                    {
                        return;
                    }

                    string variableName = dgv.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                    dgv.Rows[e.RowIndex].Cells[0].Value = variableName;

                    //prevents user from creating a new variable/argument with an already used name
                    if (_existingVarArgSearchList.Contains(variableName) && variableName != _preEditVarArgName)
                    {
                        Notify($"A variable or argument with the name '{variableName}' already exists", Color.Red);
                        dgv.Rows.RemoveAt(e.RowIndex);
                    }
                    //If the variable/argument name is valid, set value cell's readonly as false
                    else
                    {
                        foreach (DataGridViewCell cell in dgv.Rows[e.RowIndex].Cells)
                        {
                            cell.ReadOnly = false;                          
                        }

                        dgv.Rows[e.RowIndex].Cells[0].Value = variableName.Trim();

                        if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                            uiScriptTabControl.SelectedTab.Text += " *";
                    }
                }
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

        private void dgvVariables_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                //prevents the ProjectPath row from being deleted
                if (e.Row.Cells[0].Value?.ToString() == "ProjectPath")
                    e.Cancel = true;
                else
                {
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

        private void dgvVariables_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                //sets the entire ProjectPath row as readonly
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells[0].Value?.ToString() == "ProjectPath")
                        row.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }
        #endregion             
    }
}
