using System.Collections.ObjectModel;
using MobileApp.Models;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using MobileApp.API.Posts;

namespace MobileApp;

public partial class Feed : ContentPage
{
    private ObservableCollection<PostResponseModel> _posts;

    public Feed()
    {
        //InitializeComponent();

        //_posts = new ObservableCollection<PostResponseModel>();
        //PostsListView.ItemsSource = _posts;

        //LoadPosts();
    }

    private async void LoadPosts()
    {
        //try
        //{
        //    _posts.Clear();

        //    var posts = await PostService.LoadPostsAsync();

        //    foreach (var post in posts)
        //    {
        //        _posts.Add(post);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    // Handle error
        //}
    }
}