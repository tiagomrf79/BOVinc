﻿namespace DummyAPI.DTOs;

public class AnimalDto
{
    public int Id { get; set; }
    public string? RegistrationId { get; set; }
    public string? Name { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int GenderId { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int BreedId { get; set; }
    public string Breed { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string Category { get; set; } = string.Empty;
}
