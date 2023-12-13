namespace BE.Models
{
    public class OptionViewModel
    {
        public Option Option { get; set; }

        public OptionViewModel(Option option)
        {
            Option = option;
        }
    }
}
