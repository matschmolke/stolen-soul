using System.Collections.Generic;
using UnityEngine;

public class BreadcrumbTrail : MonoBehaviour
{
    public bool enableGismos = true;

    [Tooltip("How far the player must move before dropping the next breadcrumb.")]
    public float dropDistance = 2f;

    [Tooltip("Maximum number of breadcrumbs to keep active.")]
    public int maxBreadcrumbs = 40;

    [Tooltip("How long (in seconds) breadcrumbs should live before being removed. 0 = forever.")]
    public float breadcrumbLifetime = 6f;

    // Internal data
    private List<Breadcrumb> breadcrumbs = new List<Breadcrumb>();
    private Vector2 lastDropPosition;
    private BoxCollider2D playerCollider;

    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        lastDropPosition = playerCollider.bounds.center;
    }

    void Update()
    {
        float distanceMoved = Vector2.Distance(playerCollider.bounds.center, lastDropPosition);

        if (distanceMoved >= dropDistance)
        {
            DropBreadcrumb();
            lastDropPosition = playerCollider.bounds.center;
        }

        CleanupOldBreadcrumbs();
    }

    private void DropBreadcrumb()
    {
        var crumb = new Breadcrumb
        {
            position = playerCollider.bounds.center,
            timeDropped = Time.time,
            next = null
        };

        if (breadcrumbs.Count > 0)
        {
            breadcrumbs[breadcrumbs.Count - 1].next = crumb;
        }

        breadcrumbs.Add(crumb);

        if (breadcrumbs.Count > maxBreadcrumbs)
        {
            breadcrumbs.RemoveAt(0);
        }
    }

    private void CleanupOldBreadcrumbs()
    {
        if (breadcrumbLifetime <= 0) return;

        for (int i = breadcrumbs.Count - 1; i >= 0; i--)
        {
            if (Time.time - breadcrumbs[i].timeDropped > breadcrumbLifetime)
            {
                breadcrumbs.RemoveAt(i);
            }
        }
    }

    public List<Breadcrumb> GetBreadcrumbs()
    {
        return new List<Breadcrumb>(breadcrumbs);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var b in breadcrumbs)
            Gizmos.DrawSphere(b.position, 0.1f);
    }
}
