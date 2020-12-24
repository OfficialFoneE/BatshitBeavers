using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class SpamKeyMinigame : MonoBehaviour, IPunObservable
{
    private static Color player1Color = new Color(255 / 255.0f, 0 / 255.0f, 45 / 255.0f, 255 / 255.0f);
    private static Color player2Color = new Color(240 / 255.0f, 181 / 255.0f, 65 / 255.0f, 255 / 255.0f);

    private ProgressBar progressBar;

    private PhotonView photonView;

    private TextMeshProUGUI sliderText;
    private Image loadingBarImage;

    private bool _isPlayer1;
    public bool isPlayer1
    {
        private get
        {
            return _isPlayer1;
        }
        set
        {
            if (sliderText == null || loadingBarImage == null || photonView == null)
            {
                return;
            }
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

            if(shouldBeMine && !photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            } else if (!shouldBeMine && photonView.IsMine)
            {
                photonView.TransferOwnership(0);
            }

        }
    }

    public KeyCode keyCode;

    public float valuePerKeyPress = 0.1f;

    public bool hasDecay = false;
    public float decayAmount = 0.05f;

    public float currentValue = 0;

    public delegate void GeneralEventHandler();
    public GeneralEventHandler onClicked;
    public GeneralEventHandler onFinished;
    public GeneralEventHandler onZero;

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
        currentValue = 0;

        //if(((isPlayer1 && PhotonNetwork.IsMasterClient) || (!isPlayer1 && !PhotonNetwork.IsMasterClient)) && !photonView.IsMine)
        //{
        //    photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        //}
    }

    private void Update()
    {
        if (photonView.IsMine)
        {


            if (Input.GetKeyDown(keyCode))
            {
                currentValue += valuePerKeyPress;
                if (onClicked != null)
                {
                    onClicked.Invoke();
                }
            }

            if (currentValue >= 1f)
            {
                if (onFinished != null)
                {
                    onFinished.Invoke();
                }
            }

            if (hasDecay)
            {
                currentValue -= decayAmount * Time.deltaTime;
            }

            currentValue = Mathf.Clamp(currentValue, 0, 1f);

            if (currentValue <= 0)
            {
                if(onZero != null)
                {
                    onZero.Invoke();
                }
            }
        }

        progressBar.currentPercent = currentValue * 100;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.Serialize(ref currentValue);
        } else
        {
            stream.Serialize(ref currentValue);
        }
    }
}
