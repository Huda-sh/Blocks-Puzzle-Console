namespace BlocksConsole.GameLogic.models
{
    internal class Position : ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position Add(Position other)
        {
            return new Position(X + other.X, Y + other.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Position other && X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public object Clone()
        {
            return new Position(X, Y);
        }
    }
}
