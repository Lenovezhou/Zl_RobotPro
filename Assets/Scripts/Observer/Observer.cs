using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface Observer
{
    void update();
    void notifyobservers(string context);
}
