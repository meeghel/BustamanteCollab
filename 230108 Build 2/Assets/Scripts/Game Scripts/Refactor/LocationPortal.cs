using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

// Video45: Teleports the player to a different location without switching scenes

public class LocationPortal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;

    PlayerController player;

    public void OnPlayerTriggered(PlayerController _player)
    {
        player = _player;
        StartCoroutine(Teleport());
    }

    public bool TriggerRepeatedly => false;

    Fader fader;

    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    IEnumerator Teleport()
    {
        CinemachineBrain brain = FindObjectOfType<CinemachineBrain>();
        var blendTime = brain.m_DefaultBlend.m_Time;

        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);

        var destPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);
        brain.m_DefaultBlend.m_Time = 0;

        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);
        brain.m_DefaultBlend.m_Time = blendTime;
    }

    public Transform SpawnPoint => spawnPoint;
}
