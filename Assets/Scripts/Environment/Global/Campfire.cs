using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Костёр может быть зажённым и потушенным, зажённый костёр позволяет не впадать в безумие, производить спец. для этого места предметы, а также предоставляет кров.
/// !--> Для работы костра требует поленья.
/// 
/// Чтобы разжечь костёр требуются 2 палки и 1 полено
/// 
/// 
/// Рабочий, зажённый, костёр:
/// + > разжечь костёр и время горения
/// + > позволяет не впадать в безумие
/// + > возможность спать (сохранение + прокрутка времени)
/// + > Для каждого костра свой набор уникальных предметов.
/// 
/// </summary>
public class Campfire : MonoBehaviour
{
    [SerializeField] private bool _isBurning = false;
    [SerializeField] private Light _light;
    [SerializeField] private GameRuler _gameRuler;
    
    [SerializeField, Range(1, 3600)] private float _workingTimeFromFuelUnitInSeconds = 10;
    [SerializeField, Range(0.0f, 1.0f)] private float _timeProgress;

    [SerializeField] private List<GameObject> _craftItems = new List<GameObject>();
    [SerializeField] private ParticleSystem _particleSystem;

    [SerializeField] private AudioSource _audio;

    public bool IsBurning
    {
        get { return _isBurning; }
        set { _isBurning = value; }
    }
    public float TimeProgress => _timeProgress;

    [SerializeField] private SaveLoadManager _saveLoadManager;
    [SerializeField] private Daytime _daytime;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Transform _parentUI;
    [SerializeField] private GameObject _prefabUI;
    

    private FixedButton _setOnFire_button;
    private GameObject _setOnFire_instaledPrefab;

    private FixedButton _sleep_button;
    private GameObject _sleep_instaledPrefab;

    private bool _isPlayerInArea = false;
    private bool _isProlongBurning = false;

    private void Start()
    {
        var globalScript = GameObject.FindGameObjectWithTag("GlobalScript");

        _inventory ??= globalScript.GetComponent<Inventory>();
        _daytime ??= globalScript.GetComponent<Daytime>();
        _saveLoadManager ??= globalScript.GetComponent<SaveLoadManager>();

        _particleSystem ??= GameObject.FindGameObjectWithTag("CfPS").GetComponent<ParticleSystem>();

        if (_parentUI == null)
        {
            _parentUI = GameObject.FindGameObjectWithTag("TIS").transform;
        }
    }

    private void Update()
    {
        if (IsBurning)
        {
            _timeProgress += Time.deltaTime / _workingTimeFromFuelUnitInSeconds;
        }

        if (_timeProgress >= 1)
        {
            CampfireIsOut();
            _particleSystem.Stop(true);
            _audio.Stop();
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если костёр горит вывести возможность поспать до утра и кнопка крафта (ссылается на инвентарь). Когда заканчивается топливо и игрок находится в зоне, вычитать у него полено
        // иначе дать возможность развести огонь (1 палено и 7 палок)

        if (other.tag == "Player")
        {
            _isPlayerInArea = true;

            if (_isBurning)
            {
                _gameRuler.IsInCampfireArea_Burning = true;
                _sleep_instaledPrefab?.SetActive(true);

                //включаем крафты для данного костра
                _craftItems.ForEach(item => item.SetActive(true));
            }
            else
            {
                if (_setOnFire_instaledPrefab == null)
                {
                    _setOnFire_instaledPrefab = Instantiate(_prefabUI, _parentUI);

                    _setOnFire_button = _setOnFire_instaledPrefab.GetComponent<FixedButton>();
                    var banner = _setOnFire_instaledPrefab.GetComponent<Banner>();
                    banner.Title = "Light a fire";
                    banner.enabled = true;
                }
                else
                {
                    _setOnFire_instaledPrefab.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(_timeProgress >= 0.9)
        {
            if(_isPlayerInArea)
            {
                if (_inventory.SubtractingAmountItem(1, out int availableQuantity))
                {
                    // есть полена ?
                    // да:
                    //вычистить полено
                    _timeProgress = 0;
                }
                else if(availableQuantity < 0)
                { 
                    CampfireIsOut();
                    _particleSystem.Stop(true);
                    _inventory.AddAmountItem(1);

                }
            }
            
        }


        //разжигаем костёр
        if (_setOnFire_button != null && _setOnFire_button.Pressed)
        {
            if (_isPlayerInArea)
            {
                if (_inventory.SubtractingAmountItem(1, out int availableQuantity))
                {
                    IsBurning = true;
                    _gameRuler.IsInCampfireArea_Burning = true;
                    _light.enabled = true;
                    _setOnFire_button.Pressed = false;
                    _setOnFire_instaledPrefab?.SetActive(false);

                    if (_sleep_instaledPrefab == null)
                    {
                        _sleep_instaledPrefab = Instantiate(_prefabUI, _parentUI);

                        _sleep_button = _sleep_instaledPrefab.GetComponent<FixedButton>();
                        var banner = _sleep_instaledPrefab.GetComponent<Banner>();
                        banner.Title = "Sleep";
                        banner.enabled = true;
                    }
                    _sleep_instaledPrefab.SetActive(true);

                    //Вычесть предметы для растопки (сконфигурировать)

                    //включаем крафты для данного костра
                    _craftItems.ForEach(item => item.SetActive(true));
                    Debug.Log(_particleSystem.isPlaying);
                    _particleSystem?.Play(true);
                    Debug.Log(_particleSystem.isPlaying);
                    _audio.Play();

                }
                else if (availableQuantity < 0)
                {
                    Debug.Log("Недостаточно предметов");
                    _setOnFire_button.Pressed = false;
                    //_inventory.AddAmountItem(1);
                }
                else
                {
                    Debug.Log("Недостаточно предметов 2");
                    _setOnFire_button.Pressed = false;
                }
            }

        }

        //спим
        if (_sleep_button != null && _sleep_button.Pressed)
        {
            StartCoroutine(Sleeping());
        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _sleep_instaledPrefab?.SetActive(false);
            _setOnFire_instaledPrefab?.SetActive(false);
            _gameRuler.IsInCampfireArea_Burning = false;
            _isPlayerInArea = false;

            //выключаем крафты для данного костра
            _craftItems.ForEach(item => item.SetActive(false));

        }
    }


    private void CampfireIsOut()
    {
        _gameRuler.IsInCampfireArea_Burning = false;
        _light.enabled = false;
        IsBurning = false;
        _timeProgress = 0;
        _sleep_instaledPrefab?.SetActive(false);

        //выключаем крафты для данного костра
        _craftItems.ForEach(item => item.SetActive(false));
    }



    private IEnumerator Sleeping()
    {
        var waitFading = true;
        Fader.instance.FadeIn(() => waitFading = false);

        while (waitFading)
            yield return null;



        _sleep_button.Pressed = false;
        Debug.Log("I slept");
        //_sleep_instaledPrefab?.SetActive(false);

        // логика для сохранения и прокрутки времени (+ эффект закрывания и открывания глаз)
        _daytime.TimeProgress = 0.3f;
        _saveLoadManager.Save();



        waitFading = true;
        Fader.instance.FadeOut(() => waitFading = false);

        while (waitFading)
            yield return null;
    }





}
