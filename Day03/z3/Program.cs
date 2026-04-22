using System;
using System.Collections.Generic;

//только жанр
abstract class MusicAlbum
{
    public string Title;
    public string Artist;
    public int ReleaseYear;
    public int TrackCount;

    public MusicAlbum(string title, string artist, int year, int tracks)
    {
        Title = title;
        Artist = artist;
        ReleaseYear = year;
        TrackCount = tracks;
    }
}

sealed class RockAlbum : MusicAlbum
{
    public RockAlbum(string t, string a, int y, int c) : base(t, a, y, c) { }
}

sealed class PopAlbum : MusicAlbum
{
    public PopAlbum(string t, string a, int y, int c) : base(t, a, y, c) { }
}

// 3. Класс-библиотека (хранит массив и ищет данные)
class MusicLibrary
{
    public MusicAlbum[] MyAlbums;

    public MusicLibrary(MusicAlbum[] albums)
    {
        MyAlbums = albums;
    }

    public MusicAlbum GetNewestAlbum()
    {
        MusicAlbum newest = MyAlbums[0]; 

        foreach (MusicAlbum item in MyAlbums)
        {
            if (item.ReleaseYear > newest.ReleaseYear) 
            {
                newest = item; 
            }
        }
        return newest;
    }

    public List<MusicAlbum> GetAlbumsByArtist(string artistName)
    {
        List<MusicAlbum> result = new List<MusicAlbum>();

        foreach (MusicAlbum item in MyAlbums)
        {
            if (item.Artist == artistName) 
            {
                result.Add(item); 
            }
        }
        return result;
    }
}

class Program
{
    static void Main()
    {
        MusicAlbum[] data = new MusicAlbum[]
        {
            new RockAlbum("Album1", "Queen", 1975, 12),
            new PopAlbum("Album2", "Madonna", 1984, 10),
            new RockAlbum("Album3", "Queen", 1991, 14)
        };

        MusicLibrary myLib = new MusicLibrary(data);

        MusicAlbum last = myLib.GetNewestAlbum();
        Console.WriteLine("Новейший: " + last.Title);

        var queenAlbums = myLib.GetAlbumsByArtist("Queen");
        foreach (var a in queenAlbums)
        {
            Console.WriteLine("Найдено: " + a.Title);
        }
    }
}