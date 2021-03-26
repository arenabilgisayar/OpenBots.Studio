﻿namespace OpenBots.UI.Forms.Supplement_Forms
{
    partial class frmTypes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tlpCommands = new System.Windows.Forms.TableLayoutPanel();
            this.tvTypes = new OpenBots.UI.CustomControls.CustomUIControls.UITreeView();
            this.pnlCommandSearch = new System.Windows.Forms.Panel();
            this.uiBtnCollapse = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.uiBtnExpand = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.uiBtnClearTypeSearch = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.txtTypeSearch = new System.Windows.Forms.TextBox();
            this.ttTypeButtons = new System.Windows.Forms.ToolTip(this.components);
            this.tlpCommands.SuspendLayout();
            this.pnlCommandSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCollapse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnExpand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearTypeSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpCommands
            // 
            this.tlpCommands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(136)))), ((int)(((byte)(204)))));
            this.tlpCommands.ColumnCount = 1;
            this.tlpCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCommands.Controls.Add(this.tvTypes, 0, 1);
            this.tlpCommands.Controls.Add(this.pnlCommandSearch, 0, 0);
            this.tlpCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCommands.Location = new System.Drawing.Point(0, 0);
            this.tlpCommands.Margin = new System.Windows.Forms.Padding(2);
            this.tlpCommands.Name = "tlpCommands";
            this.tlpCommands.RowCount = 2;
            this.tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCommands.Size = new System.Drawing.Size(672, 670);
            this.tlpCommands.TabIndex = 11;
            // 
            // tvTypes
            // 
            this.tvTypes.AllowDrop = true;
            this.tvTypes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tvTypes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTypes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tvTypes.ForeColor = System.Drawing.Color.White;
            this.tvTypes.Location = new System.Drawing.Point(4, 35);
            this.tvTypes.Margin = new System.Windows.Forms.Padding(4);
            this.tvTypes.Name = "tvTypes";
            this.tvTypes.ShowLines = false;
            this.tvTypes.ShowNodeToolTips = true;
            this.tvTypes.Size = new System.Drawing.Size(664, 631);
            this.tvTypes.TabIndex = 9;
            this.tvTypes.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvTypes_NodeMouseDoubleClick);
            // 
            // pnlCommandSearch
            // 
            this.pnlCommandSearch.Controls.Add(this.uiBtnCollapse);
            this.pnlCommandSearch.Controls.Add(this.uiBtnExpand);
            this.pnlCommandSearch.Controls.Add(this.uiBtnClearTypeSearch);
            this.pnlCommandSearch.Controls.Add(this.txtTypeSearch);
            this.pnlCommandSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommandSearch.Location = new System.Drawing.Point(2, 2);
            this.pnlCommandSearch.Margin = new System.Windows.Forms.Padding(2);
            this.pnlCommandSearch.Name = "pnlCommandSearch";
            this.pnlCommandSearch.Size = new System.Drawing.Size(668, 27);
            this.pnlCommandSearch.TabIndex = 10;
            // 
            // uiBtnCollapse
            // 
            this.uiBtnCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnCollapse.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCollapse.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCollapse.DisplayText = null;
            this.uiBtnCollapse.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCollapse.Image = global::OpenBots.Properties.Resources.project_collapse;
            this.uiBtnCollapse.IsMouseOver = false;
            this.uiBtnCollapse.Location = new System.Drawing.Point(640, 1);
            this.uiBtnCollapse.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnCollapse.Name = "uiBtnCollapse";
            this.uiBtnCollapse.Size = new System.Drawing.Size(25, 25);
            this.uiBtnCollapse.TabIndex = 4;
            this.uiBtnCollapse.TabStop = false;
            this.uiBtnCollapse.Text = null;
            this.ttTypeButtons.SetToolTip(this.uiBtnCollapse, "Collapse");
            this.uiBtnCollapse.Click += new System.EventHandler(this.uiBtnCollapse_Click);
            // 
            // uiBtnExpand
            // 
            this.uiBtnExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnExpand.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnExpand.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnExpand.DisplayText = null;
            this.uiBtnExpand.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnExpand.Image = global::OpenBots.Properties.Resources.project_expand;
            this.uiBtnExpand.IsMouseOver = false;
            this.uiBtnExpand.Location = new System.Drawing.Point(612, 1);
            this.uiBtnExpand.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnExpand.Name = "uiBtnExpand";
            this.uiBtnExpand.Size = new System.Drawing.Size(25, 25);
            this.uiBtnExpand.TabIndex = 2;
            this.uiBtnExpand.TabStop = false;
            this.uiBtnExpand.Text = null;
            this.ttTypeButtons.SetToolTip(this.uiBtnExpand, "Expand");
            this.uiBtnExpand.Click += new System.EventHandler(this.uiBtnExpand_Click);
            // 
            // uiBtnClearTypeSearch
            // 
            this.uiBtnClearTypeSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnClearTypeSearch.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnClearTypeSearch.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnClearTypeSearch.DisplayText = null;
            this.uiBtnClearTypeSearch.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnClearTypeSearch.Image = global::OpenBots.Properties.Resources.commandsearch_clear;
            this.uiBtnClearTypeSearch.IsMouseOver = false;
            this.uiBtnClearTypeSearch.Location = new System.Drawing.Point(584, 1);
            this.uiBtnClearTypeSearch.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnClearTypeSearch.Name = "uiBtnClearTypeSearch";
            this.uiBtnClearTypeSearch.Size = new System.Drawing.Size(25, 25);
            this.uiBtnClearTypeSearch.TabIndex = 1;
            this.uiBtnClearTypeSearch.TabStop = false;
            this.uiBtnClearTypeSearch.Text = null;
            this.ttTypeButtons.SetToolTip(this.uiBtnClearTypeSearch, "Clear Search");
            this.uiBtnClearTypeSearch.Click += new System.EventHandler(this.uiBtnClearTypeSearch_Click);
            // 
            // txtTypeSearch
            // 
            this.txtTypeSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTypeSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.txtTypeSearch.ForeColor = System.Drawing.Color.LightGray;
            this.txtTypeSearch.Location = new System.Drawing.Point(0, 0);
            this.txtTypeSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtTypeSearch.Name = "txtTypeSearch";
            this.txtTypeSearch.Size = new System.Drawing.Size(580, 30);
            this.txtTypeSearch.TabIndex = 0;
            this.txtTypeSearch.Text = "Type Here to Search";
            this.txtTypeSearch.TextChanged += new System.EventHandler(this.txtTypeSearch_TextChanged);
            this.txtTypeSearch.Enter += new System.EventHandler(this.txtTypeSearch_Enter);
            this.txtTypeSearch.Leave += new System.EventHandler(this.txtTypeSearch_Leave);
            // 
            // frmTypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 670);
            this.Controls.Add(this.tlpCommands);
            this.Name = "frmTypes";
            this.Text = "Types";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmTypes_Load);
            this.tlpCommands.ResumeLayout(false);
            this.pnlCommandSearch.ResumeLayout(false);
            this.pnlCommandSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCollapse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnExpand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearTypeSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpCommands;
        private CustomControls.CustomUIControls.UITreeView tvTypes;
        private System.Windows.Forms.Panel pnlCommandSearch;
        private CustomControls.CustomUIControls.UIIconButton uiBtnClearTypeSearch;
        private System.Windows.Forms.TextBox txtTypeSearch;
        private CustomControls.CustomUIControls.UIIconButton uiBtnExpand;
        private CustomControls.CustomUIControls.UIIconButton uiBtnCollapse;
        private System.Windows.Forms.ToolTip ttTypeButtons;
    }
}