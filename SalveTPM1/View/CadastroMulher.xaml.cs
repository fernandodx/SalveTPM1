﻿using SalveTPM1.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SalveTPM1.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CadastroMulher : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public CadastroMulher()
        {
            this.InitializeComponent();

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

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            this.DataContext = (ViewModel.PivotPageViewModel)e.Parameter;


            Model.Mulher mulher = ((ViewModel.PivotPageViewModel)this.DataContext).mulherSelecionado;

            if (mulher.ID != null)
            {
                this.dataUltimaMestruacao.Date = mulher.dataUltimaMestruacao;
                if (mulher.isPrincipal)
                {
                    this.isApresentarStatus.IsOn = true;
                }
                else
                {
                    this.isApresentarStatus.IsOn = false;
                }

            }
            else
            {
                this.isApresentarStatus.IsOn = true;
            }

        }
        #region NavigationHelper registration

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ComboLigacao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            if (this.DataContext != null)
            {
                Model.Mulher mulher = ((ViewModel.PivotPageViewModel)this.DataContext).mulherSelecionado;
                mulher.ligacao = ((ComboBoxItem)combo.SelectedItem).Content.ToString(); 
            }
         
        }

        private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {

            DatePicker dataPicker = (DatePicker)sender;
            Model.Mulher mulher = ((ViewModel.PivotPageViewModel)this.DataContext).mulherSelecionado;
            mulher.dataUltimaMestruacao = dataPicker.Date.DateTime;
           
        }

        private void isApresentarStatus_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {

                Model.Mulher mulher = ((ViewModel.PivotPageViewModel)this.DataContext).mulherSelecionado;

                if (toggleSwitch.IsOn == true)
                {
                    mulher.isPrincipal = true;
                }
                else
                {
                    mulher.isPrincipal = false;
                }
            }

        }

    }
}
