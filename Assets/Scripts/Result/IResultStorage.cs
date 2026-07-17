using System.Collections.Generic;

public interface IResultStorage
{
    void Save(IReadOnlyList<string> winners);
}