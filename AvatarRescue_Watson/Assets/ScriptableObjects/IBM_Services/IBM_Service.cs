using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "IBM Serice", menuName = "IBM/Services", order = 1)]
public class IBM_Service : ScriptableObject
{
    public string ServiceURL = "";
    public string APIKey = "";
    public string workspaceID = "";
    public string versionDate = "";
}