using BowlingAverageTracker.Dto;

namespace BowlingAverageTracker.ViewModel
{
    public class ColorPickerViewModel : BaseViewModel
    {
        public enum PickType { BACKGROUND, TEXT }
        public PickType Type { get; set; }
        public ColorSettings Colors { get; set; }

        public ColorPickerViewModel(PickType type)
        {
            Type = type;
            Colors = new ColorSettings();
        }
    }
}
