using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UiCollectables : MonoBehaviour
{
    [Serializable]
    private class ValueThingy
    {
        public string Key;
        public TextMeshProUGUI Text;

        [NonSerialized]
        public Coroutine CurrentRoutine;
        [NonSerialized]
        public int ShownValue;
        [NonSerialized]
        public SavedInt SavedInt;
        [NonSerialized]
        public Action<int> Animate;
    }

    [SerializeField]
    private ValueThingy[] _tracked;

    private void Awake()
    {
        var savefile = SaveManager.GetSave();
        for (var i = 0; i < _tracked.Length; i++)
        {
            var index = i;
            var savedInt = savefile.SavedInts[_tracked[i].Key];
            _tracked[i].SavedInt = savedInt;
            _tracked[i].Text.text = savedInt.Value.ToString();
            Action<int> animate = newValue => AnimateText(index, newValue);
            _tracked[i].Animate = animate;
            savedInt.OnValueChanged += animate;
        }
    }

    private void OnDestroy()
    {
        for (var i = 0; i < _tracked.Length; i++)
        {
            _tracked[i].SavedInt.OnValueChanged -= _tracked[i].Animate;
            if(_tracked[i].CurrentRoutine != null)
            {
                StopCoroutine(_tracked[i].CurrentRoutine);
                _tracked[i].CurrentRoutine = null;
            }
        }
    }

    private void AnimateText(int index, int newValue)
    {
        var valueThingy = _tracked[index];
        if(valueThingy.CurrentRoutine != null)
        {
            StopCoroutine(valueThingy.CurrentRoutine);
        }

        valueThingy.CurrentRoutine = StartCoroutine(AnimateTextRoutine(valueThingy, newValue));
    }

    private IEnumerator AnimateTextRoutine(ValueThingy valueThingy, int to)
    {
        var from = valueThingy.ShownValue;
        for(var t = 0.0f; t<1f; t+=Time.deltaTime * 5)
        {
            var value = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
            valueThingy.ShownValue = value;
            valueThingy.Text.text = value.ToString();
            yield return null;
        }
        valueThingy.ShownValue = to;
        valueThingy.Text.text = to.ToString();
        valueThingy.CurrentRoutine = null;
    }
}
