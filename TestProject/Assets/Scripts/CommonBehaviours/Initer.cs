namespace CommonBehaviours
{
    public class Initer : IInit
    {
        private IInit[] _inits;
        
        public Initer(params IInit[] inits)
        {
            _inits = inits;
        }

        public void Init()
        {
            for (int i = 0; i < _inits.Length; i++)
            {
                _inits[i].Init();
            }
        }
    }
}
