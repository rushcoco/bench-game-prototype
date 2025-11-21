using UnityEngine;

public interface ISittable
{
    void OnSitStart(Transform actor);
    void OnSitEnd(Transform actor);
}
