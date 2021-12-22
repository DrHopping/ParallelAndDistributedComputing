using Akka.Actor;
using Lab2.Messages;
using Lab2.Models;

namespace Lab2.Actors;

public class ReaderActor: ReceiveActor
{
    public Reader Reader { get; set; }
    private IActorRef Library { get; set; }

    public ReaderActor(int readerId, string readerName, IActorRef library)
    {
        Reader = new Reader {Name = readerName, Id = readerId};
        Library = library;

        Receive<ReceiveBook>(r =>
        {
            Console.WriteLine($"Reader {Reader.Id} reading {r.Book.Id}");
            r.Book.Read();
            Console.WriteLine($"Reader {Reader.Id} returned {r.Book.Id}");
            Sender.Tell(new ReturnBook{Book = r.Book, Reader = Reader});
        });

        Receive<WaitingForBook>(r =>
        {
            Console.WriteLine($"Reader {Reader.Id} is waiting for book {r.Book.Id}");
        });

        Receive<DemoRequestBook>(r => RequestBook(r.BookId));
    }
    
    private void RequestBook(int bookId)
    {
        Console.WriteLine($"Reader {Reader.Id} requested {bookId}");
        Library.Tell(new RequestBook{BookId = bookId, IsReadRoom = true, Reader = Reader});
    }
}