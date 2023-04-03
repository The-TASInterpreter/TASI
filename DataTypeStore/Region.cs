using System.Xml.Linq;

namespace DataTypeStore
{
    public class Region
    {
        public string regionName;
        private List<Region>? subRegions;
        private readonly List<string>? subRegionsUnRead;
        public List<DirectValue> directValues;
        private string? regionSaveString;
        public string RegionSaveString
        {
            get
            {
                regionSaveString ??= GenerateSaveString();
                return regionSaveString;

            }
        }
        public List<Region> SubRegions
        {
            get
            {
                if (subRegions == null)
                    if (subRegionsUnRead == null)
                        subRegions = new List<Region>();
                    else
                        subRegions = new List<Region>(Read.TopLevelRegion(subRegionsUnRead.ToArray()));
                return subRegions;
            }
            set
            {
                if (subRegions == null)
                    if (subRegionsUnRead == null)
                        subRegions = null;
                    else
                        subRegions = new List<Region>(Read.TopLevelRegion(subRegionsUnRead.ToArray()));
                subRegions = value;
            }
        }
        public Region(string regionName, List<string> subRegions, List<DirectValue> directValues)
        {
            this.regionName = regionName;
            this.subRegionsUnRead = new List<string>(subRegions);
            this.directValues = new List<DirectValue>(directValues);
        }
        public Region(string regionName, List<Region> subRegions, List<DirectValue> directValues)
        {
            this.regionName = regionName;
            this.subRegions = new List<Region>(subRegions);
            this.directValues = new List<DirectValue>(directValues);
        }

        public DirectValue[] FindDirectValueArray(string directValueName)
        {
            return directValues.Where(x => x.name == directValueName).ToArray();
        }
        public DirectValue FindDirectValue(string directValueName)
        {
            DirectValue[] temp = directValues.Where(x => x.name == directValueName).ToArray();
            if (temp.Length != 1)
                return new("nothingFound", "", false);
            return temp[0];
        }
        public Region[] FindSubregionWithNameArray(string name)
        {
            return SubRegions.Where(x => x.regionName == name).ToArray();
        }
        public Region FindSubregionWithName(string directValueName)
        {
            Region[] temp = SubRegions.Where(x => x.regionName == directValueName).ToArray();
            if (temp.Length != 1)
                throw new Exception("There are more than 1 regions with the name " + directValueName);
            return SubRegions.Where(x => x.regionName == directValueName).ToArray()[0];
        }

        private static string ConvertListToString(List<string> strings)
        {
            string result = string.Empty;
            foreach (string s in strings)
                if (s.Contains(';'))
                    result += s;
                else
                    result += s + ";";
            return result;
        }
        private string GenerateSaveString()
        {
            List<string> saveStringList = new()
            {
                $"§{regionName}"
            };
            foreach (DirectValue directValue in directValues)
            {
                saveStringList.Add($"-{directValue.name}:{DirectValueClearify.EncodeInvalidChars(directValue.value)}");
            }

            if (SubRegions != null)
                foreach (Region region in SubRegions)
                    saveStringList.Add(region.RegionSaveString);
            saveStringList.Add("$");
            return ConvertListToString(saveStringList);
        }
    }

}