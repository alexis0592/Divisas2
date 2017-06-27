using System;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Taller2Divisas.Models;

namespace Taller2Divisas.Services
{
    public class UtilServices
    {
        public UtilServices()
        {
        }

		public async Task<Response> CheckConnectivity()
		{

			if (!CrossConnectivity.Current.IsConnected)
			{
				return new Response
				{
					IsSuccess = false,
					Message = "Please turn on your internet settings",
				};
			}

			var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");

			if (!isReachable)
			{
				return new Response
				{
					IsSuccess = false,
					Message = "Check your internet connection",
				};
			}

			return new Response
			{
				IsSuccess = true,
				Message = "Ok",
			};
		}

		public async Task ShowMessage(string title, string message)
		{
			await App.Current.MainPage.DisplayAlert(title, message, "Accept");
		}

		public async Task<bool> ShowConfirm(string title, string message)
		{
			return await App.Current.MainPage.DisplayAlert(title, message, "Yes", "No");
		}
    }
}
