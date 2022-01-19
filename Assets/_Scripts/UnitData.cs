using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : MonoBehaviour
{
    [SerializeField]
    private UnitDataSO data;

    // Zeby dane mozna bylo wpisywac w panelu Unity, poza kodem
    public UnitDataSO Data
    {
        get { return data; }
    }
}
