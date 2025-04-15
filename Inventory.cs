using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heyhey
{
    internal class Inventory
    {
        public Core core;
        public Dictionary<string, int> materials;

        public Inventory(Core core)
        {
            this.core = core;
            materials = new Dictionary<string, int>();
        }

        public void Add_Material(string label, int amount)
        {
            if (materials.ContainsKey(label))
            {
                materials[label] += amount;
            }
            else
            {
                materials[label] = amount;
            }
        }

        public void Remove_Material(string label, int amount)
        {
            materials[label] -= amount;
        }

        public bool HasEnough(string material, int requiredAmount)
        {
            return materials.TryGetValue(material, out int currentAmount) && currentAmount >= requiredAmount;
        }
    }
}
