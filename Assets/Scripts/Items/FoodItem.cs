using UnityEngine;

namespace Items
{
    public class FoodItem : GrabbableItem, ICookable, IDepositable, IRipenable
    {
        private const string BASE_COLOR_PROPERTY_NAME = "_BaseColor";
        private static readonly int BASE_COLOR_PROPERTY_ID = Shader.PropertyToID(BASE_COLOR_PROPERTY_NAME);

        [SerializeField] private Renderer _renderer;
        [Header("Ripe Parameters")]
        [SerializeField] private float _ripenDuration = 30f;
        [SerializeField] private Vector2 _ripeScaleMinMax = new Vector2(0.3f, 1f);
        [Header("Cooked Parameters")]
        [SerializeField] private float _cookDuration = 10f;
        [SerializeField] private int _rawCalories = 10;
        [SerializeField] private int _cookedCalories = 20;
        [SerializeField] private Color _rawColor = Color.green;
        [SerializeField] private Color _cookedColor = Color.yellow;

        public float RipenDuration => _ripenDuration;

        public float RipePercent
        {
            get => _ripePercent;
            set
            {
                _ripePercent = value;
                Transform.localScale = Vector3.one * Mathf.Lerp(_ripeScaleMinMax.x, _ripeScaleMinMax.y, RipePercent);
            }
        }

        public bool IsRipe => RipePercent.Equals(1f);

        public int RawCalories => _rawCalories;

        public int CookedCalories => _cookedCalories;

        public float CookDuration => _cookDuration;

        private float _cookedPercent;
        private float _ripePercent;

        public float CookedPercent
        {
            get => _cookedPercent;
            set
            {
                _cookedPercent = value;
                UpdateColor();
            }
        }
    
        public bool IsCooked => CookedPercent >= 1f;
        
        
        public bool IsDeposited { get; set; }
        public IContainer Container { get; set; }

        
        public void Deposit(IContainer container)
        {
            IsDeposited = true;
            Container = container;
        }

        public void Withdraw()
        {
            IsDeposited = false;
            Container = null;
        }

        protected override void Awake()
        {
            base.Awake();
            InitializeRenderer();
        }

        private void InitializeRenderer()
        {
            if (_renderer != null)
            {
                return;
            }

            _renderer = GetComponent<Renderer>();
        }

        private void UpdateColor()
        {
            _renderer.material.SetColor(BASE_COLOR_PROPERTY_ID, Color.Lerp(_rawColor, _cookedColor, _cookedPercent));
        }
    }
}
