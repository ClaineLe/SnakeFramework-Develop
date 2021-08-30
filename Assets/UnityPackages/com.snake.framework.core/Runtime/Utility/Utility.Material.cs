using UnityEngine;

namespace com.halo.framework
{
    namespace common
    {
        public static partial class Utility
        {
            public static class MaterialUtil {
                static public void TryDestroy(Material mat)
                {
                    if (mat != null)
                    {
                        //Debug.Log("mat = " + mat.name);
                        Material.Destroy(mat);
                    }
                }

            }
            
        }
    }
}
