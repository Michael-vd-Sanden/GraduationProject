using UnityEngine;

public class MazePuzzle : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private GameObject popUpScreen, movementControls;
    [SerializeField] private GameObject[] questions;

    [Header("-------------- Changeble Values")]
    [SerializeField] private Vector3 changeCamPosTo;
    [SerializeField] private Quaternion rotateCamPosTo;
    [SerializeField] private float turnspeed = 1.0f;

    [Header("-------------- Background Values (do not change)")]
    [SerializeField] private int questionNumber;
    private Vector3 currentCamPos, changedCamPos;
    private Quaternion currentCamAngle, startCamAngle, targetCamAngle;
    [SerializeField] private bool isCamRotating = false, isQuestionActive = false;

    private void Update()
    {
        if(isCamRotating) 
        {
            currentCamAngle = cameraObj.transform.rotation;

            var step = turnspeed * Time.deltaTime;
            cameraObj.gameObject.transform.rotation = Quaternion.RotateTowards(currentCamAngle, targetCamAngle, step);
            
            if(currentCamAngle == targetCamAngle) { isCamRotating = false; }
        }
    }

    public void StartQuestion()
    {
        if (!isQuestionActive)
        {
            isQuestionActive = true;

            //camera position & rotation
            currentCamPos = cameraObj.transform.position;
            changedCamPos = currentCamPos + changeCamPosTo;
            cameraObj.gameObject.transform.position = changedCamPos;

            startCamAngle = cameraObj.transform.rotation;
            targetCamAngle = rotateCamPosTo;
            isCamRotating = true;

            //general
            popUpScreen.SetActive(true);
            movementControls.SetActive(false);

            //question generation
            questionNumber = Random.Range(0, questions.Length);
            questions[questionNumber].SetActive(true);
        }
    }

    public void EndQuestion()
    {
        //cam position & rotation
        cameraObj.gameObject.transform.position = currentCamPos;
        targetCamAngle = startCamAngle;
        isCamRotating = true;

        //general
        questions[questionNumber].SetActive(false);
        popUpScreen.SetActive(false);
        movementControls.SetActive(true);

        isQuestionActive = false;
    }

    public void AnswerQuestion(string answer)
    {
        if(answer == questions[questionNumber].name) 
        {
            Debug.Log("correct");
            EndQuestion();
        }
        else 
        {
            Debug.Log("incorrect");
            EndQuestion();
        }
    }
    
    public void EndMaze()
    {
        Debug.Log("End maze");
    }
}
