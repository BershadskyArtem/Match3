namespace CoreGameplay.Nodes.Base
{
    public interface IDestroyable
    {
        public void Destroy();
        public bool CanDestroy();
    }
}