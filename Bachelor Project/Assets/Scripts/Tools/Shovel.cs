using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shovel : MonoBehaviour
{
    [SerializeField] ParticleSystem digEffect;
    private void OnEnable()
    {
        Step.OnStepStart += DestroyObject;
    }

    private void OnDisable()
    {
        Step.OnStepStart -= DestroyObject;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject.transform.parent.gameObject);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit tileHit;

        if (Physics.Raycast(ray, out tileHit, Mathf.Infinity))
        {
            if (tileHit.transform.gameObject.tag == "tile" && !EventSystem.current.IsPointerOverGameObject())
            {
                Tile clickedTile = tileHit.transform.gameObject.GetComponent<Tile>();

                if (!clickedTile.PlacedObject() && !clickedTile.Impassable())
                {
                    transform.position = clickedTile.transform.position + new Vector3(0, clickedTile.transform.localScale.y / 2, 0) + new Vector3(0, transform.localScale.y / 2, 0);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Dig(clickedTile);
                    }
                }
            }
        }

    }

    public void Dig(Tile tile)
    {
        ParticleSystem ps = Instantiate(digEffect, transform.position + new Vector3(0, -0.5f, 0), transform.rotation * Quaternion.Euler(0, 0, 0));
        ps.transform.SetParent(null, true);

        ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material.color = tile.GetComponent<Renderer>().material.color;

        tile.SetHeight(tile.Height() - 1);
    }
    public void DestroyObject()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
