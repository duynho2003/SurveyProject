using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BE.Models;

public partial class Faq
{
    public int Id { get; set; }
    [DisplayName("Question")]
    public string? Title { get; set; }
    
    public string? Answer { get; set; }
}
