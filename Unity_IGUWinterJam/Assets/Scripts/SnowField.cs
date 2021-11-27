using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SnowField : MonoBehaviour
{
    MeshRenderer meshRender;
    Material material;
    Texture2D orginalTexture;

    Texture2D copiedTexture;
    Color transparentColor;

    int textureWidth;
    int textureHeight;

    private void Awake()
    {
        GetVariables();
    }

    void GetVariables()
    {
        meshRender = GetComponent<MeshRenderer>();
        material = meshRender.material;
        orginalTexture = (Texture2D)material.GetTexture("_SnowTexture");
        

        textureWidth = orginalTexture.width;
        textureHeight = orginalTexture.height;

        copiedTexture = new Texture2D(textureWidth, textureHeight);
        copiedTexture.SetPixels(orginalTexture.GetPixels());
        copiedTexture.Apply();

        material.SetTexture("_SnowTexture", copiedTexture);



    }


    Vector2 minPos;
    Vector2 maxPos;

    private static Transform referenceObject;


    public Vector2 GetPosition(Transform ball)
    {

        if (referenceObject == null) referenceObject = new GameObject("Reference").transform;
        referenceObject.position = ball.position;
        referenceObject.rotation = Quaternion.identity;

        var a = referenceObject.InverseTransformPoint(meshRender.bounds.center);

        float x = a.x + meshRender.bounds.size.x / 2;
        float z = a.z + meshRender.bounds.size.z / 2;

        var x2 = x / meshRender.bounds.size.x;
        var z2 = z / meshRender.bounds.size.z;


        //x2 = Mathf.Abs(x2 - 1); 

        return new Vector2(x2,z2);
        
    }


    // 0 - 100
    public int CheckForSnow(float x, float y, int pixelRadius = 12)
    {
        if (x < 0 || x > 1 || y < 0 || y > 1)
            return 0;
        
        int xCenter = (int)(textureWidth * x);
        int yCenter = (int)(textureHeight * y);

        int snowCounter = 0;

        for (int xxx = -pixelRadius; xxx < pixelRadius; xxx++)
        {
            for (int yyy = -pixelRadius; yyy < pixelRadius; yyy++)
            {
                if (xxx+ xCenter < 0 || xxx+ xCenter >= textureWidth || yyy + yCenter < 0 || yyy + yCenter >= textureHeight)
                    continue;

                

                if(CheckIfPointIsInCircle(xxx+pixelRadius,yyy + pixelRadius, pixelRadius * 2, pixelRadius * 2)
                    && copiedTexture.GetPixel(xxx + xCenter, yyy + yCenter) == Color.white)
                {
                    snowCounter++;
                    copiedTexture.SetPixel(xxx+ xCenter, yyy + yCenter, transparentColor);
                }
            }
        }


        copiedTexture.Apply();
        //material.SetTexture("_SnowTexture", copiedTexture);

        return snowCounter;

    }

    static bool CheckIfPointIsInCircle(int x, int y, int width, int height, float precision = 1.2f)
    {
        int w = width / 2;
        int h = height / 2;

        // make round Shapes more beautiful
        if (width % 2 == 0)
        {
            w -= 1;
            if (x > w) x--;
        }
        if (height % 2 == 0)
        {
            h -= 1;
            if (y > h) y--;
        }

        var s = (Math.Pow(x - w, 2) / Math.Pow(w, 2)) +
        (Math.Pow(y - h, 2) / Math.Pow(h, 2));

        if (s > precision)
            return false;
        else
            return true;
    }
}
