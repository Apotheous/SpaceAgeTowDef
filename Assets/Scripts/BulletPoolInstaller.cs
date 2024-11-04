using Zenject;

public class BulletPoolInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PoolStorage>().FromComponentInHierarchy().AsSingle();

    }
}
