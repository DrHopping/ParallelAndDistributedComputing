using Lab2.Models;

namespace Lab2.Messages;

public record ReturnBook
{
    public Book Book { get; set; }
    public Reader Reader { get; set; }
}