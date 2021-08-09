using UnityEngine;
using System.IO.Ports;
using TMPro;
using System.Collections.Generic;
public class ComDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    string[] ports = SerialPort.GetPortNames();

    private void Start() {
        UpdateComList();
    }
    public void UpdateComList()
    {
        dropdown.ClearOptions();
        ports = SerialPort.GetPortNames();
        dropdown.AddOptions(new List<string> (ports));
    }
}
