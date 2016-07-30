using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using BowlingAverageTracker.ViewModel;

namespace BowlingAverageTracker.ViewModel
{
    public abstract class BaseViewModelLocator
    {
        public BaseViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<SelectBowlerViewModel>();
            SimpleIoc.Default.Register<SelectLeagueViewModel>();
            SimpleIoc.Default.Register<SelectSeriesViewModel>();
            SimpleIoc.Default.Register<EnterScoresViewModel>();
            SimpleIoc.Default.Register<EditNameViewModel>();
        }

        public SelectBowlerViewModel SelectBowlerPage
        {
            get { return ServiceLocator.Current.GetInstance<SelectBowlerViewModel>(); }
        }
        public SelectLeagueViewModel SelectLeaguePage
        {
            get { return ServiceLocator.Current.GetInstance<SelectLeagueViewModel>(); }
        }
        public SelectSeriesViewModel SelectSeriesPage
        {
            get { return ServiceLocator.Current.GetInstance<SelectSeriesViewModel>(); }
        }
        public EnterScoresViewModel EnterScoresPage
        {
            get { return ServiceLocator.Current.GetInstance<EnterScoresViewModel>(); }
        }
        public EditNameViewModel EditNamePage
        {
            get { return ServiceLocator.Current.GetInstance<EditNameViewModel>(); }
        }
    }

    public class WindowsViewModelLocator : BaseViewModelLocator
    {
        public WindowsViewModelLocator() : base()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<INavigationService, NavigationService>();
            }
            else
            {
                var navigationService = InitNavigationService();
                SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            }
        }

        protected INavigationService InitNavigationService()
        {
            var service = new NavigationService();

            service.Configure(typeof(SelectBowlerViewModel).FullName, typeof(SelectBowlerPage));
            service.Configure(typeof(SelectLeagueViewModel).FullName, typeof(SelectLeaguePage));
            service.Configure(typeof(SelectSeriesViewModel).FullName, typeof(SelectSeriesPage));
            service.Configure(typeof(EnterScoresViewModel).FullName, typeof(EnterScoresPage));
            service.Configure(typeof(EditNameViewModel).FullName, typeof(EditNamePage));

            return service;
        }
    }
}
