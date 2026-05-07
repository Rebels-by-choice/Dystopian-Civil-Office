using System;
using System.Collections.Generic;

namespace CivilServiceViewer.Models;

public partial class VMarriage
{
    public int? RegistryNumber { get; set; }

    public DateOnly? MarriageDate { get; set; }

    public string? MarriagePlace { get; set; }

    public DateOnly? RegistryDate { get; set; }

    public long? Spouse1Pesel { get; set; }

    public long? Spouse2Pesel { get; set; }
}
