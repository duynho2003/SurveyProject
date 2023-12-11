using Microsoft.Extensions.Options;

namespace BE.Models
{
    public class QuestionOptionsViewModel
    {
        public Question? Question { get; set; }
        public Option? Options { get; set; } = new Option();
    }
}
