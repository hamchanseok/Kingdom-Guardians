using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageSequenceController : MonoBehaviour
{
    public Image firstImage;
    public Image secondImage;
    public RawImage thirdImage; // 변경된 부분
    public Image fourthImage;
    public Image fifthImage;
    public float fadeDuration = 2f;
    public float finalFadeDuration = 1f; // 마지막 단계의 지속 시간

    void Start()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // 첫 번째 이미지의 투명도를 0에서 255로 증가
        yield return StartCoroutine(FadeImage(firstImage, 0f, 1f, fadeDuration));

        // 첫 번째 이미지의 투명도를 255에서 0으로 감소
        yield return StartCoroutine(FadeImage(firstImage, 1f, 0f, fadeDuration));

        // 두 번째 이미지의 투명도를 0에서 255로 증가시키고 스케일을 0에서 1로 증가, 동시에 세 번째 이미지의 투명도를 0에서 255로 증가
        yield return StartCoroutine(FadeInAndScaleUp(secondImage, thirdImage));

        // 네 번째 이미지와 다섯 번째 이미지의 투명도를 동시에 0에서 255로 1초 안에 증가
        yield return StartCoroutine(FadeImagesSimultaneously(fourthImage, fifthImage, 0f, 1f, finalFadeDuration));
    }

    IEnumerator FadeImage(Graphic image, float startAlpha, float endAlpha, float duration) // Image에서 Graphic으로 변경하여 Image와 RawImage 모두 지원
    {
        float elapsedTime = 0f;
        Color color = image.color;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = new Color(color.r, color.g, color.b, endAlpha);
    }

    IEnumerator FadeInAndScaleUp(Image fadeInImage, RawImage fadeInScaleImage) // 변경된 부분
    {
        float elapsedTime = 0f;
        Color fadeInColor = fadeInImage.color;
        Color scaleColor = fadeInScaleImage.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeInImage.color = new Color(fadeInColor.r, fadeInColor.g, fadeInColor.b, alpha);
            fadeInScaleImage.color = new Color(scaleColor.r, scaleColor.g, scaleColor.b, alpha);

            float scale = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeInImage.transform.localScale = new Vector3(scale, scale, 1f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeInImage.color = new Color(fadeInColor.r, fadeInColor.g, fadeInColor.b, 1f);
        fadeInScaleImage.color = new Color(scaleColor.r, scaleColor.g, scaleColor.b, 1f);
        fadeInImage.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    IEnumerator FadeImagesSimultaneously(Graphic image1, Graphic image2, float startAlpha, float endAlpha, float duration) // Image에서 Graphic으로 변경하여 Image와 RawImage 모두 지원
    {
        float elapsedTime = 0f;
        Color color1 = image1.color;
        Color color2 = image2.color;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            image1.color = new Color(color1.r, color1.g, color1.b, alpha);
            image2.color = new Color(color2.r, color2.g, color2.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image1.color = new Color(color1.r, color1.g, color1.b, endAlpha);
        image2.color = new Color(color2.r, color2.g, color2.b, endAlpha);
    }
}
