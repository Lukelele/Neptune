using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Animal
{
    [Header("Shark Settings")]
    [SerializeField] private float moveSpeed = 1;
    public float MoveSpeed => useStaticValue ? MoveSpeedStatic : moveSpeed;
    private static float MoveSpeedStatic;
    [SerializeField] private VisionCone vision;
    public HuntingSystem HuntingSystem;
    [SerializeField] private int[] netStructure = new[] { 2, 3, 2 };
    [SerializeField] private bool isIdealNet;
    [SerializeField] private bool usingNet;
    [SerializeField] public float iq;
    [SerializeField] public Color iqColor;
    [SerializeField] private bool displayIqColor;
    public bool DisplayIqColor => useStaticValue ? DisplayIqColorStatic : displayIqColor;
    private static bool DisplayIqColorStatic;
    [SerializeField] private GameObject iqColorObject;
    private Material iqMaterial;
    private Material initialIqMaterial;


    private float _feedForwardTimer = -10;
    [SerializeField] private float timeBetweenFeedForward = 1f;
    
    [SerializeField] private bool useStaticValue;
    
    void Awake()
    {
        timeBetweenFeedForward = Random.Range(0.5f, 1.5f);
        HuntingSystem = new HuntingSystem(netStructure);

        if (isIdealNet)
        {
            HuntingSystem.layers[1].weights[0, 0] = 1.0f;
            HuntingSystem.layers[1].weights[0, 1] = 0f;
            HuntingSystem.layers[1].weights[1, 0] = 0f;
            HuntingSystem.layers[1].weights[1, 1] = 1.0f;
        }
        
        vision.subject = transform;
        vision.targetTags = MyPreys;

        iqMaterial = iqColorObject.GetComponent<SkinnedMeshRenderer>().material;
        initialIqMaterial = Instantiate(iqMaterial);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (usingNet) HuntingSystem.Rotate(transform, timeBetweenFeedForward);
        Move();
    }
    
    protected void FixedUpdate()
    {
        if (usingNet)
        {
            if (Time.time - _feedForwardTimer > timeBetweenFeedForward)
            {
                HuntingSystem.FeedForward(transform, vision);
                timeBetweenFeedForward = Random.Range(0.5f, 1.5f);
                iqMaterial.color = DisplayIqColor ? iqColor : initialIqMaterial.color;
                _feedForwardTimer = Time.time;
            }
        }
    }
    
    void Move()
    {
        transform.Translate(Vector3.forward * (MoveSpeed * Time.deltaTime));
    }
    
    protected override GameObject Reproduce()
    {
        GameObject child = base.Reproduce();
        Shark childScript = child.GetComponent<Shark>();
        childScript.HuntingSystem = new HuntingSystem(HuntingSystem.CopyLayers());
        childScript.HuntingSystem.Mutate();
        return child;
    }
    
    public void VisualiseNet()
    {
        HuntingSystem.VisualiseNet();
    }
    
    public static void SetMoveSpeedStatic(float moveSpeed)
    {
        MoveSpeedStatic = moveSpeed;
    }
    
    public static void SetDisplayIqColorStatic(bool displayIqColor)
    {
        DisplayIqColorStatic = displayIqColor;
    }
}
