using System.Collections.Generic;

namespace ILS
{
    public static class LineMarkers
    {
        private static Dictionary<string, int> lineMarkers = new Dictionary<string, int>();

        public static void AddLineMarker(string name, int linenum)
        {
            if (lineMarkers.ContainsKey(name))
                throw new ILSException("Invalid line marker, " +name+ "  already exists");

            lineMarkers.Add(name, linenum);
        }



        public static int GetLineByMarker(string name)
        {
            if (!lineMarkers.TryGetValue(name, out int value))
                throw new ILSException("Invalid line marker, " + name + " does not exist");

            return value;
        }


    }
}
