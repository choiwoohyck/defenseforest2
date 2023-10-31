using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
using Unity.Burst.CompilerServices;
using UnityEngine.EventSystems;

public class Element : MonoBehaviour
{
    // Start is called before the first frame update

   
    public ElementType elementType;
    public bool isBuilding = false;
    public bool isDie = false;
    public bool isRoad = false;
    Color originColor;

    SpriteRenderer renderer;
    Animator animator;
    GameObject buildBorder;

    public GameObject buildBorderPrefab;
    public Vector2 tileMapPos;
    public int price = 100;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        originColor = renderer.material.color;
        buildBorder = Instantiate(buildBorderPrefab, transform.position, Quaternion.identity);
        animator = GetComponent<Animator>();
    }

    public void init(ElementType type)
    {
        elementType = type;
        if (type == ElementType.FIRE)
            gameObject.GetComponent<UnitInfo>().StatusInit(100, 0.5f, 25);
        if (type == ElementType.ICE)
            gameObject.GetComponent<UnitInfo>().StatusInit(100, 0.1f, 20);
        if (type == ElementType.ICEROAD)
            gameObject.GetComponent<UnitInfo>().StatusInit(100, 0.5f, 20);
        if (type == ElementType.LEAF)
            gameObject.GetComponent<UnitInfo>().StatusInit(100, 1.5f, 100);
        if (type == ElementType.LEAFROAD)
        {
            gameObject.GetComponent<UnitInfo>().StatusInit(50, 2.5f, 50);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBuilding)
        {
            Build();
        }

        if (gameObject.GetComponent<UnitInfo>().hp <= 0 && !isDie)
        {
            gameObject.transform.GetChild(0).GetComponent<ElementAttack>().isBuilding = false;
            isDie = true;
        }

        if (isDie)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, renderer.color.a - Time.deltaTime * 3f);
            if (renderer.color.a<= 0)
            {
                StartCoroutine("Die");
            }    
        }
    }

    void Build()
    {
        Color c = Color.green;
        c.a = 0.2f;
        
        renderer.material.color = c;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = TileMapManager.instance.mapPostion() + new Vector3(0.5f, 0.5f);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      
            if (Input.GetMouseButtonDown(0))
            {

                if (TileMapManager.instance.isBuild() && AllyUnitManager.instance.isBuild)
                {
                if (elementType == ElementType.STONE && TileMapManager.instance.isRoad()) return; 

                    transform.position = TileMapManager.instance.mapPostion() + new Vector3(0.5f, 0.5f);
                    isBuilding = true;

                if (elementType != ElementType.STONE)
                {
                    transform.GetChild(0).gameObject.GetComponent<ElementAttack>().isBuilding = true;
                    transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                    GetComponent<CreateEnergyComponent>().startGathering = true;

                    renderer.material.color = originColor;
                 

                    int x = (int)TileMapManager.instance.mapPostion().x + 13;
                    int y = (int)(14 - TileMapManager.instance.mapPostion().y);

                    tileMapPos = new Vector2(x, y);

                    TileMapManager.instance.changeBuildable(x, y);
                    if (TileMapManager.instance.isRoad())
                    {
                        if (elementType == ElementType.ICE)
                        {
                            elementType = ElementType.ICEROAD;
                            animator.SetBool("isRoad", true);

                        }

                        if (elementType == ElementType.LEAF)
                            elementType = ElementType.LEAFROAD;



                        AllyUnitManager.instance.allyUnits.Add(gameObject);
                            isRoad = true;
                    }

                if (elementType != ElementType.STONE)
                {
                    init(elementType);
                    transform.GetChild(0).gameObject.GetComponent<ElementAttack>().damage = GetComponent<UnitInfo>().damage;
                }

                    AllyUnitManager.instance.noBuildElements.Remove(gameObject);
                    AllyUnitManager.instance.alreadyClick = false;
                    AllyUnitManager.instance.elementUnits.Add(gameObject);
                    AudioManager.instance.PlayOnShotSFX(3);
                    Destroy(buildBorder);
                }
            }

        if (Input.GetMouseButtonDown(1))
        {
            GameManager.instance.energy += price;
            AllyUnitManager.instance.noBuildElements.Remove(gameObject);
            AllyUnitManager.instance.alreadyClick = false;
            AudioManager.instance.PlayOnShotSFX(2);

            Destroy(buildBorder);
            Destroy(gameObject);
        }
            
    }

    IEnumerator Die()
    {
        TileMapManager.instance.changeBuildable((int)tileMapPos.x, (int)tileMapPos.y);
        AllyUnitManager.instance.allyUnits.Remove(gameObject);
        AllyUnitManager.instance.elementUnits.Remove(gameObject);


        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
    }
}
