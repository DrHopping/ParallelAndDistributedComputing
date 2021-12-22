using Akka.Actor;
using Lab2.Actors;
using Lab2.Helpers;
using Lab2.Messages;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            using var system = ActorSystem.Create("LibrarySystem");
            var library = system.ActorOf<LibraryActor>("Library");
            var readers = ReaderFactory.GetReaders();
            var readerRefs = readers.Select(r => system.ActorOf(Props.Create(() => new ReaderActor(r.Id, r.Name, library)))).ToList();
            Demo(readerRefs);
            Console.ReadLine();
        }

        private static void Demo(List<IActorRef> readers)
        {
            readers[0].Tell(new DemoRequestBook{BookId = 1});
            readers[0].Tell(new DemoRequestBook{BookId = 2});
            readers[1].Tell(new DemoRequestBook{BookId = 1});
            readers[2].Tell(new DemoRequestBook{BookId = 2});
        }
    }
}