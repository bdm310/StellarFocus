namespace ASCOM.StellarFocus
{
    partial class SetupDialogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.Connect = new System.Windows.Forms.Button();
            this.ComPortSelector = new System.Windows.Forms.ComboBox();
            this.Position = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SetPos = new System.Windows.Forms.Button();
            this.Halt = new System.Windows.Forms.Button();
            this.MovingLabel = new System.Windows.Forms.Label();
            this.Temp = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TempCoeff = new System.Windows.Forms.TextBox();
            this.MaxVel = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Accel = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.IdleOff = new System.Windows.Forms.CheckBox();
            this.PositionLabel = new System.Windows.Forms.Label();
            this.TempCompEnabledCheck = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.WriteSettings = new System.Windows.Forms.Button();
            this.HomeState = new System.Windows.Forms.Label();
            this.InvertHome = new System.Windows.Forms.CheckBox();
            this.HomePositive = new System.Windows.Forms.CheckBox();
            this.HomeVel = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Reverse = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ZeroPos = new System.Windows.Forms.Button();
            this.FindHome = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.Distance = new System.Windows.Forms.TextBox();
            this.UseSwitch = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.HomePosition = new System.Windows.Forms.TextBox();
            this.Home = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(332, 296);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(69, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(407, 296);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(69, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Comm Port";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkTrace.Location = new System.Drawing.Point(257, 300);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            this.chkTrace.CheckedChanged += new System.EventHandler(this.chkTrace_CheckedChanged);
            // 
            // Connect
            // 
            this.Connect.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Connect.Location = new System.Drawing.Point(186, 17);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(128, 21);
            this.Connect.TabIndex = 7;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // ComPortSelector
            // 
            this.ComPortSelector.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComPortSelector.FormattingEnabled = true;
            this.ComPortSelector.Location = new System.Drawing.Point(100, 17);
            this.ComPortSelector.Name = "ComPortSelector";
            this.ComPortSelector.Size = new System.Drawing.Size(80, 21);
            this.ComPortSelector.TabIndex = 8;
            this.ComPortSelector.SelectedIndexChanged += new System.EventHandler(this.ComPortSelector_SelectedIndexChanged);
            // 
            // Position
            // 
            this.Position.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Position.Location = new System.Drawing.Point(100, 17);
            this.Position.Name = "Position";
            this.Position.Size = new System.Drawing.Size(80, 20);
            this.Position.TabIndex = 9;
            this.Position.Text = "0";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Position";
            // 
            // SetPos
            // 
            this.SetPos.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SetPos.Location = new System.Drawing.Point(186, 17);
            this.SetPos.Name = "SetPos";
            this.SetPos.Size = new System.Drawing.Size(60, 20);
            this.SetPos.TabIndex = 12;
            this.SetPos.Text = "Set";
            this.SetPos.UseVisualStyleBackColor = true;
            this.SetPos.Click += new System.EventHandler(this.SetPos_Click);
            // 
            // Halt
            // 
            this.Halt.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Halt.Location = new System.Drawing.Point(252, 17);
            this.Halt.Name = "Halt";
            this.Halt.Size = new System.Drawing.Size(60, 20);
            this.Halt.TabIndex = 13;
            this.Halt.Text = "Halt";
            this.Halt.UseVisualStyleBackColor = true;
            this.Halt.Click += new System.EventHandler(this.Halt_Click);
            // 
            // MovingLabel
            // 
            this.MovingLabel.AutoSize = true;
            this.MovingLabel.Location = new System.Drawing.Point(35, 47);
            this.MovingLabel.Name = "MovingLabel";
            this.MovingLabel.Size = new System.Drawing.Size(48, 13);
            this.MovingLabel.TabIndex = 14;
            this.MovingLabel.Text = "Moving: ";
            // 
            // Temp
            // 
            this.Temp.AutoSize = true;
            this.Temp.Location = new System.Drawing.Point(10, 73);
            this.Temp.Name = "Temp";
            this.Temp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Temp.Size = new System.Drawing.Size(73, 13);
            this.Temp.TabIndex = 17;
            this.Temp.Text = "Temperature: ";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Temp Coefficient";
            // 
            // TempCoeff
            // 
            this.TempCoeff.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TempCoeff.Location = new System.Drawing.Point(100, 43);
            this.TempCoeff.Name = "TempCoeff";
            this.TempCoeff.Size = new System.Drawing.Size(80, 20);
            this.TempCoeff.TabIndex = 19;
            this.TempCoeff.TextChanged += new System.EventHandler(this.TempCoeff_TextChanged);
            // 
            // MaxVel
            // 
            this.MaxVel.Location = new System.Drawing.Point(104, 17);
            this.MaxVel.Name = "MaxVel";
            this.MaxVel.Size = new System.Drawing.Size(62, 20);
            this.MaxVel.TabIndex = 23;
            this.MaxVel.TextChanged += new System.EventHandler(this.MaxVel_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Max Velocity";
            // 
            // Accel
            // 
            this.Accel.Location = new System.Drawing.Point(104, 43);
            this.Accel.Name = "Accel";
            this.Accel.Size = new System.Drawing.Size(62, 20);
            this.Accel.TabIndex = 25;
            this.Accel.TextChanged += new System.EventHandler(this.Accel_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Max Acceleration";
            // 
            // IdleOff
            // 
            this.IdleOff.AutoSize = true;
            this.IdleOff.Location = new System.Drawing.Point(21, 71);
            this.IdleOff.Name = "IdleOff";
            this.IdleOff.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.IdleOff.Size = new System.Drawing.Size(93, 17);
            this.IdleOff.TabIndex = 26;
            this.IdleOff.Text = "Idle Power Off";
            this.IdleOff.UseVisualStyleBackColor = true;
            this.IdleOff.CheckedChanged += new System.EventHandler(this.IdleOff_CheckedChanged);
            // 
            // PositionLabel
            // 
            this.PositionLabel.AutoSize = true;
            this.PositionLabel.Location = new System.Drawing.Point(33, 21);
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(50, 13);
            this.PositionLabel.TabIndex = 27;
            this.PositionLabel.Text = "Position: ";
            // 
            // TempCompEnabledCheck
            // 
            this.TempCompEnabledCheck.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TempCompEnabledCheck.AutoSize = true;
            this.TempCompEnabledCheck.Location = new System.Drawing.Point(232, 45);
            this.TempCompEnabledCheck.Name = "TempCompEnabledCheck";
            this.TempCompEnabledCheck.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.TempCompEnabledCheck.Size = new System.Drawing.Size(65, 17);
            this.TempCompEnabledCheck.TabIndex = 28;
            this.TempCompEnabledCheck.Text = "Enabled";
            this.TempCompEnabledCheck.UseVisualStyleBackColor = true;
            this.TempCompEnabledCheck.CheckedChanged += new System.EventHandler(this.TempCompEnabledCheck_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(183, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "steps/°C";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(172, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "steps/sec";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(172, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "steps/sec²";
            // 
            // WriteSettings
            // 
            this.WriteSettings.Location = new System.Drawing.Point(61, 95);
            this.WriteSettings.Name = "WriteSettings";
            this.WriteSettings.Size = new System.Drawing.Size(114, 20);
            this.WriteSettings.TabIndex = 32;
            this.WriteSettings.Text = "Write Motor Params";
            this.WriteSettings.UseVisualStyleBackColor = true;
            this.WriteSettings.Click += new System.EventHandler(this.WriteSettings_Click);
            // 
            // HomeState
            // 
            this.HomeState.AutoSize = true;
            this.HomeState.Location = new System.Drawing.Point(7, 99);
            this.HomeState.Name = "HomeState";
            this.HomeState.Size = new System.Drawing.Size(76, 13);
            this.HomeState.TabIndex = 33;
            this.HomeState.Text = "Home Switch: ";
            // 
            // InvertHome
            // 
            this.InvertHome.AutoSize = true;
            this.InvertHome.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.InvertHome.Location = new System.Drawing.Point(18, 19);
            this.InvertHome.Name = "InvertHome";
            this.InvertHome.Size = new System.Drawing.Size(119, 17);
            this.InvertHome.TabIndex = 34;
            this.InvertHome.Text = "Invert Home Switch";
            this.InvertHome.UseVisualStyleBackColor = true;
            this.InvertHome.CheckedChanged += new System.EventHandler(this.InvertHome_CheckedChanged);
            // 
            // HomePositive
            // 
            this.HomePositive.AutoSize = true;
            this.HomePositive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.HomePositive.Location = new System.Drawing.Point(17, 45);
            this.HomePositive.Name = "HomePositive";
            this.HomePositive.Size = new System.Drawing.Size(120, 17);
            this.HomePositive.TabIndex = 35;
            this.HomePositive.Text = "Home In + Direction";
            this.HomePositive.UseVisualStyleBackColor = true;
            this.HomePositive.CheckedChanged += new System.EventHandler(this.HomePositive_CheckedChanged);
            // 
            // HomeVel
            // 
            this.HomeVel.Location = new System.Drawing.Point(100, 148);
            this.HomeVel.Name = "HomeVel";
            this.HomeVel.Size = new System.Drawing.Size(62, 20);
            this.HomeVel.TabIndex = 36;
            this.HomeVel.TextChanged += new System.EventHandler(this.HomeVel_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 151);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Home Velocity";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(168, 151);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "steps/sec";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Reverse);
            this.groupBox1.Controls.Add(this.WriteSettings);
            this.groupBox1.Controls.Add(this.IdleOff);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.MaxVel);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.Accel);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(253, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 128);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Motor Parameters";
            // 
            // Reverse
            // 
            this.Reverse.AutoSize = true;
            this.Reverse.Location = new System.Drawing.Point(137, 71);
            this.Reverse.Name = "Reverse";
            this.Reverse.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Reverse.Size = new System.Drawing.Size(66, 17);
            this.Reverse.TabIndex = 33;
            this.Reverse.Text = "Reverse";
            this.Reverse.UseVisualStyleBackColor = true;
            this.Reverse.CheckedChanged += new System.EventHandler(this.Reverse_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ZeroPos);
            this.groupBox2.Controls.Add(this.FindHome);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.Distance);
            this.groupBox2.Controls.Add(this.UseSwitch);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.HomePosition);
            this.groupBox2.Controls.Add(this.InvertHome);
            this.groupBox2.Controls.Add(this.Home);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.HomePositive);
            this.groupBox2.Controls.Add(this.HomeVel);
            this.groupBox2.Location = new System.Drawing.Point(12, 149);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(235, 181);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Homing";
            // 
            // ZeroPos
            // 
            this.ZeroPos.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ZeroPos.Location = new System.Drawing.Point(167, 42);
            this.ZeroPos.Name = "ZeroPos";
            this.ZeroPos.Size = new System.Drawing.Size(60, 20);
            this.ZeroPos.TabIndex = 46;
            this.ZeroPos.Text = "Zero Pos";
            this.ZeroPos.UseVisualStyleBackColor = true;
            this.ZeroPos.Click += new System.EventHandler(this.ZeroPos_Click);
            // 
            // FindHome
            // 
            this.FindHome.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.FindHome.Location = new System.Drawing.Point(167, 16);
            this.FindHome.Name = "FindHome";
            this.FindHome.Size = new System.Drawing.Size(60, 20);
            this.FindHome.TabIndex = 30;
            this.FindHome.Text = "Home";
            this.FindHome.UseVisualStyleBackColor = true;
            this.FindHome.Click += new System.EventHandler(this.FindHome_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(112, 98);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 45;
            this.label13.Text = "Distance";
            // 
            // Distance
            // 
            this.Distance.Location = new System.Drawing.Point(167, 95);
            this.Distance.Name = "Distance";
            this.Distance.Size = new System.Drawing.Size(62, 20);
            this.Distance.TabIndex = 44;
            this.Distance.Text = "0";
            this.Distance.TextChanged += new System.EventHandler(this.Distance_TextChanged);
            // 
            // UseSwitch
            // 
            this.UseSwitch.AutoSize = true;
            this.UseSwitch.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.UseSwitch.Location = new System.Drawing.Point(6, 97);
            this.UseSwitch.Name = "UseSwitch";
            this.UseSwitch.Size = new System.Drawing.Size(80, 17);
            this.UseSwitch.TabIndex = 43;
            this.UseSwitch.Text = "Use Switch";
            this.UseSwitch.UseVisualStyleBackColor = true;
            this.UseSwitch.CheckedChanged += new System.EventHandler(this.UseSwitch_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(168, 125);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 13);
            this.label11.TabIndex = 42;
            this.label11.Text = "steps";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(19, 125);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(75, 13);
            this.label12.TabIndex = 41;
            this.label12.Text = "Home Position";
            // 
            // HomePosition
            // 
            this.HomePosition.Location = new System.Drawing.Point(100, 122);
            this.HomePosition.Name = "HomePosition";
            this.HomePosition.Size = new System.Drawing.Size(62, 20);
            this.HomePosition.TabIndex = 40;
            this.HomePosition.TextChanged += new System.EventHandler(this.HomePosition_TextChanged);
            // 
            // Home
            // 
            this.Home.AutoSize = true;
            this.Home.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Home.Location = new System.Drawing.Point(25, 71);
            this.Home.Name = "Home";
            this.Home.Size = new System.Drawing.Size(112, 17);
            this.Home.TabIndex = 39;
            this.Home.Text = "Home on Connect";
            this.Home.UseVisualStyleBackColor = true;
            this.Home.CheckedChanged += new System.EventHandler(this.Home_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.MovingLabel);
            this.groupBox3.Controls.Add(this.PositionLabel);
            this.groupBox3.Controls.Add(this.HomeState);
            this.groupBox3.Controls.Add(this.Temp);
            this.groupBox3.Location = new System.Drawing.Point(338, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(150, 131);
            this.groupBox3.TabIndex = 42;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Status";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.TempCoeff);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.TempCompEnabledCheck);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.Halt);
            this.groupBox4.Controls.Add(this.Position);
            this.groupBox4.Controls.Add(this.SetPos);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(12, 68);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(320, 75);
            this.groupBox4.TabIndex = 43;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Basic";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.Connect);
            this.groupBox5.Controls.Add(this.ComPortSelector);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(320, 50);
            this.groupBox5.TabIndex = 44;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Communication";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(500, 342);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "   Stellar Focus Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupDialogForm_FormClosing);
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.ComboBox ComPortSelector;
        private System.Windows.Forms.TextBox Position;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SetPos;
        private System.Windows.Forms.Button Halt;
        private System.Windows.Forms.Label MovingLabel;
        private System.Windows.Forms.Label Temp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TempCoeff;
        private System.Windows.Forms.TextBox MaxVel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Accel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox IdleOff;
        private System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.CheckBox TempCompEnabledCheck;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button WriteSettings;
        private System.Windows.Forms.Label HomeState;
        private System.Windows.Forms.CheckBox InvertHome;
        private System.Windows.Forms.CheckBox HomePositive;
        private System.Windows.Forms.TextBox HomeVel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button FindHome;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox Distance;
        private System.Windows.Forms.CheckBox UseSwitch;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox HomePosition;
        private System.Windows.Forms.CheckBox Home;
        private System.Windows.Forms.Button ZeroPos;
        private System.Windows.Forms.CheckBox Reverse;
    }
}