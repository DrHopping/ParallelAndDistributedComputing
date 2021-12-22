using Akka.Actor;
using Lab2.Helpers;
using Lab2.Messages;
using Lab2.Models;

namespace Lab2.Actors;

public class LibraryActor : ReceiveActor
{
    private List<WaitingReader> WaitingReaders { get; set; } = new();
    private Dictionary<int, Book> Books { get; set; }

    public LibraryActor()
    {
        Books = BookFactory.GetBooks().ToDictionary(x => x.Id);

        Receive<RequestBook>(r =>
        {
            var book = Books[r.BookId];
            if (book.IsAvailable)
            {
                book.IsAvailable = false;
                Sender.Tell(new ReceiveBook{Book = book});
            }
            else
            {
                WaitingReaders.Add(new WaitingReader {Book = book, Reader = r.Reader, Source = Sender});
                Sender.Tell(new WaitingForBook {Book = book, Reader = r.Reader});
            }
        });

        Receive<ReturnBook>(r =>
        {
            var waitingReader = WaitingReaders.FirstOrDefault(x => x.Book == r.Book);
            if (waitingReader == null)
            {
                r.Book.IsAvailable = true;
                return;
            }
            WaitingReaders.Remove(waitingReader);
            waitingReader.Source.Tell(new ReceiveBook{Book = r.Book});
        });

    }
}