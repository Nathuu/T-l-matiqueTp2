namespace Network
{
    public class Entry
    {
        public string name;
        public int cost;
        public string nextHop;

        public Entry(string name, int cost, string nextHop)
        {
            this.name = name;
            this.cost = cost;
            this.nextHop = nextHop;
        }

        public Entry(Program.SUBNETWORK name, int cost, string nextHop)
        {
            this.name = ((int)name).ToString();
            this.cost = cost;
            this.nextHop = nextHop;
        }
    }
}