using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        .-`                                                                            
                        /./                                                                             
                        +.:-                                                                            
                        `:/+/:..--....-..-.`   ``..``                                                   
                            `.-+:.``       `.--`.-.....`                                                  
                            -/`       `    `..//```   `                                                  
                            -+`        ```  `:/-s-.-.                                                     
                        `` `o:.        ..`  -oydso `..                                                    
                        -//-.`       :/:- `.dyshs   `                                                    
                        `+/.`.``  .:osys..`+s:os                                                        
                            .o--.```+dyddo:-.`.:--+/                                                       
                            //--::.`-h:oy- ````...:+                                                       
                            s--//::``-.:/-`.`````.+-                                                       
                    `-..:/+:::++/-.......```.--++                      ~ Written by @mermaid_games ~                                  
                    `/+/o+::+++++oo:-----//++oo/                               aka William Spelman bc that's my Twitter handle                         
                        `-::-/+++/+ss/:-.:oysys++/`                                                       
            `      `-::..-:++++:odhyss/-ysoyyso+-                                                       
            `----.://----:/++/+/:dddyssyhyysyssyy+--                                                     
                `//o++oo/-:+oo:-+:-dhhyhyhdysosyhhhhhy                                                     
                    :o:/+:+/-/+:.ssydddhdhdyydhhhhd`                                                     
                    `+//o`+/-:+o+..yhdhhhmhdyhhhys+d`                                                     
                    .y+s-++:/++oo-`/yysyddhhhhhyso+h`                                                     
                    .y+y+o++oo+shho.:oshmh+oso+osoo-                                                      
                    .y+so++oso+ymmyyo+oymdoo++//o+                                                        
                    :s+++osyssmdmhssshdds///o+od                                                         
                    `+o+oyyyyhmmddssyddy/:/+/shd                                                         
                    `+osyyyhhydmdysshyhs/:::yhhd        ....`                                            
            ..```-:ooyhyyhhhhhddysyyshs/+oohhhs   `./ooo++++o-                                          
                .ooss+/ohhhhysshddhyyysyhy++s:yhd/:o+ssoooo++///s:                                         
                `.. `odhysssyddmhhhyshhsyo:syosysooooooo+oyso+os:                                        
                    :shyssssyyddmhdhsyhhds-:+oys++oooooo+++ooo++os:                                       
                -shhysssssyhddmdhyshhhy/++hhs++ooooooosyy+////+os`                                      
                .yhhhyssssyhhsmdhhsshdds-.odyssooooooosydd+::////oo                                      
                `dhhhysssssyyo+mdhysshdds/:ydhhysooosshd/`so//////+s:                                     
                +dhhyssssss//omhyo++syd+.+dhhhhhyyyhddm  .y+//////+o.                                    
                -o:ooyyss+///odo+---/oyoods+osyyhhddddo   /y+//////+s.                                   
                -+--`-os+///+hs++::::+ohds////++shhddm-   `sso++////oy`                                  
                ./--`  ./+oosdo++/:::++hh/////////+syh:.    /sooo+///oo`                                 
                /o:--.`---.oyo///--:oosd///////o++//+++/.`  .oyso+///o+`                                
                `-+++o//:-.so:-::-..://d+//////++++++ssso+//-.-oyo+/osd:                                
                    `...-::osyo:----/yodhyysssoooossssssoo+++++/+ddddddd:`                              
                            ``s/--.:.-hyyyyyyyyssysso++++///////++oyhhhdddo`                             
                            :s//./o+-o         `-/+so+++////////////+oyhdmo.                            
                            :so+:/-`+:o.            .:+oooo+++//////////+smdho/-.`                       
                            `.::-    ..`                 ..-///+syso+++++yhyyhhhys+/:..`                 
                                                                -dmh/:ooydhhyoosyyyyssssoo:-/oos/`       
                                                                hm+   `+ddh+oo+oyyso++++oyhhyhdm.       
                                                                oy:     ydhyyysoshyysssosyhhhhy+`       
                                                                        `odhhhhhhhhyhhhhhhhhh/`         
                                                                            .ydddddy:` .::::::.            
                                                                            `/ohdddo.                     
                                                                            -:ydh                     
                                                                                ```       
    
                  ~ This Utility is provided to you by you local Socialist Code Witches! ~
*/

public class PathScript : MonoBehaviour {

    //Settings
    [SerializeField] private GameObject grid_obj;
    private Grid grid;
    protected Vector2[] path;

    //Init Pathfinder
	void Awake () {
        if (grid_obj != null){
		    grid = grid_obj.GetComponent<Grid>();
        }
	}

    //Path Functions
    private void createPath (Vector2 start_position, Vector2 end_position){
        Node start_node = grid.getNodeFromWorld(start_position.x, start_position.y);
        Node end_node = grid.getNodeFromWorld(end_position.x, end_position.y);

        Heap<Node> open = new Heap<Node>(grid.maxSize());
        HashSet<Node> closed = new HashSet<Node>();
        open.Add(start_node);

        while (open.Count > 0){
            Node current_node = open.removeFirst();
            closed.Add(current_node);

            if (current_node == end_node){
                setPath(start_node, end_node);
                return;
            }

            foreach (Node neighbour in grid.getNeighbors(current_node)) {
                if (!neighbour.isEmpty() || closed.Contains(neighbour)){
                    continue;
                }

                int movecost = current_node.gcost + grid.getDistance(current_node, neighbour);
                if (movecost < neighbour.gcost || !open.Contains(neighbour)){
                    neighbour.gcost = movecost;
                    neighbour.hcost = grid.getDistance(neighbour, end_node);
                    neighbour.parent = current_node;

                    if (!open.Contains(neighbour)){
                        open.Add(neighbour);
                    }
                    else {
					    open.UpdateData(neighbour);
					}
                }
            }
        }
    }

    private void setPath(Node start_node, Node end_node){
        List<Node> node_path = new List<Node>();
        Node current_node = end_node;
        
        while (current_node != start_node){
            node_path.Add(current_node);
            current_node = current_node.parent;
        }
        node_path.Reverse();

        path = new Vector2[node_path.Count];

        //Debug
        for (int i = 0; i < node_path.Count; i++){
            path[i] = node_path[i].getPosition();
        }

        grid.debug_path = path;
    }

    public Vector2[] getPath(Vector2 start_position, Vector2 end_position){
        createPath (start_position, end_position);
        return path;
    }

    public Grid Grid {
        set {
            grid = value;
        }
    }

}
