using Code.Logic.Monster;
using Zenject;

public class MonstersToDragSelectorInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindMonsterToDragSelector();
    }
    
    private void BindMonsterToDragSelector()
    {
        Container.BindInterfacesAndSelfTo<MonsterToDragSelector>()
            .AsSingle();
    }
}