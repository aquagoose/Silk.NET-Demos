using System.Numerics;
using Silk.NET.Input;

namespace ShadowMap.Shared;

public static class Input
{
    private static IInputContext _context;
    
    private static HashSet<Key> _keysDown = new HashSet<Key>();
    private static HashSet<Key> _newKeys = new HashSet<Key>();

    public static bool KeyDown(Key key) => _keysDown.Contains(key);

    public static bool KeyPressed(Key key) => _newKeys.Contains(key);

    public static Vector2 MousePosition { get; private set; }

    public static bool MouseVisible
    {
        get => _context.Mice[0].Cursor.CursorMode == CursorMode.Normal;
        set => _context.Mice[0].Cursor.CursorMode = value ? CursorMode.Normal : CursorMode.Disabled;
    }

    internal static void Initialize(IInputContext context)
    {
        _context = context;
        
        foreach (IKeyboard kb in context.Keyboards)
        {
            kb.KeyDown += KbOnKeyDown;
            kb.KeyUp += KbOnKeyUp;
        }
    }

    internal static void Update()
    {
        _newKeys.Clear();
        MousePosition = _context.Mice[0].Position;
    }

    private static void KbOnKeyUp(IKeyboard arg1, Key arg2, int arg3)
    {
        _keysDown.Remove(arg2);
        _newKeys.Remove(arg2);
    }

    private static void KbOnKeyDown(IKeyboard arg1, Key arg2, int arg3)
    {
        _keysDown.Add(arg2);
        _newKeys.Add(arg2);
    }
}