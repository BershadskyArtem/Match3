namespace CoreGameplay.Kinds
{
    public enum ExplosionKind
    {
        //Usual
        Horizontal = 1,
        Vertical = 2,
        Bomb = 3,
        Color = 4,
        
        //Combined with DestroyColor
        ColorHorizontal = 5,
        ColorVertical = 6,
        ColorBomb = 7,
        ColorColor = 8,
        
        //Combined with Bomb
        BombVertical = 8,
        BombHorizontal = 9,
        BombBomb = 10,
        
        //Combined with Horizontal
        HorizontalVertical = 11,
        HorizontalHorizontal = 12,
        
        //Combined with Vertical
        VerticalVertical = 13
    }
}