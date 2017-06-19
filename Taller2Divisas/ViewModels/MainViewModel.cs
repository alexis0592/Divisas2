using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Taller2Divisas.Models;
using System.Linq;
using Xamarin.Forms;

namespace Taller2Divisas.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ExchangeRates exchangeRates;
        private RateNames rateNames;
        private string message;
        private double sourceRate;
        private double targetRate;
        #endregion


        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public ObservableCollection<Rate> Rates
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled"));
                }
            }
        }

        public decimal Amount
        {
            get;
            set;
        }

        public double SourceRate
        {
            get{
                return sourceRate;
            }
            set{
                if(sourceRate != value){
                    sourceRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SourceRate"));
                }
            }
        }

        public double TargetRate
        {
            get{
                return targetRate;
            }
            set{
                if(targetRate != value){
                    targetRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TargetRate"));
                }
            }
        }

        public string Message
        {
            get{
                return message;
            }
            set{
                if(message != value){
                    message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
                }
            }
        }

        #endregion

        #region Constructors
        public MainViewModel()
        {
            Rates = new ObservableCollection<Rate>();

            IsEnabled = false;
            GetRates();
        }


        #endregion


        #region Methods

        private void LoadRates()
        {

            var rateValues = new List<RateValue>();
            var type = typeof(Rates);
            var properties = type.GetRuntimeFields();

            foreach(var property in properties){
                var code = property.Name.Substring(1, 3);
                rateValues.Add(new RateValue
                {
                    Code = code,
                    TaxRate = (double)property.GetValue(exchangeRates.Rates)
                });
            };

            //names
            var rateNamesList = new List<RateName>();
            type = typeof(RateNames);
            properties = type.GetRuntimeFields();

            foreach(var property in properties){
                var code = property.Name.Substring(1, 3);
                rateNamesList.Add(new RateName
                {
                    Code = code,
                    Name = (string)property.GetValue(rateNames)
                });
            }

            Rates.Clear();

            List<Rate> rateList = joinList(rateValues, rateNamesList);

            foreach (Rate property in rateList)
            {
                Rates.Add(new Rate
                {
                    Code = property.Code,
                    TaxRate = (double)property.TaxRate,
                    Name = property.FullName
                });
            }
        }


        private async void GetRates()
        {
            try
            {
                var client = new HttpClient();
                var client2 = new HttpClient();

                client.BaseAddress = new Uri("https://openexchangerates.org");
                var url = "/api/latest.json?app_id=f490efbcd52d48ee98fd62cf33c47b9e";
                var responseRates = await client.GetAsync(url);

                client2.BaseAddress = new Uri("https://gist.githubusercontent.com");
                var urlNames = "/picodotdev/88512f73b61bc11a2da4/raw/9407514be22a2f1d569e75d6b5a58bd5f0ebbad8";
                var responseNames = await client2.GetAsync(urlNames);

                if (!responseRates.IsSuccessStatusCode || !responseNames.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Error", responseRates.StatusCode.ToString(), "Aceptar");
                    IsRunning = false;
                    IsEnabled = false;
                    return;
                }

                var result1 = await responseRates.Content.ReadAsStringAsync();
                exchangeRates = JsonConvert.DeserializeObject<ExchangeRates>(result1);

                var result2 = await responseNames.Content.ReadAsStringAsync();
                rateNames = JsonConvert.DeserializeObject<RateNames>(result2);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                IsRunning = false;
                IsEnabled = false;
                return;
            }

            LoadRates();
            IsRunning = false;
            IsEnabled = true;

        }
        #endregion

        #region Commands
        public ICommand ConvertMoneyCommand
        {
            get{
                return new RelayCommand(ConvertMoney);
            }
        }

	    private async void ConvertMoney()
			{
				if (Amount <= 0)
				{
					await App.Current.MainPage.DisplayAlert(
						"Error",
						"Debes ingresar un valor a convertir",
						"Aceptar");
					return;
				}

				if (SourceRate == 0)
				{
					await App.Current.MainPage.DisplayAlert(
						"Error",
						"Debes seleccionar la moneda origen",
						"Aceptar");
					return;
				}

				if (TargetRate == 0)
				{
					await App.Current.MainPage.DisplayAlert(
						"Error",
						"Debes seleccionar la moneda destino",
						"Aceptar");
					return;
				}

				decimal amountConverted = Amount / (decimal)SourceRate * (decimal)TargetRate;

				Message = string.Format("{0:N2} = {1:N2}", Amount, amountConverted);
			}

        public ICommand ChangeCommand
        {
            get{
                return new RelayCommand(Change);
            }
        }

        private void Change()
        {
            var aux = SourceRate;
            SourceRate = TargetRate;
            TargetRate = aux;
        }

        private List<Rate> joinList(List<RateValue> l1, List<RateName> l2){

            var rateList = new List<Rate>();

            for (var i = 0; i < l1.Count(); i++){
                var auxCodeValue = l1[i].Code;
                for (var j = 0; j < l2.Count(); j++){
                    if(auxCodeValue == l2[j].Code){
                        rateList.Add(new Rate
                        {
                            Code = auxCodeValue,
                            TaxRate = (double)l1[i].TaxRate,
                            Name = l2[j].Name
                        });
                        break;
                    }
                }
            }

            return rateList;
        }

        #endregion

    }
}

