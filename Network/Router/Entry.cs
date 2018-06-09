namespace Network
{
    public class Entry
    {
        public string name;
        public int port;
        public int cost;
        public string nextHop;

        public Entry(string name, int port, int cost, string nextHop)
        {
            this.name = name;
            this.port = port;
            this.cost = cost;
            this.nextHop = nextHop;
        }
    }
}