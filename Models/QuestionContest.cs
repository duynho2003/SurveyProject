using System;
using System.Collections.Generic;

namespace BE.Models;

public partial class QuestionContest
{
    public int Id { get; set; }

    public int? ContestId { get; set; }

    public string QuestionText { get; set; } = null!;

    public string AnswerOptions { get; set; } = null!;

    public string CorrectAnswer { get; set; } = null!;

    public virtual Contest? Contest { get; set; }
}
