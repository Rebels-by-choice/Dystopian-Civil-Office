using System;
using System.Collections.Generic;

namespace CivilServiceViewer.Models;

public partial class VPersonDeath
{
    public long? Pesel { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DeathDate { get; set; }

    public string? DeathPlace { get; set; }

    public int? RegistryNumber { get; set; }

    public DateOnly? RegistryDate { get; set; }

    public string? CauseOfDeath { get; set; }
}
