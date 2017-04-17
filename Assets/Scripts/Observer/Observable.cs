using UnityEngine;
using System.Collections;

public interface Observable 
{
    void addObserver(Observer observer);
    void deletObserver(Observer observer);
}
