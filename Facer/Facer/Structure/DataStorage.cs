using System;
using System.Collections.Generic;
using System.Text;

namespace Facer.Structure
{
    public interface IDataStorage
    {
        bool SaveData(Data d);
        Data LoadData();
    }
}
