using System;

public partial class User
{
    public string Username;
    public int FollowersCount;
    public int PostsCount;
    public DateTime LastActiveDate;

    public User(string name, int followers, int posts, DateTime lastActive)
    {
        Username = name;
        FollowersCount = followers;
        PostsCount = posts;
        LastActiveDate = lastActive;
    }
}
public partial class User
{
    public void Show()
    {
        Console.WriteLine("User: " + Username);
    }
}

class SocialNetwork
{
    public User[] AllUsers;

    public SocialNetwork(User[] users)
    {
        AllUsers = users;
    }

    public List<User> GetMostPopularUsers(int minFollowers)
    {
        List<User> result = new List<User>();
        foreach (User u in AllUsers)
        {
            if (u.FollowersCount > minFollowers)
            {
                result.Add(u);
            }
        }
        return result;
    }

    public List<User> GetInactiveUsers(int days)
    {
        List<User> result = new List<User>();
        DateTime limitDate = DateTime.Now.AddDays(-days);

        foreach (User u in AllUsers)
        {
            if (u.LastActiveDate < limitDate)
            {
                result.Add(u);
            }
        }
        return result;
    }
}
class Program
{
    static void Main()
    {
        User[] users = new User[]
        {
            new User("Паша", 1000, 10, DateTime.Now.AddDays(-5)),
            new User("Ангелина", 100, 5, DateTime.Now.AddDays(-50)),
            new User("Алиса", 5000, 20, DateTime.Now)
        };

        SocialNetwork net = new SocialNetwork(users);

        var popular = net.GetMostPopularUsers(500);

        var inactive = net.GetInactiveUsers(30);

        Console.WriteLine("Неактивные: " + inactive[0].Username);
        Console.WriteLine("Популятрные: " + popular[0].Username);
    }
}