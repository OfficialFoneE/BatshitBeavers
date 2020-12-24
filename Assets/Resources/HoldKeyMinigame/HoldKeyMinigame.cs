using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class HoldKeyMinigame : MonoBehaviour, IPunObservable
{
    private static Color player1Color = new Color(255 / 255.0f, 0 / 255.0f, 45 / 255.0f, 255 / 255.0f);
    private static Color player2Color = new Color(240 / 255.0f, 181 / 255.0f, 65 / 255.0f, 255 / 255.0f);

    private ProgressBar progressBar;

    private PhotonView photonView;

    public KeyCode keyCode;

    public float holdTime = 30f;
    public float decayTime = 1f;
    public float currentHoldTime = 0;

    private TextMeshProUGUI sliderText;
    private Image loadingBarImage;

    private bool _isPlayer1;
    public bool isPlayer1
    {
        get
        {
            return _isPlayer1;
        }
        set
        {
            if (sliderText == null || loadingBarImage == null || photonView == null)
                return;

            if (value)
            {
                sliderText.color = player1Color;
                loadingBarImage.color = player1Color;
            }
            else
            {
                sliderText.color = player2Color;
                loadingBarImage.color = player2Color;
            }
            _isPlayer1 = value;

            bool shouldBeMine = ((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient));

            if (shouldBeMine && !photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            else if (!shouldBeMine && photonView.IsMine)
            {
                photonView.TransferOwnership(0);
            }
        }
    }

    public delegate void OnFinished();
    public OnFinished onFinished;

    private void Awake()
    {
        progressBar = GetComponent<ProgressBar>();
        photonView = GetComponent<PhotonView>();
        sliderText = GetComponentInChildren<TextMeshProUGUI>();
        loadingBarImage = transform.Find("Background").Find("Loading Bar").GetComponent<Image>();
    }

    private void Start()
    {
        NetworkManager.OnGameStart += OnGameStart;
    }

    private void OnGameStart()
    {
        if (((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient)) && !photonView.IsMine)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
    }

    private void OnEnable()
    {
        currentHoldTime = 0;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(keyCode))
            {
                currentHoldTime += Time.deltaTime;
            }
            else
            {
                currentHoldTime -= decayTime * Time.deltaTime;
            }

            currentHoldTime = Mathf.Clamp(currentHoldTime, 0, holdTime);

            if (currentHoldTime >= holdTime)
            {
                if (onFinished != null)
                {
                    onFinished.Invoke();
                }
            }
        }

        progressBar.currentPercent = currentHoldTime / holdTime * 100;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.Serialize(ref currentHoldTime);
        }
        else
        {
            stream.Serialize(ref currentHoldTime);
        }
    }
}
