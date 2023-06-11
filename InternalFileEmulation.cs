using DataTypeStore;
using SickFileManager;

namespace TASI
{
    public class InternalFileEmulation
    {

        public string path; //Basically just identyfier idk how to spell
        private string? hash;

        public string Hash
        {
            get
            {
                return hash ?? throw new Exception("Content.Deleted");
            }
        }

        public string? Content
        {
            get
            {
                return SFM.LoadFile(hash ?? throw new Exception("Content.Deleted"));
            }
            set
            {
                if (value == null)
                {
                    SFM.Delete(Hash);
                    hash = null;
                }
                else
                    hash = SFM.OverwriteFile(Hash, value);
            }
        }

        public static InternalFileEmulation CreateWithHash(string path, string hash)
        {
            return new()
            {
                path = path,
                hash = hash
            };
        }
        public InternalFileEmulation()
        {
            path = string.Empty;
            hash = string.Empty;
        }
        public InternalFileEmulation(string path, string content)
        {
            this.path = path;
            hash = SFM.SaveFile(content);
        }

        public static void LoadExternalFile(string path)
        {

        }
        /// <summary>
        /// </summary>
        /// <param name="region">Expected the subregions of the initialised region with the region name </param>
        /// <returns></returns>
        public static List<InternalFileEmulation> LoadInternalFiles(Region region)
        {
            List<InternalFileEmulation> result = new(region.SubRegions.Count);
            region.SubRegions.ForEach(region => result.Add(new()
            {
                path = region.regionName,
                hash = region.FindDirectValue("C").value
            }));
            return result;
        }
        public static Region SaveInternalFiles(List<InternalFileEmulation> allFiles, string regionName)
        {

            Region result = new(regionName);
            allFiles.ForEach(file =>
            {
                result.SubRegions.Add(new(file.path, new List<Region>(), new() { new DirectValue("C", file.hash, false) }));
            });
            return result;

        }
    }
}
