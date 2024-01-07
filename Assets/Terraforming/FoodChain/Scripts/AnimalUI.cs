using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimalUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Camera mainCamera;

    [Header("Animal Data")]
    [SerializeField] public Animal animal;
    [SerializeField] Image UiImage;
    public GameObject prefabToSpawn; // Reference to the prefab

    [Header("CurrentPrefab Data")]
    private bool isDragging = false;
    private GameObject spawnedPrefab; // The instance of the spawned prefab
    private bool canDrop;

    //TODO: Add fields for animal Data, and load the reference prefab from the animal data
    //TODO: Add the drop function taking in account the raycasting
    void Start()
    {
        mainCamera = Camera.main;
    }

    public void SetAnimal(Animal _animal)
    {
        animal = _animal;
        SetAnimalValues();
    }

    private void SetAnimalValues()
    {
        UiImage.sprite = animal.sprite;
        prefabToSpawn = animal._3dPrefab;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Spawn the prefab at the cursor position
        spawnedPrefab = Instantiate(prefabToSpawn, GetMouseWorldPosition(eventData), Quaternion.identity);

        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the spawned prefab based on the cursor movement
        if (isDragging)
        {
            spawnedPrefab.transform.position = GetMouseWorldPosition(eventData);
        }
    }
    /// <summary>
    /// Clean the spawned prefab if the drop is not valid
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        if(!canDrop)
        {
            Destroy(spawnedPrefab);
        }
        
        canDrop = false;
        spawnedPrefab = null;
    }

    private Vector3 GetMouseWorldPosition(PointerEventData eventData)
    {
        Ray ray = mainCamera.ScreenPointToRay(eventData.position);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            DominoPole pole = hit.transform.GetComponent<DominoPole>();
            canDrop = pole!=null? pole.CheckBiome(animal.biome) : false;

            return hit.point;
        }

        Debug.LogWarning("No hit", spawnedPrefab);
        return Vector3.zero; // Default if no hit
    }
}