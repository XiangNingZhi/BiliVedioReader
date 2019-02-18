namespace BiliViewReader2._0
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.uidBox = new System.Windows.Forms.TextBox();
            this.ViewBox = new System.Windows.Forms.TextBox();
            this.selectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(518, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "UP主uid";
            // 
            // uidBox
            // 
            this.uidBox.Location = new System.Drawing.Point(618, 126);
            this.uidBox.Name = "uidBox";
            this.uidBox.Size = new System.Drawing.Size(153, 35);
            this.uidBox.TabIndex = 1;
            // 
            // ViewBox
            // 
            this.ViewBox.Location = new System.Drawing.Point(12, 295);
            this.ViewBox.Multiline = true;
            this.ViewBox.Name = "ViewBox";
            this.ViewBox.ReadOnly = true;
            this.ViewBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ViewBox.Size = new System.Drawing.Size(1550, 522);
            this.ViewBox.TabIndex = 2;
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(618, 186);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(150, 50);
            this.selectButton.TabIndex = 3;
            this.selectButton.Text = "查询";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1574, 829);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.ViewBox);
            this.Controls.Add(this.uidBox);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "UP视频播放量获取";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox uidBox;
        private System.Windows.Forms.TextBox ViewBox;
        private System.Windows.Forms.Button selectButton;
    }
}

