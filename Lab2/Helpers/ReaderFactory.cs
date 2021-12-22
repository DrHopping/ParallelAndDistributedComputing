using Lab2.Models;

namespace Lab2.Helpers;

public class ReaderFactory
{
    public static List<Reader> GetReaders()
    {
        return new List<Reader>
        {
            new(){Name = "Jamaal", Id = 1},
            new(){Name = "Denis", Id = 2},
            new(){Name = "Camille", Id = 3},
            new(){Name = "Geoffrey", Id = 4},
            new(){Name = "Kye", Id = 5},
        };
    }
}