using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using BowlingAverageTracker.Dto;

namespace BowlingAverageTracker.ViewModel
{
    public class EditNameViewModel : BaseViewModel
    {
        public EditableNameDto Dto { get; set; }

        public EditNameViewModel()
        {
        }
    }
}
