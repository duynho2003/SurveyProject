using System;
using System.Collections.Generic;

namespace BE.Models;

public partial class Option
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Answer { get; set; }

    public int? QuestionId { get; set; }

    public virtual Question? Question { get; set; }

}
