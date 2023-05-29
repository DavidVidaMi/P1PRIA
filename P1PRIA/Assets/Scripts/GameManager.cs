//Author: David Vidal Miguéns 31/01/23
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject questionNumberText;
    public GameObject questionText;
    public List<GameObject> answerButtons;

    private Response response;
    private int round = 0;
    void Start()
    {
        //Start a coroutine to call the API
        StartCoroutine(GetRequest("https://opentdb.com/api.php?amount=10"));
    }

    IEnumerator GetRequest(string uri)
    {
       using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Gives feedback.
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("ConnectionError ");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    ParseJSON(webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public void StartGame()
    {
        round = 0;
        NextRound();
    }

    public void NextRound()
    {
        questionText.GetComponent<TMP_Text>().text = response.results[round].question;
        answerButtons[0].transform.GetChild(0).GetComponent<TMP_Text>().text = response.results[round].correct_answer;
        answerButtons[1].transform.GetChild(0).GetComponent<TMP_Text>().text = response.results[round].incorrect_answers[0];
        answerButtons[2].transform.GetChild(0).GetComponent<TMP_Text>().text = response.results[round].incorrect_answers[1];
        answerButtons[3].transform.GetChild(0).GetComponent<TMP_Text>().text = response.results[round].incorrect_answers[2];
        round++;
        questionNumberText.GetComponent<TMP_Text>().text = "Question number "+round+":";
        
    }


    private void ParseJSON(string JSONString)
    {
        response = JsonUtility.FromJson<Response>(JSONString);
        if(response.response_code == "0")
        {
            Debug.Log("Numero de preguntas : "+ response.results.Count);
            Debug.Log("Categoría da pregunta 1 : "+ response.results[0].category);
            Debug.Log("Tipo da pregunta 1 : "+ response.results[0].type);
            Debug.Log("Dificultad da pregunta 1 : "+ response.results[0].difficulty);
            Debug.Log("Pregunta 1 : "+ response.results[0].question);
        }
    }
}
