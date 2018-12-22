using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for all things related to graves in the game
/// </summary>
public class GraveGenerator : MonoBehaviour
{

    Text epitaph;

    /// <summary>
    /// Writes out the text on the gravestone
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_birthDate"></param>
    /// <param name="_deathDate"></param>
    /// <param name="_cause"></param>
    public void WriteEpitaph(string _name, string _birthDate, string _deathDate, string _cause)
    {
        epitaph = GetComponentInChildren<Text>();
        epitaph.text = string.Empty;

        // Debug.Log("Here Lies: " + _name + " Date: " + _birthDate + " - " + _deathDate + " Cause: " + _cause);

        epitaph.text = string.Format
            ("Here Lies: \n" +
            "{0} \n" +
            "{1:0000} " + "-" + " {2:0000} \n " +
            "Cause: \n" +
            "{3} ",
            _name, _birthDate, _deathDate, _cause);
    }
}
