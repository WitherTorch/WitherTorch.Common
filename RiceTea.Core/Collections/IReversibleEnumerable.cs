using System.Collections.Generic;

namespace RiceTea.Core.Collections;

public interface IReversibleEnumerable<T> : IEnumerable<T>
{
    IEnumerator<T> GetReversedEnumerator();
}
