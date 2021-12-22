using Akka.Actor;

namespace Lab2.Models;

public record WaitingReader
{
    public IActorRef Source { get; set; }
    public Reader Reader { get; set; }
    public Book Book { get; set; }
}