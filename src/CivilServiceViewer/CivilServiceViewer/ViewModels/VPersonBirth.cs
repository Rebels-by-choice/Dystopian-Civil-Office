using System;
using System.Collections.Generic;

namespace CivilServiceViewer.Models;

public partial class VPersonBirth
{
    public long? Pesel { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? BirthPlace { get; set; }

    public int? RegistryNumber { get; set; }

    public DateOnly? RegistryDate { get; set; }

    public long? PeselMatki { get; set; }

    public long? PeselOjca { get; set; }
}
