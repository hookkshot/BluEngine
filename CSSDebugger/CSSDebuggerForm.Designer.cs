namespace CSSDebugger
{
    partial class CSSDebuggerForm
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
            this.verticalSplit = new System.Windows.Forms.SplitContainer();
            this.tbInput = new FastColoredTextBoxNS.FastColoredTextBox();
            this.lblInput = new System.Windows.Forms.Label();
            this.tbOutput = new FastColoredTextBoxNS.FastColoredTextBox();
            this.lblOutput = new System.Windows.Forms.Label();
            this.horizontalSplit = new System.Windows.Forms.SplitContainer();
            this.infoLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.verticalSplit)).BeginInit();
            this.verticalSplit.Panel1.SuspendLayout();
            this.verticalSplit.Panel2.SuspendLayout();
            this.verticalSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalSplit)).BeginInit();
            this.horizontalSplit.Panel1.SuspendLayout();
            this.horizontalSplit.Panel2.SuspendLayout();
            this.horizontalSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // verticalSplit
            // 
            this.verticalSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.verticalSplit.Location = new System.Drawing.Point(0, 0);
            this.verticalSplit.Name = "verticalSplit";
            // 
            // verticalSplit.Panel1
            // 
            this.verticalSplit.Panel1.Controls.Add(this.tbInput);
            this.verticalSplit.Panel1.Controls.Add(this.lblInput);
            // 
            // verticalSplit.Panel2
            // 
            this.verticalSplit.Panel2.Controls.Add(this.tbOutput);
            this.verticalSplit.Panel2.Controls.Add(this.lblOutput);
            this.verticalSplit.Size = new System.Drawing.Size(776, 393);
            this.verticalSplit.SplitterDistance = 358;
            this.verticalSplit.TabIndex = 1;
            // 
            // tbInput
            // 
            this.tbInput.AllowDrop = true;
            this.tbInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInput.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.tbInput.BackBrush = null;
            this.tbInput.BackColor = System.Drawing.Color.DimGray;
            this.tbInput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbInput.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.tbInput.ForeColor = System.Drawing.Color.White;
            this.tbInput.IndentBackColor = System.Drawing.Color.Black;
            this.tbInput.LeftBracket = '}';
            this.tbInput.LineNumberColor = System.Drawing.Color.Orange;
            this.tbInput.Location = new System.Drawing.Point(3, 26);
            this.tbInput.Name = "tbInput";
            this.tbInput.Paddings = new System.Windows.Forms.Padding(0);
            this.tbInput.RightBracket = '{';
            this.tbInput.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.tbInput.Size = new System.Drawing.Size(352, 364);
            this.tbInput.TabIndex = 2;
            this.tbInput.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.tbInput_TextChanged);
            // 
            // lblInput
            // 
            this.lblInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInput.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInput.Location = new System.Drawing.Point(0, 0);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(358, 23);
            this.lblInput.TabIndex = 1;
            this.lblInput.Text = "Input";
            this.lblInput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbOutput
            // 
            this.tbOutput.AllowDrop = true;
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.tbOutput.BackBrush = null;
            this.tbOutput.BackColor = System.Drawing.Color.SteelBlue;
            this.tbOutput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbOutput.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.tbOutput.ForeColor = System.Drawing.Color.White;
            this.tbOutput.IndentBackColor = System.Drawing.Color.Black;
            this.tbOutput.LineNumberColor = System.Drawing.Color.Orange;
            this.tbOutput.Location = new System.Drawing.Point(3, 26);
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.Paddings = new System.Windows.Forms.Padding(0);
            this.tbOutput.ReadOnly = true;
            this.tbOutput.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.tbOutput.Size = new System.Drawing.Size(408, 364);
            this.tbOutput.TabIndex = 2;
            // 
            // lblOutput
            // 
            this.lblOutput.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOutput.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutput.Location = new System.Drawing.Point(0, 0);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(414, 23);
            this.lblOutput.TabIndex = 1;
            this.lblOutput.Text = "Output";
            this.lblOutput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // horizontalSplit
            // 
            this.horizontalSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.horizontalSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.horizontalSplit.IsSplitterFixed = true;
            this.horizontalSplit.Location = new System.Drawing.Point(0, 0);
            this.horizontalSplit.Name = "horizontalSplit";
            this.horizontalSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // horizontalSplit.Panel1
            // 
            this.horizontalSplit.Panel1.Controls.Add(this.infoLabel);
            // 
            // horizontalSplit.Panel2
            // 
            this.horizontalSplit.Panel2.Controls.Add(this.verticalSplit);
            this.horizontalSplit.Size = new System.Drawing.Size(776, 437);
            this.horizontalSplit.SplitterDistance = 40;
            this.horizontalSplit.TabIndex = 2;
            // 
            // infoLabel
            // 
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.Location = new System.Drawing.Point(0, 0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(776, 40);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.Text = "Paste your BluEngine CSS in the left textbox; All validly-parsed BluEngine CSS wi" +
    "ll be mirrored in the right box.\r\nAnything causing an error will be omitted.";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CSSDebuggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 437);
            this.Controls.Add(this.horizontalSplit);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CSSDebuggerForm";
            this.Text = "BluEngine CSS Debugger";
            this.verticalSplit.Panel1.ResumeLayout(false);
            this.verticalSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.verticalSplit)).EndInit();
            this.verticalSplit.ResumeLayout(false);
            this.horizontalSplit.Panel1.ResumeLayout(false);
            this.horizontalSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.horizontalSplit)).EndInit();
            this.horizontalSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer verticalSplit;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.Label lblOutput;
        private FastColoredTextBoxNS.FastColoredTextBox tbInput;
        private FastColoredTextBoxNS.FastColoredTextBox tbOutput;
        private System.Windows.Forms.SplitContainer horizontalSplit;
        private System.Windows.Forms.Label infoLabel;
    }
}

