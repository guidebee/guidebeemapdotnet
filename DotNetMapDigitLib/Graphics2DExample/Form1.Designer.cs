namespace Graphics2DExample
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.examplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineCapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineJoinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dashToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ovalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.polysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gradientsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beziersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.examplesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(312, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // examplesToolStripMenuItem
            // 
            this.examplesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorToolStripMenuItem,
            this.lineCapToolStripMenuItem,
            this.lineJoinToolStripMenuItem,
            this.dashToolStripMenuItem,
            this.pearToolStripMenuItem,
            this.ovalsToolStripMenuItem,
            this.pathsToolStripMenuItem,
            this.polysToolStripMenuItem,
            this.transformToolStripMenuItem,
            this.gradientsToolStripMenuItem,
            this.beziersToolStripMenuItem,
            this.fontTypesToolStripMenuItem});
            this.examplesToolStripMenuItem.Name = "examplesToolStripMenuItem";
            this.examplesToolStripMenuItem.Size = new System.Drawing.Size(69, 21);
            this.examplesToolStripMenuItem.Text = "Example";
            // 
            // colorToolStripMenuItem
            // 
            this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            this.colorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.colorToolStripMenuItem.Text = "Color";
            this.colorToolStripMenuItem.Click += new System.EventHandler(this.colorToolStripMenuItem_Click);
            // 
            // lineCapToolStripMenuItem
            // 
            this.lineCapToolStripMenuItem.Name = "lineCapToolStripMenuItem";
            this.lineCapToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lineCapToolStripMenuItem.Text = "LineCap";
            this.lineCapToolStripMenuItem.Click += new System.EventHandler(this.lineCapToolStripMenuItem_Click);
            // 
            // lineJoinToolStripMenuItem
            // 
            this.lineJoinToolStripMenuItem.Name = "lineJoinToolStripMenuItem";
            this.lineJoinToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lineJoinToolStripMenuItem.Text = "LineJoin";
            this.lineJoinToolStripMenuItem.Click += new System.EventHandler(this.lineJoinToolStripMenuItem_Click);
            // 
            // dashToolStripMenuItem
            // 
            this.dashToolStripMenuItem.Name = "dashToolStripMenuItem";
            this.dashToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dashToolStripMenuItem.Text = "Dash";
            this.dashToolStripMenuItem.Click += new System.EventHandler(this.dashToolStripMenuItem_Click);
            // 
            // pearToolStripMenuItem
            // 
            this.pearToolStripMenuItem.Name = "pearToolStripMenuItem";
            this.pearToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pearToolStripMenuItem.Text = "Pear";
            this.pearToolStripMenuItem.Click += new System.EventHandler(this.pearToolStripMenuItem_Click);
            // 
            // ovalsToolStripMenuItem
            // 
            this.ovalsToolStripMenuItem.Name = "ovalsToolStripMenuItem";
            this.ovalsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ovalsToolStripMenuItem.Text = "Ovals";
            this.ovalsToolStripMenuItem.Click += new System.EventHandler(this.ovalsToolStripMenuItem_Click);
            // 
            // pathsToolStripMenuItem
            // 
            this.pathsToolStripMenuItem.Name = "pathsToolStripMenuItem";
            this.pathsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pathsToolStripMenuItem.Text = "Paths";
            this.pathsToolStripMenuItem.Click += new System.EventHandler(this.pathsToolStripMenuItem_Click);
            // 
            // polysToolStripMenuItem
            // 
            this.polysToolStripMenuItem.Name = "polysToolStripMenuItem";
            this.polysToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.polysToolStripMenuItem.Text = "Polys";
            this.polysToolStripMenuItem.Click += new System.EventHandler(this.polysToolStripMenuItem_Click);
            // 
            // transformToolStripMenuItem
            // 
            this.transformToolStripMenuItem.Name = "transformToolStripMenuItem";
            this.transformToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.transformToolStripMenuItem.Text = "Transform";
            this.transformToolStripMenuItem.Click += new System.EventHandler(this.transformToolStripMenuItem_Click);
            // 
            // gradientsToolStripMenuItem
            // 
            this.gradientsToolStripMenuItem.Name = "gradientsToolStripMenuItem";
            this.gradientsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gradientsToolStripMenuItem.Text = "Gradients";
            // 
            // beziersToolStripMenuItem
            // 
            this.beziersToolStripMenuItem.Name = "beziersToolStripMenuItem";
            this.beziersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.beziersToolStripMenuItem.Text = "Beziers";
            this.beziersToolStripMenuItem.Click += new System.EventHandler(this.beziersToolStripMenuItem_Click);
            // 
            // fontTypesToolStripMenuItem
            // 
            this.fontTypesToolStripMenuItem.Name = "fontTypesToolStripMenuItem";
            this.fontTypesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fontTypesToolStripMenuItem.Text = "FontTypes";
            this.fontTypesToolStripMenuItem.Click += new System.EventHandler(this.fontTypesToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 446);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Graphics 2D Example";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem examplesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineCapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineJoinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dashToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ovalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pathsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem polysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transformToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gradientsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beziersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontTypesToolStripMenuItem;
    }
}

