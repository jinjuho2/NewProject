using UnityEngine;
using Data.Contents;

public interface ISaveSection
{
    public void Capture(ref SaveData data);
}
