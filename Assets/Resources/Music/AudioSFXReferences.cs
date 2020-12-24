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


    [SerializeField] private AudioClip _mainMenuMusic;
    [SerializeField] private AudioClip _queueingMusic;
    [SerializeField] private AudioClip _gameMusic;
    private static AudioClip mainMenuMusic;
    private static AudioClip queueingMusic;
    private static AudioClip gameMusic;

    private void Awake()
    {
        bufferedAudioSources = new BufferedArray<BufferedAudioSource>(InstantiateBufferedAudioSource, BufferBufferedAudioSource);

        instance = this;

        mainCameraTransform = Camera.main.transform;

        backgroundMusicAudioSource = GameObject.Find("BackgroundMusicAudioSource").GetComponent<AudioSource>();

        buttonHighlightClip = _buttonHighlightClip;
        buttonClickClip = _buttonClickClip;

        mainMenuMusic = _mainMenuMusic;
        queueingMusic = _queueingMusic;
        gameMusic = _gameMusic;

        bufferedAudioSources.UpdatePooledObjects(20);
        bufferedAudioSources.UpdatePooledObjects(0);
    }

    private void Start()
    {
        //if (backgroundMusicClips.Count > 0)
        //{
        //    currentClipIndex = 0;
        //    PlaySong();
        //}

        PlayMainMenuMusic();
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

        //Invoke("PlayNextTrackAutomatically", backgroundMusicClips[currentClipIndex].length + UnityEngine.Random.Range(15, 25));
    }

    public static void PlayMainMenuMusic()
    {
        backgroundMusicAudioSource.Stop();

        backgroundMusicAudioSource.clip = mainMenuMusic;

        backgroundMusicAudioSource.Play();
    }
    public static void PlayQueueingMusic()
    {
        backgroundMusicAudioSource.Stop();

        backgroundMusicAudioSource.clip = queueingMusic;

        backgroundMusicAudioSource.Play();
    }
    public static void PlayGameMusic()
    {
        backgroundMusicAudioSource.Stop();

        backgroundMusicAudioSource.clip = gameMusic;

        backgroundMusicAudioSource.Play();
    }

    public static void PlayButtonHighlight()
    {
        bufferedAudioSources.GetUnusedPooledObjects(1)[0].PlayOneShot(buttonHighlightClip, Random.Range(0.9f, 1.1f), 1);
    }

    public static void PlayButtonClick()
    {
        bufferedAudioSources.GetUnusedPooledObjects(1)[0].PlayOneShot(buttonClickClip, 1f, 2);
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
