namespace Network
{
    public class Entry
    {
        public string name;
        public int cost;
        public string route;

        public Entry(string name, int cost, string route)
        {
            this.name = name;
            this.cost = cost;
            this.route = route;
        }
    }
}