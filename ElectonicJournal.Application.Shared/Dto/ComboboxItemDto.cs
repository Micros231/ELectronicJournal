namespace ElectronicJournal.Application.Dto
{
    public class ComboboxItemDto
    {
        public string Value { get; set; }
        public string DisplayText { get; set; }
        public bool IsSelected { get; set; }
        public ComboboxItemDto() { }
        public ComboboxItemDto(string value, string displayText)
        {
            Value = value;
            DisplayText = displayText;
        }
    }
}
