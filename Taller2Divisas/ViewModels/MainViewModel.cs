using System;

using Xamarin.Forms;

namespace Taller2Divisas.ViewModels
{
    public class MainViewModel : ContentPage
    {
        public MainViewModel()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

