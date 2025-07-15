using TMPro;
using UnityEngine;

public class RandomIvents : MonoBehaviour
{
    private int randomIvent;
    [SerializeField] private float iventPerTime = 30f;
    private float timer = 0;

    public TextMeshProUGUI[] texts;

    private bool FCW;
    private bool FCB;
    private bool MWF;
    private bool MWB;

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= iventPerTime)
        {
            timer = 0;
            CreateIvent();
        }
    }

    private void CreateIvent()
    {
        randomIvent = Random.Range(0, 2);
        if (randomIvent == 0)
            FasterCheckers();
        if (randomIvent == 1)
            MaxWeightChecker();
    }

    private void FasterCheckers()
    {
        int colorTeam = Random.Range(0, 2);
        GameObject[] checkers = GameObject.FindGameObjectsWithTag("Checker");
        if (colorTeam == 0 && !FCW)
        {
            foreach (GameObject checker in checkers)
            {
                if (checker.name == "White(Clone)")
                {
                    DragShoot dsChecker = checker.GetComponent<DragShoot>();
                    dsChecker.maxForce = dsChecker.maxForce * 1.6f;
                }
            }
            texts[0].text = "Your checkers faster!";
            Debug.Log("Faster Checkers for white");
            FCW = true;
        }
        else if(!FCB)
        {
            foreach (GameObject checker in checkers)
            {
                if (checker.name == "Black(Clone)")
                {
                    DragShoot dsChecker = checker.GetComponent<DragShoot>();
                    dsChecker.maxForce = dsChecker.maxForce * 1.6f;
                }
            }
            texts[2].text = "Your checkers faster!";
            FCB = true;
            Debug.Log("Faster Checkers for black");
        }
    }

    private void MaxWeightChecker()
    {
        int colorTeam = Random.Range(0, 2);
        GameObject[] checkers = GameObject.FindGameObjectsWithTag("Checker");
        if (colorTeam == 0 && !MWF)
        {
            foreach (GameObject checker in checkers)
            {
                if (checker.name == "White(Clone)")
                {
                    Rigidbody rChecker = checker.GetComponent<Rigidbody>();
                    rChecker.mass = rChecker.mass * 2f;
                    DragShoot dsChecker = checker.GetComponent<DragShoot>();
                    dsChecker.maxForce = dsChecker.maxForce * 1.3f;
                }
            }
            texts[1].text = "Your checkers weigh twice as much!";
            MWF = true;
            Debug.Log("Max weight Checkers for white");
        }
        else if (!MWB)
        {
            foreach (GameObject checker in checkers)
            {
                if (checker.name == "Black(Clone)")
                {
                    Rigidbody rChecker = checker.GetComponent<Rigidbody>();
                    rChecker.mass = rChecker.mass * 2f;
                    DragShoot dsChecker = checker.GetComponent<DragShoot>();
                    dsChecker.maxForce = dsChecker.maxForce * 1.3f;
                }
            }
            texts[3].text = "Your checkers weigh twice as much!";
            MWB = true;
            Debug.Log("Max weight Checkers for black ");
        }
    }
}
