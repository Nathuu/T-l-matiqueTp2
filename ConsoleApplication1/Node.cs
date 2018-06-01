namespace NodeCollectionSolution
{
    public class Node
    {
        public string name;
        public int cost;
        public string route;

        public Node(string name, int cost, string route)
        {
            this.name = name;
            this.cost = cost;
            this.route = route;
        }
    }
}