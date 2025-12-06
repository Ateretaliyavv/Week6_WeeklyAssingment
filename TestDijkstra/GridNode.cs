public struct GridNode
{
    public int X, Y;
    public override string ToString() => $"({X},{Y})";
    public override bool Equals(object obj) => obj is GridNode node && X == node.X && Y == node.Y;
    public override int GetHashCode() => System.HashCode.Combine(X, Y);
}

