using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class DrawFillController : MonoBehaviour
{
    private Tween tweenDrawFill;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float timeFill;

    [SerializeField] private string property;

    [SerializeField] private float startValue;

    [SerializeField] private float endValue;

    private MaterialPropertyBlock _propBlock;

    public void OnDraw(Color color, Sprite sprite, Vector3 currentPosition, int sortingOrder, Vector3 scale)
    {
        _propBlock = new MaterialPropertyBlock();

        spriteRenderer.sprite = sprite;

        spriteRenderer.color = color;

        spriteRenderer.sortingOrder = sortingOrder;

        transform.position = currentPosition;

        transform.localScale = scale;

        spriteRenderer.GetPropertyBlock(_propBlock);

        // Assign our new value.

        _propBlock.SetFloat(property, startValue);

        // Apply the edited values to the renderer.

        spriteRenderer.SetPropertyBlock(_propBlock);

        spriteRenderer.enabled = true;

        if(tweenDrawFill != null)
        {
            tweenDrawFill.Kill();
        }

        tweenDrawFill = DOTween.To((x) =>
        {

            spriteRenderer.GetPropertyBlock(_propBlock);

            // Assign our new value.

            _propBlock.SetFloat(property, x);

            // Apply the edited values to the renderer.

            spriteRenderer.SetPropertyBlock(_propBlock);

        }, startValue, endValue, timeFill).OnComplete(() => 
        {
            tweenDrawFill = null;

            spriteRenderer.enabled = false;
        });
    }
}