namespace Mapdigit
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.btnUp = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblMapType = new System.Windows.Forms.Label();
            this.cboMapType = new System.Windows.Forms.ComboBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picMapCanvas = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.btnGetDirection = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMapCanvas)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUp
            // 
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnLeft
            // 
            resources.ApplyResources(this.btnLeft, "btnLeft");
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnRight
            // 
            resources.ApplyResources(this.btnRight, "btnRight");
            this.btnRight.Name = "btnRight";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnDown
            // 
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Name = "btnDown";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnZoomIn
            // 
            resources.ApplyResources(this.btnZoomIn, "btnZoomIn");
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            resources.ApplyResources(this.btnZoomOut, "btnZoomOut");
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnGo
            // 
            resources.ApplyResources(this.btnGo, "btnGo");
            this.btnGo.Name = "btnGo";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtAddress
            // 
            resources.ApplyResources(this.txtAddress, "txtAddress");
            this.txtAddress.Name = "txtAddress";
            // 
            // lblMessage
            // 
            resources.ApplyResources(this.lblMessage, "lblMessage");
            this.lblMessage.ForeColor = System.Drawing.Color.Black;
            this.lblMessage.Name = "lblMessage";
            // 
            // lblMapType
            // 
            resources.ApplyResources(this.lblMapType, "lblMapType");
            this.lblMapType.Name = "lblMapType";
            // 
            // cboMapType
            // 
            resources.ApplyResources(this.cboMapType, "cboMapType");
            this.cboMapType.FormattingEnabled = true;
            this.cboMapType.Name = "cboMapType";
            this.cboMapType.SelectedIndexChanged += new System.EventHandler(this.cboMapType_SelectedIndexChanged);
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.Name = "btnExit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.picMapCanvas);
            this.panel1.Name = "panel1";
            // 
            // picMapCanvas
            // 
            resources.ApplyResources(this.picMapCanvas, "picMapCanvas");
            this.picMapCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picMapCanvas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMapCanvas.Name = "picMapCanvas";
            this.picMapCanvas.TabStop = false;
            this.picMapCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.picMapCanvas_Paint);
            this.picMapCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picMapCanvas_MouseDown);
            this.picMapCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picMapCanvas_MouseMove);
            // 
            // lblStatus
            // 
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Name = "lblStatus";
            // 
            // txtAddress1
            // 
            resources.ApplyResources(this.txtAddress1, "txtAddress1");
            this.txtAddress1.Name = "txtAddress1";
            // 
            // txtAddress2
            // 
            resources.ApplyResources(this.txtAddress2, "txtAddress2");
            this.txtAddress2.Name = "txtAddress2";
            // 
            // btnGetDirection
            // 
            resources.ApplyResources(this.btnGetDirection, "btnGetDirection");
            this.btnGetDirection.Name = "btnGetDirection";
            this.btnGetDirection.UseVisualStyleBackColor = true;
            this.btnGetDirection.Click += new System.EventHandler(this.btnGetDirection_Click);
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnGetDirection);
            this.Controls.Add(this.txtAddress2);
            this.Controls.Add(this.txtAddress1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.cboMapType);
            this.Controls.Add(this.lblMapType);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnZoomOut);
            this.Controls.Add(this.btnZoomIn);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnUp);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picMapCanvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblMapType;
        private System.Windows.Forms.ComboBox cboMapType;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picMapCanvas;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtAddress1;
        private System.Windows.Forms.TextBox txtAddress2;
        private System.Windows.Forms.Button btnGetDirection;
    }
}