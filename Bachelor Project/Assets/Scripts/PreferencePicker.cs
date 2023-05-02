using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreferencePicker : MonoBehaviour
{
    [SerializeField] List<Grid> grids = new List<Grid>();
    [SerializeField] Transform pickedTransform;
    [SerializeField] Transform notPickedTransform;
    [SerializeField] Transform startTransform;
    [SerializeField] Transform baseTransform;
    [SerializeField] GameObject clappy;
    [SerializeField] TextHandler textHandler;
    [SerializeField] TMP_Text gridCountText;
    [SerializeField] Canvas startCanvas;
    Grid currentGrid;
    int gridsSwipedOn;
    int totalGrids;

    private void Start()
    {
        gridsSwipedOn = 0;
        totalGrids = grids.Count;
    }

    private void Update()
    {
        if (startCanvas.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.Space))
        {
            startCanvas.gameObject.SetActive(false);
            ActivateNewGrid();
        }

        if (startCanvas.gameObject.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            SendGridAway(notPickedTransform.position);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SavePrefences();
            SendGridAway(pickedTransform.position);
            clappy.GetComponent<Animator>().Play("Cheer");
        }


    }

    public void ActivateNewGrid()
    {
        if (currentGrid)
        {
            currentGrid.gameObject.SetActive(false);
            grids.Remove(currentGrid);
        }

        if(grids.Count == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        int randomIndex = Random.Range(0, grids.Count);

        currentGrid = grids[randomIndex];
        clappy.GetComponent<Animator>().Play("Cheer");

        StartCoroutine(MoveGridToStartPosition());
    }

    public void AddGridToList(Grid newGrid)
    {
        grids.Add(newGrid);
    }

    public void SendGridAway(Vector3 targetPosition)
    {
        gridsSwipedOn++;
        gridCountText.text = gridsSwipedOn + " / " + totalGrids;

        StartCoroutine(moveGrid(targetPosition));
    }

    IEnumerator moveGrid(Vector3 targetPosition)
    {
        Transform parentTransform = currentGrid.transform.parent.transform.parent;
        
        Vector3 startPos = parentTransform.gameObject.transform.position;
        Vector3 startScale = parentTransform.gameObject.transform.localScale;
        float progress = 0f;

        while (progress < 1)
        {
            parentTransform.gameObject.transform.position = Vector3.Lerp(startPos, targetPosition, progress);
            parentTransform.gameObject.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);

            if (progress > 0.95f)
            {
                progress = 1;
                parentTransform.gameObject.transform.position = targetPosition;
            }

            progress += Time.deltaTime * 1.5f;
            yield return null;
        }

        ActivateNewGrid();

        yield return 0;
    }

    IEnumerator MoveGridToStartPosition()
    {
        Transform parentTransform = currentGrid.transform.parent.transform.parent;
        
        Vector3 startPos = startTransform.position;
        Vector3 targetScale = parentTransform.gameObject.transform.localScale;

        parentTransform.gameObject.SetActive(true);

        parentTransform.gameObject.transform.localScale = Vector3.zero;

        float progress = 0f;

        while (progress < 1)
        {
            parentTransform.gameObject.transform.position = Vector3.Lerp(startPos, baseTransform.position, progress);
            parentTransform.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, progress);

            if (progress > 0.95f)
            {
                progress = 1;
                parentTransform.gameObject.transform.position = baseTransform.position;
                parentTransform.gameObject.transform.localScale = targetScale;
            }

            progress += Time.deltaTime * 1.5f;
            yield return null;
        }

        yield return 0;
    }

    public void SavePrefences()
    {
        textHandler.SavePreferenses("mapSizeX" + currentGrid.MapDimensionX().ToString());
        textHandler.SavePreferenses("mapSizeZ" + currentGrid.MapDimensionZ().ToString());

        textHandler.SavePreferenses("peakHeight" + currentGrid.PeakHeight().ToString());
        textHandler.SavePreferenses("peakHeightRange" + currentGrid.PeakHeightRange().ToString());
        textHandler.SavePreferenses("peakAmount" + currentGrid.PeakAmount().ToString());
    }
}
