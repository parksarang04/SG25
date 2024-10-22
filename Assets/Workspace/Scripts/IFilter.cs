using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFilter<T>
{
    T ApplyFilter(T input);
}
