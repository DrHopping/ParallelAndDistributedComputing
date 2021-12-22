using Lab2.Models;

namespace Lab2.Messages;

public record WaitingForBook
{
    public Reader Reader { get; set; }
    public Book Book { get; set; }
}