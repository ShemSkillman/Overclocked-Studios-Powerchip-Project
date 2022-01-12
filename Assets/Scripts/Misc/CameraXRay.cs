using UnityEngine;
using System.Collections.Generic;

public class CameraXRay : MonoBehaviour
{
    Dictionary<Renderer, bool> trackedObstructions;

    [SerializeField] float xRayDistance = 15f;
    [SerializeField] [Range(0f, 1f)] float transparencyAlpha = 0.3f;
    [SerializeField] CharacterController player;

    private void Start()
    {
        trackedObstructions = new Dictionary<Renderer, bool>();
    }

    Renderer[] GetObstructions()
    {
        Renderer[] renderers = new Renderer[trackedObstructions.Count];
        trackedObstructions.Keys.CopyTo(renderers, 0);

        return renderers;
    }

    void Update()
    {
        foreach (Renderer rend in GetObstructions())
        {
            trackedObstructions[rend] = false;
        }

        RaycastHit[] hits;
        Vector3 playerWorldPos = player.transform.position + player.center;
        hits = Physics.RaycastAll(transform.position, (playerWorldPos - transform.position).normalized, xRayDistance, LayerMask.GetMask("Terrain"));

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            Renderer rend = hit.transform.GetComponent<Renderer>();

            if (rend)
            {
                trackedObstructions[rend] = true;

                // Change the material of all hit colliders
                // to use a transparent shader.
                rend.material.shader = Shader.Find("Transparent/Diffuse");
                Color tempColor = rend.material.color;
                tempColor.a = transparencyAlpha;
                rend.material.color = tempColor;
            }
        }

        foreach (Renderer rend in GetObstructions())
        {
            if (trackedObstructions[rend] == false)
            {
                rend.material.shader = Shader.Find("Standard");
                Color tempColor = rend.material.color;
                tempColor.a = 1f;
                rend.material.color = tempColor;

                trackedObstructions.Remove(rend);
            }
        }
    }
}