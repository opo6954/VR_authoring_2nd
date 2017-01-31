using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Resources;
using System.Windows.Forms;

public class LoadShipOptionForm : Form {
	private System.ComponentModel.IContainer components = null;

	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form 디자이너에서 생성한 코드

	/// <summary>
	/// 디자이너 지원에 필요한 메서드입니다.
	/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
	/// </summary>
	private void InitializeComponent()
	{
		this.checkBox_flipXY = new System.Windows.Forms.CheckBox();
		this.button_OK = new System.Windows.Forms.Button();
		this.checkBox_VR = new System.Windows.Forms.CheckBox();
		this.comboBox_VR = new System.Windows.Forms.ComboBox();
		this.checkBox_ODT = new System.Windows.Forms.CheckBox();
		this.groupBox_Input = new System.Windows.Forms.GroupBox();
		this.radioButton_Keyboard = new System.Windows.Forms.RadioButton();
		this.radioButton_Joystick = new System.Windows.Forms.RadioButton();
		this.groupBox_Input.SuspendLayout();
		this.SuspendLayout();
		// 
		// checkBox_flipXY
		// 
		this.checkBox_flipXY.AutoSize = true;
		this.checkBox_flipXY.Location = new System.Drawing.Point(12, 12);
		this.checkBox_flipXY.Name = "checkBox_flipXY";
		this.checkBox_flipXY.Size = new System.Drawing.Size(64, 16);
		this.checkBox_flipXY.TabIndex = 0;
		this.checkBox_flipXY.Text = "Flip XY";
		this.checkBox_flipXY.UseVisualStyleBackColor = true;
		// 
		// button_OK
		// 
		this.button_OK.Location = new System.Drawing.Point(191, 111);
		this.button_OK.Name = "button_OK";
		this.button_OK.Size = new System.Drawing.Size(82, 36);
		this.button_OK.TabIndex = 1;
		this.button_OK.Text = "확인";
		this.button_OK.UseVisualStyleBackColor = true;
		this.checkBox_flipXY.CheckedChanged += new System.EventHandler(this.checkBox_flipXY_CheckedChanged);
		this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
		// 
		// checkBox_VR
		// 
		this.checkBox_VR.AutoSize = true;
		this.checkBox_VR.Location = new System.Drawing.Point(12, 34);
		this.checkBox_VR.Name = "checkBox_VR";
		this.checkBox_VR.Size = new System.Drawing.Size(87, 16);
		this.checkBox_VR.TabIndex = 2;
		this.checkBox_VR.Text = "VR Support";
		this.checkBox_VR.UseVisualStyleBackColor = true;
		this.checkBox_VR.CheckedChanged += new System.EventHandler(this.checkBox_VR_CheckedChanged);
		// 
		// comboBox_VR
		// 
		this.comboBox_VR.FormattingEnabled = true;
		this.comboBox_VR.Items.AddRange(new object[] {
			"Occulus - Rift",
			"HTC - Vive",
			"Samsung - GearVR",
			"Sony - PSVR"});
		this.comboBox_VR.Location = new System.Drawing.Point(125, 30);
		this.comboBox_VR.Name = "comboBox_VR";
		this.comboBox_VR.Size = new System.Drawing.Size(130, 20);
		this.comboBox_VR.TabIndex = 3;
		this.comboBox_VR.SelectedIndex = 0;
		this.comboBox_VR.Enabled = false;
		// 
		// checkBox_ODT
		// 
		this.checkBox_ODT.AutoSize = true;
		this.checkBox_ODT.Location = new System.Drawing.Point(12, 56);
		this.checkBox_ODT.Name = "checkBox_ODT";
		this.checkBox_ODT.Size = new System.Drawing.Size(96, 16);
		this.checkBox_ODT.TabIndex = 4;
		this.checkBox_ODT.Text = "ODT Support";
		this.checkBox_ODT.UseVisualStyleBackColor = true;
		// 
		// groupBox_Input
		// 
		this.groupBox_Input.Controls.Add(this.radioButton_Joystick);
		this.groupBox_Input.Controls.Add(this.radioButton_Keyboard);
		this.groupBox_Input.Location = new System.Drawing.Point(12, 80);
		this.groupBox_Input.Name = "groupBox_Input";
		this.groupBox_Input.Size = new System.Drawing.Size(160, 67);
		this.groupBox_Input.TabIndex = 5;
		this.groupBox_Input.TabStop = false;
		this.groupBox_Input.Text = "Input Option";
		// 
		// radioButton_Keyboard
		// 
		this.radioButton_Keyboard.AutoSize = true;
		this.radioButton_Keyboard.Checked = true;
		this.radioButton_Keyboard.Location = new System.Drawing.Point(6, 20);
		this.radioButton_Keyboard.Name = "radioButton_Keyboard";
		this.radioButton_Keyboard.Size = new System.Drawing.Size(77, 16);
		this.radioButton_Keyboard.TabIndex = 0;
		this.radioButton_Keyboard.TabStop = true;
		this.radioButton_Keyboard.Text = "Keyboard";
		this.radioButton_Keyboard.UseVisualStyleBackColor = true;
		// 
		// radioButton_Joystick
		// 
		this.radioButton_Joystick.AutoSize = true;
		this.radioButton_Joystick.Location = new System.Drawing.Point(6, 42);
		this.radioButton_Joystick.Name = "radioButton_Joystick";
		this.radioButton_Joystick.Size = new System.Drawing.Size(69, 16);
		this.radioButton_Joystick.TabIndex = 1;
		this.radioButton_Joystick.Text = "Joystick";
		this.radioButton_Joystick.UseVisualStyleBackColor = true;
		// 
		// LoadOptionForm
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(285, 159);
		this.Controls.Add(this.groupBox_Input);
		this.Controls.Add(this.checkBox_ODT);
		this.Controls.Add(this.comboBox_VR);
		this.Controls.Add(this.checkBox_VR);
		this.Controls.Add(this.button_OK);
		this.Controls.Add(this.checkBox_flipXY);
		this.Name = "LoadOptionForm";
		this.Text = "Load Options";
//		this.Icon = new Icon("Assets/Editor/options.ico");
		using (ResXResourceSet resxSet = new ResXResourceSet(@".\Assets\Editor\LoadShipOption"))
		{
//			this.Icon = (Icon)resxSet.GetObject("options", true);
//			this.Text = resxSet.GetString ("Title");
		}
		using (ResXResourceReader resx = new ResXResourceReader(@".\Assets\Editor\LoadShipOption.resx"))
		{
			foreach (DictionaryEntry entry in resx) {
				if (((string)entry.Key) == "options")
					this.Icon = (Icon)entry.Value;
			}
		}
		this.groupBox_Input.ResumeLayout(false);
		this.groupBox_Input.PerformLayout();
		this.ResumeLayout(false);
		this.PerformLayout();
	}

	#endregion

	private System.Windows.Forms.CheckBox checkBox_flipXY;
	private System.Windows.Forms.Button button_OK;
	private System.Windows.Forms.CheckBox checkBox_VR;
	private System.Windows.Forms.ComboBox comboBox_VR;
	private System.Windows.Forms.CheckBox checkBox_ODT;
	private System.Windows.Forms.GroupBox groupBox_Input;
	private System.Windows.Forms.RadioButton radioButton_Joystick;
	private System.Windows.Forms.RadioButton radioButton_Keyboard;

	private void checkBox_flipXY_CheckedChanged(object sender, EventArgs e)
	{
		flipXY = checkBox_flipXY.Checked;
//		using (ResXResourceReader resxReader = new ResXResourceReader(@".\LoadShipOption.resx"))
//		{
//			foreach (DictionaryEntry entry in resxReader) {
//				this.Text = (string)entry.Value;      
//			} 
//		}
//		using (ResXResourceWriter resx = new ResXResourceWriter(@".\aimlab.resx"))
//		{
//			Icon image = new Icon(@".\Assets\Editor\options.ico");
//			resx.AddResource ("options", image);
//			resx.AddResource("Title", "Classic American Cars");
//			resx.AddResource("HeaderString1", "Make");
//			resx.AddResource("HeaderString2", "Model");
//			resx.AddResource("HeaderString3", "Year");
//			resx.AddResource("HeaderString4", "Doors");
//			resx.AddResource("HeaderString5", "Cylinders");
//			resx.Generate();
//			resx.Close ();
//		}
	}
	private void button_OK_Click(object sender, EventArgs e) {
		this.DialogResult = DialogResult.OK;
		Close ();
	}

	public LoadShipOptionForm()
	{
		InitializeComponent();
		flipXY = this.checkBox_flipXY.Checked;
		this.button_OK.Select ();
	}

	private void checkBox_VR_CheckedChanged(object sender, EventArgs e)
	{
		this.comboBox_VR.Enabled = this.checkBox_VR.Checked;
	}

	public bool flipXY;
}
