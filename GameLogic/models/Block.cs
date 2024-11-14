namespace BlocksConsole.GameLogic.models
{
    internal class Block : ICloneable
    {
        public Position RelativePosition { get; }

        public Block(Position pos)
        {
            RelativePosition = pos;
        }

        public Position GetAbsolutePosition(Position origin)
        {
            return origin.Add(RelativePosition);
        }

        public object Clone()
        {
            return new Block((Position)RelativePosition.Clone());
        }
    }
}
