using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.AspNetCore.SignalR.Client;
using EnvShare.Models.Chat;
using System.Windows.Input;
using EnvShare.DeviceStore.Authentication;
using EnvShare.API;
namespace EnvShare.Views.Home.Chat;

public partial class DMChat : ContentView, INotifyPropertyChanged
{
    HubConnection hubConnection;
    public ObservableCollection<string> Messages { get; private set; }
    private bool _isChatVisible;
    private ChatUser _selectedUser;

    public ObservableCollection<ChatUser> Users { get; } = new ObservableCollection<ChatUser>();
    public DMChat()
	{
        InitializeComponent();

        Messages = new ObservableCollection<string>();
        BindingContext = this;
       
        hubConnection = new HubConnectionBuilder()
            .WithUrl(URLS.DMEndPoint,options =>
            {
                options.AccessTokenProvider = async () => await TokenStorage.GetTokenAsync();

            })
            .Build();
        StartConnectionAsync();
        hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            var newMessage = $"{user}: {message}";
            Messages.Add(newMessage);
        });

        
        LoadChats();

    }
    public bool IsChatVisible
    {
        get => _isChatVisible;
        set
        {
            if (_isChatVisible != value)
            {
                _isChatVisible = value;
                OnPropertyChanged(nameof(IsChatVisible));
            }
        }
    }
    public ChatUser SelectedUser
    {
        get => _selectedUser;
        set
        {
            if (_selectedUser != value)
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                Messages.Clear();
                IsChatVisible = value != null;
               
            }
        }
    }


    private async void StartConnectionAsync()
    {
        try
        {
            await hubConnection.StartAsync();
        }
        catch (Exception ex)
        {
           
        }
    }

    private async void OnSendClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(MessageEntry.Text))
        {
            await hubConnection.InvokeAsync("SendMessageToUser",
                SelectedUser.userid, MessageEntry.Text);
            Messages.Add($"Me: {MessageEntry.Text}");
            MessageEntry.Text = string.Empty;
        }
    }
    private void LoadChats()
    {
        //Replace with API call
        
        Users.Add(new ChatUser { userid = "6",username="element117" });
        Users.Add(new ChatUser { userid = "48",username="element116" });
        Users.Add(new ChatUser { userid = "2", username = "element115" });

    }
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;
}