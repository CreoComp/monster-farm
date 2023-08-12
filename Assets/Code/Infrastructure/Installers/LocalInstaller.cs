using Code.Infrastructure.Services.StaticDataService;
using Code.Logic;
using Code.Logic.Monster;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class LocalInstaller : MonoInstaller
    {
        [SerializeField, FormerlySerializedAs("bordersSpawnTransform")]
        private BordersSpawnTransform _bordersSpawnTransform;

        public override void InstallBindings()
        {
            BindStaticDataService();
            BindGameFactory();
            BindBordersSpawnTransform();
            BindMonsterToDragSelector();
        }

        private void BindStaticDataService()
        {
            Container
                .BindInterfacesAndSelfTo<StaticDataService>()
                .AsSingle();
        }

        private void BindGameFactory()
        {
            Container
                .BindInterfacesAndSelfTo<GameFactory.GameFactory>()
                .AsSingle();
        }

        private void BindBordersSpawnTransform()
        {
            Container
                .Bind<BordersSpawnTransform>()
                .FromInstance(_bordersSpawnTransform)
                .AsSingle();
        }

        private void BindMonsterToDragSelector()
        {
            Container.BindInterfacesAndSelfTo<MonsterToDragSelector>()
                .AsSingle();
        }


    }
}