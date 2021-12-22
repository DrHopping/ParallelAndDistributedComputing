namespace Lab2.Models;

public record Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsReadRoomOnly { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int ReadTime { get; set; } = 2000;

    public void Read()
    {
        Thread.Sleep(ReadTime);
    }
}