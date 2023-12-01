namespace ChatClient.Forms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Login = new GroupBox();
            textBoxToken = new TextBox();
            buttonEnter = new Button();
            labelPass = new Label();
            labelLog = new Label();
            textBoxPass = new TextBox();
            textBoxLogin = new TextBox();
            Message = new GroupBox();
            btDisconnect = new Button();
            btConnect = new Button();
            listBoxMessages = new ListBox();
            groupBox3 = new GroupBox();
            Users = new GroupBox();
            listBoxUsers = new ListBox();
            groupBox5 = new GroupBox();
            Login.SuspendLayout();
            Message.SuspendLayout();
            Users.SuspendLayout();
            SuspendLayout();
            // 
            // Login
            // 
            Login.Controls.Add(textBoxToken);
            Login.Controls.Add(buttonEnter);
            Login.Controls.Add(labelPass);
            Login.Controls.Add(labelLog);
            Login.Controls.Add(textBoxPass);
            Login.Controls.Add(textBoxLogin);
            Login.Location = new Point(12, 12);
            Login.Name = "Login";
            Login.Size = new Size(310, 780);
            Login.TabIndex = 0;
            Login.TabStop = false;
            Login.Text = "Вход - Регистрация";
            // 
            // textBoxToken
            // 
            textBoxToken.Location = new Point(6, 162);
            textBoxToken.Multiline = true;
            textBoxToken.Name = "textBoxToken";
            textBoxToken.Size = new Size(298, 612);
            textBoxToken.TabIndex = 5;
            // 
            // buttonEnter
            // 
            buttonEnter.Location = new Point(6, 133);
            buttonEnter.Name = "buttonEnter";
            buttonEnter.Size = new Size(298, 23);
            buttonEnter.TabIndex = 4;
            buttonEnter.Text = "Войти";
            buttonEnter.UseVisualStyleBackColor = true;
            buttonEnter.Click += buttonEnter_Click;
            // 
            // labelPass
            // 
            labelPass.AutoSize = true;
            labelPass.Location = new Point(10, 86);
            labelPass.Name = "labelPass";
            labelPass.Size = new Size(49, 15);
            labelPass.TabIndex = 3;
            labelPass.Text = "Пароль";
            // 
            // labelLog
            // 
            labelLog.AutoSize = true;
            labelLog.Location = new Point(6, 28);
            labelLog.Name = "labelLog";
            labelLog.Size = new Size(41, 15);
            labelLog.TabIndex = 2;
            labelLog.Text = "Логин";
            // 
            // textBoxPass
            // 
            textBoxPass.Location = new Point(6, 104);
            textBoxPass.Name = "textBoxPass";
            textBoxPass.Size = new Size(298, 23);
            textBoxPass.TabIndex = 1;
            textBoxPass.Text = "123qwe!@#";
            // 
            // textBoxLogin
            // 
            textBoxLogin.Location = new Point(6, 46);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(298, 23);
            textBoxLogin.TabIndex = 0;
            textBoxLogin.Text = "user1@sorodich.com";
            // 
            // Message
            // 
            Message.Controls.Add(btDisconnect);
            Message.Controls.Add(btConnect);
            Message.Controls.Add(listBoxMessages);
            Message.Controls.Add(groupBox3);
            Message.Location = new Point(328, 12);
            Message.Name = "Message";
            Message.Size = new Size(307, 780);
            Message.TabIndex = 1;
            Message.TabStop = false;
            Message.Text = "Чат";
            // 
            // btDisconnect
            // 
            btDisconnect.Location = new Point(10, 86);
            btDisconnect.Name = "btDisconnect";
            btDisconnect.Size = new Size(291, 23);
            btDisconnect.TabIndex = 6;
            btDisconnect.Text = "Отключится";
            btDisconnect.UseVisualStyleBackColor = true;
            // 
            // btConnect
            // 
            btConnect.Location = new Point(6, 46);
            btConnect.Name = "btConnect";
            btConnect.Size = new Size(295, 23);
            btConnect.TabIndex = 5;
            btConnect.Text = "Подключится";
            btConnect.UseVisualStyleBackColor = true;
            btConnect.Click += buttonLoad_Click;
            // 
            // listBoxMessages
            // 
            listBoxMessages.FormattingEnabled = true;
            listBoxMessages.ItemHeight = 15;
            listBoxMessages.Location = new Point(6, 162);
            listBoxMessages.Name = "listBoxMessages";
            listBoxMessages.Size = new Size(295, 604);
            listBoxMessages.TabIndex = 2;
            // 
            // groupBox3
            // 
            groupBox3.Location = new Point(311, 36);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(327, 780);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "groupBox3";
            // 
            // Users
            // 
            Users.Controls.Add(listBoxUsers);
            Users.Controls.Add(groupBox5);
            Users.Location = new Point(653, 22);
            Users.Name = "Users";
            Users.Size = new Size(307, 780);
            Users.TabIndex = 2;
            Users.TabStop = false;
            Users.Text = "Пользователи";
            // 
            // listBoxUsers
            // 
            listBoxUsers.FormattingEnabled = true;
            listBoxUsers.ItemHeight = 15;
            listBoxUsers.Location = new Point(6, 152);
            listBoxUsers.Name = "listBoxUsers";
            listBoxUsers.Size = new Size(295, 604);
            listBoxUsers.TabIndex = 3;
            // 
            // groupBox5
            // 
            groupBox5.Location = new Point(311, 36);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(327, 780);
            groupBox5.TabIndex = 1;
            groupBox5.TabStop = false;
            groupBox5.Text = "groupBox5";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(975, 804);
            Controls.Add(Users);
            Controls.Add(Message);
            Controls.Add(Login);
            Name = "Form1";
            Text = "Form1";
            Login.ResumeLayout(false);
            Login.PerformLayout();
            Message.ResumeLayout(false);
            Users.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox Login;
        private GroupBox Message;
        private GroupBox groupBox3;
        private GroupBox Users;
        private GroupBox groupBox5;
        private TextBox textBoxToken;
        private Button buttonEnter;
        private Label labelPass;
        private Label labelLog;
        private TextBox textBoxPass;
        private TextBox textBoxLogin;
        private ListBox listBoxMessages;
        private Button btDisconnect;
        private Button btConnect;
        private ListBox listBoxUsers;
    }
}