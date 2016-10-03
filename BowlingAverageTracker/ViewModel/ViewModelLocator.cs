using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

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
        public StatisticsViewModel StatisticsPage
        {
            get { return ServiceLocator.Current.GetInstance<StatisticsViewModel>(); }
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
            service.Configure(typeof(StatisticsViewModel).FullName, typeof(StatisticsPage));
            service.Configure(typeof(OptionsPage).FullName, typeof(OptionsPage));
            service.Configure(typeof(BackupPage).FullName, typeof(BackupPage));
            service.Configure(typeof(ColorsViewModel).FullName, typeof(ColorsPage));
            service.Configure(typeof(ColorPickerPage).FullName, typeof(ColorPickerPage));

            return service;
        }
    }
}
