using System.Collections.Generic;

namespace SandwichUtilities
{
    public static class UtilScripts
    {
        public static int RandomByWeightage(List<int> weightage)
        {
            int initValue = 0;

            int random;

            int totalValue = 0;

            for (int i = 0; i < weightage.Count; i++)
            {
                totalValue += weightage[i];
            }

            random = UnityEngine.Random.Range(0, totalValue);

            int target = 0;

            for (int i = 0; i < weightage.Count; i++)
            {
                if (random >= initValue && random < initValue + weightage[i])
                {
                    target = i;
                    break;
                }
                else
                {
                    initValue += weightage[i];
                }
            }

            return target;
        }
    }
}

