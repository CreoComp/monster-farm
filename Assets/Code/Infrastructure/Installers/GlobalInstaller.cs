using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;
      //  [SerializeField] private SoundDataService _soundDataService;

        public override void InstallBindings()
        {
            BindSoundData();
            BindCoroutineRunner();
        }

        private void BindSoundData()
        {

            SoundDataService _soundDataService = new GameObject("SoundDataService").AddComponent<SoundDataService>();

            Container
                .Bind<SoundDataService>()
                .FromInstance(_soundDataService)
                .AsSingle();

        }

        private void BindCoroutineRunner()
        {
            Container.Bind<ICoroutineRunner>()
                .To<CoroutineRunner>()
                .FromComponentInNewPrefab(_coroutineRunner)
                .AsSingle();
        }
    }
}