using Innouvous.Utils;
using Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HTTPUtil
{
    class MainWindowViewModel : Innouvous.Utils.Merged45.MVVM45.ViewModel
    {
        private HttpClient httpClient = new HttpClient();
        private Window window;
        private CollectionViewSource cvsMethods;

        public MainWindowViewModel(Window window)
        {
            this.window = window;

            cvsMethods = new CollectionViewSource();
            cvsMethods.Source = Enum.GetValues((typeof(Method)));
        }

        public enum Method
        {
            GET,
            POST
        }


        public ICollectionView Methods
        {
            get
            {
                return cvsMethods.View;
            }
        }


        public string Url
        {
            get
            {
                return Get<string>();
            }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public string ResponseData
        {
            get
            {
                return Get<string>();
            }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public Method SelectedMethod
        {
            get
            {
                return Get<Method>();
            }
            set
            {
                Set(value);
                RaisePropertyChanged();
            }
        }

        public ICommand QueryCommand
        {
            get
            {
                return new CommandHelper(Go);
            }
        }

        private async void Go()
        {
            try
            {
                if (string.IsNullOrEmpty(Url))
                    return;
                else if (!Url.Contains("://"))
                    Url = "http://" + Url;

                HttpResponseMessage response;

                switch (SelectedMethod)
                {
                    case Method.GET:
                        response = await httpClient.GetAsync(Url);
                        ResponseData = await response.Content.ReadAsStringAsync();
                        break;
                    case Method.POST:
                        response = await httpClient.PostAsync(Url, null);
                        ResponseData = await response.Content.ReadAsStringAsync();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e);
            }
        }
    }
}
