using Events;
using UnityEngine;

public class AnimalBehaviour : MonoBehaviour
{
    [SerializeField] public Animal animalData;

    public void OnAnimalDroped()
    {
       EventManager.Dispatch(ENUM_SFXEvent.animalSound,animalData.animalSFX);
    }
}