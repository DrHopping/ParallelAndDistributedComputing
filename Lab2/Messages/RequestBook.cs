using Lab2.Models;

namespace Lab2.Messages;

public record RequestBook
{
    public int BookId { get; set; }
    public bool IsReadRoom { get; set; }
    public Reader Reader { get; set; }
}