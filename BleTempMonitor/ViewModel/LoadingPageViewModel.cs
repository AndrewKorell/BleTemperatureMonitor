using AsyncAwaitBestPractices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.ViewModel
{
    public partial class LoadingPageViewModel : BaseViewModel
    {
        public LoadingPageViewModel()
        {
            GotoMainPage().SafeFireAndForget();
        }

        private async Task GotoMainPage()
        {
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
    }
}
