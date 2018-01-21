using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    //Settings
    public LayerMask collision_layer;
    public Vector2 level_size;
    public float node_size;

    public Vector2[] debug_path;

    //Variables
    private Node[,] grid;

	void Start () {
		createGrid();
	}

    //Grid Functions
    private void createGrid () {
        int grid_width = Mathf.RoundToInt(level_size.x / (node_size * 2));
        int grid_height = Mathf.RoundToInt(level_size.y / (node_size * 2));

        float start_x = transform.position.x;
        float start_y = transform.position.y;
        
        grid = new Node[grid_width, grid_height];

        int i = 0;
        for (int y = 0; y < grid_height; y++){
            for (int x = 0; x < grid_width; x++){
                bool is_empty = true;
                Vector2 node_position = new Vector2(start_x + (x * node_size), start_y - (y * node_size));
                RaycastHit2D node_check = Physics2D.BoxCast(node_position, new Vector2(node_size, node_size), 0f, new Vector2(0f, 0f), Mathf.Infinity, collision_layer, -Mathf.Infinity, Mathf.Infinity);
                if (node_check.collider != null){
                    is_empty = false;
                }

                grid[x, y] = new Node(is_empty, node_position.x, node_position.y, i);
                i++;
            }
        }
    }

    public List<Node> getNeighbors(Node node){
        List<Node> neighbors = new List<Node>();

        int grid_width = grid.GetLength(0);
        int grid_height = grid.GetLength(1);
        int node_check_x = node.getGridNum() % grid_width;
        int node_check_y = node.getGridNum() / grid_width;

        for (int w = -1; w <= 1; w++){
            for (int h = -1; h <= 1; h++){
                if (w == 0 && h == 0){
                    continue;
                }

                if (node_check_x + w >= 0 && node_check_x + w < grid_width){
                    if (node_check_y + h >= 0 && node_check_y + h < grid_height){
                        neighbors.Add(grid[node_check_x + w, node_check_y + h]);
                    }
                }
            }
        }

        return neighbors;
    }

    public int getDistance(Node nodeA, Node nodeB){
        int temp_distance;

        int grid_width = grid.GetLength(0);
        int w = Mathf.Abs((nodeA.getGridNum() % grid_width) - (nodeB.getGridNum() % grid_width));
        int h = Mathf.Abs((nodeA.getGridNum() / grid_width) - (nodeB.getGridNum() / grid_width));

        if (w > h){
            temp_distance = (h * 14) + ((w - h) * 10);
        }
        else {
            temp_distance = (w * 14) + ((h - w) * 10);
        }
        return temp_distance;
    }

    public Node getNodeFromWorld(float x, float y){
        Node start_node = grid[0, 0];
        Vector2 start_position = start_node.getPosition();

        float world_node_x = (x - start_position.x) / node_size;
        float world_node_y = (start_position.y - y) / node_size;

        int node_x = Mathf.RoundToInt(Mathf.Clamp(world_node_x, 0, grid.GetLength(0) - 1));
        int node_y = Mathf.RoundToInt(Mathf.Clamp(world_node_y, 0, grid.GetLength(1) - 1));

        return grid[node_x, node_y]; 
    }

    public int maxSize(){
        return (grid.GetLength(0) * grid.GetLength(1));
    }

    //Debug
    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(node_size, node_size, node_size));

        if (grid != null){
            for (int j = 0; j < grid.GetLength(0); j++){
                for (int k = 0; k < grid.GetLength(1); k++){
                    Node draw_node = grid[j, k];
                    Vector3 draw_position = new Vector3(draw_node.getPosition().x, draw_node.getPosition().y, transform.position.z);

                    Gizmos.color = Color.white;
                    if (!draw_node.isEmpty()){
                        Gizmos.color = Color.red;
                    }
                    Gizmos.DrawWireCube(draw_position, new Vector3(node_size, node_size, node_size));
                }
            }
        }

        if (debug_path != null){
            for (int d = 0; d < debug_path.Length; d++){
                Vector3 debug_pos = new Vector3(debug_path[d].x, debug_path[d].y, transform.position.z);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(debug_pos, new Vector3(node_size, node_size, node_size));
            }
        }
    }
}
