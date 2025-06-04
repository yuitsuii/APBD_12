﻿namespace APBD_12.Models.DTOs;

public class TripDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }

    public List<CountryDTO> Countries { get; set; } = new();
    public List<ClientDTO> Clients { get; set; } = new();
}

public class CountryDTO
{
    public string Name { get; set; } = null!;
}

public class ClientDTO
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}