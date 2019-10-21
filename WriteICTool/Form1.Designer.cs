using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace WriteICTool
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
            this.cbPortName = new ComboBox();
            this.btnOpen = new Button();
            this.label2 = new Label();
            this.tbICNumber = new TextBox();
            this.btnWrite = new Button();
            this.rtbRecord = new RichTextBox();
            this.btnRead = new Button();
            this.tbNumber = new TextBox();
            this.label3 = new Label();
            this.btnReadIc = new Button();
            this.label1 = new Label();
            this.tbClientNumber = new TextBox();
            this.label4 = new Label();
            this.btnSetting = new Button();
            this.cbAutoWriteChecked = new CheckBox();
            this.cbDelay = new ComboBox();
            this.cbContrastDelay = new ComboBox();
            this.cbAutoContrastChecked = new CheckBox();
            this.btnContrast = new Button();
            this.pbImage = new PictureBox();
            ((ISupportInitialize)this.pbImage).BeginInit();
            base.SuspendLayout();
            this.cbPortName.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbPortName.FormattingEnabled = true;
            this.cbPortName.Location = new Point(162, 16);
            this.cbPortName.Margin = new Padding(3, 5, 3, 5);
            this.cbPortName.Name = "cbPortName";
            this.cbPortName.Size = new Size(140, 25);
            this.cbPortName.TabIndex = 1;
            this.btnOpen.Location = new Point(308, 15);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new Size(75, 26);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "打开";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += this.BtnOpen_Click;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(100, 120);
            this.label2.Name = "label2";
            this.label2.Size = new Size(56, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "IC卡号：";
            this.tbICNumber.Enabled = false;
            this.tbICNumber.Location = new Point(162, 117);
            this.tbICNumber.Margin = new Padding(3, 5, 3, 5);
            this.tbICNumber.MaxLength = 8;
            this.tbICNumber.Name = "tbICNumber";
            this.tbICNumber.Size = new Size(140, 23);
            this.tbICNumber.TabIndex = 4;
            this.tbICNumber.KeyPress += this.ICNumber_KeyPress;
            this.btnWrite.Enabled = false;
            this.btnWrite.Location = new Point(308, 149);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new Size(75, 26);
            this.btnWrite.TabIndex = 5;
            this.btnWrite.Text = "写入";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += this.BtnWrite_Click;
            this.rtbRecord.BackColor = Color.White;
            this.rtbRecord.Location = new Point(81, 218);
            this.rtbRecord.Name = "rtbRecord";
            this.rtbRecord.ReadOnly = true;
            this.rtbRecord.Size = new Size(302, 181);
            this.rtbRecord.TabIndex = 6;
            this.rtbRecord.Text = "";
            this.btnRead.Enabled = false;
            this.btnRead.Location = new Point(308, 82);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new Size(75, 26);
            this.btnRead.TabIndex = 9;
            this.btnRead.Text = "读取";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += this.BtnRead_Click;
            this.tbNumber.BackColor = Color.White;
            this.tbNumber.Enabled = false;
            this.tbNumber.Location = new Point(162, 84);
            this.tbNumber.Margin = new Padding(3, 5, 3, 5);
            this.tbNumber.MaxLength = 8;
            this.tbNumber.Name = "tbNumber";
            this.tbNumber.ReadOnly = true;
            this.tbNumber.Size = new Size(140, 23);
            this.tbNumber.TabIndex = 8;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(108, 87);
            this.label3.Name = "label3";
            this.label3.Size = new Size(48, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "卡 号：";
            this.btnReadIc.Location = new Point(308, 115);
            this.btnReadIc.Name = "btnReadIc";
            this.btnReadIc.Size = new Size(75, 26);
            this.btnReadIc.TabIndex = 1;
            this.btnReadIc.Text = "读取";
            this.btnReadIc.Click += this.BtnReadIc_Click;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(100, 20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(56, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "端口号：";
            this.tbClientNumber.BackColor = Color.White;
            this.tbClientNumber.Enabled = false;
            this.tbClientNumber.Location = new Point(162, 51);
            this.tbClientNumber.Margin = new Padding(3, 5, 3, 5);
            this.tbClientNumber.MaxLength = 4;
            this.tbClientNumber.Name = "tbClientNumber";
            this.tbClientNumber.Size = new Size(140, 23);
            this.tbClientNumber.TabIndex = 11;
            this.tbClientNumber.Text = "9887";
            this.tbClientNumber.KeyPress += this.ClientNumber_KeyPress;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(88, 54);
            this.label4.Name = "label4";
            this.label4.Size = new Size(68, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "客户编号：";
            this.btnSetting.Enabled = false;
            this.btnSetting.Location = new Point(308, 49);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new Size(75, 26);
            this.btnSetting.TabIndex = 12;
            this.btnSetting.Text = "设置";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += this.BtnSetting_Click;
            this.cbAutoWriteChecked.AutoSize = true;
            this.cbAutoWriteChecked.CheckAlign = ContentAlignment.MiddleRight;
            this.cbAutoWriteChecked.Enabled = false;
            this.cbAutoWriteChecked.Location = new Point(81, 152);
            this.cbAutoWriteChecked.Name = "cbAutoWriteChecked";
            this.cbAutoWriteChecked.Size = new Size(75, 21);
            this.cbAutoWriteChecked.TabIndex = 13;
            this.cbAutoWriteChecked.Text = "自动写入";
            this.cbAutoWriteChecked.UseVisualStyleBackColor = true;
            this.cbAutoWriteChecked.CheckedChanged += this.AutoWriteChecked_CheckedChanged;
            this.cbDelay.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbDelay.Enabled = false;
            this.cbDelay.FormattingEnabled = true;
            this.cbDelay.Items.AddRange(new object[]
            {
                "延迟一秒",
                "延迟二秒",
                "延迟三秒",
                "延迟四秒",
                "延迟五秒",
                "延迟六秒",
                "延迟七秒",
                "延迟八秒",
                "延迟九秒",
                "延迟十秒"
            });
            this.cbDelay.Location = new Point(162, 150);
            this.cbDelay.Margin = new Padding(3, 5, 3, 5);
            this.cbDelay.Name = "cbDelay";
            this.cbDelay.Size = new Size(140, 25);
            this.cbDelay.TabIndex = 14;
            this.cbDelay.SelectedIndexChanged += this.Delay_SelectedIndexChanged;
            this.cbContrastDelay.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbContrastDelay.Enabled = false;
            this.cbContrastDelay.FormattingEnabled = true;
            this.cbContrastDelay.Items.AddRange(new object[]
            {
                "延迟一秒",
                "延迟二秒",
                "延迟三秒",
                "延迟四秒",
                "延迟五秒",
                "延迟六秒",
                "延迟七秒",
                "延迟八秒",
                "延迟九秒",
                "延迟十秒"
            });
            this.cbContrastDelay.Location = new Point(162, 185);
            this.cbContrastDelay.Margin = new Padding(3, 5, 3, 5);
            this.cbContrastDelay.Name = "cbContrastDelay";
            this.cbContrastDelay.Size = new Size(140, 25);
            this.cbContrastDelay.TabIndex = 17;
            this.cbContrastDelay.SelectedIndexChanged += this.ContrastDelay_SelectedIndexChanged;
            this.cbAutoContrastChecked.AutoSize = true;
            this.cbAutoContrastChecked.CheckAlign = ContentAlignment.MiddleRight;
            this.cbAutoContrastChecked.Enabled = false;
            this.cbAutoContrastChecked.Location = new Point(81, 187);
            this.cbAutoContrastChecked.Name = "cbAutoContrastChecked";
            this.cbAutoContrastChecked.Size = new Size(75, 21);
            this.cbAutoContrastChecked.TabIndex = 16;
            this.cbAutoContrastChecked.Text = "自动测试";
            this.cbAutoContrastChecked.UseVisualStyleBackColor = true;
            this.cbAutoContrastChecked.CheckedChanged += this.AutoContrastChecked_CheckedChanged;
            this.btnContrast.Enabled = false;
            this.btnContrast.Location = new Point(308, 184);
            this.btnContrast.Name = "btnContrast";
            this.btnContrast.Size = new Size(75, 26);
            this.btnContrast.TabIndex = 15;
            this.btnContrast.Text = "对比";
            this.btnContrast.UseVisualStyleBackColor = true;
            this.btnContrast.Click += this.BtnContrast_Click;
            this.pbImage.Location = new Point(389, 185);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new Size(24, 24);
            this.pbImage.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pbImage.TabIndex = 18;
            this.pbImage.TabStop = false;
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(464, 411);
            base.Controls.Add(this.pbImage);
            base.Controls.Add(this.cbContrastDelay);
            base.Controls.Add(this.cbAutoContrastChecked);
            base.Controls.Add(this.btnContrast);
            base.Controls.Add(this.cbDelay);
            base.Controls.Add(this.cbAutoWriteChecked);
            base.Controls.Add(this.btnSetting);
            base.Controls.Add(this.tbClientNumber);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnReadIc);
            base.Controls.Add(this.btnRead);
            base.Controls.Add(this.tbNumber);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.rtbRecord);
            base.Controls.Add(this.btnWrite);
            base.Controls.Add(this.tbICNumber);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.btnOpen);
            base.Controls.Add(this.cbPortName);
            this.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.KeyPreview = true;
            base.Margin = new Padding(3, 4, 3, 4);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmMain";
            this.Text = "写IC卡编号工具";
            base.Load += this.Main_Load;
            base.Shown += this.Main_Shown;
            base.KeyUp += this.Main_KeyUp;
            ((ISupportInitialize)this.pbImage).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();

        }

        #endregion

        // Token: 0x0400000A RID: 10
        private ComboBox cbPortName;

        // Token: 0x0400000B RID: 11
        private Button btnOpen;

        // Token: 0x0400000C RID: 12
        private Label label2;

        // Token: 0x0400000D RID: 13
        private TextBox tbICNumber;

        // Token: 0x0400000E RID: 14
        private Button btnWrite;

        // Token: 0x0400000F RID: 15
        private RichTextBox rtbRecord;

        // Token: 0x04000010 RID: 16
        private Button btnRead;

        // Token: 0x04000011 RID: 17
        private TextBox tbNumber;

        // Token: 0x04000012 RID: 18
        private Label label3;

        // Token: 0x04000013 RID: 19
        private Button btnReadIc;

        // Token: 0x04000014 RID: 20
        private Label label1;

        // Token: 0x04000015 RID: 21
        private TextBox tbClientNumber;

        // Token: 0x04000016 RID: 22
        private Label label4;

        // Token: 0x04000017 RID: 23
        private Button btnSetting;

        // Token: 0x04000018 RID: 24
        private CheckBox cbAutoWriteChecked;

        // Token: 0x04000019 RID: 25
        private ComboBox cbDelay;

        // Token: 0x0400001A RID: 26
        private ComboBox cbContrastDelay;

        // Token: 0x0400001B RID: 27
        private CheckBox cbAutoContrastChecked;

        // Token: 0x0400001C RID: 28
        private Button btnContrast;

        // Token: 0x0400001D RID: 29
        private PictureBox pbImage;
    }
}

