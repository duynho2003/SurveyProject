using System;
using System.Collections.Generic;

namespace BE.Models;

public partial class FilledSurvey
{
    public byte[] CreatedAt { get; set; } = null!;

    public int? UserId { get; set; }

    public int? SurveyId { get; set; }

    public int? OptionId { get; set; }

    public virtual Option? Option { get; set; }

    public virtual Survey? Survey { get; set; }

    public virtual User? User { get; set; }
}
