using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BE.Models;

public partial class Question
{
    public int Id { get; set; }
    [DisplayName("Survey title")]
    public int? SurveyId { get; set; }
    [DisplayName("Question")]
    public string? Title { get; set; }
    [DisplayName("Correct Answer")]
    public string? CorrectAnswer { get; set; }
    
    public virtual ICollection<Option> Options { get; set; } = new List<Option>();

    public virtual Survey? Survey { get; set; }
}
