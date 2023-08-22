using TASI.Types.Definition.Things;

namespace TASI.Types.Definition
{
    public class Implementation
    {
        public TypeDef implementing;
        public List<Thing> implemented;
        public string? GetIncorrectImplementation
        {
            get
            {
                List<Thing> allNonImplementedThings = implementing.things.Where(x => x.isUnimplemented).ToList();
                if (allNonImplementedThings.Count != implemented.Count)
                {
                    if (allNonImplementedThings.Count < implemented.Count)
                        return "An implementation of a parent type can only implement the things the parent type specified to be implemented.";
                    return "Some things weren't implemented. Look at the parent type to see which things need to be implemented.";
                }


                for (int i = 0; i < allNonImplementedThings.Count; i++)
                {
                    Thing? implementedItem = allNonImplementedThings.FirstOrDefault(x => x.name == implemented[i].name);
                    if (implementedItem == null)
                        return $"The thing \"{implemented[i].name}\" doesn't exist in the parent type as unimplemented or has already been implemented.";
                    allNonImplementedThings.Remove(implemented[i]);
                }
                return null;
            }
        }

    }
}
