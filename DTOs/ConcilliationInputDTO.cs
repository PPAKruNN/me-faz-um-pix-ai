using System.ComponentModel.DataAnnotations;

namespace FazUmPix.DTOs;

public class ConcilliationInputDTO
{
    [Url]
    public required string File { get; set; }
    [Url]
    public required string Postback { get; set; }
    public required DateTime Date { get; set; }
}