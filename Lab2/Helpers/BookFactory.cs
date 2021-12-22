using Lab2.Models;

namespace Lab2.Helpers;

public class BookFactory
{
    public static List<Book> GetBooks()
    {
        return new List<Book>
        {
            new(){Name = "Warrior Of Heaven", IsReadRoomOnly = false, Id = 1},
            new(){Name = "Baker Without Glory", IsReadRoomOnly = false, Id = 2},
            new(){Name = "Friends Of Yesterday", IsReadRoomOnly = false, Id = 3},
            new(){Name = "Creators Without Hope", IsReadRoomOnly = true, Id = 4},
            new(){Name = "Assassins And Traitors", IsReadRoomOnly = true, Id = 5},
        };
    }
}