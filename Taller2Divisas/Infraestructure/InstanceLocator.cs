using System;
using Taller2Divisas.ViewModels;
using Xamarin.Forms;

namespace Taller2Divisas.Infraestructure
{
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}

