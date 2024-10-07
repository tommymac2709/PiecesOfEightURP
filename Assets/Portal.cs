using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    enum DestinationIdentifier
    {
        A,
        B,
        C,
        D
    }

    [SerializeField] int sceneToLoad = -1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationIdentifier destination;
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float fadeWaitTime = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(SceneTransition());
        }
    }

    private IEnumerator SceneTransition()
    {
        transform.parent = null;

        DontDestroyOnLoad(gameObject);

        Fader fader = FindObjectOfType<Fader>();

        yield return fader.FadeOut(fadeOutTime);

        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.SaveGame();

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        wrapper.LoadGame();

        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);

        wrapper.SaveGame();

        yield return new WaitForSeconds(fadeWaitTime);

        yield return fader.FadeIn(fadeInTime);

        Destroy(gameObject);
        
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerStateMachine stateMachine = player.GetComponent<PlayerStateMachine>();
        stateMachine.Controller.enabled = false;
        player.transform.position = otherPortal.spawnPoint.position;
        player.transform.rotation = otherPortal.spawnPoint.rotation;
        stateMachine.Controller.enabled = true;
    }

    private Portal GetOtherPortal()
    {
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            if (portal.destination != destination) continue;

            return portal;
        }
        return null;
    }
}
