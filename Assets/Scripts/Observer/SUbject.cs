using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class SUbject
{
    private List<Observer> obsvector = new List<Observer>();

    public void AddObserver(Observer o) 
    {
        this.obsvector.Add(o);
    }

    public void DelObserver(Observer o) 
    {
        this.obsvector.Remove(o);
    }

    public void NotifyObservers() 
    {
        for (int i = 0; i < obsvector.Count; i++)
        {
            obsvector[i].update();
        }
    }

}
