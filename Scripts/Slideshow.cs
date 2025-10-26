using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Slideshow : MonoBehaviour
{
    public Image image;
    public Sprite[] sprites;
    public float gap;
    void Start()
    {
        StartCoroutine(PlaySlideshow());
    }

    IEnumerator PlaySlideshow()
    {
        yield return new WaitForSecondsRealtime(gap);

        foreach (var sprite in sprites)
        {
            image.sprite = sprite;
            yield return new WaitForSecondsRealtime(gap);
        }
    }

}
