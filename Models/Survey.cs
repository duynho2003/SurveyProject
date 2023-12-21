using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BE.Models;

public partial class Survey
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? UserType { get; set; }

    public string? Form { get; set; }

    public int? UserPost { get; set; }
    [DisplayName("Start Date")]
    public DateTime? CreatedAt { get; set; }
    [DisplayName("End Date")]
    public DateTime? EndAt { get; set; }

    public bool? IsVisible { get; set; }

    public virtual ICollection<Award> Awards { get; set; } = new List<Award>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
