using System.Linq;
using Context;

namespace CommonBehaviours
{
    public class Initer : IInit
    {
        private IInit[] _inits;


        public void Init()
        {
            _inits = SceneContext.Instance.GetMultiple<IInit>().ToArray();

            for (int i = 0; i < _inits.Length; i++)
            {
                _inits[i].Init();
            }
        }
    }
}