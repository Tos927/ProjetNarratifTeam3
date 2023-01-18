using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogDatabase", menuName = "Database/Dialog", order = 1)]
public class DialogDatabase : ScriptableObject
{
    public List<DialogData> dialogDatas = new();
}
