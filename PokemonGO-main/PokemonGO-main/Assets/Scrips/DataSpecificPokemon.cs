using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Ability2
{
    public string name;
    public string url;
    public Ability2(string name, string url)
    {
        this.name = name;
        this.url = url;
    }
}
public class Ability
{
    public Ability2 ability2;
    public bool is_hidden;
    public int slot;
    public Ability(Ability2 ability2, bool is_hidden, int slot)
    {
        this.ability2 = ability2;
        this.is_hidden = is_hidden;
        this.slot = slot;
    }
}


public class Type
{
    public string name;
    public string url;
    public Type(string name, string url)
    {
        this.name = name;
        this.url = url;
    }
}

public class Types
{
    public Type type;
    public Types(Type type)
    {
        this.type = type;
    }
}

public class DataSpecificPokemon
{
    public List<Ability> abilities;
    public int base_experience;
    public int height;
    public int id;
    public string location_area_encounters;
    public string name;
    public List<Type> type;
    public int weight;

    public DataSpecificPokemon(List<Ability> abilities, List<Ability2> abilities2, int base_experience, int height, int id,
                               string location_area_encounters, string name, List<Type> type, int weight)
    {
        this.abilities = abilities;
        this.base_experience = base_experience;
        this.height = height;
        this.id = id;
        this.location_area_encounters = location_area_encounters;
        this.name = name;
        this.type = type;
        this.weight = weight;
    }
}
