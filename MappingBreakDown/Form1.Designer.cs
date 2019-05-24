﻿namespace MappingBreakDown
{
    partial class MappingPackageAutomation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MappingPackageAutomation));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PathToFile = new System.Windows.Forms.TextBox();
            this.OpenButton = new System.Windows.Forms.Button();
            this.SaveAsButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.RegNameText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.MAISOpts = new System.Windows.Forms.ComboBox();
            this.lable5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FPGAOpts = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TypeOpts = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CommentText = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.InitText = new System.Windows.Forms.TextBox();
            this.InsertButton = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.Load = new System.Windows.Forms.Button();
            this.NewGroupText = new System.Windows.Forms.TextBox();
            this.AddGroupButton = new System.Windows.Forms.Button();
            this.RegGroupOpts = new System.Windows.Forms.ComboBox();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.LSBOpts = new System.Windows.Forms.ComboBox();
            this.MSBOpts = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ClearButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ErrorMessage = new System.Windows.Forms.RichTextBox();
            this.treeGridView1 = new AdvancedDataGridView.TreeGridView();
            this.Registers = new AdvancedDataGridView.TreeGridColumn();
            this.AddressColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAISColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LSBColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MSBColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FPGAColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InitColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CommentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecondaryIndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Path to file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(32, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Register name";
            // 
            // PathToFile
            // 
            this.PathToFile.Location = new System.Drawing.Point(97, 12);
            this.PathToFile.Name = "PathToFile";
            this.PathToFile.ReadOnly = true;
            this.PathToFile.Size = new System.Drawing.Size(337, 20);
            this.PathToFile.TabIndex = 2;
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(453, 12);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(75, 23);
            this.OpenButton.TabIndex = 3;
            this.OpenButton.Text = "Open";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.Location = new System.Drawing.Point(546, 12);
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(75, 23);
            this.SaveAsButton.TabIndex = 4;
            this.SaveAsButton.Text = "Save As";
            this.SaveAsButton.UseVisualStyleBackColor = true;
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(639, 12);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // RegNameText
            // 
            this.RegNameText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.RegNameText.Location = new System.Drawing.Point(138, 95);
            this.RegNameText.Name = "RegNameText";
            this.RegNameText.Size = new System.Drawing.Size(166, 24);
            this.RegNameText.TabIndex = 6;
            this.RegNameText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RegNameText_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "MAIS";
            // 
            // MAISOpts
            // 
            this.MAISOpts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MAISOpts.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.MAISOpts.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "4"});
            this.MAISOpts.Location = new System.Drawing.Point(60, 73);
            this.MAISOpts.Name = "MAISOpts";
            this.MAISOpts.Size = new System.Drawing.Size(77, 26);
            this.MAISOpts.TabIndex = 10;
            // 
            // lable5
            // 
            this.lable5.AutoSize = true;
            this.lable5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lable5.Location = new System.Drawing.Point(17, 12);
            this.lable5.Name = "lable5";
            this.lable5.Size = new System.Drawing.Size(36, 18);
            this.lable5.TabIndex = 11;
            this.lable5.Text = "LSB";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 18);
            this.label6.TabIndex = 13;
            this.label6.Text = "MSB";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(30, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 18);
            this.label5.TabIndex = 15;
            this.label5.Text = "Register group";
            // 
            // FPGAOpts
            // 
            this.FPGAOpts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FPGAOpts.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FPGAOpts.FormattingEnabled = true;
            this.FPGAOpts.Items.AddRange(new object[] {
            "G",
            "D",
            "A",
            "B",
            "C",
            "ABC",
            "ABCG"});
            this.FPGAOpts.Location = new System.Drawing.Point(60, 135);
            this.FPGAOpts.Name = "FPGAOpts";
            this.FPGAOpts.Size = new System.Drawing.Size(77, 26);
            this.FPGAOpts.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 18);
            this.label3.TabIndex = 17;
            this.label3.Text = "FPGA";
            // 
            // TypeOpts
            // 
            this.TypeOpts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeOpts.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.TypeOpts.FormattingEnabled = true;
            this.TypeOpts.Items.AddRange(new object[] {
            "RD",
            "WR",
            "RD_WR",
            "FIELD"});
            this.TypeOpts.Location = new System.Drawing.Point(60, 105);
            this.TypeOpts.Name = "TypeOpts";
            this.TypeOpts.Size = new System.Drawing.Size(77, 26);
            this.TypeOpts.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(17, 105);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 18);
            this.label7.TabIndex = 19;
            this.label7.Text = "Type";
            // 
            // CommentText
            // 
            this.CommentText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.CommentText.Location = new System.Drawing.Point(434, 97);
            this.CommentText.Name = "CommentText";
            this.CommentText.Size = new System.Drawing.Size(254, 24);
            this.CommentText.TabIndex = 21;
            this.CommentText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CommentText_KeyUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(356, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 18);
            this.label8.TabIndex = 22;
            this.label8.Text = "Comment";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(393, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 18);
            this.label9.TabIndex = 24;
            this.label9.Text = "Init";
            // 
            // InitText
            // 
            this.InitText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.InitText.Location = new System.Drawing.Point(434, 132);
            this.InitText.Name = "InitText";
            this.InitText.Size = new System.Drawing.Size(254, 24);
            this.InitText.TabIndex = 23;
            this.InitText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.InitText_KeyUp);
            // 
            // InsertButton
            // 
            this.InsertButton.Location = new System.Drawing.Point(307, 13);
            this.InsertButton.Name = "InsertButton";
            this.InsertButton.Size = new System.Drawing.Size(81, 22);
            this.InsertButton.TabIndex = 27;
            this.InsertButton.Text = "Insert";
            this.InsertButton.UseVisualStyleBackColor = true;
            this.InsertButton.Click += new System.EventHandler(this.InsertButton_Click);
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(105, 12);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(81, 23);
            this.Delete.TabIndex = 26;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Load
            // 
            this.Load.Location = new System.Drawing.Point(10, 12);
            this.Load.Name = "Load";
            this.Load.Size = new System.Drawing.Size(75, 23);
            this.Load.TabIndex = 25;
            this.Load.Text = "Load";
            this.Load.UseVisualStyleBackColor = true;
            this.Load.Click += new System.EventHandler(this.Load_Click);
            // 
            // NewGroupText
            // 
            this.NewGroupText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.NewGroupText.Location = new System.Drawing.Point(35, 185);
            this.NewGroupText.Name = "NewGroupText";
            this.NewGroupText.Size = new System.Drawing.Size(176, 24);
            this.NewGroupText.TabIndex = 30;
            this.NewGroupText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NewGroupText_KeyUp);
            // 
            // AddGroupButton
            // 
            this.AddGroupButton.Location = new System.Drawing.Point(227, 185);
            this.AddGroupButton.Name = "AddGroupButton";
            this.AddGroupButton.Size = new System.Drawing.Size(75, 23);
            this.AddGroupButton.TabIndex = 31;
            this.AddGroupButton.Text = "Add Group";
            this.AddGroupButton.UseVisualStyleBackColor = true;
            this.AddGroupButton.Click += new System.EventHandler(this.AddGroupButton_Click);
            // 
            // RegGroupOpts
            // 
            this.RegGroupOpts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RegGroupOpts.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.RegGroupOpts.FormattingEnabled = true;
            this.RegGroupOpts.Items.AddRange(new object[] {
            ""});
            this.RegGroupOpts.Location = new System.Drawing.Point(138, 130);
            this.RegGroupOpts.Name = "RegGroupOpts";
            this.RegGroupOpts.Size = new System.Drawing.Size(166, 26);
            this.RegGroupOpts.TabIndex = 32;
            // 
            // searchBox
            // 
            this.searchBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.searchBox.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.searchBox.Location = new System.Drawing.Point(92, 226);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(213, 24);
            this.searchBox.TabIndex = 33;
            this.searchBox.TextChanged += new System.EventHandler(this.TextBox2_TextChanged);
            // 
            // LSBOpts
            // 
            this.LSBOpts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LSBOpts.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.LSBOpts.FormattingEnabled = true;
            this.LSBOpts.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.LSBOpts.Location = new System.Drawing.Point(60, 10);
            this.LSBOpts.Name = "LSBOpts";
            this.LSBOpts.Size = new System.Drawing.Size(75, 26);
            this.LSBOpts.TabIndex = 35;
            // 
            // MSBOpts
            // 
            this.MSBOpts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MSBOpts.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.MSBOpts.FormattingEnabled = true;
            this.MSBOpts.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.MSBOpts.Location = new System.Drawing.Point(60, 42);
            this.MSBOpts.Name = "MSBOpts";
            this.MSBOpts.Size = new System.Drawing.Size(75, 26);
            this.MSBOpts.TabIndex = 36;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.CloseButton);
            this.panel1.Controls.Add(this.PathToFile);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.OpenButton);
            this.panel1.Controls.Add(this.SaveAsButton);
            this.panel1.Controls.Add(this.SaveButton);
            this.panel1.Location = new System.Drawing.Point(26, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(835, 47);
            this.panel1.TabIndex = 37;
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(730, 12);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ClearButton);
            this.panel2.Controls.Add(this.InsertButton);
            this.panel2.Controls.Add(this.Load);
            this.panel2.Controls.Add(this.Delete);
            this.panel2.Location = new System.Drawing.Point(333, 198);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(400, 50);
            this.panel2.TabIndex = 38;
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(208, 12);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 23);
            this.ClearButton.TabIndex = 28;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.Clear_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(32, 228);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 18);
            this.label10.TabIndex = 39;
            this.label10.Text = "Search";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.LSBOpts);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.MAISOpts);
            this.panel3.Controls.Add(this.lable5);
            this.panel3.Controls.Add(this.MSBOpts);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.FPGAOpts);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.TypeOpts);
            this.panel3.Location = new System.Drawing.Point(757, 95);
            this.panel3.Margin = new System.Windows.Forms.Padding(1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(143, 162);
            this.panel3.TabIndex = 40;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ErrorMessage);
            this.panel4.Location = new System.Drawing.Point(925, 95);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(206, 161);
            this.panel4.TabIndex = 41;
            // 
            // ErrorMessage
            // 
            this.ErrorMessage.Enabled = false;
            this.ErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ErrorMessage.Location = new System.Drawing.Point(13, 14);
            this.ErrorMessage.Name = "ErrorMessage";
            this.ErrorMessage.Size = new System.Drawing.Size(178, 134);
            this.ErrorMessage.TabIndex = 0;
            this.ErrorMessage.Text = "";
            // 
            // treeGridView1
            // 
            this.treeGridView1.AllowUserToAddRows = false;
            this.treeGridView1.AllowUserToDeleteRows = false;
            this.treeGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.treeGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.treeGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Registers,
            this.AddressColumn,
            this.MAISColumn,
            this.LSBColumn,
            this.MSBColumn,
            this.TypeColumn,
            this.FPGAColumn,
            this.InitColumn,
            this.CommentColumn,
            this.IndexColumn,
            this.SecondaryIndexColumn});
            this.treeGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.treeGridView1.ImageList = null;
            this.treeGridView1.Location = new System.Drawing.Point(13, 312);
            this.treeGridView1.Margin = new System.Windows.Forms.Padding(1);
            this.treeGridView1.Name = "treeGridView1";
            this.treeGridView1.Size = new System.Drawing.Size(1125, 290);
            this.treeGridView1.TabIndex = 42;
            this.treeGridView1.SelectionChanged += new System.EventHandler(this.TreeGridView1_SelectionChanged);
            this.treeGridView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TreeGridView1_KeyUp);
            // 
            // Registers
            // 
            this.Registers.DataPropertyName = "Registers";
            this.Registers.DefaultNodeImage = null;
            this.Registers.HeaderText = "Registers";
            this.Registers.Name = "Registers";
            this.Registers.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Registers.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AddressColumn
            // 
            this.AddressColumn.HeaderText = "Address";
            this.AddressColumn.Name = "AddressColumn";
            this.AddressColumn.ReadOnly = true;
            this.AddressColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.AddressColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MAISColumn
            // 
            this.MAISColumn.HeaderText = "MAIS";
            this.MAISColumn.Name = "MAISColumn";
            this.MAISColumn.ReadOnly = true;
            this.MAISColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MAISColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LSBColumn
            // 
            this.LSBColumn.HeaderText = "LSB";
            this.LSBColumn.Name = "LSBColumn";
            this.LSBColumn.ReadOnly = true;
            this.LSBColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LSBColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MSBColumn
            // 
            this.MSBColumn.HeaderText = "MSB";
            this.MSBColumn.Name = "MSBColumn";
            this.MSBColumn.ReadOnly = true;
            this.MSBColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MSBColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TypeColumn
            // 
            this.TypeColumn.HeaderText = "Type";
            this.TypeColumn.Name = "TypeColumn";
            this.TypeColumn.ReadOnly = true;
            this.TypeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FPGAColumn
            // 
            this.FPGAColumn.HeaderText = "FPGA";
            this.FPGAColumn.Name = "FPGAColumn";
            this.FPGAColumn.ReadOnly = true;
            this.FPGAColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FPGAColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // InitColumn
            // 
            this.InitColumn.HeaderText = "Init";
            this.InitColumn.Name = "InitColumn";
            this.InitColumn.ReadOnly = true;
            this.InitColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.InitColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CommentColumn
            // 
            this.CommentColumn.HeaderText = "Comment";
            this.CommentColumn.Name = "CommentColumn";
            this.CommentColumn.ReadOnly = true;
            this.CommentColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CommentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // IndexColumn
            // 
            this.IndexColumn.HeaderText = "Index";
            this.IndexColumn.Name = "IndexColumn";
            this.IndexColumn.ReadOnly = true;
            this.IndexColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IndexColumn.Visible = false;
            // 
            // SecondaryIndexColumn
            // 
            this.SecondaryIndexColumn.HeaderText = "SecondaryIndex";
            this.SecondaryIndexColumn.Name = "SecondaryIndexColumn";
            this.SecondaryIndexColumn.ReadOnly = true;
            this.SecondaryIndexColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecondaryIndexColumn.Visible = false;
            // 
            // MappingPackageAutomation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1153, 442);
            this.Controls.Add(this.treeGridView1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.RegGroupOpts);
            this.Controls.Add(this.AddGroupButton);
            this.Controls.Add(this.NewGroupText);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.InitText);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.CommentText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.RegNameText);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MappingPackageAutomation";
            this.ShowIcon = false;
            this.Text = "Mapping Package Automation";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PathToFile;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button SaveAsButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TextBox RegNameText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox MAISOpts;
        private System.Windows.Forms.Label lable5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox FPGAOpts;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox TypeOpts;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox CommentText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox InitText;
        private System.Windows.Forms.Button InsertButton;
        private System.Windows.Forms.Button Delete;
        private new System.Windows.Forms.Button Load;
        private System.Windows.Forms.TextBox NewGroupText;
        private System.Windows.Forms.Button AddGroupButton;
        private System.Windows.Forms.ComboBox RegGroupOpts;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.ComboBox LSBOpts;
        private System.Windows.Forms.ComboBox MSBOpts;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button ClearButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RichTextBox ErrorMessage;
        private AdvancedDataGridView.TreeGridView treeGridView1;
        private AdvancedDataGridView.TreeGridColumn Registers;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddressColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAISColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LSBColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MSBColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FPGAColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn InitColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CommentColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecondaryIndexColumn;
        //private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

