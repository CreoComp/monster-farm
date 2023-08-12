using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDataService : MonoBehaviour
{
    private AudioClip jumpMonster;
    private AudioClip connectDivideMonsters;
    private AudioClip buildingUnlock;
    private AudioClip unlockNewMonster;
    private AudioClip openEgg;

    private AudioSource audioSource;

    public void Awake() 
    {
        DontDestroyOnLoad(this);
        SetSounds();

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void SetSounds()
    {
        jumpMonster = Resources.Load<AudioClip>("Sound/jump");
        connectDivideMonsters = Resources.Load<AudioClip>("Sound/connectDivideMonsters");
        buildingUnlock = Resources.Load<AudioClip>("Sound/buildingUnlock");
        unlockNewMonster = Resources.Load<AudioClip>("Sound/unlockNewMonster");
        openEgg = Resources.Load<AudioClip>("Sound/egg");
    }

    public void JumpMonster() => audioSource.PlayOneShot(jumpMonster);
    public void ConnectDivideMonsters() => audioSource.PlayOneShot(connectDivideMonsters);
    public void BuildingUnlock() => audioSource.PlayOneShot(buildingUnlock);
    public void UnlockNewMonster() => audioSource.PlayOneShot(unlockNewMonster);
    public void OpenEgg() => audioSource.PlayOneShot(openEgg);
}
