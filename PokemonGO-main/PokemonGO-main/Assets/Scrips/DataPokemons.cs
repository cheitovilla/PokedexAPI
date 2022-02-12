using System.Collections.Generic;

[System.Serializable]
public class Result
{
    public string name;
    public string url;

    public Result(string name, string url)
    {
        this.name = name;
        this.url = url;
    }
}

public class DataPokemons
{
    public int count;
    public string next;
    public string previous;
    public List<Result> results;

    public DataPokemons(int count, string next, string previous, List<Result> results)
    {
        this.count = count;
        this.next = next;
        this.previous = previous;
        this.results = results;
    }
}
