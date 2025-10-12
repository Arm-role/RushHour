using System.Collections;
using UnityEngine;

public interface IWorkStation 
{
   void OnStart(Station station);
    void OnUpdate(Station station);
    void OnRecieveExternalInput(Station station);
    bool IsComplete(Station station);
    void OnComplete(Station station);
    bool IsCancel(Station station);
    void OnCancel(Station station);

}
