using HoloToolkit.Examples.GazeRuler;
using UnityEngine;
using UnityEngine.Windows.Speech;
using Random = System.Random;

public class VoiceRecognizer : MonoBehaviour
{
    [Tooltip("The object you want to place")]
    public GameObject objectToPlace;

    private void Start()
    {
        var keywordRecognizer = new KeywordRecognizer(new[] {"Stop"});
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        var confidence = args.confidence;
        if (args.text == "Stop" &&
            (confidence == ConfidenceLevel.Medium || confidence == ConfidenceLevel.High))
        {
            Place();
        }
    }

    private void Place()
    {
        if (objectToPlace == null)
            return;
        LineManager a = new LineManager();
        a.EndLine();
        //var distance = new Random().Next(2, 10);
        //var location =
        //    transform.position +
        //    transform.forward*distance;

        //Instantiate(
        //    objectToPlace,
        //    location,
        //    Quaternion.LookRotation(transform.up, transform.forward));
    }
}