namespace DataTypeStore
{
    public class Read
    {
        public static List<Region> TopLevelRegion(string[] file)
        {
            var result = new List<Region>();
            var currentSubRegions = new List<string>();
            var directValues = new List<DirectValue>();
            List<string> directValueTemp;
            string topLevelName = string.Empty;
            int deph = 0;
            bool canAddToSub;
            foreach (string line in file)
            {
                if (line == string.Empty) continue;
                canAddToSub = true;
                
                switch (line.Substring(0, 1))
                {
                    case "§":
                        if (deph == 0)
                        {
                            topLevelName = line.Substring(1);
                            canAddToSub = false;
                        }
                        deph++;
                        break;

                    case "$":
                        deph--;
                        if (deph < 0)
                            throw new Exception("Invalid file formating. Invalid deph.");
                        if (deph == 0)
                        {
                            canAddToSub = false;
                            result.Add(new Region(topLevelName, currentSubRegions, directValues));
                            currentSubRegions.Clear();
                            directValues.Clear();
                        }
                        break;

                    case "-":
                        if (deph == 1)
                        {
                            directValueTemp = line.Substring(1).Split(':').ToList();
                            if (directValueTemp.Count != 2)
                                throw new Exception("Invalid file formating. Invalid direct value formating.");
                            directValues.Add(new DirectValue(directValueTemp[0], directValueTemp[1], true));
                            canAddToSub = false;
                        }
                        break;
                }
                if (canAddToSub && deph != 0)
                    currentSubRegions.Add(line);


            }
            return result;
        }
    }

}