using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BowlingAverageTracker
{
    public sealed partial class EditNamePage : Page
    {
        public EditNameViewModel ViewModel { get; set; }

        public EditNamePage()
        {
            this.InitializeComponent();
        }

        override
        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Dto = e.Parameter as EditableNameDto;
        }
    }
}
