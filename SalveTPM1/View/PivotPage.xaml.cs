using SalveTPM1.Common;
using SalveTPM1.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace SalveTPM1
{
    public sealed partial class PivotPage : Page
    {
        private const string FirstGroupName = "Status TPM";
        private const string SecondGroupName = "Detalhes";

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        private ViewModel.PivotPageViewModel pivotViewModel { get; set; }



        public PivotPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

      
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-1");
            this.DefaultViewModel[FirstGroupName] = sampleDataGroup;
        }

     
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Adds an item to the list when the app bar button is clicked.
        /// </summary>
        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {

            this.pivotViewModel.abrirTelaCadastroAtualizaCommand.Execute(new Model.Mulher());

           
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(View.CadastroMulher), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }


        }

        /// <summary>
        /// Loads the content for the second pivot item when it is scrolled into view.
        /// </summary>
        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-2");
            this.DefaultViewModel[SecondGroupName] = sampleDataGroup;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.pivotViewModel = new ViewModel.PivotPageViewModel();
            this.listaMulheresListView.DataContext = pivotViewModel;
            this.nomePrincipal.DataContext = pivotViewModel;
            this.velocimetro.DataContext = pivotViewModel;


            BitmapImage bitmapImageDica = new BitmapImage();

            if (pivotViewModel.mulherPrincipal != null)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(this.BaseUri, pivotViewModel.mulherPrincipal.caminhoImagemTpm);
                this.imagePrincipal.ImageSource = bitmapImage;

                bitmapImageDica.UriSource = new Uri(this.BaseUri, pivotViewModel.mulherPrincipal.caminhoImagemFraseTpm);
                this.imageDicas.ImageSource = bitmapImageDica;
            }
            else
            {
                bitmapImageDica.UriSource = new Uri(this.BaseUri, "ms-appx:///Assets/fraseMulherNaoAdicionada.png");
                this.imageDicas.ImageSource = bitmapImageDica;
            }
           
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            ViewModel.NotificacaoUtil notificacao = new ViewModel.NotificacaoUtil();
          //  notificacao.showNotificacao("Notificacao do Fernando", this.BaseUri + "../Assets/medidor.jpg", "Notificao1");

           DateTime data = DateTime.Now.AddSeconds(5);
        //    DateTime data = DateTime.ParseExact("02/07/2015", "dd/MM/yyyy", null);


           notificacao.agendarNotificacao("Notificacao Agendada", "ms-appx:///Assets/velocimetroSemTpmPequeno.png","1", data);
            

        }

       
        #region NavigationHelper registration

        #endregion
    }
}
