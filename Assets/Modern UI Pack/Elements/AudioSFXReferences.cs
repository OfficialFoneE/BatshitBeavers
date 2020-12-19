using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXReferences : MonoBehaviour
{

    private static GameObject audioSourcePrefab;

    private Transform mainCameraTransform;

    public static AudioSFXReferences instance;

    private static AudioSource backgroundMusicAudioSource;

    private static BufferedArray<BufferedAudioSource> bufferedAudioSources;

    public delegate void OnMusicChanged(string name);
    public static OnMusicChanged onMusicChanged;

    [SerializeField] private List<AudioClip> backgroundMusicClips = new List<AudioClip>();
    private static int currentClipIndex = 0;

    [SerializeField] private AudioClip _buttonHighlightClip;
    [SerializeField] private AudioClip _buttonClickClip;
    private static AudioClip buttonHighlightClip;
    private static AudioClip buttonClickClip;

    [SerializeField] private AudioClip graphNodeHighlight;
    [SerializeField] private AudioClip _nodeCreateClip;
    [SerializeField] private AudioClip _nodeDestroyClip;
    private static AudioClip nodeCreateClip;
    private static AudioClip nodeDestroyClip;

    [SerializeField] private AudioClip graphEdgeHighlight;
    [SerializeField] private AudioClip _edgeCreateClip;
    [SerializeField] private AudioClip _edgeDestroyClip;
    private static AudioClip edgeCreateClip;
    private static AudioClip edgeDestroyClip;

    private void Awake()
    {
        bufferedAudioSources = new BufferedArray<BufferedAudioSource>(InstantiateBufferedAudioSource, BufferBufferedAudioSource);

        instance = this;

        mainCameraTransform = Camera.main.transform;

        backgroundMusicAudioSource = GameObject.Find("BackgroundMusicAudioSource").GetComponent<AudioSource>();

        buttonHighlightClip = _buttonHighlightClip;
        buttonClickClip = _buttonClickClip;

        nodeCreateClip = _nodeCreateClip;
        nodeDestroyClip = _nodeDestroyClip; ;
        edgeCreateClip = _edgeCreateClip;
        edgeDestroyClip = _edgeDestroyClip;

        bufferedAudioSources.UpdatePooledObjects(20);
        bufferedAudioSources.UpdatePooledObjects(0);
    }

    private void Start()
    {
        if (backgroundMusicClips.Count > 0)
        {
            currentClipIndex = 0;
            PlaySong();
        }
    }

    private void PlayNextTrackAutomatically()
    {
        currentClipIndex++;

        PlaySong();
    }

    public void PlayNextSong()
    {
        CancelInvoke();
        currentClipIndex++;
        PlaySong();
    }
    public void PlayPreviousSong()
    {
        CancelInvoke();
        currentClipIndex--;
        PlaySong();
    }

    private void PlaySong()
    {
        backgroundMusicAudioSource.Stop();

        currentClipIndex = currentClipIndex %= backgroundMusicClips.Count;

        if(currentClipIndex < 0)
        {
            currentClipIndex = currentClipIndex + backgroundMusicClips.Count;
        }

        backgroundMusicAudioSource.clip = backgroundMusicClips[currentClipIndex];

        backgroundMusicAudioSource.Play();

        if(onMusicChanged != null)
        {
            onMusicChanged.Invoke(backgroundMusicClips[currentClipIndex].name);
        }

        Invoke("PlayNextTrackAutomatically", backgroundMusicClips[currentClipIndex].length + UnityEngine.Random.Range(15, 25));
    }

    public static void PlayButtonHighlight()
    {
        bufferedAudioSources.GetUnusedPooledObjects(1)[0].PlayOneShot(buttonHighlightClip, Random.Range(0.9f, 1.1f), 1);
    }

    public static void PlayButtonClick()
    {
        bufferedAudioSources.GetUnusedPooledObjects(1)[0].PlayOneShot(buttonClickClip, 1f, 2);
    }

    public static void PlayCreateNode()
    {
        bufferedAudioSources.GetUnusedPooledObjects(1)[0].PlayOneShot(nodeCreateClip, Random.Range(0.9f, 1.1f), 1);
    }
    public static void PlayDestroyNode()
    {
        bufferedAudioSources.GetUnusedPooledObjects(1)[0].PlayOneShot(nodeDestroyClip, Random.Range(0.9f, 1.1f), 1.35f);
    }

    public static void PlayCreateEdge()
    {
        bufferedAudioSources.GetUnusedPooledObjects(1)[0].PlayOneShot(edgeCreateClip, Random.Range(0.9f, 1.1f), 1);
    }
    public static void PlayDestroyEdge()
    {
        bufferedAudioSources.GetUnusedPooledObjects(1)[0].PlayOneShot(edgeDestroyClip, Random.Range(0.9f, 1.1f), 1.35f);
    }









    private BufferedAudioSource InstantiateBufferedAudioSource()
    {
        return new BufferedAudioSource(Instantiate(audioSourcePrefab, mainCameraTransform));
    }

    private void BufferBufferedAudioSource(BufferedAudioSource bufferedAudioSource, bool state)
    {

    }

    private class BufferedAudioSource : BufferedObject
    {
        private AudioSource audioSource;

        public BufferedAudioSource(GameObject gameObject) : base(gameObject)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        public void PlayOneShot(AudioClip audioClip, float pitch, float volume)
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip, volume);

            instance.StartCoroutine(OnClickEnd(this, audioClip.length));
        }
    }

    private static IEnumerator OnClickEnd(BufferedAudioSource bufferedAudioSource, float time)
    {
        yield return new WaitForSeconds(time);

        bufferedAudioSources.Buffer(bufferedAudioSource);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void OnLoadResources()
    {
        audioSourcePrefab = Resources.Load<GameObject>("AudioSourcePrefab");
    }

}
