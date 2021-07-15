
namespace era_tw_trainer
{
    partial class FrameForm
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
            this.buttonScan = new System.Windows.Forms.Button();
            this.textBoxSearchByNamePinyin = new System.Windows.Forms.TextBox();
            this.listBoxToons = new System.Windows.Forms.ListBox();
            this.labelName = new System.Windows.Forms.Label();
            this.labelHaoGan = new System.Windows.Forms.Label();
            this.labelKeYin = new System.Windows.Forms.Label();
            this.timerForLockLoop = new System.Windows.Forms.Timer(this.components);
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelPalam = new System.Windows.Forms.Label();
            this.labelSkills = new System.Windows.Forms.Label();
            this.checkBoxShowUnavailableToons = new System.Windows.Forms.CheckBox();
            this.labelFeatures = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonScan
            // 
            this.buttonScan.Location = new System.Drawing.Point(13, 13);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(50, 23);
            this.buttonScan.TabIndex = 0;
            this.buttonScan.Text = "Scan";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // textBoxSearchByNamePinyin
            // 
            this.textBoxSearchByNamePinyin.Location = new System.Drawing.Point(13, 42);
            this.textBoxSearchByNamePinyin.Name = "textBoxSearchByNamePinyin";
            this.textBoxSearchByNamePinyin.Size = new System.Drawing.Size(150, 21);
            this.textBoxSearchByNamePinyin.TabIndex = 49;
            this.textBoxSearchByNamePinyin.TextChanged += new System.EventHandler(this.textBoxSearchByNamePinyin_TextChanged);
            // 
            // listBoxToons
            // 
            this.listBoxToons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxToons.FormattingEnabled = true;
            this.listBoxToons.ItemHeight = 12;
            this.listBoxToons.Location = new System.Drawing.Point(13, 69);
            this.listBoxToons.Name = "listBoxToons";
            this.listBoxToons.Size = new System.Drawing.Size(150, 748);
            this.listBoxToons.TabIndex = 50;
            this.listBoxToons.SelectedIndexChanged += new System.EventHandler(this.listBoxToons_SelectedIndexChanged);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(169, 13);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 12);
            this.labelName.TabIndex = 51;
            this.labelName.Text = "Name:";
            // 
            // labelHaoGan
            // 
            this.labelHaoGan.AutoSize = true;
            this.labelHaoGan.Location = new System.Drawing.Point(460, 13);
            this.labelHaoGan.Name = "labelHaoGan";
            this.labelHaoGan.Size = new System.Drawing.Size(41, 12);
            this.labelHaoGan.TabIndex = 53;
            this.labelHaoGan.Text = "HaoGan";
            // 
            // labelKeYin
            // 
            this.labelKeYin.AutoSize = true;
            this.labelKeYin.Location = new System.Drawing.Point(169, 42);
            this.labelKeYin.Name = "labelKeYin";
            this.labelKeYin.Size = new System.Drawing.Size(41, 12);
            this.labelKeYin.TabIndex = 54;
            this.labelKeYin.Text = "Ke Yin";
            // 
            // timerForLockLoop
            // 
            this.timerForLockLoop.Enabled = true;
            this.timerForLockLoop.Interval = 200;
            this.timerForLockLoop.Tick += new System.EventHandler(this.timerForLockLoop_Tick);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(169, 71);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(41, 12);
            this.labelStatus.TabIndex = 56;
            this.labelStatus.Text = "Status";
            // 
            // labelPalam
            // 
            this.labelPalam.AutoSize = true;
            this.labelPalam.Location = new System.Drawing.Point(169, 158);
            this.labelPalam.Name = "labelPalam";
            this.labelPalam.Size = new System.Drawing.Size(35, 12);
            this.labelPalam.TabIndex = 57;
            this.labelPalam.Text = "Palam";
            // 
            // labelSkills
            // 
            this.labelSkills.AutoSize = true;
            this.labelSkills.Location = new System.Drawing.Point(580, 158);
            this.labelSkills.Name = "labelSkills";
            this.labelSkills.Size = new System.Drawing.Size(41, 12);
            this.labelSkills.TabIndex = 58;
            this.labelSkills.Text = "Skills";
            // 
            // checkBoxShowUnavailableToons
            // 
            this.checkBoxShowUnavailableToons.AutoSize = true;
            this.checkBoxShowUnavailableToons.Location = new System.Drawing.Point(70, 19);
            this.checkBoxShowUnavailableToons.Name = "checkBoxShowUnavailableToons";
            this.checkBoxShowUnavailableToons.Size = new System.Drawing.Size(60, 16);
            this.checkBoxShowUnavailableToons.TabIndex = 59;
            this.checkBoxShowUnavailableToons.Text = "未登场";
            this.checkBoxShowUnavailableToons.UseVisualStyleBackColor = true;
            this.checkBoxShowUnavailableToons.CheckedChanged += new System.EventHandler(this.checkBoxShowUnavailableToons_CheckedChanged);
            // 
            // labelFeatures
            // 
            this.labelFeatures.AutoSize = true;
            this.labelFeatures.Location = new System.Drawing.Point(169, 361);
            this.labelFeatures.Name = "labelFeatures";
            this.labelFeatures.Size = new System.Drawing.Size(35, 12);
            this.labelFeatures.TabIndex = 60;
            this.labelFeatures.Text = "Feats";
            // 
            // FrameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 837);
            this.Controls.Add(this.labelFeatures);
            this.Controls.Add(this.checkBoxShowUnavailableToons);
            this.Controls.Add(this.labelSkills);
            this.Controls.Add(this.labelPalam);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelKeYin);
            this.Controls.Add(this.labelHaoGan);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.listBoxToons);
            this.Controls.Add(this.textBoxSearchByNamePinyin);
            this.Controls.Add(this.buttonScan);
            this.Name = "FrameForm";
            this.Text = "FrameForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.TextBox textBoxSearchByNamePinyin;
        private System.Windows.Forms.ListBox listBoxToons;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelHaoGan;
        private System.Windows.Forms.Label labelKeYin;
        private System.Windows.Forms.Timer timerForLockLoop;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelPalam;
        private System.Windows.Forms.Label labelSkills;
        private System.Windows.Forms.CheckBox checkBoxShowUnavailableToons;
        private System.Windows.Forms.Label labelFeatures;
    }
}