namespace CoreGameplay.Matches
{
    public interface IMatchable
    {
        public int GetID();
        public bool CanMatch();
    }
}