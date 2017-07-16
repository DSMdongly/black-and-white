using UnityEngine;

public class Stage : MonoBehaviour
{
    public int width;                             // 스테이지의 가로 길이
    public int height;                            // 스테이지의 세로 길이

    Tile[,] tiles;                             // 스테이지 내 타일의 배열

    public GameObject blackTile;                    // 검은색 타일 프리펩
    public GameObject whiteTile;                    // 하얀색 타일 프리펩

    public GameObject blackStone;                     // 검은색 돌 프리펩
    public GameObject whiteStone;                     // 하얀색 돌 프리펩

    Stone stone;    // 이동 제어 테스트를 위한 임의의 Stone 객체 인스턴스

    void Start()
    {
        // 객체가 생성된 후 호출

        GetComponent<BoxCollider2D>().size = new Vector2(width, height);                                                             // 마우스 클릭 이벤트를 위한 충돌체 크기 설정

        tiles = new Tile[height, width];                                                                                        // 가로, 세로 길이를 이용해 tiles 배열을 동적 할당

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Object tilePrefab = (i + j) % 2 == 0 ? blackTile : whiteTile;                               // 이중 for문 내에서 (i + j)의 홀수, 짝수 여부에 따른 타일 프리팹 설정

                tiles[i, j] = (Instantiate(tilePrefab, transform) as GameObject).GetComponent<Tile>();    // tiles 배열에 타일 프리팹 인스턴스 생성 및 Tile 컴포넌트를 배열에 할당
                tiles[i, j].transform.localPosition = new Vector3(j - width / 2, height / 2 - i, 0);                                                // 인스턴스의 로컬 좌표계 설정
            }
        }

        int x = Random.Range(0, width);                                                                                   // 가로 길이 범위 내에서 Random 난수를 이용한 x좌표 생성
        int y = Random.Range(0, height);                                                                                  // 세로 길이 범위 내에서 Random 난수를 이용한 y좌표 생성

        stone = (Instantiate(blackStone) as GameObject).GetComponent<Stone>();                                                               // 프리펩을 이용해 돌 인스턴스를 생성
        MoveStone(ref stone, x, y);                                                                                                               // x, y좌표를 이용해 타일에 배치
    }

    void OnMouseDown()
    {
        // 객체에 마우스 클릭이 적용될 때 호출

        Vector2 mousePosition = Input.mousePosition;                           // 현재 마우스의 스크린 좌표계 구하기
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(mousePosition);    // 스크린 좌표계를 월드 좌표계로 변환

        int x = (int)(Mathf.Round(mousePoint.x) + width / 2);                          // 클릭한 타일의 x좌표 구하기
        int y = (int)(Mathf.Round(mousePoint.y) * -1 + height / 2);                    // 클릭한 타일의 y좌표 구하기 

        if ((x >= 0 && x <= width - 1) && (y >= 0 && y <= height - 1))
        {
            Debug.Log(string.Format("{0}, {1}", x, y));
            MoveStone(ref stone, x, y);                                // 클릭한 타일의 좌표값을 이용해 stone을 이동
        }
    }

    public void MoveStone(ref Stone stone, int x, int y)
    {
        // 돌의 위치 변경 (stone : 움직이고자 하는 GameObject 객체, x : 목표 지점의 x좌표, y : 목표 지점의 y좌표)

        Tile tile = tiles[y, x];                                                       // 돌이 이동하고자 하는 타일
        Renderer stoneRenderer = stone.GetComponent<Renderer>();    // 돌의 디스플레이를 제어하는 Renderer 컴포넌트

        stone.transform.parent = tile.transform;       // stone에 x, y 좌표에 위치한 타일의 transform을 부모로 설정
        stone.transform.localPosition = new Vector3(0, 0, 0);                           // stone의 로컬 좌표계 설정

        // 돌과 타일 사이 태그의 일치 여부에 따라 돌의 디스플레이 조정

        if (stone.color.Equals(tile.color))
        {
            stoneRenderer.enabled = false;
        }

        else
        {
            stoneRenderer.enabled = true;
        }
    }
}