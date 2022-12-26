using UniMob;

namespace ViewModel
{
    public abstract class ViewModelBase<T>
    {
        public abstract void Compose(Lifetime lifetime ,T panel);
    }
}
