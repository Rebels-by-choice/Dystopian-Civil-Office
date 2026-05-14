using System;
using System.Collections.Generic;

namespace CivilServiceViewer.Models;

public partial class VPersonAddressStatus
{
    public long? Pesel { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? BirthPlace { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public int? HouseNumber { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? StatusZycia { get; set; }
}
