using ChatClient.Forms.Token;
using Microsoft.AspNetCore.SignalR.Client;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.Connections.Client;

namespace ChatClient.Forms
{
    public partial class Form1 : Form
    {
        private HubConnection? _connection;
        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonEnter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxLogin.Text))
            {
                listBoxMessages.Items.Add("Имя пользователя не указано!");
                return;
            }
            if (string.IsNullOrEmpty(textBoxPass.Text))
            {
                listBoxMessages.Items.Add("Пароль не указан!");
                return;
            }
            try
            {
                var jsonToken = await TokenLoader.RequestToken(textBoxLogin.Text, textBoxPass.Text, AppData.ServerTocenUrl);

                var token = (JwtSecurityToken)new JwtSecurityTokenHandler().ReadToken(jsonToken.AccessToken);

                var encodeJwt = new JwtSecurityTokenHandler().WriteToken(token);

                if (jsonToken == null)
                {
                    listBoxMessages.Items.Add("Не удается получить токен от IdentityServer!");
                    return;
                }
                textBoxToken.Text = jsonToken.AccessToken;


                //btConnect.Enabled = true;
                //btConnect.Visible = true;
                //btDisconnect.Enabled = false;
                //btDisconnect.Visible = true;
            }
            catch (Exception ex)
            {
                listBoxMessages.Items.Add(ex.Message);
            }
        }

        private async void buttonLoad_Click(object sender, EventArgs e)
        {
            await ConnectToChatAsync();
        }

        private async Task ConnectToChatAsync()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(AppData.ServerChatUrl, options =>
                {
                    options.Headers.Add("Authorization", $"{textBoxToken.Text}");
                })
                .WithAutomaticReconnect()
                .Build();

            _connection.On<IEnumerable<string>>("UpdateUserAsync", users =>
            {
                listBoxUsers.Items.Clear();
                foreach (var user in users)
                {
                    listBoxUsers.Items.Add(user);
                }
            });

            _connection.On<string, string>("SendMessageAsync", (user, text) =>
            {
                var item = $"{user} --- {text}";
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate { listBoxMessages.Items.Add(item); }));
                }
            });
            try
            {
                await _connection!.StartAsync();
                btConnect.Enabled = false;
                btDisconnect.Enabled = true;
            }
            catch (Exception ex)
            {
                listBoxMessages.Items.Add(ex.Message);
            }
        }
    }
}