using UnityEngine;
using System.Collections;



//얘도 아마 걍 task manager에 통합할 수 있을 것 같은데 일단 립모션 나오기 전까지는 걍 냅두장~!
public class ExtinguisherWorking : MonoBehaviour
{
    FireExtManager fireExtManagerInstance;


    private bool On = false;//fire extinguisher On/off

    //fire extinguisher particle movement by the key input

    private bool initOn = false;

    [SerializeField]
    private float particleRotLimit = 50.0f;
    private float particlePosXLimit = 2.2f;
    private float particlePosZLeftLimit = 0.81f;//near 220 degree
    private float particlePosZRightLimit = 1.41f;//near 320 degree

    private float particlePosZOri = 3.51f;
    
    

    //fire extinguisher movement by the key input    
    private float extinguisherRotDelta = 2.0F;//one movement is one degree
    private float extinguisherPosDelta;//movement of pos according to the rotation

    private float extinguisherRotOri = 270.0F;
    private float extinguisherRotLimit = 50.0f;


    private float samplingRate = 0.5f;//send the signal per 0.5 second
    private float prevTime;

    private bool isPrevLimitLeft = false;
    private bool isPrevLimitRight = false;

    private bool isMeetLimitLeft = false;
    private bool isMeetLimitRight = false;

    
        
    public int comm=99;

    

    // Use this for initialization
    void Start()
    {
        extinguisherPosDelta = 0.9f / (2 * extinguisherRotLimit) * extinguisherRotDelta;

        prevTime = Time.time;
    }

    // Update is called once per frame

    void particleOnOff(bool buttonOnOffFire, Transform fireExtinguisherParticle)
    {
        

        

        //fire extinguisher on/off

        if (buttonOnOffFire == true)
        {
            if (!On)
            {
                fireExtinguisherParticle.gameObject.SetActive(true);
                On = !On;
            }
            else
            {
                fireExtinguisherParticle.gameObject.SetActive(false);
                On = !On;
            }
        }
    }

    void joystickMapping(ref bool buttonOnOffFire, ref bool buttonFireExtinguisherLeft, ref bool buttonFireExtinguisherRight)
    {
        
        if ((gameObject.transform.GetComponent("CentralSystem") as CentralSystem).isJoystick == true)
        {
            
            buttonOnOffFire = Input.GetKeyDown( CentralSystem.getJoystickMappingInfo(JoystickType.X));
            buttonFireExtinguisherLeft = Input.GetKey( CentralSystem.getJoystickMappingInfo(JoystickType.LB));
            buttonFireExtinguisherRight = Input.GetKey(CentralSystem.getJoystickMappingInfo(JoystickType.RB));
        }
        else
        { 
            buttonOnOffFire = Input.GetKeyDown("x");
            buttonFireExtinguisherLeft = Input.GetKey("q");
            buttonFireExtinguisherRight = Input.GetKey("e");
        }
    }

    float getParticleZDelta(float angle, bool isLeftPressed)
    {
        float delta;
        if (angle < extinguisherRotOri)
        {
            delta = (particlePosZLeftLimit) / extinguisherRotLimit * extinguisherRotDelta;
            if (isLeftPressed == true)
                delta = -delta;
            
        }
        else
        { 
            delta = (particlePosZRightLimit) / extinguisherRotLimit * extinguisherRotDelta;
            if (isLeftPressed == false)
                delta = -delta;
        }
        
        return delta;
    }

    //if nothings happens, then return the true(None)
    bool moveHoseAndParticle(bool buttonFireExtinguisherLeft, bool buttonFireExtinguisherRight, Transform fireExtinguisher, Transform fireExtinguisherParticle)
    {
        bool nothingMove = false;

        fireExtinguisher.gameObject.SetActive(true);


        Vector3 oriPosParticle = fireExtinguisherParticle.transform.localPosition;
        Vector3 oriRotParticle = fireExtinguisherParticle.transform.localRotation.eulerAngles;
        Vector3 oriPosExtinguisher = fireExtinguisher.transform.localPosition;
        Vector3 oriRotExtinguisher = fireExtinguisher.transform.localRotation.eulerAngles;
        

        if (buttonFireExtinguisherLeft)
        {
            if (oriRotExtinguisher.y > extinguisherRotOri - extinguisherRotLimit)
            {
                //for counting contacting limitation

                isPrevLimitLeft = false;

                //for particle movement
                float particleXLeftDelta = (2 * particlePosXLimit) / (2 * extinguisherRotLimit) * extinguisherRotDelta;
                float particleZLeftDelta = getParticleZDelta(oriRotExtinguisher.y, true);
                float particleYRotDelta = (2 * particleRotLimit) / (2 * extinguisherRotLimit) * extinguisherRotDelta;

                float particleZPos = oriPosParticle.z + particleZLeftDelta;

                

                float rotDelta = oriRotExtinguisher.y - extinguisherRotOri;
                if (rotDelta < 0) rotDelta = -rotDelta;

                if (rotDelta < 2.0f)
                {
                    particleZPos = particlePosZOri;
                }

                fireExtinguisherParticle.transform.localPosition = new Vector3(oriPosParticle.x - particleXLeftDelta, oriPosParticle.y, particleZPos);
                fireExtinguisherParticle.transform.localRotation = Quaternion.Euler(new Vector3(oriRotParticle.x, oriRotParticle.y - particleYRotDelta, oriRotParticle.z));

                //for extingiusher movement
                fireExtinguisher.transform.localPosition = new Vector3(oriPosExtinguisher.x - extinguisherPosDelta, oriPosExtinguisher.y, oriPosExtinguisher.z);
                fireExtinguisher.localRotation = Quaternion.Euler(new Vector3(oriRotExtinguisher.x, oriRotExtinguisher.y - extinguisherRotDelta, oriRotExtinguisher.z));
            }
            else
            {
                

                if (isPrevLimitLeft == false)
                {
                    isPrevLimitLeft = true;
                    isMeetLimitLeft = true;

                }
            }
        }
        else if (buttonFireExtinguisherRight)
        {
            if (oriRotExtinguisher.y < extinguisherRotOri + extinguisherRotLimit)
            {
                //for counting contacting limitation
                isPrevLimitRight = false;
                //for particle movement
                float particleXLeftDelta = (2 * particlePosXLimit) / (2 * extinguisherRotLimit) * extinguisherRotDelta;
                float particleZRightDelta = getParticleZDelta(oriRotExtinguisher.y, false);
                float particleYRotDelta = (2 * particleRotLimit) / (2 * extinguisherRotLimit) * extinguisherRotDelta;

                float particleZPos = oriPosParticle.z + particleZRightDelta;


                float rotDelta = oriRotExtinguisher.y - extinguisherRotOri;
                if (rotDelta < 0) rotDelta = -rotDelta;

                if (rotDelta < 2.0f)
                {
                    particleZPos = particlePosZOri;
                }

                fireExtinguisherParticle.transform.localPosition = new Vector3(oriPosParticle.x + particleXLeftDelta, oriPosParticle.y, particleZPos);
                fireExtinguisherParticle.transform.localRotation = Quaternion.Euler(new Vector3(oriRotParticle.x, oriRotParticle.y + particleYRotDelta, oriRotParticle.z));

                //for extinguisher movement
                fireExtinguisher.transform.localPosition = new Vector3(oriPosExtinguisher.x + extinguisherPosDelta, oriPosExtinguisher.y, oriPosExtinguisher.z);
                fireExtinguisher.localRotation = Quaternion.Euler(new Vector3(oriRotExtinguisher.x, oriRotExtinguisher.y + extinguisherRotDelta, oriRotExtinguisher.z));
            }
            else
            {
                if (isPrevLimitRight == false)
                {
                    isPrevLimitRight = true;
                    isMeetLimitRight = true;
                }
            }
        }
        else
            nothingMove = true;

        return nothingMove;
    }

    void sendInfo2Central(bool extinguisherOn, bool isNone, float currAngle)
    {

        fireExtManagerInstance.getCountofSpread(extinguisherOn, isMeetLimitLeft, isMeetLimitRight, isNone, currAngle);
        
        isMeetLimitLeft = false;
        isMeetLimitRight = false;
    }

    
    
    void Update()
    {
        if(initOn == false)
        {
            CentralSystem tmp = gameObject.transform.GetComponent("CentralSystem") as CentralSystem;
            if (tmp.taskManagerList != null)
            {
                fireExtManagerInstance = (gameObject.transform.GetComponent("CentralSystem") as CentralSystem).getTaskManager<FireExtManager>();
                initOn = true;
            }
        }

        if (fireExtManagerInstance.isDoingTask == true && fireExtManagerInstance.isDoneTask == false)
        {
           

            //get instance of particle and hose
            Transform fireExtinguisherParticle = gameObject.transform.FindChild("Fire_Extinguisher_Particle_Effect");
            Transform fireExtinguisher = gameObject.transform.FindChild("Fire_Extinguisher_FPS");
            
            //joystick mapping
            bool buttonOnOffFire = false;
            bool buttonFireExtinguisherLeft = false;
            bool buttonFireExtinguisherRight = false;
            bool buttonFireExtinguisherNone = false;

            joystickMapping(ref buttonOnOffFire, ref buttonFireExtinguisherLeft, ref buttonFireExtinguisherRight);

            particleOnOff(buttonOnOffFire, fireExtinguisherParticle);

            buttonFireExtinguisherNone = moveHoseAndParticle(buttonFireExtinguisherLeft, buttonFireExtinguisherRight, fireExtinguisher, fireExtinguisherParticle);
            
            sendInfo2Central(On, buttonFireExtinguisherNone,fireExtinguisher.transform.localRotation.eulerAngles.y);
        }
        else if(fireExtManagerInstance.isDoneTask == true)
        {
            CentralSystem.setActiveChild(gameObject, "Fire_Extinguisher_Particle_Effect", false);
            CentralSystem.setActiveChild(gameObject, "Fire_Extinguisher_FPS", false);
        } 
    }
    

    



}
