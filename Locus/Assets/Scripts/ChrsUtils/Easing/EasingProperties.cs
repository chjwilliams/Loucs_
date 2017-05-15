using UnityEngine;
using ChrsUtils.EasingEquations;

[CreateAssetMenu (menuName = "Easing Properties")]
public class EasingProperties : ScriptableObject
{
    [SerializeField] private Easing.FunctionType _FadeIn;
    [SerializeField] private Easing.FunctionType _FadeOut;
    public Easing.Function FadeInEasing { get { return Easing.GetFunctionWithTypeEnum(_FadeIn); }}
    public Easing.Function FadeOutEasing { get { return Easing.GetFunctionWithTypeEnum(_FadeOut); }}
}
