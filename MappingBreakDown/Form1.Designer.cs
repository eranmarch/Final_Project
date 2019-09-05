namespace MappingBreakDown
{
    partial class MappingBreakDown
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
            this.label2 = new System.Windows.Forms.Label();
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
            this.searchBox = new System.Windows.Forms.TextBox();
            this.LSBOpts = new System.Windows.Forms.ComboBox();
            this.MSBOpts = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label10 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.PathToFileLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.IsCommentCheckBox = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ErrorMessage = new System.Windows.Forms.Label();
            this.hierarchicalGridView1 = new HierarchicalGrid.HierarchicalGridView();
            this.Group = new HierarchicalGrid.HierarchicalGridColumn();
            this.NameColumn = new HierarchicalGrid.HierarchicalGridColumn();
            this.Address = new HierarchicalGrid.HierarchicalGridColumn();
            this.MAIS = new HierarchicalGrid.HierarchicalGridColumn();
            this.LSB = new HierarchicalGrid.HierarchicalGridColumn();
            this.MSB = new HierarchicalGrid.HierarchicalGridColumn();
            this.Type = new HierarchicalGrid.HierarchicalGridColumn();
            this.FPGA = new HierarchicalGrid.HierarchicalGridColumn();
            this.Init = new HierarchicalGrid.HierarchicalGridColumn();
            this.Comment = new HierarchicalGrid.HierarchicalGridColumn();
            this.IsValid = new HierarchicalGrid.HierarchicalGridColumn();
            this.Reason = new HierarchicalGrid.HierarchicalGridColumn();
            this.Index = new HierarchicalGrid.HierarchicalGridColumn();
            this.SecondaryIndex = new HierarchicalGrid.HierarchicalGridColumn();
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
            this.ExpandAll = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.UnCommentButton = new System.Windows.Forms.Button();
            this.CommentButton = new System.Windows.Forms.Button();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hierarchicalGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Register name";
            // 
            // RegNameText
            // 
            this.RegNameText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.RegNameText.Location = new System.Drawing.Point(120, 15);
            this.RegNameText.Name = "RegNameText";
            this.RegNameText.Size = new System.Drawing.Size(166, 24);
            this.RegNameText.TabIndex = 6;
            this.RegNameText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RegNameText_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 86);
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
            this.MAISOpts.Location = new System.Drawing.Point(60, 83);
            this.MAISOpts.Name = "MAISOpts";
            this.MAISOpts.Size = new System.Drawing.Size(77, 26);
            this.MAISOpts.TabIndex = 10;
            // 
            // lable5
            // 
            this.lable5.AutoSize = true;
            this.lable5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lable5.Location = new System.Drawing.Point(10, 18);
            this.lable5.Name = "lable5";
            this.lable5.Size = new System.Drawing.Size(36, 18);
            this.lable5.TabIndex = 11;
            this.lable5.Text = "LSB";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(10, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 18);
            this.label6.TabIndex = 13;
            this.label6.Text = "MSB";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 122);
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
            this.FPGAOpts.Location = new System.Drawing.Point(60, 150);
            this.FPGAOpts.Name = "FPGAOpts";
            this.FPGAOpts.Size = new System.Drawing.Size(77, 26);
            this.FPGAOpts.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 152);
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
            this.TypeOpts.Location = new System.Drawing.Point(60, 118);
            this.TypeOpts.Name = "TypeOpts";
            this.TypeOpts.Size = new System.Drawing.Size(77, 26);
            this.TypeOpts.TabIndex = 20;
            this.TypeOpts.DataSourceChanged += new System.EventHandler(this.TypeOpts_DataSourceChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(10, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 18);
            this.label7.TabIndex = 19;
            this.label7.Text = "Type";
            // 
            // CommentText
            // 
            this.CommentText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.CommentText.Location = new System.Drawing.Point(120, 55);
            this.CommentText.Name = "CommentText";
            this.CommentText.Size = new System.Drawing.Size(166, 24);
            this.CommentText.TabIndex = 21;
            this.CommentText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CommentText_KeyUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(14, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 18);
            this.label8.TabIndex = 22;
            this.label8.Text = "Comment";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(14, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 18);
            this.label9.TabIndex = 24;
            this.label9.Text = "Init Value";
            // 
            // InitText
            // 
            this.InitText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.InitText.Location = new System.Drawing.Point(120, 90);
            this.InitText.Name = "InitText";
            this.InitText.Size = new System.Drawing.Size(166, 24);
            this.InitText.TabIndex = 23;
            this.InitText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.InitText_KeyUp);
            // 
            // InsertButton
            // 
            this.InsertButton.Location = new System.Drawing.Point(377, 290);
            this.InsertButton.Name = "InsertButton";
            this.InsertButton.Size = new System.Drawing.Size(81, 22);
            this.InsertButton.TabIndex = 27;
            this.InsertButton.Text = "Insert";
            this.InsertButton.UseVisualStyleBackColor = true;
            this.InsertButton.Click += new System.EventHandler(this.InsertButton_Click);
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(464, 291);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(81, 21);
            this.Delete.TabIndex = 26;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Load
            // 
            this.Load.Location = new System.Drawing.Point(551, 291);
            this.Load.Name = "Load";
            this.Load.Size = new System.Drawing.Size(75, 21);
            this.Load.TabIndex = 25;
            this.Load.Text = "Load";
            this.Load.UseVisualStyleBackColor = true;
            this.Load.Click += new System.EventHandler(this.Load_Click);
            // 
            // NewGroupText
            // 
            this.NewGroupText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.NewGroupText.Location = new System.Drawing.Point(120, 122);
            this.NewGroupText.Name = "NewGroupText";
            this.NewGroupText.Size = new System.Drawing.Size(166, 24);
            this.NewGroupText.TabIndex = 30;
            this.NewGroupText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NewGroupText_KeyUp);
            // 
            // searchBox
            // 
            this.searchBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.searchBox.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.searchBox.Location = new System.Drawing.Point(83, 290);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(200, 24);
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
            this.LSBOpts.Location = new System.Drawing.Point(60, 15);
            this.LSBOpts.Name = "LSBOpts";
            this.LSBOpts.Size = new System.Drawing.Size(77, 26);
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
            this.MSBOpts.Location = new System.Drawing.Point(60, 50);
            this.MSBOpts.Name = "MSBOpts";
            this.MSBOpts.Size = new System.Drawing.Size(77, 26);
            this.MSBOpts.TabIndex = 36;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(9, 291);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 18);
            this.label10.TabIndex = 39;
            this.label10.Text = "Search";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.panel3.Location = new System.Drawing.Point(327, 36);
            this.panel3.Margin = new System.Windows.Forms.Padding(1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(148, 192);
            this.panel3.TabIndex = 40;
            // 
            // PathToFileLabel
            // 
            this.PathToFileLabel.AutoSize = true;
            this.PathToFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PathToFileLabel.Location = new System.Drawing.Point(13, 9);
            this.PathToFileLabel.Name = "PathToFileLabel";
            this.PathToFileLabel.Size = new System.Drawing.Size(42, 18);
            this.PathToFileLabel.TabIndex = 45;
            this.PathToFileLabel.Text = "Path:";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.IsCommentCheckBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.CommentText);
            this.panel2.Controls.Add(this.RegNameText);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.InitText);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.NewGroupText);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Location = new System.Drawing.Point(12, 36);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(300, 192);
            this.panel2.TabIndex = 41;
            // 
            // IsCommentCheckBox
            // 
            this.IsCommentCheckBox.AutoSize = true;
            this.IsCommentCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.IsCommentCheckBox.Location = new System.Drawing.Point(170, 159);
            this.IsCommentCheckBox.Name = "IsCommentCheckBox";
            this.IsCommentCheckBox.Size = new System.Drawing.Size(115, 21);
            this.IsCommentCheckBox.TabIndex = 31;
            this.IsCommentCheckBox.Text = "Add Comment";
            this.IsCommentCheckBox.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1099, 24);
            this.menuStrip1.TabIndex = 44;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.About_MenuButtonClick);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.ExpandAll);
            this.panel5.Controls.Add(this.ErrorMessage);
            this.panel5.Controls.Add(this.PathToFileLabel);
            this.panel5.Controls.Add(this.panel2);
            this.panel5.Controls.Add(this.ClearButton);
            this.panel5.Controls.Add(this.panel3);
            this.panel5.Controls.Add(this.UnCommentButton);
            this.panel5.Controls.Add(this.Load);
            this.panel5.Controls.Add(this.InsertButton);
            this.panel5.Controls.Add(this.Delete);
            this.panel5.Controls.Add(this.hierarchicalGridView1);
            this.panel5.Controls.Add(this.CommentButton);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.searchBox);
            this.panel5.Location = new System.Drawing.Point(15, 27);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1075, 560);
            this.panel5.TabIndex = 45;
            // 
            // ErrorMessage
            // 
            this.ErrorMessage.AutoSize = true;
            this.ErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorMessage.Location = new System.Drawing.Point(9, 254);
            this.ErrorMessage.Name = "ErrorMessage";
            this.ErrorMessage.Size = new System.Drawing.Size(73, 18);
            this.ErrorMessage.TabIndex = 44;
            this.ErrorMessage.Text = "Message:";
            // 
            // hierarchicalGridView1
            // 
            this.hierarchicalGridView1.AllowUserToAddRows = false;
            this.hierarchicalGridView1.AllowUserToDeleteRows = false;
            this.hierarchicalGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Group,
            this.NameColumn,
            this.Address,
            this.MAIS,
            this.LSB,
            this.MSB,
            this.Type,
            this.FPGA,
            this.Init,
            this.Comment,
            this.IsValid,
            this.Reason,
            this.Index,
            this.SecondaryIndex});
            this.hierarchicalGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.hierarchicalGridView1.ImageList = null;
            this.hierarchicalGridView1.Location = new System.Drawing.Point(1, 318);
            this.hierarchicalGridView1.Margin = new System.Windows.Forms.Padding(1);
            this.hierarchicalGridView1.Name = "hierarchicalGridView1";
            this.hierarchicalGridView1.Size = new System.Drawing.Size(1073, 240);
            this.hierarchicalGridView1.TabIndex = 43;
            this.hierarchicalGridView1.SelectionChanged += new System.EventHandler(this.HierarchialGridView1_SelectionChanged);
            this.hierarchicalGridView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HierarchialGridView1_KeyUp);
            // 
            // Group
            // 
            this.Group.DataPropertyName = "Group";
            this.Group.DefaultNodeImage = null;
            this.Group.HeaderText = "Group";
            this.Group.Name = "Group";
            this.Group.ReadOnly = true;
            // 
            // NameColumn
            // 
            this.NameColumn.DataPropertyName = "Name";
            this.NameColumn.DefaultNodeImage = null;
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            // 
            // Address
            // 
            this.Address.DataPropertyName = "Address";
            this.Address.DefaultNodeImage = null;
            this.Address.HeaderText = "Address";
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            // 
            // MAIS
            // 
            this.MAIS.DataPropertyName = "MAIS";
            this.MAIS.DefaultNodeImage = null;
            this.MAIS.HeaderText = "MAIS";
            this.MAIS.Name = "MAIS";
            this.MAIS.ReadOnly = true;
            // 
            // LSB
            // 
            this.LSB.DataPropertyName = "LSB";
            this.LSB.DefaultNodeImage = null;
            this.LSB.HeaderText = "LSB";
            this.LSB.Name = "LSB";
            this.LSB.ReadOnly = true;
            // 
            // MSB
            // 
            this.MSB.DataPropertyName = "MSB";
            this.MSB.DefaultNodeImage = null;
            this.MSB.HeaderText = "MSB";
            this.MSB.Name = "MSB";
            this.MSB.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "Type";
            this.Type.DefaultNodeImage = null;
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // FPGA
            // 
            this.FPGA.DataPropertyName = "FPGA";
            this.FPGA.DefaultNodeImage = null;
            this.FPGA.HeaderText = "FPGA";
            this.FPGA.Name = "FPGA";
            this.FPGA.ReadOnly = true;
            // 
            // Init
            // 
            this.Init.DataPropertyName = "Init";
            this.Init.DefaultNodeImage = null;
            this.Init.HeaderText = "Init";
            this.Init.Name = "Init";
            this.Init.ReadOnly = true;
            // 
            // Comment
            // 
            this.Comment.DataPropertyName = "Comment";
            this.Comment.DefaultNodeImage = null;
            this.Comment.HeaderText = "Comment";
            this.Comment.Name = "Comment";
            this.Comment.ReadOnly = true;
            // 
            // IsValid
            // 
            this.IsValid.DataPropertyName = "IsValid";
            this.IsValid.DefaultNodeImage = null;
            this.IsValid.HeaderText = "IsValid";
            this.IsValid.Name = "IsValid";
            this.IsValid.ReadOnly = true;
            this.IsValid.Visible = false;
            // 
            // Reason
            // 
            this.Reason.DataPropertyName = "Reason";
            this.Reason.DefaultNodeImage = null;
            this.Reason.HeaderText = "Reason";
            this.Reason.Name = "Reason";
            this.Reason.ReadOnly = true;
            this.Reason.Visible = false;
            // 
            // Index
            // 
            this.Index.DataPropertyName = "Index";
            this.Index.DefaultNodeImage = null;
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Visible = false;
            // 
            // SecondaryIndex
            // 
            this.SecondaryIndex.DataPropertyName = "SecondaryIndex";
            this.SecondaryIndex.DefaultNodeImage = null;
            this.SecondaryIndex.HeaderText = "SecondaryIndex";
            this.SecondaryIndex.Name = "SecondaryIndex";
            this.SecondaryIndex.ReadOnly = true;
            this.SecondaryIndex.Visible = false;
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
            // ExpandAll
            // 
            this.ExpandAll.Image = global::MappingBreakDown.Properties.Resources.expand;
            this.ExpandAll.Location = new System.Drawing.Point(347, 290);
            this.ExpandAll.Name = "ExpandAll";
            this.ExpandAll.Size = new System.Drawing.Size(23, 22);
            this.ExpandAll.TabIndex = 46;
            this.ExpandAll.UseVisualStyleBackColor = true;
            this.ExpandAll.Click += new System.EventHandler(this.ExpandAll_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Image = global::MappingBreakDown.Properties.Resources.clear;
            this.ClearButton.Location = new System.Drawing.Point(1041, 291);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(22, 21);
            this.ClearButton.TabIndex = 28;
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.Clear_Click);
            // 
            // UnCommentButton
            // 
            this.UnCommentButton.Image = global::MappingBreakDown.Properties.Resources.uncomment;
            this.UnCommentButton.Location = new System.Drawing.Point(318, 290);
            this.UnCommentButton.Name = "UnCommentButton";
            this.UnCommentButton.Size = new System.Drawing.Size(23, 22);
            this.UnCommentButton.TabIndex = 42;
            this.UnCommentButton.UseVisualStyleBackColor = true;
            this.UnCommentButton.Click += new System.EventHandler(this.UnCommentButton_Click);
            // 
            // CommentButton
            // 
            this.CommentButton.Image = global::MappingBreakDown.Properties.Resources.comment;
            this.CommentButton.Location = new System.Drawing.Point(289, 290);
            this.CommentButton.Name = "CommentButton";
            this.CommentButton.Size = new System.Drawing.Size(23, 22);
            this.CommentButton.TabIndex = 41;
            this.CommentButton.UseVisualStyleBackColor = true;
            this.CommentButton.Click += new System.EventHandler(this.CommentButton_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::MappingBreakDown.Properties.Resources.save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::MappingBreakDown.Properties.Resources.open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::MappingBreakDown.Properties.Resources.close;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Image = global::MappingBreakDown.Properties.Resources.help;
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.manualToolStripMenuItem.Text = "Manual";
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.help_MenuButtonClick);
            // 
            // MappingBreakDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1099, 601);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1115, 640);
            this.Name = "MappingBreakDown";
            this.Text = "MappingBreakDown";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MappingBreakDown_FormClosing);
            this.Shown += new System.EventHandler(this.MappingBreakDown_Shown);
            this.Resize += new System.EventHandler(this.frm_sizeChanged);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hierarchicalGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
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
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.ComboBox LSBOpts;
        private System.Windows.Forms.ComboBox MSBOpts;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Panel panel3;
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
        private System.Windows.Forms.Button CommentButton;
        private System.Windows.Forms.Button UnCommentButton;
        private HierarchicalGrid.HierarchicalGridView hierarchicalGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label PathToFileLabel;
        private System.Windows.Forms.Label ErrorMessage;
        private HierarchicalGrid.HierarchicalGridColumn Group;
        private HierarchicalGrid.HierarchicalGridColumn NameColumn;
        private HierarchicalGrid.HierarchicalGridColumn Address;
        private HierarchicalGrid.HierarchicalGridColumn MAIS;
        private HierarchicalGrid.HierarchicalGridColumn LSB;
        private HierarchicalGrid.HierarchicalGridColumn MSB;
        private HierarchicalGrid.HierarchicalGridColumn Type;
        private HierarchicalGrid.HierarchicalGridColumn FPGA;
        private HierarchicalGrid.HierarchicalGridColumn Init;
        private HierarchicalGrid.HierarchicalGridColumn Comment;
        private HierarchicalGrid.HierarchicalGridColumn IsValid;
        private HierarchicalGrid.HierarchicalGridColumn Reason;
        private HierarchicalGrid.HierarchicalGridColumn Index;
        private HierarchicalGrid.HierarchicalGridColumn SecondaryIndex;
        private System.Windows.Forms.CheckBox IsCommentCheckBox;
        private System.Windows.Forms.Button ExpandAll;
        //private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

