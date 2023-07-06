namespace Rummikub.Shared;

public readonly record struct Tile(Suit Suit, byte Value)
{
    public const byte Joker = 255;
    
    public bool IsJoker => Value is Joker;

    public static IComparer<Tile> TileByValueComparer => Comparer<Tile>.Create((l, r) => l.Value.CompareTo(r.Value));
}