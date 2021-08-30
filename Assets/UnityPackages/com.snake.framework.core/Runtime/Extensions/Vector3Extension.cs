using UnityEngine;

static public class Vector3Extensions
{
    /// <summary>
    /// 算出xz平面内两个向量的夹角
    /// </summary>
    /// <param name="pSelf"></param>
    /// <param name="pTarget"></param>
    /// <returns></returns>
    public static float Cal_PanelAngle_With_XZ(this Vector3 pSelf, Vector3 pTarget)
    {
        pSelf = pSelf - Vector3.Project(pSelf, Vector3.up);
        pTarget = pTarget - Vector3.Project(pTarget, Vector3.up);
        float angle = Vector3.Angle(pSelf, pTarget);
        float dir = (Vector3.Dot(Vector3.up, Vector3.Cross(pSelf, pTarget)) < 0 ? -1 : 1);
        angle *= dir;
        return angle;
    }
}