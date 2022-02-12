using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Sprites
{
    public string back_default;
    public Sprites(string back_default)
    {
        this.back_default = back_default;
    }

}


public class DataSpritePokemons
{
    public int id;
    public Sprites sprites;
    public DataSpritePokemons(int id, Sprites sprites)
    {
        this.id = id;
        this.sprites = sprites;
    }

}

