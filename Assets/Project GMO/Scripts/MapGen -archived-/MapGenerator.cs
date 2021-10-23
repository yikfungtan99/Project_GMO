using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Cell
{
    public int posX;
    public int posY;

    public GameObject cellObject;
 
    public Vector2Int cellPos { get => new Vector2Int(posX, posY); }

    public List<Road> road;

    public Cell(int posX, int posY, GameObject cell)
    {
        this.posX = posX;
        this.posY = posY;
        this.cellObject = cell;
    }
}

[System.Serializable]
public struct BuildingTier
{
    public string tierName;
    public int tierMin;
    public int tierMax;

    public bool unlimited;
}

public class MapGenerator : MonoBehaviour
{

    [SerializeField] private int seed;

    //=======================================================Grid======================================================
    [Header("Grid Generation")]
    [SerializeField] private int gridSize;

    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    [SerializeField] private GameObject cellPrefab;

    private List<Cell> cellList = new List<Cell>();
    private List<Cell> edgeCells = new List<Cell>();
    private List<Cell> cornerCells = new List<Cell>();

    //=====================================================NavMesh================================================
    [SerializeField] private NavMeshSurface navMesh;

    //====================================================Map Properties===================================================
    [Header("Road Generation")]
    [SerializeField] private bool generateRoad = false;

    [Range(0, 15)]
    [SerializeField] private int wayPointNodesCount = 0;

    [Range(0, 8)]
    [SerializeField] private int minNodeRange;

    [SerializeField] private RoadStorage roadStorage;

    [Header("Start Finish Setting")]
    [SerializeField] private int exitsNumber;
    private Cell startCell;
    private List<Cell> exitCells = new List<Cell>();

    [SerializeField] private int startToExitMinDistance;

    [Header("Building Generation")]
    [SerializeField] private bool generateBuilding = false;
    [SerializeField] private BuildingStorage buildingStorage;
    [SerializeField] private int failThreshold = 100;

    [Header("Generation Setting")]
    [SerializeField] private List<BuildingTier> buildingTierList;

    //=======================================================Road======================================================

    Cell walker = null;
    Cell walkerLastCell = null;
    BuildDirection lastWalkerDirection = BuildDirection.NORTH;

    bool xFirst = false;

    private List<Cell> wayPointNodes = new List<Cell>();
    private List<Cell> roadCells = new List<Cell>();
    private List<Cell> intersectionCells = new List<Cell>();
    private List<Cell> buildableCells = new List<Cell>();

    //======================================================Buidling====================================================
    private List<Cell> roadsideArea = new List<Cell>();

    [SerializeField] int[] numberofBuildings = { 0, 0, 0 };
    [SerializeField] int[] currentNumOfBuilding = { 0, 0, 0 };

    //=====================================================Map Legend Color================================================

    [SerializeField] private Color startColor;
    [SerializeField] private Color connectionColor;
    [SerializeField] private Color roadColor;
    [SerializeField] private Color intersectionColor;
    [SerializeField] private Color roadsideColor;
    [SerializeField] private Color buildingColor;
    [SerializeField] private Color buildingPivotColor;
    [SerializeField] private Color edgeColor;
    [SerializeField] private Color exitColor;

    float timeStart = 0;

    //public override void OnStartServer()
    //{
    //    SetSeed(Random.Range(0, 999));
    //    base.OnStartServer();
    //}
    //public override void OnStartClient()
    //{
    //    base.OnStartClient();

    //    Random.InitState(seed);
    //    GenerateMap();
    //}

    private void Start()
    {
        SetSeed(Random.Range(0, 999));
        Random.InitState(seed);
        GenerateMap();
    }

    public void SetSeed(int seed)
    {
        this.seed = seed;
    }

    public void GenerateMap()
    {
        timeStart = Time.realtimeSinceStartup;

        GenerateGrid();

        SpawnStartCell();

        SpawnExitCells();

        GenerateRoad();

        PaintLegends();

        if (generateBuilding) GenerateBuilding();

        BuildNavMesh();

        //RemoveEmptyCell();

        print("Took " + (Time.realtimeSinceStartup - timeStart) + " to load map");
    }

    private void BuildNavMesh()
    {
        navMesh.buildHeightMesh = true;
        navMesh.BuildNavMesh();
    }

    private void RemoveEmptyCell()
    {
        foreach (var item in cellList)
        {
            Cell target = item;
            if (!buildableCells.Contains(target))
            {
                Destroy(target.cellObject);
            }
        }
    }

    #region grid generation
    void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject cellObject = Instantiate(cellPrefab, transform);

                cellObject.name = "x: " + x + "y: " + y;

                Cell cellInstance = new Cell(x, y, cellObject);

                cellList.Add(cellInstance);

                buildableCells.Add(cellInstance);

                if (x == 0)
                {
                    if (y != 0 && y != gridSize - 1)
                    {
                        if (!edgeCells.Contains(cellInstance))
                        {
                            edgeCells.Add(cellInstance);
                        }
                    }
                    else
                    {
                        if (!cornerCells.Contains(cellInstance))
                        {
                            cornerCells.Add(cellInstance);
                            buildableCells.Remove(cellInstance);
                        }
                    }

                }
                else if (x == gridSize - 1)
                {
                    if (y != gridSize - 1 && y != 0)
                    {
                        if (!edgeCells.Contains(cellInstance))
                        {
                            edgeCells.Add(cellInstance);
                        }
                    }
                    else
                    {
                        if (!cornerCells.Contains(cellInstance))
                        {
                            cornerCells.Add(cellInstance);
                            buildableCells.Remove(cellInstance);
                        }
                    }
                }

                if (y == 0)
                {
                    if (x != 0 && x != gridSize - 1)
                    {
                        if (!edgeCells.Contains(cellInstance))
                        {
                            edgeCells.Add(cellInstance);
                        }
                    }
                    else
                    {
                        if (!cornerCells.Contains(cellInstance))
                        {
                            cornerCells.Add(cellInstance);
                            buildableCells.Remove(cellInstance);
                        }
                    }
                }
                else if (y == gridSize - 1)
                {
                    if (x != gridSize - 1 && x != 0)
                    {
                        if (!edgeCells.Contains(cellInstance))
                        {
                            edgeCells.Add(cellInstance);
                        }
                    }
                    else
                    {
                        if (!cornerCells.Contains(cellInstance))
                        {
                            cornerCells.Add(cellInstance);
                            buildableCells.Remove(cellInstance);
                        }
                    }
                }

                Vector3 cellPosition = new Vector3(x + x * offsetX, 0, y + y * offsetY);

                cellObject.transform.position = cellPosition;
            }
        }
    }

    #endregion

    #region start exit generation
    private void SpawnStartCell()
    {
        Cell actualStart = RandomEdgeCell();
        BuildDirection dir = BuildDirection.NORTH;

        foreach (Cell target in GetNeighbourCellExcludeEdge(actualStart))
        {
            startCell = target;
            break;
        }

        for (int i = 0; i < GetNeighbourCell(startCell).Length; i++)
        {
            if(GetNeighbourCell(startCell)[i] == actualStart)
            {
                switch (i)
                {
                    case 0:

                        dir = BuildDirection.NORTH;
                        break;

                    case 1:

                        dir = BuildDirection.EAST;
                        break;

                    case 2:

                        dir = BuildDirection.NORTH;
                        break;

                    case 3:

                        dir = BuildDirection.EAST;
                        break;
                }
            }
        }

        LayRoad(actualStart, dir);
        wayPointNodes.Add(startCell);
        AddIntersectionCell(startCell);
    }

    private void SpawnExitCells()
    {
        for (int i = 0; i < exitsNumber; i++)
        {
            Cell actualExit;
            Cell targetCell = null;
            BuildDirection dir = BuildDirection.NORTH;

            do
            {
                actualExit = RandomEdgeCell();
                foreach (Cell target in GetNeighbourCellExcludeEdge(actualExit))
                {
                    targetCell = target;
                    break;
                }

            }
            while (GetDistance(startCell, targetCell) < startToExitMinDistance || exitCells.Contains(targetCell) || SpawnRequirement(targetCell));

            for (int u = 0; u < GetNeighbourCell(targetCell).Length; u++)
            {
                if (GetNeighbourCell(targetCell)[u] == actualExit)
                {
                    switch (u)
                    {
                        case 0:

                            dir = BuildDirection.NORTH;
                            break;

                        case 1:

                            dir = BuildDirection.EAST;
                            break;

                        case 2:

                            dir = BuildDirection.NORTH;
                            break;

                        case 3:

                            dir = BuildDirection.EAST;
                            break;
                    }
                }
            }

            LayRoad(actualExit, dir);
            exitCells.Add(targetCell);
            wayPointNodes.Add(targetCell);
            AddIntersectionCell(targetCell);
        }
    }

    private Cell NearEdgeCell()
    {
        Cell targetCell = null;
        Cell selectedStartingCell = RandomEdgeCell();

        foreach (Cell target in GetNeighbourCellExcludeEdge(selectedStartingCell))
        {
            targetCell = target;
            break;
        }

        return targetCell;
    }

    #endregion

    #region road generation
    private void GenerateRoad()
    {
        //Generate Connect Nodes Cell
        GenerateConnectNodes();
        ConnectNodes();
        LayIntersection();
        LayRoadSide();
    }

    private void GenerateConnectNodes()
    {
        for (int i = 0; i < wayPointNodesCount; i++)
        {

            Cell randNode = GetRandomCellExcludeEdge();

            while (SpawnRequirement(randNode))
            {
                randNode = GetRandomCellExcludeEdge();
            }

            wayPointNodes.Add(randNode);
        }
    }

    private bool SpawnRequirement(Cell targetNode)
    {
        bool haveNearbyNode = false;

        for (int i = 0; i < wayPointNodes.Count; i++)
        {
            Vector2Int targetNodePos = targetNode.cellPos;

            Vector2Int otherNodePos = wayPointNodes[i].cellPos;

            if (Vector2Int.Distance(targetNode.cellPos, otherNodePos) < minNodeRange)
            {
                haveNearbyNode = true;
            }

            if (targetNodePos.x - 1 == otherNodePos.x || targetNodePos.x + 1 == otherNodePos.x || targetNodePos.y - 1 == otherNodePos.y || targetNodePos.y + 1 == otherNodePos.y)
            {
                haveNearbyNode = true;
            }
        }

        return haveNearbyNode;
    }

    private void ConnectNodes()
    {
        int currentConnectIndex = 1;

        xFirst = Random.value > 0.5f;

        walker = wayPointNodes[0];
        walkerLastCell = walker;

        while (walker != wayPointNodes[currentConnectIndex])
        {
            MoveWalker(walker, wayPointNodes[currentConnectIndex], xFirst);

            //Reached a node
            if (walker == wayPointNodes[currentConnectIndex])
            {
                currentConnectIndex += 1;
                xFirst = Random.value > 0.5f;
            }

            //Finished all node
            if (currentConnectIndex == wayPointNodes.Count)
            {
                return;
            }
        }
    }

    private void MoveWalker(Cell walker, Cell end, bool xFirst)
    {
        if (xFirst)
        {
            if (walker.posX < end.posX)
            {
                LayRoadOnWalker(walker, GetNeighbourCell(walker)[1]);
            }
            else if (walker.posX > end.posX)
            {
                LayRoadOnWalker(walker, GetNeighbourCell(walker)[3]);
            }
            else if (walker.posY < end.posY)
            {
                LayRoadOnWalker(walker, GetNeighbourCell(walker)[0]);
            }
            else if (walker.posY > end.posY)
            {
                LayRoadOnWalker(walker, GetNeighbourCell(walker)[2]);
            }
        }
        else
        {
            if (walker.posY < end.posY)
            {
                LayRoadOnWalker(walker, GetNeighbourCell(walker)[0]);
            }
            else if (walker.posY > end.posY)
            {
                LayRoadOnWalker(walker, GetNeighbourCell(walker)[2]);
            }
            else if (walker.posX < end.posX)
            {
                LayRoadOnWalker(walker, GetNeighbourCell(walker)[1]);
            }
            else if (walker.posX > end.posX)
            {
                LayRoadOnWalker(walker, GetNeighbourCell(walker)[3]);
            }
        }
    }

    private void LayRoadOnWalker(Cell walker, Cell current)
    {
        BuildDirection dir = BuildDirection.NORTH;

        this.walker = current;

        if (walkerLastCell != null)
        {
            if (walkerLastCell.posX > walker.posX || walkerLastCell.posX < walker.posX)
            {
                dir = BuildDirection.EAST;
            }

            if (walkerLastCell.posY > walker.posY || walkerLastCell.posY < walker.posY)
            {
                dir = BuildDirection.NORTH;
            }
        }

        if(lastWalkerDirection != dir)
        {
            AddIntersectionCell(walkerLastCell);
            lastWalkerDirection = dir;
        }

        LayRoad(walker, dir);

        walkerLastCell = walker;
    }

    private void LayRoad(Cell road, BuildDirection dir)
    {
        GameObject roadInstance = null;
       
        if (roadCells.Contains(road))
        {
            if (road.cellObject.GetComponentInChildren<Road>())
            {
                if(road.cellObject.GetComponentInChildren<Road>().dir != dir)
                {
                    List<GameObject> childRoad = new List<GameObject>();

                    AddIntersectionCell(road);
                }
            }
        }
        else
        {
            roadCells.Add(road);

            if (generateRoad)
            {
                road.cellObject.transform.position += new Vector3(0, -0.35f, 0);
                roadInstance = Instantiate(roadStorage.road, road.cellObject.transform);
            }
        }

        if (roadInstance != null)
        {
            switch (dir)
            {
                case BuildDirection.NORTH:
                    roadInstance.GetComponent<Road>().dir = dir;
                    break;

                case BuildDirection.EAST:

                    roadInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    roadInstance.GetComponent<Road>().dir = dir;
                    break;
            }
        }

        if (buildableCells.Contains(road)) buildableCells.Remove(road);
    }

    private void LayRoadSide()
    {
        foreach (Cell roadCell in roadCells)
        {
            foreach (Cell roadNeighbour in GetNeighbourCellExcludeEdge(roadCell))
            {
                if (!roadCells.Contains(roadNeighbour)) roadsideArea.Add(roadNeighbour);
            }
        }
    }

    private void LayIntersection()
    {
        foreach (Cell intersectionCell in intersectionCells)
        {

            Cell[] neighbour = GetNeighbourCell(intersectionCell);
            List<int> neighbouringRoad = new List<int>();
            BuildDirection dir = BuildDirection.NORTH;

            GameObject roadToSpawn = null;

            Vector3 intersectionRotation = new Vector3();

            int roadCount = 0;

            for (int i = 0; i < neighbour.Length; i++)
            {
                if (roadCells.Contains(neighbour[i]))
                {
                    roadCount++;
                    neighbouringRoad.Add(i);
                }
            }

            switch (roadCount)
            {
                case 2:

                    roadToSpawn = roadStorage.LRoad;

                    switch (neighbouringRoad[0])
                    {
                        case 0:

                            switch (neighbouringRoad[1])
                            {
                                case 1:

                                    intersectionRotation = new Vector3(0, 180, 0);
                                    break;

                                case 2:

                                    roadToSpawn = roadStorage.road;
                                    intersectionRotation = new Vector3(0, 0, 0);
                                    break;

                                case 3:

                                    intersectionRotation = new Vector3(0, 90, 0);
                                    break;
                            }

                            break;

                        case 1:

                            switch (neighbouringRoad[1])
                            {
                                case 0:

                                    intersectionRotation = new Vector3(0, 180, 0);
                                    break;

                                case 2:

                                    intersectionRotation = new Vector3(0, 270, 0);
                                    break;

                                case 3:

                                    roadToSpawn = roadStorage.road;
                                    intersectionRotation = new Vector3(0, 90, 0);
                                    break;
                            }

                            break;

                        case 2:

                            switch (neighbouringRoad[1])
                            {
                                case 0:

                                    roadToSpawn = roadStorage.road;
                                    intersectionRotation = new Vector3(0, 0, 0);
                                    break;

                                case 1:

                                    intersectionRotation = new Vector3(0, 270, 0);
                                    break;

                                case 3:

                                    intersectionRotation = new Vector3(0, 0, 0);
                                    break;
                            }

                            break;

                        case 3:

                            switch (neighbouringRoad[1])
                            {
                                case 0:

                                    intersectionRotation = new Vector3(0, 90, 0);
                                    break;

                                case 1:

                                    roadToSpawn = roadStorage.road;
                                    intersectionRotation = new Vector3(0, 90, 0);
                                    break;

                                case 2:

                                    intersectionRotation = new Vector3(0, 0, 0);
                                    break;
                            }

                            break;
                    }

                    
                    break;

                case 3:

                    roadToSpawn = roadStorage.TRoad;

                    int tTurnNum = 0;

                    for (int i = 0; i < neighbouringRoad.Count; i++)
                    {
                        tTurnNum += neighbouringRoad[i];
                    }

                    switch (tTurnNum)
                    {
                        case 4:

                            intersectionRotation = new Vector3(0, 270, 0);
                            dir = BuildDirection.NORTH;
                            break;

                        case 3:

                            intersectionRotation = new Vector3(0, 0, 0);
                            dir = BuildDirection.EAST;
                            break;

                        case 6:

                            intersectionRotation = new Vector3(0, 90, 0);
                            dir = BuildDirection.SOUTH;
                            break;


                        case 5:

                            intersectionRotation = new Vector3(0, 180, 0);
                            dir = BuildDirection.WEST;
                            break;
                    }
                    break;

                case 4:

                    roadToSpawn = roadStorage.CrossRoad;
                    break;

            }

            if (roadToSpawn != null)
            {
                Destroy(intersectionCell.cellObject.transform.GetChild(0).gameObject);
                GameObject intersection = Instantiate(roadToSpawn, intersectionCell.cellObject.transform);
                intersection.transform.rotation = Quaternion.Euler(intersectionRotation);
                intersection.GetComponent<Road>().dir = dir;
            }
        }
    }

    void AddIntersectionCell(Cell cell)
    {
        if (intersectionCells.Contains(cell)) return;
        intersectionCells.Add(cell);
    }

    #endregion

    #region building generation

    void GenerateBuilding()
    {
        for (int i = buildingTierList.Count - 1; i >= 0; i--)
        {
            int failAttempt = 0;

            if (buildingTierList[i].unlimited)
            {
                while (currentNumOfBuilding[i] < roadsideArea.Count)
                {
                    if (PlaceBuilding(i))
                    {
                        currentNumOfBuilding[i] += 1;
                    }
                    else
                    {
                        failAttempt++;

                        if (failAttempt >= (currentNumOfBuilding[i] + 10) * failThreshold)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                numberofBuildings[i] = Random.Range(buildingTierList[i].tierMin, buildingTierList[i].tierMax);

                while (currentNumOfBuilding[i] < numberofBuildings[i])
                {
                    if (PlaceBuilding(i))
                    {
                        currentNumOfBuilding[i] += 1;
                    }
                    else
                    {
                        failAttempt++;

                        if (failAttempt >= (currentNumOfBuilding[i] + 10) * failThreshold)
                        {
                            break;
                        }
                    }
                }
            }

        }
    }

    private bool PlaceBuilding(int tier)
    {
        bool placed = false;

        Building buildingToSpawn = null;

        switch (tier)
        {
            case 0:

                buildingToSpawn = GetRandomBuilding(buildingStorage.tier1Buildings);
                break;

            case 1:

                buildingToSpawn = GetRandomBuilding(buildingStorage.tier2Buildings);
                break;

            case 2:

                buildingToSpawn = GetRandomBuilding(buildingStorage.tier3Buildings);
                break;
        }

        Cell cellToSpawnBuilding = GetRandomCellOnRoadSideArea();

        BuildDirection buildDir = CheckBuildingFaceDirection(cellToSpawnBuilding);

        Cell[] buildingBuildArea = GetBuildingBuildAreaCells(buildingToSpawn, cellToSpawnBuilding, buildDir);

        if (CheckIfBuildingCanFit(buildingBuildArea))
        {
            SpawnBuilding(buildingToSpawn, cellToSpawnBuilding, buildingBuildArea, buildDir);
            placed = true;
        }

        return placed;
    }

    void SpawnBuilding(Building build, Cell locCell, Cell[] buildArea, BuildDirection buildDir)
    {
        foreach (Cell buildingCell in buildArea)
        {
            buildableCells.Remove(buildingCell);
            PaintCell(buildingCell, buildingColor);
        }

        PaintCell(locCell, buildingPivotColor);

        Transform locCellTransform = locCell.cellObject.transform;
        Building buildInstance = Instantiate(build, locCellTransform);
        buildInstance.transform.position = new Vector3(locCellTransform.position.x, locCellTransform.position.y, locCellTransform.position.z);

        float buildingDirection = 0;

        switch (buildDir)
        {
            case BuildDirection.NORTH:

                buildingDirection = 0;
                break;

            case BuildDirection.EAST:

                buildingDirection = 90;
                break;

            case BuildDirection.SOUTH:

                buildingDirection = -180;
                break;

            case BuildDirection.WEST:

                buildingDirection = -90;
                break;

            default:
                break;
        }

        buildInstance.transform.rotation = Quaternion.Euler(0, buildingDirection, 0);
        buildInstance.dir = buildDir;
    }

    bool CheckIfBuildingCanFit(Cell[] buildArea)
    {
        bool canFit = true;

        for (int i = 0; i < buildArea.Length; i++)
        {
            if(buildArea[i] == null || !buildableCells.Contains(buildArea[i]))
            {
                canFit = false;
                break;
            }
        }

        return canFit;
    }

    Cell[] GetBuildingBuildAreaCells(Building build, Cell LocCell, BuildDirection dir)
    {
        Cell[] buildAreaCells = new Cell[build.size.x * build.size.y];

        Vector2Int checkDir = new Vector2Int(0, 0);

        int counter = 0;
      
        //Move Pivot

        Vector2Int pivot = Vector2Int.zero;

        //Find out how many x from pivot

        for (int i = 0; i < build.size.x; i++)
        {
            for (int u = 0; u < build.size.y; u++)
            {
                switch (dir)
                {
                    case BuildDirection.NORTH:

                        pivot = LocCell.cellPos + new Vector2Int(build.size.x / 2, 0);
                        checkDir = new Vector2Int(i * -1, u * -1);
                        break;

                    case BuildDirection.EAST:

                        pivot = LocCell.cellPos + new Vector2Int(0, -build.size.x / 2);
                        checkDir = new Vector2Int(u * -1, i * 1);
                        break;

                    case BuildDirection.SOUTH:

                        pivot = LocCell.cellPos + new Vector2Int(-build.size.x / 2, 0);
                        checkDir = new Vector2Int(i * 1, u * 1);
                        break;

                    case BuildDirection.WEST:

                        pivot = LocCell.cellPos + new Vector2Int(0, build.size.x / 2);
                        checkDir = new Vector2Int(u * 1, i * -1);
                        break;
                }

                Vector2Int cellPos = checkDir + pivot;

                buildAreaCells[counter] = GetCellOnPos(cellPos);

                counter++;
            }
        }

        return buildAreaCells;
    }

    BuildDirection CheckBuildingFaceDirection(Cell buildingCell)
    {
        BuildDirection buildDir = BuildDirection.NORTH;

        List<int> faceDir = new List<int>();

        for (int i = 0; i < GetNeighbourCell(buildingCell).Length; i++)
        {
            if (roadCells.Contains(GetNeighbourCell(buildingCell)[i]))
            {
                faceDir.Add(i);
            }
        }

        buildDir = (BuildDirection)faceDir[Random.Range(0, faceDir.Count)];

        return buildDir;
    }

    #endregion

    #region Util
    private Cell GetRandomCell()
    {
        return cellList[Random.Range(0, cellList.Count)];
    }

    private Cell GetRandomCellExcludeEdge()
    {
        Cell targetCell = null;

        do
        {
            targetCell = GetRandomCell();
        }
        while (edgeCells.Contains(targetCell) || cornerCells.Contains(targetCell));

        return targetCell;
    }

    private Cell GetRandomCellOnRoadSideArea()
    {
        Cell targetCell = null;

        targetCell = roadsideArea[Random.Range(0, roadsideArea.Count)];

        return targetCell;
    }

    private Cell RandomEdgeCell()
    {
        Cell targetCell = null;

        targetCell = edgeCells[Random.Range(0, edgeCells.Count)];

        return targetCell;
    }
  
    List<Cell> GetNeighbourCellExcludeEdge(Cell cell)
    {
        return GetNeighbourCellExcludeEdge(cell.posX, cell.posY);
    }

    List<Cell> GetNeighbourCellExcludeEdge(int x, int y)
    {
        List<Cell> neighbour = new List<Cell>();

        Vector2Int[] neighbourPos = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };

        for (int i = 0; i < neighbourPos.Length; i++)
        {

            Cell targetNeighbour = GetCellOnPos(x + neighbourPos[i].x, y + neighbourPos[i].y);

            if (targetNeighbour != null)
            {
                if (edgeCells.Contains(targetNeighbour) || cornerCells.Contains(targetNeighbour)) continue;
                neighbour.Add(GetCellOnPos(x + neighbourPos[i].x, y + neighbourPos[i].y));
            }
        }

        return neighbour;
    }

    Cell[] GetNeighbourCell(Cell cell)
    {
        return GetNeighbourCell(cell.posX, cell.posY);
    }

    Cell[] GetNeighbourCell(int x, int y)
    {
        Cell[] neighbour = new Cell[4];

        Vector2Int[] neighbourPos = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };

        for (int i = 0; i < neighbourPos.Length; i++)
        {
            Vector2Int targetCellPos = GetCellOnPos(x, y).cellPos + neighbourPos[i];

            Cell targetCell = GetCellOnPos(targetCellPos.x, targetCellPos.y);

            if (targetCell != null)
            {
                neighbour[i] = targetCell;
            }
        }

        return neighbour;
    }

    Cell GetCellOnPos(Vector2Int cellPos)
    {
        return GetCellOnPos(cellPos.x, cellPos.y);
    }

    Cell GetCellOnPos(int x, int y)
    {
        Cell targetCell = null;

        foreach (var item in cellList)
        {
            if(item.posX == x && item.posY == y)
            {
                targetCell = item;
                break;
            }
        }

        return targetCell;
    }

    int GetDistance(Cell start, Cell end)
    {
        int distance = (int) Vector2Int.Distance(new Vector2Int(end.posX, end.posY), new Vector2Int(start.posX, start.posY));

        return distance;
    }

    Building GetRandomBuilding(List<BuildingContainer> buildContainerList)
    {
        float initChance = 0;

        float random;

        float randomChance = 0;

        random = UnityEngine.Random.Range(0, totalChance(buildContainerList));

        Building targetBuilding = null;

        for (int i = 0; i < buildContainerList.Count; i++)
        {
            randomChance = buildContainerList[i].buildChance;

            if (random >= initChance && random < initChance + randomChance)
            {
                targetBuilding = buildContainerList[i].building;
                break;
            }
            else
            {
                initChance += randomChance;
            }
        }

        if (targetBuilding == null) { print(random); }

        return targetBuilding;
    }

    //Add up to total
    float totalChance(List<BuildingContainer> buildContainerList)
    {
        float totalValue = 0;

        for (int i = 0; i < buildContainerList.Count; i++)
        {
            totalValue += buildContainerList[i].buildChance;
        }

        return totalValue;
    }

    void PaintCell(Cell target, Color c)
    {
        PaintCell(target.posX, target.posY, c);
    }

    void PaintCell(int x, int y, Color c)
    {
        Cell targetCell = null;

        foreach (Cell cell in cellList)
        {
            if (cell.posX == x && cell.posY == y)
            {
                targetCell = cell;
            }
        }

        if (targetCell != null)
        {
            targetCell.cellObject.GetComponent<MeshRenderer>().material.color = c;
        }
    }

    void PaintLegends()
    {
        PaintEdgeCells();
        PaintConnectNode();
        PaintRoad();
        PaintRoadside();
        PaintStartExit();
        //PaintIntersection();
    }

    void PaintRoad()
    {
        foreach (var item in roadCells)
        {
            PaintCell(item.posX, item.posY, roadColor);
        }
    }

    private void PaintIntersection()
    {
        foreach (Cell intersection in intersectionCells)
        {
            PaintCell(intersection, intersectionColor);
        }
    }


    void PaintRoadside()
    {
        foreach (var item in roadsideArea)
        {
            PaintCell(item.posX, item.posY, roadsideColor);
        }
    }

    void PaintConnectNode()
    {
        foreach (var item in wayPointNodes)
        {
            PaintCell(item.posX, item.posY, connectionColor);
        }
    }

    void PaintEdgeCells()
    {
        foreach (var item in edgeCells)
        {
            PaintCell(item.posX, item.posY, edgeColor);
        }
    }

    void PaintStartExit()
    {
        foreach (var item in exitCells)
        {
            PaintCell(item, exitColor);
        }

        PaintCell(startCell, startColor);
    }

    #endregion
}

