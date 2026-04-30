public class FontManager
{
    private static FontManager _instance;

    private readonly HashSet<string> _loadedFonts;

    private FontManager()
    {
        _loadedFonts = new HashSet<string>();
        Console.WriteLine("[Система]: FontManager инициализирован.");
    }

    public static FontManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new FontManager();
        }
        return _instance;
    }

    public void LoadFont(string fontName)
    {
        if (!_loadedFonts.Contains(fontName))
        {
            _loadedFonts.Add(fontName);
            Console.WriteLine($"Шрифт '{fontName}' успешно загружен в память.");
        }
        else
        {
            Console.WriteLine($"Шрифт '{fontName}' уже доступен.");
        }
    }

    public string GetFont(string fontName)
    {
        if (_loadedFonts.Contains(fontName))
        {
            return $"[Объект шрифта: {fontName}]";
        }
        return $"[Ошибка]: Шрифт '{fontName}' не найден. Загрузите его перед использованием.";
    }
}
