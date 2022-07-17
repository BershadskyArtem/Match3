namespace CoreGameplay.Nodes.Base
{
    public interface IMatchable
    {
        public bool CanMatch();
        public int GetId();
        public void Resolve();
    }
}