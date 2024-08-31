using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    [SerializeField] List<PlayerAttributes> characters;

    private void Awake()
    {
        foreach (var character in characters)
        {
            character.Init();
        }
    }

    public PlayerAttributes GetHealthyCharacter()
    {
        return characters.Where(x => x.HP > 0).FirstOrDefault();
    }

    public List<PlayerAttributes> Characters
    {
        get { return characters; }
    }
}
