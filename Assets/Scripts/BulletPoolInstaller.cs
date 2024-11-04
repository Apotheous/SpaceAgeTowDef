using Zenject;

public class BulletPoolInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PoolStorage>().FromComponentInHierarchy().AsSingle();

        // EnemyAttackState Factory için baðlantý kur
        Container.BindFactory<Enemy, EnemyStateMachine, EnemyAttackState, EnemyAttackStateFactory>()
                 .AsSingle();
    }
}
